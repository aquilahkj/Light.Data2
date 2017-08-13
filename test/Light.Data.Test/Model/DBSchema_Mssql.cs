
using System;
using System.Collections.Generic;
using System.Text;
using Light.Data;

namespace Light.Data.Test
{
	/// <summary>
    /// Te_BaseField
    /// </summary>
    [DataTable("Te_BaseField")]
    public class TeBaseField 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
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
        [DataField("BoolField")]
        public bool BoolField
        {
            get { 
                return this.boolField; 
            }
            set { 
                this.boolField = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public sbyte SbyteField
        {
            get { 
                return this.sbyteField; 
            }
            set { 
                this.sbyteField = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField
        {
            get { 
                return this.byteField; 
            }
            set { 
                this.byteField = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field
        {
            get { 
                return this.int16Field; 
            }
            set { 
                this.int16Field = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public ushort UInt16Field
        {
            get { 
                return this.uInt16Field; 
            }
            set { 
                this.uInt16Field = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public uint UInt32Field
        {
            get { 
                return this.uInt32Field; 
            }
            set { 
                this.uInt32Field = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field
        {
            get { 
                return this.int64Field; 
            }
            set { 
                this.int64Field = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public ulong UInt64Field
        {
            get { 
                return this.uInt64Field; 
            }
            set { 
                this.uInt64Field = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField
        {
            get { 
                return this.floatField; 
            }
            set { 
                this.floatField = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textField;

        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        [DataField("TextField")]
        public string TextField
        {
            get { 
                return this.textField; 
            }
            set { 
                this.textField = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true)]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private byte[] bigDataField;

        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        [DataField("BigDataField")]
        public byte[] BigDataField
        {
            get { 
                return this.bigDataField; 
            }
            set { 
                this.bigDataField = value; 
            }
        }
        private byte[] bigDataFieldNull;

        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull", IsNullable = true)]
        public byte[] BigDataFieldNull
        {
            get { 
                return this.bigDataFieldNull; 
            }
            set { 
                this.bigDataFieldNull = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public EnumInt64Type EnumInt64Field
        {
            get { 
                return this.enumInt64Field; 
            }
            set { 
                this.enumInt64Field = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseFieldNoIdentity
    /// </summary>
    [DataTable("Te_BaseFieldNoIdentity")]
    public class TeBaseFieldNoIdentity 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseFieldExpression
    /// </summary>
    [DataTable("Te_BaseFieldExpression")]
    public class TeBaseFieldExpression 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
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
        [DataField("BoolField")]
        public bool BoolField
        {
            get { 
                return this.boolField; 
            }
            set { 
                this.boolField = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public sbyte SbyteField
        {
            get { 
                return this.sbyteField; 
            }
            set { 
                this.sbyteField = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField
        {
            get { 
                return this.byteField; 
            }
            set { 
                this.byteField = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field
        {
            get { 
                return this.int16Field; 
            }
            set { 
                this.int16Field = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public ushort UInt16Field
        {
            get { 
                return this.uInt16Field; 
            }
            set { 
                this.uInt16Field = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public uint UInt32Field
        {
            get { 
                return this.uInt32Field; 
            }
            set { 
                this.uInt32Field = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field
        {
            get { 
                return this.int64Field; 
            }
            set { 
                this.int64Field = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public ulong UInt64Field
        {
            get { 
                return this.uInt64Field; 
            }
            set { 
                this.uInt64Field = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField
        {
            get { 
                return this.floatField; 
            }
            set { 
                this.floatField = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public EnumInt64Type EnumInt64Field
        {
            get { 
                return this.enumInt64Field; 
            }
            set { 
                this.enumInt64Field = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseFieldExpression_Extend
    /// </summary>
    [DataTable("Te_BaseFieldExpression_Extend")]
    public class TeBaseFieldExpressionExtend 
    {
        #region "Data Property"
        private int extendId;

        /// <summary>
        /// ExtendId
        /// </summary>
        /// <value></value>
        [DataField("ExtendId", IsIdentity = true, IsPrimaryKey = true)]
        public int ExtendId
        {
            get { 
                return this.extendId; 
            }
            set { 
                this.extendId = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseFieldSelectField
    /// </summary>
    [DataTable("Te_BaseFieldSelectField")]
    public class TeBaseFieldSelectField 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
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
        [DataField("BoolField")]
        public bool BoolField
        {
            get { 
                return this.boolField; 
            }
            set { 
                this.boolField = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public sbyte SbyteField
        {
            get { 
                return this.sbyteField; 
            }
            set { 
                this.sbyteField = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField
        {
            get { 
                return this.byteField; 
            }
            set { 
                this.byteField = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field
        {
            get { 
                return this.int16Field; 
            }
            set { 
                this.int16Field = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public ushort UInt16Field
        {
            get { 
                return this.uInt16Field; 
            }
            set { 
                this.uInt16Field = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public uint UInt32Field
        {
            get { 
                return this.uInt32Field; 
            }
            set { 
                this.uInt32Field = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field
        {
            get { 
                return this.int64Field; 
            }
            set { 
                this.int64Field = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public ulong UInt64Field
        {
            get { 
                return this.uInt64Field; 
            }
            set { 
                this.uInt64Field = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField
        {
            get { 
                return this.floatField; 
            }
            set { 
                this.floatField = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textField;

        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        [DataField("TextField")]
        public string TextField
        {
            get { 
                return this.textField; 
            }
            set { 
                this.textField = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true)]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private byte[] bigDataField;

        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        [DataField("BigDataField")]
        public byte[] BigDataField
        {
            get { 
                return this.bigDataField; 
            }
            set { 
                this.bigDataField = value; 
            }
        }
        private byte[] bigDataFieldNull;

        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull", IsNullable = true)]
        public byte[] BigDataFieldNull
        {
            get { 
                return this.bigDataFieldNull; 
            }
            set { 
                this.bigDataFieldNull = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public EnumInt64Type EnumInt64Field
        {
            get { 
                return this.enumInt64Field; 
            }
            set { 
                this.enumInt64Field = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseFieldAggregateField
    /// </summary>
    [DataTable("Te_BaseFieldAggregateField")]
    public class TeBaseFieldAggregateField 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
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
        [DataField("BoolField")]
        public bool BoolField
        {
            get { 
                return this.boolField; 
            }
            set { 
                this.boolField = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public sbyte SbyteField
        {
            get { 
                return this.sbyteField; 
            }
            set { 
                this.sbyteField = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField
        {
            get { 
                return this.byteField; 
            }
            set { 
                this.byteField = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field
        {
            get { 
                return this.int16Field; 
            }
            set { 
                this.int16Field = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public ushort UInt16Field
        {
            get { 
                return this.uInt16Field; 
            }
            set { 
                this.uInt16Field = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public uint UInt32Field
        {
            get { 
                return this.uInt32Field; 
            }
            set { 
                this.uInt32Field = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field
        {
            get { 
                return this.int64Field; 
            }
            set { 
                this.int64Field = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public ulong UInt64Field
        {
            get { 
                return this.uInt64Field; 
            }
            set { 
                this.uInt64Field = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField
        {
            get { 
                return this.floatField; 
            }
            set { 
                this.floatField = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textField;

        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        [DataField("TextField")]
        public string TextField
        {
            get { 
                return this.textField; 
            }
            set { 
                this.textField = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true)]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private byte[] bigDataField;

        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        [DataField("BigDataField")]
        public byte[] BigDataField
        {
            get { 
                return this.bigDataField; 
            }
            set { 
                this.bigDataField = value; 
            }
        }
        private byte[] bigDataFieldNull;

        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull", IsNullable = true)]
        public byte[] BigDataFieldNull
        {
            get { 
                return this.bigDataFieldNull; 
            }
            set { 
                this.bigDataFieldNull = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public EnumInt64Type EnumInt64Field
        {
            get { 
                return this.enumInt64Field; 
            }
            set { 
                this.enumInt64Field = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseField_SelectInsert
    /// </summary>
    [DataTable("Te_BaseField_SelectInsert")]
    public class TeBaseFieldSelectInsert 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
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
        [DataField("BoolField")]
        public bool BoolField
        {
            get { 
                return this.boolField; 
            }
            set { 
                this.boolField = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public sbyte SbyteField
        {
            get { 
                return this.sbyteField; 
            }
            set { 
                this.sbyteField = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField
        {
            get { 
                return this.byteField; 
            }
            set { 
                this.byteField = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field
        {
            get { 
                return this.int16Field; 
            }
            set { 
                this.int16Field = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public ushort UInt16Field
        {
            get { 
                return this.uInt16Field; 
            }
            set { 
                this.uInt16Field = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public uint UInt32Field
        {
            get { 
                return this.uInt32Field; 
            }
            set { 
                this.uInt32Field = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field
        {
            get { 
                return this.int64Field; 
            }
            set { 
                this.int64Field = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public ulong UInt64Field
        {
            get { 
                return this.uInt64Field; 
            }
            set { 
                this.uInt64Field = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField
        {
            get { 
                return this.floatField; 
            }
            set { 
                this.floatField = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textField;

        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        [DataField("TextField")]
        public string TextField
        {
            get { 
                return this.textField; 
            }
            set { 
                this.textField = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true)]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private byte[] bigDataField;

        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        [DataField("BigDataField")]
        public byte[] BigDataField
        {
            get { 
                return this.bigDataField; 
            }
            set { 
                this.bigDataField = value; 
            }
        }
        private byte[] bigDataFieldNull;

        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull", IsNullable = true)]
        public byte[] BigDataFieldNull
        {
            get { 
                return this.bigDataFieldNull; 
            }
            set { 
                this.bigDataFieldNull = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public EnumInt64Type EnumInt64Field
        {
            get { 
                return this.enumInt64Field; 
            }
            set { 
                this.enumInt64Field = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseField_SelectInsert_NoIdentity
    /// </summary>
    [DataTable("Te_BaseField_SelectInsert_NoIdentity")]
    public class TeBaseFieldSelectInsertNoIdentity 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsPrimaryKey = true)]
        public int Id
        {
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
        [DataField("BoolField")]
        public bool BoolField
        {
            get { 
                return this.boolField; 
            }
            set { 
                this.boolField = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public sbyte SbyteField
        {
            get { 
                return this.sbyteField; 
            }
            set { 
                this.sbyteField = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField
        {
            get { 
                return this.byteField; 
            }
            set { 
                this.byteField = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field
        {
            get { 
                return this.int16Field; 
            }
            set { 
                this.int16Field = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public ushort UInt16Field
        {
            get { 
                return this.uInt16Field; 
            }
            set { 
                this.uInt16Field = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public uint UInt32Field
        {
            get { 
                return this.uInt32Field; 
            }
            set { 
                this.uInt32Field = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field
        {
            get { 
                return this.int64Field; 
            }
            set { 
                this.int64Field = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public ulong UInt64Field
        {
            get { 
                return this.uInt64Field; 
            }
            set { 
                this.uInt64Field = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField
        {
            get { 
                return this.floatField; 
            }
            set { 
                this.floatField = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textField;

        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        [DataField("TextField")]
        public string TextField
        {
            get { 
                return this.textField; 
            }
            set { 
                this.textField = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true)]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private byte[] bigDataField;

        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        [DataField("BigDataField")]
        public byte[] BigDataField
        {
            get { 
                return this.bigDataField; 
            }
            set { 
                this.bigDataField = value; 
            }
        }
        private byte[] bigDataFieldNull;

        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull", IsNullable = true)]
        public byte[] BigDataFieldNull
        {
            get { 
                return this.bigDataFieldNull; 
            }
            set { 
                this.bigDataFieldNull = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public EnumInt64Type EnumInt64Field
        {
            get { 
                return this.enumInt64Field; 
            }
            set { 
                this.enumInt64Field = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseField_NullMiniValue
    /// </summary>
    [DataTable("Te_BaseField_NullMiniValue")]
    public class TeBaseFieldNullMiniValue 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull")]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull")]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull")]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull")]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull")]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull")]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull")]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull")]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull")]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull")]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull")]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull")]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", DefaultValue = "2017-01-02 12:00:00")]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull")]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull")]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private byte[] bigDataFieldNull;

        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull")]
        public byte[] BigDataFieldNull
        {
            get { 
                return this.bigDataFieldNull; 
            }
            set { 
                this.bigDataFieldNull = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull")]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull")]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseField_DefaultValue
    /// </summary>
    [DataTable("Te_BaseField_DefaultValue")]
    public class TeBaseFieldDefaultValue 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true, DefaultValue = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true, DefaultValue = 20)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true, DefaultValue = 20)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true, DefaultValue = 20)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true, DefaultValue = 20)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true, DefaultValue = 20)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true, DefaultValue = 20)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true, DefaultValue = 20)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true, DefaultValue = 20)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true, DefaultValue = 20.5)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true, DefaultValue = 20.5)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true, DefaultValue = 20.5)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true, DefaultValue = "2017-01-02 12:00:00")]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private DateTime? nowFieldNull;

        /// <summary>
        /// NowFieldNull
        /// </summary>
        /// <value></value>
        [DataField("NowFieldNull", IsNullable = true, DefaultValue = DefaultTime.Now)]
        public DateTime? NowFieldNull
        {
            get { 
                return this.nowFieldNull; 
            }
            set { 
                this.nowFieldNull = value; 
            }
        }
        private DateTime? todayFieldNull;

        /// <summary>
        /// TodayFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TodayFieldNull", IsNullable = true, DefaultValue = DefaultTime.Today)]
        public DateTime? TodayFieldNull
        {
            get { 
                return this.todayFieldNull; 
            }
            set { 
                this.todayFieldNull = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true, DefaultValue = "testtest")]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true, DefaultValue = "testtest")]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true, DefaultValue = EnumInt32Type.Positive1)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true, DefaultValue = EnumInt64Type.Positive1)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField", DefaultValue = "2017-01-02 12:00:00")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime nowField;

        /// <summary>
        /// NowField
        /// </summary>
        /// <value></value>
        [DataField("NowField", DefaultValue = DefaultTime.Now)]
        public DateTime NowField
        {
            get { 
                return this.nowField; 
            }
            set { 
                this.nowField = value; 
            }
        }
        private DateTime todayField;

        /// <summary>
        /// TodayField
        /// </summary>
        /// <value></value>
        [DataField("TodayField", DefaultValue = DefaultTime.Today)]
        public DateTime TodayField
        {
            get { 
                return this.todayField; 
            }
            set { 
                this.todayField = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseField_Alias
    /// </summary>
    [DataTable("Te_BaseField_Alias")]
    public class TeBaseFieldAlias 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
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
        [DataField("BoolField")]
        public bool BoolField
        {
            get { 
                return this.boolField; 
            }
            set { 
                this.boolField = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public sbyte SbyteField
        {
            get { 
                return this.sbyteField; 
            }
            set { 
                this.sbyteField = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField
        {
            get { 
                return this.byteField; 
            }
            set { 
                this.byteField = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field
        {
            get { 
                return this.int16Field; 
            }
            set { 
                this.int16Field = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public ushort UInt16Field
        {
            get { 
                return this.uInt16Field; 
            }
            set { 
                this.uInt16Field = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public uint UInt32Field
        {
            get { 
                return this.uInt32Field; 
            }
            set { 
                this.uInt32Field = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field
        {
            get { 
                return this.int64Field; 
            }
            set { 
                this.int64Field = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public ulong UInt64Field
        {
            get { 
                return this.uInt64Field; 
            }
            set { 
                this.uInt64Field = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField
        {
            get { 
                return this.floatField; 
            }
            set { 
                this.floatField = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textField;

        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        [DataField("TextField")]
        public string TextField
        {
            get { 
                return this.textField; 
            }
            set { 
                this.textField = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true)]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private byte[] bigDataField;

        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        [DataField("BigDataField")]
        public byte[] BigDataField
        {
            get { 
                return this.bigDataField; 
            }
            set { 
                this.bigDataField = value; 
            }
        }
        private byte[] bigDataFieldNull;

        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull", IsNullable = true)]
        public byte[] BigDataFieldNull
        {
            get { 
                return this.bigDataFieldNull; 
            }
            set { 
                this.bigDataFieldNull = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public EnumInt64Type EnumInt64Field
        {
            get { 
                return this.enumInt64Field; 
            }
            set { 
                this.enumInt64Field = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_MainTable
    /// </summary>
    [DataTable("Te_MainTable")]
    public class TeMainTable 
    {
        #region "Data Property"
        private int mainId;

        /// <summary>
        /// MainId
        /// </summary>
        /// <value></value>
        [DataField("MainId", IsIdentity = true, IsPrimaryKey = true)]
        public int MainId
        {
            get { 
                return this.mainId; 
            }
            set { 
                this.mainId = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private int subId;

        /// <summary>
        /// SubId
        /// </summary>
        /// <value></value>
        [DataField("SubId")]
        public int SubId
        {
            get { 
                return this.subId; 
            }
            set { 
                this.subId = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_SubTable
    /// </summary>
    [DataTable("Te_SubTable")]
    public class TeSubTable 
    {
        #region "Data Property"
        private int subId;

        /// <summary>
        /// SubId
        /// </summary>
        /// <value></value>
        [DataField("SubId", IsIdentity = true, IsPrimaryKey = true)]
        public int SubId
        {
            get { 
                return this.subId; 
            }
            set { 
                this.subId = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_JoinTable_SelectInsert
    /// </summary>
    [DataTable("Te_JoinTable_SelectInsert")]
    public class TeJoinTableSelectInsert 
    {
        #region "Data Property"
        private int mainId;

        /// <summary>
        /// MainId
        /// </summary>
        /// <value></value>
        [DataField("MainId", IsPrimaryKey = true)]
        public int MainId
        {
            get { 
                return this.mainId; 
            }
            set { 
                this.mainId = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private int subId;

        /// <summary>
        /// SubId
        /// </summary>
        /// <value></value>
        [DataField("SubId")]
        public int SubId
        {
            get { 
                return this.subId; 
            }
            set { 
                this.subId = value; 
            }
        }
        private int subInt32Field;

        /// <summary>
        /// SubInt32Field
        /// </summary>
        /// <value></value>
        [DataField("SubInt32Field")]
        public int SubInt32Field
        {
            get { 
                return this.subInt32Field; 
            }
            set { 
                this.subInt32Field = value; 
            }
        }
        private int? subInt32FieldNull;

        /// <summary>
        /// SubInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("SubInt32FieldNull", IsNullable = true)]
        public int? SubInt32FieldNull
        {
            get { 
                return this.subInt32FieldNull; 
            }
            set { 
                this.subInt32FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_RelateA
    /// </summary>
    [DataTable("Te_RelateA")]
    public class TeRelateA 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId
        {
            get { 
                return this.relateBId; 
            }
            set { 
                this.relateBId = value; 
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId
        {
            get { 
                return this.relateCId; 
            }
            set { 
                this.relateCId = value; 
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId
        {
            get { 
                return this.relateDId; 
            }
            set { 
                this.relateDId = value; 
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId
        {
            get { 
                return this.relateEId; 
            }
            set { 
                this.relateEId = value; 
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId
        {
            get { 
                return this.relateFId; 
            }
            set { 
                this.relateFId = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
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
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
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
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_RelateB
    /// </summary>
    [DataTable("Te_RelateB")]
    public class TeRelateB 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId
        {
            get { 
                return this.relateAId; 
            }
            set { 
                this.relateAId = value; 
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId
        {
            get { 
                return this.relateCId; 
            }
            set { 
                this.relateCId = value; 
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId
        {
            get { 
                return this.relateDId; 
            }
            set { 
                this.relateDId = value; 
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId
        {
            get { 
                return this.relateEId; 
            }
            set { 
                this.relateEId = value; 
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId
        {
            get { 
                return this.relateFId; 
            }
            set { 
                this.relateFId = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
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
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
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
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_RelateC
    /// </summary>
    [DataTable("Te_RelateC")]
    public class TeRelateC 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId
        {
            get { 
                return this.relateAId; 
            }
            set { 
                this.relateAId = value; 
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId
        {
            get { 
                return this.relateBId; 
            }
            set { 
                this.relateBId = value; 
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId
        {
            get { 
                return this.relateDId; 
            }
            set { 
                this.relateDId = value; 
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId
        {
            get { 
                return this.relateEId; 
            }
            set { 
                this.relateEId = value; 
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId
        {
            get { 
                return this.relateFId; 
            }
            set { 
                this.relateFId = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_RelateD
    /// </summary>
    [DataTable("Te_RelateD")]
    public class TeRelateD 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId
        {
            get { 
                return this.relateAId; 
            }
            set { 
                this.relateAId = value; 
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId
        {
            get { 
                return this.relateBId; 
            }
            set { 
                this.relateBId = value; 
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId
        {
            get { 
                return this.relateCId; 
            }
            set { 
                this.relateCId = value; 
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId
        {
            get { 
                return this.relateEId; 
            }
            set { 
                this.relateEId = value; 
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId
        {
            get { 
                return this.relateFId; 
            }
            set { 
                this.relateFId = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_RelateE
    /// </summary>
    [DataTable("Te_RelateE")]
    public class TeRelateE 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId
        {
            get { 
                return this.relateAId; 
            }
            set { 
                this.relateAId = value; 
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId
        {
            get { 
                return this.relateBId; 
            }
            set { 
                this.relateBId = value; 
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId
        {
            get { 
                return this.relateCId; 
            }
            set { 
                this.relateCId = value; 
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId
        {
            get { 
                return this.relateDId; 
            }
            set { 
                this.relateDId = value; 
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId
        {
            get { 
                return this.relateFId; 
            }
            set { 
                this.relateFId = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_RelateF
    /// </summary>
    [DataTable("Te_RelateF")]
    public class TeRelateF 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId
        {
            get { 
                return this.relateAId; 
            }
            set { 
                this.relateAId = value; 
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId
        {
            get { 
                return this.relateBId; 
            }
            set { 
                this.relateBId = value; 
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId
        {
            get { 
                return this.relateCId; 
            }
            set { 
                this.relateCId = value; 
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId
        {
            get { 
                return this.relateDId; 
            }
            set { 
                this.relateDId = value; 
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId
        {
            get { 
                return this.relateEId; 
            }
            set { 
                this.relateEId = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_RelateCollection
    /// </summary>
    [DataTable("Te_RelateCollection")]
    public class TeRelateCollection 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId
        {
            get { 
                return this.relateAId; 
            }
            set { 
                this.relateAId = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseFieldAggregateField_GroupBy
    /// </summary>
    [DataTable("Te_BaseFieldAggregateField_GroupBy")]
    public class TeBaseFieldAggregateFieldGroupBy 
    {
        #region "Data Property"
        private int keyData;

        /// <summary>
        /// KeyData
        /// </summary>
        /// <value></value>
        [DataField("KeyData", IsPrimaryKey = true)]
        public int KeyData
        {
            get { 
                return this.keyData; 
            }
            set { 
                this.keyData = value; 
            }
        }
        private int monthData;

        /// <summary>
        /// MonthData
        /// </summary>
        /// <value></value>
        [DataField("MonthData", IsPrimaryKey = true)]
        public int MonthData
        {
            get { 
                return this.monthData; 
            }
            set { 
                this.monthData = value; 
            }
        }
        private int countData;

        /// <summary>
        /// CountData
        /// </summary>
        /// <value></value>
        [DataField("CountData")]
        public int CountData
        {
            get { 
                return this.countData; 
            }
            set { 
                this.countData = value; 
            }
        }
        private int countFieldData;

        /// <summary>
        /// CountFieldData
        /// </summary>
        /// <value></value>
        [DataField("CountFieldData")]
        public int CountFieldData
        {
            get { 
                return this.countFieldData; 
            }
            set { 
                this.countFieldData = value; 
            }
        }
        private int countConditionData;

        /// <summary>
        /// CountConditionData
        /// </summary>
        /// <value></value>
        [DataField("CountConditionData")]
        public int CountConditionData
        {
            get { 
                return this.countConditionData; 
            }
            set { 
                this.countConditionData = value; 
            }
        }
        private int sumData;

        /// <summary>
        /// SumData
        /// </summary>
        /// <value></value>
        [DataField("SumData")]
        public int SumData
        {
            get { 
                return this.sumData; 
            }
            set { 
                this.sumData = value; 
            }
        }
        private double avgData;

        /// <summary>
        /// AvgData
        /// </summary>
        /// <value></value>
        [DataField("AvgData")]
        public double AvgData
        {
            get { 
                return this.avgData; 
            }
            set { 
                this.avgData = value; 
            }
        }
        private DateTime maxData;

        /// <summary>
        /// MaxData
        /// </summary>
        /// <value></value>
        [DataField("MaxData")]
        public DateTime MaxData
        {
            get { 
                return this.maxData; 
            }
            set { 
                this.maxData = value; 
            }
        }
        private DateTime minData;

        /// <summary>
        /// MinData
        /// </summary>
        /// <value></value>
        [DataField("MinData")]
        public DateTime MinData
        {
            get { 
                return this.minData; 
            }
            set { 
                this.minData = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_ObjectField
    /// </summary>
    [DataTable("Te_ObjectField")]
    public class TeObjectField 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private ObjectType objectField;

        /// <summary>
        /// ObjectField
        /// </summary>
        /// <value></value>
        [DataField("ObjectField")]
        public ObjectType ObjectField
        {
            get { 
                return this.objectField; 
            }
            set { 
                this.objectField = value; 
            }
        }
        private ObjectType objectFieldNull;

        /// <summary>
        /// ObjectFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ObjectFieldNull", IsNullable = true)]
        public ObjectType ObjectFieldNull
        {
            get { 
                return this.objectFieldNull; 
            }
            set { 
                this.objectFieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseField_Entity
    /// </summary>
    [DataTable("Te_BaseField_Entity")]
    public class TeBaseFieldEntity : DataTableEntity
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id
        {
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
        [DataField("BoolField")]
        public bool BoolField
        {
            get { 
                return this.boolField; 
            }
            set { 
                this.boolField = value; 
            }
        }
        private bool? boolFieldNull;

        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull
        {
            get { 
                return this.boolFieldNull; 
            }
            set { 
                this.boolFieldNull = value; 
            }
        }
        private sbyte sbyteField;

        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public sbyte SbyteField
        {
            get { 
                return this.sbyteField; 
            }
            set { 
                this.sbyteField = value; 
            }
        }
        private sbyte? sbyteFieldNull;

        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public sbyte? SbyteFieldNull
        {
            get { 
                return this.sbyteFieldNull; 
            }
            set { 
                this.sbyteFieldNull = value; 
            }
        }
        private byte byteField;

        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField
        {
            get { 
                return this.byteField; 
            }
            set { 
                this.byteField = value; 
            }
        }
        private byte? byteFieldNull;

        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull
        {
            get { 
                return this.byteFieldNull; 
            }
            set { 
                this.byteFieldNull = value; 
            }
        }
        private short int16Field;

        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field
        {
            get { 
                return this.int16Field; 
            }
            set { 
                this.int16Field = value; 
            }
        }
        private short? int16FieldNull;

        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull
        {
            get { 
                return this.int16FieldNull; 
            }
            set { 
                this.int16FieldNull = value; 
            }
        }
        private ushort uInt16Field;

        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public ushort UInt16Field
        {
            get { 
                return this.uInt16Field; 
            }
            set { 
                this.uInt16Field = value; 
            }
        }
        private ushort? uInt16FieldNull;

        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public ushort? UInt16FieldNull
        {
            get { 
                return this.uInt16FieldNull; 
            }
            set { 
                this.uInt16FieldNull = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull
        {
            get { 
                return this.int32FieldNull; 
            }
            set { 
                this.int32FieldNull = value; 
            }
        }
        private uint uInt32Field;

        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public uint UInt32Field
        {
            get { 
                return this.uInt32Field; 
            }
            set { 
                this.uInt32Field = value; 
            }
        }
        private uint? uInt32FieldNull;

        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public uint? UInt32FieldNull
        {
            get { 
                return this.uInt32FieldNull; 
            }
            set { 
                this.uInt32FieldNull = value; 
            }
        }
        private long int64Field;

        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field
        {
            get { 
                return this.int64Field; 
            }
            set { 
                this.int64Field = value; 
            }
        }
        private long? int64FieldNull;

        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull
        {
            get { 
                return this.int64FieldNull; 
            }
            set { 
                this.int64FieldNull = value; 
            }
        }
        private ulong uInt64Field;

        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public ulong UInt64Field
        {
            get { 
                return this.uInt64Field; 
            }
            set { 
                this.uInt64Field = value; 
            }
        }
        private ulong? uInt64FieldNull;

        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public ulong? UInt64FieldNull
        {
            get { 
                return this.uInt64FieldNull; 
            }
            set { 
                this.uInt64FieldNull = value; 
            }
        }
        private float floatField;

        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField
        {
            get { 
                return this.floatField; 
            }
            set { 
                this.floatField = value; 
            }
        }
        private float? floatFieldNull;

        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull
        {
            get { 
                return this.floatFieldNull; 
            }
            set { 
                this.floatFieldNull = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private double? doubleFieldNull;

        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull
        {
            get { 
                return this.doubleFieldNull; 
            }
            set { 
                this.doubleFieldNull = value; 
            }
        }
        private decimal decimalField;

        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField
        {
            get { 
                return this.decimalField; 
            }
            set { 
                this.decimalField = value; 
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull
        {
            get { 
                return this.decimalFieldNull; 
            }
            set { 
                this.decimalFieldNull = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull
        {
            get { 
                return this.dateTimeFieldNull; 
            }
            set { 
                this.dateTimeFieldNull = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull
        {
            get { 
                return this.varcharFieldNull; 
            }
            set { 
                this.varcharFieldNull = value; 
            }
        }
        private string textField;

        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        [DataField("TextField")]
        public string TextField
        {
            get { 
                return this.textField; 
            }
            set { 
                this.textField = value; 
            }
        }
        private string textFieldNull;

        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true)]
        public string TextFieldNull
        {
            get { 
                return this.textFieldNull; 
            }
            set { 
                this.textFieldNull = value; 
            }
        }
        private byte[] bigDataField;

        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        [DataField("BigDataField")]
        public byte[] BigDataField
        {
            get { 
                return this.bigDataField; 
            }
            set { 
                this.bigDataField = value; 
            }
        }
        private byte[] bigDataFieldNull;

        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull", IsNullable = true)]
        public byte[] BigDataFieldNull
        {
            get { 
                return this.bigDataFieldNull; 
            }
            set { 
                this.bigDataFieldNull = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull
        {
            get { 
                return this.enumInt32FieldNull; 
            }
            set { 
                this.enumInt32FieldNull = value; 
            }
        }
        private EnumInt64Type enumInt64Field;

        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public EnumInt64Type EnumInt64Field
        {
            get { 
                return this.enumInt64Field; 
            }
            set { 
                this.enumInt64Field = value; 
            }
        }
        private EnumInt64Type? enumInt64FieldNull;

        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public EnumInt64Type? EnumInt64FieldNull
        {
            get { 
                return this.enumInt64FieldNull; 
            }
            set { 
                this.enumInt64FieldNull = value; 
            }
        }
        #endregion
    }

	/// <summary>
    /// Te_BaseFieldNoIdentity_Entity
    /// </summary>
    [DataTable("Te_BaseFieldNoIdentity_Entity")]
    public class TeBaseFieldNoIdentityEntity : DataTableEntity
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsPrimaryKey = true)]
        public int Id
        {
            get { 
                return this.id; 
            }
            set { 
                this.id = value; 
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field
        {
            get { 
                return this.int32Field; 
            }
            set { 
                this.int32Field = value; 
            }
        }
        private double doubleField;

        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField
        {
            get { 
                return this.doubleField; 
            }
            set { 
                this.doubleField = value; 
            }
        }
        private string varcharField;

        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField
        {
            get { 
                return this.varcharField; 
            }
            set { 
                this.varcharField = value; 
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField
        {
            get { 
                return this.dateTimeField; 
            }
            set { 
                this.dateTimeField = value; 
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public EnumInt32Type EnumInt32Field
        {
            get { 
                return this.enumInt32Field; 
            }
            set { 
                this.enumInt32Field = value; 
            }
        }
        #endregion
    }

}

