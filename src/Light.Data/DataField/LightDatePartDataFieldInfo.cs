namespace Light.Data
{
	class LightDatePartDataFieldInfo : LightDataFieldInfo
	{
		readonly DatePart _part;

		readonly DataFieldInfo _baseFieldInfo;

		internal LightDatePartDataFieldInfo (DataFieldInfo info, DatePart part)
			: base (info.TableMapping)
		{
			_baseFieldInfo = info;
			_part = part;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			string field = _baseFieldInfo.CreateSqlString (factory, isFullName, state);
			switch (_part) {
			case DatePart.Year:
				sql = factory.CreateYearSql (field);
				break;
			case DatePart.Month:
				sql = factory.CreateMonthSql (field);
				break;
			case DatePart.Day:
				sql = factory.CreateDaySql (field);
				break;
			case DatePart.Hour:
				sql = factory.CreateHourSql (field);
				break;
			case DatePart.Minute:
				sql = factory.CreateMinuteSql (field);
				break;
			case DatePart.Second:
				sql = factory.CreateSecondSql (field);
				break;
			case DatePart.Week:
				sql = factory.CreateWeekSql (field);
				break;
			case DatePart.DayOfWeek:
				sql = factory.CreateWeekDaySql (field);
				break;
			case DatePart.DayOfYear:
				sql = factory.CreateYearDaySql (field);
				break;
			}

			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

