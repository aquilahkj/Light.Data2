using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Mysql.Test
{
    public class TeRelateMainConfig
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
        #endregion

        private TeRelateSubConfig subConfig;

        public TeRelateSubConfig SubConfig {
            get {
                return this.subConfig;
            }
            set {
                this.subConfig = value;
            }
        }
    }

    public class TeRelateSubConfig
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
        private int mianId;

        /// <summary>
        /// SubId
        /// </summary>
        /// <value></value>
        public int MainId {
            get {
                return this.mianId;
            }
            set {
                this.mianId = value;
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
        #endregion
    }

    public class TeRelateMainBaseConfig
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
        #endregion
    }

    public class TeRelateMainExtendConfig : TeRelateMainBaseConfig
    {
        private TeRelateSubConfig subConfig;

        public TeRelateSubConfig SubConfig {
            get {
                return this.subConfig;
            }
            set {
                this.subConfig = value;
            }
        }
    }
}
