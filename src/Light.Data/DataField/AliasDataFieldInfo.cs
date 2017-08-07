namespace Light.Data
{
    /// <summary>
    /// Alias data field info.
    /// </summary>
    class AliasDataFieldInfo : DataFieldInfo, IAliasDataFieldInfo
    {
        readonly DataFieldInfo _baseFieldInfo;

        readonly string _aliasName;

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public string AliasName {
            get {
                return _aliasName;
            }
        }

        /// <summary>
        /// Gets the base field info.
        /// </summary>
        /// <value>The base field info.</value>
        public DataFieldInfo BaseFieldInfo {
            get {
                return _baseFieldInfo;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasDataFieldInfo"/> class.
        /// </summary>
        /// <param name="info">Info.</param>
        /// <param name="alias">Alias.</param>
        internal AliasDataFieldInfo(DataFieldInfo info, string alias)
            : base(info.TableMapping, info.DataField)
        {
            _baseFieldInfo = info;
            _aliasName = alias;
        }

        internal AliasDataFieldInfo(DataFieldInfo info, string alias, string aliasTableName)
            : base(info.TableMapping, info.DataField, aliasTableName)
        {
            _baseFieldInfo = info;
            _aliasName = alias;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            if (isFullName) {
                if (this._aliasTableName != null) {
                    return factory.CreateFullDataFieldSql(this._aliasTableName, FieldName);
                }
                else {
                    return factory.CreateFullDataFieldSql(TableMapping, FieldName, state);
                }
            }
            else {
                return factory.CreateDataFieldSql(FieldName);
            }
        }

        public string CreateAliasDataFieldSql(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            string field;
            if (isFullName) {
                if (this._aliasTableName != null) {
                    field = factory.CreateFullDataFieldSql(this._aliasTableName, FieldName);
                }
                else {
                    field = factory.CreateFullDataFieldSql(TableMapping, FieldName, state);
                }
            }
            else {
                field = factory.CreateDataFieldSql(FieldName);
            }

            return factory.CreateAliasFieldSql(field, this._aliasName);
        }
    }
}

