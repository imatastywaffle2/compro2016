using UnityEngine;
using System.Collections;

namespace CodingJar.MultiScene.Editor
{

#if TODO_DEPRECATED
		/// <summary>
		/// We can only restore cross-scene references when playing in the Editor.
		/// Use this function to restore references in the 'from scene'.
		/// </summary>
		/// <param name="scene">Scene to restore cross-scene references</param>
        public static void RestoreCrossSceneReferences( Scene scene )
        {
            List<EditorUniqueObject>			uniqueObjects = new List<EditorUniqueObject>();
            List<EditorCrossSceneReference>		removableRedirects = new List<EditorCrossSceneReference>();
            List<Object>						placeHoldersToRemove = new List<Object>();

			AmsCrossSceneReferences crossSceneReferenceObject = GetCrossSceneReferenceBehaviour( scene );
            foreach( var crossSceneReference in crossSceneReferenceObject._crossSceneReferences )
            {
                bool bShouldRemoveFromList = true;
                var fromObj = uniqueObjects.FirstOrDefault( x => x.hashId == crossSceneReference.fromObjectHash );
                var toObj = uniqueObjects.FirstOrDefault( x => x.hashId == crossSceneReference.toObjectHash );

                string errorString = null;
                if ( fromObj == null )
                {
                    bShouldRemoveFromList = !crossSceneReference.fromSubScene || crossSceneReference.fromSubScene.IsLoaded();
                    errorString = "Could not find source object for cross-scene reference";
                }
                else if ( !fromObj.obj )
                {
                    bShouldRemoveFromList = !crossSceneReference.fromSubScene || crossSceneReference.fromSubScene.IsLoaded();
                    errorString = "Source object for cross-scene reference is null";
                }

                if ( errorString != null )
                {
                    if ( bShouldRemoveFromList )
                    {
                        removableRedirects.Add(crossSceneReference);
                        errorString += " (Removed)";
                    }

                    LogMissingCrossSceneReference( crossSceneReference, fromObj, toObj, errorString );
                    continue;
                }

                SerializedObject serializedObject = new SerializedObject(fromObj.obj);
                try
                {
                    var fromProperty = serializedObject.FindProperty(crossSceneReference.fromPropertyPath);
                    if ( fromProperty == null )
                    {
                        bShouldRemoveFromList = true;
                        errorString = "Could not find source object property for cross-scene reference. Cross-Scene Reference is no longer valid";
                    }
                    else if ( toObj == null )
                    {
                        bShouldRemoveFromList = !crossSceneReference.toSubScene || crossSceneReference.toSubScene.IsLoaded();
                        errorString = "Could not find destination object for cross-scene reference";
                    }
                    else if ( !toObj.obj )
                    {
                        bShouldRemoveFromList = !crossSceneReference.toSubScene || crossSceneReference.toSubScene.IsLoaded();
                        errorString = "Destination object for cross-scene reference is null";
                    }

                    Object currentRef = (fromProperty != null) ? fromProperty.objectReferenceValue : null;
                    if ( bShouldRemoveFromList )
                    {
                        if ( currentRef )
                            placeHoldersToRemove.Add( currentRef );

                        removableRedirects.Add( crossSceneReference );

                        if ( errorString != null )
                            errorString += " (Removed)";
                    }

                    if ( currentRef && !SceneDataEx.IsPlaceholderObject(currentRef) )
                    {
                        errorString = "Cross-Scene reference changed to a local, non-placeholder reference (Removed)";
                        placeHoldersToRemove.Remove(currentRef);
                        removableRedirects.Add( crossSceneReference );
                    }

                    if ( errorString != null )
                    {
                        LogMissingCrossSceneReference( crossSceneReference, fromObj, toObj, errorString );
                        continue;
                    }

                    fromProperty.objectReferenceValue = toObj.obj;
                    serializedObject.ApplyModifiedProperties(); // Unity 5.2: WithoutUndo if possible

                    fromProperty.Dispose();
                }
                finally
                {
                    serializedObject.Dispose();
                }

                removableRedirects.Add(crossSceneReference);
                SubSceneEx.EditorLog( string.Format("Restored Cross-Scene Reference {0}.{1} => {2}", fromObj.obj, crossSceneReference.fromPropertyPath, toObj.obj), fromObj.obj );
            } // foreach crossSceneReference

            subSceneData._crossSceneReferences.RemoveAll( x => removableRedirects.Contains(x) );

            // Remove all of the place holders we've reassigned
            Undo.IncrementCurrentGroup();
            foreach( var placeHolder in placeHoldersToRemove )
                SceneDataEx.RemovePlaceholder( placeHolder );
        }

