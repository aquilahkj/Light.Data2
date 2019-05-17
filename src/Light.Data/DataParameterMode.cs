using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    /// <summary>
    /// Data Parameter Direction Mode
    /// </summary>
    [Flags]
    public enum DataParameterMode
    {
        /// <summary>
        /// Input
        /// </summary>
        Input = 1,
        /// <summary>
        /// Output
        /// </summary>
        Output = 2,
        /// <summary>
        /// Input or output
        /// </summary>
        InputOutput = Input | Output,
    }
}
