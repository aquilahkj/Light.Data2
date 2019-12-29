using System;
using System.Collections.Generic;
using System.Text;
using Light.Data;

namespace Light.Data.Sample
{
    [DataTable("Te_BaseField2")]
    public class TeBaseField2
    {
        #region "Data Property"
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsPrimaryKey = true, DbType = "char")]
        public Guid Id {
            get;
            set;
        }
        /// <summary>
        /// BoolField
        /// </summary>
        /// <value></value>
        [DataField("BoolField")]
        public bool BoolField {
            get;
            set;
        }
        /// <summary>
        /// BoolFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BoolFieldNull", IsNullable = true)]
        public bool? BoolFieldNull {
            get;
            set;
        }
        /// <summary>
        /// SbyteField
        /// </summary>
        /// <value></value>
        [DataField("SbyteField")]
        public short SbyteField {
            get;
            set;
        }
        /// <summary>
        /// SbyteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("SbyteFieldNull", IsNullable = true)]
        public short? SbyteFieldNull {
            get;
            set;
        }
        /// <summary>
        /// ByteField
        /// </summary>
        /// <value></value>
        [DataField("ByteField")]
        public byte ByteField {
            get;
            set;
        }
        /// <summary>
        /// ByteFieldNull
        /// </summary>
        /// <value></value>
        [DataField("ByteFieldNull", IsNullable = true)]
        public byte? ByteFieldNull {
            get;
            set;
        }
        /// <summary>
        /// Int16Field
        /// </summary>
        /// <value></value>
        [DataField("Int16Field")]
        public short Int16Field {
            get;
            set;
        }
        /// <summary>
        /// Int16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int16FieldNull", IsNullable = true)]
        public short? Int16FieldNull {
            get;
            set;
        }
        /// <summary>
        /// UInt16Field
        /// </summary>
        /// <value></value>
        [DataField("UInt16Field")]
        public int UInt16Field {
            get;
            set;
        }
        /// <summary>
        /// UInt16FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt16FieldNull", IsNullable = true)]
        public int? UInt16FieldNull {
            get;
            set;
        }
        /// <summary>
        /// Int32Field
        /// </summary>
        /// <value></value>
        [DataField("Int32Field")]
        public int Int32Field {
            get;
            set;
        }
        /// <summary>
        /// Int32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32FieldNull", IsNullable = true)]
        public int? Int32FieldNull {
            get;
            set;
        }
        /// <summary>
        /// UInt32Field
        /// </summary>
        /// <value></value>
        [DataField("UInt32Field")]
        public long UInt32Field {
            get;
            set;
        }
        /// <summary>
        /// UInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt32FieldNull", IsNullable = true)]
        public long? UInt32FieldNull {
            get;
            set;
        }
        /// <summary>
        /// Int64Field
        /// </summary>
        /// <value></value>
        [DataField("Int64Field")]
        public long Int64Field {
            get;
            set;
        }
        /// <summary>
        /// Int64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int64FieldNull", IsNullable = true)]
        public long? Int64FieldNull {
            get;
            set;
        }
        /// <summary>
        /// UInt64Field
        /// </summary>
        /// <value></value>
        [DataField("UInt64Field")]
        public decimal UInt64Field {
            get;
            set;
        }
        /// <summary>
        /// UInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("UInt64FieldNull", IsNullable = true)]
        public decimal? UInt64FieldNull {
            get;
            set;
        }
        /// <summary>
        /// FloatField
        /// </summary>
        /// <value></value>
        [DataField("FloatField")]
        public float FloatField {
            get;
            set;
        }
        /// <summary>
        /// FloatFieldNull
        /// </summary>
        /// <value></value>
        [DataField("FloatFieldNull", IsNullable = true)]
        public float? FloatFieldNull {
            get;
            set;
        }
        /// <summary>
        /// DoubleField
        /// </summary>
        /// <value></value>
        [DataField("DoubleField")]
        public double DoubleField {
            get;
            set;
        }
        /// <summary>
        /// DoubleFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DoubleFieldNull", IsNullable = true)]
        public double? DoubleFieldNull {
            get;
            set;
        }
        /// <summary>
        /// DecimalField
        /// </summary>
        /// <value></value>
        [DataField("DecimalField")]
        public decimal DecimalField {
            get;
            set;
        }
        /// <summary>
        /// DecimalFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DecimalFieldNull", IsNullable = true)]
        public decimal? DecimalFieldNull {
            get;
            set;
        }
        /// <summary>
        /// DateTimeField
        /// </summary>
        /// <value></value>
        [DataField("DateTimeField")]
        public DateTime DateTimeField {
            get;
            set;
        }
        /// <summary>
        /// DateTimeFieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTimeFieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull {
            get;
            set;
        }
        /// <summary>
        /// VarcharField
        /// </summary>
        /// <value></value>
        [DataField("VarcharField")]
        public string VarcharField {
            get;
            set;
        }
        /// <summary>
        /// VarcharFieldNull
        /// </summary>
        /// <value></value>
        [DataField("VarcharFieldNull", IsNullable = true)]
        public string VarcharFieldNull {
            get;
            set;
        }
        /// <summary>
        /// TextField
        /// </summary>
        /// <value></value>
        [DataField("TextField")]
        public string TextField {
            get;
            set;
        }
        /// <summary>
        /// TextFieldNull
        /// </summary>
        /// <value></value>
        [DataField("TextFieldNull", IsNullable = true)]
        public string TextFieldNull {
            get;
            set;
        }
        /// <summary>
        /// BigDataField
        /// </summary>
        /// <value></value>
        [DataField("BigDataField")]
        public byte[] BigDataField {
            get;
            set;
        }
        /// <summary>
        /// BigDataFieldNull
        /// </summary>
        /// <value></value>
        [DataField("BigDataFieldNull", IsNullable = true)]
        public byte[] BigDataFieldNull {
            get;
            set;
        }
        /// <summary>
        /// EnumInt32Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32Field")]
        public int EnumInt32Field {
            get;
            set;
        }
        /// <summary>
        /// EnumInt32FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32FieldNull", IsNullable = true)]
        public int? EnumInt32FieldNull {
            get;
            set;
        }
        /// <summary>
        /// EnumInt64Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64Field")]
        public long EnumInt64Field {
            get;
            set;
        }
        /// <summary>
        /// EnumInt64FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt64FieldNull", IsNullable = true)]
        public long? EnumInt64FieldNull {
            get;
            set;
        }
        #endregion
    }
}
