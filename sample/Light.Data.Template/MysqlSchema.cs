using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Data.Template
{
    public class MysqlSchema
    {
        string _dataBaseName;

        Dictionary<string, Table> _dict = null;

        string _connectionString;

        public MysqlSchema(string dataBaseName, string connectionString)
        {
            this._dataBaseName = dataBaseName;
            this._connectionString = connectionString;
            this._dict = new Dictionary<string, Table>();
        }

        public List<Table> GetTables()
        {
            const string tableCommandText = @"select TABLE_NAME as TableName,TABLE_COMMENT as CommentText from information_schema.TABLES where TABLE_SCHEMA='{0}' and TABLE_NAME='{1}'";

            const string columnCommandText = @"
select 
ORDINAL_POSITION as 'ColumnId',
case when COLUMN_KEY='PRI' then 1 else 0 end as 'ColumnKey',
CHARACTER_MAXIMUM_LENGTH AS 'MaxLength',
COLUMN_NAME AS 'ColumnName',
DATA_TYPE AS 'DataType',
case when EXTRA='auto_increment' then 1 else 0 end as 'IsIdentity',
case when IS_NULLABLE='YES' then 1 else 0 end as 'AllowNull',
NUMERIC_PRECISION as 'Precision',
NUMERIC_SCALE as 'Scale',
COLUMN_COMMENT as 'ColumnComment',
COLUMN_TYPE AS 'ColumnType'
from information_schema.COLUMNS where TABLE_SCHEMA='{0}' and TABLE_NAME='{1}'
order by ORDINAL_POSITION";

            List<Table> tables = new List<Table>();
            foreach (TableNameSet tableNameSet in DbSetting.GetTables()) {
                string tableCommandStr = String.Format(tableCommandText, this._dataBaseName, tableNameSet.TableName);
                MySqlConnection tableConn = new MySqlConnection(_connectionString);
                tableConn.Open();
                MySqlCommand tableCommand = new MySqlCommand(tableCommandStr, tableConn);
                MySqlDataAdapter tableAd = new MySqlDataAdapter(tableCommand);
                DataSet tableDs = new DataSet();
                tableAd.Fill(tableDs);
                DataTable tableColumns = tableDs.Tables[0];
                tableConn.Close();
                if (tableColumns.Rows.Count == 0) {
                    continue;
                }
                string tableComment = Convert.ToString(tableColumns.Rows[0]["CommentText"]);
                string columnCommandStr = String.Format(columnCommandText, this._dataBaseName, tableNameSet.TableName);
                MySqlConnection columnConn = new MySqlConnection(_connectionString);
                columnConn.Open();
                MySqlCommand columnCommand = new MySqlCommand(columnCommandStr, tableConn);
                MySqlDataAdapter columnAd = new MySqlDataAdapter(columnCommand);
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

            string columnType = dataRow["ColumnType"].ToString();
            string dataType = dataRow["DataType"].ToString();
            column.RawType = dataType;
            if (dataType.Equals("bit", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "bool";
            }
            else if (dataType.Equals("datetime", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("timestamp", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("date", StringComparison.InvariantCultureIgnoreCase)
                ) {
                column.DataType = "DateTime";
            }
            else if (dataType.Equals("decimal", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("money", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "decimal";
            }
            else if (dataType.Equals("double", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "double";
            }
            else if (dataType.Equals("float", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "float";
            }
            else if (dataType.Equals("char", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("varchar", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("text", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("tinytext", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("mediumtext", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("longtext", StringComparison.InvariantCultureIgnoreCase)
                ) {
                column.DataType = "string";
            }
            else if (dataType.Equals("smallint", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "short";
            }
            else if (dataType.Equals("tinyint", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "sbyte";
            }
            else if (dataType.Equals("mediumint", StringComparison.InvariantCultureIgnoreCase)
                || dataType.Equals("int", StringComparison.InvariantCultureIgnoreCase)
                ) {
                column.DataType = "int";
            }
            else if (dataType.Equals("bigint", StringComparison.InvariantCultureIgnoreCase)) {
                column.DataType = "long";
            }
            else {
                column.DataType = "byte[]";
            }
            string tmpType = column.DataType;
            if (string.IsNullOrEmpty(column.ColumnComment)) {
                column.ColumnComment = column.ColumnName;
            }

            if (DbSetting.GetSpecifiedType(column.TableName + "." + column.ColumnName, out string specifiedName) || DbSetting.GetSpecifiedType("*." + column.ColumnName, out specifiedName)) {
                column.DataType = specifiedName;
            }
            else if (columnType.Contains("unsigned")) {
                if (column.DataType == "int" || column.DataType == "short" || column.DataType == "long") {
                    column.DataType = string.Format("u{0}", column.DataType);
                }
                else if (column.DataType == "sbyte") {
                    column.DataType = "byte";
                }
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

