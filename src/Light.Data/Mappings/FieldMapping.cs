using System;

namespace Light.Data
{
	/// <summary>
	/// Field mapping.
	/// </summary>
	abstract class FieldMapping
	{
		//protected static readonly string _fieldRegex = @"^([a-zA-Z][a-z0-9A-Z_]*)$";

		#region 私有变量

		protected string _dbType;

		protected bool _isNullable;

		protected Type _objectType;

		protected string _name;

		protected string _indexName;

		protected DataMapping _typeMapping;

		protected TypeCode _typeCode = TypeCode.Empty;

		#endregion

		#region 公共属性

		public virtual string DBType {
			get {
				return _dbType;
			}
		}

		public virtual bool IsNullable {
			get {
				return _isNullable;
			}
		}

		public Type ObjectType {
			get {
				return _objectType;
			}
		}

		public string Name {
			get {
				return _name;
			}
		}

		public string IndexName {
			get {
				return _indexName;
			}
		}

		public DataMapping TypeMapping {
			get {
				return _typeMapping;
			}
		}

		public TypeCode TypeCode {
			get {
				return _typeCode;
			}
		}

		#endregion

		#region 公共方法

		protected FieldMapping (Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType)
		{
			this._objectType = type;
			if (type != null) {
				this._typeCode = Type.GetTypeCode (type);
			}
			this._name = fieldName;
			this._indexName = indexName;
			this._typeMapping = mapping;
			this._isNullable = isNullable;
			this._dbType = dbType;
		}

		public abstract object ToProperty (object value);



		#endregion
	}
}
