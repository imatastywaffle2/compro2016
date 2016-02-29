using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Linq;

using CodingJar;

#if UNITY_EDITOR
	using UnityEditor;
	using UnityEditor.SceneManagement;
#endif

namespace CodingJar.MultiScene
{
	[ExecuteInEditMode]
	public class AmsMultiSceneSetup : MonoBehaviour, ISerializationCallbackReceiver
	{
		[System.Serializable]
		public enum LoadMethod
		{
			Baked,
			Additive,
			AdditiveAsync,
			DontLoad
		}

		[System.Serializable]
		public class SceneEntry
		{
			// The scene that is automatically processed both in Editor and Runtime
			[BeginReadonly]
			public AmsSceneReference	scene;

			[Tooltip("Should this be automatically loaded in the Editor?")]
			[UnityEngine.Serialization.FormerlySerializedAs("isLoaded")]
			public bool					loadInEditor;

			[EndReadonly]
			[Tooltip("How should we load this scene at Runtime?")]
			public LoadMethod			loadMethod;

			public AsyncOperation		asyncOp { get; set; }

			public override string ToString()
			{
				return string.Format( "{0} loadInEditor: {1} loadMethod: {2}", scene.name, loadInEditor, loadMethod );
			}

			/// <summary>
			/// Overridden Equals to we can compare entries.  Entries with the same scene references and load settings are considered equal.
			/// </summary>
			public override bool Equals( object obj )
			{
				if ( this == obj )
					return true;

				var other = obj as SceneEntry;
				if ( other == null )
					return false;

				return (scene.Equals(other.scene)) && (loadInEditor == other.loadInEditor) && (loadMethod == other.loadMethod) && (asyncOp == other.asyncOp);
			}

			public override int GetHashCode()
			{
				return scene.GetHashCode() * 4 + loadInEditor.GetHashCode() * 2 + loadMethod.GetHashCode();
			}

#if UNITY_EDITOR
			/// <summary>
			/// Construct from a Unity SceneSetup
			/// </summary>
			public SceneEntry( UnityEditor.SceneManagement.SceneSetup sceneSetup )
			{
				scene = new AmsSceneReference( sceneSetup.path );

				loadInEditor = sceneSetup.isLoaded;
				loadMethod = LoadMethod.Additive;
			}
#endif
		}

		[SerializeField]	bool				_isMainScene = false;
		[SerializeField]	List<SceneEntry>	_sceneSetup = new List<SceneEntry>();
		[Readonly, SerializeField]	string				_thisScenePath;

		public static System.Action<AmsMultiSceneSetup>	OnAwake;
		
#if UNITY_EDITOR
		/// <summary>
		/// Easy accessor for Editor
		/// </summary>
		public bool isMainScene
		{
			get { return _isMainScene; }
			set { _isMainScene = value; }
		}

		/// <summary>
		/// Easy accessor for the Editor
		/// </summary>
		public string scenePath
		{
			get { return _thisScenePath; }
		}
#endif

		/// <summary>
		/// Read-only access to the Scene Setup.
		/// </summary>
		public System.Collections.ObjectModel.ReadOnlyCollection<SceneEntry>	GetSceneSetup()
		{
			return _sceneSetup.AsReadOnly();
		}

		/// <summary>
		/// Awake can be used to tell anyone that a Scene has just been loaded.
		/// Due to a bug in PostProcessScene, this is the first thing to occur in a loaded scene.
		/// </summary>
		void Awake()
		{
			AmsDebug.Log( this, "{0}.Awake() (Scene {1}). Frame: {2}", GetType().Name, gameObject.scene.name, Time.frameCount );
			_thisScenePath = gameObject.scene.path;

			// Notify any listeners we're now awake
			if ( OnAwake != null )
				OnAwake( this );

			if ( _isMainScene )
			{
				if ( !Application.isEditor || gameObject.scene.isLoaded || Time.frameCount > 1 )
					LoadSceneSetup();
			}
		}

		void OnDestroy()
		{
#if UNITY_EDITOR
			// Make sure we update the settings every time we unload/load a Scene
			EditorApplication.hierarchyWindowChanged -= OnBeforeSerialize;
#endif
		}

