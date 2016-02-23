using UnityEngine;
using System.Collections;

namespace CodingJar.MultiScene
{
	/// <summary>
	/// A UniqueObject that is represented by a Scene and an ID in the Scene.
	/// </summary>
	[System.Serializable]
	public partial struct UniqueObject
	{
		public AmsSceneReference	scene;
		public string				fullPath;
		public string				componentName;

		/// <summary>
		/// Resolve a cross-scene reference if possible.
		/// </summary>
		/// <returns>The cross-scene referenced object if it's possible</returns>
		public Object	Resolve()
		{
			var scene = this.scene.scene;

			if ( !scene.IsValid() )
				return null;

			// Try to find the Object
			GameObject gameObject = GameObjectEx.FindBySceneAndPath( scene, fullPath );
			if ( !gameObject )
				return null;

			if ( string.IsNullOrEmpty(componentName) )
				return gameObject;

			return gameObject.GetComponent( componentName );
		}

		public override string ToString()
		{
			return string.Format( "{0}'{1}' ({2})", scene.name, fullPath, string.IsNullOrEmpty(componentName) ? "GameObject" : componentName );
		}

	} // struct
} // namespace 
