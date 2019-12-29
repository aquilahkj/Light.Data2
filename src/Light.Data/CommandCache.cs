using System.Collections.Generic;

namespace Light.Data
{
    internal class CommandCache
    {
        public static string CreateKey(DataTableEntityMapping mapping, CreateSqlState state)
        {
            state.TryGetAliasTableName(mapping, out var aliasName);
            var type = mapping.ObjectType;
            var name = aliasName == null ? type.FullName : string.Concat(type.FullName, "_" , aliasName);
            return name;
        }

        private readonly Dictionary<string, string> commandDict = new Dictionary<string, string>();

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
    }
}