        /// <summary>
        /// Destroy all of the Cross-Scene Placeholder objects for a given SceneData instance and all of the invalid entries.
        /// </summary>
        /// <param name="instance">The instance of SceneData to destroy cross-scene references for</param>
        internal static void CleanUpLingeringReferences( Scene scene )
        {
			var instance = AmsCrossSceneReferences.GetSceneSingleton( scene, false );
			if ( !instance )
				return;

            List<Transform> children = new List<Transform>( instance.transform.Cast<Transform>() );
            foreach( var child in children )
            {
                List<Object> targets = new List<Object>( child.gameObject.GetComponentsInChildren<Component>(true) );
                targets.Add( child.gameObject );

                bool bAnyRef = DoesAnyObjectReferenceThese( targets.ToArray() );
                if ( !bAnyRef )
                {
                    Debug.LogWarning( "Unreferenced Cross-Scene Placeholder: " + child + ". Destroyed.", child );
                    Object.DestroyImmediate(child.gameObject);
                }
            }

			// TODO
            //instance._uniqueObjects.RemoveAll( x => !x.obj );
        }

		
        private static void LogMissingCrossSceneReference( EditorCrossSceneReference crossSceneReference, UniqueObject fromObj, UniqueObject toObj, string errorString )
        {
            Scene fromScene = crossSceneReference.fromScene;
            Scene toScene = crossSceneReference.toScene;

            string toSceneString = toScene.IsValid() ? toScene.name : "Missing";
            string fromSceneString = fromScene.IsValid() ? fromScene.name : "Missing";

            Debug.LogError( string.Format( "{0}: {1}.{2}.{3} => {4}.{5}", errorString, fromSceneString, fromObj, crossSceneReference.fromProperty.propertyPath, toSceneString, toObj ) );
        }

		/**
		 * Spit out a friendly way of reading a scene's cross-reference data
		 */
		private static void LogCrossSceneReference( EditorCrossSceneReference crossRef )
		{
			Object fromObj = crossRef.fromProperty.serializedObject.targetObject;

			string fromText = fromObj.name;
			string toText = crossRef.toInstance.name;

			// Try better at describing where the reference is coming from
			Component fromComp = fromObj as Component;
			if ( fromComp && fromComp.transform )
			{
				fromText = TransformEx.FullPath( fromComp.transform );
			}

			Debug.LogErrorFormat( crossRef.fromProperty.serializedObject.targetObject, "Cross Scene Reference: {0}.{1} ({2}) -> {3} ({4})", fromText, crossRef.fromProperty.propertyPath, crossRef.fromScene, toText, crossRef.toScene );
		}

	        /// <summary>
        /// Add a set of cross-scene references from a loaded SubScene.
        /// </summary>
        /// <param name="crossRefs">The list of cross-scene references which to add to</param>
        /// <param name="subScene">The SubScene to consider</param>
        private static void AddCrossSceneReferences( List<EditorCrossSceneReference> crossRefs, Scene scene )
        {
			var rootGameObjects = scene.GetRootGameObjects();

			// Grab all of the MonoBehaviours
			List<Object> allMonoBehavioursInSubScene = new List<Object>();
			foreach( var root in rootGameObjects )
			{
				allMonoBehavioursInSubScene.AddRange( root.GetComponentsInChildren<MonoBehaviour>(true).Cast<Object>() );
			}

#if SCRIPTABLE_OBJECTS_COUNT
			// Look at all scriptable objects... do I need to do this?  I'm not even sure since they should technically not be in a scene.
			var allScriptableObjs = Resources.FindObjectsOfTypeAll<ScriptableObject>().Where( x => 
					!EditorUtility.IsPersistent(x) && !typeof(EditorWindow).IsAssignableFrom(x.GetType())
				).Cast<Object>();
#endif

            if ( _referenceMap == null )
			    _referenceMap = new List<KeyValuePair<SerializedProperty, Object>>( allMonoBehavioursInSubScene.Count() );

            PopulateReferenceMap( _referenceMap, allMonoBehavioursInSubScene );
            //PopulateReferenceMap( _referenceMap, allScriptableObjs );

            var newXrefs = ComputeCrossSceneReferences( _referenceMap );

            // Now add all of the new unique ones...
            foreach( var newRef in newXrefs )
            {
                if ( !crossRefs.Any(x => newRef.Equals(x)) )
                    crossRefs.Add( newRef );
            }
        }

		/// <summary>
		/// Simple function for determining if this object is referenced
		/// </summary>
		/// <param name="targets">The targets to determine if they are referenced</param>
		/// <returns>True iff the targets have a cross-scene reference</returns>
        public static bool     DoesAnyObjectReferenceThese( Object[] targets )
        {
            UpdateReferencesMap();

            Object[] ignoreThese = EditorUtility.CollectDeepHierarchy( targets );

            foreach( var pair in _referenceMap )
            {
                foreach( var target in targets )
                {
                    if ( pair.Value == target )
                    {
                        if ( !ignoreThese.Contains(pair.Key.serializedObject.targetObject) )
                            return true;
                    }
                }
            }

            return false;
        }
#endif

}
