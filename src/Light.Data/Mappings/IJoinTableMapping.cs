using System;
using System.Data;

namespace Light.Data
{
    interface IJoinTableMapping
    {
        Type ObjectType {
            get;
        }

        object InitialData();

        object LoadAliasJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string aliasName);
    }
}

