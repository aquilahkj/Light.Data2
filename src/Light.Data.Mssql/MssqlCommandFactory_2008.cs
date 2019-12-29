using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data.Mssql
{
    internal class MssqlCommandFactory_2008 : MssqlCommandFactory
    {
        public override bool SupportBatchInsertIdentity => true;
       
        public override CommandData CreateBaseInsertCommand(DataTableEntityMapping mapping, object entity, bool refresh, bool updateIdentity, CreateSqlState state)
        {
            string cachekey = null;
            var identity = updateIdentity && mapping.HasIdentity;
            if (state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (identity) {
                    cachekey = string.Concat(cachekey, "|id");
                }
                if (_baseInsertCache.TryGetCommand(cachekey, out var cache)) {
                    var command1 = new CommandData(cache);
                    command1.IdentitySql = identity;
                    foreach (var field in mapping.CreateFieldList) {
                        var value = field.ToInsert(entity, refresh);
                        state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                    }
                    return command1;
                }
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
            if (insertLen == 0) {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            var insertList = new string[insertLen];
            var valuesList = new string[insertLen];
            for (var i = 0; i < insertLen; i++) {
                var field = fields[i];
                var value = field.ToInsert(entity, refresh);
                insertList[i] = CreateDataFieldSql(field.Name);
                valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
            }
            var insert = string.Join(",", insertList);
            var values = string.Join(",", valuesList);
            string sql;
            if (identity) {
                sql = string.Format("insert into {0}({1}) output inserted.{3} values({2})", CreateDataTableMappingSql(mapping, state), insert, values, CreateDataFieldSql(mapping.IdentityField.Name));
            }
            else {
                sql = string.Format("insert into {0}({1})values({2})", CreateDataTableMappingSql(mapping, state), insert, values);
            }

            var command = new CommandData(sql);
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
            var totalCount = entitys.Count;
            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
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
                if (_batchInsertCache.TryGetCommand(cachekey, out var cache)) {
                    insertSql = cache;
                }
            }
            if (insertSql == null) {
                var insertList = new string[insertLen];
                for (var i = 0; i < insertLen; i++) {
                    var field = fields[i];
                    insertList[i] = CreateDataFieldSql(field.Name);
                }
                var insert = string.Join(",", insertList);
                insertSql = string.Format("insert into {0}({1})", CreateDataTableMappingSql(mapping, state), insert);
                if (cachekey != null) {
                    _batchInsertCache.SetCommand(cachekey, insertSql);
                }
            }
            var totalSql = new StringBuilder();

            totalSql.AppendFormat("{0}output inserted.{1} as id values", insertSql, CreateDataFieldSql(mapping.IdentityField.Name));
            var cur = 0;
            var end = entitys.Count;
            foreach (var entity in entitys) {
                var valuesList = new string[insertLen];
                for (var i = 0; i < insertLen; i++) {
                    var field = fields[i];
                    var value = field.ToInsert(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }
                var values = string.Join(",", valuesList);
                totalSql.AppendFormat("({0})", values);
                cur++;
                if (cur < end) {
                    totalSql.Append(',');
                }
                else {
                    totalSql.Append(';');
                }
            }
            var command = new CommandData(totalSql.ToString());
            return command;
        }

        public override CommandData CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys, bool refresh, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0) {
                throw new ArgumentNullException(nameof(entitys));
            }
            var totalCount = entitys.Count;
            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
            if (insertLen == 0) {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }
            string insertSql = null;
            string cachekey = null;
            if (state.Seed == 0) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_batchInsertCache.TryGetCommand(cachekey, out var cache)) {
                    insertSql = cache;
                }
            }
            if (insertSql == null) {
                var insertList = new string[insertLen];
                for (var i = 0; i < insertLen; i++) {
                    var field = fields[i];
                    insertList[i] = CreateDataFieldSql(field.Name);
                }
                var insert = string.Join(",", insertList);
                insertSql = string.Format("insert into {0}({1})", CreateDataTableMappingSql(mapping, state), insert);
                if (cachekey != null) {
                    _batchInsertCache.SetCommand(cachekey, insertSql);
                }
            }
            var totalSql = new StringBuilder();

            totalSql.AppendFormat("{0}values", insertSql);
            var cur = 0;
            var end = entitys.Count;
            foreach (var entity in entitys) {
                var valuesList = new string[insertLen];
                for (var i = 0; i < insertLen; i++) {
                    var field = fields[i];
                    var value = field.ToInsert(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }
                var values = string.Join(",", valuesList);
                totalSql.AppendFormat("({0})", values);
                cur++;
                if (cur < end) {
                    totalSql.Append(',');
                }
                else {
                    totalSql.Append(';');
                }
            }
            var command = new CommandData(totalSql.ToString());
            return command;
        }
    }
}
