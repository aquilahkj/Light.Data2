using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
// ReSharper disable All

namespace Light.Data.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Test2(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }


        static void Test(string[] args)
        {
            var times = 10;
            if (args.Length > 0)
            {
                times = Convert.ToInt32(args[0]);
            }

            var round = 5;
            Console.WriteLine($"Run {times} times, {round} rounds.");
            var context1 = new DataContext();
            context1.TruncateTable<TeBaseField>();
            var command = new CommandOutput();
            command.Enable = true;
            command.OutputFullCommand = true;
            command.UseConsoleOutput = true;

            var datalist = new List<TeBaseField>();
            {
                Console.WriteLine("Run Insert");
                var d1 = DateTime.Now;
                var count = 0;
                var list = CreateBaseFieldTableList(times);
                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    var context = new DataContext();
                    //DateTime d2 = DateTime.Now;
                    count += context.Insert(item);
                    //Console.WriteLine($"times {i + 1}, {count} records insert to database, spend time {(DateTime.Now - d2).TotalSeconds} second");
                    datalist.Add(item);
                }
            
                Console.WriteLine(
                    $"insert times {times}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }
            
            {
                Console.WriteLine("Run Read");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < times; i++)
                {
                    var item = datalist[i];
                    var context = new DataContext();
                    //DateTime d2 = DateTime.Now;
                    item.BoolField = !item.BoolField;
                    item.DateTimeField = DateTime.Now;
                    var rs = context.Query<TeBaseField>().Where(x => x.Id == item.Id).First();
                    if (rs != null)
                        count += 1;
                    //Console.WriteLine($"times {i + 1}, {count} records update to database, spend time {(DateTime.Now - d2).TotalSeconds} second");
                }
            
                Console.WriteLine(
                    $"read times {times}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }
            
            {
                Console.WriteLine("Run Update");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < times; i++)
                {
                    var item = datalist[i];
                    var context = new DataContext();
                    //DateTime d2 = DateTime.Now;
                    item.BoolField = !item.BoolField;
                    item.DateTimeField = DateTime.Now;
                    count += context.Update(item);
                    //Console.WriteLine($"times {i + 1}, {count} records update to database, spend time {(DateTime.Now - d2).TotalSeconds} second");
                }
            
                Console.WriteLine(
                    $"update times {times}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }

            {
                Console.WriteLine("Run Update Lambda");
                var d1 = DateTime.Now;
                var count = 0;
                var context = new DataContext();
                context.SetCommandOutput(command);
                var d = 1;
                context.Query<TeBaseField>().Where(x => x.BoolField).ToList();
                context.Query<TeBaseField>().Where(x => !x.BoolField).ToList();
                count = context.Query<TeBaseField>().Where(x => x.Id == 1).Update(x => new TeBaseField
                {
                    BoolField = !x.BoolField,
                    Int32Field = x.Int32Field + 1,
                    VarcharFieldNull = d > 1 ? "22" : "33"
                });

                Console.WriteLine(
                    $"update lambda, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }

            {
                Console.WriteLine("Run Delete");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < times; i++)
                {
                    var item = datalist[i];
                    var context = new DataContext();
                    //DateTime d2 = DateTime.Now;
                    count += context.Delete(item);
                    //Console.WriteLine($"times {i + 1}, {count} records update to database, spend time {(DateTime.Now - d2).TotalSeconds} second");
                }

                Console.WriteLine(
                    $"delete times {times}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }

            var totalList = new List<TeBaseField>();
            {
                var temp = new List<TeBaseField>();
                Console.WriteLine("Run Batch Insert");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < round; i++)
                {
                    var list = CreateBaseFieldTableList(times);
                    var context = new DataContext();
                    var d2 = DateTime.Now;
                    var res = context.BatchInsert(list);
                    count += res;
                    Console.WriteLine(
                        $"insert round {i + 1}, item {res}, spend time {(DateTime.Now - d2).TotalSeconds} second");
                    temp.AddRange(list);
                }

                Console.WriteLine(
                    $"batch insert round {round}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
                totalList = Random(temp);
                //totalList = context1.Query<TeBaseField>().ToList();
                //totalList = Random(totalList);
            }

            {
                var minid = times + 1;
                Console.WriteLine("Run Range Read");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < round; i++)
                {
                    var context = new DataContext();
                    var t1 = DateTime.Now;
                    var start = minid + i * times;
                    var list = context.Query<TeBaseField>().Where(x => x.Id >= start && x.Id < start + times).ToList();
                    Console.WriteLine(
                        $"read round {i + 1}, item {list.Count}, spend time {(DateTime.Now - t1).TotalSeconds} second");
                    count += list.Count;
                }

                Console.WriteLine(
                    $"range read round {round}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }

            {
                Console.WriteLine("Run Batch Update");
                totalList.ForEach(x =>
                {
                    x.BoolField = !x.BoolField;
                    x.DateTimeField = DateTime.Now;
                });
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < round; i++)
                {
                    var list = Range(totalList, i * times, times);
                    var context = new DataContext();
                    //context.SetCommandOutput(command);
                    var t1 = DateTime.Now;
                    var ret = context.BatchUpdate(list);
                    count += ret;
                    Console.WriteLine(
                        $"update round {i + 1}, item {ret}, spend time {(DateTime.Now - t1).TotalSeconds} second");
                }

                Console.WriteLine(
                    $"batch update round {round}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }

            {
                Console.WriteLine("Run Batch Delete");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < round; i++)
                {
                    var list = Range(totalList, i * times, times);
                    var context = new DataContext();
                    var t1 = DateTime.Now;
                    var ret = context.BatchDelete(list);
                    count += ret;
                    Console.WriteLine(
                        $"delete round {i + 1}, item {ret}, spend time {(DateTime.Now - t1).TotalSeconds} second");
                }

                Console.WriteLine(
                    $"batch delete round {round}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }
        }

        static void Test2(string[] args)
        {
            var times = 10;
            if (args.Length > 0)
            {
                times = Convert.ToInt32(args[0]);
            }

            var round = 5;
            Console.WriteLine($"Run {times} times, {round} rounds.");
            var context1 = new DataContext();
            context1.TruncateTable<TeBaseField2>();
            var command = new CommandOutput();
            command.Enable = true;
            command.OutputFullCommand = true;
            command.UseConsoleOutput = true;

            var datalist = new List<TeBaseField2>();
            {
                Console.WriteLine("Run Insert");
                var d1 = DateTime.Now;
                var count = 0;
                var list = CreateBaseFieldTableList2(times);
                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    var context = new DataContext();
                    //DateTime d2 = DateTime.Now;
                    count += context.Insert(item);
                    //Console.WriteLine($"times {i + 1}, {count} records insert to database, spend time {(DateTime.Now - d2).TotalSeconds} second");
                    datalist.Add(item);
                }
            
                Console.WriteLine(
                    $"insert times {times}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }
            
            {
                Console.WriteLine("Run Read");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < times; i++)
                {
                    var item = datalist[i];
                    var context = new DataContext();
                    //DateTime d2 = DateTime.Now;
                    item.BoolField = !item.BoolField;
                    item.DateTimeField = DateTime.Now;
                    var rs = context.Query<TeBaseField2>().Where(x => x.Id == item.Id).First();
                    if (rs != null)
                        count += 1;
                    //Console.WriteLine($"times {i + 1}, {count} records update to database, spend time {(DateTime.Now - d2).TotalSeconds} second");
                }
            
                Console.WriteLine(
                    $"read times {times}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }
            
            {
                Console.WriteLine("Run Update");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < times; i++)
                {
                    var item = datalist[i];
                    var context = new DataContext();
                    //DateTime d2 = DateTime.Now;
                    item.BoolField = !item.BoolField;
                    item.DateTimeField = DateTime.Now;
                    count += context.Update(item);
                    //Console.WriteLine($"times {i + 1}, {count} records update to database, spend time {(DateTime.Now - d2).TotalSeconds} second");
                }
            
                Console.WriteLine(
                    $"update times {times}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }

            // {
            //     Console.WriteLine("Run Update Lambda");
            //     var d1 = DateTime.Now;
            //     var count = 0;
            //     var context = new DataContext();
            //     context.SetCommandOutput(command);
            //     var d = 1;
            //     context.Query<TeBaseField2>().Where(x => x.BoolField).ToList();
            //     context.Query<TeBaseField2>().Where(x => !x.BoolField).ToList();
            //     count = context.Query<TeBaseField2>().Where(x => x.Id == 1).Update(x => new TeBaseField2
            //     {
            //         BoolField = !x.BoolField,
            //         Int32Field = x.Int32Field + 1,
            //         VarcharFieldNull = d > 1 ? "22" : "33"
            //     });
            //
            //     Console.WriteLine(
            //         $"update lambda, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            // }

            // {
            //     Console.WriteLine("Run Delete");
            //     var d1 = DateTime.Now;
            //     var count = 0;
            //     for (var i = 0; i < times; i++)
            //     {
            //         var item = datalist[i];
            //         var context = new DataContext();
            //         //DateTime d2 = DateTime.Now;
            //         count += context.Delete(item);
            //         //Console.WriteLine($"times {i + 1}, {count} records update to database, spend time {(DateTime.Now - d2).TotalSeconds} second");
            //     }
            //
            //     Console.WriteLine(
            //         $"delete times {times}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            // }

            var totalList = new List<TeBaseField>();
            {
                var temp = new List<TeBaseField>();
                Console.WriteLine("Run Batch Insert");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < round; i++)
                {
                    var list = CreateBaseFieldTableList(times);
                    var context = new DataContext();
                    var d2 = DateTime.Now;
                    var res = context.BatchInsert(list);
                    count += res;
                    Console.WriteLine(
                        $"insert round {i + 1}, item {res}, spend time {(DateTime.Now - d2).TotalSeconds} second");
                    temp.AddRange(list);
                }

                Console.WriteLine(
                    $"batch insert round {round}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
                totalList = Random(temp);
                //totalList = context1.Query<TeBaseField>().ToList();
                //totalList = Random(totalList);
            }

            {
                var minid = times + 1;
                Console.WriteLine("Run Range Read");
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < round; i++)
                {
                    var context = new DataContext();
                    var t1 = DateTime.Now;
                    var start = minid + i * times;
                    var list = context.Query<TeBaseField>().Where(x => x.Id >= start && x.Id < start + times).ToList();
                    Console.WriteLine(
                        $"read round {i + 1}, item {list.Count}, spend time {(DateTime.Now - t1).TotalSeconds} second");
                    count += list.Count;
                }

                Console.WriteLine(
                    $"range read round {round}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }

            {
                Console.WriteLine("Run Batch Update");
                totalList.ForEach(x =>
                {
                    x.BoolField = !x.BoolField;
                    x.DateTimeField = DateTime.Now;
                });
                var d1 = DateTime.Now;
                var count = 0;
                for (var i = 0; i < round; i++)
                {
                    var list = Range(totalList, i * times, times);
                    var context = new DataContext();
                    //context.SetCommandOutput(command);
                    var t1 = DateTime.Now;
                    var ret = context.BatchUpdate(list);
                    count += ret;
                    Console.WriteLine(
                        $"update round {i + 1}, item {ret}, spend time {(DateTime.Now - t1).TotalSeconds} second");
                }

                Console.WriteLine(
                    $"batch update round {round}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            }

            // {
            //     Console.WriteLine("Run Batch Delete");
            //     var d1 = DateTime.Now;
            //     var count = 0;
            //     for (var i = 0; i < round; i++)
            //     {
            //         var list = Range(totalList, i * times, times);
            //         var context = new DataContext();
            //         var t1 = DateTime.Now;
            //         var ret = context.BatchDelete(list);
            //         count += ret;
            //         Console.WriteLine(
            //             $"delete round {i + 1}, item {ret}, spend time {(DateTime.Now - t1).TotalSeconds} second");
            //     }
            //
            //     Console.WriteLine(
            //         $"batch delete round {round}, total item {count}, spend time {(DateTime.Now - d1).TotalSeconds} second");
            // }
        }


        static List<TeBaseField> Range(List<TeBaseField> list, int index, int size)
        {
            var target = new List<TeBaseField>();
            var end = index + size;
            var len = end > list.Count ? list.Count : end;
            for (var i = 0; i < len; i++)
            {
                if (i >= index && i < end)
                {
                    target.Add(list[i]);
                }
            }

            return target;
        }

        static List<TeBaseField> Random(List<TeBaseField> list)
        {
            var temp = new List<TeBaseField>(list);
            var rand = new Random();
            var target = new List<TeBaseField>(list);
            while (temp.Count > 0)
            {
                var index = rand.Next(0, temp.Count - 1);
                var item = temp[index];
                temp.RemoveAt(index);
                target.Add(item);
            }

            return target;
        }

        static List<TeBaseField> CreateBaseFieldTableList(int count)
        {
            var list = new List<TeBaseField>();
            var now = DateTime.Now;
            var d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (var i = 1; i <= count; i++)
            {
                var x = i % 5 == 0 ? -1 : 1;
                var item = new TeBaseField();
                item.BoolField = i % 3 == 0;
                item.BoolFieldNull = i % 2 == 0 ? null : (bool?) (item.BoolField);
                item.ByteField = (byte) (i % 256);
                item.ByteFieldNull = i % 2 == 0 ? null : (byte?) (item.ByteField);
                item.SbyteField = (sbyte) ((i % 128) * x);
                item.SbyteFieldNull = i % 2 == 0 ? null : (sbyte?) (item.SbyteField);
                item.Int16Field = (short) ((i % 20) * x);
                item.Int16FieldNull = i % 2 == 0 ? null : (short?) (item.Int16Field);
                item.Int32Field = (int) ((i % 23) * x);
                item.Int32FieldNull = i % 2 == 0 ? null : (int?) (item.Int32Field);
                item.Int64Field = (long) ((i % 25) * x);
                item.Int64FieldNull = i % 2 == 0 ? null : (long?) (item.Int64Field);
                item.UInt16Field = (ushort) (i % 27);
                item.UInt16FieldNull = i % 2 == 0 ? null : (ushort?) (item.UInt16Field);
                item.UInt32Field = (uint) (i % 28);
                item.UInt32FieldNull = i % 2 == 0 ? null : (uint?) (item.UInt32Field);
                item.UInt64Field = (ulong) (i % 31);
                item.UInt64FieldNull = i % 2 == 0 ? null : (ulong?) (item.UInt64Field);
                item.FloatField = (float) ((i % 19) * 0.1 * x);
                item.FloatFieldNull = i % 2 == 0 ? null : (float?) (item.FloatField);
                item.DoubleField = (double) ((i % 22) * 0.1 * x);
                item.DoubleFieldNull = i % 2 == 0 ? null : (double?) (item.DoubleField);
                item.DecimalField = (decimal) ((i % 26) * 0.1 * x);
                item.DecimalFieldNull = i % 2 == 0 ? null : (decimal?) (item.DecimalField);
                item.DateTimeField = d.AddMinutes(i * 2);
                item.DateTimeFieldNull = i % 2 == 0 ? null : (DateTime?) (item.DateTimeField);
                item.VarcharField = "testText" + item.Int32Field;
                item.VarcharFieldNull = i % 2 == 0 ? null : item.VarcharField;
                item.TextField = "textText" + item.Int32Field;
                item.TextFieldNull = i % 2 == 0 ? null : item.TextField;
                item.BigDataField = Encoding.UTF8.GetBytes(item.VarcharField);
                item.BigDataFieldNull = i % 2 == 0 ? null : item.BigDataField;
                list.Add(item);
            }

            return list;
        }
        
        static List<TeBaseField2> CreateBaseFieldTableList2(int count)
        {
            var list = new List<TeBaseField2>();
            var now = DateTime.Now;
            var d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (var i = 1; i <= count; i++)
            {
                var x = i % 5 == 0 ? -1 : 1;
                var item = new TeBaseField2();
                item.Id = Guid.NewGuid();
                item.BoolField = i % 3 == 0;
                item.BoolFieldNull = i % 2 == 0 ? null : (bool?) (item.BoolField);
                item.ByteField = (byte) (i % 256);
                item.ByteFieldNull = i % 2 == 0 ? null : (byte?) (item.ByteField);
                item.SbyteField = (sbyte) ((i % 128) * x);
                item.SbyteFieldNull = i % 2 == 0 ? null : (sbyte?) (item.SbyteField);
                item.Int16Field = (short) ((i % 20) * x);
                item.Int16FieldNull = i % 2 == 0 ? null : (short?) (item.Int16Field);
                item.Int32Field = (int) ((i % 23) * x);
                item.Int32FieldNull = i % 2 == 0 ? null : (int?) (item.Int32Field);
                item.Int64Field = (long) ((i % 25) * x);
                item.Int64FieldNull = i % 2 == 0 ? null : (long?) (item.Int64Field);
                item.UInt16Field = (ushort) (i % 27);
                item.UInt16FieldNull = i % 2 == 0 ? null : (ushort?) (item.UInt16Field);
                item.UInt32Field = (uint) (i % 28);
                item.UInt32FieldNull = i % 2 == 0 ? null : (uint?) (item.UInt32Field);
                item.UInt64Field = (ulong) (i % 31);
                item.UInt64FieldNull = i % 2 == 0 ? null : (ulong?) (item.UInt64Field);
                item.FloatField = (float) ((i % 19) * 0.1 * x);
                item.FloatFieldNull = i % 2 == 0 ? null : (float?) (item.FloatField);
                item.DoubleField = (double) ((i % 22) * 0.1 * x);
                item.DoubleFieldNull = i % 2 == 0 ? null : (double?) (item.DoubleField);
                item.DecimalField = (decimal) ((i % 26) * 0.1 * x);
                item.DecimalFieldNull = i % 2 == 0 ? null : (decimal?) (item.DecimalField);
                item.DateTimeField = d.AddMinutes(i * 2);
                item.DateTimeFieldNull = i % 2 == 0 ? null : (DateTime?) (item.DateTimeField);
                item.VarcharField = "testText" + item.Int32Field;
                item.VarcharFieldNull = i % 2 == 0 ? null : item.VarcharField;
                item.TextField = "textText" + item.Int32Field;
                item.TextFieldNull = i % 2 == 0 ? null : item.TextField;
                item.BigDataField = Encoding.UTF8.GetBytes(item.VarcharField);
                item.BigDataFieldNull = i % 2 == 0 ? null : item.BigDataField;
                list.Add(item);
            }

            return list;
        }
    }
}