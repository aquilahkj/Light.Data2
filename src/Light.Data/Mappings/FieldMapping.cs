using System;

namespace Light.Data
{
    /// <summary>
    /// Field mapping.
    /// </summary>
    internal abstract class FieldMapping
    {
        #region Private Field

        protected string _dbType;

        protected bool _isNullable;

        protected Type _objectType;

        protected string _name;

        protected string _indexName;

        protected DataMapping _typeMapping;

        protected TypeCode _typeCode = TypeCode.Empty;

        #endregion

        #region Public Property

        public virtual string DBType => _dbType;

        public virtual bool IsNullable => _isNullable;

        public Type ObjectType => _objectType;

        public string Name => _name;

        public string IndexName => _indexName;

        public DataMapping TypeMapping => _typeMapping;

        public TypeCode TypeCode => _typeCode;

        #endregion

        #region Public Method

        protected FieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType)
        {
            _objectType = type;
            if (type != null) {
                _typeCode = Type.GetTypeCode(type);
            }
            _name = fieldName;
            _indexName = indexName;
            _typeMapping = mapping;
            _isNullable = isNullable;
            if (dbType != null) {
                _dbType = dbType.Trim();
            }
        }

        public abstract object ToProperty(object value);



        #endregion
    }
}
