﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CodingJar;

using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Reflection;
#endif

namespace CodingJar.MultiScene
{
    /// <summary>
	/// Holds cross-scene references for a particular scene.
    /// </summary>
	[ExecuteInEditMode]
    public sealed class AmsCrossSceneReferences : MonoBehaviour
#if UNITY_EDITOR
		, ISerializationCallbackReceiver
#endif
    {
        [SerializeField]    private List<RuntimeCrossSceneReference>	_crossSceneReferences = new List<RuntimeCrossSceneReference>();
		[SerializeField, HideInInspector]	private List<GameObject>	_realSceneRootsForPostBuild = new List<GameObject>();
		
		private List<RuntimeCrossSceneReference>	_referencesToResolve;

		/// <summary>
		/// Return the Singleton for a given scene (there is one per Scene).
		/// </summary>
		/// <param name="scene">The Scene to obtain the singleton for</param>
		/// <returns>The per-scene singleton</returns>
		public static AmsCrossSceneReferences	GetSceneSingleton( Scene scene, bool bCreateIfNotFound )
		{
			var multiSceneSetup = GameObjectEx.GetSceneSingleton<AmsMultiSceneSetup>( scene, bCreateIfNotFound );
			if ( !multiSceneSetup )
				return null;

			var existing = multiSceneSetup.gameObject.GetComponent<AmsCrossSceneReferences>();
			if ( existing )
				return existing;
			else if ( bCreateIfNotFound )
				return multiSceneSetup.gameObject.AddComponent<AmsCrossSceneReferences>();

			return null;
		}

		/// <summary>
		/// Adds a cross-scene reference.
		/// </summary>
		/// <param name="reference"></param>
		public void AddReference( RuntimeCrossSceneReference reference )
		{
			int index = _crossSceneReferences.FindIndex( reference.IsSameSource );
			if ( index >= 0 )
			{
				_crossSceneReferences[index] = reference;
			}
			else
			{
				_crossSceneReferences.Add( reference );
			}
		}

		/// <summary>
		/// Remove all of the stored cross-scene references that reference 'toScene'.
		/// </summary>
		public void ResetCrossSceneReferences( Scene toScene )
		{
			_crossSceneReferences.RemoveAll( x => (x.toScene.scene == toScene) );
		}

		void Awake()
		{
			AmsDebug.Log( this, "{0}.Awake() Scene: {1}. Path: {2}. Frame: {3}. Root Count: {4}", GetType().Name, gameObject.scene.name, gameObject.scene.path, Time.frameCount, gameObject.scene.rootCount );

			_referencesToResolve = new List<RuntimeCrossSceneReference>( _crossSceneReferences );

			//ConditionalResolveReferences( _referencesToResolve );
			StartCoroutine( CoWaitForSceneLoadThenResolveReferences(gameObject.scene) );

			AmsMultiSceneSetup.OnAwake += HandleNewSceneLoaded;
			AmsMultiSceneSetup.OnDestroyed += HandleSceneDestroyed;
		}

		void Start()
		{
			AmsDebug.Log( this, "{0}.Start() Scene: {1}. Path: {2}. Frame: {3}. Root Count: {4}", GetType().Name, gameObject.scene.name, gameObject.scene.path, Time.frameCount, gameObject.scene.rootCount );

			// A build might have just been performed, in that case clean-up the leftovers.
			PerformPostBuildCleanup();

			// Give us a second chance (helps initial load of scene)
			// For some reason in Awake(), the scene we belong to isn't considered "loaded"?!
			// Unfortunately we can get a Start() before CoWaitForSceneLoadThenResolveReferences finishes... so we need to nuke it if it's still running.
			StopAllCoroutines();
			ResolvePendingCrossSceneReferences();
		}

		void OnDestroy()
		{
			AmsMultiSceneSetup.OnAwake -= HandleNewSceneLoaded;
			AmsMultiSceneSetup.OnDestroyed -= HandleSceneDestroyed;
		}

		/// <summary>
		/// Whenever another scene is loaded, we have another shot at resolving more cross-scene references
		/// </summary>
		/// <param name="sceneSetup">The AmsMultiSceneSetup that was loaded</param>
		private void HandleNewSceneLoaded( AmsMultiSceneSetup sceneSetup )
		{
			StartCoroutine( CoWaitForSceneLoadThenResolveReferences(sceneSetup.gameObject.scene) );
		}

		/// <summary>
		/// Whenever a scene is destroyed, we will receive this callback.  In the editor, we can remember that we may be about to lose a cross-scene reference.
		/// </summary>
		/// <param name="sceneSetup"></param>
		private void HandleSceneDestroyed( AmsMultiSceneSetup sceneSetup )
		{
			var destroyedScene = sceneSetup.gameObject.scene;
			if ( !destroyedScene.IsValid() )
				return;

			// If our own scene is being destroyed, we don't need to do anymore work
			if ( destroyedScene == gameObject.scene )
				return;

			// Remove all of the pending refs for that scene.
			_referencesToResolve.RemoveAll( x => x.toScene.scene == destroyedScene );

			// Now we re-add all of the relevant refs to pending.  They'll be re-resolved when the scene is loaded again.
			var allRelevantRefs = _crossSceneReferences.Where( x => x.toScene.scene == destroyedScene );
			_referencesToResolve.AddRange( allRelevantRefs );
		}

		/// <summary>
		/// This is a co-routine for waiting until a given scene is loaded, then performing a cross-scene reference resolve
		/// </summary>
		/// <param name="scene">The scene to guarantee loaded</param>
		System.Collections.IEnumerator	CoWaitForSceneLoadThenResolveReferences( Scene scene )
		{
			if ( !scene.IsValid() )
				yield break;

			while ( !scene.isLoaded )
				yield return null;

			ResolvePendingCrossSceneReferences();
		}

		[ContextMenu("Retry Pending Resolves")]
		public void ResolvePendingCrossSceneReferences()
		{
			ConditionalResolveReferences( _referencesToResolve );
		}

		[ContextMenu("Retry ALL Resolves")]
		private void RetryAllResolves()
		{
			_referencesToResolve = new List<RuntimeCrossSceneReference>( _crossSceneReferences );
			ResolvePendingCrossSceneReferences();
		}

		private void ConditionalResolveReferences( List<RuntimeCrossSceneReference> references )
		{
			for(int i = references.Count - 1 ; i >= 0 ; --i)
			{
				var xRef = references[i];

				try
				{
					var fromScene = xRef.fromScene;
					var toScene= xRef.toScene;
					bool bIsReady = fromScene.isLoaded && toScene.isLoaded;

					AmsDebug.Log( this, "{0}.ConditionalResolveReferences() Scene: {1}. xRef: {2}. isReady: {3}. fromSceneLoaded: {4}. toSceneLoaded: {5}.", GetType().Name, gameObject.scene.name, xRef, bIsReady, fromScene.isLoaded, toScene.isLoaded );

					if ( bIsReady )
					{
						// Remove it from our list (assuming it goes through)
						references.RemoveAt(i);

						AmsDebug.Log( this, "Restoring Cross-Scene Reference {0}", xRef );
						xRef.Resolve();
					}
				}
				catch ( System.Exception ex )
				{
					Debug.LogException( ex, this );
				}
			}
		}

		void PerformPostBuildCleanup()
		{
			if ( Application.isEditor && !Application.isPlaying && _realSceneRootsForPostBuild.Count > 0 )
			{
				GameObject[] newSceneRoots = gameObject.scene.GetRootGameObjects();
				foreach( GameObject root in newSceneRoots )
				{
					if ( !_realSceneRootsForPostBuild.Contains(root) )
					{
						AmsDebug.LogWarning( this, "Destroying '{0}/{1}' since we've determined it's a temporary for a cross-scene reference", gameObject.scene.name, root.name );
						DestroyImmediate( root );
					}
				}

				_realSceneRootsForPostBuild.Clear();
			}
		}

#if UNITY_EDITOR
		public void OnAfterDeserialize() {}

		public void OnBeforeSerialize()
		{
			if ( !BuildPipeline.isBuildingPlayer )
				return;

			if ( EditorSceneManager.sceneCount < 2 )
				return;

			//
			// This deserves an explanation.  This gets serialized right before we do a build IF this scene is active
			// at the time of building.  The problem is, when we come back from the build, we get duplicate prefabs in
			// the scene since the cross-scene reference was still active.  So what we wanna do, is FAIL a resolve.
			//
			foreach( var xRef in _crossSceneReferences )
			{
				try
				{
					Debug.LogWarningFormat( "Nulling out cross-scene reference: {0}", xRef );
					xRef.SetToNull();
				} catch ( System.Exception ) {}
			}

			// Unfortunately, Unity 5.3.1p4 still causes these references to be saved into the temporary file... so we need to remove them when we come back from building.
			gameObject.scene.GetRootGameObjects( _realSceneRootsForPostBuild  );
		}
#endif

	} // class
} // namespace
