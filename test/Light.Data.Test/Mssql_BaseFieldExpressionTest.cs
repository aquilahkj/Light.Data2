using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace Light.Data.Test
{
    public class Mssql_BaseFieldExpressionTest : BaseTest
    {
        public Mssql_BaseFieldExpressionTest(ITestOutputHelper output) : base(output)
        {
        }

        #region base test
        List<TeBaseFieldExpression> CreateBaseFieldTableList(int count)
        {
            List<TeBaseFieldExpression> list = new List<TeBaseFieldExpression>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeBaseFieldExpression item = new TeBaseFieldExpression();
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
                item.VarcharField = "tEettest" + item.Int32Field;
                item.VarcharFieldNull = i % 2 == 0 ? null : " " + item.VarcharField + " ";
                item.EnumInt32Field = (EnumInt32Type)(i % 5 - 1);
                item.EnumInt32FieldNull = i % 2 == 0 ? null : (EnumInt32Type?)(item.EnumInt32Field);
                item.EnumInt64Field = (EnumInt64Type)(i % 5 - 1);
                item.EnumInt64FieldNull = i % 2 == 0 ? null : (EnumInt64Type?)(item.EnumInt64Field);
                list.Add(item);
            }
            return list;
        }

        List<TeBaseFieldExpression> CreateAndInsertBaseFieldTableList(int count)
        {
            var list = CreateBaseFieldTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeBaseFieldExpression>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        List<TeBaseFieldExpressionExtend> CreateBaseFieldExtendTableList(int count)
        {
            List<TeBaseFieldExpressionExtend> list = new List<TeBaseFieldExpressionExtend>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeBaseFieldExpressionExtend item = new TeBaseFieldExpressionExtend();
                item.Int32Field = (int)((i % 5) * x);
                item.Int32FieldNull = i % 2 == 0 ? null : (int?)(item.Int32Field);
                item.DecimalField = (decimal)((i % 26) * 0.1 * x);
                item.DecimalFieldNull = i % 2 == 0 ? null : (decimal?)(item.DecimalField);
                item.DateTimeField = d.AddMinutes(i * 2);
                item.DateTimeFieldNull = i % 2 == 0 ? null : (DateTime?)(item.DateTimeField);
                item.VarcharField = "testtest" + item.Int32Field;
                item.VarcharFieldNull = i % 2 == 0 ? null : item.VarcharField;
                list.Add(item);
            }
            return list;
        }

        List<TeBaseFieldExpressionExtend> CreateAndInsertBaseFieldExtendTableList(int count)
        {
            var list = CreateBaseFieldExtendTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeBaseFieldExpressionExtend>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }


        #region where

        [Fact]
        public void TestCase_QueryWhere_AndOr()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Id >= 5 && x.Id <= 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id >= 5 && x.Id <= 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id >= 5).WhereWithAnd(x => x.Id <= 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id < 5 || x.Id > 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id < 5 || x.Id > 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id < 5).WhereWithOr(x => x.Id > 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id <= 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id >= 5).Where(x => x.Id <= 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list;
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id >= 5).WhereReset().ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryWhere_Multi()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<int> listx = new List<int>() { -3, -1, 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => x.VarcharField.EndsWith("1") && x.Int32FieldNull != null && listx.Contains(x.Int32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.EndsWith("1") && x.Int32FieldNull != null && listx.Contains(x.Int32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.EndsWith("1") && x.Int32FieldNull != null && listx.Contains(x.Int32Field) && (x.Id >= 18 || x.Id <= -1)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.StartsWith("1") && x.Int32FieldNull != null && listx.Contains(x.Int32Field) && (x.Id >= 18 || x.Id <= -1)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Enum()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.EnumInt32Field == EnumInt32Type.Zero).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt32Field == EnumInt32Type.Zero).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt32Field != EnumInt32Type.Zero).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt32Field != EnumInt32Type.Zero).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt64Field == EnumInt64Type.Zero).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt64Field == EnumInt64Type.Zero).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt64Field != EnumInt64Type.Zero).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt64Field != EnumInt64Type.Zero).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Enum_Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.EnumInt32FieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt32FieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt32FieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt32FieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt64FieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt64FieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt64FieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt64FieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt32FieldNull == EnumInt32Type.Zero).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt32FieldNull == EnumInt32Type.Zero).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt32FieldNull != EnumInt32Type.Zero).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt32FieldNull == null || x.EnumInt32FieldNull != EnumInt32Type.Zero).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt64FieldNull == EnumInt64Type.Zero).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt64FieldNull == EnumInt64Type.Zero).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.EnumInt64FieldNull != EnumInt64Type.Zero).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt64FieldNull == null || x.EnumInt64FieldNull != EnumInt64Type.Zero).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Id()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Id == 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id == 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id != 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id != 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id >= 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id >= 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id < 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id < 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id <= 10).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id <= 10).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > 5 && x.Id < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > 5 && x.Id < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id >= 5 && x.Id < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id >= 5 && x.Id < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > 5 && x.Id <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > 5 && x.Id <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id >= 5 && x.Id <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id >= 5 && x.Id <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_Bool()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.BoolField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !x.BoolField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !x.BoolField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.BoolField != false).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolField != false).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.BoolField == false).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolField == false).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.BoolField != true).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolField != true).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_BoolNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.BoolFieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.BoolFieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolFieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.BoolFieldNull != false).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolFieldNull == null || x.BoolFieldNull != false).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.BoolFieldNull == false).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolFieldNull == false).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.BoolFieldNull != true).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.BoolFieldNull == null || x.BoolFieldNull != true).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Sbyte()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.SbyteField == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField > -5 && x.SbyteField < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField > -5 && x.SbyteField < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField >= -5 && x.SbyteField < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField >= -5 && x.SbyteField < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField > -5 && x.SbyteField <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField > -5 && x.SbyteField <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteField >= -5 && x.SbyteField <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteField >= -5 && x.SbyteField <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_SbyteNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.SbyteFieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull == null || x.SbyteFieldNull != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull > -5 && x.SbyteFieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull > -5 && x.SbyteFieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull >= -5 && x.SbyteFieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull >= -5 && x.SbyteFieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull > -5 && x.SbyteFieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull > -5 && x.SbyteFieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.SbyteFieldNull >= -5 && x.SbyteFieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.SbyteFieldNull >= -5 && x.SbyteFieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Byte()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.ByteField == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField > 5 && x.ByteField < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField > 5 && x.ByteField < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField >= 5 && x.ByteField < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField >= 5 && x.ByteField < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField > 5 && x.ByteField <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField > 5 && x.ByteField <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteField >= 5 && x.ByteField <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteField >= 5 && x.ByteField <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_ByteNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.ByteFieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull == null || x.ByteFieldNull != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull > 5 && x.ByteFieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull > 5 && x.ByteFieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull >= 5 && x.ByteFieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull >= 5 && x.ByteFieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull > 5 && x.ByteFieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull > 5 && x.ByteFieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.ByteFieldNull >= 5 && x.ByteFieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.ByteFieldNull >= 5 && x.ByteFieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Int16()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Int16Field == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field > -5 && x.Int16Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field > -5 && x.Int16Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field >= -5 && x.Int16Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field >= -5 && x.Int16Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field > -5 && x.Int16Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field > -5 && x.Int16Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16Field >= -5 && x.Int16Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16Field >= -5 && x.Int16Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Int16Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Int16FieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull == null || x.Int16FieldNull != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull > -5 && x.Int16FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull > -5 && x.Int16FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull >= -5 && x.Int16FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull >= -5 && x.Int16FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull > -5 && x.Int16FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull > -5 && x.Int16FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int16FieldNull >= -5 && x.Int16FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull >= -5 && x.Int16FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Int32()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Int32Field == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field > -5 && x.Int32Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field > -5 && x.Int32Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field >= -5 && x.Int32Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field >= -5 && x.Int32Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field > -5 && x.Int32Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field > -5 && x.Int32Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field >= -5 && x.Int32Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field >= -5 && x.Int32Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Int32Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Int32FieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == null || x.Int32FieldNull != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull > -5 && x.Int32FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull > -5 && x.Int32FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull >= -5 && x.Int32FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull >= -5 && x.Int32FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull > -5 && x.Int32FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull > -5 && x.Int32FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull >= -5 && x.Int32FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull >= -5 && x.Int32FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Int64()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Int64Field == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field > -5 && x.Int64Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field > -5 && x.Int64Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field >= -5 && x.Int64Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field >= -5 && x.Int64Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field > -5 && x.Int64Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field > -5 && x.Int64Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64Field >= -5 && x.Int64Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64Field >= -5 && x.Int64Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Int64Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Int64FieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull == null || x.Int64FieldNull != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull > -5 && x.Int64FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull > -5 && x.Int64FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull >= -5 && x.Int64FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull >= -5 && x.Int64FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull > -5 && x.Int64FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull > -5 && x.Int64FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int64FieldNull >= -5 && x.Int64FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull >= -5 && x.Int64FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_UInt16()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.UInt16Field == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field > 5 && x.UInt16Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field > 5 && x.UInt16Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field >= 5 && x.UInt16Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field >= 5 && x.UInt16Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field > 5 && x.UInt16Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field > 5 && x.UInt16Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16Field >= 5 && x.UInt16Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16Field >= 5 && x.UInt16Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_UInt16Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.UInt16FieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull == null || x.UInt16FieldNull != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull > 5 && x.UInt16FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull > 5 && x.UInt16FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull >= 5 && x.UInt16FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull >= 5 && x.UInt16FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull > 5 && x.UInt16FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull > 5 && x.UInt16FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt16FieldNull >= 5 && x.UInt16FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull >= 5 && x.UInt16FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_UInt32()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.UInt32Field == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field > 5 && x.UInt32Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field > 5 && x.UInt32Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field >= 5 && x.UInt32Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field >= 5 && x.UInt32Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field > 5 && x.UInt32Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field > 5 && x.UInt32Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32Field >= 5 && x.UInt32Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32Field >= 5 && x.UInt32Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_UInt32Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.UInt32FieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull == null || x.UInt32FieldNull != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull > 5 && x.UInt32FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull > 5 && x.UInt32FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull >= 5 && x.UInt32FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull >= 5 && x.UInt32FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull > 5 && x.UInt32FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull > 5 && x.UInt32FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt32FieldNull >= 5 && x.UInt32FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull >= 5 && x.UInt32FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_UInt64()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.UInt64Field == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field > 5 && x.UInt64Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field > 5 && x.UInt64Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field >= 5 && x.UInt64Field < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field >= 5 && x.UInt64Field < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field > 5 && x.UInt64Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field > 5 && x.UInt64Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64Field >= 5 && x.UInt64Field <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64Field >= 5 && x.UInt64Field <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_UInt64Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.UInt64FieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull != 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull == null || x.UInt64FieldNull != 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull > 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull > 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull >= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull >= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull < 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull < 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull <= 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull <= 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull > 5 && x.UInt64FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull > 5 && x.UInt64FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull >= 5 && x.UInt64FieldNull < 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull >= 5 && x.UInt64FieldNull < 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull > 5 && x.UInt64FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull > 5 && x.UInt64FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.UInt64FieldNull >= 5 && x.UInt64FieldNull <= 15).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull >= 5 && x.UInt64FieldNull <= 15).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Float()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.FloatField > 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatField > 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatField >= 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatField >= 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatField < 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatField < 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatField <= 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatField <= 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatField > -1.05f && x.FloatField < 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatField > -1.05f && x.FloatField < 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatField >= -1.05f && x.FloatField < 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatField >= -1.05f && x.FloatField < 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatField > -1.05f && x.FloatField <= 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatField > -1.05f && x.FloatField <= 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatField >= -1.05f && x.FloatField <= 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatField >= -1.05f && x.FloatField <= 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_FloatNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.FloatFieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull > 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull > 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull >= 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull >= 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull < 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull < 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull <= 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull <= 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull > -1.05f && x.FloatFieldNull < 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull > -1.05f && x.FloatFieldNull < 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull >= -1.05f && x.FloatFieldNull < 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull >= -1.05f && x.FloatFieldNull < 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull > -1.05f && x.FloatFieldNull <= 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull > -1.05f && x.FloatFieldNull <= 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.FloatFieldNull >= -1.05f && x.FloatFieldNull <= 1.15f).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.FloatFieldNull >= -1.05f && x.FloatFieldNull <= 1.15f).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Double()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.DoubleField > 1.10d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField > 1.10d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField >= 1.10d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField >= 1.10d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField < 1.10d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField < 1.10d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField <= 1.10d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField <= 1.10d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField > -1.05d && x.DoubleField < 1.15d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField > -1.05d && x.DoubleField < 1.15d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField >= -1.05d && x.DoubleField < 1.15d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField >= -1.05d && x.DoubleField < 1.15d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField > -1.05d && x.DoubleField <= 1.15d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField > -1.05d && x.DoubleField <= 1.15d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField >= -1.05d && x.DoubleField <= 1.15d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField >= -1.05d && x.DoubleField <= 1.15d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_DoubleNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.DoubleFieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull > 1.10d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull > 1.10d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull >= 1.10d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull >= 1.10d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull < 1.10d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull < 1.10d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull <= 1.10d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull <= 1.10d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull > -1.05d && x.DoubleFieldNull < 1.15d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull > -1.05d && x.DoubleFieldNull < 1.15d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull >= -1.05d && x.DoubleFieldNull < 1.15d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull >= -1.05d && x.DoubleFieldNull < 1.15d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull > -1.05d && x.DoubleFieldNull <= 1.15d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull > -1.05d && x.DoubleFieldNull <= 1.15d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull >= -1.05d && x.DoubleFieldNull <= 1.15d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull >= -1.05d && x.DoubleFieldNull <= 1.15d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_Decimal()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.DecimalField == 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField == 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField != 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField != 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField > 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField > 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField >= 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField >= 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField < 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField < 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField <= 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField <= 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField > -1.05m && x.DecimalField < 1.15m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField > -1.05m && x.DecimalField < 1.15m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField >= -1.05m && x.DecimalField < 1.15m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField >= -1.05m && x.DecimalField < 1.15m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField > -1.05m && x.DecimalField <= 1.15m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField > -1.05m && x.DecimalField <= 1.15m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalField >= -1.05m && x.DecimalField <= 1.15m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalField >= -1.05m && x.DecimalField <= 1.15m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_DecimalNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.DecimalFieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull == 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull == 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull != 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull == null || x.DecimalFieldNull != 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull > 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull > 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull >= 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull >= 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull < 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull < 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull <= 1.10m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull <= 1.10m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull > -1.05m && x.DecimalFieldNull < 1.15m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull > -1.05m && x.DecimalFieldNull < 1.15m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull >= -1.05m && x.DecimalFieldNull < 1.15m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull >= -1.05m && x.DecimalFieldNull < 1.15m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull > -1.05m && x.DecimalFieldNull <= 1.15m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull > -1.05m && x.DecimalFieldNull <= 1.15m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DecimalFieldNull >= -1.05m && x.DecimalFieldNull <= 1.15m).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull >= -1.05m && x.DecimalFieldNull <= 1.15m).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_DateTime()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            DateTime dt = DateTime.Now;

            listEx = list.Where(x => x.DateTimeField == dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField == dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField != dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField != dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField > dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField > dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField >= dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField >= dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField < dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField < dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField <= dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField <= dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField > dt.AddMinutes(5) && x.DateTimeField < dt.AddMinutes(15)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField > dt.AddMinutes(5) && x.DateTimeField < dt.AddMinutes(15)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField >= dt.AddMinutes(5) && x.DateTimeField < dt.AddMinutes(15)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField >= dt.AddMinutes(5) && x.DateTimeField < dt.AddMinutes(15)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField > dt.AddMinutes(5) && x.DateTimeField <= dt.AddMinutes(15)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField > dt.AddMinutes(5) && x.DateTimeField <= dt.AddMinutes(15)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField >= dt.AddMinutes(5) && x.DateTimeField <= dt.AddMinutes(15)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField >= dt.AddMinutes(5) && x.DateTimeField <= dt.AddMinutes(15)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_DateTimeNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            DateTime dt = DateTime.Now;

            listEx = list.Where(x => x.DateTimeFieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull == dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull == dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull != dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull == null || x.DateTimeFieldNull != dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull > dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull > dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull >= dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull >= dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull < dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull < dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull <= dt.AddMinutes(10)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull <= dt.AddMinutes(10)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull > dt.AddMinutes(5) && x.DateTimeFieldNull < dt.AddMinutes(15)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull > dt.AddMinutes(5) && x.DateTimeFieldNull < dt.AddMinutes(15)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull >= dt.AddMinutes(5) && x.DateTimeFieldNull < dt.AddMinutes(15)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull >= dt.AddMinutes(5) && x.DateTimeFieldNull < dt.AddMinutes(15)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull > dt.AddMinutes(5) && x.DateTimeFieldNull <= dt.AddMinutes(15)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull > dt.AddMinutes(5) && x.DateTimeFieldNull <= dt.AddMinutes(15)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeFieldNull >= dt.AddMinutes(5) && x.DateTimeFieldNull <= dt.AddMinutes(15)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull >= dt.AddMinutes(5) && x.DateTimeFieldNull <= dt.AddMinutes(15)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_String()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            TeBaseFieldExpression item = list[11];

            listEx = list.Where(x => x.VarcharField == item.VarcharField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField == item.VarcharField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField != item.VarcharField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField != item.VarcharField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QuerySingleParam_StringNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            TeBaseFieldExpression item = list[10];

            listEx = list.Where(x => x.VarcharFieldNull == item.VarcharFieldNull).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharFieldNull == item.VarcharFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharFieldNull != item.VarcharFieldNull).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharFieldNull == null || x.VarcharFieldNull != item.VarcharFieldNull).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharFieldNull == null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharFieldNull != null).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharFieldNull != null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_Enum()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<EnumInt32Type> listx = new List<EnumInt32Type>() { EnumInt32Type.Positive1, EnumInt32Type.Positive2 };
            List<EnumInt64Type> listy = new List<EnumInt64Type>() { EnumInt64Type.Positive1, EnumInt64Type.Positive2 };

            listEx = list.Where(x => listx.Contains(x.EnumInt32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.EnumInt32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.EnumInt32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.EnumInt32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => listy.Contains(x.EnumInt64Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listy.Contains(x.EnumInt64Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listy.Contains(x.EnumInt64Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listy.Contains(x.EnumInt64Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_EnumNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<EnumInt32Type?> listx = new List<EnumInt32Type?>() { EnumInt32Type.Positive1, EnumInt32Type.Positive2 };
            List<EnumInt64Type?> listy = new List<EnumInt64Type?>() { EnumInt64Type.Positive1, EnumInt64Type.Positive2 };

            listEx = list.Where(x => listx.Contains(x.EnumInt32FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.EnumInt32FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.EnumInt32FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt32FieldNull == null || !listx.Contains(x.EnumInt32FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => listy.Contains(x.EnumInt64FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listy.Contains(x.EnumInt64FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listy.Contains(x.EnumInt64FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.EnumInt64FieldNull == null || !listy.Contains(x.EnumInt64FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_Int16()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<short> listx = new List<short>() { -3, -1, 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.Int16Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.Int16Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.Int16Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.Int16Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_Int16Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<short?> listx = new List<short?>() { -3, -1, 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.Int16FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.Int16FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.Int16FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int16FieldNull == null || !listx.Contains(x.Int16FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_Int32()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<int> listx = new List<int>() { -3, -1, 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.Int32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.Int32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.Int32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.Int32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_Int32Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<int?> listx = new List<int?>() { -3, -1, 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.Int32FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.Int32FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.Int32FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == null || !listx.Contains(x.Int32FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_Int64()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<long> listx = new List<long>() { -3, -1, 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.Int64Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.Int64Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.Int64Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.Int64Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_Int64Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<long?> listx = new List<long?>() { -3, -1, 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.Int64FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.Int64FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.Int64FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int64FieldNull == null || !listx.Contains(x.Int64FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_UInt16()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<ushort> listx = new List<ushort>() { 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.UInt16Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.UInt16Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.UInt16Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.UInt16Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_UInt16Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<ushort?> listx = new List<ushort?>() { 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.UInt16FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.UInt16FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.UInt16FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt16FieldNull == null || !listx.Contains(x.UInt16FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_UInt32()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<uint> listx = new List<uint>() { 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.UInt32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.UInt32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.UInt32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.UInt32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_UInt32Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<uint?> listx = new List<uint?>() { 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.UInt32FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.UInt32FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.UInt32FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt32FieldNull == null || !listx.Contains(x.UInt32FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_UInt64()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<ulong> listx = new List<ulong>() { 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.UInt64Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.UInt64Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.UInt64Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.UInt64Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_UInt64Null()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<ulong?> listx = new List<ulong?>() { 3, 5, 7, 10, 30, 35 };

            listEx = list.Where(x => listx.Contains(x.UInt64FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.UInt64FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.UInt64FieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.UInt64FieldNull == null || !listx.Contains(x.UInt64FieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_Decimal()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<decimal> listx = new List<decimal>() { list[0].DecimalField, list[1].DecimalField, list[4].DecimalField, list[9].DecimalField };

            listEx = list.Where(x => listx.Contains(x.DecimalField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.DecimalField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.DecimalField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.DecimalField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_DecimalNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            List<decimal?> listx = new List<decimal?>() { list[0].DecimalField, list[1].DecimalField, list[4].DecimalField, list[9].DecimalField };

            listEx = list.Where(x => listx.Contains(x.DecimalFieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.DecimalFieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.DecimalFieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DecimalFieldNull == null || !listx.Contains(x.DecimalFieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_DateTime()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            List<DateTime> listx = new List<DateTime>() { list[0].DateTimeField, list[1].DateTimeField, list[5].DateTimeField, list[7].DateTimeField };

            listEx = list.Where(x => listx.Contains(x.DateTimeField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.DateTimeField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.DateTimeField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.DateTimeField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_DateTimeNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            List<DateTime?> listx = new List<DateTime?>() { list[0].DateTimeField, list[1].DateTimeField, list[5].DateTimeField, list[7].DateTimeField };

            listEx = list.Where(x => listx.Contains(x.DateTimeFieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.DateTimeFieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.DateTimeFieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeFieldNull == null || !listx.Contains(x.DateTimeFieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_String()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            List<string> listx = new List<string>() { list[0].VarcharField, list[1].VarcharField, list[5].VarcharField, list[8].VarcharField };

            listEx = list.Where(x => listx.Contains(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !listx.Contains(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryIn_StringNull()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            List<string> listx = new List<string>() { list[0].VarcharField, list[1].VarcharField, list[5].VarcharField, list[8].VarcharField };

            listEx = list.Where(x => listx.Contains(x.VarcharFieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => listx.Contains(x.VarcharFieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !listx.Contains(x.VarcharFieldNull)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharFieldNull == null || !listx.Contains(x.VarcharFieldNull)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_QueryLikeMatch_Single_String()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            TeBaseFieldExpression item = list[0];
            string start = item.VarcharField.Substring(0, 4);
            string end = item.VarcharField.Substring(item.VarcharField.Length - 2);
            string mid = item.VarcharField.Substring(item.VarcharField.Length - 4, 2);

            listEx = list.Where(x => x.VarcharField.StartsWith(start)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.StartsWith(start)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !x.VarcharField.StartsWith(start)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !x.VarcharField.StartsWith(start)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.EndsWith(end)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.EndsWith(end)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !x.VarcharField.EndsWith(end)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !x.VarcharField.EndsWith(end)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.Contains("est2")).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.Contains("est2")).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !x.VarcharField.Contains("est2")).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !x.VarcharField.Contains("est2")).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.StartsWith(start)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.StartsWith(start)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !x.VarcharField.StartsWith(start)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !x.VarcharField.StartsWith(start)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.EndsWith(end)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.EndsWith(end)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !x.VarcharField.EndsWith(end)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !x.VarcharField.EndsWith(end)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.Contains(mid)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.Contains(mid)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !x.VarcharField.Contains(mid)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !x.VarcharField.Contains(mid)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
            AssertExtend.StrictEqual(listEx, listAc);

        }

        [Fact]
        public void TestCAse_QueryLikeMatch_Single_String_Reverse()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            TeBaseFieldExpression item = list[0];
            string start = item.VarcharField + "===";
            string end = "===" + item.VarcharField;
            string mid = "===" + item.VarcharField + "===";

            listEx = list.Where(x => start.StartsWith(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => start.StartsWith(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !start.StartsWith(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !start.StartsWith(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => mid.StartsWith(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => mid.StartsWith(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !mid.StartsWith(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !mid.StartsWith(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => end.EndsWith(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => end.EndsWith(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);


            listEx = list.Where(x => !end.EndsWith(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !end.EndsWith(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => mid.EndsWith(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => mid.EndsWith(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !mid.EndsWith(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !mid.EndsWith(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => mid.Contains(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => mid.Contains(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !mid.Contains(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !mid.Contains(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => "test".Contains(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => "test".Contains(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !"test".Contains(x.VarcharField)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !"test".Contains(x.VarcharField)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        #endregion

        #region order 

        [Fact]
        public void TestCase_OrderBy_Id()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_OrderBy_Int32()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(20);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.OrderBy(x => x.Int32Field).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderBy(x => x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Int32Field).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_OrderBy_Double()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(20);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.OrderBy(x => x.DoubleField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderBy(x => x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.DoubleField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_OrderBy_DateTime()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.OrderBy(x => x.DateTimeField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderBy(x => x.DateTimeField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.DateTimeField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.DateTimeField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_OrderBy_Catch()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.OrderBy(x => x.Int32Field).ThenBy(x => x.DoubleField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderBy(x => x.Int32Field).OrderByCatch(x => x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderBy(x => x.Int32Field).ThenByDescending(x => x.DoubleField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderBy(x => x.Int32Field).OrderByDescendingCatch(x => x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Int32Field).ThenBy(x => x.DoubleField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Int32Field).OrderByCatch(x => x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Int32Field).ThenByDescending(x => x.DoubleField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Int32Field).OrderByDescendingCatch(x => x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_OrderBy_Random()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> list1 = context.Query<TeBaseFieldExpression>().OrderByRandom().ToList();
            Assert.Equal(45, list1.Count);
            List<TeBaseFieldExpression> list2 = context.Query<TeBaseFieldExpression>().OrderByRandom().ToList();
            Assert.Equal(45, list1.Count);
            int[] array1 = new int[list1.Count];
            for (int i = 0; i < list1.Count; i++) {
                array1[i] = list1[i].Id;
            }
            int[] array2 = new int[list2.Count];
            for (int i = 0; i < list2.Count; i++) {
                array2[i] = list2[i].Id;
            }

            string s1 = string.Join("-", array1);
            string s2 = string.Join("-", array2);
            Assert.NotEqual(s1, s2);
        }

        [Fact]
        public void TestCase_OrderBy_Random_Replace()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(20);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;
            listEx = list.OrderByDescending(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByRandom().OrderByDescending(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByRandom().OrderByDescending(x => x.Int32Field).OrderByReset().ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderBy(x => x.Int32Field).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByRandom().OrderByDescending(x => x.Id).OrderBy(x => x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            var list1 = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).OrderByRandom().ToList();
            Assert.Equal(list.Count, list1.Count);
            var list2 = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).OrderByRandom().ToList();
            Assert.Equal(list.Count, list1.Count);
            int[] array1 = new int[list1.Count];
            for (int i = 0; i < list1.Count; i++) {
                array1[i] = list1[i].Id;
            }
            int[] array2 = new int[list2.Count];
            for (int i = 0; i < list2.Count; i++) {
                array2[i] = list2[i].Id;
            }
            string s1 = string.Join("-", array1);
            string s2 = string.Join("-", array2);
            Assert.NotEqual(s1, s2);
        }
        #endregion

        #region pagesize

        [Fact]
        public void Query_PageSize_Multi()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Skip(10).Take(20).ToList();
            listAc = context.Query<TeBaseFieldExpression>().PageSize(1, 15).Skip(10).Take(20).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list;
            listAc = context.Query<TeBaseFieldExpression>().PageSize(1, 15).RangeReset().ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void Query_PageSizeTest()
        {
            const int tol = 21;
            const int cnt = 8;
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                listEx = list.Skip(cnt * i).Take(cnt).ToList();
                listAc = context.Query<TeBaseFieldExpression>().PageSize(i + 1, cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);

                listEx = list.Skip(cnt * i).Take(cnt).ToList();
                listAc = context.Query<TeBaseFieldExpression>().Range(i * cnt, (i + 1) * cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            listEx = list.Where(x => x.Id > cnt).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > cnt).PageSize(1, cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).PageSize(1, cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > cnt).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > cnt).Range(0, cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).Range(0, cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }

        [Fact]
        public void Query_PageTakeSkipTest()
        {
            const int tol = 21;
            const int cnt = 8;

            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;


            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                listEx = list.Skip(cnt * i).ToList();
                listAc = context.Query<TeBaseFieldExpression>().Skip(cnt * i).ToList();
                AssertExtend.StrictEqual(listEx, listAc);

                listEx = list.Take(cnt * i).ToList();
                listAc = context.Query<TeBaseFieldExpression>().Take(cnt * i).ToList();
                AssertExtend.StrictEqual(listEx, listAc);

                listEx = list.Skip(cnt * i).Take(cnt).ToList();
                listAc = context.Query<TeBaseFieldExpression>().Skip(cnt * i).Take(cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            listEx = list.Where(x => x.Id > cnt).Skip(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > cnt).Skip(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Skip(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).Skip(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > cnt).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > cnt).Take(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).Take(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > cnt).Skip(cnt).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > cnt).Skip(cnt).Take(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Skip(cnt).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).Skip(cnt).Take(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        #endregion

        #region extend
        [Fact]
        public void TestCase_Query_SubQuery_Exists()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpressionExtend> liste = CreateAndInsertBaseFieldExtendTableList(10);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => liste.Exists(y => y.ExtendId == x.Id)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.Exists<TeBaseFieldExpressionExtend>(y => y.ExtendId == x.Id)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !liste.Exists(y => y.ExtendId == x.Id)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !ExtendQuery.Exists<TeBaseFieldExpressionExtend>(y => y.ExtendId == x.Id)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_SubQuery_In()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpressionExtend> liste = CreateAndInsertBaseFieldExtendTableList(10);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => liste.Exists(y => y.ExtendId == x.Id)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.In<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => !liste.Exists(y => y.ExtendId == x.Id)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => !ExtendQuery.In<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.Exists(y => y.ExtendId == x.Id && x.Int32Field != y.Int32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.In<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId, y => x.Int32Field != y.Int32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.TrueForAll(y => x.Id > y.ExtendId)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.GtAll<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.TrueForAll(y => x.Id >= y.ExtendId)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.GtEqAll<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.Exists(y => x.Id > y.ExtendId)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.GtAny<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.Exists(y => x.Id >= y.ExtendId)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.GtEqAny<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.TrueForAll(y => x.Id < y.ExtendId)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.LtAll<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.TrueForAll(y => x.Id <= y.ExtendId)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.LtEqAll<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.Exists(y => x.Id < y.ExtendId)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.LtAny<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => liste.Exists(y => x.Id <= y.ExtendId)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => ExtendQuery.LtEqAny<TeBaseFieldExpressionExtend, int>(x.Id, y => y.ExtendId)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        #endregion

        #region Field
        [Fact]
        public void TestCase_Query_FieldMatch()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(51);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.Int32FieldNull == x.Int32Field).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == x.Int32Field + 2).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == x.Int32Field + 2).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == 2 + x.Int32Field).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == 2 + x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == x.Int32Field - 2).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == x.Int32Field - 2).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == 2 - x.Int32Field).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == 2 - x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == x.Int32Field * 2).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == x.Int32Field * 2).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == 2 * x.Int32Field).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == 2 * x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull >= x.DoubleField / 2).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull >= x.DoubleField / 2).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField > 0 && x.DoubleFieldNull >= 2 / x.DoubleField).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField > 0 && x.DoubleFieldNull >= 2 / x.DoubleField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == x.Int32Field % 2).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == x.Int32Field % 2).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field > 0 && x.Int32FieldNull == 2 % x.Int32Field).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field > 0 && x.Int32FieldNull == 2 % x.Int32Field).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull >= Math.Pow(x.Int32Field, 2)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull >= Math.Pow(x.Int32Field, 2)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleFieldNull >= Math.Pow(2, x.Int32Field)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleFieldNull >= Math.Pow(2, x.Int32Field)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == 2 * (x.Int32Field + 2)).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == 2 * (x.Int32Field + 2)).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32FieldNull == 2 + x.Int32Field * 2).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32FieldNull == 2 + x.Int32Field * 2).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }


        [Fact]
        public void TestCase_Query_MathMatch()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(21);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => Math.Abs(x.Int32Field) > 5).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Abs(x.Int32Field) > 5).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field > 0 && Math.Log(x.Int32Field) > 1.2d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field > 0 && Math.Log(x.Int32Field) > 1.2d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field > 0 && Math.Log(x.Int32Field, 10) > 1.2d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field > 0 && Math.Log(x.Int32Field, 10) > 1.2d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field > 0 && Math.Log10(x.Int32Field) > 1.2d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field > 0 && Math.Log10(x.Int32Field) > 1.2d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Exp(x.Int32Field) > 5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Exp(x.Int32Field) > 5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Pow(x.Int32Field, 2) > 20).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Pow(x.Int32Field, 2) > 20).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Sin(x.Int32Field) > 0.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Sin(x.Int32Field) > 0.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Cos(x.Int32Field) > 0.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Cos(x.Int32Field) > 0.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Tan(x.Int32Field) > 0.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Tan(x.Int32Field) > 0.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Atan(x.Int32Field) > 0.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Atan(x.Int32Field) > 0.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Sign(x.Int32Field) > 0.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Sign(x.Int32Field) > 0.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField >= -1 && x.DoubleField <= 1 && Math.Asin(x.DoubleField) > 0.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField >= -1 && x.DoubleField <= 1 && Math.Asin(x.DoubleField) > 0.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DoubleField >= -1 && x.DoubleField <= 1 && Math.Acos(x.DoubleField) > 0.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DoubleField >= -1 && x.DoubleField <= 1 && Math.Acos(x.DoubleField) > 0.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Int32Field > 0 && Math.Atan2(x.Int32Field, 2) > 0.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Int32Field > 0 && Math.Atan2(x.Int32Field, 2) > 0.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Floor(x.DoubleField) > 1.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Floor(x.DoubleField) > 1.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Ceiling(x.DoubleField) < 1.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Ceiling(x.DoubleField) < 1.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Round(x.DoubleField) < 1.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Round(x.DoubleField) < 1.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Round(x.DoubleField, 2) < 1.5d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Round(x.DoubleField, 2) < 1.5d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Truncate(x.DoubleField) > 1d).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Truncate(x.DoubleField) > 1d).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Max(x.Id, x.Int32Field) > 5).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Max(x.Id, x.Int32Field) > 5).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => Math.Min(x.Id, x.Int32Field) < 5).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => Math.Min(x.Id, x.Int32Field) < 5).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_StringMatch()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(51);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.VarcharField.Length == 9).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.Length == 9).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.Substring(4) == "test1").ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.Substring(4) == "test1").ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.Substring(4, 5) == "test1").ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.Substring(4, 5) == "test1").ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.IndexOf("test1") == 4).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.IndexOf("test1") == 4).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.IndexOf("t", 4) == 4).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.IndexOf("t", 4) == 4).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.IndexOf("1") == -1).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.IndexOf("1") == -1).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.IndexOf("1", 9) == -1).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.IndexOf("1", 9) == -1).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.Replace("test", "tttt") == "tEsttttt1").ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.Replace("test", "tttt") == "tEsttttt1").ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.ToLower() == "testtest1").ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.ToLower() == "testtest1").ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharField.ToUpper() == "TESTTEST1").ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharField.ToUpper() == "TESTTEST1").ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.VarcharFieldNull != null && x.VarcharFieldNull.Trim() == "tEsttest1").ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.VarcharFieldNull != null && x.VarcharFieldNull.Trim() == "tEsttest1").ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Query_DateTimeMatch()
        {
            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(21);
            DateTime dt = new DateTime(2011, 1, 1, 1, 1, 1);
            list[10].DateTimeField = dt;
            list[20].DateTimeField = dt;
            context.Update(list[10]);
            context.Update(list[20]);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;

            listEx = list.Where(x => x.DateTimeField.Year == dt.Year).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.Year == dt.Year).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField.Month == dt.Month).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.Month == dt.Month).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField.Day == dt.Day).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.Day == dt.Day).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField.Hour == dt.Hour).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.Hour == dt.Hour).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField.Minute == dt.Minute).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.Minute == dt.Minute).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField.Second == dt.Second).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.Second == dt.Second).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField.DayOfYear == dt.DayOfYear).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.DayOfYear == dt.DayOfYear).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField.DayOfWeek == dt.DayOfWeek).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.DayOfWeek == dt.DayOfWeek).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.DateTimeField.Date == dt.Date).OrderBy(x => x.Id).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.Date == dt.Date).ToList().OrderBy(x => x.Id).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            List<string> formats = new List<string>();
            formats.Add("yyyyMMdd");
            formats.Add("yyyyMM");
            formats.Add("yyyy-MM-dd");
            formats.Add("yyyy-MM");
            formats.Add("dd-MM-yyyy");
            formats.Add("MM-dd-yyyy");
            formats.Add("yyyy/MM/dd");
            formats.Add("yyyy/MM");
            formats.Add("dd/MM/yyyy");
            formats.Add("MM/dd/yyyy");
            foreach (string format in formats) {
                listEx = list.Where(x => x.DateTimeField.ToString(format) == dt.ToString(format)).OrderBy(x => x.Id).ToList();
                listAc = context.Query<TeBaseFieldExpression>().Where(x => x.DateTimeField.ToString(format) == dt.ToString(format)).ToList().OrderBy(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

        }
        #endregion
        #endregion

        #region version
        [Fact]
        public void Query_PageTakeSkipTest_Ver2012()
        {
            DataContext context = CreateContext("mssql_2012");
            const int tol = 21;
            const int cnt = 8;

            List<TeBaseFieldExpression> list = CreateAndInsertBaseFieldTableList(45);
            List<TeBaseFieldExpression> listEx;
            List<TeBaseFieldExpression> listAc;


            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                listEx = list.Skip(cnt * i).ToList();
                listAc = context.Query<TeBaseFieldExpression>().Skip(cnt * i).ToList();
                AssertExtend.StrictEqual(listEx, listAc);

                listEx = list.Take(cnt * i).ToList();
                listAc = context.Query<TeBaseFieldExpression>().Take(cnt * i).ToList();
                AssertExtend.StrictEqual(listEx, listAc);

                listEx = list.Skip(cnt * i).Take(cnt).ToList();
                listAc = context.Query<TeBaseFieldExpression>().Skip(cnt * i).Take(cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            listEx = list.Where(x => x.Id > cnt).Skip(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > cnt).Skip(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Skip(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).Skip(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > cnt).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > cnt).Take(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).Take(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.Where(x => x.Id > cnt).Skip(cnt).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().Where(x => x.Id > cnt).Skip(cnt).Take(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = list.OrderByDescending(x => x.Id).Skip(cnt).Take(cnt).ToList();
            listAc = context.Query<TeBaseFieldExpression>().OrderByDescending(x => x.Id).Skip(cnt).Take(cnt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }
        #endregion
    }
}
