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
        /// <summary>
        /// 是否已经读取数据
        /// </summary>
        bool _hasLoadData;

        /// <summary>
        /// 是否允许记录update字段
        /// </summary>
        bool _allowNotifyUpdateField;

        /// <summary>
        /// 保存数据
        /// </summary>
        public int Save()
        {
            if (Context != null) {
                int ret;
                DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
                if (_hasLoadData) {
                    ret = Context.Update(mapping, this);
                    Clear();
                }
                else {
                    ret = Context.Insert(mapping, this);
                    _hasLoadData = true;
                }
                return ret;
            }
            else {
                throw new LightDataException(SR.DataContextIsNotExists);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public async Task<int> SaveAsync()
        {
            return await SaveAsync(CancellationToken.None);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            if (Context != null) {
                int ret;
                DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
                if (_hasLoadData) {
                    ret = await Context.UpdateAsync(mapping, this, cancellationToken);
                    Clear();
                }
                else {
                    ret = await Context.InsertAsync(mapping, this, cancellationToken);
                    Clear();
                    _hasLoadData = true;
                }
                return ret;
            }
            else {
                throw new LightDataException(SR.DataContextIsNotExists);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public int Erase()
        {
            if (Context != null) {
                int ret;
                DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
                ret = Context.Delete(mapping, this);
                Clear();
                _hasLoadData = false;
                return ret;
            }
            else {
                throw new LightDataException(SR.DataContextIsNotExists);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public async Task<int> EraseAsync()
        {
            return await EraseAsync(CancellationToken.None);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public async Task<int> EraseAsync(CancellationToken cancellationToken)
        {
            if (Context != null) {
                int ret;
                DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(this.GetType());
                ret = await Context.DeleteAsync(mapping, this, cancellationToken);
                Clear();
                _hasLoadData = false;
                return ret;
            }
            else {
                throw new LightDataException(SR.DataContextIsNotExists);
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
            if (_allowNotifyUpdateField) {
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

        private void Clear()
        {
            if (_updateFields != null) {
                _updateFields.Clear();
                _updateFields = null;
            }
        }

        /// <summary>
        /// 读取数据完成
        /// </summary>
        internal override void LoadDataComplete()
        {
            _hasLoadData = true;
            _allowNotifyUpdateField = true;
        }
    }
}
