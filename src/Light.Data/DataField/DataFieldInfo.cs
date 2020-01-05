using System;

namespace Light.Data
{
    /// <summary>
    /// Data field info.
    /// </summary>
    internal class DataFieldInfo : BasicFieldInfo
    {
        /// <summary>
        /// Creates the alias table info.
        /// </summary>
        /// <returns>The alias table info.</returns>
        /// <param name="aliasTableName">Alias table name.</param>
        public virtual DataFieldInfo CreateAliasTableInfo(string aliasTableName)
        {
            var info = (DataFieldInfo)MemberwiseClone();
            info._aliasTableName = aliasTableName;
            return info;
        }

        internal DataFieldInfo(Type type, string name)
            : this(DataEntityMapping.GetEntityMapping(type), false, name)
        {

        }

        internal DataFieldInfo(DataEntityMapping mapping, bool customName, string name)
            : base(mapping, customName, name)
        {
        }

        internal DataFieldInfo(DataEntityMapping mapping, bool customName, string name, string aliasTableName)
            : base(mapping, customName, name)
        {
            _aliasTableName = aliasTableName;
        }

        internal DataFieldInfo(DataEntityMapping mapping, DataFieldMapping fieldMapping)
            : base(mapping, fieldMapping)
        {
        }

        internal DataFieldInfo(DataEntityMapping mapping, DataFieldMapping fieldMapping, string aliasTableName)
            : base(mapping, fieldMapping)
        {
            _aliasTableName = aliasTableName;
        }

        internal DataFieldInfo(DataFieldMapping fieldMapping)
            : base(fieldMapping.EntityMapping, fieldMapping)
        {
        }

        internal DataFieldInfo(DataFieldMapping fieldMapping, string aliasTableName)
            : base(fieldMapping.EntityMapping, fieldMapping)
        {
            _aliasTableName = aliasTableName;
        }

        internal DataFieldInfo(DataEntityMapping mapping)
            : base(mapping)
        {
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public int Position {
            get
            {
                if (DataField != null) {
                    return DataField.PositionOrder;
                }

                return -1;
            }
        }

        /// <summary>
        /// Gets the DbType of the field.
        /// </summary>
        /// <value>The type of the DB.</value>
        internal virtual string DBType => DataField.DBType;

        /// <summary>
        /// The name of the alias table.
        /// </summary>
        protected string _aliasTableName;

        internal virtual string AliasTableName => _aliasTableName;

        internal virtual string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            if (isFullName)
            {
                if (_aliasTableName != null) {
					return factory.CreateFullDataFieldSql(_aliasTableName, FieldName);
				}

                return factory.CreateFullDataFieldSql(TableMapping, FieldName, state);
            }

            return factory.CreateDataFieldSql(FieldName);
        }

        /// <summary>
        /// Tos the parameter.
        /// </summary>
        /// <returns>The parameter.</returns>
        /// <param name="value">Value.</param>
        internal virtual object ToParameter(object value)
        {
            return DataField.ToParameter(value);
        }
    }
}
