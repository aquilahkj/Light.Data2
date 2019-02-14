using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Light.Data
{
    class DataParameterMapping
    {
        private GetValueHandler mGetValue;
        private PropertyInfo mProperty;
        private SetValueHandler mSetValue;
        private string mName;
        private Type mType;
        private DataParameterMode mDirection;
        private bool mConvertString;

        public DataParameterMode Direction {
            get {
                return mDirection;
            }
        }

        public Type ParameterType {
            get {
                return mType;
            }
        }

        public bool ConvertString {
            get {
                return mConvertString;
            }
        }

        public GetValueHandler Get {
            get {
                return this.mGetValue;
            }
        }

        public SetValueHandler Set {
            get {
                return this.mSetValue;
            }
        }

        public PropertyInfo Property {
            get {
                return this.mProperty;
            }
        }

        public string Name {
            get {
                return this.mName;
            }
        }

        public DataParameterMapping(PropertyInfo property, string name, DataParameterMode direction)
        {
            if (property.CanRead) {
                this.mGetValue = ReflectionHandlerFactory.PropertyGetHandler(property);
            }
            if (property.CanWrite) {
                this.mSetValue = ReflectionHandlerFactory.PropertySetHandler(property);
            }
            this.mType = property.PropertyType;
            TypeCode code = Type.GetTypeCode(this.mType);
            this.mConvertString = code == TypeCode.Object || code == TypeCode.DBNull || code == TypeCode.Empty;
            if ((this.mDirection == DataParameterMode.InputOutput || this.mDirection == DataParameterMode.Output) && mConvertString) {
                throw new LightDataException(SR.OutputParameterNotSupportObjectType);
            }
            this.mProperty = property;
            this.mName = string.IsNullOrEmpty(name) ? property.Name : name;
            this.mDirection = direction;
        }
    }
}
