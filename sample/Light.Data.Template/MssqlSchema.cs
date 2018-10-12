using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Data.Template
{
    public class MssqlSchema
    {
        string _dataBaseName;

        Dictionary<string, Table> _dict = null;

        string _connectionString;

        public MssqlSchema(string dataBaseName, string connectionString)
        {
            this._dataBaseName = dataBaseName;
            this._connectionString = connectionString;
            this._dict = new Dictionary<string, Table>();
        }

        public List<Table> GetTables()
        {
            const string tableCommandText = @"SELECT A.name as TableName,A.object_id as TableCode, C.value as CommentText FROM sys.tables A left JOIN sys.extended_properties C ON C.major_id = A.object_id  and minor_id=0 WHERE A.name = N'{0}'";

            const string columnCommandText = @"
with indexcte as(
	select
		ic.column_id,
		ic.index_column_id,
		ic.object_id
	from
		{0}.sys.indexes idx
	inner join {0}.sys.index_columns ic on idx.index_id = ic.index_id
	and idx.object_id = ic.object_id
	where
		idx.object_id = object_id('{0}.dbo.{1}')
	and idx.is_primary_key = 1
) select
	colm.column_id ColumnId,
	cast(
		case
		when indexcte.column_id is null then
			0
		else
			1
		end as bit
	) ColumnKey,
	cast(colm.max_length as int) bytelength,
	(
		case
		when systype.name = 'nvarchar'
		and colm.max_length > 0 then
			colm.max_length / 2
		when systype.name = 'nchar'
		and colm.max_length > 0 then
			colm.max_length / 2
		when systype.name = 'ntext'
		and colm.max_length > 0 then
			colm.max_length / 2
		else
			colm.max_length
		end
	) MaxLength,
	colm.name ColumnName,
	systype.name DataType,
	colm.is_identity IsIdentity,
	colm.is_nullable AllowNull,
	cast(colm.precision as int) Precision,
	cast(colm.scale as int) Scale,
	prop.value ColumnComment
from
	{0}.sys.columns colm
inner join {0}.sys.types systype on colm.system_type_id = systype.system_type_id
and colm.user_type_id = systype.user_type_id
left join {0}.sys.extended_properties prop on colm.object_id = prop.major_id
and colm.column_id = prop.minor_id
left join indexcte on colm.column_id = indexcte.column_id
and colm.object_id = indexcte.object_id
where
	colm.object_id = object_id('{0}.dbo.{1}')
order by
	colm.column_id";

            List<Table> tables = new List<Table>();
            foreach (TableNameSet tableNameSet in DbSetting.GetTables()) {
                string tableCommandStr = String.Format(tableCommandText, tableNameSet.TableName);
                SqlConnection tableConn = new SqlConnection(_connectionString);
                tableConn.Open();
                SqlCommand tableCommand = new SqlCommand(tableCommandStr, tableConn);
                SqlDataAdapter tableAd = new SqlDataAdapter(tableCommand);
                DataSet tableDs = new DataSet();
                tableAd.Fill(tableDs);
                DataTable tableColumns = tableDs.Tables[0];
                tableConn.Close();
                if (tableColumns.Rows.Count == 0) {
                    continue;
                }
                string tableComment = Convert.ToString(tableColumns.Rows[0]["CommentText"]);
                string tableCode = Convert.ToString(tableColumns.Rows[0]["TableCode"]);
                string columnCommandStr = String.Format(columnCommandText, this._dataBaseName, tableNameSet.TableName);
                SqlConnection columnConn = new SqlConnection(_connectionString);
                columnConn.Open();
                SqlCommand columnCommand = new SqlCommand(columnCommandStr, tableConn);
                SqlDataAdapter columnAd = new SqlDataAdapter(columnCommand);
                DataSet columnDs = new DataSet();
                columnAd.Fill(columnDs);
                DataTable columnColumns = columnDs.Tables[0];
                tableConn.Close();

                Table table = new Table(tableNameSet.AliasName, tableNameSet.TableName);
                if (String.IsNullOrEmpty(tableComment)) {
                    tableComment = tableNameSet.TableName;
                }
                table.CommentText = tableComment;
                foreach (DataRow item in columnColumns.Rows) {
                    Column column = CreateColumn(table, item);
                    if (column != null) {
                        table.SetColumn(column);
                    }
                }
                tables.Add(table);
            }
            return tables;
        }

        Column CreateColumn(Table table, DataRow dataRow)
        {
            Column column = new Column(table);
            column.AllowNull = Convert.ToBoolean(dataRow["AllowNull"]);
            column.ColumnName = dataRow["ColumnName"].ToString();
            column.ColumnComment = dataRow["ColumnComment"].ToString();
            column.IsPrimaryKey = Convert.ToBoolean(dataRow["ColumnKey"]);
            column.IsIdentity = Convert.ToBoolean(dataRow["IsIdentity"]);

            if (DbSetting.CheckIgnoreField(column.TableName + "." + column.ColumnName) || DbSetting.CheckIgnoreField("*." + column.ColumnName)) {
                return null;
            }

            if (DbSetting.GetAliasField(column.TableName + "." + column.ColumnName, out string fieldName) || DbSetting.GetAliasField("*." + column.ColumnName, out fieldName)) {
                column.FieldName = fieldName;
            }
            else {
                column.FieldName = column.ColumnName;
            }

            string dataType = dataRow["DataType"].ToString();
            column.RawType = dataType;
            if (dataType.Equals("bit", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "bool";
            }
            else if (dataType.Equals("datetime", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("datetime2", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("smalldatetime", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("time", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("timestamp", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("date", StringComparison.InvariantCultureIgnoreCase)
                ) {
                column.DataType = "DateTime";
            }
            else if (dataType.Equals("datetimeoffset", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "DateTimeOffset";
            }
            else if (dataType.Equals("decimal", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("money", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("smallmoney", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("numeric", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "decimal";
            }
            else if (dataType.Equals("float", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "double";
            }
            else if (dataType.Equals("real", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "float";
            }
            else if (dataType.Equals("char", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("nchar", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("varchar", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("nvarchar", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("text", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("ntext", StringComparison.InvariantCultureIgnoreCase)
                ) {
                column.DataType = "string";
            }
            else if (dataType.Equals("smallint", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "short";
            }
            else if (dataType.Equals("tinyint", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "byte";
            }
            else if (dataType.Equals("int", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "int";
            }
            else if (dataType.Equals("bigint", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "long";
            }
            else if (dataType.Equals("binary", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("varbinary", StringComparison.InvariantCultureIgnoreCase)
            ) {
                column.DataType = "byte[]";
            }
            else {
                column.DataType = dataType;
            }
            string tmpType = column.DataType;
            if (string.IsNullOrEmpty(column.ColumnComment)) {
                column.ColumnComment = column.ColumnName;
            }

            if (DbSetting.GetSpecifiedType(column.TableName + "." + column.ColumnName, out string specifiedName) || DbSetting.GetSpecifiedType("*." + column.ColumnName, out specifiedName)) {
                column.DataType = specifiedName;
            }

            if (column.AllowNull && !string.Equals(tmpType, "string", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(tmpType, "byte[]", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = string.Format("{0}?", column.DataType);
            }

            if (int.TryParse(dataRow["MaxLength"].ToString(), out int maxLength)) {
                column.MaxLength = maxLength;
            }

            if (column.AllowNull) {
                if (DbSetting.CheckNotNullField(column.TableName + "." + column.ColumnName) || DbSetting.CheckNotNullField(column.TableName + ".*") || DbSetting.CheckNotNullField("*." + column.ColumnName)) {
                    column.AllowNull = false;
                }
            }

            if (DbSetting.GetDefaultValue(column.TableName + "." + column.ColumnName, out string defaultValue) || DbSetting.GetDefaultValue("*." + column.ColumnName, out defaultValue)) {
                if (DbSetting.GetDefaultValueStringMode()) {
                    column.DefaultValue = "\"" + defaultValue + "\"";
                }
                else if (column.DataType == "string") {
                    column.DefaultValue = "\"" + defaultValue + "\"";
                }
                else if (column.DataType == "DateTime" || column.DataType == "DateTime?") {
                    if (defaultValue.StartsWith("DefaultTime.")) {
                        column.DefaultValue = defaultValue;
                    }
                    else {
                        column.DefaultValue = "\"" + defaultValue + "\"";
                    }
                }
                else {
                    column.DefaultValue = defaultValue;
                }
            }

            if (DbSetting.GetControl(column.TableName + "." + column.ColumnName, out string control) || DbSetting.GetControl("*." + column.ColumnName, out control)) {
                column.Control = "FunctionControl." + control;
                if (control == "Create" || control == "Read") {
                    column.NoUpdate = true;
                }
            }
            return column;
        }
    }
}

