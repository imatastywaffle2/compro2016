using UnityEngine;
using UnityEngine.SceneManagement;
using CodingJar.MultiScene;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[System.Serializable]
public struct AmsSceneReference
{
	public string	editorAssetGUID;
	public string	name;

	[UnityEngine.Serialization.FormerlySerializedAs("path")]
	[SerializeField]	string	_path;

	public AmsSceneReference( Scene scene ) : this( scene.path ) {}

	public AmsSceneReference( string scenePath )
	{
#if UNITY_EDITOR
		var scene = EditorSceneManager.GetSceneByPath( scenePath );
		editorAssetGUID = AssetDatabase.AssetPathToGUID( scenePath );
#else
		var scene = SceneManager.GetSceneByPath( scenePath );
		editorAssetGUID = "";
#endif

		name = scene.name;
		_path = scene.path;
	}
	
	public Scene scene
	{
		get
		{
#if UNITY_EDITOR
			if ( !Application.isPlaying )
			{
				// The easy case
				var scene = EditorSceneManager.GetSceneByPath( editorPath );
				if ( scene.IsValid() )
					return scene;

				// Welcome to my hell!
				// If we're building a scene that was open, we have temporary scenes named 0.backup, 1.backup etc.
				if ( BuildPipeline.isBuildingPlayer )
				{
					// Find the scene by using the stored path
					var allMultiSceneSetups = Resources.FindObjectsOfTypeAll<AmsMultiSceneSetup>();
					foreach( var sceneSetup in allMultiSceneSetups )
					{
						// Did we just find it?  Should I double-check that we're in a Temp path?
						if ( sceneSetup.scenePath == editorPath )
							return sceneSetup.gameObject.scene;
					}
				}

				return scene;
			}
#endif	// UNITY_EDITOR

			// Try the editor path first, it works at least in the Editor
			var editorScene = SceneManager.GetSceneByPath( editorPath );
			if ( editorScene.IsValid() )
				return editorScene;

			return SceneManager.GetSceneByPath( runtimePath );
		}
	}

	public bool	IsValid()
	{
		return scene.IsValid();
	}

	public bool isLoaded
	{
		get	{	return scene.isLoaded;	}
	}

	public string editorPath
	{
		get { return _path; }
		set { _path = value; }
	}

	public string runtimePath
	{
		get
		{
			int startIndex = 0;
			int endIndex = _path.Length;

			if ( _path.StartsWith( "Assets/" ) )
			{
				startIndex = "Assets/".Length;
				endIndex -= startIndex;
			}

			if ( _path.EndsWith(".unity") )
			{
				endIndex -= ".unity".Length;
			}

			return _path.Substring( startIndex, endIndex );
		}
	}
}
