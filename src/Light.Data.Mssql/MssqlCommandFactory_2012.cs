using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data.Mssql
{
    internal class MssqlCommandFactory_2012 : MssqlCommandFactory_2008
    {
        public override CommandData CreateSelectBaseCommand(DataEntityMapping mapping, string customSelect,
            QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            if (region != null && region.Start > 0)
            {
                if (order == null)
                {
                    order = CreatePrimaryKeyOrderExpression(mapping);
                }

                if (order != null)
                {
                    var commandData = base.CreateSelectBaseCommand(mapping, customSelect, query, order, null, state);
                    commandData.CommandText =
                        $"{commandData.CommandText} offset {region.Start} row fetch next {region.Size} rows only";
                    commandData.InnerPage = true;
                    return commandData;
                }
            }

            return base.CreateSelectBaseCommand(mapping, customSelect, query, order, region, state);
        }

        public override CommandData CreateSelectJoinTableBaseCommand(string customSelect, List<IJoinModel> modelList,
            QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            if (region != null && region.Start > 0)
            {
                if (order != null)
                {
                    var command =
                        base.CreateSelectJoinTableBaseCommand(customSelect, modelList, query, order, null, state);
                    command.CommandText =
                        $"{command.CommandText} offset {region.Start} row fetch next {region.Size} rows only";
                    command.InnerPage = true;
                    return command;
                }
            }

            return base.CreateSelectJoinTableBaseCommand(customSelect, modelList, query, order, region, state);
        }

        public override CommandData CreateAggregateTableCommand(DataEntityMapping mapping, AggregateSelector selector,
            AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order,
            Region region, CreateSqlState state)
        {
            if (region != null && region.Start > 0)
            {
                if (order == null)
                {
                    order = CreateGroupByOrderExpression(groupBy);
                }

                if (order != null)
                {
                    var command = base.CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order,
                        null, state);
                    command.CommandText =
                        $"{command.CommandText} offset {region.Start} row fetch next {region.Size} rows only";
                    command.InnerPage = true;
                    return command;
                }
            }

            return base.CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, region, state);
        }
    }
}