using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Mysql.Test
{
    [DataTable("Te_BaseField_Config_Replace1")]
    public class TeBaseFieldConfigReplace
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id1", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int int32Field;

        /// <summary>
        /// Int32_Field
        /// </summary>
        /// <value></value>
        [DataField("Int32_Field1")]
        public int Int32Field {
            get {
                return this.int32Field;
            }
            set {
                this.int32Field = value;
            }
        }
        private int? int32FieldNull;

        /// <summary>
        /// Int32_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Int32_FieldNull1", IsNullable = true, DefaultValue = 20)]
        public int? Int32FieldNull {
            get {
                return this.int32FieldNull;
            }
            set {
                this.int32FieldNull = value;
            }
        }
        private decimal decimalField;

        /// <summary>
        /// Decimal_Field
        /// </summary>
        /// <value></value>
        [DataField("Decimal_Field1")]
        public decimal DecimalField {
            get {
                return this.decimalField;
            }
            set {
                this.decimalField = value;
            }
        }
        private decimal? decimalFieldNull;

        /// <summary>
        /// Decimal_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Decimal_FieldNull1", IsNullable = true, DefaultValue = 20.5)]
        public decimal? DecimalFieldNull {
            get {
                return this.decimalFieldNull;
            }
            set {
                this.decimalFieldNull = value;
            }
        }
        private DateTime dateTimeField;

        /// <summary>
        /// DateTime_Field
        /// </summary>
        /// <value></value>
        [DataField("DateTime_Field")]
        public DateTime DateTimeField {
            get {
                return this.dateTimeField;
            }
            set {
                this.dateTimeField = value;
            }
        }
        private DateTime? dateTimeFieldNull;

        /// <summary>
        /// DateTime_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("DateTime_FieldNull", IsNullable = true)]
        public DateTime? DateTimeFieldNull {
            get {
                return this.dateTimeFieldNull;
            }
            set {
                this.dateTimeFieldNull = value;
            }
        }
        private string varcharField;

        /// <summary>
        /// Varchar_Field
        /// </summary>
        /// <value></value>
        [DataField("Varchar_Field")]
        public string VarcharField {
            get {
                return this.varcharField;
            }
            set {
                this.varcharField = value;
            }
        }
        private string varcharFieldNull;

        /// <summary>
        /// Varchar_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Varchar_FieldNull", IsNullable = true)]
        public string VarcharFieldNull {
            get {
                return this.varcharFieldNull;
            }
            set {
                this.varcharFieldNull = value;
            }
        }
        private DateTime nowField;

        /// <summary>
        /// Now_Field
        /// </summary>
        /// <value></value>
        [DataField("Now_Field")]
        public DateTime NowField {
            get {
                return this.nowField;
            }
            set {
                this.nowField = value;
            }
        }
        private DateTime? nowFieldNull;

        /// <summary>
        /// Now_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Now_FieldNull", IsNullable = true)]
        public DateTime? NowFieldNull {
            get {
                return this.nowFieldNull;
            }
            set {
                this.nowFieldNull = value;
            }
        }
        private DateTime todayField;

        /// <summary>
        /// Today_Field
        /// </summary>
        /// <value></value>
        [DataField("Today_Field")]
        public DateTime TodayField {
            get {
                return this.todayField;
            }
            set {
                this.todayField = value;
            }
        }
        private DateTime? todayFieldNull;

        /// <summary>
        /// Today_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("Today_FieldNull", IsNullable = true)]
        public DateTime? TodayFieldNull {
            get {
                return this.todayFieldNull;
            }
            set {
                this.todayFieldNull = value;
            }
        }
        private EnumInt32Type enumInt32Field;

        /// <summary>
        /// EnumInt32_Field
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32_Field")]
        public EnumInt32Type EnumInt32Field {
            get {
                return this.enumInt32Field;
            }
            set {
                this.enumInt32Field = value;
            }
        }
        private EnumInt32Type? enumInt32FieldNull;

        /// <summary>
        /// EnumInt32_FieldNull
        /// </summary>
        /// <value></value>
        [DataField("EnumInt32_FieldNull", IsNullable = true)]
        public EnumInt32Type? EnumInt32FieldNull {
            get {
                return this.enumInt32FieldNull;
            }
            set {
                this.enumInt32FieldNull = value;
            }
        }
        #endregion
    }
}
