using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Test
{
    public class TeBaseFieldConfig 
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
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
        /// Int32_Field
        /// </summary>
        /// <value></value>
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
        /// Int32_FieldNull
        /// </summary>
        /// <value></value>
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
        /// Decimal_Field
        /// </summary>
        /// <value></value>
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
        /// Decimal_FieldNull
        /// </summary>
        /// <value></value>
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
        /// DateTime_Field
        /// </summary>
        /// <value></value>
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
        /// DateTime_FieldNull
        /// </summary>
        /// <value></value>
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
        /// Varchar_Field
        /// </summary>
        /// <value></value>
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
        /// Varchar_FieldNull
        /// </summary>
        /// <value></value>
        public string VarcharFieldNull
        {
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
        public DateTime NowField
        {
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
        public DateTime? NowFieldNull
        {
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
        public DateTime TodayField
        {
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
        public DateTime? TodayFieldNull
        {
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
        /// EnumInt32_FieldNull
        /// </summary>
        /// <value></value>
        public EnumInt32Type? EnumInt32FieldNull
        {
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

