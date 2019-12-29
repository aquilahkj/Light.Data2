using System;
using System.Data;

namespace Light.Data
{
    /// <summary>
    /// Store procedure parameter attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataParameterAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DataParameterAttribute(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 
        /// </summary>
        public DataParameterAttribute()
        {

        }

        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data Parameter Direction Mode
        /// </summary>
        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;
    }
    
}
