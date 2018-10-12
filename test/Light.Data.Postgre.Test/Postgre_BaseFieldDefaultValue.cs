using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Light.Data.Postgre.Test
{
    public class Postgre_BaseFieldDefaultValue : BaseTest
    {
        public Postgre_BaseFieldDefaultValue(ITestOutputHelper output) : base(output)
        {
            DefaultTimeFunction.RemoveMillisecond = true;
        }

        #region base test
        [Fact]
        public void TestCase_MiniValue()
        {
            context.TruncateTable<TeBaseFieldNullMiniValue>();


            var value = context.CreateNew<TeBaseFieldNullMiniValue>();
            context.Insert(value);
            var ac = context.SelectById<TeBaseFieldNullMiniValue>(value.Id);

            Assert.Equal(1, ac.Id);
            Assert.Equal(false, ac.BoolFieldNull);
            Assert.Equal((byte)0, ac.ByteFieldNull);
            Assert.Equal((sbyte)0, ac.SbyteFieldNull);
            Assert.Equal((short)0, ac.Int16FieldNull);
            Assert.Equal(0, ac.Int32FieldNull);
            Assert.Equal(0L, ac.Int64FieldNull);
            Assert.Equal((ushort)0, ac.UInt16FieldNull);
            Assert.Equal(0u, ac.UInt32FieldNull);
            Assert.Equal(0uL, ac.UInt64FieldNull);
            Assert.Equal(0f, ac.FloatFieldNull);
            Assert.Equal(0d, ac.DoubleFieldNull);
            Assert.Equal(0m, ac.DecimalFieldNull);
            //Assert.Equal(DateTime.MinValue, ac.DateTimeFieldNull);
            Assert.Equal(string.Empty, ac.VarcharFieldNull);
            Assert.Equal(string.Empty, ac.TextFieldNull);
            Assert.Equal(new byte[0], ac.BigDataFieldNull);
            Assert.Equal(EnumInt32Type.Zero, ac.EnumInt32FieldNull);
            Assert.Equal(EnumInt64Type.Zero, ac.EnumInt64FieldNull);


            Assert.Equal(1, value.Id);
            Assert.Equal(null, value.BoolFieldNull);
            Assert.Equal(null, value.ByteFieldNull);
            Assert.Equal(null, value.SbyteFieldNull);
            Assert.Equal(null, value.Int16FieldNull);
            Assert.Equal(null, value.Int32FieldNull);
            Assert.Equal(null, value.Int64FieldNull);
            Assert.Equal(null, value.UInt16FieldNull);
            Assert.Equal(null, value.UInt32FieldNull);
            Assert.Equal(null, value.UInt64FieldNull);
            Assert.Equal(null, value.FloatFieldNull);
            Assert.Equal(null, value.DoubleFieldNull);
            Assert.Equal(null, value.DecimalFieldNull);
            Assert.Equal(null, value.DateTimeFieldNull);
            Assert.Equal(null, value.VarcharFieldNull);
            Assert.Equal(null, value.TextFieldNull);
            Assert.Equal(null, value.BigDataFieldNull);
            Assert.Equal(null, value.EnumInt32FieldNull);
            Assert.Equal(null, value.EnumInt64FieldNull);
        }

        [Fact]
        public async Task TestCase_MiniValue_Async()
        {
            await context.TruncateTableAsync<TeBaseFieldNullMiniValue>();


            var value = context.CreateNew<TeBaseFieldNullMiniValue>();
            await context.InsertAsync(value);
            var ac = await context.SelectByIdAsync<TeBaseFieldNullMiniValue>(value.Id);

            Assert.Equal(1, ac.Id);
            Assert.Equal(false, ac.BoolFieldNull);
            Assert.Equal((byte)0, ac.ByteFieldNull);
            Assert.Equal((sbyte)0, ac.SbyteFieldNull);
            Assert.Equal((short)0, ac.Int16FieldNull);
            Assert.Equal(0, ac.Int32FieldNull);
            Assert.Equal(0L, ac.Int64FieldNull);
            Assert.Equal((ushort)0, ac.UInt16FieldNull);
            Assert.Equal(0u, ac.UInt32FieldNull);
            Assert.Equal(0uL, ac.UInt64FieldNull);
            Assert.Equal(0f, ac.FloatFieldNull);
            Assert.Equal(0d, ac.DoubleFieldNull);
            Assert.Equal(0m, ac.DecimalFieldNull);
            //Assert.Equal(DateTime.MinValue, ac.DateTimeFieldNull);
            Assert.Equal(string.Empty, ac.VarcharFieldNull);
            Assert.Equal(string.Empty, ac.TextFieldNull);
            Assert.Equal(new byte[0], ac.BigDataFieldNull);
            Assert.Equal(EnumInt32Type.Zero, ac.EnumInt32FieldNull);
            Assert.Equal(EnumInt64Type.Zero, ac.EnumInt64FieldNull);


            Assert.Equal(1, value.Id);
            Assert.Equal(null, value.BoolFieldNull);
            Assert.Equal(null, value.ByteFieldNull);
            Assert.Equal(null, value.SbyteFieldNull);
            Assert.Equal(null, value.Int16FieldNull);
            Assert.Equal(null, value.Int32FieldNull);
            Assert.Equal(null, value.Int64FieldNull);
            Assert.Equal(null, value.UInt16FieldNull);
            Assert.Equal(null, value.UInt32FieldNull);
            Assert.Equal(null, value.UInt64FieldNull);
            Assert.Equal(null, value.FloatFieldNull);
            Assert.Equal(null, value.DoubleFieldNull);
            Assert.Equal(null, value.DecimalFieldNull);
            Assert.Equal(null, value.DateTimeFieldNull);
            Assert.Equal(null, value.VarcharFieldNull);
            Assert.Equal(null, value.TextFieldNull);
            Assert.Equal(null, value.BigDataFieldNull);
            Assert.Equal(null, value.EnumInt32FieldNull);
            Assert.Equal(null, value.EnumInt64FieldNull);
        }

        [Fact]
        public void TestCase_MiniValue_BulkInsert()
        {
            context.TruncateTable<TeBaseFieldNullMiniValue>();

            var list = new List<TeBaseFieldNullMiniValue>();
            for (int i = 0; i < 10; i++) {
                var value = context.CreateNew<TeBaseFieldNullMiniValue>();
                list.Add(value);
            }
            context.BatchInsert(list);
            var listAc = context.Query<TeBaseFieldNullMiniValue>().ToList();

            for (int i = 0; i < listAc.Count; i++) {
                var ac = listAc[i];
                Assert.Equal(i + 1, ac.Id);
                Assert.Equal(false, ac.BoolFieldNull);
                Assert.Equal((byte)0, ac.ByteFieldNull);
                Assert.Equal((sbyte)0, ac.SbyteFieldNull);
                Assert.Equal((short)0, ac.Int16FieldNull);
                Assert.Equal(0, ac.Int32FieldNull);
                Assert.Equal(0L, ac.Int64FieldNull);
                Assert.Equal((ushort)0, ac.UInt16FieldNull);
                Assert.Equal(0u, ac.UInt32FieldNull);
                Assert.Equal(0uL, ac.UInt64FieldNull);
                Assert.Equal(0f, ac.FloatFieldNull);
                Assert.Equal(0d, ac.DoubleFieldNull);
                Assert.Equal(0m, ac.DecimalFieldNull);
                //Assert.Equal(DateTime.MinValue, ac.DateTimeFieldNull);
                Assert.Equal(string.Empty, ac.VarcharFieldNull);
                Assert.Equal(string.Empty, ac.TextFieldNull);
                Assert.Equal(new byte[0], ac.BigDataFieldNull);
                Assert.Equal(EnumInt32Type.Zero, ac.EnumInt32FieldNull);
                Assert.Equal(EnumInt64Type.Zero, ac.EnumInt64FieldNull);

                var value = list[i];
                Assert.Equal(i + 1, value.Id);
                Assert.Equal(null, value.BoolFieldNull);
                Assert.Equal(null, value.ByteFieldNull);
                Assert.Equal(null, value.SbyteFieldNull);
                Assert.Equal(null, value.Int16FieldNull);
                Assert.Equal(null, value.Int32FieldNull);
                Assert.Equal(null, value.Int64FieldNull);
                Assert.Equal(null, value.UInt16FieldNull);
                Assert.Equal(null, value.UInt32FieldNull);
                Assert.Equal(null, value.UInt64FieldNull);
                Assert.Equal(null, value.FloatFieldNull);
                Assert.Equal(null, value.DoubleFieldNull);
                Assert.Equal(null, value.DecimalFieldNull);
                Assert.Equal(null, value.DateTimeFieldNull);
                Assert.Equal(null, value.VarcharFieldNull);
                Assert.Equal(null, value.TextFieldNull);
                Assert.Equal(null, value.BigDataFieldNull);
                Assert.Equal(null, value.EnumInt32FieldNull);
                Assert.Equal(null, value.EnumInt64FieldNull);
            }
        }

        [Fact]
        public async Task TestCase_MiniValue_BulkInsert_Async()
        {
            await context.TruncateTableAsync<TeBaseFieldNullMiniValue>();

            var list = new List<TeBaseFieldNullMiniValue>();
            for (int i = 0; i < 10; i++) {
                var value = context.CreateNew<TeBaseFieldNullMiniValue>();
                list.Add(value);
            }
            await context.BatchInsertAsync(list);
            var listAc = await context.Query<TeBaseFieldNullMiniValue>().ToListAsync();

            for (int i = 0; i < listAc.Count; i++) {
                var ac = listAc[i];
                Assert.Equal(i + 1, ac.Id);
                Assert.Equal(false, ac.BoolFieldNull);
                Assert.Equal((byte)0, ac.ByteFieldNull);
                Assert.Equal((sbyte)0, ac.SbyteFieldNull);
                Assert.Equal((short)0, ac.Int16FieldNull);
                Assert.Equal(0, ac.Int32FieldNull);
                Assert.Equal(0L, ac.Int64FieldNull);
                Assert.Equal((ushort)0, ac.UInt16FieldNull);
                Assert.Equal(0u, ac.UInt32FieldNull);
                Assert.Equal(0uL, ac.UInt64FieldNull);
                Assert.Equal(0f, ac.FloatFieldNull);
                Assert.Equal(0d, ac.DoubleFieldNull);
                Assert.Equal(0m, ac.DecimalFieldNull);
                //Assert.Equal(DateTime.MinValue, ac.DateTimeFieldNull);
                Assert.Equal(string.Empty, ac.VarcharFieldNull);
                Assert.Equal(string.Empty, ac.TextFieldNull);
                Assert.Equal(new byte[0], ac.BigDataFieldNull);
                Assert.Equal(EnumInt32Type.Zero, ac.EnumInt32FieldNull);
                Assert.Equal(EnumInt64Type.Zero, ac.EnumInt64FieldNull);

                var value = list[i];
                Assert.Equal(i + 1, value.Id);
                Assert.Equal(null, value.BoolFieldNull);
                Assert.Equal(null, value.ByteFieldNull);
                Assert.Equal(null, value.SbyteFieldNull);
                Assert.Equal(null, value.Int16FieldNull);
                Assert.Equal(null, value.Int32FieldNull);
                Assert.Equal(null, value.Int64FieldNull);
                Assert.Equal(null, value.UInt16FieldNull);
                Assert.Equal(null, value.UInt32FieldNull);
                Assert.Equal(null, value.UInt64FieldNull);
                Assert.Equal(null, value.FloatFieldNull);
                Assert.Equal(null, value.DoubleFieldNull);
                Assert.Equal(null, value.DecimalFieldNull);
                Assert.Equal(null, value.DateTimeFieldNull);
                Assert.Equal(null, value.VarcharFieldNull);
                Assert.Equal(null, value.TextFieldNull);
                Assert.Equal(null, value.BigDataFieldNull);
                Assert.Equal(null, value.EnumInt32FieldNull);
                Assert.Equal(null, value.EnumInt64FieldNull);
            }
        }

        [Fact]
        public void TestCase_DefalutValue()
        {
            context.TruncateTable<TeBaseFieldDefaultValue>();
            var value = context.CreateNew<TeBaseFieldDefaultValue>();
            context.Insert(value);
            var ac = context.SelectById<TeBaseFieldDefaultValue>(value.Id);
            Assert.Equal(1, ac.Id);
            Assert.Equal(true, ac.BoolFieldNull);
            Assert.Equal((byte)20, ac.ByteFieldNull);
            Assert.Equal((sbyte)20, ac.SbyteFieldNull);
            Assert.Equal((short)20, ac.Int16FieldNull);
            Assert.Equal(20, ac.Int32FieldNull);
            Assert.Equal(20L, ac.Int64FieldNull);
            Assert.Equal((ushort)20, ac.UInt16FieldNull);
            Assert.Equal(20u, ac.UInt32FieldNull);
            Assert.Equal(20uL, ac.UInt64FieldNull);
            Assert.Equal(20.5f, ac.FloatFieldNull);
            Assert.Equal(20.5d, ac.DoubleFieldNull.Value, 4);
            Assert.Equal(20.5m, ac.DecimalFieldNull);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeFieldNull);
            Assert.True((DateTime.Now - ac.NowFieldNull.Value).Seconds <= 1);
            Assert.Equal(DateTime.Now.Date, ac.TodayFieldNull);
            Assert.Equal("testtest", ac.VarcharFieldNull);
            Assert.Equal("testtest", ac.TextFieldNull);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeField);
            Assert.True((DateTime.Now - ac.NowField).Seconds <= 1);
            Assert.Equal(DateTime.Now.Date, ac.TodayField);
            Assert.Equal(EnumInt32Type.Positive1, ac.EnumInt32FieldNull);
            Assert.Equal(EnumInt64Type.Positive1, ac.EnumInt64FieldNull);

            Assert.Equal(1, value.Id);
            Assert.Equal(null, value.BoolFieldNull);
            Assert.Equal(null, value.ByteFieldNull);
            Assert.Equal(null, value.SbyteFieldNull);
            Assert.Equal(null, value.Int16FieldNull);
            Assert.Equal(null, value.Int32FieldNull);
            Assert.Equal(null, value.Int64FieldNull);
            Assert.Equal(null, value.UInt16FieldNull);
            Assert.Equal(null, value.UInt32FieldNull);
            Assert.Equal(null, value.UInt64FieldNull);
            Assert.Equal(null, value.FloatFieldNull);
            Assert.Equal(null, value.DoubleFieldNull);
            Assert.Equal(null, value.DecimalFieldNull);
            Assert.Equal(null, value.DateTimeFieldNull);
            Assert.Equal(null, value.NowFieldNull);
            Assert.Equal(null, value.TodayFieldNull);
            Assert.Equal(null, value.VarcharFieldNull);
            Assert.Equal(null, value.TextFieldNull);
            Assert.Equal(DateTime.MinValue, value.DateTimeField);
            Assert.Equal(DateTime.MinValue, value.NowField);
            Assert.Equal(DateTime.MinValue, value.TodayField);
            Assert.Equal(null, value.EnumInt32FieldNull);
            Assert.Equal(null, value.EnumInt64FieldNull);
        }

        [Fact]
        public void TestCase_DefaultValue_BulkInsert()
        {
            context.TruncateTable<TeBaseFieldDefaultValue>();

            var list = new List<TeBaseFieldDefaultValue>();
            for (int i = 0; i < 10; i++) {
                var value = context.CreateNew<TeBaseFieldDefaultValue>();
                list.Add(value);
            }
            context.BatchInsert(list);
            var listAc = context.Query<TeBaseFieldDefaultValue>().ToList();

            for (int i = 0; i < listAc.Count; i++) {
                var ac = listAc[i];
                Assert.Equal(i + 1, ac.Id);
                Assert.Equal(true, ac.BoolFieldNull);
                Assert.Equal((byte)20, ac.ByteFieldNull);
                Assert.Equal((sbyte)20, ac.SbyteFieldNull);
                Assert.Equal((short)20, ac.Int16FieldNull);
                Assert.Equal(20, ac.Int32FieldNull);
                Assert.Equal(20L, ac.Int64FieldNull);
                Assert.Equal((ushort)20, ac.UInt16FieldNull);
                Assert.Equal(20u, ac.UInt32FieldNull);
                Assert.Equal(20uL, ac.UInt64FieldNull);
                Assert.Equal(20.5f, ac.FloatFieldNull);
                Assert.Equal(20.5d, ac.DoubleFieldNull.Value, 4);
                Assert.Equal(20.5m, ac.DecimalFieldNull);
                Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeFieldNull);
                Assert.True((DateTime.Now - ac.NowFieldNull.Value).Seconds <= 1);
                Assert.Equal(DateTime.Now.Date, ac.TodayFieldNull);
                Assert.Equal("testtest", ac.VarcharFieldNull);
                Assert.Equal("testtest", ac.TextFieldNull);
                Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeField);
                Assert.True((DateTime.Now - ac.NowField).Seconds <= 1);
                Assert.Equal(DateTime.Now.Date, ac.TodayField);
                Assert.Equal(EnumInt32Type.Positive1, ac.EnumInt32FieldNull);
                Assert.Equal(EnumInt64Type.Positive1, ac.EnumInt64FieldNull);

                var value = list[i];
                Assert.Equal(null, value.BoolFieldNull);
                Assert.Equal(null, value.ByteFieldNull);
                Assert.Equal(null, value.SbyteFieldNull);
                Assert.Equal(null, value.Int16FieldNull);
                Assert.Equal(null, value.Int32FieldNull);
                Assert.Equal(null, value.Int64FieldNull);
                Assert.Equal(null, value.UInt16FieldNull);
                Assert.Equal(null, value.UInt32FieldNull);
                Assert.Equal(null, value.UInt64FieldNull);
                Assert.Equal(null, value.FloatFieldNull);
                Assert.Equal(null, value.DoubleFieldNull);
                Assert.Equal(null, value.DecimalFieldNull);
                Assert.Equal(null, value.DateTimeFieldNull);
                Assert.Equal(null, value.NowFieldNull);
                Assert.Equal(null, value.TodayFieldNull);
                Assert.Equal(null, value.VarcharFieldNull);
                Assert.Equal(null, value.TextFieldNull);
                Assert.Equal(DateTime.MinValue, value.DateTimeField);
                Assert.Equal(DateTime.MinValue, value.NowField);
                Assert.Equal(DateTime.MinValue, value.TodayField);
                Assert.Equal(null, value.EnumInt32FieldNull);
                Assert.Equal(null, value.EnumInt64FieldNull);
            }
        }

        [Fact]
        public void TestCase_MiniValue_Refresh()
        {
            context.TruncateTable<TeBaseFieldNullMiniValue>();

            var value = context.CreateNew<TeBaseFieldNullMiniValue>();
            context.Insert(value, true);
            var ac = context.SelectById<TeBaseFieldNullMiniValue>(value.Id);

            AssertExtend.Equal(value, ac);
        }

        [Fact]
        public async Task TestCase_MiniValue_Refresh_Async()
        {
            await context.TruncateTableAsync<TeBaseFieldNullMiniValue>();

            var value = context.CreateNew<TeBaseFieldNullMiniValue>();
            await context.InsertAsync(value, true);
            var ac = await context.SelectByIdAsync<TeBaseFieldNullMiniValue>(value.Id);

            AssertExtend.Equal(value, ac);
        }

        [Fact]
        public void TestCase_MiniValue_BulkInsert_Refresh()
        {
            context.TruncateTable<TeBaseFieldNullMiniValue>();

            var list = new List<TeBaseFieldNullMiniValue>();
            for (int i = 0; i < 10; i++) {
                var value = context.CreateNew<TeBaseFieldNullMiniValue>();
                list.Add(value);
            }
            context.BatchInsert(list, true);
            var listAc = context.Query<TeBaseFieldNullMiniValue>().ToList();

            AssertExtend.Equal(list, listAc);
        }

        [Fact]
        public async Task TestCase_MiniValue_BulkInsert_Refresh_Async()
        {
            await context.TruncateTableAsync<TeBaseFieldNullMiniValue>();

            var list = new List<TeBaseFieldNullMiniValue>();
            for (int i = 0; i < 10; i++) {
                var value = context.CreateNew<TeBaseFieldNullMiniValue>();
                list.Add(value);
            }
            await context.BatchInsertAsync(list, true);
            var listAc = await context.Query<TeBaseFieldNullMiniValue>().ToListAsync();

            AssertExtend.Equal(list, listAc);
        }

        [Fact]
        public void TestCase_DefalutValue_Refresh()
        {
            context.TruncateTable<TeBaseFieldDefaultValue>();
            var value = context.CreateNew<TeBaseFieldDefaultValue>();
            context.Insert(value, true);
            var ac = context.SelectById<TeBaseFieldDefaultValue>(value.Id);

            AssertExtend.Equal(value, ac);
        }

        [Fact]
        public void TestCase_DefaultValue_BulkInsert_Refresh()
        {
            context.TruncateTable<TeBaseFieldDefaultValue>();

            var list = new List<TeBaseFieldDefaultValue>();
            for (int i = 0; i < 10; i++) {
                var value = context.CreateNew<TeBaseFieldDefaultValue>();
                list.Add(value);
            }
            context.BatchInsert(list, true);
            var listAc = context.Query<TeBaseFieldDefaultValue>().ToList();

            AssertExtend.Equal(list, listAc);
        }
        #endregion

        [Fact]
        public void TestCase_DefalutValue_TimeStamp()
        {
            context.TruncateTable<MyBase4>();
            var value = context.CreateNew<MyBase4>();

            context.Insert(value);
            var ac = context.SelectById<MyBase4>(value.Id);

            Assert.True((DateTime.Now - ac.NowField).Seconds <= 1);
            Assert.True((DateTime.Now - ac.NowFieldNull.Value).Seconds <= 1);

            ac.NowField = DateTime.Now.AddDays(-1).Date;
            ac.NowFieldNull = DateTime.Now.AddDays(-1).Date;
            System.Threading.Thread.Sleep(1000);
            int ret = context.Update(ac);
            Assert.Equal(1, ret);
            var ac1 = context.SelectById<MyBase4>(value.Id);
            Assert.NotEqual(ac.NowField, ac1.NowField);
            Assert.NotEqual(ac.NowFieldNull, ac1.NowFieldNull);
            Assert.True((DateTime.Now - ac1.NowField).Seconds <= 1);
            Assert.True((DateTime.Now - ac1.NowFieldNull.Value).Seconds <= 1);


        }
    }
}
