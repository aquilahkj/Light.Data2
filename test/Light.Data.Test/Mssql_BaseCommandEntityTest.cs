using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Light.Data.Test
{
    public class Mssql_BaseCommandEntityTest : BaseTest
    {
        public Mssql_BaseCommandEntityTest(ITestOutputHelper output) : base(output)
        {
        }

        #region base test

        List<TeBaseFieldEntity> CreateBaseFieldEntityTableList(int count)
        {
            List<TeBaseFieldEntity> list = new List<TeBaseFieldEntity>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeBaseFieldEntity item = context.CreateNew<TeBaseFieldEntity>();
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

        List<TeBaseFieldEntity> CreateAndInsertBaseFieldEntityTableList(int count)
        {
            var list = CreateBaseFieldEntityTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeBaseFieldEntity>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        [Fact]
        public void TestCase_Query()
        {
            List<TeBaseFieldEntity> list = CreateAndInsertBaseFieldEntityTableList(45);
            List<TeBaseFieldEntity> listEx;
            List<TeBaseFieldEntity> listAc;

            listEx = list;
            listAc = context.Query<TeBaseFieldEntity>().ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public async Task TestCase_Query_Async()
        {
            List<TeBaseFieldEntity> list = CreateAndInsertBaseFieldEntityTableList(45);
            List<TeBaseFieldEntity> listEx;
            List<TeBaseFieldEntity> listAc;

            listEx = list;
            listAc = await context.Query<TeBaseFieldEntity>().ToListAsync();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Single()
        {
            List<TeBaseFieldEntity> list = CreateAndInsertBaseFieldEntityTableList(45);
            TeBaseFieldEntity ex;
            TeBaseFieldEntity ac;

            ex = list[0];
            ac = context.Query<TeBaseFieldEntity>().First();
            AssertExtend.StrictEqual(ex, ac);

            ex = list[10];
            ac = context.Query<TeBaseFieldEntity>().ElementAt(10);
            AssertExtend.StrictEqual(ex, ac);
        }

        [Fact]
        public async Task TestCase_Single_Async()
        {
            List<TeBaseFieldEntity> list = CreateAndInsertBaseFieldEntityTableList(45);
            TeBaseFieldEntity ex;
            TeBaseFieldEntity ac;

            ex = list[0];
            ac = await context.Query<TeBaseFieldEntity>().FirstAsync();
            AssertExtend.StrictEqual(ex, ac);

            ex = list[10];
            ac = await context.Query<TeBaseFieldEntity>().ElementAtAsync(10);
            AssertExtend.StrictEqual(ex, ac);
        }

        [Fact]
        public void TestCase_CUD_Single()
        {
            context.TruncateTable<TeBaseFieldEntity>();
            var item1 = CreateBaseFieldEntityTableList(1)[0];
            var retInsert = item1.Save();
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retInsert);
            var item2 = context.SelectById<TeBaseFieldEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item2);
            item1.DateTimeField = GetNow();
            item1.DateTimeFieldNull = null;
            item1.Int32Field = 2;
            item1.Int32FieldNull = null;
            item1.DoubleField = 2.0d;
            item1.DoubleFieldNull = null;
            item1.VarcharField = "abc";
            item1.VarcharFieldNull = null;
            item1.EnumInt32Field = EnumInt32Type.Zero;
            item1.EnumInt32FieldNull = null;
            item1.EnumInt64Field = EnumInt64Type.Zero;
            item1.EnumInt64FieldNull = null;
            var retUpdate = item1.Save();
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retUpdate);
            var item3 = context.SelectById<TeBaseFieldEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item3);
            var retDelete = item1.Erase();
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retDelete);
            var item4 = context.SelectById<TeBaseFieldEntity>(item1.Id);
            Assert.Null(item4);
        }

        [Fact]
        public async Task TestCase_CUD_Single_Async()
        {
            await context.TruncateTableAsync<TeBaseFieldEntity>();
            var item1 = CreateBaseFieldEntityTableList(1)[0];
            var retInsert = await item1.SaveAsync();
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retInsert);
            var item2 = await context.SelectByIdAsync<TeBaseFieldEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item2);
            item1.DateTimeField = GetNow();
            item1.DateTimeFieldNull = null;
            item1.Int32Field = 2;
            item1.Int32FieldNull = null;
            item1.DoubleField = 2.0d;
            item1.DoubleFieldNull = null;
            item1.VarcharField = "abc";
            item1.VarcharFieldNull = null;
            item1.EnumInt32Field = EnumInt32Type.Zero;
            item1.EnumInt32FieldNull = null;
            item1.EnumInt64Field = EnumInt64Type.Zero;
            item1.EnumInt64FieldNull = null;
            var retUpdate = await item1.SaveAsync();
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retUpdate);
            var item3 = await context.SelectByIdAsync<TeBaseFieldEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item3);
            var retDelete = await item1.EraseAsync();
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retDelete);
            var item4 = await context.SelectByIdAsync<TeBaseFieldEntity>(item1.Id);
            Assert.Null(item4);
        }

        [Fact]
        public void TestCase_CUD_Single_NoIdentity_Key()
        {
            context.TruncateTable<TeBaseFieldNoIdentityEntity>();
            var item1 = context.CreateNew<TeBaseFieldNoIdentityEntity>();
            item1.Id = 0;
            item1.Int32Field = 1;
            item1.DoubleField = 0.1;
            item1.VarcharField = "level1";
            item1.DateTimeField = GetNow();
            item1.EnumInt32Field = EnumInt32Type.Positive1;
            var retInsert = item1.Save();
            Assert.Equal(0, item1.Id);
            Assert.Equal(1, retInsert);
            var item2 = context.SelectByKey<TeBaseFieldNoIdentityEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item2);
            item1.Id = 1;
            item1.DateTimeField = GetNow();
            item1.Int32Field = 2;
            item1.VarcharField = "level2";
            item1.DoubleField = 0.2;
            item1.EnumInt32Field = EnumInt32Type.Negative1;
            var retUpdate = item1.Save();
            Assert.Equal(0, item2.Id);
            Assert.Equal(1, retUpdate);
            var item3 = context.SelectByKey<TeBaseFieldNoIdentityEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item3);
            var itemn = context.SelectByKey<TeBaseFieldNoIdentityEntity>(0);
            Assert.Null(itemn);
            var retDelete = item1.Erase();
            Assert.Equal(1, item3.Id);
            Assert.Equal(1, retDelete);
            var item4 = context.SelectByKey<TeBaseFieldNoIdentityEntity>(item1.Id);
            Assert.Null(item4);
        }

        [Fact]
        public void TestCase_CUD_Single_NoIdentity()
        {
            context.TruncateTable<TeBaseFieldNoIdentityEntity>();
            var item1 = context.CreateNew<TeBaseFieldNoIdentityEntity>();
            item1.Id = 0;
            item1.Int32Field = 1;
            item1.DoubleField = 0.1;
            item1.VarcharField = "level1";
            item1.DateTimeField = GetNow();
            item1.EnumInt32Field = EnumInt32Type.Positive1;
            var retInsert = item1.Save();
            Assert.Equal(0, item1.Id);
            Assert.Equal(1, retInsert);
            var item2 = context.SelectByKey<TeBaseFieldNoIdentityEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item2);
            item1.DateTimeField = GetNow();
            item1.Int32Field = 2;
            item1.VarcharField = "level2";
            item1.DoubleField = 0.2;
            item1.EnumInt32Field = EnumInt32Type.Negative1;
            var retUpdate = item1.Save();
            Assert.Equal(0, item2.Id);
            Assert.Equal(1, retUpdate);
            var item3 = context.SelectByKey<TeBaseFieldNoIdentityEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item3);
            var retDelete = item1.Erase();
            Assert.Equal(0, item3.Id);
            Assert.Equal(1, retDelete);
            var item4 = context.SelectByKey<TeBaseFieldNoIdentityEntity>(item1.Id);
            Assert.Null(item4);
        }

        [Fact]
        public async Task TestCase_CUD_Single_NoIdentity_Async()
        {
            await context.TruncateTableAsync<TeBaseFieldNoIdentityEntity>();
            var item1 = context.CreateNew<TeBaseFieldNoIdentityEntity>();
            item1.Id = 0;
            item1.Int32Field = 1;
            item1.DoubleField = 0.1;
            item1.VarcharField = "level1";
            item1.DateTimeField = GetNow();
            item1.EnumInt32Field = EnumInt32Type.Positive1;
            var retInsert = await item1.SaveAsync();
            Assert.Equal(0, item1.Id);
            Assert.Equal(1, retInsert);
            var item2 = await context.SelectByKeyAsync<TeBaseFieldNoIdentityEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item2);
            item1.DateTimeField = GetNow();
            item1.Int32Field = 2;
            item1.VarcharField = "level2";
            item1.DoubleField = 0.2;
            item1.EnumInt32Field = EnumInt32Type.Negative1;
            var retUpdate = await item1.SaveAsync();
            Assert.Equal(0, item2.Id);
            Assert.Equal(1, retUpdate);
            var item3 = await context.SelectByKeyAsync<TeBaseFieldNoIdentityEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item3);
            var retDelete = await item1.EraseAsync();
            Assert.Equal(0, item3.Id);
            Assert.Equal(1, retDelete);
            var item4 = await context.SelectByKeyAsync<TeBaseFieldNoIdentityEntity>(item1.Id);
            Assert.Null(item4);
        }

        [Fact]
        public void TestCase_InsertOrUpdate_Single()
        {
            context.TruncateTable<TeBaseFieldEntity>();
            var item1 = CreateBaseFieldEntityTableList(1)[0];
            var retInsert = context.InsertOrUpdate(item1);
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retInsert);
            var item2 = context.SelectById<TeBaseFieldEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item2);
            item1.DateTimeField = GetNow();
            item1.DateTimeFieldNull = null;
            item1.Int32Field = 2;
            item1.Int32FieldNull = null;
            item1.DoubleField = 2.0d;
            item1.DoubleFieldNull = null;
            item1.VarcharField = "abc";
            item1.VarcharFieldNull = null;
            item1.EnumInt32Field = EnumInt32Type.Zero;
            item1.EnumInt32FieldNull = null;
            item1.EnumInt64Field = EnumInt64Type.Zero;
            item1.EnumInt64FieldNull = null;
            var retUpdate = context.InsertOrUpdate(item1);
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retUpdate);
            var item3 = context.SelectById<TeBaseFieldEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item3);
            var retDelete = context.Delete(item1);
            Assert.Equal(1, retDelete);
            var item4 = context.SelectById<TeBaseFieldEntity>(item1.Id);
            Assert.Null(item4);
        }

        [Fact]
        public async Task TestCase_InsertOrUpdate_Single_Async()
        {
            await context.TruncateTableAsync<TeBaseFieldEntity>();
            var item1 = CreateBaseFieldEntityTableList(1)[0];
            var retInsert = await context.InsertOrUpdateAsync(item1);
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retInsert);
            var item2 = await context.SelectByIdAsync<TeBaseFieldEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item2);
            item1.DateTimeField = GetNow();
            item1.DateTimeFieldNull = null;
            item1.Int32Field = 2;
            item1.Int32FieldNull = null;
            item1.DoubleField = 2.0d;
            item1.DoubleFieldNull = null;
            item1.VarcharField = "abc";
            item1.VarcharFieldNull = null;
            item1.EnumInt32Field = EnumInt32Type.Zero;
            item1.EnumInt32FieldNull = null;
            item1.EnumInt64Field = EnumInt64Type.Zero;
            item1.EnumInt64FieldNull = null;
            var retUpdate = await context.InsertOrUpdateAsync(item1);
            Assert.Equal(1, item1.Id);
            Assert.Equal(1, retUpdate);
            var item3 = await context.SelectByIdAsync<TeBaseFieldEntity>(item1.Id);
            AssertExtend.StrictEqual(item1, item3);
            var retDelete = await context.DeleteAsync(item1);
            Assert.Equal(1, retDelete);
            var item4 = await context.SelectByIdAsync<TeBaseFieldEntity>(item1.Id);
            Assert.Null(item4);
        }

        [Fact]
        public void TestCase_CUD_Bulk()
        {
            const int count = 33;
            var listEx = CreateBaseFieldEntityTableList(count);
            List<TeBaseFieldEntity> listAc;
            context.TruncateTable<TeBaseFieldEntity>();
            var retInsert = context.BatchInsert(listEx);
            Assert.Equal(count, retInsert);
            listAc = context.Query<TeBaseFieldEntity>().ToList();
            AssertExtend.Equal(listEx, listAc);
            DateTime d = GetNow();
            listEx.ForEach(x => {
                x.DateTimeField = d;
                x.DateTimeFieldNull = null;
                x.Int32Field = 2;
                x.Int32FieldNull = null;
                x.DoubleField = 2.0d;
                x.DoubleFieldNull = null;
                x.VarcharField = "abc";
                x.VarcharFieldNull = null;
                x.EnumInt32Field = EnumInt32Type.Zero;
                x.EnumInt32FieldNull = null;
                x.EnumInt64Field = EnumInt64Type.Zero;
                x.EnumInt64FieldNull = null;
            });
            var retUpdate = context.BatchUpdate(listEx);
            Assert.Equal(count, retUpdate);
            listAc = context.Query<TeBaseFieldEntity>().ToList();
            AssertExtend.Equal(listEx, listAc);
            var retDelete = context.BatchDelete(listEx);
            Assert.Equal(count, retDelete);
            listAc = context.Query<TeBaseFieldEntity>().ToList();
            AssertExtend.Equal(0, listAc.Count);
        }

        [Fact]
        public async Task TestCase_CUD_Bulk_Async()
        {
            const int count = 33;
            var listEx = CreateBaseFieldEntityTableList(count);
            List<TeBaseFieldEntity> listAc;
            await context.TruncateTableAsync<TeBaseFieldEntity>();
            var retInsert = await context.BatchInsertAsync(listEx);
            Assert.Equal(count, retInsert);
            listAc = await context.Query<TeBaseFieldEntity>().ToListAsync();
            AssertExtend.Equal(listEx, listAc);
            DateTime d = GetNow();
            listEx.ForEach(x => {
                x.DateTimeField = d;
                x.DateTimeFieldNull = null;
                x.Int32Field = 2;
                x.Int32FieldNull = null;
                x.DoubleField = 2.0d;
                x.DoubleFieldNull = null;
                x.VarcharField = "abc";
                x.VarcharFieldNull = null;
                x.EnumInt32Field = EnumInt32Type.Zero;
                x.EnumInt32FieldNull = null;
                x.EnumInt64Field = EnumInt64Type.Zero;
                x.EnumInt64FieldNull = null;
            });
            var retUpdate = await context.BatchUpdateAsync(listEx);
            Assert.Equal(count, retUpdate);
            listAc = await context.Query<TeBaseFieldEntity>().ToListAsync();
            AssertExtend.Equal(listEx, listAc);
            var retDelete = await context.BatchDeleteAsync(listEx);
            Assert.Equal(count, retDelete);
            listAc = await context.Query<TeBaseFieldEntity>().ToListAsync();
            AssertExtend.Equal(0, listAc.Count);
        }
        #endregion
    }
}

