using System;
using System.Data;

namespace Light.Data
{
    /// <summary>
    /// Data parameter.
    /// </summary>
    public class DataParameter
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="DataParameter"/> class.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="paramValue">Parameter value.</param>
		/// <param name="dbType">Db type.</param>
		/// <param name="direction">Direction.</param>
        /// <param name="dataType">Data type.</param>
		internal DataParameter(string paramName, object paramValue, string dbType, ParameterDirection direction, Type dataType)
        {
            if (string.IsNullOrEmpty(paramName)) {
                throw new ArgumentNullException(nameof(paramName));
            }

            ParameterName = paramName;
            DbType = dbType;
            Value = paramValue;
            Direction = direction;
            DataType = dataType;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DataParameter"/> class.
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        /// <param name="direction">Direction.</param>
        public DataParameter(string paramName, object paramValue, string dbType, ParameterDirection direction)
            : this(paramName, paramValue, dbType, direction, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParameter"/> class.
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value.</param>
        public DataParameter(string paramName, object paramValue)
            : this(paramName, paramValue, null, ParameterDirection.Input, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParameter"/> class.
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="direction">Direction.</param>
        public DataParameter(string paramName, object paramValue, ParameterDirection direction)
            : this(paramName, paramValue, null, direction, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParameter"/> class.
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        public DataParameter(string paramName, object paramValue, string dbType)
            : this(paramName, paramValue, dbType, ParameterDirection.Input, null)
        {

        }

        private IDataParameter _dataParameter;

        internal IDataParameter ConvertDbParameter(DatabaseProvider database, CommandType commandType)
        {
            var dataParameter = database.CreateParameter(ParameterName, Value, DbType, Direction, DataType, commandType);
            _dataParameter = dataParameter;
            return dataParameter;
        }

        internal virtual bool Callback()
        {
            if (_dataParameter == null) {
                return false;
            }
            if (_dataParameter.Direction != ParameterDirection.Input) {
                Value = _dataParameter.Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName
        {
            get;
            //internal set {
            //    _parameterName = value;
            //}
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get;
            private set;
            //internal set {
            //    _value = value;
            //}
        }

        /// <summary>
        /// Gets or sets the DBType.
        /// </summary>
        /// <value>The type of the db.</value>
        public string DbType
        {
            get;
            //internal set {
            //    _dbType = value;
            //}
        }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public ParameterDirection Direction
        {
            get;
            //internal set {
            //    _direction = value;
            //}
        }

        /// <summary>
        /// Gets the data type
        /// </summary>
        public Type DataType
        {
            get;
            //internal set {
            //    _dataType = value;
            //}
        }
    }
}
