using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Light.Data.Mssql.Test
{
    public class Mssql_BaseFieldDefaultValue : BaseTest
    {
        public Mssql_BaseFieldDefaultValue(ITestOutputHelper output) : base(output)
        {
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
            Assert.Equal(0, (DateTime.Now - ac.NowFieldNull.Value).Seconds);
            Assert.Equal(DateTime.Now.Date, ac.TodayFieldNull);
            Assert.Equal("testtest", ac.VarcharFieldNull);
            Assert.Equal("testtest", ac.TextFieldNull);
            Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeField);
            Assert.Equal(0, (DateTime.Now - ac.NowField).Seconds);
            Assert.Equal(DateTime.Now.Date, ac.TodayField);
            Assert.Equal(EnumInt32Type.Positive1, ac.EnumInt32FieldNull);
            Assert.Equal(EnumInt64Type.Positive1, ac.EnumInt64FieldNull);
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
                Assert.Equal(0, (DateTime.Now - ac.NowFieldNull.Value).Seconds);
                Assert.Equal(DateTime.Now.Date, ac.TodayFieldNull);
                Assert.Equal("testtest", ac.VarcharFieldNull);
                Assert.Equal("testtest", ac.TextFieldNull);
                Assert.Equal(new DateTime(2017, 1, 2, 12, 0, 0), ac.DateTimeField);
                Assert.Equal(0, (DateTime.Now - ac.NowField).Seconds);
                Assert.Equal(DateTime.Now.Date, ac.TodayField);
                Assert.Equal(EnumInt32Type.Positive1, ac.EnumInt32FieldNull);
                Assert.Equal(EnumInt64Type.Positive1, ac.EnumInt64FieldNull);
            }
        }
        #endregion
    }
}
