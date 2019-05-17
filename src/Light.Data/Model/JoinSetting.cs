using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    /// <summary>
    /// The setting of join table.
    /// </summary>
    [Flags]
    public enum JoinSetting
    {
        /// <summary>
        /// Nothing to setup
        /// </summary>
        None = 0,
        /// <summary>
        /// Use distinct query
        /// </summary>
        QueryDistinct = 1,
        /// <summary>
        /// When all fields are null, the object entity is set to null
        /// </summary>
        NoDataSetEntityNull = 2
    }
}
