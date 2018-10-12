using System.Collections.Generic;

namespace Light.Data.Template
{
    public static class DbSetting
    {
        static bool defaultValueStringMode = false;
        static Dictionary<string, string> specifiedDict = new Dictionary<string, string>();
        static Dictionary<string, string> defaultValueDict = new Dictionary<string, string>();
        static Dictionary<string, string> controlDict = new Dictionary<string, string>();
        static HashSet<string> tableHash = new HashSet<string>();
        static Dictionary<string, string> aliasTableDict = new Dictionary<string, string>();
        static Dictionary<string, string> aliasFieldDict = new Dictionary<string, string>();
        static HashSet<string> notnullFieldHash = new HashSet<string>();
        static HashSet<string> entityHash = new HashSet<string>();
        static HashSet<string> ignoreFieldHash = new HashSet<string>();

        public static void Initial()
        {
            defaultValueStringMode = false;
            specifiedDict.Clear();
            defaultValueDict.Clear();
            controlDict.Clear();
            tableHash.Clear();
            aliasTableDict.Clear();
            notnullFieldHash.Clear();
            entityHash.Clear();
            ignoreFieldHash.Clear();
            aliasFieldDict.Clear();
        }

        public static void SetAliasField(string fieldName, string aliasName)
        {
            aliasFieldDict[fieldName] = aliasName;
        }

        public static bool GetAliasField(string fieldName, out string aliasName)
        {
            return aliasFieldDict.TryGetValue(fieldName, out aliasName);
        }

        public static void SetSpecifiedType(string fieldName, string specifiedType)
        {
            specifiedDict[fieldName] = specifiedType;
        }

        public static bool GetSpecifiedType(string fieldName, out string specifiedType)
        {
            return specifiedDict.TryGetValue(fieldName, out specifiedType);
        }

        public static void SetDefaultValue(string fieldName, string value)
        {
            defaultValueDict[fieldName] = value;
        }

        public static bool GetDefaultValue(string fieldName, out string value)
        {
            return defaultValueDict.TryGetValue(fieldName, out value);
        }

        public static void SetReadControl(string fieldName)
        {
            controlDict[fieldName] = "Read";
        }

        public static void SetCreateControl(string fieldName)
        {
            controlDict[fieldName] = "Create";
        }

        public static void SetUpdateControl(string fieldName)
        {
            controlDict[fieldName] = "Update";
        }

        public static bool GetControl(string fieldName, out string value)
        {
            return controlDict.TryGetValue(fieldName, out value);
        }

        public static void SetTable(string tableName)
        {
            tableHash.Add(tableName);
        }

        public static void SetTable(string tableName, string aliasName)
        {
            aliasTableDict[aliasName] = tableName;
        }

        public static bool CheckTable(string tableName)
        {
            if (tableHash.Count > 0) {
                return tableHash.Contains(tableName);
            }
            else {
                return true;
            }
        }

        public static TableNameSet[] GetTables()
        {
            List<TableNameSet> list = new List<TableNameSet>();
            foreach (var item in tableHash) {
                TableNameSet ts = new TableNameSet();
                ts.TableName = item;
                ts.AliasName = item;
                list.Add(ts);
            }
            foreach (var kvs in aliasTableDict) {
                TableNameSet ts = new TableNameSet();
                ts.TableName = kvs.Value;
                ts.AliasName = kvs.Key;
                list.Add(ts);
            }
            return list.ToArray();
        }

        public static void SetIgnoreField(string fieldName)
        {
            ignoreFieldHash.Add(fieldName);
        }

        public static bool CheckIgnoreField(string fieldName)
        {
            return ignoreFieldHash.Contains(fieldName);
        }

        public static void SetNotNullField(string fieldName)
        {
            notnullFieldHash.Add(fieldName);
        }

        public static bool CheckNotNullField(string fieldName)
        {
            return notnullFieldHash.Contains(fieldName);
        }

        public static void SetEntityTable(string tableName)
        {
            tableHash.Add(tableName);
            entityHash.Add(tableName);
        }

        public static void SetEntityTable(string tableName, string aliasName)
        {
            aliasTableDict[aliasName] = tableName;
            entityHash.Add(aliasName);
        }

        public static bool CheckEntity(string tableName)
        {
            return entityHash.Contains(tableName);
        }

        public static bool HasEntityTable {
            get {
                return entityHash.Count > 0;
            }
        }

        public static bool GetDefaultValueStringMode()
        {
            return defaultValueStringMode;
        }

        public static void SetDefaultValueStringMode(bool mode)
        {
            defaultValueStringMode = mode;
        }
    }
}