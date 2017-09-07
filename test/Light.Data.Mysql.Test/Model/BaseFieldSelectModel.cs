using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Mysql.Test
{
    class BaseFieldSelectModel
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private bool boolField;

        /// <summary>
        /// BoolField
        /// </summary>
        /// <value></value>
        public bool BoolField {
            get {
                return this.boolField;
            }
            set {
                this.boolField = value;
            }
        }

        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        public sbyte SbyteField {
            get {
                return this.sbyteField;
            }
            set {
                this.sbyteField = value;
            }
        }

        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        public byte ByteField {
            get {
                return this.byteField;
            }
            set {
                this.byteField = value;
            }
        }

        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        public short Int16Field {
            get {
                return this.int16Field;
            }
            set {
                this.int16Field = value;
            }
        }

        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        public ushort UInt16Field {
            get {
                return this.uInt16Field;
            }
            set {
                this.uInt16Field = value;
            }
        }

        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        public int Int32Field {
            get {
                return this.int32Field;
            }
            set {
                this.int32Field = value;
            }
        }

        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        public uint UInt32Field {
            get {
                return this.uInt32Field;
            }
            set {
                this.uInt32Field = value;
            }
        }

        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        public long Int64Field {
            get {
                return this.int64Field;
            }
            set {
                this.int64Field = value;
            }
        }

        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        public ulong UInt64Field {
            get {
                return this.uInt64Field;
            }
            set {
                this.uInt64Field = value;
            }
        }

        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        public float FloatField {
            get {
                return this.floatField;
            }
            set {
                this.floatField = value;
            }
        }

        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        public double DoubleField {
            get {
                return this.doubleField;
            }
            set {
                this.doubleField = value;
            }
        }

        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        public decimal DecimalField {
            get {
                return this.decimalField;
            }
            set {
                this.decimalField = value;
            }
        }

        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        public DateTime DateTimeField {
            get {
                return this.dateTimeField;
            }
            set {
                this.dateTimeField = value;
            }
        }

        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        public string VarcharField {
            get {
                return this.varcharField;
            }
            set {
                this.varcharField = value;
            }
        }

        private string textField;

        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        public string TextField {
            get {
                return this.textField;
            }
            set {
                this.textField = value;
            }
        }

        private byte[] bigDataField;

        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        public byte[] BigDataField {
            get {
                return this.bigDataField;
            }
            set {
                this.bigDataField = value;
            }
        }

        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        public EnumInt32Type EnumInt32Field {
            get {
                return this.enumInt32Field;
            }
            set {
                this.enumInt32Field = value;
            }
        }

        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        public EnumInt64Type EnumInt64Field {
            get {
                return this.enumInt64Field;
            }
            set {
                this.enumInt64Field = value;
            }
        }
        #endregion
    }
}
