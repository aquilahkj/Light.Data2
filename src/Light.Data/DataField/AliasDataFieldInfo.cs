namespace Light.Data
{
    /// <summary>
    /// Alias data field info.
    /// </summary>
    internal class AliasDataFieldInfo : DataFieldInfo, IAliasDataFieldInfo
    {
        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public string AliasName { get; }

        /// <summary>
        /// Gets the base field info.
        /// </summary>
        /// <value>The base field info.</value>
        public DataFieldInfo BaseFieldInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasDataFieldInfo"/> class.
        /// </summary>
        /// <param name="info">Info.</param>
        /// <param name="alias">Alias.</param>
        internal AliasDataFieldInfo(DataFieldInfo info, string alias)
            : base(info.TableMapping, info.DataField)
        {
            BaseFieldInfo = info;
            AliasName = alias;
        }

        internal AliasDataFieldInfo(DataFieldInfo info, string alias, string aliasTableName)
            : base(info.TableMapping, info.DataField, aliasTableName)
        {
            BaseFieldInfo = info;
            AliasName = alias;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
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

        public string CreateAliasDataFieldSql(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            string field;
            if (isFullName) {
                if (_aliasTableName != null) {
                    field = factory.CreateFullDataFieldSql(_aliasTableName, FieldName);
                }
                else {
                    field = factory.CreateFullDataFieldSql(TableMapping, FieldName, state);
                }
            }
            else {
                field = factory.CreateDataFieldSql(FieldName);
            }

            return factory.CreateAliasFieldSql(field, AliasName);
        }
    }
}

