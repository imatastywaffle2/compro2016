using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

namespace CodingJar.MultiScene
{
    internal static class CrossSceneReferencesEx
    {
#if false
        internal static void SaveCrossSceneReference( this SceneData instance, SerializedProperty property )
        {
            Object objRef = property.objectReferenceValue;
            if ( !objRef )
                throw new UnassignedReferenceException( "Cannot save a null cross-scene reference" );

            if ( !(objRef is Component || objRef is GameObject) )
                throw new UnityException( "You are only able to reference Components or GameObjects across scenes, not " + objRef.GetType() );

            // Book keeping on the FROM side.
            Object fromObj = property.serializedObject.targetObject;
            SubScene fromSubScene = SubSceneEx.GetSubScene( fromObj );
            SceneData fromSceneData = SceneDataEx.GetSceneData( fromSubScene );
            string fromHash = fromSceneData.FindOrCreateHashCode( fromObj );

            if ( fromObj is SceneData )
                return;

            // Book keeping on the TO side.
            SubScene toSubScene = SubSceneEx.GetSubScene( objRef );
            bool bIsDestinationUnlocked = !toSubScene || !toSubScene.IsLocked();
            var toSceneData = SceneDataEx.GetSceneData( toSubScene, bIsDestinationUnlocked );

            string toCompHash = null;
            if ( bIsDestinationUnlocked )
                toCompHash = toSceneData.FindOrCreateHashCode( objRef );
            else if ( toSceneData )
                toCompHash = toSceneData.FindHashCodeForObject( objRef );

            SceneData.CrossSceneReference entry = new SceneData.CrossSceneReference()
            {
                fromSubScene = fromSubScene,
                fromObjectHash = fromHash,
                fromPropertyPath = property.propertyPath,
                toSubScene = toSubScene,
                toObjectHash = toCompHash
            };

            if ( !toSceneData )
                throw new UnityException( string.Format("Cannot save cross-scene reference {0}. The SubScene '{1}' needs to be unlocked in order to record its referenced objects", entry, toSubScene) );
            else if ( fromSubScene && fromSubScene.GetRuntimeLoadSettings() != SubScene.RuntimeLoadSettings.BakeIntoScene )
                throw new UnityException( string.Format("Cannot save cross-scene reference {0}. The Source SubScene is not set to BakeIntoScene.  See the documentation on 'Cross-Scene References'", entry) );
            else if ( toSubScene && toSubScene.GetRuntimeLoadSettings() != SubScene.RuntimeLoadSettings.BakeIntoScene )
                throw new UnityException( string.Format("Cannot save cross-scene reference {0}. The Destination SubScene is not set to BakeIntoScene.  See the documentation on 'Cross-Scene References'", entry) );

            instance.AddOrUpdateCrossSceneReference( entry );
            EditorUtility.SetDirty( instance );
            property.serializedObject.Update();

            // Now reference the newly created component instead...
            Object placeHolder = GetPlaceholder( fromSubScene, objRef, toCompHash );

            // Deal with Prefab issues in Unity 5.x by specifying this is definitely an override
            if ( property.isInstantiatedPrefab )
            {
                var targetObject = property.serializedObject.targetObject;
                var prefabParent = PrefabUtility.GetPrefabParent(targetObject);
                var propMod = new PropertyModification() { objectReference = placeHolder, propertyPath = property.propertyPath, target = prefabParent };
                
                // Create at least this change...
                var newMods = new List<PropertyModification>();
                newMods.Add( propMod );

                // Get the existing ones
                var existingMods = PrefabUtility.GetPropertyModifications(targetObject);
                if ( existingMods != null )
                    newMods.AddRange( existingMods );

                // Now change all of the existing ones to match ours...
                foreach( var mod in newMods )
                {
                    if ( mod.propertyPath.Equals(propMod.propertyPath) )
                        mod.objectReference = propMod.objectReference;
                }

                // Tell the prefab this is an override
                PrefabUtility.SetPropertyModifications( targetObject, newMods.ToArray() );
            }

            property.objectReferenceValue = placeHolder;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        /// <summary>
        /// Create a PlaceHolder object for a given Component.  We can reference this component later.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private static Object GetPlaceholder( SubScene subScene, Object originalObj, string hashId )
        {
            string name = string.Format( "Cross-Scene Ref ({0})", hashId );
            GameObject newGameObj = null;

            SceneData sceneData = SceneDataEx.GetSceneData(subScene);

            // First try searching by name (old behaviour)
            Transform existsChild = sceneData.transform.FindChild( name );
            if ( existsChild )
                newGameObj = existsChild.gameObject;

            if ( !newGameObj )
            {
                newGameObj = EditorUtility.CreateGameObjectWithHideFlags( name, HideFlags.None );
                newGameObj.transform.parent = sceneData.transform;
                newGameObj.SetActive(false);

				SubSceneEx.SetIsDirty( subScene, true );
            }

            if ( !AdditiveScenePreferences.DebugShowBookkepingObjects )
                newGameObj.hideFlags = HideFlags.HideInHierarchy;

            // Create a new, cloned component on the current GameObject.
            if ( originalObj is GameObject )
            {
                return newGameObj;
            }
            else if ( originalObj is Transform )
            {
                return newGameObj.transform;
            }
            else if ( originalObj is Component )
            {
                Component component = (Component)originalObj;
                Unsupported.CopyComponentToPasteboard( component );

                Component newComp = newGameObj.GetComponent( originalObj.GetType() );
                if ( !newComp )
				{
                    newComp = newGameObj.AddComponent( component.GetType() );
					SubSceneEx.SetIsDirty( subScene, true );
				}

                Unsupported.PasteComponentValuesFromPasteboard( newComp );
                return newComp;
            }

            throw new System.ArgumentException( "Placeholders can only be created for GameObjects or Components", "originalObj" );
        }

		        /// <summary>
        /// Retrieve a hash code for a particular component.  Will re-use the existing hash code if it exists for that component.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        internal static string     FindOrCreateHashCode( this SceneData instance, Object obj )
        {
            string hashId = FindHashCodeForObject(instance, obj);
            if ( !string.IsNullOrEmpty(hashId) )
                return hashId;

            var newEntry = new SceneData.UniqueObject(obj, System.Guid.NewGuid().ToString());
            instance._uniqueObjects.Add(newEntry);

            return newEntry.hashId;
        }

        internal static string      FindHashCodeForObject( this SceneData instance, Object obj )
        {
            var uniqueObjs = instance._uniqueObjects;

            foreach( var entry in uniqueObjs )
            {
                if ( entry.obj == obj )
                    return entry.hashId;
            }

            return null;
        }

        /// <summary>
        /// Adds a cross-scene reference to a given SceneData.
        /// </summary>
        /// <param name="instance">The SceneData to add the reference to</param>
        /// <param name="xRef">The cross-scene reference</param>
        internal static void AddOrUpdateCrossSceneReference( this SceneData instance, SceneData.CrossSceneReference xRef )
        {
            var xSceneRefs = instance._crossSceneReferences;
            for (int i = 0 ; i < xSceneRefs.Count ; ++i)
            {
                var xSceneRef = xSceneRefs[i];

                // We could be just updating an existing one...
                if ( xSceneRef.fromObjectHash == xRef.fromObjectHash &&
                    xSceneRef.fromPropertyPath == xRef.fromPropertyPath &&
                    xSceneRef.fromSubScene == xRef.fromSubScene )
                {
                    xSceneRefs.RemoveAt(i);
                    --i;
                }
            }

            xSceneRefs.Add( xRef );
        }

        internal static GameObject   GetPlaceholderGameObject( Object placeHolder )
        {
            if ( !placeHolder )
                return null;

            int owningID = UnityEditorInternal.InternalEditorUtility.GetGameObjectInstanceIDFromComponent( placeHolder.GetInstanceID() );
            GameObject owningGameObj = UnityEditor.EditorUtility.InstanceIDToObject(owningID) as GameObject;
            if ( !owningGameObj )
                owningGameObj = placeHolder as GameObject;

            return owningGameObj;
        }

        /// <summary>
        /// Is this GameObject a placeholder GameObject (for cross-scene referencing)?
        /// </summary>
        internal static bool IsPlaceholderGameObject( GameObject gameObj )
        {
            if ( !gameObj )
                return false;

            Transform parent = gameObj.transform.parent;
            if ( !parent )
                return false;

            return parent.GetComponent<SceneData>();
        }

        /// <summary>
        /// Is this object reference a placeholder object reference (for cross-scene referencing)?
        /// </summary>
        internal static bool IsPlaceholderObject( Object placeHolder )
        {
            GameObject gameObj = GetPlaceholderGameObject( placeHolder );
            return IsPlaceholderGameObject( gameObj );
        }


        /// <summary>
        /// Remove a particular placeholder from a SceneData instance
        /// </summary>
        /// <param name="instance">The instance of SceneData</param>
        /// <param name="placeHolder">The placeholder to remove</param>
        internal static void RemovePlaceholder( Object placeHolder )
        {
            if ( !placeHolder )
                return;

            GameObject owningGameObj = GetPlaceholderGameObject( placeHolder );
            if ( !owningGameObj )
            {
                Debug.LogError( "Trying to remove a Cross-Scene Reference Place Holder: " + placeHolder + " yet it is not a Component or GameObject", placeHolder );
                return;
            }

            if ( IsPlaceholderGameObject(owningGameObj) )
            {
                Object.DestroyImmediate( owningGameObj, true );

                // You must still Undo.IncrementCurrentGroup before this to not dirty the scene
                Undo.ClearUndo( owningGameObj );
            }
            else
            {
                Debug.LogError( "Trying to remove a PlaceHolder that is NOT a child of a SceneData object: " + owningGameObj.transform.FullPath(), owningGameObj );
            }
        }
#endif
    }
}