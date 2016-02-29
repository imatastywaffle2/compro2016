using UnityEngine;
using UnityEditor;

using UnityEngine.SceneManagement;

namespace CodingJar.MultiScene.Editor
{
	/// <summary>
	/// Structure containing an serialized cross-scene reference.
	/// </summary>
	public struct EditorCrossSceneReference
	{
		public SerializedProperty	fromProperty;
		public Object				toInstance;
			
		public Scene		fromScene;
		public Scene		toScene;

		public Object		fromObject
		{
			get {	return fromProperty.serializedObject.targetObject;	}
		}

        public override string ToString()
        {
			string fromString = fromObject ? fromObject.ToString() : "(null)";
			string toString = toInstance ? toInstance.ToString() : "(null)";
			
			var fromGameObject = GameObjectEx.EditorGetGameObjectFromComponent(fromObject);
			if ( fromGameObject )
				fromString = string.Format( "{0} ({1})", fromGameObject.GetFullName(), fromObject.GetType() );

			var toGameObject = GameObjectEx.EditorGetGameObjectFromComponent(toInstance);
			if ( toGameObject )
				toString = string.Format( "{0} ({1})", toGameObject.GetFullName(), toInstance.GetType() );

            return string.Format("{0}.{1} => {2}", fromString, fromProperty.propertyPath, toString);
        }

        public override bool Equals( object obj )
        {
            if ( obj is EditorCrossSceneReference )
            {
                var other = (EditorCrossSceneReference)obj;
                return ( other.fromProperty == fromProperty && other.toInstance == toInstance );
            }

            return base.Equals( obj );
        }

        public override int GetHashCode()
        {
            return fromProperty.GetHashCode() + toInstance.GetHashCode();
        }

		public RuntimeCrossSceneReference	ToSerializable()
		{
			string fromField = ToSerializableField( fromProperty );
			return new RuntimeCrossSceneReference( new UniqueObject(fromObject), fromField, new UniqueObject(toInstance) );
		}

		private string ToSerializableField( SerializedProperty property )
		{
			string returnValue = property.name;
			if ( returnValue != "data" )
				return returnValue;

			// "data" indicates an array.
			// If we're serializing an array, the field is "fieldName.Array.data[0]".  So we need to chop-off .Array.data[x].
			const string arrayIndicator = ".Array.data[";
			string propertyPath = fromProperty.propertyPath;

			// Now try to extract the index from .Array.data[index].
			int propertyIndex = propertyPath.IndexOf( arrayIndicator );
			if ( propertyIndex >= 0 )
			{
				string fieldName = propertyPath.Substring( 0, propertyIndex );

				int indexOffset = propertyIndex + arrayIndicator.Length;
				string indexString = propertyPath.Substring( indexOffset, propertyPath.Length - indexOffset - 1 );

				int arrayIndex;
				if ( int.TryParse(indexString, out arrayIndex) )
				{
					return string.Format( "{0},{1}", fieldName, arrayIndex );
				}
				else
				{
					arrayIndex = -1;
					AmsDebug.LogError( null, "Could not parse array index for property path {0}", propertyPath );
				}
			}

			return returnValue;
		}
	} // struct
}
