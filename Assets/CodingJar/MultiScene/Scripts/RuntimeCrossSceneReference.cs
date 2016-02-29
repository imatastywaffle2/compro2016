using UnityEngine;
using System.Reflection;

namespace CodingJar.MultiScene
{
	public class ResolveException : System.Exception
	{
		public ResolveException( string message ) : base( message ) {}
	}

	[System.Serializable]
    public struct RuntimeCrossSceneReference
    {
        // From which UniqueObject.PropertyPath?
		[SerializeField]	UniqueObject	_fromObject;
		[SerializeField]	string			_fromField;

        // Which UniqueObject are we referencing?
        [SerializeField]	UniqueObject	_toObject;

		public RuntimeCrossSceneReference( UniqueObject from, string fromField, UniqueObject to )
		{
			_fromObject = from;
			_fromField = fromField;
			_toObject = to;

			_fromObjectCached = null;
			_toObjectCached = null;
		}

		private	Object	_fromObjectCached;
		public	Object	fromObject
		{
			get
			{
				if ( !_fromObjectCached )
					_fromObjectCached = _fromObject.Resolve();

				return _fromObjectCached;
			}
		}

		private	Object	_toObjectCached;
		public	Object	toObject
		{
			get
			{
				if ( !_toObjectCached )
					_toObjectCached = _toObject.Resolve();

				return _toObjectCached;
			}
		}

		public	AmsSceneReference	fromScene
		{
			get { return _fromObject.scene; }
		}

		public AmsSceneReference	toScene
		{
			get { return _toObject.scene; }
		}

        public override string ToString()
        {
            return string.Format( "{0}.{1} => {2}", _fromObject, _fromField, _toObject );
        }

		public bool IsSameSource( RuntimeCrossSceneReference other )
		{
			return other._fromObject.Equals(this._fromObject) &&
				other._fromField == this._fromField;
		}

		/// <summary>
		/// Set the cross-scene reference to null.  We want to do this to prevent saves.
		/// </summary>
		public void SetToNull()
		{
			var fromObject = this.fromObject;
			if ( !fromObject )
				throw new ResolveException( string.Format( "Cross-Scene Ref: {0}. Could not Resolve fromObject {1}", this, fromObject ) );

			ResolveInternal( fromObject, null, _fromField, this );
		}

		/// <summary>
		/// Perform a resolve on a cross-scene reference.
		/// This functions throws an exception if it fails.
		/// </summary>
		public void Resolve()
		{
			var fromObject = this.fromObject;
			if ( !fromObject )
				throw new ResolveException( string.Format( "Cross-Scene Ref: {0}. Could not Resolve fromObject {1}", this, fromObject ) );

			Object toObject = this.toObject;
			if ( !toObject )
				throw new ResolveException( string.Format( "Cross-Scene Ref: {0}. Could not Resolve toObject {1}", this, toObject ) );

			ResolveInternal( fromObject, toObject, _fromField, this );
		}

		private static void ResolveInternal( Object fromObject, Object toObject, string fromField, RuntimeCrossSceneReference debugThis )
		{
			string[] parseField = fromField.Split(',');
			string fieldName = parseField[0];
			
			// Check if it's an array
			int arrayIndex = -1;
			if ( parseField.Length > 1 )
			{
				if ( !int.TryParse(parseField[1], out arrayIndex) )
					throw new ResolveException( string.Format("Cross-Scene Ref: {0}. Could not parse list index {1} from {2}", debugThis, parseField[1], fromField) );
			}

			FieldInfo field = fromObject.GetType().GetField( fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
			if ( field == null )
				throw new ResolveException( string.Format( "Cross-Scene Ref: {0}. Could not find Field {1}", debugThis, fieldName ) );

			AssignField( fromObject, toObject, field, arrayIndex );
			// Success!
		}

		/// <summary>
		/// Assigns a specific field fromObject.field[arrayIndex] -> toObject
		/// </summary>
		/// <param name="fromObject">The object to assign from</param>
		/// <param name="toObject">The target object</param>
		/// <param name="field">The field that should be assigned</param>
		/// <param name="arrayIndex">The array index of that field (or negative if it's not an array)</param>
		private static void AssignField( Object fromObject, Object toObject, FieldInfo field, int arrayIndex )
		{
			bool isArray = arrayIndex >= 0;
			if ( isArray )
			{
				var list = field.GetValue( fromObject ) as System.Collections.IList;
				if ( list == null )
					throw new ResolveException( string.Format( "Expected collection of elements for property {0} but field type is {1}", field.Name, field.FieldType.Name ) );
				else if ( list.Count < arrayIndex )
					throw new ResolveException( string.Format( "Expected collection of at least {0} elements from property {1}", arrayIndex, field.Name ) );

				// Successful assign!
				list[arrayIndex] = toObject;
				return;
			}
			else if ( !field.FieldType.IsAssignableFrom( toObject.GetType() ) )
			{
				throw new ResolveException( string.Format( "Field {0} of type {1} is not compatible with {2} of type {3}", field.Name, field.FieldType, toObject, toObject.GetType().Name ) );
			}

			// Successful assign!
			field.SetValue( fromObject, toObject );
		}
	} // struct 
} // namespace 