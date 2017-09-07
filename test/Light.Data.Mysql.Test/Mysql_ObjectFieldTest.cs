using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace Light.Data.Mysql.Test
{
    public class Mysql_ObjectFieldTest : BaseTest
    {
        public Mysql_ObjectFieldTest(ITestOutputHelper output) : base(output)
        {
        }

        #region base test
        List<TeObjectField> CreateObjectFieldTableList(int count)
        {
            List<TeObjectField> list = new List<TeObjectField>();
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            for (int i = 1; i <= count; i++) {
                int x = i % 5 == 0 ? -1 : 1;
                TeObjectField item = new TeObjectField();

                item.VarcharField = "testtest" + i;
                item.ObjectField = new ObjectType() {
                    DataInt = i,
                    DataDate = d.AddMinutes(i * 2),
                    DataString = "datastring" + i
                };
                item.ObjectFieldNull = i % 2 == 0 ? null : new ObjectType() {
                    DataInt = i,
                    DataDate = d.AddMinutes(i * 2),
                    DataString = "datastring" + i
                };
                list.Add(item);
            }
            return list;
        }

        List<TeObjectField> CreateAndInsertObjectFieldTableList(int count)
        {
            var list = CreateObjectFieldTableList(count);
            commandOutput.Enable = false;
            context.TruncateTable<TeObjectField>();
            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }


        #endregion

        [Fact]
        public void TestCase_ObjectField_Query()
        {
            List<TeObjectField> list = CreateAndInsertObjectFieldTableList(45);

            List<TeObjectField> listEx = list;
            List<TeObjectField> listAc = context.Query<TeObjectField>().ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }

        [Fact]
        public void TestCase_ObjectField_Query_Where()
        {
            List<TeObjectField> list = CreateAndInsertObjectFieldTableList(45);
            TeObjectField item = list[2];
            List<TeObjectField> listEx = list.Where(x => x.ObjectField == item.ObjectField).ToList();
            List<TeObjectField> listAc = context.Query<TeObjectField>().Where(x => x.ObjectField == item.ObjectField).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }

        [Fact]
        public void TestCase_ObjectField_Query_Null()
        {
            List<TeObjectField> list = CreateAndInsertObjectFieldTableList(45);

            List<TeObjectField> listEx = list.Where(x => x.ObjectFieldNull == null).ToList();
            List<TeObjectField> listAc = context.Query<TeObjectField>().Where(x => x.ObjectFieldNull == null).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }

        [Fact]
        public void TestCase_ObjectField_Aggregate()
        {
            List<TeObjectField> list = CreateAndInsertObjectFieldTableList(45);

            var temlist = list.Take(5).ToList();

            list.AddRange(temlist);
            context.BatchInsert(temlist);
            var listEx = list.GroupBy(x => x.ObjectField).Select(g => new {
                ObjectField = g.Key,
                Count = g.Count()
            }).ToList().OrderBy(x => x.ObjectField.DataInt).ToList();
            var listAc = context.Query<TeObjectField>().GroupBy(x => new {
                ObjectField = x.ObjectField,
                Count = Function.Count()
            }).ToList().OrderBy(x => x.ObjectField.DataInt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }

        [Fact]
        public void TestCase_ObjectField_Join()
        {
            List<TeObjectField> list = CreateAndInsertObjectFieldTableList(45);

            var temlist = list.Where(x => x.Id <= 5).ToList();

            list.AddRange(temlist);
            var sublist = temlist.Select(x => new {
                x.Id,
                x.ObjectField,
                x.ObjectFieldNull
            }).ToList();

            context.BatchInsert(temlist);
            var listEx = list.GroupBy(x => x.ObjectField).Select(g => new {
                ObjectField = g.Key,
                Count = g.Count()
            }).ToList().OrderBy(x => x.ObjectField.DataInt).ToList()
            .Join(sublist, x => x.ObjectField, y => y.ObjectField, (x, y) => new {
                x.ObjectField,
                x.Count,
                y.Id,
                y.ObjectFieldNull
            }).ToList();
            var ft = context.Query<TeObjectField>().Where(x => x.Id <= 5).Select(x => new {
                x.Id,
                x.ObjectField,
                x.ObjectFieldNull
            });

            var listAc = context.Query<TeObjectField>().GroupBy(x => new {
                ObjectField = x.ObjectField,
                Count = Function.Count()
            })
            .Join(ft, (x, y) => x.ObjectField == y.ObjectField)
            .Select((x, y) => new {
                x.ObjectField,
                x.Count,
                y.Id,
                y.ObjectFieldNull
            })
            .ToList().OrderBy(x => x.ObjectField.DataInt).ToList();
            AssertExtend.StrictEqual(listEx, listAc);

        }
    }
}
