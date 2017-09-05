using System;
using System.Data;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Light.Data
{
    /// <summary>
    /// Command output.
    /// </summary>
    public class CommandOutput : ICommandOutput
    {
        /// <summary>
        /// Occurs when on command output.
        /// </summary>
        public event CommandOutputEventHandle OnCommandOutput;

        bool outputFullCommand;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandOutput"/> output runnable command.
        /// </summary>
        /// <value><c>true</c> if output runnable command; otherwise, <c>false</c>.</value>
        public bool OutputFullCommand {
            get {
                return outputFullCommand;
            }
            set {
                outputFullCommand = value;
            }
        }

        bool useConsoleOutput;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandOutput"/> use console output.
        /// </summary>
        /// <value><c>true</c> if use console output; otherwise, <c>false</c>.</value>
        public bool UseConsoleOutput {
            get {
                return useConsoleOutput;
            }
            set {
                useConsoleOutput = value;
            }
        }

        bool enable;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandOutput"/> is enable.
        /// </summary>
        /// <value><c>true</c> if enable; otherwise, <c>false</c>.</value>
        public bool Enable {
            get {
                return enable;
            }
            set {
                enable = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandOutput"/> class.
        /// </summary>
        public CommandOutput() : this(true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandOutput"/> class.
        /// </summary>
        /// <param name="defaultEnable">If set to <c>true</c> default enable.</param>
        public CommandOutput(bool defaultEnable)
        {
            this.enable = defaultEnable;
        }

        #region ICommandOutput implementation


        void ICommandOutput.Output(CommandOutputInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (this.enable && (OnCommandOutput != null || this.useConsoleOutput)) {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("action\t:" + info.Action);
                sb.AppendLine("type\t:" + info.CommandType);
                sb.AppendLine("level\t:" + info.Level);
                sb.AppendLine("region\t:" + info.Start + "," + info.Size);
                sb.AppendLine("time\t:" + info.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                sb.AppendLine("span\t:" + (info.EndTime - info.StartTime).TotalMilliseconds);
                sb.AppendLine("trans\t:" + (info.IsTransaction ? "true" : "false"));
                sb.AppendLine("success\t:" + (info.Success ? "true" : "false"));
                if (info.Success) {
                    sb.AppendLine("result\t:" + (info.Result != null ? info.Result.ToString() : "null"));
                }
                else {
                    sb.AppendLine("error\t:" + info.ExceptionMessage);
                }
                var datas = info.Datas;
                var command = info.Command;
                if (datas != null && datas.Length > 0) {
                    sb.AppendLine("params\t:");
                    foreach (IDataParameter data in datas) {
                        sb.AppendLine(string.Format("  {2},{3},{0}={1}", data.ParameterName, data.Value, data.Direction, data.DbType));
                    }
                }
                sb.AppendLine("command\t:");
                sb.Append("  ");
                sb.AppendLine(command);
                string commandInfo = sb.ToString();
                string runnableCommand = null;
                if (this.outputFullCommand) {
                    if (datas != null && datas.Length > 0) {
                        string temp = command;
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        List<string> patterns = new List<string>();
                        foreach (IDataParameter data in datas) {
                            string value;
                            if (data.Value == null) {
                                value = "null";
                            }
                            else {
                                TypeCode code = Type.GetTypeCode(data.Value.GetType());
                                Type type = data.Value.GetType();
                                if (code == TypeCode.Empty) {
                                    value = "null";
                                }
                                else if (type.GetTypeInfo().IsEnum) {
                                    value = Convert.ToInt64(data.Value).ToString();
                                }
                                else if (code == TypeCode.String || code == TypeCode.Char) {
                                    string content = data.Value.ToString();
                                    content = content.Replace("'", "''");
                                    value = string.Concat("'", content, "'");
                                }
                                else if (code == TypeCode.DateTime) {
                                    DateTime dt = (DateTime)data.Value;
                                    value = string.Concat("'", dt.ToString("yyyy-MM-dd HH:mm:ss"), "'");
                                }
                                else {
                                    value = data.Value.ToString();
                                }
                            }
                            dict[data.ParameterName] = value;
                            string pname = data.ParameterName.Replace("?", "\\?");
                            patterns.Add(pname + "\\b");
                        }
                        Regex regex = new Regex(string.Join("|", patterns), RegexOptions.Compiled);
                        temp = regex.Replace(temp, x => {
                            if (dict.TryGetValue(x.Value, out string data)) {
                                return data;
                            }
                            else {
                                return x.Value;
                            }
                        });
                        runnableCommand = temp;
                    }
                    else {
                        runnableCommand = command;
                    }
                }

                if (OnCommandOutput != null) {
                    CommandOutputEventArgs args = new CommandOutputEventArgs();
                    args.CommandInfo = commandInfo;
                    args.RunnableCommand = runnableCommand;
                    OnCommandOutput(this, args);
                }
                if (this.useConsoleOutput) {
                    if (runnableCommand != null) {
                        sb.AppendLine("--------------------");
                        sb.Append("  ");
                        sb.AppendLine(runnableCommand);
                    }
                    Console.WriteLine(sb);
                }
            }
        }

        #endregion
    }
}

