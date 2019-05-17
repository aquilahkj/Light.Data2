using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Light.Data.Mysql.Test
{
    public class Mysql_JoinTableTest : BaseTest
    {
        public Mysql_JoinTableTest(ITestOutputHelper output) : base(output)
        {
        }

        #region base test
        List<TeMainTable> CreateMainTableList(int count, int subCount)
        {
            List<TeMainTable> list = new List<TeMainTable>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeMainTable item = new TeMainTable();
                item.Int32Field = (int)((i % 23) * x);
                item.Int32FieldNull = i % 2 == 0 ? null : (int?)(item.Int32Field);
                item.DecimalField = (decimal)((i % 26) * 0.1 * x);
                item.DecimalFieldNull = i % 2 == 0 ? null : (decimal?)(item.DecimalField);
                item.DateTimeField = d.AddMinutes(i * 2);
                item.DateTimeFieldNull = i % 2 == 0 ? null : (DateTime?)(item.DateTimeField);
                item.VarcharField = "testtest" + item.Int32Field;
                item.VarcharFieldNull = i % 2 == 0 ? null : item.VarcharField;
                item.SubId = i % subCount;
                if (item.SubId == 0)
                    item.SubId = subCount;
                list.Add(item);
            }
            return list;
        }

        List<TeMainTable> CreateAndInsertMainTableList(int count, int subCount)
        {
            var list = CreateMainTableList(count, subCount);
            commandOutput.Enable = false;
            context.TruncateTable<TeMainTable>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        List<TeSubTable> CreateSubTableList(int count)
        {
            List<TeSubTable> list = new List<TeSubTable>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeSubTable item = new TeSubTable();
                item.Int32Field = (i % 23) * x;
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

        List<TeSubTable> CreateAndInsertSubTableList(int count)
        {
            var list = CreateSubTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeSubTable>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        public class ComposeTable
        {
            #region "Data Property"
            private int mainId;

            /// <summary>
            /// MainId
            /// </summary>
            /// <value></value>
            public int MainId {
                get {
                    return this.mainId;
                }
                set {
                    this.mainId = value;
                }
            }
            private int int32Field;

            /// <summary>
            /// Int32Field
            /// </summary>
            /// <value></value>
            public int Int32Field {
                get {
                    return this.int32Field;
                }
                set {
                    this.int32Field = value;
                }
            }
            private int? int32FieldNull;

            /// <summary>
            /// Int32FieldNull
            /// </summary>
            /// <value></value>
            public int? Int32FieldNull {
                get {
                    return this.int32FieldNull;
                }
                set {
                    this.int32FieldNull = value;
                }
            }
            private decimal decimalField;

            /// <summary>
            /// DecimalField
            /// </summary>
            /// <value></value>
            public decimal DecimalField {
                get {
                    return this.decimalField;
                }
                set {
                    this.decimalField = value;
                }
            }
            private decimal? decimalFieldNull;

            /// <summary>
            /// DecimalFieldNull
            /// </summary>
            /// <value></value>
            public decimal? DecimalFieldNull {
                get {
                    return this.decimalFieldNull;
                }
                set {
                    this.decimalFieldNull = value;
                }
            }
            private DateTime dateTimeField;

            /// <summary>
            /// DateTimeField
            /// </summary>
            /// <value></value>
            public DateTime DateTimeField {
                get {
                    return this.dateTimeField;
                }
                set {
                    this.dateTimeField = value;
                }
            }
            private DateTime? dateTimeFieldNull;

            /// <summary>
            /// DateTimeFieldNull
            /// </summary>
            /// <value></value>
            public DateTime? DateTimeFieldNull {
                get {
                    return this.dateTimeFieldNull;
                }
                set {
                    this.dateTimeFieldNull = value;
                }
            }
            private string varcharField;

            /// <summary>
            /// VarcharField
            /// </summary>
            /// <value></value>
            public string VarcharField {
                get {
                    return this.varcharField;
                }
                set {
                    this.varcharField = value;
                }
            }
            private string varcharFieldNull;

            /// <summary>
            /// VarcharFieldNull
            /// </summary>
            /// <value></value>
            public string VarcharFieldNull {
                get {
                    return this.varcharFieldNull;
                }
                set {
                    this.varcharFieldNull = value;
                }
            }
            private int subId;

            /// <summary>
            /// SubId
            /// </summary>
            /// <value></value>
            public int SubId {
                get {
                    return this.subId;
                }
                set {
                    this.subId = value;
                }
            }
            #endregion

            private int sub_int32Field;

            /// <summary>
            /// Int32Field
            /// </summary>
            /// <value></value>
            public int SubInt32Field {
                get {
                    return this.sub_int32Field;
                }
                set {
                    this.sub_int32Field = value;
                }
            }
            private int? sub_int32FieldNull;

            /// <summary>
            /// Int32FieldNull
            /// </summary>
            /// <value></value>
            public int? SubInt32FieldNull {
                get {
                    return this.sub_int32FieldNull;
                }
                set {
                    this.sub_int32FieldNull = value;
                }
            }
            private decimal sub_decimalField;

            /// <summary>
            /// DecimalField
            /// </summary>
            /// <value></value>
            public decimal SubDecimalField {
                get {
                    return this.sub_decimalField;
                }
                set {
                    this.sub_decimalField = value;
                }
            }
            private decimal? sub_decimalFieldNull;

            /// <summary>
            /// DecimalFieldNull
            /// </summary>
            /// <value></value>
            public decimal? SubDecimalFieldNull {
                get {
                    return this.sub_decimalFieldNull;
                }
                set {
                    this.sub_decimalFieldNull = value;
                }
            }
            private DateTime sub_dateTimeField;

            /// <summary>
            /// DateTimeField
            /// </summary>
            /// <value></value>
            public DateTime SubDateTimeField {
                get {
                    return this.sub_dateTimeField;
                }
                set {
                    this.sub_dateTimeField = value;
                }
            }
            private DateTime? sub_dateTimeFieldNull;

            /// <summary>
            /// DateTimeFieldNull
            /// </summary>
            /// <value></value>
            public DateTime? SubDateTimeFieldNull {
                get {
                    return this.sub_dateTimeFieldNull;
                }
                set {
                    this.sub_dateTimeFieldNull = value;
                }
            }
            private string sub_varcharField;

            /// <summary>
            /// VarcharField
            /// </summary>
            /// <value></value>
            public string SubVarcharField {
                get {
                    return this.sub_varcharField;
                }
                set {
                    this.sub_varcharField = value;
                }
            }
            private string sub_varcharFieldNull;

            /// <summary>
            /// VarcharFieldNull
            /// </summary>
            /// <value></value>
            public string SubVarcharFieldNull {
                get {
                    return this.sub_varcharFieldNull;
                }
                set {
                    this.sub_varcharFieldNull = value;
                }
            }
        }

        [Fact]
        public void TestCase_JoinQuery()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToList();

            var listAc = context.Query<TeMainTable>().Join<TeSubTable>(context.Query<TeSubTable>(), (x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_JoinBase()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToList();

            var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToArray();

            var arrayAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToArray();
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var selectEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            });

            var selectAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                });
            AssertExtend.Equal(selectEx, selectAc);

            var firstEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).First();

            var firstAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).First();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ElementAt(5);

            var elementAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ElementAt(5);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public async Task TestCase_JoinBase_Async()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToList();

            var listAc = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToListAsync(CancellationToken.None);
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToArray();

            var arrayAc = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToArrayAsync(CancellationToken.None);
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var firstEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).First();

            var firstAc = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).FirstAsync(CancellationToken.None);
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ElementAt(5);

            var elementAc = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ElementAtAsync(5, CancellationToken.None);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public void TestCase_JoinBase_M2()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToList();

            var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToArray();

            var arrayAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToArray();
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var selectEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            });

            var selectAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                });
            AssertExtend.Equal(selectEx, selectAc);

            var firstEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).First();

            var firstAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).First();
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ElementAt(5);

            var elementAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ElementAt(5);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }

        [Fact]
        public async Task TestCase_JoinBase_M2_Async()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToList();

            var listAc = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToListAsync(CancellationToken.None);
            AssertExtend.StrictEqual(listEx, listAc);

            var arrayEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ToArray();

            var arrayAc = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToArrayAsync(CancellationToken.None);
            AssertExtend.StrictEqual(arrayEx, arrayAc);

            var firstEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).First();

            var firstAc = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).FirstAsync(CancellationToken.None);
            AssertExtend.StrictEqual(firstEx, firstAc);

            var elementEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).ElementAt(5);

            var elementAc = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ElementAtAsync(5, CancellationToken.None);
            AssertExtend.StrictEqual(elementEx, elementAc);
        }



        [Fact]
        public void TestCase_LeftJoin_SubEntity()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(5);

            var listEx = (from x in listMain
                          join y in listSub on x.SubId equals y.SubId into ps
                          from p in ps.DefaultIfEmpty()
                          select new {
                              MainId = x.MainId,
                              Int32Field = x.Int32Field,
                              Int32FieldNull = x.Int32FieldNull,
                              DecimalField = x.DecimalField,
                              DecimalFieldNull = x.DecimalFieldNull,
                              DateTimeField = x.DateTimeField,
                              DateTimeFieldNull = x.DateTimeFieldNull,
                              VarcharField = x.VarcharField,
                              VarcharFieldNull = x.VarcharFieldNull,
                              SubId = x.SubId,
                              SubTable = p
                          }).OrderBy(x => x.MainId).ToList();

            var listAc = context.Query<TeMainTable>().LeftJoin<TeSubTable>((x, y) => x.SubId == y.SubId, JoinSetting.NoDataSetEntityNull).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubTable = y
                }).ToList().OrderBy(x => x.MainId).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_LeftJoin_SubField()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(5);

            var listEx = (from x in listMain
                          join y in listSub on x.SubId equals y.SubId into ps
                          from p in ps.DefaultIfEmpty()
                          select new {
                              MainId = x.MainId,
                              Int32Field = x.Int32Field,
                              Int32FieldNull = x.Int32FieldNull,
                              DecimalField = x.DecimalField,
                              DecimalFieldNull = x.DecimalFieldNull,
                              DateTimeField = x.DateTimeField,
                              DateTimeFieldNull = x.DateTimeFieldNull,
                              VarcharField = x.VarcharField,
                              VarcharFieldNull = x.VarcharFieldNull,
                              SubId = x.SubId,
                              SubVarcharFieldNull = p?.VarcharFieldNull
                          }).OrderBy(x => x.MainId).ToList();

            var listAc = context.Query<TeMainTable>().LeftJoin<TeSubTable>((x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubVarcharFieldNull = y.VarcharFieldNull
                }).ToList().OrderBy(x => x.MainId).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        #region order by test
        [Fact]
        public void TestCase_JoinBase_OrderBy_Int()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            List<ComposeTable> listEx = null;
            List<ComposeTable> listAc = null;

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.Int32Field).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderBy((x, y) => x.Int32Field)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderByDescending(x => x.Int32Field).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderByDescending((x, y) => x.Int32Field)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.SubInt32Field).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderBy((x, y) => y.Int32Field)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderByDescending(x => x.SubInt32Field).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderByDescending((x, y) => y.Int32Field)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_JoinBase_OrderBy_Decimal()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            List<ComposeTable> listEx = null;
            List<ComposeTable> listAc = null;

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.DecimalField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderBy((x, y) => x.DecimalField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderByDescending(x => x.DecimalField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderByDescending((x, y) => x.DecimalField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).OrderByDescending(x => x.DecimalField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.SubDecimalField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderBy((x, y) => y.DecimalField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderByDescending(x => x.SubDecimalField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderByDescending((x, y) => y.DecimalField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).OrderByDescending(x => x.DecimalField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_JoinBase_OrderBy_Datetime()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            List<ComposeTable> listEx = null;
            List<ComposeTable> listAc = null;

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.SubDateTimeField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderBy((x, y) => y.DateTimeField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderByDescending(x => x.SubDateTimeField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderByDescending((x, y) => y.DateTimeField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.SubDateTimeField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderBy((x, y) => y.DateTimeField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderByDescending(x => x.SubDateTimeField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderByDescending((x, y) => y.DateTimeField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_JoinBase_OrderBy_Replace()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            List<ComposeTable> listEx = null;
            List<ComposeTable> listAc = null;

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderByDescending(x => x.DecimalField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderBy((x, y) => x.Int32Field)
                .OrderByDescending((x, y) => x.DecimalField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }

        [Fact]
        public void TestCase_JoinBase_OrderBy_Catch()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(45, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            List<ComposeTable> listEx = null;
            List<ComposeTable> listAc = null;

            listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                MainId = x.MainId,
                Int32Field = x.Int32Field,
                Int32FieldNull = x.Int32FieldNull,
                DecimalField = x.DecimalField,
                DecimalFieldNull = x.DecimalFieldNull,
                DateTimeField = x.DateTimeField,
                DateTimeFieldNull = x.DateTimeFieldNull,
                VarcharField = x.VarcharField,
                VarcharFieldNull = x.VarcharFieldNull,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.Int32Field).ThenByDescending(x => x.DecimalField).ToList();

            listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .OrderBy((x, y) => x.Int32Field)
                .OrderByDescendingConcat((x, y) => x.DecimalField)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }

        #endregion

        #region where test
        [Fact]
        public void TestCase_JoinBase_Where()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            List<ComposeTable> listEx = null;
            List<ComposeTable> listAc = null;

            listEx = listMain.Where(x => x.Int32Field > 5)
                .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToList();

            listAc = context.Query<TeMainTable>().Where(x => x.Int32Field > 5)
                .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain.Where(x => x.Int32Field > 5 && x.Int32Field < 9)
                .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToList();

            listAc = context.Query<TeMainTable>().Where(x => x.Int32Field > 5).WhereWithAnd(x => x.Int32Field < 9)
                .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain
                .Join(listSub.Where(x => x.Int32Field > 5), x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToList();

            listAc = context.Query<TeMainTable>()
                .Join<TeSubTable>(x => x.Int32Field > 5, (x, y) => x.SubId == y.SubId)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain
              .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                  MainId = x.MainId,
                  Int32Field = x.Int32Field,
                  Int32FieldNull = x.Int32FieldNull,
                  DecimalField = x.DecimalField,
                  DecimalFieldNull = x.DecimalFieldNull,
                  DateTimeField = x.DateTimeField,
                  DateTimeFieldNull = x.DateTimeFieldNull,
                  VarcharField = x.VarcharField,
                  VarcharFieldNull = x.VarcharFieldNull,
                  SubId = x.SubId,
                  SubInt32Field = y.Int32Field,
                  SubInt32FieldNull = y.Int32FieldNull,
                  SubDecimalField = y.DecimalField,
                  SubDecimalFieldNull = y.DecimalFieldNull,
                  SubDateTimeField = y.DateTimeField,
                  SubDateTimeFieldNull = y.DateTimeFieldNull,
                  SubVarcharField = y.VarcharField,
                  SubVarcharFieldNull = y.VarcharFieldNull,
              }).Where(x => x.Int32Field > 5).ToList();

            listAc = context.Query<TeMainTable>()
                .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .Where((x, y) => x.Int32Field > 5)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_JoinBase_Where_And()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            List<ComposeTable> listEx = null;
            List<ComposeTable> listAc = null;

            listEx = listMain
                .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.Int32Field > 5 && x.Int32Field < 8).ToList();

            listAc = context.Query<TeMainTable>()
                .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .Where((x, y) => x.Int32Field > 5)
                .WhereWithAnd((x, y) => x.Int32Field < 8)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain
                .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.Int32Field > 5 && x.SubInt32Field < 8).ToList();

            listAc = context.Query<TeMainTable>()
                .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .Where((x, y) => x.Int32Field > 5)
                .WhereWithAnd((x, y) => y.Int32Field < 8)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_JoinBase_Where_Or()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            List<ComposeTable> listEx = null;
            List<ComposeTable> listAc = null;

            listEx = listMain
                .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.Int32Field < 5 || x.Int32Field > 8).ToList();

            listAc = context.Query<TeMainTable>()
                .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .Where((x, y) => x.Int32Field < 5)
                .WhereWithOr((x, y) => x.Int32Field > 8)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

            listEx = listMain
                .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new ComposeTable {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.Int32Field < 5 || x.SubInt32Field > 8).ToList();

            listAc = context.Query<TeMainTable>()
                .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                .Where((x, y) => x.Int32Field < 5)
                .WhereWithOr((x, y) => y.Int32Field > 8)
                .Select(
               (x, y) => new ComposeTable {
                   MainId = x.MainId,
                   Int32Field = x.Int32Field,
                   Int32FieldNull = x.Int32FieldNull,
                   DecimalField = x.DecimalField,
                   DecimalFieldNull = x.DecimalFieldNull,
                   DateTimeField = x.DateTimeField,
                   DateTimeFieldNull = x.DateTimeFieldNull,
                   VarcharField = x.VarcharField,
                   VarcharFieldNull = x.VarcharFieldNull,
                   SubId = x.SubId,
                   SubInt32Field = y.Int32Field,
                   SubInt32FieldNull = y.Int32FieldNull,
                   SubDecimalField = y.DecimalField,
                   SubDecimalFieldNull = y.DecimalFieldNull,
                   SubDateTimeField = y.DateTimeField,
                   SubDateTimeFieldNull = y.DateTimeFieldNull,
                   SubVarcharField = y.VarcharField,
                   SubVarcharFieldNull = y.VarcharFieldNull,
               }).ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        #endregion

        #region join other test
        [Fact]
        public void TestCase_Select_Join()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listMainSelect = listMain.Select(x => new {
                x.MainId,
                x.SubId,
                x.Int32Field,
                x.Int32FieldNull
            }).ToList();

            var listSubSelect = listMain.Select(x => new {
                x.SubId,
                x.Int32Field,
                x.Int32FieldNull
            }).ToList();

            {
                var listEx = listMainSelect
                    .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainSelect
                    .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join(context.Query<TeSubTable>(), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainSelect
                    .Join(listSub.Where(x => x.Int32Field > 5), x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join(context.Query<TeSubTable>().Where(x => x.Int32Field > 5), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain
                  .Join(listSubSelect, x => x.SubId, y => y.SubId, (x, y) => new {
                      MainId = x.MainId,
                      Int32Field = x.Int32Field,
                      Int32FieldNull = x.Int32FieldNull,
                      DecimalField = x.DecimalField,
                      DecimalFieldNull = x.DecimalFieldNull,
                      DateTimeField = x.DateTimeField,
                      DateTimeFieldNull = x.DateTimeFieldNull,
                      VarcharField = x.VarcharField,
                      VarcharFieldNull = x.VarcharFieldNull,
                      SubId = x.SubId,
                      SubInt32Field = y.Int32Field,
                      SubInt32FieldNull = y.Int32FieldNull,
                  }).ToList();

                var listAc = context.Query<TeMainTable>()
                      .Join(context.Query<TeSubTable>().Select(x => new {
                          x.SubId,
                          x.Int32Field,
                          x.Int32FieldNull
                      }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         MainId = x.MainId,
                         Int32Field = x.Int32Field,
                         Int32FieldNull = x.Int32FieldNull,
                         DecimalField = x.DecimalField,
                         DecimalFieldNull = x.DecimalFieldNull,
                         DateTimeField = x.DateTimeField,
                         DateTimeFieldNull = x.DateTimeFieldNull,
                         VarcharField = x.VarcharField,
                         VarcharFieldNull = x.VarcharFieldNull,
                         SubId = x.SubId,
                         SubInt32Field = y.Int32Field,
                         SubInt32FieldNull = y.Int32FieldNull,
                     }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainSelect
                  .Join(listSubSelect, x => x.SubId, y => y.SubId, (x, y) => new {
                      MainId = x.MainId,
                      Int32Field = x.Int32Field,
                      Int32FieldNull = x.Int32FieldNull,
                      SubId = x.SubId,
                      SubInt32Field = y.Int32Field,
                      SubInt32FieldNull = y.Int32FieldNull,
                  }).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join(context.Query<TeSubTable>().Select(x => new {
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         MainId = x.MainId,
                         Int32Field = x.Int32Field,
                         Int32FieldNull = x.Int32FieldNull,
                         SubId = x.SubId,
                         SubInt32Field = y.Int32Field,
                         SubInt32FieldNull = y.Int32FieldNull,
                     }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        [Fact]
        public void TestCase_Select_Join_Entity()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listMainSelect = listMain.Select(x => new {
                x.MainId,
                x.SubId,
                x.Int32Field,
                x.Int32FieldNull
            }).ToList();

            var listSubSelect = listMain.Select(x => new {
                x.SubId,
                x.Int32Field,
                x.Int32FieldNull
            }).ToList();

            {
                var listEx = listMainSelect
                    .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubTable = y,
                    }).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubTable = y
                   }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainSelect
                    .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubTable = y,
                    }).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join(context.Query<TeSubTable>(), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubTable = y
                   }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainSelect
                    .Join(listSub.Where(x => x.Int32Field > 5), x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubTable = y,
                    }).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join(context.Query<TeSubTable>().Where(x => x.Int32Field > 5), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubTable = y
                   }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain
                  .Join(listSubSelect, x => x.SubId, y => y.SubId, (x, y) => new {
                      MainTable = x,
                      SubId = y.SubId,
                      SubInt32Field = y.Int32Field,
                      SubInt32FieldNull = y.Int32FieldNull,
                  }).ToList();

                var listAc = context.Query<TeMainTable>()
                      .Join(context.Query<TeSubTable>().Select(x => new {
                          x.SubId,
                          x.Int32Field,
                          x.Int32FieldNull
                      }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         MainTable = x,
                         SubId = y.SubId,
                         SubInt32Field = y.Int32Field,
                         SubInt32FieldNull = y.Int32FieldNull,
                     }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainSelect
                  .Join(listSubSelect, x => x.SubId, y => y.SubId, (x, y) => new {
                      MainTable = x,
                      SubTable = y,
                  }).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join(context.Query<TeSubTable>().Select(x => new {
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         MainTable = x,
                         SubTable = y,
                     }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        [Fact]
        public void TestCase_Aggregate_Join()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(40, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listMainAggregate = listMain.GroupBy(x => x.SubId).Select(x => new {
                SubId = x.Key,
                Count = x.Count(),
            }).ToList();

            var listSubAggregate = listSub.GroupBy(x => x.SubId).Select(x => new {
                SubId = x.Key,
                Count = x.Count(),
            }).ToList();

            {
                var listEx = listMainAggregate
                    .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        SubId = x.SubId,
                        Count = x.Count,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).OrderBy(x => x.SubId).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    })
                    .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       SubId = x.SubId,
                       Count = x.Count,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList().OrderBy(x => x.SubId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainAggregate
                    .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        SubId = x.SubId,
                        Count = x.Count,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).OrderBy(x => x.SubId).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    })
                    .Join(context.Query<TeSubTable>(), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       SubId = x.SubId,
                       Count = x.Count,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList().OrderBy(x => x.SubId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainAggregate
                    .Join(listSub.Where(x => x.Int32Field > 5), x => x.SubId, y => y.SubId, (x, y) => new {
                        SubId = x.SubId,
                        Count = x.Count,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).OrderBy(x => x.SubId).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    })
                    .Join(context.Query<TeSubTable>().Where(x => x.Int32Field > 5), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       SubId = x.SubId,
                       Count = x.Count,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList().OrderBy(x => x.SubId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain
                  .Join(listSubAggregate, x => x.SubId, y => y.SubId, (x, y) => new {
                      MainId = x.MainId,
                      Int32Field = x.Int32Field,
                      Int32FieldNull = x.Int32FieldNull,
                      DecimalField = x.DecimalField,
                      DecimalFieldNull = x.DecimalFieldNull,
                      DateTimeField = x.DateTimeField,
                      DateTimeFieldNull = x.DateTimeFieldNull,
                      VarcharField = x.VarcharField,
                      VarcharFieldNull = x.VarcharFieldNull,
                      SubId = y.SubId,
                      Count = y.Count
                  }).OrderBy(x => x.MainId).ToList();

                var listAc = context.Query<TeMainTable>()
                      .Join(context.Query<TeSubTable>().Aggregate(x => new {
                          SubId = x.SubId,
                          Count = Function.Count(),
                      }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         MainId = x.MainId,
                         Int32Field = x.Int32Field,
                         Int32FieldNull = x.Int32FieldNull,
                         DecimalField = x.DecimalField,
                         DecimalFieldNull = x.DecimalFieldNull,
                         DateTimeField = x.DateTimeField,
                         DateTimeFieldNull = x.DateTimeFieldNull,
                         VarcharField = x.VarcharField,
                         VarcharFieldNull = x.VarcharFieldNull,
                         SubId = y.SubId,
                         Count = y.Count,
                     }).OrderBy(x => x.MainId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainAggregate
                  .Join(listSubAggregate, x => x.SubId, y => y.SubId, (x, y) => new {
                      SubIdx = x.SubId,
                      SubIdy = y.SubId,
                      Countx = x.Count,
                      County = y.Count,
                  }).ToList().OrderBy(x => x.SubIdx).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    })
                    .Join(context.Query<TeSubTable>().Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         SubIdx = x.SubId,
                         SubIdy = y.SubId,
                         Countx = x.Count,
                         County = y.Count,
                     }).ToList().OrderBy(x => x.SubIdx).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        [Fact]
        public void TestCase_Aggregate_Join_Entity()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(40, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listMainAggregate = listMain.GroupBy(x => x.SubId).Select(x => new {
                SubId = x.Key,
                Count = x.Count(),
            }).ToList();

            var listSubAggregate = listSub.GroupBy(x => x.SubId).Select(x => new {
                SubId = x.Key,
                Count = x.Count(),
            }).ToList();

            {
                var listEx = listMainAggregate
                    .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainTable = x,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).OrderBy(x => x.MainTable.SubId).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    })
                    .Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainTable = x,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList().OrderBy(x => x.MainTable.SubId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainAggregate
                    .Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainTable = x,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).OrderBy(x => x.MainTable.SubId).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    })
                    .Join(context.Query<TeSubTable>(), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainTable = x,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList().OrderBy(x => x.MainTable.SubId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainAggregate
                    .Join(listSub.Where(x => x.Int32Field > 5), x => x.SubId, y => y.SubId, (x, y) => new {
                        MainTable = x,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).OrderBy(x => x.MainTable.SubId).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    })
                    .Join(context.Query<TeSubTable>().Where(x => x.Int32Field > 5), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainTable = x,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                       SubDecimalField = y.DecimalField,
                       SubDecimalFieldNull = y.DecimalFieldNull,
                       SubDateTimeField = y.DateTimeField,
                       SubDateTimeFieldNull = y.DateTimeFieldNull,
                       SubVarcharField = y.VarcharField,
                       SubVarcharFieldNull = y.VarcharFieldNull,
                   }).ToList().OrderBy(x => x.MainTable.SubId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain
                  .Join(listSubAggregate, x => x.SubId, y => y.SubId, (x, y) => new {
                      MainId = x.MainId,
                      Int32Field = x.Int32Field,
                      Int32FieldNull = x.Int32FieldNull,
                      DecimalField = x.DecimalField,
                      DecimalFieldNull = x.DecimalFieldNull,
                      DateTimeField = x.DateTimeField,
                      DateTimeFieldNull = x.DateTimeFieldNull,
                      VarcharField = x.VarcharField,
                      VarcharFieldNull = x.VarcharFieldNull,
                      SubTable = y,
                  }).OrderBy(x => x.MainId).ToList();

                var listAc = context.Query<TeMainTable>()
                      .Join(context.Query<TeSubTable>().Aggregate(x => new {
                          SubId = x.SubId,
                          Count = Function.Count(),
                      }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         MainId = x.MainId,
                         Int32Field = x.Int32Field,
                         Int32FieldNull = x.Int32FieldNull,
                         DecimalField = x.DecimalField,
                         DecimalFieldNull = x.DecimalFieldNull,
                         DateTimeField = x.DateTimeField,
                         DateTimeFieldNull = x.DateTimeFieldNull,
                         VarcharField = x.VarcharField,
                         VarcharFieldNull = x.VarcharFieldNull,
                         SubTable = y,
                     }).OrderBy(x => x.MainId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainAggregate
                  .Join(listSubAggregate, x => x.SubId, y => y.SubId, (x, y) => new {
                      MainTable = x,
                      SubTable = y,
                  }).ToList().OrderBy(x => x.MainTable.SubId).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    })
                    .Join(context.Query<TeSubTable>().Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         MainTable = x,
                         SubTable = y,
                     }).ToList().OrderBy(x => x.MainTable.SubId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        [Fact]
        public void TestCase_Select_Aggregate_Join()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listMainSelect = listMain.Select(x => new {
                x.MainId,
                x.SubId,
                x.Int32Field,
                x.Int32FieldNull
            }).ToList();

            var listSubSelect = listMain.Select(x => new {
                x.SubId,
                x.Int32Field,
                x.Int32FieldNull
            }).ToList();

            var listMainAggregate = listMain.GroupBy(x => x.SubId).Select(x => new {
                SubId = x.Key,
                Count = x.Count(),
            }).ToList();

            var listSubAggregate = listSub.GroupBy(x => x.SubId).Select(x => new {
                SubId = x.Key,
                Count = x.Count(),
            }).ToList();

            {
                var listEx = listMainSelect
                    .Join(listSubAggregate, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = y.SubId,
                        Count = y.Count,
                    }).OrderBy(x => x.MainId).ToList();

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join(context.Query<TeSubTable>().Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    }), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = y.SubId,
                       Count = y.Count,
                   }).ToList().OrderBy(x => x.MainId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainAggregate
                  .Join(listSubSelect, x => x.SubId, y => y.SubId, (x, y) => new {
                      SubId = x.SubId,
                      Count = x.Count,
                      SubInt32Field = y.Int32Field,
                      SubInt32FieldNull = y.Int32FieldNull,
                  }).OrderBy(x => x.SubId).ToList();

                var listAc = context.Query<TeMainTable>().Aggregate(x => new {
                    SubId = x.SubId,
                    Count = Function.Count()
                }).Join(context.Query<TeSubTable>().Select(x => new {
                    x.SubId,
                    x.Int32Field,
                    x.Int32FieldNull
                }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         SubId = x.SubId,
                         Count = x.Count,
                         SubInt32Field = y.Int32Field,
                         SubInt32FieldNull = y.Int32FieldNull
                     }).ToList().OrderBy(x => x.SubId).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

        }

        [Fact]
        public void TestCase_Select_Aggregate_Join_Entity()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(10, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            var listMainSelect = listMain.Select(x => new {
                x.MainId,
                x.SubId,
                x.Int32Field,
                x.Int32FieldNull
            }).ToList();

            var listSubSelect = listMain.Select(x => new {
                x.SubId,
                x.Int32Field,
                x.Int32FieldNull
            }).ToList();

            var listMainAggregate = listMain.GroupBy(x => x.SubId).Select(x => new {
                SubId = x.Key,
                Count = x.Count(),
            }).ToList();

            var listSubAggregate = listSub.GroupBy(x => x.SubId).Select(x => new {
                SubId = x.Key,
                Count = x.Count(),
            }).ToList();

            {
                var listEx = listMainSelect
                    .Join(listSubAggregate, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainTable = x,
                        SubTable = y,
                    }).OrderBy(x => x.MainTable.MainId);

                var listAc = context.Query<TeMainTable>()
                    .Select(x => new {
                        x.MainId,
                        x.SubId,
                        x.Int32Field,
                        x.Int32FieldNull
                    })
                    .Join(context.Query<TeSubTable>().Aggregate(x => new {
                        SubId = x.SubId,
                        Count = Function.Count()
                    }), (x, y) => x.SubId == y.SubId)
                    .Select(
                   (x, y) => new {
                       MainTable = x,
                       SubTable = y,
                   }).ToList().OrderBy(x => x.MainTable.MainId);
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMainAggregate
                  .Join(listSubSelect, x => x.SubId, y => y.SubId, (x, y) => new {
                      MainTable = x,
                      SubTable = y,
                  }).OrderBy(x => x.MainTable.SubId);

                var listAc = context.Query<TeMainTable>().Aggregate(x => new {
                    SubId = x.SubId,
                    Count = Function.Count()
                }).Join(context.Query<TeSubTable>().Select(x => new {
                    x.SubId,
                    x.Int32Field,
                    x.Int32FieldNull
                }), (x, y) => x.SubId == y.SubId)
                      .Select(
                     (x, y) => new {
                         MainTable = x,
                         SubTable = y,
                     }).ToList().OrderBy(x => x.MainTable.SubId);
                AssertExtend.StrictEqual(listEx, listAc);
            }

        }
        #endregion


        #region select insert

        [Fact]
        public void TestCase_Join_SelectInsert()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(45, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            {
                context.TruncateTable<TeJoinTableSelectInsert>();
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId,
                    (x, y) => new TeJoinTableSelectInsert {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                    }).OrderBy(x => x.MainId).ToList();

                var ret = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).OrderBy((x, y) => x.MainId).SelectInsert(
                   (x, y) => new TeJoinTableSelectInsert {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                   });
                Assert.Equal(listEx.Count, ret);
                var listAc = context.Query<TeJoinTableSelectInsert>().ToList();

                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                context.TruncateTable<TeJoinTableSelectInsert>();
                var listEx = listMain.Where(x => x.MainId > 15).Join(listSub, x => x.SubId, y => y.SubId,
                    (x, y) => new TeJoinTableSelectInsert {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                    }).OrderBy(x => x.MainId).ToList();

                var ret = context.Query<TeMainTable>().Where(x => x.MainId > 15).Join<TeSubTable>((x, y) => x.SubId == y.SubId).OrderBy((x, y) => x.MainId).SelectInsert(
                   (x, y) => new TeJoinTableSelectInsert {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                   });
                Assert.Equal(listEx.Count, ret);
                var listAc = context.Query<TeJoinTableSelectInsert>().ToList();

                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        [Fact]
        public async Task TestCase_Join_SelectInsert_Async()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(45, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            {
                context.TruncateTable<TeJoinTableSelectInsert>();
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId,
                    (x, y) => new TeJoinTableSelectInsert {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                    }).OrderBy(x => x.MainId).ToList();

                var ret = await context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId).OrderBy((x, y) => x.MainId).SelectInsertAsync(
                   (x, y) => new TeJoinTableSelectInsert {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                   }, CancellationToken.None);
                Assert.Equal(listEx.Count, ret);
                var listAc = context.Query<TeJoinTableSelectInsert>().ToList();

                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                context.TruncateTable<TeJoinTableSelectInsert>();
                var listEx = listMain.Where(x => x.MainId > 15).Join(listSub, x => x.SubId, y => y.SubId,
                    (x, y) => new TeJoinTableSelectInsert {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                    }).OrderBy(x => x.MainId).ToList();

                var ret = await context.Query<TeMainTable>().Where(x => x.MainId > 15).Join<TeSubTable>((x, y) => x.SubId == y.SubId).OrderBy((x, y) => x.MainId).SelectInsertAsync(
                   (x, y) => new TeJoinTableSelectInsert {
                       MainId = x.MainId,
                       Int32Field = x.Int32Field,
                       Int32FieldNull = x.Int32FieldNull,
                       SubId = x.SubId,
                       SubInt32Field = y.Int32Field,
                       SubInt32FieldNull = y.Int32FieldNull,
                   }, CancellationToken.None);
                Assert.Equal(listEx.Count, ret);
                var listAc = context.Query<TeJoinTableSelectInsert>().ToList();

                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        #endregion

        [Fact]
        public void TestCase_JoinBase_PageSize()
        {
            const int tol = 21;
            const int cnt = 8;

            List<TeMainTable> listMain = CreateAndInsertMainTableList(30, 30);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(30);
            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                {
                    var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).Skip(cnt * i).Take(cnt).ToList();

                    var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                        .PageSize(i + 1, cnt)
                        .Select(
                        (x, y) => new {
                            MainId = x.MainId,
                            Int32Field = x.Int32Field,
                            Int32FieldNull = x.Int32FieldNull,
                            DecimalField = x.DecimalField,
                            DecimalFieldNull = x.DecimalFieldNull,
                            DateTimeField = x.DateTimeField,
                            DateTimeFieldNull = x.DateTimeFieldNull,
                            VarcharField = x.VarcharField,
                            VarcharFieldNull = x.VarcharFieldNull,
                            SubId = x.SubId,
                            SubInt32Field = y.Int32Field,
                            SubInt32FieldNull = y.Int32FieldNull,
                            SubDecimalField = y.DecimalField,
                            SubDecimalFieldNull = y.DecimalFieldNull,
                            SubDateTimeField = y.DateTimeField,
                            SubDateTimeFieldNull = y.DateTimeFieldNull,
                            SubVarcharField = y.VarcharField,
                            SubVarcharFieldNull = y.VarcharFieldNull,
                        }).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }

                {
                    var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).Skip(cnt * i).Take(cnt).ToList();

                    var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                        .Range(i * cnt, (i + 1) * cnt)
                        .Select(
                        (x, y) => new {
                            MainId = x.MainId,
                            Int32Field = x.Int32Field,
                            Int32FieldNull = x.Int32FieldNull,
                            DecimalField = x.DecimalField,
                            DecimalFieldNull = x.DecimalFieldNull,
                            DateTimeField = x.DateTimeField,
                            DateTimeFieldNull = x.DateTimeFieldNull,
                            VarcharField = x.VarcharField,
                            VarcharFieldNull = x.VarcharFieldNull,
                            SubId = x.SubId,
                            SubInt32Field = y.Int32Field,
                            SubInt32FieldNull = y.Int32FieldNull,
                            SubDecimalField = y.DecimalField,
                            SubDecimalFieldNull = y.DecimalFieldNull,
                            SubDateTimeField = y.DateTimeField,
                            SubDateTimeFieldNull = y.DateTimeFieldNull,
                            SubVarcharField = y.VarcharField,
                            SubVarcharFieldNull = y.VarcharFieldNull,
                        }).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.MainId > cnt).Take(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Where((x, y) => x.MainId > cnt)
                    .PageSize(1, cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).OrderByDescending(x => x.MainId).Take(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .OrderByDescending((x, y) => x.MainId)
                    .PageSize(1, cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.MainId > cnt).Take(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Where((x, y) => x.MainId > cnt)
                    .Range(0, cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).OrderByDescending(x => x.MainId).Take(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .OrderByDescending((x, y) => x.MainId)
                    .Range(0, cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }


        [Fact]
        public void TestCase_JoinBase_PageTakeSkip()
        {
            const int tol = 21;
            const int cnt = 8;

            List<TeMainTable> listMain = CreateAndInsertMainTableList(30, 30);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(30);
            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++) {
                {
                    var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).Skip(cnt * i).ToList();

                    var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                        .Skip(cnt * i)
                        .Select(
                        (x, y) => new {
                            MainId = x.MainId,
                            Int32Field = x.Int32Field,
                            Int32FieldNull = x.Int32FieldNull,
                            DecimalField = x.DecimalField,
                            DecimalFieldNull = x.DecimalFieldNull,
                            DateTimeField = x.DateTimeField,
                            DateTimeFieldNull = x.DateTimeFieldNull,
                            VarcharField = x.VarcharField,
                            VarcharFieldNull = x.VarcharFieldNull,
                            SubId = x.SubId,
                            SubInt32Field = y.Int32Field,
                            SubInt32FieldNull = y.Int32FieldNull,
                            SubDecimalField = y.DecimalField,
                            SubDecimalFieldNull = y.DecimalFieldNull,
                            SubDateTimeField = y.DateTimeField,
                            SubDateTimeFieldNull = y.DateTimeFieldNull,
                            SubVarcharField = y.VarcharField,
                            SubVarcharFieldNull = y.VarcharFieldNull,
                        }).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }

                {
                    var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).Take(cnt * i).ToList();

                    var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                        .Take(cnt * i)
                        .Select(
                        (x, y) => new {
                            MainId = x.MainId,
                            Int32Field = x.Int32Field,
                            Int32FieldNull = x.Int32FieldNull,
                            DecimalField = x.DecimalField,
                            DecimalFieldNull = x.DecimalFieldNull,
                            DateTimeField = x.DateTimeField,
                            DateTimeFieldNull = x.DateTimeFieldNull,
                            VarcharField = x.VarcharField,
                            VarcharFieldNull = x.VarcharFieldNull,
                            SubId = x.SubId,
                            SubInt32Field = y.Int32Field,
                            SubInt32FieldNull = y.Int32FieldNull,
                            SubDecimalField = y.DecimalField,
                            SubDecimalFieldNull = y.DecimalFieldNull,
                            SubDateTimeField = y.DateTimeField,
                            SubDateTimeFieldNull = y.DateTimeFieldNull,
                            SubVarcharField = y.VarcharField,
                            SubVarcharFieldNull = y.VarcharFieldNull,
                        }).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }

                {
                    var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).Skip(cnt * i).Take(cnt).ToList();

                    var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                        .Skip(cnt * i).Take(cnt)
                        .Select(
                        (x, y) => new {
                            MainId = x.MainId,
                            Int32Field = x.Int32Field,
                            Int32FieldNull = x.Int32FieldNull,
                            DecimalField = x.DecimalField,
                            DecimalFieldNull = x.DecimalFieldNull,
                            DateTimeField = x.DateTimeField,
                            DateTimeFieldNull = x.DateTimeFieldNull,
                            VarcharField = x.VarcharField,
                            VarcharFieldNull = x.VarcharFieldNull,
                            SubId = x.SubId,
                            SubInt32Field = y.Int32Field,
                            SubInt32FieldNull = y.Int32FieldNull,
                            SubDecimalField = y.DecimalField,
                            SubDecimalFieldNull = y.DecimalFieldNull,
                            SubDateTimeField = y.DateTimeField,
                            SubDateTimeFieldNull = y.DateTimeFieldNull,
                            SubVarcharField = y.VarcharField,
                            SubVarcharFieldNull = y.VarcharFieldNull,
                        }).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.MainId > cnt).Skip(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Where((x, y) => x.MainId > cnt).Skip(cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).OrderByDescending(x => x.MainId).Skip(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .OrderByDescending((x, y) => x.MainId).Skip(cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.MainId > cnt).Take(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Where((x, y) => x.MainId > cnt).Take(cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).OrderByDescending(x => x.MainId).Take(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .OrderByDescending((x, y) => x.MainId).Take(cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).Where(x => x.MainId > cnt).Skip(cnt).Take(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .Where((x, y) => x.MainId > cnt).Skip(cnt).Take(cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = listMain.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                    MainId = x.MainId,
                    Int32Field = x.Int32Field,
                    Int32FieldNull = x.Int32FieldNull,
                    DecimalField = x.DecimalField,
                    DecimalFieldNull = x.DecimalFieldNull,
                    DateTimeField = x.DateTimeField,
                    DateTimeFieldNull = x.DateTimeFieldNull,
                    VarcharField = x.VarcharField,
                    VarcharFieldNull = x.VarcharFieldNull,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).OrderByDescending(x => x.MainId).Skip(cnt).Take(cnt).ToList();

                var listAc = context.Query<TeMainTable>().Join<TeSubTable>((x, y) => x.SubId == y.SubId)
                    .OrderByDescending((x, y) => x.MainId).Skip(cnt).Take(cnt)
                    .Select(
                    (x, y) => new {
                        MainId = x.MainId,
                        Int32Field = x.Int32Field,
                        Int32FieldNull = x.Int32FieldNull,
                        DecimalField = x.DecimalField,
                        DecimalFieldNull = x.DecimalFieldNull,
                        DateTimeField = x.DateTimeField,
                        DateTimeFieldNull = x.DateTimeFieldNull,
                        VarcharField = x.VarcharField,
                        VarcharFieldNull = x.VarcharFieldNull,
                        SubId = x.SubId,
                        SubInt32Field = y.Int32Field,
                        SubInt32FieldNull = y.Int32FieldNull,
                        SubDecimalField = y.DecimalField,
                        SubDecimalFieldNull = y.DecimalFieldNull,
                        SubDateTimeField = y.DateTimeField,
                        SubDateTimeFieldNull = y.DateTimeFieldNull,
                        SubVarcharField = y.VarcharField,
                        SubVarcharFieldNull = y.VarcharFieldNull,
                    }).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }
        #endregion

        [Fact]
        public void TestCase_JoinQuery_Distinct()
        {
            List<TeMainTable> listMain = CreateAndInsertMainTableList(20, 10);
            List<TeSubTable> listSub = CreateAndInsertSubTableList(10);

            for (int i = 5; i < listMain.Count; i++) {
                listMain[i].Int32Field = 10;
            }
            context.BatchUpdate(listMain);
            var list = listMain.Select(x => new {
                x.Int32Field,
                x.SubId
            }).Distinct().ToList();

            var listEx = list.Join(listSub, x => x.SubId, y => y.SubId, (x, y) => new {
                Int32Field = x.Int32Field,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.Int32Field).OrderBy(x => x.SubId).ToList();

            var listAc = context.Query<TeMainTable>().SetJoinSetting(JoinSetting.QueryDistinct)
                .Select(x => new {
                    x.Int32Field,
                    x.SubId
                }).Join<TeSubTable>(context.Query<TeSubTable>(), (x, y) => x.SubId == y.SubId).Select(
                (x, y) => new {
                    Int32Field = x.Int32Field,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToList().OrderBy(x => x.Int32Field).OrderBy(x => x.SubId).ToList();
            AssertExtend.StrictEqual(listEx, listAc);


            var listEx2 = listSub.Join(list, x => x.SubId, y => y.SubId, (y, x) => new {
                Int32Field = x.Int32Field,
                SubId = x.SubId,
                SubInt32Field = y.Int32Field,
                SubInt32FieldNull = y.Int32FieldNull,
                SubDecimalField = y.DecimalField,
                SubDecimalFieldNull = y.DecimalFieldNull,
                SubDateTimeField = y.DateTimeField,
                SubDateTimeFieldNull = y.DateTimeFieldNull,
                SubVarcharField = y.VarcharField,
                SubVarcharFieldNull = y.VarcharFieldNull,
            }).OrderBy(x => x.Int32Field).OrderBy(x => x.SubId).ToList();

            var listAc2 = context.Query<TeSubTable>()
                .Join(context.Query<TeMainTable>().Select(x => new {
                    x.Int32Field,
                    x.SubId
                }), (x, y) => x.SubId == y.SubId, JoinSetting.QueryDistinct).Select(
                (y, x) => new {
                    Int32Field = x.Int32Field,
                    SubId = x.SubId,
                    SubInt32Field = y.Int32Field,
                    SubInt32FieldNull = y.Int32FieldNull,
                    SubDecimalField = y.DecimalField,
                    SubDecimalFieldNull = y.DecimalFieldNull,
                    SubDateTimeField = y.DateTimeField,
                    SubDateTimeFieldNull = y.DateTimeFieldNull,
                    SubVarcharField = y.VarcharField,
                    SubVarcharFieldNull = y.VarcharFieldNull,
                }).ToList().OrderBy(x => x.Int32Field).OrderBy(x => x.SubId).ToList();
            AssertExtend.StrictEqual(listEx2, listAc2);
        }
    }
}
