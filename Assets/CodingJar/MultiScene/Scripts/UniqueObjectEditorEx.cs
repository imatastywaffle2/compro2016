using UnityEngine;
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
	/// Editor extensions for UniqueObject to make manipulations quicker (and stronger) when using the Editor.
	/// </summary>
	public partial struct UniqueObject
	{
#if USE_STRONG_EDITOR_REFS
		public int					editorLocalId;

		private int	GetEditorId( Object obj )
		{
			int editorId = 0;

			PropertyInfo inspectorModeInfo = typeof(UnityEditor.SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
			SerializedObject sObj = new SerializedObject(obj);
			inspectorModeInfo.SetValue( sObj, UnityEditor.InspectorMode.Debug, null );

			SerializedProperty sProp = sObj.FindProperty( "m_LocalIdentfierInFile" );
			if ( sProp != null )
			{
				editorId = sProp.intValue;
				sProp.Dispose();
			}

			sObj.Dispose();

			return editorId;
		}

		public Object	EditorResolveSlow( bool bLoadScene )
		{
			var scene = this.scene.scene;

			if ( !scene.IsValid() )
				return null;

			if ( !scene.isLoaded )
			{
				if ( !bLoadScene )
					return null;

				EditorSceneManager.LoadScene( scene.path );
			}

			AmsDebug.LogError( null, "Performing EditorResolveSlow but it was never tested" );

			// Find the object (this is potentially very slow).
			Object[] allObjs = EditorUtility.CollectDeepHierarchy( scene.GetRootGameObjects() );
			foreach( var obj in allObjs )
			{
				if ( editorLocalId == Unsupported.GetLocalIdentifierInFile( obj.GetInstanceID() ) )
					return obj;
			}

			return null;
		}
#endif

#if UNITY_EDITOR
		public UniqueObject( Object obj )
		{
#if USE_STRONG_EDITOR_REFS
			editorLocalId = 0;
#endif

			scene = new AmsSceneReference();
			fullPath = string.Empty;
			componentName = string.Empty;
			version = CurrentSerializedVersion;
			componentIndex = 0;

			if ( !obj )
				return;

			GameObject gameObject = GameObjectEx.EditorGetGameObjectFromComponent( obj );
			if ( gameObject )
			{
				scene = new AmsSceneReference( gameObject.scene );
				fullPath = gameObject.GetFullName();

				Component comp = obj as Component;
				if ( comp )
				{
					componentName = obj.GetType().AssemblyQualifiedName;
					gameObject.GetComponents( obj.GetType(), _reusableComponentsList );
					componentIndex = _reusableComponentsList.IndexOf( comp );
				}
			}

#if USE_STRONG_EDITOR_REFS
			editorLocalId = GetEditorId( obj );
#endif
		}
#endif

	} // struct
} // namespace 
