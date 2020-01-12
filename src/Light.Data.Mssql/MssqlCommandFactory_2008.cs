using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Mssql
{
    internal class MssqlCommandFactory_2008 : MssqlCommandFactory
    {
        public override bool SupportBatchInsertIdentity => true;

        public override CommandData CreateBaseInsertCommand(DataTableEntityMapping mapping, object entity, bool refresh,
            bool updateIdentity, CreateSqlState state)
        {
            string cacheKey = null;
            var identity = updateIdentity && mapping.HasIdentity;
            if (state.Seed == 0 && !state.UseDirectNull)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (identity)
                {
                    cacheKey = string.Concat(cacheKey, "|id");
                }

                if (_baseInsertCache.TryGetCommand(cacheKey, out var cache))
                {
                    var command1 = new CommandData(cache) {IdentitySql = identity};
                    foreach (var field in mapping.CreateFieldList)
                    {
                        var value = field.ToInsert(entity, refresh);
                        state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                    }

                    return command1;
                }
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
            if (insertLen == 0)
            {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            var insertList = new string[insertLen];
            var valuesList = new string[insertLen];
            for (var i = 0; i < insertLen; i++)
            {
                var field = fields[i];
                var value = field.ToInsert(entity, refresh);
                insertList[i] = CreateDataFieldSql(field.Name);
                valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
            }

            var insert = string.Join(",", insertList);
            var values = string.Join(",", valuesList);
            var sql = identity
                ? $"insert into {CreateDataTableMappingSql(mapping, state)}({insert}) output inserted.{CreateDataFieldSql(mapping.IdentityField.Name)} values({values})"
                : $"insert into {CreateDataTableMappingSql(mapping, state)}({insert})values({values})";

            var command = new CommandData(sql) {IdentitySql = identity};
            if (cacheKey != null)
            {
                _baseInsertCache.SetCommand(cacheKey, command.CommandText);
            }

            return command;
        }

        public override CommandData CreateBatchInsertWithIdentityCommand(DataTableEntityMapping mapping, IList entitys,
            bool refresh, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0)
            {
                throw new ArgumentNullException(nameof(entitys));
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
            if (insertLen == 0)
            {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            if (!mapping.HasIdentity)
            {
                throw new LightDataException(string.Format(SR.NoIdentityField, mapping.ObjectType));
            }

            string insertSql = null;
            string cacheKey = null;
            if (state.Seed == 0)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_batchInsertCache.TryGetCommand(cacheKey, out var cache))
                {
                    insertSql = cache;
                }
            }

            if (insertSql == null)
            {
                var insertList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    insertList[i] = CreateDataFieldSql(field.Name);
                }

                var insert = string.Join(",", insertList);
                insertSql = $"insert into {CreateDataTableMappingSql(mapping, state)}({insert})";
                if (cacheKey != null)
                {
                    _batchInsertCache.SetCommand(cacheKey, insertSql);
                }
            }

            var totalSql = new StringBuilder();

            totalSql.Append(
                $"{insertSql}output inserted.{CreateDataFieldSql(mapping.IdentityField.Name)} as id values");
            var cur = 0;
            var end = entitys.Count;
            foreach (var entity in entitys)
            {
                var valuesList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    var value = field.ToInsert(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }

                var values = string.Join(",", valuesList);
                totalSql.Append($"({values})");
                cur++;
                totalSql.Append(cur < end ? ',' : ';');
            }

            var command = new CommandData(totalSql.ToString());
            return command;
        }

        public override CommandData CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys,
            bool refresh, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0)
            {
                throw new ArgumentNullException(nameof(entitys));
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
            if (insertLen == 0)
            {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            string insertSql = null;
            string cacheKey = null;
            if (state.Seed == 0)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_batchInsertCache.TryGetCommand(cacheKey, out var cache))
                {
                    insertSql = cache;
                }
            }

            if (insertSql == null)
            {
                var insertList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    insertList[i] = CreateDataFieldSql(field.Name);
                }

                var insert = string.Join(",", insertList);
                insertSql = $"insert into {CreateDataTableMappingSql(mapping, state)}({insert})";
                if (cacheKey != null)
                {
                    _batchInsertCache.SetCommand(cacheKey, insertSql);
                }
            }

            var totalSql = new StringBuilder();

            totalSql.Append($"{insertSql}values");
            var cur = 0;
            var end = entitys.Count;
            foreach (var entity in entitys)
            {
                var valuesList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    var value = field.ToInsert(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }

                var values = string.Join(",", valuesList);
                totalSql.Append($"({values})");
                cur++;
                totalSql.Append(cur < end ? ',' : ';');
            }

            var command = new CommandData(totalSql.ToString());
            return command;
        }
    }
}