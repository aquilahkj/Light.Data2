using System.Collections.Generic;

namespace Light.Data
{
    internal class CommandCache
    {

        //public CommandCache(string command)
        //{
        //    _command = command; ;
        //}

        //private string _command;

        //public string Command {
        //    get {
        //        return _command;
        //    }
        //}



        public static string CreateKey(DataTableEntityMapping mapping, CreateSqlState state)
        {
            string name;
            state.TryGetAliasTableName(mapping, out var aliasName);
            var type = mapping.ObjectType;
            if (aliasName == null) {
                name = type.FullName;
            }
            else {
                name = string.Concat(type.FullName, "_" , aliasName);
            }
            return name;
        }

        private Dictionary<string, string> commandDict = new Dictionary<string, string>();

        public bool TryGetCommand(string name, out string command)
        {
            return commandDict.TryGetValue(name, out command);
        }

        public void SetCommand(string name, string command)
        {
            lock (this) {
                commandDict[name] = command;
            }
        }

        //public void SetAliasCommand(string aliasName, string command)
        //{
        //    aliasCommand[aliasName] = command;
        //}
    }
}
