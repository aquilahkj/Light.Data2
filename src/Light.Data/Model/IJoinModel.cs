using System;
namespace Light.Data
{
    interface IJoinModel
    {
        JoinConnect Connect {
            get;
        }

        IJoinTableMapping JoinMapping {
            get;
        }

        string AliasTableName {
            get;
        }

        OrderExpression Order {
            get;
        }

        string CreateSqlString(CommandFactory factory, CreateSqlState state);
    }
}

