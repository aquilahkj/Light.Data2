
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Data.Template
{
    public class PostgreSchema
    {
        string _dataBaseName;

        Dictionary<string, Table> _dict = null;

        string _connectionString;

        public PostgreSchema(string dataBaseName, string connectionString)
        {
            this._dataBaseName = dataBaseName;
            this._connectionString = connectionString;
            this._dict = new Dictionary<string, Table>();
        }

        public List<Table> GetTables()
        {
            const string tableCommandText = @"select relname as ""TableName"",relfilenode as ""TableCode"",col_description(relfilenode,0) as ""CommentText"" from pg_class where relname = '{0}'";

            const string columnCommandText = @"select 
a.ordinal_position as ""ColumnId"",
(case when b.constraint_type='PRIMARY KEY' then 1 else 0 end)::BOOLEAN as ""ColumnKey"",
a.character_maximum_length as ""MaxLength"",
a.column_name as ""ColumnName"",
a.udt_name as ""DataType"",
(case when a.column_default like 'nextval%' then 1 else 0 end)::BOOLEAN as ""IsIdentity"",
(case when a.is_nullable='YES' then 1 else 0 end)::BOOLEAN as ""AllowNull"",
a.numeric_precision  as ""Precision"",
a.numeric_scale  as ""Scale"",
a.column_comment as ""ColumnComment""
from
(
select table_name,
ordinal_position,
column_name,
column_default,
is_nullable,
udt_name,
character_maximum_length,
numeric_precision,
numeric_scale,
col_description({2},ordinal_position) as column_comment
from information_schema.columns
where table_name='{1}' and table_catalog='{0}'
) as a
left join 
(
SELECT kcu.table_name,kcu.column_name,tc.constraint_type
FROM information_schema.key_column_usage kcu
JOIN information_schema.table_constraints tc
ON kcu.constraint_name=tc.constraint_name
where kcu.table_name='{1}' and kcu.table_catalog='{0}'
and tc.table_name='{1}' and tc.table_catalog='{0}'
) as b
on a.table_name=b.table_name and a.column_name=b.column_name
";

            List<Table> tables = new List<Table>();
            foreach (TableNameSet tableNameSet in DbSetting.GetTables()) {
                string tableCommandStr = String.Format(tableCommandText, tableNameSet.TableName);
                NpgsqlConnection tableConn = new NpgsqlConnection(_connectionString);
                tableConn.Open();
                NpgsqlCommand tableCommand = new NpgsqlCommand(tableCommandStr, tableConn);
                NpgsqlDataAdapter tableAd = new NpgsqlDataAdapter(tableCommand);
                DataSet tableDs = new DataSet();
                tableAd.Fill(tableDs);
                DataTable tableColumns = tableDs.Tables[0];
                tableConn.Close();
                if (tableColumns.Rows.Count == 0) {
                    continue;
                }
                string tableComment = Convert.ToString(tableColumns.Rows[0]["CommentText"]);
                string tableCode = Convert.ToString(tableColumns.Rows[0]["TableCode"]);
                string columnCommandStr = String.Format(columnCommandText, this._dataBaseName, tableNameSet.TableName, tableCode);
                NpgsqlConnection columnConn = new NpgsqlConnection(_connectionString);
                columnConn.Open();
                NpgsqlCommand columnCommand = new NpgsqlCommand(columnCommandStr, tableConn);
                NpgsqlDataAdapter columnAd = new NpgsqlDataAdapter(columnCommand);
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

            if (dataType.Equals("bool", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "bool";
            }
            else if (dataType.Equals("timestamp", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("date", StringComparison.InvariantCultureIgnoreCase)
                ) {
                column.DataType = "DateTime";
            }
            else if (dataType.Equals("money", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("numeric", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "decimal";
            }
            else if (dataType.Equals("float8", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "double";
            }
            else if (dataType.Equals("float4", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "float";
            }
            else if (dataType.Equals("char", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("varchar", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("text", StringComparison.InvariantCultureIgnoreCase)
                ) {
                column.DataType = "string";
            }
            else if (dataType.Equals("int2", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "short";
            }
            else if (dataType.Equals("int4", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "int";
            }
            else if (dataType.Equals("int8", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "long";
            }
            else if (dataType.Equals("bytea", StringComparison.InvariantCultureIgnoreCase)) {
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

