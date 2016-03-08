﻿using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.SceneManagement;

using CodingJar.MultiScene;

namespace CodingJar.MultiScene.Editor
{
	public class AmsSaveProcessor : UnityEditor.AssetModificationProcessor
	{
		static string[] OnWillSaveAssets( string[] filenames )
		{
			var crossSceneReferenceBehaviour = AmsPreferences.CrossSceneReferencing;

			// If we don't allow cross-scene references, then just save.
			if ( crossSceneReferenceBehaviour == AmsPreferences.CrossSceneReferenceHandling.UnityDefault )
				return filenames;

			// Check if we're saving any scenes
			List<Scene> savingScenes = new List<Scene>();
			foreach( var filename in filenames )
			{
				var scene = EditorSceneManager.GetSceneByPath(filename);
				if ( scene.IsValid() )
				{
					savingScenes.Add(scene);
				}
			}

			HandleCrossSceneReferences( savingScenes );
			return filenames;
		}

		public static void HandleCrossSceneReferences( IList<Scene> scenes )
		{
			// If we are...
			if ( scenes.Count < 1 )
				return;

			// We need to create an AmsMultiSceneSetup singleton in every scene.  This is how we keep track of Awake scenes and
			// it also allows us to use cross-scene references.
			foreach( var scene in scenes )
			{
				if ( !scene.isLoaded )
					continue;

				var sceneSetup = GameObjectEx.GetSceneSingleton<AmsMultiSceneSetup>( scene, true );
				sceneSetup.OnBeforeSerialize();

				// Reset all of the cross-scene references.
				var crossSceneRefBehaviour = AmsCrossSceneReferences.GetSceneSingleton( scene, true );
				crossSceneRefBehaviour.ResetCrossSceneReferences();
			}

			var crossSceneReferenceBehaviour = AmsPreferences.CrossSceneReferencing;
			var xSceneRefs = AmsCrossSceneReferenceProcessor.GetCrossSceneReferencesForScenes( scenes );
			
			bool bSaveCrossSceneReferences = crossSceneReferenceBehaviour == AmsPreferences.CrossSceneReferenceHandling.Save;
			if ( bSaveCrossSceneReferences && xSceneRefs.Count > 0 )
			{
				AmsDebug.LogWarning( null, "Ams Plugin: Found {0} Cross-Scene References. Saving them.", xSceneRefs.Count );
				AmsCrossSceneReferenceProcessor.SaveCrossSceneReferences( xSceneRefs );
			}

			// Zero-out these cross-scene references so we can save without pulling in those assets.
			for(int i = 0 ; i < xSceneRefs.Count ; ++i)
			{
				var xRef = xSceneRefs[i];
				int refIdToRestore = xRef.fromProperty.objectReferenceInstanceIDValue;
					
				if ( !bSaveCrossSceneReferences )
					Debug.LogWarningFormat( "Cross-Scene Reference {0} will become null", xRef );

				// Set it to null.
				xRef.fromProperty.objectReferenceInstanceIDValue = 0;
				xRef.fromProperty.serializedObject.ApplyModifiedPropertiesWithoutUndo();

				// Restore if we're not about to enter play mode
				if ( !EditorApplication.isPlayingOrWillChangePlaymode )
				{
					EditorApplication.delayCall += () =>
						{
							AmsDebug.Log( null, "Restoring Cross-Scene Ref (Post-Save): {0}", xRef );
							xRef.fromProperty.objectReferenceInstanceIDValue = refIdToRestore;
							xRef.fromProperty.serializedObject.ApplyModifiedPropertiesWithoutUndo();
						};
				}
			}
		}

	} // class
} // namespace