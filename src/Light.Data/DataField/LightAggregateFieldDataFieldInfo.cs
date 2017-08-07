namespace Light.Data
{
	class LightAggregateFieldDataFieldInfo : LightAggregateDataFieldInfo
	{
		readonly DataFieldInfo _baseFieldInfo;

		readonly AggregateType _type;

		readonly bool _distinct;

		public LightAggregateFieldDataFieldInfo (DataFieldInfo fieldInfo, AggregateType type, bool distinct)
			: base (fieldInfo.TableMapping)
		{
			_baseFieldInfo = fieldInfo;
			_type = type;
			_distinct = distinct;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			string field = _baseFieldInfo.CreateSqlString (factory, isFullName, state);

			switch (_type) {
			case AggregateType.COUNT:
				sql = factory.CreateCountSql (field, _distinct);
				break;
			case AggregateType.SUM:
				sql = factory.CreateSumSql (field, _distinct);
				break;
			case AggregateType.AVG:
				sql = factory.CreateAvgSql (field, _distinct);
				break;
			case AggregateType.MAX:
				sql = factory.CreateMaxSql (field);
				break;
			case AggregateType.MIN:
				sql = factory.CreateMinSql (field);
				break;
			}

			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

