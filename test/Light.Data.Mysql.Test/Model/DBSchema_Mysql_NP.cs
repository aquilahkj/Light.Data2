using System;
using System.Collections.Generic;
using System.Text;
using Light.Data;

namespace Light.Data.Mysql.Test
{
	/// <summary>
    /// Te_BaseField_Config
    /// </summary>
    [DataTable("Te_BaseField_Config")]
    public class TeBaseFieldConfig 
    {
        #region "Data Property"
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
	    public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// Int32_Field
        /// </summary>
        /// <value></value>
        [DataField("Int32_Field")]
	    public int Int32Field
        {
            get;
            set;
        }
        /// <summary>
        /// Int32_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32_FieldNull", IsNullable = true)]
	    public int? Int32FieldNull
        {
            get;
            set;
        }
        /// <summary>
        /// Decimal_Field
        /// </summary>
        /// <value></value>
        [DataField("Decimal_Field")]
	    public decimal DecimalField
        {
            get;
            set;
        }
        /// <summary>
        /// Decimal_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Decimal_FieldNull", IsNullable = true)]
	    public decimal? DecimalFieldNull
        {
            get;
            set;
        }
        /// <summary>
        /// DateTime_Field
        /// </summary>
        /// <value></value>
        [DataField("DateTime_Field")]
	    public DateTime DateTimeField
        {
            get;
            set;
        }
        /// <summary>
        /// DateTime_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTime_FieldNull", IsNullable = true)]
	    public DateTime? DateTimeFieldNull
        {
            get;
            set;
        }
        /// <summary>
        /// Varchar_Field
        /// </summary>
        /// <value></value>
        [DataField("Varchar_Field")]
	    public string VarcharField
        {
            get;
            set;
        }
        /// <summary>
        /// Varchar_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Varchar_FieldNull", IsNullable = true)]
	    public string VarcharFieldNull
        {
            get;
            set;
        }
        /// <summary>
        /// Now_Field
        /// </summary>
        /// <value></value>
        [DataField("Now_Field")]
	    public DateTime NowField
        {
            get;
            set;
        }
        /// <summary>
        /// Now_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Now_FieldNull", IsNullable = true)]
	    public DateTime? NowFieldNull
        {
            get;
            set;
        }
        /// <summary>
        /// Today_Field
        /// </summary>
        /// <value></value>
        [DataField("Today_Field")]
	    public DateTime TodayField
        {
            get;
            set;
        }
        /// <summary>
        /// Today_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Today_FieldNull", IsNullable = true)]
	    public DateTime? TodayFieldNull
        {
            get;
            set;
        }
        /// <summary>
        /// EnumInt32_Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32_Field")]
	    public EnumInt32Type EnumInt32Field
        {
            get;
            set;
        }
        /// <summary>
        /// EnumInt32_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32_FieldNull", IsNullable = true)]
	    public EnumInt32Type? EnumInt32FieldNull
        {
            get;
            set;
        }
        #endregion
    }

}

