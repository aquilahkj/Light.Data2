using System;
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandOutput"/> output runnable command.
        /// </summary>
        /// <value><c>true</c> if output runnable command; otherwise, <c>false</c>.</value>
        public bool OutputFullCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandOutput"/> use console output.
        /// </summary>
        /// <value><c>true</c> if use console output; otherwise, <c>false</c>.</value>
        public bool UseConsoleOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandOutput"/> is enable.
        /// </summary>
        /// <value><c>true</c> if enable; otherwise, <c>false</c>.</value>
        public bool Enable { get; set; }

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
            Enable = defaultEnable;
        }

        #region ICommandOutput implementation


        void ICommandOutput.Output(CommandOutputInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (Enable && (OnCommandOutput != null || UseConsoleOutput)) {
                var sb = new StringBuilder();
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
                    foreach (var data in datas) {
                        sb.AppendLine(string.Format("  {2},{3},{0}={1}", data.ParameterName, data.Value, data.Direction, data.DbType));
                    }
                }
                sb.AppendLine("command\t:");
                sb.Append("  ");
                sb.AppendLine(command);
                var commandInfo = sb.ToString();
                string runnableCommand = null;
                if (OutputFullCommand) {
                    if (datas != null && datas.Length > 0) {
                        var temp = command;
                        var dict = new Dictionary<string, string>();
                        var patterns = new List<string>();
                        foreach (var data in datas) {
                            string value;
                            if (data.Value == null) {
                                value = "null";
                            }
                            else if (data.Value == DBNull.Value) {
                                value = "null";
                            }
                            else {
                                var code = Type.GetTypeCode(data.Value.GetType());
                                var type = data.Value.GetType();
                                if (code == TypeCode.Empty) {
                                    value = "null";
                                }
                                else if (type.GetTypeInfo().IsEnum) {
                                    value = Convert.ToInt64(data.Value).ToString();
                                }
                                else if (code == TypeCode.String || code == TypeCode.Char) {
                                    var content = data.Value.ToString();
                                    content = content.Replace("'", "''");
                                    value = string.Concat("'", content, "'");
                                }
                                else if (code == TypeCode.DateTime) {
                                    var dt = (DateTime)data.Value;
                                    value = string.Concat("'", dt.ToString("yyyy-MM-dd HH:mm:ss"), "'");
                                }
                                else if(data.Value is byte[]) {
                                    value = "[bytes]";
                                }
                                else {
                                    value = data.Value.ToString();
                                }
                            }
                            dict[data.ParameterName] = value;
                            var pname = data.ParameterName.Replace("?", "\\?");
                            patterns.Add(pname + "\\b");
                        }
                        var regex = new Regex(string.Join("|", patterns), RegexOptions.Compiled);
                        temp = regex.Replace(temp, x => {
                            if (dict.TryGetValue(x.Value, out var data)) {
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
                    var args = new CommandOutputEventArgs();
                    args.CommandInfo = commandInfo;
                    args.RunnableCommand = runnableCommand;
                    OnCommandOutput(this, args);
                }
                if (UseConsoleOutput) {
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

