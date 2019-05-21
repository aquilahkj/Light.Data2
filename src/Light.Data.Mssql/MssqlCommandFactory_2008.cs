using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data.Mssql
{
    class MssqlCommandFactory_2008 : MssqlCommandFactory
    {
        public override bool SupportBatchInsertIdentity => true;
       
        public override CommandData CreateBaseInsertCommand(DataTableEntityMapping mapping, object entity, bool refresh, bool updateIdentity, CreateSqlState state)
        {
            string cachekey = null;
            bool identity = updateIdentity && mapping.HasIdentity;
            if (state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (identity) {
                    cachekey = string.Concat(cachekey, "|id");
                }
                if (_baseInsertCache.TryGetCommand(cachekey, out string cache)) {
                    CommandData command1 = new CommandData(cache);
                    command1.IdentitySql = identity;
                    foreach (DataFieldMapping field in mapping.CreateFieldList) {
                        object value = field.GetInsertData(entity, refresh);
                        state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                    }
                    return command1;
                }
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            int insertLen = fields.Count;
            if (insertLen == 0) {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            string[] insertList = new string[insertLen];
            string[] valuesList = new string[insertLen];
            for (int i = 0; i < insertLen; i++) {
                DataFieldMapping field = fields[i];
                object value = field.GetInsertData(entity, refresh);
                insertList[i] = CreateDataFieldSql(field.Name);
                valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
            }
            string insert = string.Join(",", insertList);
            string values = string.Join(",", valuesList);
            string sql;
            if (identity) {
                sql = string.Format("insert into {0}({1}) output inserted.{3} values({2})", CreateDataTableMappingSql(mapping, state), insert, values, CreateDataFieldSql(mapping.IdentityField.Name));
            }
            else {
                sql = string.Format("insert into {0}({1})values({2})", CreateDataTableMappingSql(mapping, state), insert, values);
            }

            CommandData command = new CommandData(sql);
            command.IdentitySql = identity;
            if (cachekey != null) {
                _baseInsertCache.SetCommand(cachekey, command.CommandText);
            }
            return command;
        }

        public override CommandData CreateBatchInsertWithIdentityCommand(DataTableEntityMapping mapping, IList entitys, bool refresh, CreateSqlState state)
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
            if (!mapping.HasIdentity) {
                throw new LightDataException(string.Format(SR.NoIdentityField, mapping.ObjectType));
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
            StringBuilder totalSql = new StringBuilder();

            totalSql.AppendFormat("{0}output inserted.{1} as id values", insertSql, CreateDataFieldSql(mapping.IdentityField.Name));
            int cur = 0;
            int end = entitys.Count;
            foreach (object entity in entitys) {
                string[] valuesList = new string[insertLen];
                for (int i = 0; i < insertLen; i++) {
                    DataFieldMapping field = fields[i];
                    object value = field.GetInsertData(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }
                string values = string.Join(",", valuesList);
                totalSql.AppendFormat("({0})", values);
                cur++;
                if (cur < end) {
                    totalSql.Append(',');
                }
                else {
                    totalSql.Append(';');
                }
            }
            CommandData command = new CommandData(totalSql.ToString());
            return command;
        }

        public override CommandData CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys, bool refresh, CreateSqlState state)
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
            StringBuilder totalSql = new StringBuilder();

            totalSql.AppendFormat("{0}values", insertSql);
            int cur = 0;
            int end = entitys.Count;
            foreach (object entity in entitys) {
                string[] valuesList = new string[insertLen];
                for (int i = 0; i < insertLen; i++) {
                    DataFieldMapping field = fields[i];
                    object value = field.GetInsertData(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }
                string values = string.Join(",", valuesList);
                totalSql.AppendFormat("({0})", values);
                cur++;
                if (cur < end) {
                    totalSql.Append(',');
                }
                else {
                    totalSql.Append(';');
                }
            }
            CommandData command = new CommandData(totalSql.ToString());
            return command;
        }
    }
}
