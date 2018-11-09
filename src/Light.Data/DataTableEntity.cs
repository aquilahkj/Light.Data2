using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    /// <summary>
    /// Data table entity
    /// </summary>
    public class DataTableEntity : DataEntity
    {
        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            return CheckDbSave(true, SafeLevel.Default);
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="checkDb">Whether to check the data exists in the database</param>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <returns></returns>
        public int Save(bool checkDb, bool refresh)
        {
            if (checkDb) {
                return CheckDbSave(refresh, SafeLevel.Default);
            }
            else {
                return SimpleSave(refresh);
            }
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="safeLevel">Translation level</param>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <returns></returns>
        public int Save(SafeLevel safeLevel, bool refresh)
        {
            return CheckDbSave(refresh, safeLevel);
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="safeLevel">Translation level</param>
        /// <returns></returns>
        public int Save(SafeLevel safeLevel)
        {
            return CheckDbSave(true, safeLevel);
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <returns></returns>
        public int Save(bool refresh)
        {
            return CheckDbSave(refresh, SafeLevel.Default);
        }

        private int SimpleSave(bool refresh)
        {
            DataContext context = GetContext();
            int ret;
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
            if (_hasLoadData) {
                ret = context.Update(mapping, this, refresh);
            }
            else {
                ret = context.Insert(mapping, this, refresh);
            }
            return ret;
        }

        private int CheckDbSave(bool refresh, SafeLevel level)
        {
            DataContext context = GetContext();
            int ret;
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
            ret = context.InsertOrUpdate(mapping, this, level, refresh);
            return ret;
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await CheckDbSaveAsync(true, SafeLevel.Default, cancellationToken);
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="checkDb">Whether to check the data exists in the database</param>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <returns></returns>
        public async Task<int> SaveAsync(bool checkDb, bool refresh, CancellationToken cancellationToken)
        {
            if (checkDb) {
                return await CheckDbSaveAsync(refresh, SafeLevel.Default, cancellationToken);
            }
            else {
                return await SimpleSaveAsync(refresh, cancellationToken);
            }
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="safeLevel">Translation level</param>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <returns></returns>
        public async Task<int> SaveAsync(SafeLevel safeLevel, bool refresh, CancellationToken cancellationToken)
        {
            return await CheckDbSaveAsync(refresh, safeLevel, cancellationToken);
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="safeLevel">Translation level</param>
        /// <returns></returns>
        public async Task<int> SaveAsync(SafeLevel safeLevel, CancellationToken cancellationToken)
        {
            return await CheckDbSaveAsync(true, safeLevel, cancellationToken);
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <returns></returns>
        public async Task<int> SaveAsync(bool refresh, CancellationToken cancellationToken)
        {
            return await CheckDbSaveAsync(refresh, SafeLevel.Default, cancellationToken);
        }

        private async Task<int> SimpleSaveAsync(bool refresh, CancellationToken cancellationToken)
        {
            DataContext context = GetContext();
            int ret;
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
            if (_hasLoadData) {
                ret = await context.UpdateAsync(mapping, this, refresh, cancellationToken);
            }
            else {
                ret = await context.InsertAsync(mapping, this, refresh, cancellationToken);
            }
            return ret;
        }

        private async Task<int> CheckDbSaveAsync(bool refresh, SafeLevel level, CancellationToken cancellationToken)
        {
            DataContext context = GetContext();
            int ret;
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
            ret = await context.InsertOrUpdateAsync(mapping, this, level, refresh, cancellationToken);
            return ret;
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <returns></returns>
        public int Erase()
        {
            DataContext context = GetContext();
            int ret;
            if (_hasLoadData) {
                DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
                ret = context.Delete(mapping, this);
            }
            else {
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> EraseAsync(CancellationToken cancellationToken)
        {
            DataContext context = GetContext();
            int ret;
            if (_hasLoadData) {
                DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
                ret = await context.DeleteAsync(mapping, this, cancellationToken);
            }
            else {
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// Reset inner data
        /// </summary>
        public void Reset()
        {
            _hasLoadData = false;
            rawKeys = null;
            if (_updateFields != null) {
                _updateFields.Clear();
            }
        }

        HashSet<string> _updateFields;

        /// <summary>
        /// Set update field
        /// </summary>
        /// <param name="fieldName">Field name</param>
        protected void UpdateDataNotify(string fieldName)
        {
            if (_hasLoadData) {
                if (_updateFields == null) {
                    _updateFields = new HashSet<string>();
                }
                _updateFields.Add(fieldName);
            }
        }

        internal string[] GetUpdateFields()
        {
            if (_updateFields != null) {
                string[] array = new string[_updateFields.Count];
                _updateFields.CopyTo(array);
                return array;
            }
            else {
                return null;
            }
        }

        internal void ClearUpdateFields()
        {
            if (_updateFields != null) {
                _updateFields.Clear();
                _updateFields = null;
            }
        }

        bool allowUpdatePrimaryKey;

        internal bool IsAllowUpdatePrimaryKey()
        {
            return allowUpdatePrimaryKey;
        }

        public void AllowUpdatePrimaryKey(bool allow = true)
        {
            if (allow) {
                allowUpdatePrimaryKey = true;
                if (_hasLoadData && rawKeys == null) {
                    DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(GetType());
                    SetRawPrimaryKeys(mapping.GetPrimaryKeys(this));
                }
            }
            else {
                allowUpdatePrimaryKey = false;
            }
        }

        object[] rawKeys = null;

        internal void SetRawPrimaryKeys(object[] keys)
        {
            rawKeys = keys;
        }

        internal void ClearRawPrimaryKeys()
        {
            rawKeys = null;
        }

        internal object[] GetRawPrimaryKeys()
        {
            return rawKeys;
        }


        bool _hasLoadData;

        internal void LoadData()
        {
            _hasLoadData = true;
        }
    }
}