		/// <summary>
		/// Start is executed just before the first Update (a frame after Awake/OnEnable).
		/// We execute the Scene loading here because Unity has issues loading scenes during the initial Awake/OnEnable calls.
		/// </summary>
		void Start()
		{
			AmsDebug.Log( this, "{0}.Start() Scene: {1}. Frame: {2}", GetType().Name, gameObject.scene.name, Time.frameCount );

			// Second chance at loading scenes
			if ( _isMainScene )
				LoadSceneSetup();

#if UNITY_EDITOR
			// Make sure we update the settings every time we unload/load a Scene
			// This is strategically placed after the LoadSceneSetup().
			if ( !EditorApplication.isPlaying )
			{
				EditorApplication.hierarchyWindowChanged -= OnBeforeSerialize;
				EditorApplication.hierarchyWindowChanged += OnBeforeSerialize;
			}
#endif
		}

		void LoadSceneSetup()
		{
			if ( !_isMainScene )
				return;

		#if UNITY_EDITOR

			if ( !Application.isPlaying )
				LoadSceneSetupInEditor();
			else
				LoadSceneSetupAtRuntime();
			
		#else

			LoadSceneSetupAtRuntime();
			
		#endif
		}

		/// <summary>
		/// Load Scene Setup at Runtime.
		/// </summary>
		private void LoadSceneSetupAtRuntime()
		{
			foreach( var entry in _sceneSetup )
			{
				LoadEntryAtRuntime( entry );
			}
		}

		/// <summary>
		/// Load a particular Scene Entry
		/// </summary>
		/// <param name="entry">The Entry to load</param>
		private void LoadEntryAtRuntime( SceneEntry entry )
		{
			// Don't load 
			if ( entry.loadMethod == LoadMethod.DontLoad )
				return;

			// Already loaded, try editor first
			var existingScene = SceneManager.GetSceneByPath(entry.scene.editorPath);

			// Try runtime path
			if ( !existingScene.IsValid() )
				existingScene = SceneManager.GetSceneByPath(entry.scene.runtimePath);

			if ( Application.isEditor )
			{
				if ( entry.loadMethod == LoadMethod.Baked )
				{
					if ( !existingScene.IsValid() )
					{
						AmsDebug.LogWarning( this, "Attemping to load {0} even though it's baked. It will most likely fail which is not indicative of runtime behaviour", entry.scene.name );
						SceneManager.LoadScene( entry.scene.runtimePath, LoadSceneMode.Additive );
						StartCoroutine( CoWaitAndBake(entry) );
					}
					else
					{
						BakeScene( entry );
					}

					return;
				}
			}

			// If it's already loaded, return early
			if( existingScene.IsValid() )
				return;

			if ( entry.loadMethod == LoadMethod.AdditiveAsync )
			{
				AmsDebug.Log( this, "Loading {0} Asynchronously", entry.scene.name );
				entry.asyncOp = SceneManager.LoadSceneAsync( entry.scene.runtimePath, LoadSceneMode.Additive );
				return;
			}

			if ( entry.loadMethod == LoadMethod.Additive )
			{
				AmsDebug.Log( this, "Loading {0}", entry.scene.name );
				SceneManager.LoadScene( entry.scene.runtimePath, LoadSceneMode.Additive );
				return;
			}
		}

		/// <summary>
		/// This executes in the Editor when a behaviour is initially added to a GameObject.
		/// </summary>
		void Reset()
		{
			_isMainScene = (SceneManager.GetActiveScene() == gameObject.scene);
		}

		// Intentionally left blank
		public void OnAfterDeserialize() {}

		/// <summary>
		/// OnBeforeSerialize is called whenever we're about to save or inspect this Component.
		/// We want to match exactly what the Editor has in terms of Scene setup, so we do it here.
		/// </summary>
		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR
			if ( !this || BuildPipeline.isBuildingPlayer || Application.isPlaying )
				return;

			// Save off the scene path
			if ( gameObject && gameObject.scene.IsValid() )
				_thisScenePath = gameObject.scene.path;

			// We don't care about the scene setup
			if ( !_isMainScene )
				return;

			var newSceneSetup = new List<SceneEntry>();
			var activeScene = EditorSceneManager.GetActiveScene();

