using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data.Mssql
{
    class MssqlCommandFactory_2008 : MssqlCommandFactory
    {
        public override Tuple<CommandData, int> CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys, int start, int batchCount, bool refresh, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0) {
                throw new ArgumentNullException(nameof(entitys));
            }
            int totalCount = entitys.Count;
            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            int insertLen = fields.Count;
            if (insertLen == 0) {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }
            string insertSql = null;
            string cachekey = null;
            if (state.Seed == 0) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_batchInsertCache.TryGetCommand(cachekey, out string cache)) {
                    insertSql = cache;
                }
            }
            if (insertSql == null) {
                string[] insertList = new string[insertLen];
                for (int i = 0; i < insertLen; i++) {
                    DataFieldMapping field = fields[i];
                    insertList[i] = CreateDataFieldSql(field.Name);
                }
                string insert = string.Join(",", insertList);
                insertSql = string.Format("insert into {0}({1})", CreateDataTableMappingSql(mapping, state), insert);
                if (cachekey != null) {
                    _batchInsertCache.SetCommand(cachekey, insertSql);
                }
            }
            int createCount = 0;

            StringBuilder totalSql = new StringBuilder();
            int end = start + batchCount;
            if (end > totalCount) {
                end = totalCount;
            }

            totalSql.AppendFormat("{0}values", insertSql);
            for (int index = start; index < end; index++) {
                object entity = entitys[index];
                string[] valuesList = new string[insertLen];
                for (int i = 0; i < insertLen; i++) {
                    DataFieldMapping field = fields[i];
                    //object obj = field.Handler.Get(entity);
                    //object value = field.ToColumn(obj);
                    object value = field.GetInsertData(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType);
                }
                string values = string.Join(",", valuesList);
                totalSql.AppendFormat("({0})", values);
                if (index < end - 1) {
                    totalSql.Append(',');
                } else {
                    totalSql.Append(';');
                }
                createCount++;
            }
            CommandData command = new CommandData(totalSql.ToString());
            Tuple<CommandData, int> result = new Tuple<CommandData, int>(command, createCount);
            return result;
        }
    }
}
