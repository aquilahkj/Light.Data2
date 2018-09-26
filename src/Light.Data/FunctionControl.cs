using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    [Flags]
    public enum FunctionControl
    {
        Default = 0,
        Read = 1,
        Create = 2,
        Update = 4,
        Full = Read | Create | Update
    }
}