			// Update our scene setup
			SceneSetup[] editorSceneSetup = EditorSceneManager.GetSceneManagerSetup();
			for(int i = 0 ; i < editorSceneSetup.Length ; ++i)
			{
				// If we're the active scene, don't save it.
				var editorEntry = editorSceneSetup[i];
				if ( editorEntry.path == activeScene.path )
					continue;

				var newEntry = new SceneEntry(editorEntry);
				newSceneSetup.Add( newEntry );
				
				// Save the baked settings
				var oldEntry = _sceneSetup.Find( x => newEntry.scene.Equals(x.scene) );
				if ( oldEntry != null )
				{
					newEntry.loadMethod = oldEntry.loadMethod;
				}
			}

			// If we had a new scene setup...
			if ( !newSceneSetup.SequenceEqual(_sceneSetup) )
			{
				_sceneSetup = newSceneSetup;
				EditorUtility.SetDirty( this );

				if ( gameObject )
					EditorSceneManager.MarkSceneDirty( gameObject.scene );
			}
#endif
		}

#if UNITY_EDITOR
		/// <summary>
		/// Loads the scene setup in the Editor
		/// </summary>
		private void LoadSceneSetupInEditor()
		{
			// Don't load this if we just dragged in a scene.
			if ( EditorSceneManager.GetActiveScene() != gameObject.scene )
				return;

			foreach( var entry in _sceneSetup )
			{
				LoadEntryInEditor(entry);
			}
		}

		/// <summary>
		/// Loads a particular Scene Entry in the Editor
		/// </summary>
		/// <param name="entry">The entry to load</param>
		private void LoadEntryInEditor( SceneEntry entry )
		{
			// Bad time to do this.
			if ( EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying )
				return;

			// We can't do this
			if ( string.IsNullOrEmpty(entry.scene.editorPath) || entry.scene.editorPath == gameObject.scene.path )
				return;

			bool bShouldLoad = entry.loadInEditor && AmsPreferences.AllowAutoload;
			var scene = entry.scene.scene;

			try
			{
				if ( !scene.IsValid() )
				{
					if ( bShouldLoad )
						EditorSceneManager.OpenScene( entry.scene.editorPath, OpenSceneMode.Additive );
					else
						EditorSceneManager.OpenScene( entry.scene.editorPath, OpenSceneMode.AdditiveWithoutLoading );
				}
				else if ( bShouldLoad != scene.isLoaded )
				{
					if ( bShouldLoad && !scene.isLoaded )
						EditorSceneManager.OpenScene( entry.scene.editorPath, OpenSceneMode.Additive );
					else
						EditorSceneManager.CloseScene( scene, false );
				}
			} catch ( System.Exception ex ) {	Debug.LogException( ex, this );	}
		}
#endif

		/// <summary>
		/// Scene loads take a frame to complete, so we wait a frame.
		/// </summary>
		/// <param name="entry">The scene entry</param>
		private System.Collections.IEnumerator	CoWaitAndBake( SceneEntry entry )
		{
			int currentFrame = Time.frameCount;
			while ( Time.frameCount != currentFrame )
				yield return null;

			BakeScene( entry );
		}

		void BakeScene( SceneEntry entry )
		{
			var scene = entry.scene.scene;
			var activeScene = SceneManager.GetActiveScene();
			if ( scene == activeScene || scene == gameObject.scene )
				return;

			if ( entry.loadMethod != LoadMethod.Baked )
				return;

			if ( !gameObject.scene.isLoaded )
				return;

			AmsDebug.Log( this, "Merging {0} into {1}", scene.path, gameObject.scene.path );

			var targetCrossRefs = AmsCrossSceneReferences.GetSceneSingleton( gameObject.scene, false );
			if ( targetCrossRefs )
				targetCrossRefs.ResolvePendingCrossSceneReferences();

			var sourceCrossRefs = AmsCrossSceneReferences.GetSceneSingleton( entry.scene.scene, false );
			if ( sourceCrossRefs )
			{
				sourceCrossRefs.ResolvePendingCrossSceneReferences();
				
				// #warning this isn't right
				// TODO: This isn't right because we still need this as a marker for future cross-scene references
				GameObject.Destroy( sourceCrossRefs.gameObject );
			}

			SceneManager.MergeScenes( scene, gameObject.scene );
		}
	}
}
