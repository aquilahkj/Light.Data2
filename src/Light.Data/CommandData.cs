using System;
using System.Data;
using System.Data.Common;

namespace Light.Data
{
    class CommandData
    {
        string commandText;

        bool returnRowCount = true;

        bool innerPage;

        public string CommandText {
            get {
                return commandText;
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException(nameof(value));
                }
                commandText = value;
            }
        }

        public CommandData(string commandText)
        {
            this.commandText = commandText;
        }

        CommandType commandType = CommandType.Text;

        public CommandType CommandType {
            get {
                return commandType;
            }
            set {
                commandType = value;
            }
        }

        bool transParamName;

        public bool TransParamName {
            get {
                return transParamName;
            }
            set {
                transParamName = value;
            }
        }

        public bool InnerPage {
            get {
                return innerPage;
            }

            set {
                innerPage = value;
            }
        }

        public bool ReturnRowCount {
            get {
                return returnRowCount;
            }

            set {
                returnRowCount = value;
            }
        }

        public DbCommand CreateCommand(DatabaseProvider database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            string sql = this.commandText;
            DbCommand command = database.CreateCommand(sql);
            command.CommandType = this.commandType;
            return command;
        }

        public DbCommand CreateCommand(DatabaseProvider database, CreateSqlState state)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            IDataParameter[] idataParameters = null;
            string sql = this.commandText;
            DataParameter[] dps = state.GetDataParameters();
            int length = dps.Length;
            if (length > 0) {
                idataParameters = new IDataParameter[length];
                for (int i = 0; i < length; i++) {
                    DataParameter dp = dps[i];
                    IDataParameter idp = database.CreateParameter(dp.ParameterName, dp.Value, dp.DbType, (ParameterDirection)dp.Direction, dp.DataType);
                    idataParameters[i] = idp;
                }
            }
            DbCommand command = database.CreateCommand(sql);
            command.CommandType = this.commandType;
            if (idataParameters != null) {
                foreach (IDataParameter param in idataParameters) {
                    command.Parameters.Add(param);
                }
            }
            return command;
        }
    }
}

