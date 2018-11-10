using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Light.Data.Mysql.Test
{
    public class Mysql_BaseConfigTest : BaseTest
    {
        public Mysql_BaseConfigTest(ITestOutputHelper output) : base(output)
        {

        }
        #region base test
        List<TeRelateMainConfig> CreateMainTableList(int count)
        {
            List<TeRelateMainConfig> list = new List<TeRelateMainConfig>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeRelateMainConfig item = new TeRelateMainConfig();
                item.DecimalField = (decimal)((i % 26) * 0.1 * x);
                item.DateTimeField = d.AddMinutes(i * 2);
                item.VarcharField = "testtest" + item.DecimalField;
                list.Add(item);
            }
            return list;
        }

        List<TeRelateMainConfig> CreateAndInsertMainTableList(int count)
        {
            var list = CreateMainTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateMainConfig>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        List<TeRelateMainExtendConfig> CreateMainTableListExtend(int count)
        {
            List<TeRelateMainExtendConfig> list = new List<TeRelateMainExtendConfig>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeRelateMainExtendConfig item = new TeRelateMainExtendConfig();
                item.DecimalField = (decimal)((i % 26) * 0.1 * x);
                item.DateTimeField = d.AddMinutes(i * 2);
                item.VarcharField = "testtest" + item.DecimalField;
                list.Add(item);
            }
            return list;
        }

        List<TeRelateMainExtendConfig> CreateAndInsertMainTableListExtend(int count)
        {
            var list = CreateMainTableListExtend(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateMainExtendConfig>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        List<TeRelateSubConfig> CreateSubTableList(int count)
        {
            List<TeRelateSubConfig> list = new List<TeRelateSubConfig>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeRelateSubConfig item = new TeRelateSubConfig();
                item.MainId = i;
                item.DecimalField = (decimal)((i % 26) * 0.1 * x);
                item.DateTimeField = d.AddMinutes(i * 2);
                item.VarcharField = "testtest" + item.DecimalField;
                list.Add(item);
            }
            return list;
        }

        List<TeRelateSubConfig> CreateAndInsertSubTableList(int count)
        {
            var list = CreateSubTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateSubConfig>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }


        [Fact]
        public void TestCase_Config()
        {
            context.TruncateTable<TeBaseFieldConfig>();
            var item = context.CreateNew<TeBaseFieldConfig>();
            var ret = context.Insert(item);
            Assert.Equal(1, ret);
            var ac = context.Query<TeBaseFieldConfig>().First();

            Assert.Equal(1, ac.Id);
            Assert.Equal(0, ac.Int32Field);
            Assert.Equal(20, ac.Int32FieldNull);
            Assert.Equal(0m, ac.DecimalField);
            Assert.Equal(20.5m, ac.DecimalFieldNull);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeField);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeFieldNull);
            Assert.Equal("", ac.VarcharField);
            Assert.Equal("testtest", ac.VarcharFieldNull);
            Assert.True((DateTime.Now - ac.NowField).Seconds <= 1);
            Assert.True((DateTime.Now - ac.NowFieldNull.Value).Seconds <= 1);
            Assert.Equal(DateTime.Now.Date, ac.TodayField);
            Assert.Equal(DateTime.Now.Date, ac.TodayFieldNull);
            Assert.Equal(EnumInt32Type.Zero, ac.EnumInt32Field);
            Assert.Equal(EnumInt32Type.Positive1, ac.EnumInt32FieldNull);
        }


        [Fact]
        public void TestCase_Config_Replace()
        {
            context.TruncateTable<TeBaseFieldConfigReplace>();
            var item = context.CreateNew<TeBaseFieldConfigReplace>();
            var ret = context.Insert(item);
            Assert.Equal(1, ret);
            var ac = context.Query<TeBaseFieldConfigReplace>().First();

            Assert.Equal(1, ac.Id);
            Assert.Equal(0, ac.Int32Field);
            Assert.Equal(30, ac.Int32FieldNull);
            Assert.Equal(0m, ac.DecimalField);
            Assert.Equal(30.5m, ac.DecimalFieldNull);
            Assert.Equal(new DateTime(2017, 1, 3, 12, 0, 0), ac.DateTimeField);
            Assert.Equal(new DateTime(2017, 1, 3, 12, 0, 0), ac.DateTimeFieldNull);
            Assert.Equal("", ac.VarcharField);
            Assert.Equal("testtest2", ac.VarcharFieldNull);
            Assert.True((DateTime.Now - ac.NowField).Seconds <= 1);
            Assert.True((DateTime.Now - ac.NowFieldNull.Value).Seconds <= 1);
            Assert.Equal(DateTime.Now.Date, ac.TodayField);
            Assert.Equal(DateTime.Now.Date, ac.TodayFieldNull);
            Assert.Equal(EnumInt32Type.Zero, ac.EnumInt32Field);
            Assert.Equal(EnumInt32Type.Positive2, ac.EnumInt32FieldNull);
        }
        

        [Fact]
        public void TestCase_Config_Relate()
        {
            var listA = CreateAndInsertMainTableList(10);
            var listB = CreateAndInsertSubTableList(5);

            {
                var listEx = (from x in listA
                              join y in listB on x.Id equals y.MainId into ps
                              from p in ps.DefaultIfEmpty()
                              select new TeRelateMainConfig {
                                  Id = x.Id,
                                  DecimalField = x.DecimalField,
                                  DateTimeField = x.DateTimeField,
                                  VarcharField = x.VarcharField,
                                  SubConfig = p
                              }).OrderByDescending(x => x.Id).ToList();

                var listAc = context.Query<TeRelateMainConfig>().OrderByDescending(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }


        [Fact]
        public void TestCase_Config_Relate_Extend()
        {
            var listA = CreateAndInsertMainTableListExtend(10);
            var listB = CreateAndInsertSubTableList(5);

            {
                var listEx = (from x in listA
                              join y in listB on x.Id equals y.MainId into ps
                              from p in ps.DefaultIfEmpty()
                              select new TeRelateMainExtendConfig {
                                  Id = x.Id,
                                  DecimalField = x.DecimalField,
                                  DateTimeField = x.DateTimeField,
                                  VarcharField = x.VarcharField,
                                  SubConfig = p
                              }).OrderByDescending(x => x.Id).ToList();

                var listAc = context.Query<TeRelateMainExtendConfig>().OrderByDescending(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }
        #endregion

        [Fact]
        public void TestCase_Config1()
        {
            context.TruncateTable<MyConfig1>();
            var item = context.CreateNew<MyConfig1>();
            item.Int32FieldNull = 1000;
            var ret = context.Insert(item);
            Assert.Equal(1, ret);
            var ac = context.Query<MyConfig1>().First();

            Assert.Equal(1, ac.Id);
            Assert.Equal(0, ac.Int32Field);
            Assert.Null(ac.Int32FieldNull);
            Assert.Equal(0m, ac.DecimalField);
            Assert.Equal(20.5m, ac.DecimalFieldNull);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeField);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeFieldNull);
            Assert.Equal("", ac.VarcharField);
            Assert.Equal("testtest", ac.VarcharFieldNull);
            Assert.True((DateTime.Now - ac.NowField).Seconds <= 1);
            Assert.True((DateTime.Now - ac.NowFieldNull.Value).Seconds <= 1);
            Assert.Equal(DateTime.Now.Date, ac.TodayField);
            Assert.Equal(DateTime.Now.Date, ac.TodayFieldNull);
            Assert.Equal(EnumInt32Type.Zero, ac.EnumInt32Field);
            Assert.Equal(EnumInt32Type.Positive1, ac.EnumInt32FieldNull);

            ac.Int32FieldNull = 1000;
            ac.NowField = DateTime.Now.AddDays(-1).Date;
            ret = context.Update(ac);
            Assert.Equal(1, ret);
            ac = context.Query<MyConfig1>().First();
            Assert.Equal(1, ac.Id);
            Assert.Equal(0, ac.Int32Field);
            Assert.Null(ac.Int32FieldNull);
            Assert.Equal(0m, ac.DecimalField);
            Assert.Equal(20.5m, ac.DecimalFieldNull);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeField);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeFieldNull);
            Assert.Equal("", ac.VarcharField);
            Assert.Equal("testtest", ac.VarcharFieldNull);
            Assert.True((DateTime.Now - ac.NowField).Seconds <= 1);
            Assert.True((DateTime.Now - ac.NowFieldNull.Value).Seconds <= 1);
            Assert.Equal(DateTime.Now.Date, ac.TodayField);
            Assert.Equal(DateTime.Now.Date, ac.TodayFieldNull);
            Assert.Equal(EnumInt32Type.Zero, ac.EnumInt32Field);
            Assert.Equal(EnumInt32Type.Positive1, ac.EnumInt32FieldNull);

        }

        [Fact]
        public void TestCase_Config_DbType()
        {
            context.TruncateTable<MyConfig2>();
            var item = context.CreateNew<MyConfig2>();
            item.Int32FieldNull = 1000;
            item.VarcharField = "11111111111111111111111111";
            var ret = context.Insert(item);
            Assert.Equal(1, ret);
            var ac = context.Query<MyConfig2>().First();
            Assert.Equal(1, ac.Id);
            Assert.Equal(1000, ac.Int32FieldNull);
            Assert.Equal("111111", ac.VarcharField);
        }
    }
}
