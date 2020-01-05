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

            return SimpleSave(refresh);
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
            var context = GetContext();
            var mapping = DataEntityMapping.GetTableMapping(GetType());
            var ret = _hasLoadData ? context.Update(mapping, this, refresh) : context.Insert(mapping, this, refresh);
            return ret;
        }

        private int CheckDbSave(bool refresh, SafeLevel level)
        {
            var context = GetContext();
            var mapping = DataEntityMapping.GetTableMapping(GetType());
            var ret = context.InsertOrUpdate(mapping, this, level, refresh);
            return ret;
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await CheckDbSaveAsync(true, SafeLevel.Default, cancellationToken);
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="checkDb">Whether to check the data exists in the database</param>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<int> SaveAsync(bool checkDb, bool refresh, CancellationToken cancellationToken = default)
        {
            if (checkDb) {
                return await CheckDbSaveAsync(refresh, SafeLevel.Default, cancellationToken);
            }

            return await SimpleSaveAsync(refresh, cancellationToken);
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="safeLevel">Translation level</param>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<int> SaveAsync(SafeLevel safeLevel, bool refresh, CancellationToken cancellationToken = default)
        {
            return await CheckDbSaveAsync(refresh, safeLevel, cancellationToken);
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="safeLevel">Translation level</param>
        /// <returns></returns>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> SaveAsync(SafeLevel safeLevel, CancellationToken cancellationToken = default)
        {
            return await CheckDbSaveAsync(true, safeLevel, cancellationToken);
        }

        /// <summary>
        /// Check the data exists in the database and save data
        /// </summary>
        /// <param name="refresh">Whether to set the default value to null field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<int> SaveAsync(bool refresh, CancellationToken cancellationToken = default)
        {
            return await CheckDbSaveAsync(refresh, SafeLevel.Default, cancellationToken);
        }

        private async Task<int> SimpleSaveAsync(bool refresh, CancellationToken cancellationToken = default)
        {
            var context = GetContext();
            int ret;
            var mapping = DataEntityMapping.GetTableMapping(GetType());
            if (_hasLoadData) {
                ret = await context.UpdateAsync(mapping, this, refresh, cancellationToken);
            }
            else {
                ret = await context.InsertAsync(mapping, this, refresh, cancellationToken);
            }
            return ret;
        }

        private async Task<int> CheckDbSaveAsync(bool refresh, SafeLevel level, CancellationToken cancellationToken = default)
        {
            var context = GetContext();
            var mapping = DataEntityMapping.GetTableMapping(GetType());
            var ret = await context.InsertOrUpdateAsync(mapping, this, level, refresh, cancellationToken);
            return ret;
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <returns></returns>
        public int Erase()
        {
            var context = GetContext();
            int ret;
            if (_hasLoadData) {
                var mapping = DataEntityMapping.GetTableMapping(GetType());
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
        public async Task<int> EraseAsync(CancellationToken cancellationToken = default)
        {
            var context = GetContext();
            int ret;
            if (_hasLoadData) {
                var mapping = DataEntityMapping.GetTableMapping(GetType());
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
            _updateFields?.Clear();
        }

        private HashSet<string> _updateFields;

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
                var array = new string[_updateFields.Count];
                _updateFields.CopyTo(array);
                return array;
            }

            return null;
        }

        internal void ClearUpdateFields()
        {
            if (_updateFields != null) {
                _updateFields.Clear();
                _updateFields = null;
            }
        }

        private bool allowUpdatePrimaryKey;

        internal bool IsAllowUpdatePrimaryKey()
        {
            return allowUpdatePrimaryKey;
        }

        /// <summary>
        /// Allow update primary key when change the primary key.
        /// </summary>
        /// <param name="allow"></param>
        public void AllowUpdatePrimaryKey(bool allow = true)
        {
            if (allow) {
                allowUpdatePrimaryKey = true;
                if (_hasLoadData && rawKeys == null) {
                    var mapping = DataEntityMapping.GetTableMapping(GetType());
                    SetRawPrimaryKeys(mapping.GetPrimaryKeys(this));
                }
            }
            else {
                allowUpdatePrimaryKey = false;
            }
        }

        private object[] rawKeys;

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


        private bool _hasLoadData;

        internal void LoadData()
        {
            _hasLoadData = true;
        }
    }
}
