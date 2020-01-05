using System;
using System.Data;
using System.Data.Common;

namespace Light.Data
{
    internal class CommandData
    {
        private string commandText;

        public string CommandText
        {
            get => commandText;
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

        public CommandType CommandType { get; } = CommandType.Text;

        public bool InnerPage { get; set; }

        public bool IdentitySql { get; set; }

        public DbCommand CreateCommand(DatabaseProvider database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            var sql = commandText;
            var command = database.CreateCommand(sql);
            command.CommandType = CommandType;
            return command;
        }

        public DbCommand CreateCommand(DatabaseProvider database, CreateSqlState state)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            IDataParameter[] parameters = null;
            var sql = commandText;
            var dps = state.GetDataParameters();
            var length = dps.Length;
            if (length > 0) {
                parameters = new IDataParameter[length];
                for (var i = 0; i < length; i++) {
                    var dp = dps[i];
                    var parameter = dp.ConvertDbParameter(database, CommandType.Text);
                    parameters[i] = parameter;
                }
            }
            var command = database.CreateCommand(sql);
            command.CommandType = CommandType;
            if (parameters != null) {
                foreach (var param in parameters) {
                    command.Parameters.Add(param);
                }
            }
            return command;
        }
    }
}

