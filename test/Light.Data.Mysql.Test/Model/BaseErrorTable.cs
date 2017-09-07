using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Mysql.Test
{
    [DataTable("Te_BaseErrorTable")]
    public class BaseErrorTable
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
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
        [DataField("BoolField")]
        public bool BoolField {
            get {
                return this.boolField;
            }
            set {
                this.boolField = value;
            }
        }
        #endregion
    }
}