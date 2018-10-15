using System;
using System.Data;

namespace Light.Data.Template
{
    public class Column
    {
        public Column(Table table)
        {
            this._table = table;
            this._tableName = table.TableName;
        }

        Table _table = null;
        public Table Table {
            get {
                return this._table;
            }
        }

        string _tableName = null;
        public string TableName {
            get {
                return this._tableName;
            }
        }

        string _columnName = null;
        public string ColumnName {
            get {
                return this._columnName;
            }
            set {
                this._columnName = value;
            }
        }

        string _fieldName = null;
        public string FieldName {
            get {
                return this._fieldName;
            }
            set {
                this._fieldName = value;
            }
        }

        string _columnComment = null;
        public string ColumnComment {
            get {
                return this._columnComment;
            }
            set {
                this._columnComment = value;
            }
        }

        public string _defaultValue = null;
        public string DefaultValue {
            get {
                return this._defaultValue;
            }
            set {
                this._defaultValue = value;
            }
        }

        public string _control = null;
        public string Control {
            get {
                return this._control;
            }
            set {
                this._control = value;
            }
        }

        bool _allowNull = false;
        public bool AllowNull {
            get {
                return this._allowNull;
            }
            set {
                this._allowNull = value;
            }
        }

        string _rawType = null;
        public string RawType {
            get {
                return this._rawType;
            }
            set {
                this._rawType = value;
            }
        }

        string _dataType = null;
        public string DataType {
            get {
                return this._dataType;
            }
            set {
                this._dataType = value;
            }
        }

        string _dbType = null;
        public string DBType {
            get {
                return this._dbType;
            }
            set {
                this._dbType = value;
            }
        }

        int? _maxLength = null;
        public int? MaxLength {
            get {
                return this._maxLength;
            }
            set {
                this._maxLength = value;
            }
        }

        bool _isPrimaryKey = false;
        public bool IsPrimaryKey {
            get {
                return this._isPrimaryKey;
            }
            set {
                this._isPrimaryKey = value;
            }
        }

        bool _isIdentity = false;
        public bool IsIdentity {
            get {
                return this._isIdentity;
            }
            set {
                this._isIdentity = value;
            }
        }

        bool _noUpdate = false;
        public bool NoUpdate {
            get {
                return this._noUpdate;
            }
            set {
                this._noUpdate = value;
            }
        }

        //string _upColumnName = null;
        //public string UpColumnName {
        //    get {
        //        return this._upColumnName;
        //    }
        //    set {
        //        this._upColumnName = value;
        //    }
        //}

        //string _lowerColumnName = null;
        //public string LowerColumnName {
        //    get {
        //        return this._lowerColumnName;
        //    }
        //    set {
        //        this._lowerColumnName = value;
        //    }
        //}
    }
}