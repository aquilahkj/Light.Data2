using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data
{
    /// <summary>
    /// Store procedure parameter attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DataParameterAttribute : Attribute
    {
        private string name;

        private ParameterDirection direction = ParameterDirection.Input;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DataParameterAttribute(string name)
        {
            this.name = name;
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
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }
        /// <summary>
        /// Data Parameter Direction Mode
        /// </summary>
        public ParameterDirection Direction {
            get {
                return direction;
            }
            set {
                direction = value;
            }
        }
    }
    
}
