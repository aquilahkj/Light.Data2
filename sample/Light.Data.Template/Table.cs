using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Data.Template
{
    public class Table
    {
        public Table(string tableName, string rawName)
        {
            this._tableName = tableName;
            this._rawName = rawName;
            this._columns = new List<Column>();
        }

        string _commentText = null;
        public string CommentText {
            set {
                this._commentText = value;
            }
            get {
                return this._commentText;
            }
        }
        string _rawName = null;
        public string RawName {
            get {
                return this._rawName;
            }
        }

        string _tableName = null;
        public string TableName {
            get {
                return this._tableName;
            }
        }

        List<Column> _columns = null;

        public void SetColumn(Column column)
        {
            this._columns.Add(column);
        }

        public Column[] GetColumns()
        {
            return this._columns.ToArray();
        }

        //Dictionary<string, Column> _columns = null;
        //public Dictionary<string, Column> Columns {
        //    get {
        //        return this._columns;
        //    }
        //}

        //public void SetColumn(DataRow dataRow)
        //{
        //    string columnName = dataRow["ColumnName"].ToString();
        //    Column column = null;
        //    if (!this._columns.TryGetValue(columnName, out column)) {
        //        column = new Column(this, dataRow);
        //    }
        //    this._columns[columnName] = column;
        //}

        //public bool GetColumn(string columnName, out Column column)
        //{
        //    column = null;
        //    Dictionary<string, Column> dict = this._columns;
        //    if (dict != null) {
        //        return dict.TryGetValue(columnName, out column);
        //    }
        //    return false;
        //}
    }
}
