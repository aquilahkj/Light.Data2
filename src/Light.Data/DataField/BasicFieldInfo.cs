
using System;

namespace Light.Data
{
    /// <summary>
    /// Basic field info.
    /// </summary>
    internal abstract class BasicFieldInfo
    {
        internal BasicFieldInfo(DataEntityMapping tableMapping)
        {
            if (tableMapping == null)
                throw new ArgumentNullException(nameof(tableMapping));
            TableMapping = tableMapping;
        }

        internal BasicFieldInfo(DataEntityMapping tableMapping, DataFieldMapping dataField)
        {
            if (tableMapping == null)
                throw new ArgumentNullException(nameof(tableMapping));
            if (tableMapping != DataEntityMapping.Default && dataField == null)
                throw new ArgumentNullException(nameof(dataField));
            TableMapping = tableMapping;
            DataField = dataField;
        }

        internal BasicFieldInfo(DataEntityMapping tableMapping, bool customName, string name)
        {
            if (tableMapping == null)
                throw new ArgumentNullException(nameof(tableMapping));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            TableMapping = tableMapping;
            if (customName) {
                DataField = new CustomFieldMapping(name, tableMapping);
            }
            else {
                DataField = TableMapping.FindDataEntityField(name);
                if (DataField == null) {
                    DataField = new CustomFieldMapping(name, tableMapping);
                }
            }
        }

        internal DataFieldMapping DataField { get; }

        /// <summary>
        /// Gets or sets the table mapping.
        /// </summary>
        /// <value>The table mapping.</value>
        internal DataEntityMapping TableMapping { get; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public virtual string FieldName {
            get
            {
                if (DataField != null) {
                    return DataField.Name;
                }

                return null;
            }
        }
    }
}
