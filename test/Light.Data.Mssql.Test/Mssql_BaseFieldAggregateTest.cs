using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Light.Data.Mssql.Test
{
    public class Mssql_BaseFieldAggregateTest : BaseTest
    {
        class BytesEqualityComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] x, byte[] y)
            {
                if (x == null && y == null) {
                    return true;
                }
                else if (x != null && y != null) {
                    if (x.Length != y.Length) {
                        return false;
                    }
                    else {
                        for (int i = 0; i < x.Length; i++) {
                            if (x[i] != y[i]) {
                                return false;
                            }
                        }
                        return true;
                    }
                }
                else {
                    return false;
                }
            }

            public int GetHashCode(byte[] obj)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < obj.Length; i++) {
                    sb.AppendFormat("{0:X2}", obj[i]);
                }
                return sb.ToString().GetHashCode();
            }
        }

        public Mssql_BaseFieldAggregateTest(ITestOutputHelper output) : base(output)
        {
        }

        #region base test
        List<TeBaseFieldAggregateField> CreateBaseFieldTableList(int count)
        {
            List<TeBaseFieldAggregateField> list = new List<TeBaseFieldAggregateField>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeBaseFieldAggregateField item = new TeBaseFieldAggregateField();
                item.BoolField = i % 3 == 0;
                item.BoolFieldNull = i % 2 == 0 ? null : (bool?)(item.BoolField);
                item.ByteField = (byte)(i % 256);
                item.ByteFieldNull = i % 2 == 0 ? null : (byte?)(item.ByteField);
                item.SbyteField = (sbyte)((i % 128) * x);
                item.SbyteFieldNull = i % 2 == 0 ? null : (sbyte?)(item.SbyteField);
                item.Int16Field = (short)((i % 20) * x);
                item.Int16FieldNull = i % 2 == 0 ? null : (short?)(item.Int16Field);
                item.Int32Field = (int)((i % 23) * x);
                item.Int32FieldNull = i % 2 == 0 ? null : (int?)(item.Int32Field);
                item.Int64Field = (long)((i % 25) * x);
                item.Int64FieldNull = i % 2 == 0 ? null : (long?)(item.Int64Field);
                item.UInt16Field = (ushort)(i % 27);
                item.UInt16FieldNull = i % 2 == 0 ? null : (ushort?)(item.UInt16Field);
                item.UInt32Field = (uint)(i % 28);
                item.UInt32FieldNull = i % 2 == 0 ? null : (uint?)(item.UInt32Field);
                item.UInt64Field = (ulong)(i % 31);
                item.UInt64FieldNull = i % 2 == 0 ? null : (ulong?)(item.UInt64Field);
                item.FloatField = (float)((i % 19) * 0.1 * x);
                item.FloatFieldNull = i % 2 == 0 ? null : (float?)(item.FloatField);
                item.DoubleField = (double)((i % 22) * 0.1 * x);
                item.DoubleFieldNull = i % 2 == 0 ? null : (double?)(item.DoubleField);
                item.DecimalField = (decimal)((i % 26) * 0.1 * x);
                item.DecimalFieldNull = i % 2 == 0 ? null : (decimal?)(item.DecimalField);
                item.DateTimeField = d.AddHours(i * 2);
                item.DateTimeFieldNull = i % 2 == 0 ? null : (DateTime?)(item.DateTimeField);
                item.VarcharField = "testtest" + item.Int32Field;
                item.VarcharFieldNull = i % 2 == 0 ? null : item.VarcharField;
                item.TextField = "texttext" + item.Int32Field;
                item.TextFieldNull = i % 2 == 0 ? null : item.TextField;
                item.BigDataField = Encoding.UTF8.GetBytes(item.VarcharField);
                item.BigDataFieldNull = i % 2 == 0 ? null : item.BigDataField;
                item.EnumInt32Field = (EnumInt32Type)(i % 5 - 1);
                item.EnumInt32FieldNull = i % 2 == 0 ? null : (EnumInt32Type?)(item.EnumInt32Field);
                item.EnumInt64Field = (EnumInt64Type)(i % 5 - 1);
                item.EnumInt64FieldNull = i % 2 == 0 ? null : (EnumInt64Type?)(item.EnumInt64Field);
                list.Add(item);
            }
            return list;
        }

        List<TeBaseFieldAggregateField> CreateAndInsertBaseFieldTableList(int count)
        {
            var list = CreateBaseFieldTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeBaseFieldAggregateField>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        [Fact]
        public void TestCase_Query_Select_Where()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            int ex = 0;
            int ac = 0;

            ex = list.Where(x => x.Int16Field > 10).Count();
            ac = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int16Field > 10).AggregateField(x => new { Count = Function.Count() }).Count;
            Assert.Equal(ex, ac);

            ex = list.Where(x => x.Int16Field > 10 && x.Int16Field < 20).Count();
            ac = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int16Field > 10 && x.Int16Field < 20).AggregateField(x => new { Count = Function.Count() }).Count;
            Assert.Equal(ex, ac);

            ex = list.Where(x => x.Int16Field < 10 || x.Int16Field > 20).Count();
            ac = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int16Field < 10 || x.Int16Field > 20).AggregateField(x => new { Count = Function.Count() }).Count;
            Assert.Equal(ex, ac);

            ex = list.Where(x => x.Int16Field > 10 && x.Int16Field < 20).Count();
            ac = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int16Field > 10).WhereWithAnd(x => x.Int16Field < 20).AggregateField(x => new { Count = Function.Count() }).Count;
            Assert.Equal(ex, ac);

            ex = list.Where(x => x.Int16Field < 10 || x.Int16Field > 20).Count();
            ac = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int16Field < 10).WhereWithOr(x => x.Int16Field > 20).AggregateField(x => new { Count = Function.Count() }).Count;
            Assert.Equal(ex, ac);

            ex = list.Where(x => x.Int16Field > 20).Count();
            ac = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int16Field < 10).Where(x => x.Int16Field > 20).AggregateField(x => new { Count = Function.Count() }).Count;
            Assert.Equal(ex, ac);

            ex = list.Count();
            ac = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int16Field < 10).WhereReset().AggregateField(x => new { Count = Function.Count() }).Count;
            Assert.Equal(ex, ac);
        }

        [Fact]
        public async Task TestCase_Query_Select_Async()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            int ex = 0;
            int ac = 0;

            ex = list.Count();
            ac = (await context.Query<TeBaseFieldAggregateField>().AggregateFieldAsync(x => new { Count = Function.Count() }, CancellationToken.None)).Count;
            Assert.Equal(ex, ac);
        }

        [Fact]
        public void TestCase_Query_Select_Count()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            int ex = 0;
            int ac = 0;

            ex = list.Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count() }).Count;
            Assert.Equal(ex, ac);


            ex = list.Where(x => x.Int16Field > 10).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.CountCondition(x.Int16Field > 10) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BoolField).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.BoolField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BoolFieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.BoolFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.ByteField).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.ByteField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.ByteFieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.ByteFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.SbyteField).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.SbyteField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.SbyteFieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.SbyteFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int16Field).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.Int16Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int16FieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.Int16FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int32Field).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.Int32Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int32FieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.Int32FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int64Field).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.Int64Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int64FieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.Int64FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt16Field).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.UInt16Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt16FieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.UInt16FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt32Field).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.UInt32Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt32FieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.UInt32FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt64Field).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.UInt64Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt64FieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.UInt64FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.FloatField).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.FloatField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.FloatFieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.FloatFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DoubleField).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.DoubleField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DoubleFieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.DoubleFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DateTimeField).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.DateTimeField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DateTimeFieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.DateTimeFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.VarcharField).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.VarcharField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.VarcharFieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.VarcharFieldNull) }).Count;
            Assert.Equal(ex, ac);

            //ex = list.Select(x => x.TextField).Count();
            //ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.TextField) }).Count;
            //Assert.Equal(ex, ac);

            //ex = list.Select(x => x.TextFieldNull).Where(x => x != null).Count();
            //ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.TextFieldNull) }).Count;
            //Assert.Equal(ex, ac);

            ex = list.Select(x => x.BigDataField).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.BigDataField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BigDataFieldNull).Where(x => x != null).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.Count(x.BigDataFieldNull) }).Count;
            Assert.Equal(ex, ac);
        }

        [Fact]
        public void TestCase_Query_Select_DistinctCount()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            int ex = 0;
            int ac = 0;
            ex = list.Select(x => x.BoolField).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.BoolField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BoolFieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.BoolFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.ByteField).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.ByteField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.ByteFieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.ByteFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.SbyteField).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.SbyteField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.SbyteFieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.SbyteFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int16Field).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.Int16Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int16FieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.Int16FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int32Field).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.Int32Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int32FieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.Int32FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int64Field).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.Int64Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int64FieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.Int64FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt16Field).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.UInt16Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt16FieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.UInt16FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt32Field).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.UInt32Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt32FieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.UInt32FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt64Field).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.UInt64Field) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt64FieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.UInt64FieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.FloatField).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.FloatField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.FloatFieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.FloatFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DoubleField).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.DoubleField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DoubleFieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.DoubleFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DateTimeField).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.DateTimeField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DateTimeFieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.DateTimeFieldNull) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.VarcharField).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.VarcharField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.VarcharFieldNull).Where(x => x != null).Distinct().Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.VarcharFieldNull) }).Count;
            Assert.Equal(ex, ac);

            //ex = list.Select(x => x.TextField).Distinct().Count();
            //ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.TextField) }).Count;
            //Assert.Equal(ex, ac);

            //ex = list.Select(x => x.TextFieldNull).Where(x => x != null).Distinct().Count();
            //ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.TextFieldNull) }).Count;
            //Assert.Equal(ex, ac);

            ex = list.Select(x => x.BigDataField).Distinct(new BytesEqualityComparer()).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.BigDataField) }).Count;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BigDataFieldNull).Where(x => x != null).Distinct(new BytesEqualityComparer()).Count();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Count = Function.DistinctCount(x.BigDataFieldNull) }).Count;
            Assert.Equal(ex, ac);
        }

        [Fact]
        public void TestCase_Query_Select_LongCount()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            long ex = 0;
            long ac = 0;

            ex = list.LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount() }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Where(x => x.Int16Field > 10).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCountCondition(x.Int16Field > 10) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BoolField).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.BoolField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BoolFieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.BoolFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.ByteField).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.ByteField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.ByteFieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.ByteFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.SbyteField).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.SbyteField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.SbyteFieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.SbyteFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int16Field).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.Int16Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int16FieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.Int16FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int32Field).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.Int32Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int32FieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.Int32FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int64Field).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.Int64Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int64FieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.Int64FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt16Field).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.UInt16Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt16FieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.UInt16FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt32Field).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.UInt32Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt32FieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.UInt32FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt64Field).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.UInt64Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt64FieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.UInt64FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.FloatField).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.FloatField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.FloatFieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.FloatFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DoubleField).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.DoubleField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DoubleFieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.DoubleFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DateTimeField).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.DateTimeField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DateTimeFieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.DateTimeFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.VarcharField).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.VarcharField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.VarcharFieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.VarcharFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            //ex = list.Select(x => x.TextField).LongCount();
            //ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.TextField) }).LongCount;
            //Assert.Equal(ex, ac);

            //ex = list.Select(x => x.TextFieldNull).Where(x => x != null).LongCount();
            //ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.TextFieldNull) }).LongCount;
            //Assert.Equal(ex, ac);

            ex = list.Select(x => x.BigDataField).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.BigDataField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BigDataFieldNull).Where(x => x != null).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.LongCount(x.BigDataFieldNull) }).LongCount;
            Assert.Equal(ex, ac);
        }

        [Fact]
        public void TestCase_Query_Select_DistinctLongCount()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            long ex = 0;
            long ac = 0;
            ex = list.Select(x => x.BoolField).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.BoolField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BoolFieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.BoolFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.ByteField).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.ByteField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.ByteFieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.ByteFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.SbyteField).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.SbyteField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.SbyteFieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.SbyteFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int16Field).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.Int16Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int16FieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.Int16FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int32Field).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.Int32Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int32FieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.Int32FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int64Field).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.Int64Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.Int64FieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.Int64FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt16Field).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.UInt16Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt16FieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.UInt16FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt32Field).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.UInt32Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt32FieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.UInt32FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt64Field).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.UInt64Field) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.UInt64FieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.UInt64FieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.FloatField).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.FloatField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.FloatFieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.FloatFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DoubleField).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.DoubleField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DoubleFieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.DoubleFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DateTimeField).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.DateTimeField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.DateTimeFieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.DateTimeFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.VarcharField).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.VarcharField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.VarcharFieldNull).Where(x => x != null).Distinct().LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.VarcharFieldNull) }).LongCount;
            Assert.Equal(ex, ac);

            //ex = list.Select(x => x.TextField).Distinct().LongCount();
            //ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.TextField) }).LongCount;
            //Assert.Equal(ex, ac);

            //ex = list.Select(x => x.TextFieldNull).Where(x => x != null).Distinct().LongCount();
            //ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.TextFieldNull) }).LongCount;
            //Assert.Equal(ex, ac);

            ex = list.Select(x => x.BigDataField).Distinct(new BytesEqualityComparer()).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.BigDataField) }).LongCount;
            Assert.Equal(ex, ac);

            ex = list.Select(x => x.BigDataFieldNull).Where(x => x != null).Distinct(new BytesEqualityComparer()).LongCount();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongCount = Function.DistinctLongCount(x.BigDataFieldNull) }).LongCount;
            Assert.Equal(ex, ac);
        }

        [Fact]
        public void TestCase_Query_Select_Sum()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            int ex = 0;
            int ac = 0;
            int? ex_n = null;
            int? ac_n = null;


            ex = list.Select(x => (int)x.ByteField).Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.ByteField) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.ByteFieldNull).Where(x => x != null).Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.ByteFieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (int)x.SbyteField).Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.SbyteField) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.SbyteFieldNull).Where(x => x != null).Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.SbyteFieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (int)x.Int16Field).Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.Int16Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.Int16FieldNull).Where(x => x != null).Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.Int16FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => x.Int32Field).Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.Int32Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => x.Int32FieldNull).Where(x => x != null).Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.Int32FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (int)x.UInt16Field).Sum();
            ac = (int)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.UInt16Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.UInt16FieldNull).Where(x => x != null).Sum();
            ac_n = (int?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.UInt16FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (int)x.UInt32Field).Sum();
            ac = (int)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.UInt32Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.UInt32FieldNull).Where(x => x != null).Sum();
            ac_n = (int?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.UInt32FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            long exl = 0;
            long acl = 0;

            long? exl_n = null;
            long? acl_n = null;

            exl = list.Select(x => x.Int64Field).Sum();
            acl = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.Int64Field) }).Sum;
            Assert.Equal(exl, acl);

            exl_n = list.Select(x => x.Int64FieldNull).Where(x => x != null).Sum();
            acl_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.Int64FieldNull) }).Sum;
            Assert.Equal(exl_n, acl_n);

            exl = list.Select(x => (long)x.UInt64Field).Sum();
            acl = (long)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.UInt64Field) }).Sum;
            Assert.Equal(exl, acl);

            exl_n = list.Select(x => (long?)x.UInt64FieldNull).Where(x => x != null).Sum();
            acl_n = (long?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.UInt64FieldNull) }).Sum;
            Assert.Equal(exl_n, acl_n);

            float exf = 0;
            float acf = 0;

            float? exf_n = null;
            float? acf_n = null;

            exf = list.Select(x => x.FloatField).Sum();
            acf = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.FloatField) }).Sum;
            Assert.Equal(exf, acf, 4);

            exf_n = list.Select(x => x.FloatFieldNull).Where(x => x != null).Sum();
            acf_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.FloatFieldNull) }).Sum;
            Assert.Equal(exf_n.Value, acf_n.Value, 4);

            double exd = 0;
            double acd = 0;

            double? exd_n = null;
            double? acd_n = null;

            exd = list.Select(x => x.DoubleField).Sum();
            acd = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.DoubleField) }).Sum;
            Assert.Equal(exd, acd);

            exd_n = list.Select(x => x.DoubleFieldNull).Where(x => x != null).Sum();
            acd_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.DoubleFieldNull) }).Sum;
            Assert.Equal(exd_n, acd_n);


            decimal exm = 0;
            decimal acm = 0;

            decimal? exm_n = null;
            decimal? acm_n = null;

            exm = list.Select(x => x.DecimalField).Sum();
            acm = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.DecimalField) }).Sum;
            Assert.Equal(exm, acm);

            exm_n = list.Select(x => x.DecimalFieldNull).Where(x => x != null).Sum();
            acm_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.Sum(x.DecimalFieldNull) }).Sum;
            Assert.Equal(exm_n, acm_n);

            ac_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int32FieldNull == null).AggregateField(x => new { Sum = Function.Sum(x.Int32FieldNull) }).Sum;
            Assert.Null(ac_n);

            acm_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.DecimalFieldNull == null).AggregateField(x => new { Sum = Function.Sum(x.DecimalFieldNull) }).Sum;
            Assert.Null(acm_n);
        }

        [Fact]
        public void TestCase_Query_Select_LongSum()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            long ex = 0;
            long ac = 0;
            long? ex_n = null;
            long? ac_n = null;

            ex = list.Select(x => (long)x.ByteField).Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.LongSum(x.ByteField) }).LongSum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.ByteFieldNull).Where(x => x != null).Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.LongSum(x.ByteFieldNull) }).LongSum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.SbyteField).Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.LongSum(x.SbyteField) }).LongSum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.SbyteFieldNull).Where(x => x != null).Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.LongSum(x.SbyteFieldNull) }).LongSum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.Int16Field).Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.LongSum(x.Int16Field) }).LongSum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.Int16FieldNull).Where(x => x != null).Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.LongSum(x.Int16FieldNull) }).LongSum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.Int32Field).Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.LongSum(x.Int32Field) }).LongSum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.Int32FieldNull).Where(x => x != null).Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.LongSum(x.Int32FieldNull) }).LongSum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.UInt16Field).Sum();
            ac = (long)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.LongSum(x.UInt16Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.UInt16FieldNull).Where(x => x != null).Sum();
            ac_n = (long?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.LongSum(x.UInt16FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.UInt32Field).Sum();
            ac = (long)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.LongSum(x.UInt32Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.UInt32FieldNull).Where(x => x != null).Sum();
            ac_n = (long?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.LongSum(x.UInt32FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ac_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int32FieldNull == null).AggregateField(x => new { LongSum = Function.LongSum(x.Int32FieldNull) }).LongSum;
            Assert.Null(ac_n);
        }

        [Fact]
        public void TestCase_Query_Select_DistinctSum()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            int ex = 0;
            int ac = 0;
            int? ex_n = null;
            int? ac_n = null;


            ex = list.Select(x => (int)x.ByteField).Distinct().Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.ByteField) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.ByteFieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.ByteFieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (int)x.SbyteField).Distinct().Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.SbyteField) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.SbyteFieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.SbyteFieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (int)x.Int16Field).Distinct().Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.Int16Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.Int16FieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.Int16FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => x.Int32Field).Distinct().Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.Int32Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => x.Int32FieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.Int32FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (int)x.UInt16Field).Distinct().Sum();
            ac = (int)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.UInt16Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.UInt16FieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = (int?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.UInt16FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (int)x.UInt32Field).Distinct().Sum();
            ac = (int)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.UInt32Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (int?)x.UInt32FieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = (int?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.UInt32FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            long exl = 0;
            long acl = 0;

            long? exl_n = null;
            long? acl_n = null;

            exl = list.Select(x => x.Int64Field).Distinct().Sum();
            acl = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.Int64Field) }).Sum;
            Assert.Equal(exl, acl);

            exl_n = list.Select(x => x.Int64FieldNull).Where(x => x != null).Distinct().Sum();
            acl_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.Int64FieldNull) }).Sum;
            Assert.Equal(exl_n, acl_n);

            exl = list.Select(x => (long)x.UInt64Field).Distinct().Sum();
            acl = (long)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.UInt64Field) }).Sum;
            Assert.Equal(exl, acl);

            exl_n = list.Select(x => (long?)x.UInt64FieldNull).Where(x => x != null).Distinct().Sum();
            acl_n = (long?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.UInt64FieldNull) }).Sum;
            Assert.Equal(exl_n, acl_n);

            float exf = 0;
            float acf = 0;

            float? exf_n = null;
            float? acf_n = null;

            exf = list.Select(x => x.FloatField).Distinct().Sum();
            acf = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.FloatField) }).Sum;
            Assert.Equal(exf, acf);

            exf_n = list.Select(x => x.FloatFieldNull).Where(x => x != null).Distinct().Sum();
            acf_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.FloatFieldNull) }).Sum;
            Assert.Equal(exf_n, acf_n);

            double exd = 0;
            double acd = 0;

            double? exd_n = null;
            double? acd_n = null;

            exd = list.Select(x => x.DoubleField).Distinct().Sum();
            acd = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.DoubleField) }).Sum;
            Assert.Equal(exd, acd, 4);

            exd_n = list.Select(x => x.DoubleFieldNull).Where(x => x != null).Distinct().Sum();
            acd_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.DoubleFieldNull) }).Sum;
            Assert.Equal(exd_n, acd_n);


            decimal exm = 0;
            decimal acm = 0;

            decimal? exm_n = null;
            decimal? acm_n = null;

            exm = list.Select(x => x.DecimalField).Distinct().Sum();
            acm = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.DecimalField) }).Sum;
            Assert.Equal(exm, acm);

            exm_n = list.Select(x => x.DecimalFieldNull).Where(x => x != null).Distinct().Sum();
            acm_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctSum(x.DecimalFieldNull) }).Sum;
            Assert.Equal(exm_n, acm_n);

            ac_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int32FieldNull == null).AggregateField(x => new { LongSum = Function.DistinctSum(x.Int32FieldNull) }).LongSum;
            Assert.Null(ac_n);

            acm_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.DecimalFieldNull == null).AggregateField(x => new { Sum = Function.DistinctSum(x.DecimalFieldNull) }).Sum;
            Assert.Null(acm_n);
        }

        [Fact]
        public void TestCase_Query_Select_DistinctLongSum()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            long ex = 0;
            long ac = 0;
            long? ex_n = null;
            long? ac_n = null;

            ex = list.Select(x => (long)x.ByteField).Distinct().Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.DistinctLongSum(x.ByteField) }).LongSum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.ByteFieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.DistinctLongSum(x.ByteFieldNull) }).LongSum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.SbyteField).Distinct().Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.DistinctLongSum(x.SbyteField) }).LongSum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.SbyteFieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.DistinctLongSum(x.SbyteFieldNull) }).LongSum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.Int16Field).Distinct().Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.DistinctLongSum(x.Int16Field) }).LongSum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.Int16FieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.DistinctLongSum(x.Int16FieldNull) }).LongSum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.Int32Field).Distinct().Sum();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.DistinctLongSum(x.Int32Field) }).LongSum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.Int32FieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { LongSum = Function.DistinctLongSum(x.Int32FieldNull) }).LongSum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.UInt16Field).Distinct().Sum();
            ac = (long)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctLongSum(x.UInt16Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.UInt16FieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = (long?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctLongSum(x.UInt16FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ex = list.Select(x => (long)x.UInt32Field).Distinct().Sum();
            ac = (long)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctLongSum(x.UInt32Field) }).Sum;
            Assert.Equal(ex, ac);

            ex_n = list.Select(x => (long?)x.UInt32FieldNull).Where(x => x != null).Distinct().Sum();
            ac_n = (long?)context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Sum = Function.DistinctLongSum(x.UInt32FieldNull) }).Sum;
            Assert.Equal(ex_n, ac_n);

            ac_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int32FieldNull == null).AggregateField(x => new { LongSum = Function.DistinctLongSum(x.Int32FieldNull) }).LongSum;
            Assert.Null(ac_n);
        }

        [Fact]
        public void TestCase_Query_Select_Avg()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            double ex = 0;
            double ac = 0;

            double? ex_n = null;
            double? ac_n = null;

            ex = list.Select(x => (int)x.ByteField).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.ByteField) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.ByteFieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.ByteFieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (int)x.SbyteField).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.SbyteField) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.SbyteFieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.SbyteFieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (int)x.Int16Field).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.Int16Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.Int16FieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.Int16FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => x.Int32Field).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.Int32Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => x.Int32FieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.Int32FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => x.Int64Field).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.Int64Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => x.Int64FieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.Int64FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (int)x.UInt16Field).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.UInt16Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.UInt16FieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.UInt16FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (int)x.UInt32Field).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.UInt32Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.UInt32FieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.UInt32FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (long)x.UInt64Field).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.UInt64Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (long?)x.UInt64FieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.UInt64FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => x.FloatField).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.FloatField) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => x.FloatFieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.FloatFieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => x.DoubleField).Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.DoubleField) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => x.DoubleFieldNull).Where(x => x != null).Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.DoubleFieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            decimal exm = 0;
            decimal acm = 0;

            decimal? exm_n = null;
            decimal? acm_n = null;

            exm = list.Select(x => x.DecimalField).Average();
            acm = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.DecimalField) }).Avg;
            Assert.Equal(exm, acm, 4);

            exm_n = list.Select(x => x.DecimalFieldNull).Where(x => x != null).Average();
            acm_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.Avg(x.DecimalFieldNull) }).Avg;
            Assert.Equal(exm_n.Value, acm_n.Value, 4);

            ac_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int32FieldNull == null).AggregateField(x => new { Avg = Function.Avg(x.Int32FieldNull) }).Avg;
            Assert.Null(ac_n);

            acm_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.DecimalFieldNull == null).AggregateField(x => new { Avg = Function.Avg(x.DecimalFieldNull) }).Avg;
            Assert.Null(acm_n);
        }

        [Fact]
        public void TestCase_Query_Select_DistinctAvg()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            double ex = 0;
            double ac = 0;

            double? ex_n = null;
            double? ac_n = null;

            ex = list.Select(x => (int)x.ByteField).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.ByteField) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.ByteFieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.ByteFieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (int)x.SbyteField).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.SbyteField) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.SbyteFieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.SbyteFieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (int)x.Int16Field).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.Int16Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.Int16FieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.Int16FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => x.Int32Field).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.Int32Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => x.Int32FieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.Int32FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => x.Int64Field).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.Int64Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => x.Int64FieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.Int64FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (int)x.UInt16Field).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.UInt16Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.UInt16FieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.UInt16FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (int)x.UInt32Field).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.UInt32Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (int?)x.UInt32FieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.UInt32FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => (long)x.UInt64Field).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.UInt64Field) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => (long?)x.UInt64FieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.UInt64FieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => x.FloatField).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.FloatField) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => x.FloatFieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.FloatFieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            ex = list.Select(x => x.DoubleField).Distinct().Average();
            ac = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.DoubleField) }).Avg;
            Assert.Equal(ex, ac, 4);

            ex_n = list.Select(x => x.DoubleFieldNull).Where(x => x != null).Distinct().Average();
            ac_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.DoubleFieldNull) }).Avg;
            Assert.Equal(ex_n.Value, ac_n.Value, 4);

            decimal exm = 0;
            decimal acm = 0;

            decimal? exm_n = null;
            decimal? acm_n = null;

            exm = list.Select(x => x.DecimalField).Distinct().Average();
            acm = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.DecimalField) }).Avg;
            Assert.Equal(exm, acm, 4);

            exm_n = list.Select(x => x.DecimalFieldNull).Where(x => x != null).Distinct().Average();
            acm_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Avg = Function.DistinctAvg(x.DecimalFieldNull) }).Avg;
            Assert.Equal(exm_n.Value, acm_n.Value, 4);

            ac_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int32FieldNull == null).AggregateField(x => new { Avg = Function.DistinctAvg(x.Int32FieldNull) }).Avg;
            Assert.Null(ac_n);

            acm_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.DecimalFieldNull == null).AggregateField(x => new { Avg = Function.DistinctAvg(x.DecimalFieldNull) }).Avg;
            Assert.Null(acm_n);
        }

        [Fact]
        public void TestCase_Query_Select_Max()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            byte exb = list.Select(x => x.ByteField).Max();
            byte acb = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.ByteField) }).Max;
            Assert.Equal(exb, acb);

            byte? exb_n = list.Select(x => x.ByteFieldNull).Max();
            byte? acb_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.ByteFieldNull) }).Max;
            Assert.Equal(exb_n, acb_n);

            sbyte exsb = list.Select(x => x.SbyteField).Max();
            sbyte acsb = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.SbyteField) }).Max;
            Assert.Equal(exb, acb);

            sbyte? exsb_n = list.Select(x => x.SbyteFieldNull).Max();
            sbyte? acsb_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.SbyteFieldNull) }).Max;
            Assert.Equal(exsb_n, acsb_n);

            short exi16 = list.Select(x => x.Int16Field).Max();
            short aci16 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.Int16Field) }).Max;
            Assert.Equal(exi16, aci16);

            short? exi16_n = list.Select(x => x.Int16FieldNull).Max();
            short? aci16_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.Int16FieldNull) }).Max;
            Assert.Equal(exi16_n, aci16_n);

            int exi32 = list.Select(x => x.Int32Field).Max();
            int aci32 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.Int32Field) }).Max;
            Assert.Equal(exi32, aci32);

            int? exi32_n = list.Select(x => x.Int32FieldNull).Max();
            int? aci32_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.Int32FieldNull) }).Max;
            Assert.Equal(exi32_n, aci32_n);

            long exi64 = list.Select(x => x.Int64Field).Max();
            long aci64 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.Int64Field) }).Max;
            Assert.Equal(exi64, aci64);

            long? exi64_n = list.Select(x => x.Int64FieldNull).Max();
            long? aci64_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.Int64FieldNull) }).Max;
            Assert.Equal(exi64_n, aci64_n);

            ushort exui16 = list.Select(x => x.UInt16Field).Max();
            ushort acui16 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.UInt16Field) }).Max;
            Assert.Equal(exi16, aci16);

            ushort? exui16_n = list.Select(x => x.UInt16FieldNull).Max();
            ushort? acui16_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.UInt16FieldNull) }).Max;
            Assert.Equal(exi16_n, aci16_n);

            uint exui32 = list.Select(x => x.UInt32Field).Max();
            uint acui32 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.UInt32Field) }).Max;
            Assert.Equal(exi32, aci32);

            uint? exui32_n = list.Select(x => x.UInt32FieldNull).Max();
            uint? acui32_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.UInt32FieldNull) }).Max;
            Assert.Equal(exi32_n, aci32_n);

            ulong exui64 = list.Select(x => x.UInt64Field).Max();
            ulong acui64 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.UInt64Field) }).Max;
            Assert.Equal(exi64, aci64);

            ulong? exui64_n = list.Select(x => x.UInt64FieldNull).Max();
            ulong? acui64_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.UInt64FieldNull) }).Max;
            Assert.Equal(exi64_n, aci64_n);

            float exf = list.Select(x => x.FloatField).Max();
            float acf = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.FloatField) }).Max;
            Assert.Equal(exf, acf);

            float? exf_n = list.Select(x => x.FloatFieldNull).Max();
            float? acf_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.FloatFieldNull) }).Max;
            Assert.Equal(exf_n, acf_n);

            double exd = list.Select(x => x.DoubleField).Max();
            double acd = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.DoubleField) }).Max;
            Assert.Equal(exd, acd);

            double? exd_n = list.Select(x => x.DoubleFieldNull).Max();
            double? acd_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.DoubleFieldNull) }).Max;
            Assert.Equal(exd_n, acd_n);

            decimal exm = list.Select(x => x.DecimalField).Max();
            decimal acm = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.DecimalField) }).Max;
            Assert.Equal(exm, acm);

            decimal? exm_n = list.Select(x => x.DecimalFieldNull).Max();
            decimal? acm_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.DecimalFieldNull) }).Max;
            Assert.Equal(exm_n, acm_n);

            DateTime ext = list.Select(x => x.DateTimeField).Max();
            DateTime act = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.DateTimeField) }).Max;
            Assert.Equal(exm, acm);

            DateTime? ext_n = list.Select(x => x.DateTimeFieldNull).Max();
            DateTime? act_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Max = Function.Max(x.DateTimeFieldNull) }).Max;
            Assert.Equal(exm_n, acm_n);

            aci32_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int32FieldNull == null).AggregateField(x => new { Max = Function.Max(x.Int32FieldNull) }).Max;
            Assert.Null(aci32_n);
        }

        [Fact]
        public void TestCase_Query_Select_Min()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            byte exb = list.Select(x => x.ByteField).Min();
            byte acb = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.ByteField) }).Min;
            Assert.Equal(exb, acb);

            byte? exb_n = list.Select(x => x.ByteFieldNull).Min();
            byte? acb_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.ByteFieldNull) }).Min;
            Assert.Equal(exb_n, acb_n);

            sbyte exsb = list.Select(x => x.SbyteField).Min();
            sbyte acsb = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.SbyteField) }).Min;
            Assert.Equal(exb, acb);

            sbyte? exsb_n = list.Select(x => x.SbyteFieldNull).Min();
            sbyte? acsb_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.SbyteFieldNull) }).Min;
            Assert.Equal(exsb_n, acsb_n);

            short exi16 = list.Select(x => x.Int16Field).Min();
            short aci16 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.Int16Field) }).Min;
            Assert.Equal(exi16, aci16);

            short? exi16_n = list.Select(x => x.Int16FieldNull).Min();
            short? aci16_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.Int16FieldNull) }).Min;
            Assert.Equal(exi16_n, aci16_n);

            int exi32 = list.Select(x => x.Int32Field).Min();
            int aci32 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.Int32Field) }).Min;
            Assert.Equal(exi32, aci32);

            int? exi32_n = list.Select(x => x.Int32FieldNull).Min();
            int? aci32_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.Int32FieldNull) }).Min;
            Assert.Equal(exi32_n, aci32_n);

            long exi64 = list.Select(x => x.Int64Field).Min();
            long aci64 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.Int64Field) }).Min;
            Assert.Equal(exi64, aci64);

            long? exi64_n = list.Select(x => x.Int64FieldNull).Min();
            long? aci64_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.Int64FieldNull) }).Min;
            Assert.Equal(exi64_n, aci64_n);

            ushort exui16 = list.Select(x => x.UInt16Field).Min();
            ushort acui16 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.UInt16Field) }).Min;
            Assert.Equal(exi16, aci16);

            ushort? exui16_n = list.Select(x => x.UInt16FieldNull).Min();
            ushort? acui16_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.UInt16FieldNull) }).Min;
            Assert.Equal(exi16_n, aci16_n);

            uint exui32 = list.Select(x => x.UInt32Field).Min();
            uint acui32 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.UInt32Field) }).Min;
            Assert.Equal(exi32, aci32);

            uint? exui32_n = list.Select(x => x.UInt32FieldNull).Min();
            uint? acui32_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.UInt32FieldNull) }).Min;
            Assert.Equal(exi32_n, aci32_n);

            ulong exui64 = list.Select(x => x.UInt64Field).Min();
            ulong acui64 = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.UInt64Field) }).Min;
            Assert.Equal(exi64, aci64);

            ulong? exui64_n = list.Select(x => x.UInt64FieldNull).Min();
            ulong? acui64_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.UInt64FieldNull) }).Min;
            Assert.Equal(exi64_n, aci64_n);

            float exf = list.Select(x => x.FloatField).Min();
            float acf = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.FloatField) }).Min;
            Assert.Equal(exf, acf);

            float? exf_n = list.Select(x => x.FloatFieldNull).Min();
            float? acf_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.FloatFieldNull) }).Min;
            Assert.Equal(exf_n, acf_n);

            double exd = list.Select(x => x.DoubleField).Min();
            double acd = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.DoubleField) }).Min;
            Assert.Equal(exd, acd);

            double? exd_n = list.Select(x => x.DoubleFieldNull).Min();
            double? acd_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.DoubleFieldNull) }).Min;
            Assert.Equal(exd_n, acd_n);

            decimal exm = list.Select(x => x.DecimalField).Min();
            decimal acm = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.DecimalField) }).Min;
            Assert.Equal(exm, acm);

            decimal? exm_n = list.Select(x => x.DecimalFieldNull).Min();
            decimal? acm_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.DecimalFieldNull) }).Min;
            Assert.Equal(exm_n, acm_n);

            DateTime ext = list.Select(x => x.DateTimeField).Min();
            DateTime act = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.DateTimeField) }).Min;
            Assert.Equal(exm, acm);

            DateTime? ext_n = list.Select(x => x.DateTimeFieldNull).Min();
            DateTime? act_n = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new { Min = Function.Min(x.DateTimeFieldNull) }).Min;
            Assert.Equal(exm_n, acm_n);

            aci32_n = context.Query<TeBaseFieldAggregateField>().Where(x => x.Int32FieldNull == null).AggregateField(x => new { Min = Function.Min(x.Int32FieldNull) }).Min;
            Assert.Null(aci32_n);
        }

        [Fact]
        public void TestCase_Query_Select_Mutli()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            var ex1 = list.Count();
            var ex2 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Count();
            var ex3 = list.Where(x => x.Int16Field > 10).Count();
            var ex4 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Sum();
            var ex5 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Average();
            var ex6 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Max();
            var ex7 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Min();

            var obj = context.Query<TeBaseFieldAggregateField>().AggregateField(x => new {
                Count = Function.Count(),
                CountField = Function.Count(x.Int32FieldNull),
                CountCondition = Function.CountCondition(x.Int16Field > 10),
                Sum = Function.Sum(x.Int32FieldNull),
                Avg = Function.Avg(x.Int32FieldNull),
                Max = Function.Max(x.Int32FieldNull),
                Min = Function.Min(x.Int32FieldNull),
            });

            Assert.Equal(ex1, obj.Count);
            Assert.Equal(ex2, obj.CountField);
            Assert.Equal(ex3, obj.CountCondition);
            Assert.Equal(ex4, obj.Sum);
            Assert.Equal(ex5.Value, obj.Avg.Value, 4);
            Assert.Equal(ex6, obj.Max);
            Assert.Equal(ex7, obj.Min);
        }


        class AggregateModel
        {
            public int KeyData { get; set; }
            public int Count { get; set; }
            public int CountField { get; set; }
            public int CountCondition { get; set; }
            public int Sum { get; set; }
            public double Avg { get; set; }
            public DateTime Max { get; set; }
            public DateTime Min { get; set; }
        }

        [Fact]
        public void TestCase_Aggregate_GroupBy()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            var ex1 = list.GroupBy(x => x.Int32Field).Select(g => new {
                KeyData = g.Key,
                Count = g.Count(),
                CountField = g.Count(x => x.Int32FieldNull != null),
                CountCondition = g.Count(x => x.Int16Field > 10),
                Sum = g.Sum(x => x.ByteField),
                Avg = g.Average(x => x.Int64Field),
                Max = g.Max(x => x.DateTimeField),
                Min = g.Min(x => x.DateTimeField),
            }).ToList().OrderBy(x => x.KeyData).ToList();

            var ac1 = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                KeyData = x.Int32Field,
                Count = Function.Count(),
                CountField = Function.Count(x.Int32FieldNull),
                CountCondition = Function.CountCondition(x.Int16Field > 10),
                Sum = Function.Sum(x.ByteField),
                Avg = Function.Avg(x.Int64Field),
                Max = Function.Max(x.DateTimeField),
                Min = Function.Min(x.DateTimeField),
            }).ToList().OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex1, ac1);

            var ex1_1 = list.GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                KeyData = g.Key,
                Count = g.Count(),
                CountField = g.Count(x => x.Int32FieldNull != null),
                CountCondition = g.Count(x => x.Int16Field > 10),
                Sum = g.Sum(x => x.ByteField),
                Avg = g.Average(x => x.Int64Field),
                Max = g.Max(x => x.DateTimeField),
                Min = g.Min(x => x.DateTimeField),
            }).ToList().OrderBy(x => x.KeyData).ToList();

            var ac1_1 = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new AggregateModel {
                KeyData = x.Int32Field,
                Count = Function.Count(),
                CountField = Function.Count(x.Int32FieldNull),
                CountCondition = Function.CountCondition(x.Int16Field > 10),
                Sum = Function.Sum(x.ByteField),
                Avg = Function.Avg(x.Int64Field),
                Max = Function.Max(x.DateTimeField),
                Min = Function.Min(x.DateTimeField),
            }).ToList().OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex1_1, ac1_1);

            var ex2 = list.GroupBy(x => new { x.VarcharField, x.ByteField }).Select(g => new {
                Key1 = g.Key.VarcharField,
                Key2 = g.Key.ByteField,
                Count = g.Count(),
                CountField = g.Count(x => x.Int32FieldNull != null),
                CountCondition = g.Count(x => x.Int16Field > 10),
                Sum = g.Sum(x => x.Int32Field),
                Avg = g.Average(x => x.Int32Field),
                Max = g.Max(x => x.Int32Field),
                Min = g.Min(x => x.Int32Field),
            }).ToList().OrderBy(x => x.Key1).ThenBy(x => x.Key2).ToList();

            var ac2 = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                Key1 = x.VarcharField,
                Key2 = x.ByteField,
                Count = Function.Count(),
                CountField = Function.Count(x.Int32FieldNull),
                CountCondition = Function.CountCondition(x.Int16Field > 10),
                Sum = Function.Sum(x.Int32Field),
                Avg = Function.Avg(x.Int32Field),
                Max = Function.Max(x.Int32Field),
                Min = Function.Min(x.Int32Field),
            }).ToList().OrderBy(x => x.Key1).ThenBy(x => x.Key2).ToList();
            AssertExtend.StrictEqual(ex2, ac2);

            var ex3 = list.GroupBy(x => x.VarcharField).Select(g => new {
                KeyData = g.Key,
                CountField = g.Count(x => x.Int32FieldNull != null)
            }).ToList().OrderBy(x => x.KeyData).ToList();

            var ac3 = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                KeyData = x.VarcharField,
                CountField = Function.Count(x.Int32FieldNull),
            }).ToList().OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex3, ac3);
        }

        [Fact]
        public async Task TestCase_Aggregate_GroupBy_Async()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            var ex1 = list.GroupBy(x => x.Int32Field).Select(g => new {
                KeyData = g.Key,
                Count = g.Count(),
                CountField = g.Count(x => x.Int32FieldNull != null),
                CountCondition = g.Count(x => x.Int16Field > 10),
                Sum = g.Sum(x => x.ByteField),
                Avg = g.Average(x => x.Int64Field),
                Max = g.Max(x => x.DateTimeField),
                Min = g.Min(x => x.DateTimeField),
            }).ToList().OrderBy(x => x.KeyData).ToList();

            var ac1 = (await context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                KeyData = x.Int32Field,
                Count = Function.Count(),
                CountField = Function.Count(x.Int32FieldNull),
                CountCondition = Function.CountCondition(x.Int16Field > 10),
                Sum = Function.Sum(x.ByteField),
                Avg = Function.Avg(x.Int64Field),
                Max = Function.Max(x.DateTimeField),
                Min = Function.Min(x.DateTimeField),
            }).ToListAsync(CancellationToken.None)).OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex1, ac1);

            var ex1_1 = list.GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                KeyData = g.Key,
                Count = g.Count(),
                CountField = g.Count(x => x.Int32FieldNull != null),
                CountCondition = g.Count(x => x.Int16Field > 10),
                Sum = g.Sum(x => x.ByteField),
                Avg = g.Average(x => x.Int64Field),
                Max = g.Max(x => x.DateTimeField),
                Min = g.Min(x => x.DateTimeField),
            }).ToList().OrderBy(x => x.KeyData).ToList();

            var ac1_1 = (await context.Query<TeBaseFieldAggregateField>().Aggregate(x => new AggregateModel {
                KeyData = x.Int32Field,
                Count = Function.Count(),
                CountField = Function.Count(x.Int32FieldNull),
                CountCondition = Function.CountCondition(x.Int16Field > 10),
                Sum = Function.Sum(x.ByteField),
                Avg = Function.Avg(x.Int64Field),
                Max = Function.Max(x.DateTimeField),
                Min = Function.Min(x.DateTimeField),
            }).ToListAsync(CancellationToken.None)).OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex1_1, ac1_1);
        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_Nofield()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            var ex1 = list.Count();
            var ex2 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Count();
            var ex3 = list.Where(x => x.Int16Field > 10).Count();
            var ex4 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Sum();
            var ex5 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Average();
            var ex6 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Max();
            var ex7 = list.Where(x => x.Int32FieldNull != null).Select(x => x.Int32FieldNull).Min();

            var obj = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                Count = Function.Count(),
                CountField = Function.Count(x.Int32FieldNull),
                CountCondition = Function.CountCondition(x.Int16Field > 10),
                Sum = Function.Sum(x.Int32FieldNull),
                Avg = Function.Avg(x.Int32FieldNull),
                Max = Function.Max(x.Int32FieldNull),
                Min = Function.Min(x.Int32FieldNull),
            }).First();

            Assert.Equal(ex1, obj.Count);
            Assert.Equal(ex2, obj.CountField);
            Assert.Equal(ex3, obj.CountCondition);
            Assert.Equal(ex4, obj.Sum);
            Assert.Equal(ex5.Value, obj.Avg.Value, 4);
            Assert.Equal(ex6, obj.Max);
            Assert.Equal(ex7, obj.Min);
        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_Where()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            List<AggregateModel> ex = null;
            List<AggregateModel> ac = null;
            ex = list
                .Where(x => x.Id > 10)
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).ToList().OrderBy(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                .Where(x => x.Id > 10)
                .Aggregate(x => new AggregateModel {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).ToList().OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
              .Where(x => x.DateTimeFieldNull != null)
              .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                  KeyData = g.Key,
                  Count = g.Count(),
                  CountField = g.Count(x => x.Int32FieldNull != null),
                  CountCondition = g.Count(x => x.Int16Field > 10),
                  Sum = g.Sum(x => x.ByteField),
                  Avg = g.Average(x => x.Int64Field),
                  Max = g.Max(x => x.DateTimeField),
                  Min = g.Min(x => x.DateTimeField),
              }).ToList().OrderBy(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
               .Where(x => x.DateTimeFieldNull != null)
               .Aggregate(x => new AggregateModel {
                   KeyData = x.Int32Field,
                   Count = Function.Count(),
                   CountField = Function.Count(x.Int32FieldNull),
                   CountCondition = Function.CountCondition(x.Int16Field > 10),
                   Sum = Function.Sum(x.ByteField),
                   Avg = Function.Avg(x.Int64Field),
                   Max = Function.Max(x.DateTimeField),
                   Min = Function.Min(x.DateTimeField),
               }).ToList().OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
              .Where(x => x.DateTimeFieldNull != null && x.Id > 10)
              .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                  KeyData = g.Key,
                  Count = g.Count(),
                  CountField = g.Count(x => x.Int32FieldNull != null),
                  CountCondition = g.Count(x => x.Int16Field > 10),
                  Sum = g.Sum(x => x.ByteField),
                  Avg = g.Average(x => x.Int64Field),
                  Max = g.Max(x => x.DateTimeField),
                  Min = g.Min(x => x.DateTimeField),
              }).ToList().OrderBy(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
               .Where(x => x.DateTimeFieldNull != null)
               .WhereWithAnd(x => x.Id > 10)
               .Aggregate(x => new AggregateModel {
                   KeyData = x.Int32Field,
                   Count = Function.Count(),
                   CountField = Function.Count(x.Int32FieldNull),
                   CountCondition = Function.CountCondition(x.Int16Field > 10),
                   Sum = Function.Sum(x.ByteField),
                   Avg = Function.Avg(x.Int64Field),
                   Max = Function.Max(x.DateTimeField),
                   Min = Function.Min(x.DateTimeField),
               }).ToList().OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
            .Where(x => x.DateTimeFieldNull != null || x.Id > 10)
            .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                KeyData = g.Key,
                Count = g.Count(),
                CountField = g.Count(x => x.Int32FieldNull != null),
                CountCondition = g.Count(x => x.Int16Field > 10),
                Sum = g.Sum(x => x.ByteField),
                Avg = g.Average(x => x.Int64Field),
                Max = g.Max(x => x.DateTimeField),
                Min = g.Min(x => x.DateTimeField),
            }).ToList().OrderBy(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Where(x => x.DateTimeFieldNull != null)
                 .WhereWithOr(x => x.Id > 10)
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.Int32Field,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).ToList().OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);
        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_OrderBy()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            List<AggregateModel> ex = null;
            List<AggregateModel> ac = null;

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                .Aggregate(x => new AggregateModel {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
              .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                  KeyData = g.Key,
                  Count = g.Count(),
                  CountField = g.Count(x => x.Int32FieldNull != null),
                  CountCondition = g.Count(x => x.Int16Field > 10),
                  Sum = g.Sum(x => x.ByteField),
                  Avg = g.Average(x => x.Int64Field),
                  Max = g.Max(x => x.DateTimeField),
                  Min = g.Min(x => x.DateTimeField),
              }).OrderBy(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
               .Aggregate(x => new AggregateModel {
                   KeyData = x.Int32Field,
                   Count = Function.Count(),
                   CountField = Function.Count(x.Int32FieldNull),
                   CountCondition = Function.CountCondition(x.Int16Field > 10),
                   Sum = Function.Sum(x.ByteField),
                   Avg = Function.Avg(x.Int64Field),
                   Max = Function.Max(x.DateTimeField),
                   Min = Function.Min(x.DateTimeField),
               }).OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.Max).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
               .Aggregate(x => new AggregateModel {
                   KeyData = x.Int32Field,
                   Count = Function.Count(),
                   CountField = Function.Count(x.Int32FieldNull),
                   CountCondition = Function.CountCondition(x.Int16Field > 10),
                   Sum = Function.Sum(x.ByteField),
                   Avg = Function.Avg(x.Int64Field),
                   Max = Function.Max(x.DateTimeField),
                   Min = Function.Min(x.DateTimeField),
               }).OrderBy(x => x.Max).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.Max).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
               .Aggregate(x => new AggregateModel {
                   KeyData = x.Int32Field,
                   Count = Function.Count(),
                   CountField = Function.Count(x.Int32FieldNull),
                   CountCondition = Function.CountCondition(x.Int16Field > 10),
                   Sum = Function.Sum(x.ByteField),
                   Avg = Function.Avg(x.Int64Field),
                   Max = Function.Max(x.DateTimeField),
                   Min = Function.Min(x.DateTimeField),
               }).OrderByDescending(x => x.Max).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.Sum).ThenBy(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
               .Aggregate(x => new AggregateModel {
                   KeyData = x.Int32Field,
                   Count = Function.Count(),
                   CountField = Function.Count(x.Int32FieldNull),
                   CountCondition = Function.CountCondition(x.Int16Field > 10),
                   Sum = Function.Sum(x.ByteField),
                   Avg = Function.Avg(x.Int64Field),
                   Max = Function.Max(x.DateTimeField),
                   Min = Function.Min(x.DateTimeField),
               }).OrderByDescending(x => x.Sum).OrderByConcat(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
               .Aggregate(x => new AggregateModel {
                   KeyData = x.Int32Field,
                   Count = Function.Count(),
                   CountField = Function.Count(x.Int32FieldNull),
                   CountCondition = Function.CountCondition(x.Int16Field > 10),
                   Sum = Function.Sum(x.ByteField),
                   Avg = Function.Avg(x.Int64Field),
                   Max = Function.Max(x.DateTimeField),
                   Min = Function.Min(x.DateTimeField),
               }).OrderByDescending(x => x.Sum).OrderBy(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = context.Query<TeBaseFieldAggregateField>()
               .Aggregate(x => new AggregateModel {
                   KeyData = x.Int32Field,
                   Count = Function.Count(),
                   CountField = Function.Count(x.Int32FieldNull),
                   CountCondition = Function.CountCondition(x.Int16Field > 10),
                   Sum = Function.Sum(x.ByteField),
                   Avg = Function.Avg(x.Int64Field),
                   Max = Function.Max(x.DateTimeField),
                   Min = Function.Min(x.DateTimeField),
               }).ToList();
            ac = context.Query<TeBaseFieldAggregateField>()
           .Aggregate(x => new AggregateModel {
               KeyData = x.Int32Field,
               Count = Function.Count(),
               CountField = Function.Count(x.Int32FieldNull),
               CountCondition = Function.CountCondition(x.Int16Field > 10),
               Sum = Function.Sum(x.ByteField),
               Avg = Function.Avg(x.Int64Field),
               Max = Function.Max(x.DateTimeField),
               Min = Function.Min(x.DateTimeField),
           }).OrderByDescending(x => x.Sum).OrderByReset().ToList();
            AssertExtend.StrictEqual(ex, ac);
        }



        [Fact]
        public void TestCase_Aggregate_GroupBy_Having()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            List<AggregateModel> ex = null;
            List<AggregateModel> ac = null;

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).Where(x => x.Sum > 10).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                .Aggregate(x => new AggregateModel {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).Having(x => x.Sum > 10).ToList().OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                 .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                     KeyData = g.Key,
                     Count = g.Count(),
                     CountField = g.Count(x => x.Int32FieldNull != null),
                     CountCondition = g.Count(x => x.Int16Field > 10),
                     Sum = g.Sum(x => x.ByteField),
                     Avg = g.Average(x => x.Int64Field),
                     Max = g.Max(x => x.DateTimeField),
                     Min = g.Min(x => x.DateTimeField),
                 }).Where(x => x.Sum > 10 && x.Sum < 20).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                .Aggregate(x => new AggregateModel {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).Having(x => x.Sum > 10 && x.Sum < 20).ToList().OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                 .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                     KeyData = g.Key,
                     Count = g.Count(),
                     CountField = g.Count(x => x.Int32FieldNull != null),
                     CountCondition = g.Count(x => x.Int16Field > 10),
                     Sum = g.Sum(x => x.ByteField),
                     Avg = g.Average(x => x.Int64Field),
                     Max = g.Max(x => x.DateTimeField),
                     Min = g.Min(x => x.DateTimeField),
                 }).Where(x => x.Sum > 10 && x.Sum < 20).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                .Aggregate(x => new AggregateModel {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).Having(x => x.Sum > 10).HavingWithAnd(x => x.Sum < 20).ToList().OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).Where(x => x.Sum < 10 || x.Sum > 20).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                .Aggregate(x => new AggregateModel {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).Having(x => x.Sum < 10).HavingWithOr(x => x.Sum > 20).ToList().OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).Where(x => x.Sum > 20).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                .Aggregate(x => new AggregateModel {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).Having(x => x.Sum < 10).Having(x => x.Sum > 20).ToList().OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
                .GroupBy(x => x.Int32Field).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                .Aggregate(x => new AggregateModel {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).Having(x => x.Sum < 10).HavingReset().ToList().OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);
        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_Date()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            var ex = list
                  .GroupBy(x => x.DateTimeField.Date).Select(g => new {
                      KeyData = g.Key,
                      Count = g.Count(),
                      CountField = g.Count(x => x.Int32FieldNull != null),
                      CountCondition = g.Count(x => x.Int16Field > 10),
                      Sum = g.Sum(x => x.ByteField),
                      Avg = g.Average(x => x.Int64Field),
                      Max = g.Max(x => x.DateTimeField),
                      Min = g.Min(x => x.DateTimeField),
                  }).OrderByDescending(x => x.KeyData).ToList();

            var ac = context.Query<TeBaseFieldAggregateField>()
                  .Aggregate(x => new {
                      KeyData = x.DateTimeField.Date,
                      Count = Function.Count(),
                      CountField = Function.Count(x.Int32FieldNull),
                      CountCondition = Function.CountCondition(x.Int16Field > 10),
                      Sum = Function.Sum(x.ByteField),
                      Avg = Function.Avg(x.Int64Field),
                      Max = Function.Max(x.DateTimeField),
                      Min = Function.Min(x.DateTimeField),
                  }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_DateTime()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            List<AggregateModel> ex = null;
            List<AggregateModel> ac = null;

            ex = list
                 .GroupBy(x => x.DateTimeField.Year).Select(g => new AggregateModel {
                     KeyData = g.Key,
                     Count = g.Count(),
                     CountField = g.Count(x => x.Int32FieldNull != null),
                     CountCondition = g.Count(x => x.Int16Field > 10),
                     Sum = g.Sum(x => x.ByteField),
                     Avg = g.Average(x => x.Int64Field),
                     Max = g.Max(x => x.DateTimeField),
                     Min = g.Min(x => x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.DateTimeField.Year,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
               .GroupBy(x => x.DateTimeField.Month).Select(g => new AggregateModel {
                   KeyData = g.Key,
                   Count = g.Count(),
                   CountField = g.Count(x => x.Int32FieldNull != null),
                   CountCondition = g.Count(x => x.Int16Field > 10),
                   Sum = g.Sum(x => x.ByteField),
                   Avg = g.Average(x => x.Int64Field),
                   Max = g.Max(x => x.DateTimeField),
                   Min = g.Min(x => x.DateTimeField),
               }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.DateTimeField.Month,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);


            ex = list
               .GroupBy(x => x.DateTimeField.Month).Select(g => new AggregateModel {
                   KeyData = g.Key,
                   Count = g.Count(),
                   CountField = g.Count(x => x.Int32FieldNull != null),
                   CountCondition = g.Count(x => x.Int16Field > 10),
                   Sum = g.Sum(x => x.ByteField),
                   Avg = g.Average(x => x.Int64Field),
                   Max = g.Max(x => x.DateTimeField),
                   Min = g.Min(x => x.DateTimeField),
               }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.DateTimeField.Month,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);


            ex = list
                .GroupBy(x => x.DateTimeField.Month).Select(g => new AggregateModel {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.DateTimeField.Month,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);


            ex = list
                 .GroupBy(x => x.DateTimeField.Day).Select(g => new AggregateModel {
                     KeyData = g.Key,
                     Count = g.Count(),
                     CountField = g.Count(x => x.Int32FieldNull != null),
                     CountCondition = g.Count(x => x.Int16Field > 10),
                     Sum = g.Sum(x => x.ByteField),
                     Avg = g.Average(x => x.Int64Field),
                     Max = g.Max(x => x.DateTimeField),
                     Min = g.Min(x => x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.DateTimeField.Day,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);


            ex = list
              .GroupBy(x => x.DateTimeField.Hour).Select(g => new AggregateModel {
                  KeyData = g.Key,
                  Count = g.Count(),
                  CountField = g.Count(x => x.Int32FieldNull != null),
                  CountCondition = g.Count(x => x.Int16Field > 10),
                  Sum = g.Sum(x => x.ByteField),
                  Avg = g.Average(x => x.Int64Field),
                  Max = g.Max(x => x.DateTimeField),
                  Min = g.Min(x => x.DateTimeField),
              }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.DateTimeField.Hour,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
               .GroupBy(x => x.DateTimeField.Minute).Select(g => new AggregateModel {
                   KeyData = g.Key,
                   Count = g.Count(),
                   CountField = g.Count(x => x.Int32FieldNull != null),
                   CountCondition = g.Count(x => x.Int16Field > 10),
                   Sum = g.Sum(x => x.ByteField),
                   Avg = g.Average(x => x.Int64Field),
                   Max = g.Max(x => x.DateTimeField),
                   Min = g.Min(x => x.DateTimeField),
               }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.DateTimeField.Minute,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

            ex = list
              .GroupBy(x => x.DateTimeField.Second).Select(g => new AggregateModel {
                  KeyData = g.Key,
                  Count = g.Count(),
                  CountField = g.Count(x => x.Int32FieldNull != null),
                  CountCondition = g.Count(x => x.Int16Field > 10),
                  Sum = g.Sum(x => x.ByteField),
                  Avg = g.Average(x => x.Int64Field),
                  Max = g.Max(x => x.DateTimeField),
                  Min = g.Min(x => x.DateTimeField),
              }).OrderByDescending(x => x.KeyData).ToList();

            ac = context.Query<TeBaseFieldAggregateField>()
                 .Aggregate(x => new AggregateModel {
                     KeyData = x.DateTimeField.Second,
                     Count = Function.Count(),
                     CountField = Function.Count(x.Int32FieldNull),
                     CountCondition = Function.CountCondition(x.Int16Field > 10),
                     Sum = Function.Sum(x.ByteField),
                     Avg = Function.Avg(x.Int64Field),
                     Max = Function.Max(x.DateTimeField),
                     Min = Function.Min(x.DateTimeField),
                 }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);
        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_DateTime_Format()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            List<string> formats = new List<string> {
                "yyyyMMdd",
                "yyyyMM",
                "yyyy-MM-dd",
                "yyyy-MM",
                "dd-MM-yyyy",
                "MM-dd-yyyy",
                "yyyy/MM/dd",
                "yyyy/MM",
                "dd/MM/yyyy",
                "MM/dd/yyyy"
            };
            foreach (string format in formats) {
                var ex = list
                      .GroupBy(x => x.DateTimeField.ToString(format)).Select(g => new {
                          KeyData = g.Key,
                          Count = g.Count(),
                          CountField = g.Count(x => x.Int32FieldNull != null),
                          CountCondition = g.Count(x => x.Int16Field > 10),
                          Sum = g.Sum(x => x.ByteField),
                          Avg = g.Average(x => x.Int64Field),
                          Max = g.Max(x => x.DateTimeField),
                          Min = g.Min(x => x.DateTimeField),
                      }).OrderByDescending(x => x.KeyData).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>()
                      .Aggregate(x => new {
                          KeyData = x.DateTimeField.ToString(format),
                          Count = Function.Count(),
                          CountField = Function.Count(x.Int32FieldNull),
                          CountCondition = Function.CountCondition(x.Int16Field > 10),
                          Sum = Function.Sum(x.ByteField),
                          Avg = Function.Avg(x.Int64Field),
                          Max = Function.Max(x.DateTimeField),
                          Min = Function.Min(x.DateTimeField),
                      }).OrderByDescending(x => x.KeyData).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_String()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            var ex = list
                  .GroupBy(x => x.VarcharField.Substring(0, 9)).Select(g => new {
                      KeyData = g.Key,
                      Count = g.Count(),
                      CountField = g.Count(x => x.Int32FieldNull != null),
                      CountCondition = g.Count(x => x.Int16Field > 10),
                      Sum = g.Sum(x => x.ByteField),
                      Avg = g.Average(x => x.Int64Field),
                      Max = g.Max(x => x.DateTimeField),
                      Min = g.Min(x => x.DateTimeField),
                  }).OrderByDescending(x => x.KeyData).ToList();

            var ac = context.Query<TeBaseFieldAggregateField>()
                  .Aggregate(x => new {
                      KeyData = x.VarcharField.Substring(0, 9),
                      Count = Function.Count(),
                      CountField = Function.Count(x.Int32FieldNull),
                      CountCondition = Function.CountCondition(x.Int16Field > 10),
                      Sum = Function.Sum(x.ByteField),
                      Avg = Function.Avg(x.Int64Field),
                      Max = Function.Max(x.DateTimeField),
                      Min = Function.Min(x.DateTimeField),
                  }).OrderByDescending(x => x.KeyData).ToList();
            AssertExtend.StrictEqual(ex, ac);

        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_SelectInsert()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(200);
            context.TruncateTable<TeBaseFieldAggregateFieldGroupBy>();
            var ex = list
                  .Where(x => x.Id > 10)
                  .GroupBy(x => new {
                      KeyData = x.Int32Field,
                      MonthData = x.DateTimeField.Month
                  }).Select(g => new TeBaseFieldAggregateFieldGroupBy {
                      KeyData = g.Key.KeyData,
                      MonthData = g.Key.MonthData,
                      CountData = g.Count(),
                      CountFieldData = g.Count(x => x.Int32FieldNull != null),
                      CountConditionData = g.Count(x => x.Int16Field > 10),
                      SumData = g.Sum(x => x.ByteField),
                      AvgData = g.Average(x => x.Int64Field),
                      MaxData = g.Max(x => x.DateTimeField),
                      MinData = g.Min(x => x.DateTimeField),
                  }).ToList().OrderBy(x => x.KeyData).ThenBy(x => x.MonthData).ToList();
            
            var ret = context.Query<TeBaseFieldAggregateField>()
                 .Where(x => x.Id > 10)
                 .Aggregate(x => new {
                     KeyData = x.Int32Field,
                     MonthData = x.DateTimeField.Month,
                     CountData = Function.Count(),
                     CountFieldData = Function.Count(x.Int32FieldNull),
                     CountConditionData = Function.CountCondition(x.Int16Field > 10),
                     SumData = Function.Sum(x.ByteField),
                     AvgData = Function.Avg(x.Int64Field),
                     MaxData = Function.Max(x.DateTimeField),
                     MinData = Function.Min(x.DateTimeField),
                 }).OrderBy(x => x.KeyData).OrderByConcat(x => x.MonthData)
                 .SelectInsert(x => new TeBaseFieldAggregateFieldGroupBy() {
                     KeyData = x.KeyData,
                     MonthData = x.MonthData,
                     CountData = x.CountData,
                     CountFieldData = x.CountFieldData,
                     CountConditionData = x.CountConditionData,
                     SumData = x.SumData,
                     AvgData = x.AvgData,
                     MaxData = x.MaxData,
                     MinData = x.MinData
                 });
            Assert.Equal(ex.Count, ret);
            var ac = context.Query<TeBaseFieldAggregateFieldGroupBy>().ToList();

            AssertExtend.StrictEqual(ex, ac);
        }

        [Fact]
        public async Task TestCase_Aggregate_GroupBy_SelectInsertAsync()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(200);
            context.TruncateTable<TeBaseFieldAggregateFieldGroupBy>();
            var ex = list
                  .Where(x => x.Id > 10)
                  .GroupBy(x => new {
                      KeyData = x.Int32Field,
                      MonthData = x.DateTimeField.Month
                  }).Select(g => new TeBaseFieldAggregateFieldGroupBy {
                      KeyData = g.Key.KeyData,
                      MonthData = g.Key.MonthData,
                      CountData = g.Count(),
                      CountFieldData = g.Count(x => x.Int32FieldNull != null),
                      CountConditionData = g.Count(x => x.Int16Field > 10),
                      SumData = g.Sum(x => x.ByteField),
                      AvgData = g.Average(x => x.Int64Field),
                      MaxData = g.Max(x => x.DateTimeField),
                      MinData = g.Min(x => x.DateTimeField),
                  }).ToList().OrderBy(x => x.KeyData).ThenBy(x => x.MonthData).ToList();

            var ret = await context.Query<TeBaseFieldAggregateField>()
                 .Where(x => x.Id > 10)
                 .Aggregate(x => new {
                     KeyData = x.Int32Field,
                     MonthData = x.DateTimeField.Month,
                     CountData = Function.Count(),
                     CountFieldData = Function.Count(x.Int32FieldNull),
                     CountConditionData = Function.CountCondition(x.Int16Field > 10),
                     SumData = Function.Sum(x.ByteField),
                     AvgData = Function.Avg(x.Int64Field),
                     MaxData = Function.Max(x.DateTimeField),
                     MinData = Function.Min(x.DateTimeField),
                 }).OrderBy(x => x.KeyData).OrderByConcat(x => x.MonthData)
                 .SelectInsertAsync(x => new TeBaseFieldAggregateFieldGroupBy() {
                     KeyData = x.KeyData,
                     MonthData = x.MonthData,
                     CountData = x.CountData,
                     CountFieldData = x.CountFieldData,
                     CountConditionData = x.CountConditionData,
                     SumData = x.SumData,
                     AvgData = x.AvgData,
                     MaxData = x.MaxData,
                     MinData = x.MinData
                 }, CancellationToken.None);
            Assert.Equal(ex.Count, ret);
            var ac = context.Query<TeBaseFieldAggregateFieldGroupBy>().ToList();

            AssertExtend.StrictEqual(ex, ac);
        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_Single()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).First();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderBy(x => x.KeyData).First();
                AssertExtend.StrictEqual(ex, ac);
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).ElementAt(5);

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderBy(x => x.KeyData).ElementAt(5);
                AssertExtend.StrictEqual(ex, ac);
            }
        }

        [Fact]
        public async Task TestCase_Aggregate_GroupBy_SingleAsync()
        {
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);
            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).First();

                var ac = await context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderBy(x => x.KeyData).FirstAsync(CancellationToken.None);
                AssertExtend.StrictEqual(ex, ac);
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).ElementAt(5);

                var ac = await context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderBy(x => x.KeyData).ElementAtAsync(5, CancellationToken.None);
                AssertExtend.StrictEqual(ex, ac);
            }
        }

        [Fact]
        public void TestCase_Aggregate_GroupBy_PageSize()
        {
            const int tol = 21;
            const int cnt = 8;
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                {
                    var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                        KeyData = g.Key,
                        Count = g.Count(),
                        CountField = g.Count(x => x.Int32FieldNull != null),
                        CountCondition = g.Count(x => x.Int16Field > 10),
                        Sum = g.Sum(x => x.ByteField),
                        Avg = g.Average(x => x.Int64Field),
                        Max = g.Max(x => x.DateTimeField),
                        Min = g.Min(x => x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Skip(cnt * i).Take(cnt).ToList();

                    var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                        KeyData = x.Int32Field,
                        Count = Function.Count(),
                        CountField = Function.Count(x.Int32FieldNull),
                        CountCondition = Function.CountCondition(x.Int16Field > 10),
                        Sum = Function.Sum(x.ByteField),
                        Avg = Function.Avg(x.Int64Field),
                        Max = Function.Max(x.DateTimeField),
                        Min = Function.Min(x.DateTimeField),
                    }).OrderBy(x => x.KeyData).PageSize(i + 1, cnt).ToList();
                    AssertExtend.StrictEqual(ex, ac);
                }

                {
                    var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                        KeyData = g.Key,
                        Count = g.Count(),
                        CountField = g.Count(x => x.Int32FieldNull != null),
                        CountCondition = g.Count(x => x.Int16Field > 10),
                        Sum = g.Sum(x => x.ByteField),
                        Avg = g.Average(x => x.Int64Field),
                        Max = g.Max(x => x.DateTimeField),
                        Min = g.Min(x => x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Skip(cnt * i).Take(cnt).ToList();

                    var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                        KeyData = x.Int32Field,
                        Count = Function.Count(),
                        CountField = Function.Count(x.Int32FieldNull),
                        CountCondition = Function.CountCondition(x.Int16Field > 10),
                        Sum = Function.Sum(x.ByteField),
                        Avg = Function.Avg(x.Int64Field),
                        Max = Function.Max(x.DateTimeField),
                        Min = Function.Min(x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Range(i * cnt, (i + 1) * cnt).ToList();
                    AssertExtend.StrictEqual(ex, ac);
                }
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).Where(x => x.KeyData > cnt).Take(cnt).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderBy(x => x.KeyData).Having(x => x.KeyData > cnt).PageSize(1, cnt).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).Take(cnt).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).PageSize(1, cnt).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).Where(x => x.KeyData > cnt).Take(cnt).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderBy(x => x.KeyData).Having(x => x.KeyData > cnt).Range(0, cnt).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).Take(cnt).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).Range(0, cnt).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

        }


        [Fact]
        public void TestCase_Aggregate_GroupBy_TakeSkip()
        {
            const int tol = 21;
            const int cnt = 8;
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                {
                    var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                        KeyData = g.Key,
                        Count = g.Count(),
                        CountField = g.Count(x => x.Int32FieldNull != null),
                        CountCondition = g.Count(x => x.Int16Field > 10),
                        Sum = g.Sum(x => x.ByteField),
                        Avg = g.Average(x => x.Int64Field),
                        Max = g.Max(x => x.DateTimeField),
                        Min = g.Min(x => x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Skip(cnt * i).ToList();

                    var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                        KeyData = x.Int32Field,
                        Count = Function.Count(),
                        CountField = Function.Count(x.Int32FieldNull),
                        CountCondition = Function.CountCondition(x.Int16Field > 10),
                        Sum = Function.Sum(x.ByteField),
                        Avg = Function.Avg(x.Int64Field),
                        Max = Function.Max(x.DateTimeField),
                        Min = Function.Min(x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Skip(cnt * i).ToList();
                    AssertExtend.StrictEqual(ex, ac);
                }

                {
                    var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                        KeyData = g.Key,
                        Count = g.Count(),
                        CountField = g.Count(x => x.Int32FieldNull != null),
                        CountCondition = g.Count(x => x.Int16Field > 10),
                        Sum = g.Sum(x => x.ByteField),
                        Avg = g.Average(x => x.Int64Field),
                        Max = g.Max(x => x.DateTimeField),
                        Min = g.Min(x => x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Take(cnt * i).ToList();

                    var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                        KeyData = x.Int32Field,
                        Count = Function.Count(),
                        CountField = Function.Count(x.Int32FieldNull),
                        CountCondition = Function.CountCondition(x.Int16Field > 10),
                        Sum = Function.Sum(x.ByteField),
                        Avg = Function.Avg(x.Int64Field),
                        Max = Function.Max(x.DateTimeField),
                        Min = Function.Min(x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Take(cnt * i).ToList();
                    AssertExtend.StrictEqual(ex, ac);
                }

                {
                    var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                        KeyData = g.Key,
                        Count = g.Count(),
                        CountField = g.Count(x => x.Int32FieldNull != null),
                        CountCondition = g.Count(x => x.Int16Field > 10),
                        Sum = g.Sum(x => x.ByteField),
                        Avg = g.Average(x => x.Int64Field),
                        Max = g.Max(x => x.DateTimeField),
                        Min = g.Min(x => x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Skip(cnt * i).Take(cnt).ToList();

                    var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                        KeyData = x.Int32Field,
                        Count = Function.Count(),
                        CountField = Function.Count(x.Int32FieldNull),
                        CountCondition = Function.CountCondition(x.Int16Field > 10),
                        Sum = Function.Sum(x.ByteField),
                        Avg = Function.Avg(x.Int64Field),
                        Max = Function.Max(x.DateTimeField),
                        Min = Function.Min(x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Skip(cnt * i).Take(cnt).ToList();
                    AssertExtend.StrictEqual(ex, ac);
                }
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).Where(x => x.KeyData > cnt).Take(cnt).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderBy(x => x.KeyData).Having(x => x.KeyData > cnt).Take(cnt).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).Take(cnt).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).Take(cnt).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

        }
        #endregion

        [Fact]
        public void TestCase_Aggregate_GroupBy_TakeSkip_Ver2012()
        {
            DataContext context = CreateContext("mssql_2012");
            const int tol = 21;
            const int cnt = 8;
            List<TeBaseFieldAggregateField> list = CreateAndInsertBaseFieldTableList(45);

            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                {
                    var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                        KeyData = g.Key,
                        Count = g.Count(),
                        CountField = g.Count(x => x.Int32FieldNull != null),
                        CountCondition = g.Count(x => x.Int16Field > 10),
                        Sum = g.Sum(x => x.ByteField),
                        Avg = g.Average(x => x.Int64Field),
                        Max = g.Max(x => x.DateTimeField),
                        Min = g.Min(x => x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Skip(cnt * i).ToList();

                    var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                        KeyData = x.Int32Field,
                        Count = Function.Count(),
                        CountField = Function.Count(x.Int32FieldNull),
                        CountCondition = Function.CountCondition(x.Int16Field > 10),
                        Sum = Function.Sum(x.ByteField),
                        Avg = Function.Avg(x.Int64Field),
                        Max = Function.Max(x.DateTimeField),
                        Min = Function.Min(x.DateTimeField),
                    }).Skip(cnt * i).ToList();
                    AssertExtend.StrictEqual(ex, ac);
                }

                {
                    var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                        KeyData = g.Key,
                        Count = g.Count(),
                        CountField = g.Count(x => x.Int32FieldNull != null),
                        CountCondition = g.Count(x => x.Int16Field > 10),
                        Sum = g.Sum(x => x.ByteField),
                        Avg = g.Average(x => x.Int64Field),
                        Max = g.Max(x => x.DateTimeField),
                        Min = g.Min(x => x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Take(cnt * i).ToList();

                    var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                        KeyData = x.Int32Field,
                        Count = Function.Count(),
                        CountField = Function.Count(x.Int32FieldNull),
                        CountCondition = Function.CountCondition(x.Int16Field > 10),
                        Sum = Function.Sum(x.ByteField),
                        Avg = Function.Avg(x.Int64Field),
                        Max = Function.Max(x.DateTimeField),
                        Min = Function.Min(x.DateTimeField),
                    }).Take(cnt * i).ToList();
                    AssertExtend.StrictEqual(ex, ac);
                }

                {
                    var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                        KeyData = g.Key,
                        Count = g.Count(),
                        CountField = g.Count(x => x.Int32FieldNull != null),
                        CountCondition = g.Count(x => x.Int16Field > 10),
                        Sum = g.Sum(x => x.ByteField),
                        Avg = g.Average(x => x.Int64Field),
                        Max = g.Max(x => x.DateTimeField),
                        Min = g.Min(x => x.DateTimeField),
                    }).OrderBy(x => x.KeyData).Skip(cnt * i).Take(cnt).ToList();

                    var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                        KeyData = x.Int32Field,
                        Count = Function.Count(),
                        CountField = Function.Count(x.Int32FieldNull),
                        CountCondition = Function.CountCondition(x.Int16Field > 10),
                        Sum = Function.Sum(x.ByteField),
                        Avg = Function.Avg(x.Int64Field),
                        Max = Function.Max(x.DateTimeField),
                        Min = Function.Min(x.DateTimeField),
                    }).Skip(cnt * i).Take(cnt).ToList();
                    AssertExtend.StrictEqual(ex, ac);
                }
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderBy(x => x.KeyData).Where(x => x.KeyData > cnt).Take(cnt).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderBy(x => x.KeyData).Having(x => x.KeyData > cnt).Take(cnt).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

            {
                var ex = list.GroupBy(x => x.Int32Field).Select(g => new {
                    KeyData = g.Key,
                    Count = g.Count(),
                    CountField = g.Count(x => x.Int32FieldNull != null),
                    CountCondition = g.Count(x => x.Int16Field > 10),
                    Sum = g.Sum(x => x.ByteField),
                    Avg = g.Average(x => x.Int64Field),
                    Max = g.Max(x => x.DateTimeField),
                    Min = g.Min(x => x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).Take(cnt).ToList();

                var ac = context.Query<TeBaseFieldAggregateField>().Aggregate(x => new {
                    KeyData = x.Int32Field,
                    Count = Function.Count(),
                    CountField = Function.Count(x.Int32FieldNull),
                    CountCondition = Function.CountCondition(x.Int16Field > 10),
                    Sum = Function.Sum(x.ByteField),
                    Avg = Function.Avg(x.Int64Field),
                    Max = Function.Max(x.DateTimeField),
                    Min = Function.Min(x.DateTimeField),
                }).OrderByDescending(x => x.KeyData).Take(cnt).ToList();
                AssertExtend.StrictEqual(ex, ac);
            }

        }
    }
}