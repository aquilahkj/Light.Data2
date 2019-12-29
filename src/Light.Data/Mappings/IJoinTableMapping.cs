using System;
using System.Data;

namespace Light.Data
{
    internal interface IJoinTableMapping
    {
        Type ObjectType {
            get;
        }

        object InitialData();

        object LoadAliasJoinTableData(DataContext context, IDataReader dataReader, QueryState queryState, string aliasName);
    }
}

