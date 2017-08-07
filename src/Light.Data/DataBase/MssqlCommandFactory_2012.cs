using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data.Mssql
{
    class MssqlCommandFactory_2012 : MssqlCommandFactory_2008
    {
        public override CommandData CreateSelectBaseCommand(DataEntityMapping mapping, string customSelect, QueryExpression query, OrderExpression order, Region region, CreateSqlState state)//, bool distinct)
        {
            if (region != null && region.Size > 0) {
                if (order != null) {
                    CommandData command = base.CreateSelectBaseCommand(mapping, customSelect, query, order, null, state);
                    command.CommandText = string.Format("{0} offset {1} row fetch next {2} rows only", command.CommandText, region.Start, region.Size);
                    command.InnerPage = true;
                    return command;
                } else {
                    if (mapping is DataTableEntityMapping tableMapping && tableMapping.HasPrimaryKey) {
                        foreach (DataFieldMapping fieldMapping in tableMapping.PrimaryKeyFields) {
                            DataFieldOrderExpression keyOrder = new DataFieldOrderExpression(new DataFieldInfo(fieldMapping), OrderType.ASC);
                            order = OrderExpression.Catch(order, keyOrder);
                        }
                        CommandData command = base.CreateSelectBaseCommand(mapping, customSelect, query, order, null, state);
                        command.CommandText = string.Format("{0} offset {1} row fetch next {2} rows only", command.CommandText, region.Start, region.Size);
                        command.InnerPage = true;
                        return command;
                    }
                }
            }
            return base.CreateSelectBaseCommand(mapping, customSelect, query, order, region, state);
        }

        public override CommandData CreateSelectJoinTableBaseCommand(string customSelect, List<IJoinModel> modelList, QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            if (region != null && region.Size > 0) {
                if (order != null || modelList.Exists(x => x.Order != null)) {
                    CommandData command = base.CreateSelectJoinTableBaseCommand(customSelect, modelList, query, order, null, state);
                    command.CommandText = string.Format("{0} offset {1} row fetch next {2} rows only", command.CommandText, region.Start, region.Size);
                    command.InnerPage = true;
                    return command;
                }
            }
            return base.CreateSelectJoinTableBaseCommand(customSelect, modelList, query, order, region, state);

        }

        public override CommandData CreateAggregateTableCommand(DataEntityMapping mapping, AggregateSelector selector, AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order, Region region, CreateSqlState state)
        {
            if (region != null && region.Size > 0) {
                if (order != null) {
                    CommandData command = base.CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, null, state);
                    command.CommandText = string.Format("{0} offset {1} row fetch next {2} rows only", command.CommandText, region.Start, region.Size);
                    command.InnerPage = true;
                    return command;
                } else {
                    if (groupBy != null && groupBy.FieldCount > 0) {
                        order = new DataFieldOrderExpression(groupBy[0], OrderType.ASC);
                    }
                    CommandData command = base.CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, null, state);
                    command.CommandText = string.Format("{0} offset {1} row fetch next {2} rows only", command.CommandText, region.Start, region.Size);
                    command.InnerPage = true;
                    return command;
                }
            }
            return base.CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, region, state);
        }
    }
}
