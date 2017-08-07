
namespace Light.Data
{
    /// <summary>
    /// Basic field info.
    /// </summary>
    abstract class BasicFieldInfo
    {
        internal BasicFieldInfo(DataEntityMapping tableMapping)
        {
            if (tableMapping == null)
                throw new System.ArgumentNullException(nameof(tableMapping));
            _tableMapping = tableMapping;
        }

        internal BasicFieldInfo(DataEntityMapping tableMapping, DataFieldMapping dataField)
        {
            if (tableMapping == null)
                throw new System.ArgumentNullException(nameof(tableMapping));
            if (tableMapping != DataEntityMapping.Default && dataField == null)
                throw new System.ArgumentNullException(nameof(dataField));
            _tableMapping = tableMapping;
            _dataField = dataField;
        }

        internal BasicFieldInfo(DataEntityMapping tableMapping, bool customName, string name)
        {
            if (tableMapping == null)
                throw new System.ArgumentNullException(nameof(tableMapping));
            if (name == null)
                throw new System.ArgumentNullException(nameof(name));
            _tableMapping = tableMapping;
            if (customName) {
                _dataField = new CustomFieldMapping(name, tableMapping);
            }
            else {
                _dataField = TableMapping.FindDataEntityField(name);
                if (_dataField == null) {
                    _dataField = new CustomFieldMapping(name, tableMapping);
                }
            }
        }

        readonly DataFieldMapping _dataField;

        internal DataFieldMapping DataField {
            get {
                return _dataField;
            }
        }

        readonly DataEntityMapping _tableMapping;

        /// <summary>
        /// Gets or sets the table mapping.
        /// </summary>
        /// <value>The table mapping.</value>
        internal DataEntityMapping TableMapping {
            get {
                return _tableMapping;
            }
        }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public virtual string FieldName {
            get {
                if (_dataField != null) {
                    return _dataField.Name;
                }
                else {
                    return null;
                }
            }
        }
    }
}
