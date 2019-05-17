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
		internal DataParameter(string paramName, object paramValue, string dbType, DataParameterMode direction, Type dataType)
        {
            if (string.IsNullOrEmpty(paramName)) {
                throw new ArgumentNullException(nameof(paramName));
            }

            _parameterName = paramName;
            _dbType = dbType;
            _value = paramValue;
            _direction = direction;
            _dataType = dataType;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DataParameter"/> class.
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        /// <param name="direction">Direction.</param>
        public DataParameter(string paramName, object paramValue, string dbType, DataParameterMode direction)
            : this(paramName, paramValue, dbType, direction, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParameter"/> class.
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value.</param>
        public DataParameter(string paramName, object paramValue)
            : this(paramName, paramValue, null, DataParameterMode.Input, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParameter"/> class.
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="direction">Direction.</param>
        public DataParameter(string paramName, object paramValue, DataParameterMode direction)
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
            : this(paramName, paramValue, dbType, DataParameterMode.Input, null)
        {

        }

        IDataParameter _dataParameter;

        /// <summary>
        /// Sets the data parameter.
        /// </summary>
        /// <param name="dataParameter">Data parameter.</param>
        internal virtual void SetDataParameter(IDataParameter dataParameter)
        {
            _dataParameter = dataParameter;
        }

        internal virtual bool CallbackOutputValue()
        {
            if ((_direction & DataParameterMode.Output) == DataParameterMode.Output) {
                this._value = _dataParameter.Value;
                return true;
            }
            return false;
        }

        string _parameterName;

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName {
            get {

                return _parameterName;
            }
            //internal set {
            //    _parameterName = value;
            //}
        }

        object _value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value {
            get {
                return _value;
            }
            //internal set {
            //    _value = value;
            //}
        }

        string _dbType;

        /// <summary>
        /// Gets or sets the DBType.
        /// </summary>
        /// <value>The type of the db.</value>
        public string DbType {
            get {
                return _dbType;
            }
            //internal set {
            //    _dbType = value;
            //}
        }

        DataParameterMode _direction;

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public DataParameterMode Direction {
            get {
                return _direction;
            }
            //internal set {
            //    _direction = value;
            //}
        }

        ///// <summary>
        ///// Gets the output value.
        ///// </summary>
        ///// <value>The output value.</value>
        //public object OutputValue {
        //    get {
        //        if (_dataParameter == null)
        //            return null;
        //        if (_direction == DataParameterMode.InputOutput || _direction == DataParameterMode.Output)
        //            return _dataParameter.Value;
        //        else
        //            return null;
        //    }
        //}

        Type _dataType;
        /// <summary>
        /// Gets the data type
        /// </summary>
        public Type DataType {
            get {
                return _dataType;
            }
            //internal set {
            //    _dataType = value;
            //}
        }
    }
}
