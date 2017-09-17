using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Light.Data.Postgre.Test
{
    public class Postgre_BaseFieldSelectTest : BaseTest
    {
        public Postgre_BaseFieldSelectTest(ITestOutputHelper output) : base(output)
        {
        }

        #region base test
        List<TeBaseFieldSelectField> CreateBaseFieldTableList(int count)
        {
            List<TeBaseFieldSelectField> list = new List<TeBaseFieldSelectField>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeBaseFieldSelectField item = new TeBaseFieldSelectField();
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
                item.DateTimeField = d.AddMinutes(i * 2);
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

        List<TeBaseFieldSelectField> CreateAndInsertBaseFieldTableList(int count)
        {
            var list = CreateBaseFieldTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeBaseFieldSelectField>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        #region select single field

        [Fact]
        public async Task TestCase_Query_Select_Id_Async()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Id).ToList();
            var listAc = await context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id).ToListAsync();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = list.Select(x => x.Id).ToArray();
            var arrayAc = await context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id).ToArrayAsync();
            AssertExtend.StrictEqual(arrayEx, arrayAc);
        }

        [Fact]
        public void TestCase_Query_Select_Id()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Id).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = list.Select(x => x.Id).ToArray();
            var arrayAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id).ToArray();
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var selectEx = list.Select(x => x.Id);
            var selectAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id);
            AssertExtend.Equal(selectEx, selectAc);
        }

        [Fact]
        public async Task TestCase_Query_Select_Id_Single_Async()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var firstEx = list.Select(x => x.Id).First();
            var firstAc = await context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id).FirstAsync();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = list.Select(x => x.Id).ElementAt(10);
            var elementAc = await context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id).ElementAtAsync(10);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public void TestCase_Query_Select_Id_Single()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var firstEx = list.Select(x => x.Id).First();
            var firstAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id).First();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = list.Select(x => x.Id).ElementAt(10);
            var elementAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Id).ElementAt(10);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public void TestCase_Query_Select_Bool()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.BoolField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.BoolField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Bool_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.BoolFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.BoolFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Byte()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.ByteField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.ByteField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Byte_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.ByteFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.ByteFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Sbyte()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.SbyteField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.SbyteField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Sbyte_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.SbyteFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.SbyteFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int16()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int16Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Int16Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int16_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int16FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Int16FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int32()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int32Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int32_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int32FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Int32FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int64()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int64Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Int64Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int64_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int64FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.Int64FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt16()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt16Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.UInt16Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt16_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt16FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.UInt16FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt32()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt32Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.UInt32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt32_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt32FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.UInt32FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt64()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt64Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.UInt64Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt64_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt64FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.UInt64FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Float()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.FloatField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.FloatField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Float_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.FloatFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.FloatFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Double()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DoubleField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Double_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DoubleFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.DoubleFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }


        [Fact]
        public void TestCase_Query_Select_Decimal()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DecimalField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.DecimalField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Decimal_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DecimalFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.DecimalFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_DateTime()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DateTimeField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.DateTimeField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_DateTime_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DateTimeFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.DateTimeFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Varchar()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.VarcharField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.VarcharField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Varchar_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.VarcharFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.VarcharFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Text()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.TextField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.TextField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Text_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.TextFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.TextFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_BigData()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.BigDataField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.BigDataField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_BigData_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.BigDataFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.BigDataFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Enum32()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.EnumInt32Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.EnumInt32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Enum32_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.EnumInt32FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.EnumInt32FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Enum64()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.EnumInt64Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.EnumInt64Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Enum64_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.EnumInt64FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().SelectField(x => x.EnumInt64FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        #endregion

        #region select single field 2

        [Fact]
        public async Task TestCase_Query_Select_Id_M2_Async()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Id).ToList();
            var listAc = await context.Query<TeBaseFieldSelectField>().Select(x => x.Id).ToListAsync();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = list.Select(x => x.Id).ToArray();
            var arrayAc = await context.Query<TeBaseFieldSelectField>().Select(x => x.Id).ToArrayAsync();
            AssertExtend.StrictEqual(arrayEx, arrayAc);
        }

        [Fact]
        public void TestCase_Query_Select_Id_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Id).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = list.Select(x => x.Id).ToArray();
            var arrayAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Id).ToArray();
            AssertExtend.StrictEqual(arrayEx, arrayAc);
        }


        [Fact]
        public async Task TestCase_Query_Select_Id_Single_M2_Async()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var firstEx = list.Select(x => x.Id).First();
            var firstAc = await context.Query<TeBaseFieldSelectField>().Select(x => x.Id).FirstAsync();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = list.Select(x => x.Id).ElementAt(10);
            var elementAc = await context.Query<TeBaseFieldSelectField>().Select(x => x.Id).ElementAtAsync(10);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public void TestCase_Query_Select_Id_Single_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var firstEx = list.Select(x => x.Id).First();
            var firstAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Id).First();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = list.Select(x => x.Id).ElementAt(10);
            var elementAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Id).ElementAt(10);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public void TestCase_Query_Select_Bool_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.BoolField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.BoolField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Bool_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.BoolFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.BoolFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Byte_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.ByteField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.ByteField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Byte_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.ByteFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.ByteFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Sbyte_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.SbyteField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.SbyteField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Sbyte_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.SbyteFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.SbyteFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int16_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int16Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Int16Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int16_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int16FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Int16FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int32_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int32Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int32_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int32FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Int32FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int64_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int64Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Int64Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Int64_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.Int64FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.Int64FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt16_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt16Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.UInt16Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt16_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt16FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.UInt16FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt32_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt32Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.UInt32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt32_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt32FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.UInt32FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt64_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt64Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.UInt64Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_UInt64_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.UInt64FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.UInt64FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Float_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.FloatField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.FloatField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Float_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.FloatFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.FloatFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Double_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DoubleField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Double_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DoubleFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.DoubleFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }


        [Fact]
        public void TestCase_Query_Select_Decimal_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DecimalField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.DecimalField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Decimal_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DecimalFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.DecimalFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_DateTime_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DateTimeField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.DateTimeField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_DateTime_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.DateTimeFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.DateTimeFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Varchar_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.VarcharField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.VarcharField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Varchar_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.VarcharFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.VarcharFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Text_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.TextField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.TextField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Text_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.TextFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.TextFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_BigData_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.BigDataField).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.BigDataField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_BigData_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.BigDataFieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.BigDataFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Enum32_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.EnumInt32Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.EnumInt32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Enum32_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.EnumInt32FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.EnumInt32FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Enum64_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.EnumInt64Field).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.EnumInt64Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Enum64_Null_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => x.EnumInt64FieldNull).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => x.EnumInt64FieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        #endregion

        #region select multi
        [Fact]
        public void TestCase_Query_Select_Multi_Fields()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ToArray();
            var arrayAc = context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ToArray();
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var selectEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            });
            var selectAc = context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            });
            AssertExtend.Equal(selectEx, selectAc);

            var firstEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).First();
            var firstAc = context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).First();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ElementAt(10);
            var elementAc = context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ElementAt(10);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public async Task TestCase_Query_Select_Multi_Fields_Async()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ToList();
            var listAc = await context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ToListAsync();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ToArray();
            var arrayAc = await context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ToArrayAsync();
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var firstEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).First();
            var firstAc = await context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).FirstAsync();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = list.Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ElementAt(10);
            var elementAc = await context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.Id,
                x.BoolField,
                x.SbyteField,
                x.ByteField,
                x.Int16Field,
                x.Int32Field,
                x.Int64Field,
                x.UInt16Field,
                x.UInt32Field,
                x.UInt64Field,
                x.FloatField,
                x.DoubleField,
                x.DecimalField,
                x.DateTimeField,
                x.VarcharField,
                x.TextField,
                x.BigDataField
            }).ElementAtAsync(10);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public void TestCase_Query_Select_Multi_Fields_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => new {
                x.BoolFieldNull,
                x.SbyteFieldNull,
                x.ByteFieldNull,
                x.Int16FieldNull,
                x.Int32FieldNull,
                x.Int64FieldNull,
                x.UInt16FieldNull,
                x.UInt32FieldNull,
                x.UInt64FieldNull,
                x.FloatFieldNull,
                x.DoubleFieldNull,
                x.DecimalFieldNull,
                x.DateTimeFieldNull,
                x.VarcharFieldNull,
                x.TextFieldNull,
                x.BigDataFieldNull
            }).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => new {
                x.BoolFieldNull,
                x.SbyteFieldNull,
                x.ByteFieldNull,
                x.Int16FieldNull,
                x.Int32FieldNull,
                x.Int64FieldNull,
                x.UInt16FieldNull,
                x.UInt32FieldNull,
                x.UInt64FieldNull,
                x.FloatFieldNull,
                x.DoubleFieldNull,
                x.DecimalFieldNull,
                x.DateTimeFieldNull,
                x.VarcharFieldNull,
                x.TextFieldNull,
                x.BigDataFieldNull
            }).ToList();
            AssertExtend.Equal(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Select_Multi_Fields_M2()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ToArray();
            var arrayAc = context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ToArray();
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var selectEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            });
            var selectAc = context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            });
            AssertExtend.Equal(selectEx, selectAc);

            var firstEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).First();
            var firstAc = context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).First();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ElementAt(10);
            var elementAc = context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ElementAt(10);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public async Task TestCase_Query_Select_Multi_Fields_M2_Async()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ToList();
            var listAc = await context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ToListAsync();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ToArray();
            var arrayAc = await context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ToArrayAsync();
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var firstEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).First();
            var firstAc = await context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).FirstAsync();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = list.Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ElementAt(10);
            var elementAc = await context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModel {
                Id = x.Id,
                BoolField = x.BoolField,
                SbyteField = x.SbyteField,
                ByteField = x.ByteField,
                Int16Field = x.Int16Field,
                Int32Field = x.Int32Field,
                Int64Field = x.Int64Field,
                UInt16Field = x.UInt16Field,
                UInt32Field = x.UInt32Field,
                UInt64Field = x.UInt64Field,
                FloatField = x.FloatField,
                DoubleField = x.DoubleField,
                DecimalField = x.DecimalField,
                DateTimeField = x.DateTimeField,
                VarcharField = x.VarcharField,
                TextField = x.TextField,
                BigDataField = x.BigDataField
            }).ElementAtAsync(10);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public void TestCase_Query_Select_Multi_Fields_M2_Null()
        {
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            var listEx = list.Select(x => new BaseFieldSelectModelNull {
                Id = x.Id,
                BoolFieldNull = x.BoolFieldNull,
                SbyteFieldNull = x.SbyteFieldNull,
                ByteFieldNull = x.ByteFieldNull,
                Int16FieldNull = x.Int16FieldNull,
                Int32FieldNull = x.Int32FieldNull,
                Int64FieldNull = x.Int64FieldNull,
                UInt16FieldNull = x.UInt16FieldNull,
                UInt32FieldNull = x.UInt32FieldNull,
                UInt64FieldNull = x.UInt64FieldNull,
                FloatFieldNull = x.FloatFieldNull,
                DoubleFieldNull = x.DoubleFieldNull,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharFieldNull = x.VarcharFieldNull,
                TextFieldNull = x.TextFieldNull,
                BigDataFieldNull = x.BigDataFieldNull
            }).ToList();
            var listAc = context.Query<TeBaseFieldSelectField>().Select(x => new BaseFieldSelectModelNull {
                Id = x.Id,
                BoolFieldNull = x.BoolFieldNull,
                SbyteFieldNull = x.SbyteFieldNull,
                ByteFieldNull = x.ByteFieldNull,
                Int16FieldNull = x.Int16FieldNull,
                Int32FieldNull = x.Int32FieldNull,
                Int64FieldNull = x.Int64FieldNull,
                UInt16FieldNull = x.UInt16FieldNull,
                UInt32FieldNull = x.UInt32FieldNull,
                UInt64FieldNull = x.UInt64FieldNull,
                FloatFieldNull = x.FloatFieldNull,
                DoubleFieldNull = x.DoubleFieldNull,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharFieldNull = x.VarcharFieldNull,
                TextFieldNull = x.TextFieldNull,
                BigDataFieldNull = x.BigDataFieldNull
            }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }
        #endregion



        [Fact]
        public void Query_PageSizeTest()
        {
            const int tol = 21;
            const int cnt = 8;
            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            List<int> listEx;
            List<int> listAc;

            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                listEx = list.Skip(cnt * i).Take(cnt).Select(x => x.Id).ToList();
                listAc = context.Query<TeBaseFieldSelectField>().PageSize(i + 1, cnt).Select(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);

                listEx = list.Skip(cnt * i).Take(cnt).Select(x => x.Id).ToList();
                listAc = context.Query<TeBaseFieldSelectField>().Range(i * cnt, (i + 1) * cnt).Select(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            listEx = list.Where(x => x.Id > cnt).Take(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().Where(x => x.Id > cnt).PageSize(1, cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Take(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().OrderByDescending(x => x.Id).PageSize(1, cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > cnt).Take(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().Where(x => x.Id > cnt).Range(0, cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Take(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().OrderByDescending(x => x.Id).Range(0, cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }

        [Fact]
        public void Query_PageTakeSkipTest()
        {
            const int tol = 21;
            const int cnt = 8;

            List<TeBaseFieldSelectField> list = CreateAndInsertBaseFieldTableList(45);
            List<int> listEx;
            List<int> listAc;


            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                listEx = list.Skip(cnt * i).Select(x => x.Id).ToList();
                listAc = context.Query<TeBaseFieldSelectField>().Skip(cnt * i).Select(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);

                listEx = list.Take(cnt).Select(x => x.Id).ToList();
                listAc = context.Query<TeBaseFieldSelectField>().Take(cnt).Select(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);

                listEx = list.Skip(cnt * i).Take(cnt).Select(x => x.Id).ToList();
                listAc = context.Query<TeBaseFieldSelectField>().Skip(cnt * i).Take(cnt).Select(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            listEx = list.Where(x => x.Id > cnt).Skip(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().Where(x => x.Id > cnt).Skip(cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Skip(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().OrderByDescending(x => x.Id).Skip(cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > cnt).Take(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().Where(x => x.Id > cnt).Take(cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Take(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().OrderByDescending(x => x.Id).Take(cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > cnt).Skip(cnt).Take(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().Where(x => x.Id > cnt).Skip(cnt).Take(cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Skip(cnt).Take(cnt).Select(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldSelectField>().OrderByDescending(x => x.Id).Skip(cnt).Take(cnt).Select(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }
        #endregion
    }
}
