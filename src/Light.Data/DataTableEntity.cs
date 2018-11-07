using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    /// <summary>
    /// 表数据实体
    /// </summary>
    public class DataTableEntity : DataEntity
    {
        //object[] rawKeys = null;

        /// <summary>
        /// 保存数据
        /// </summary>
        public int Save()
        {
            return Save(true);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="refresh">is refresh null data field</param>
        public int Save(bool refresh)
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

        /// <summary>
        /// 保存数据
        /// </summary>
        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await SaveAsync(true, cancellationToken);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="refresh">is refresh null data field</param>
        public async Task<int> SaveAsync(bool refresh, CancellationToken cancellationToken)
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

        /// <summary>
        /// 删除数据
        /// </summary>
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
        /// 删除数据
        /// </summary>
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

        /// <summary>
        /// 被更新的数据字段
        /// </summary>
        HashSet<string> _updateFields;

        /// <summary>
        /// 更新字段
        /// </summary>
        /// <param name="fieldName">字段名字</param>
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
                    SetRawPrimaryKeys(mapping.GetRawKeys(this));
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


        ///// <summary>
        ///// 读取数据完成
        ///// </summary>
        //internal override void LoadDataComplete()
        //{
        //    _hasLoadData = true;
        //    _allowNotifyUpdateField = true;
        //}

        bool _hasLoadData;

        internal void LoadData()
        {
            _hasLoadData = true;
        }
    }
}
