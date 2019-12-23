using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Light.Data.Mysql.Test
{
    public class Mysql_RelateModeTest : BaseTest
    {
        public Mysql_RelateModeTest(ITestOutputHelper output) : base(output)
        {
        }

        #region base test

        protected List<TeRelateA> InitialRelateTableA(int count)
        {
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateA>();
            List<TeRelateA> list = new List<TeRelateA>();
            for (int i = 1; i <= count; i++)
            {
                TeRelateA item = new TeRelateA()
                {
                    Id = 1000 + i,
                    RelateBId = 2000 + i,
                    RelateCId = 3000 + i,
                    RelateDId = 4000 + i,
                    RelateEId = 5000 + i,
                    RelateFId = 6000 + i,
                    DecimalField = 0.01m * i,
                    VarcharField = "A" + i,
                    DateTimeField = DateTime.Now.Date.AddHours(i)
                };
                list.Add(item);
            }

            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        protected List<TeRelateCollection> InitialRelateTableCollection(int count)
        {
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateCollection>();
            List<TeRelateCollection> list = new List<TeRelateCollection>();
            for (int i = 1; i <= count; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    TeRelateCollection item = new TeRelateCollection()
                    {
                        RelateAId = 1000 + i,
                        VarcharField = "Collect" + j + "_" + i
                    };
                    list.Add(item);
                }
            }

            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        protected List<TeRelateB> InitialRelateTableB(int count)
        {
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateB>();
            List<TeRelateB> list = new List<TeRelateB>();
            for (int i = 1; i <= count; i++)
            {
                TeRelateB item = new TeRelateB()
                {
                    Id = 2000 + i,
                    RelateAId = 1000 + i,
                    RelateCId = 3000 + i,
                    RelateDId = 4000 + i,
                    RelateEId = 5000 + i,
                    RelateFId = 6000 + i,
                    DecimalField = 0.02m * i % 10,
                    VarcharField = "B" + i,
                    DateTimeField = DateTime.Now.Date.AddHours(i).AddMinutes(30)
                };
                list.Add(item);
            }

            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        protected List<TeRelateC> InitialRelateTableC(int count)
        {
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateC>();
            List<TeRelateC> list = new List<TeRelateC>();
            for (int i = 1; i <= count; i++)
            {
                TeRelateC item = new TeRelateC()
                {
                    Id = 3000 + i,
                    RelateAId = 1000 + i,
                    RelateBId = 2000 + i,
                    RelateDId = 4000 + i,
                    RelateEId = 5000 + i,
                    RelateFId = 6000 + i,
                    VarcharField = "C" + i
                };
                list.Add(item);
            }

            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        protected List<TeRelateD> InitialRelateTableD(int count)
        {
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateD>();
            List<TeRelateD> list = new List<TeRelateD>();
            for (int i = 1; i <= count; i++)
            {
                TeRelateD item = new TeRelateD()
                {
                    Id = 4000 + i,
                    RelateAId = 1000 + i,
                    RelateBId = 2000 + i,
                    RelateCId = 3000 + i,
                    RelateEId = 5000 + i,
                    RelateFId = 6000 + i,
                    VarcharField = "D" + i
                };
                list.Add(item);
            }

            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        protected List<TeRelateE> InitialRelateTableE(int count)
        {
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateE>();
            List<TeRelateE> list = new List<TeRelateE>();
            for (int i = 1; i <= count; i++)
            {
                TeRelateE item = new TeRelateE()
                {
                    Id = 5000 + i,
                    RelateAId = 1000 + i,
                    RelateBId = 2000 + i,
                    RelateCId = 3000 + i,
                    RelateDId = 4000 + i,
                    RelateFId = 6000 + i,
                    VarcharField = "E" + i
                };
                list.Add(item);
            }

            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        protected List<TeRelateF> InitialRelateTableF(int count)
        {
            commandOutput.Enable = false;
            context.TruncateTable<TeRelateF>();
            List<TeRelateF> list = new List<TeRelateF>();
            for (int i = 1; i <= count; i++)
            {
                TeRelateF item = new TeRelateF()
                {
                    Id = 6000 + i,
                    RelateAId = 1000 + i,
                    RelateBId = 2000 + i,
                    RelateCId = 3000 + i,
                    RelateDId = 4000 + i,
                    RelateEId = 5000 + i,
                    VarcharField = "F" + i
                };
                list.Add(item);
            }

            context.BatchInsert(list);
            commandOutput.Enable = true;
            return list;
        }

        [Fact]
        public void TestCase_Base()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(20);

            var listEx = (from x in listA
                join y in listB on x.Id equals y.RelateAId into ps
                from p in ps.DefaultIfEmpty()
                select new TeRelateA_B
                {
                    Id = x.Id,
                    RelateBId = x.RelateBId,
                    RelateCId = x.RelateCId,
                    RelateDId = x.RelateDId,
                    RelateEId = x.RelateEId,
                    RelateFId = x.RelateFId,
                    DecimalField = x.DecimalField,
                    VarcharField = x.VarcharField,
                    DateTimeField = x.DateTimeField,
                    RelateB = p
                }).ToList();
            var listAc = context.Query<TeRelateA_B>().ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Base_Null()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(10);

            var listEx = (from x in listA
                join y in listB on x.Id equals y.RelateAId into ps
                from p in ps.DefaultIfEmpty()
                select new TeRelateA_B
                {
                    Id = x.Id,
                    RelateBId = x.RelateBId,
                    RelateCId = x.RelateCId,
                    RelateDId = x.RelateDId,
                    RelateEId = x.RelateEId,
                    RelateFId = x.RelateFId,
                    DecimalField = x.DecimalField,
                    VarcharField = x.VarcharField,
                    DateTimeField = x.DateTimeField,
                    RelateB = p
                }).ToList();
            var listAc = context.Query<TeRelateA_B>().ToList();
            AssertExtend.StrictEqual(listEx, listAc);
            for (int i = 0; i < 10; i++)
            {
                Assert.NotNull(listAc[i].RelateB);
            }

            for (int i = 10; i < 20; i++)
            {
                Assert.Null(listAc[i].RelateB);
            }
        }

        [Fact]
        public void TestCase_Base_2Single()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(20);
            var listC = InitialRelateTableC(20);

            var listEx = (from x in listA
                join y in listB on x.Id equals y.RelateAId into ps
                from p in ps.DefaultIfEmpty()
                join z in listC on x.Id equals z.RelateAId into pd
                from q in pd.DefaultIfEmpty()
                select new TeRelateA_BC
                {
                    Id = x.Id,
                    RelateBId = x.RelateBId,
                    RelateCId = x.RelateCId,
                    RelateDId = x.RelateDId,
                    RelateEId = x.RelateEId,
                    RelateFId = x.RelateFId,
                    DecimalField = x.DecimalField,
                    VarcharField = x.VarcharField,
                    DateTimeField = x.DateTimeField,
                    RelateB = p,
                    RelateC = q
                }).ToList();
            var listAc = context.Query<TeRelateA_BC>().ToList();
            AssertExtend.StrictEqual(listEx, listAc);
        }

        [Fact]
        public void TestCase_Base_Where()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(18);
            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).Where(x => x.Id <= 1010).ToList();

                var listAc = context.Query<TeRelateA_B>().Where(x => x.Id <= 1010).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).Where(x => x.RelateB != null && x.RelateB.Id <= 2010).ToList();

                var listAc = context.Query<TeRelateA_B>().Where(x => x.RelateB.Id <= 2010).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).Where(x => x.Id > 1005 && x.RelateB != null && x.RelateB.Id <= 2010).ToList();

                var listAc = context.Query<TeRelateA_B>().Where(x => x.Id > 1005 && x.RelateB.Id <= 2010).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
                var listAc1 = context.Query<TeRelateA_B>().Where(x => x.Id > 1005)
                    .WhereWithAnd(x => x.RelateB.Id <= 2010).ToList();
                AssertExtend.StrictEqual(listEx, listAc1);
            }
        }

        [Fact]
        public void TestCase_Base_Order()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(20);
            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).OrderByDescending(x => x.Id).ToList();

                var listAc = context.Query<TeRelateA_B>().OrderByDescending(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).OrderByDescending(x => x.RelateB.Id).ToList();

                var listAc = context.Query<TeRelateA_B>().OrderByDescending(x => x.RelateB.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).OrderByDescending(x => x.Id).ToList();

                var listAc = context.Query<TeRelateA_B>().OrderBy(x => x.RelateB.DecimalField)
                    .OrderByDescending(x => x.Id).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        [Fact]
        public void TestCase_Base_Collection()
        {
            var listA = InitialRelateTableA(20);
            var listCollection = InitialRelateTableCollection(15);
            {
                var listAc = context.Query<TeRelateA_LCollection>().ToList();
                AssertExtend.Equal(listA, listAc);
                for (int i = 0; i < listAc.Count; i++)
                {
                    var item = listAc[i];
                    var collectionEx = listCollection.Where(x => x.RelateAId == item.Id).ToList();
                    var collectionAc = item.RelateCollection.ToList();
                    AssertExtend.StrictEqual(collectionEx, collectionAc);
                }
            }
            {
                var listAc = context.Query<TeRelateA_ICollection>().ToList();
                AssertExtend.Equal(listA, listAc);
                for (int i = 0; i < listAc.Count; i++)
                {
                    var item = listAc[i];
                    var collectionEx = listCollection.Where(x => x.RelateAId == item.Id).ToList();
                    var collectionAc = item.RelateCollection.ToList();
                    AssertExtend.StrictEqual(collectionEx, collectionAc);
                }
            }
            {
                var listAc = context.Query<TeRelateA_2Collection>().ToList();
                AssertExtend.Equal(listA, listAc);
                for (int i = 0; i < listAc.Count; i++)
                {
                    var item = listAc[i];
                    var collectionEx = listCollection.Where(x => x.RelateAId == item.Id).ToList();
                    var collectionLAc = item.RelateLCollection.ToList();
                    AssertExtend.StrictEqual(collectionEx, collectionLAc);
                    var collectionIAc = item.RelateICollection.ToList();
                    AssertExtend.StrictEqual(collectionEx, collectionIAc);
                }
            }
        }

        [Fact]
        public void TestCase_Base_Single_Collection()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(18);
            var listCollection = InitialRelateTableCollection(15);
            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).ToList();
                var listAc = context.Query<TeRelateA_B_Collection>().ToList();
                AssertExtend.Equal(listA, listAc);
                for (int i = 0; i < listAc.Count; i++)
                {
                    var item = listAc[i];
                    AssertExtend.Equal(listEx[i], item);
                    var collectionEx = listCollection.Where(x => x.RelateAId == item.Id).ToList();
                    var collectionAc = item.RelateCollection.ToList();
                    AssertExtend.StrictEqual(collectionEx, collectionAc);
                }
            }
        }

        [Fact]
        public void TestCase_Single_Relate1()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(20);

            var list = context.Query<TeRelateA_B_A>().ToList();
            Assert.Equal(listA.Count, list.Count);
            int i = 0;
            foreach (var item in list)
            {
                AssertExtend.Equal(listA[i], item);
                AssertExtend.Equal(listB[i], item.RelateB);
                Assert.Equal(item, item.RelateB.RelateA);
                i++;
            }
        }

        [Fact]
        public void TestCase_Base_Select()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(20);
            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateBV = p.VarcharField
                    }).ToList();

                var listAc = context.Query<TeRelateA_B>()
                    .Select(x => new
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateBV = x.RelateB.VarcharField
                    })
                    .ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
            
            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateBV = p.VarcharField,
                        RelateBD = p.DateTimeField
                    }).ToList();

                var listAc = context.Query<TeRelateA_B>()
                    .Select(x => new
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateBV = x.RelateB.VarcharField,
                        RelateBD = x.RelateB.DateTimeField
                    })
                    .ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
            
            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateBV = p.VarcharField,
                        RelateBD = p.DateTimeField,
                        RelateBM = p.DecimalField
                    }).ToList();

                var listAc = context.Query<TeRelateA_B>()
                    .Select(x => new
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateBV = x.RelateB.VarcharField,
                        RelateBD = x.RelateB.DateTimeField,
                        RelateBM = x.RelateB.DecimalField,
                    })
                    .ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        [Fact]
        public void TestCase_Single_Relate2()
        {
            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(20);
            var listC = InitialRelateTableC(20);
            var listD = InitialRelateTableD(20);
            var listE = InitialRelateTableE(20);

            var list = context.Query<TeRelateA_BX>().ToList();
            Assert.Equal(listA.Count, list.Count);
            int i = 0;
            foreach (var item in list)
            {
                AssertExtend.Equal(listA[i], item);
                AssertExtend.Equal(listB[i], item.RelateB);
                AssertExtend.Equal(listC[i], item.RelateB.RelateC);
                AssertExtend.Equal(listD[i], item.RelateB.RelateD);
                AssertExtend.Equal(listE[i], item.RelateB.RelateE);
                AssertExtend.Equal(listE[i], item.RelateE);
                Assert.Equal(item, item.RelateE.RelateA);
                Assert.NotEqual(item.RelateE, item.RelateB.RelateE);
                Assert.Equal(item, item.RelateB.RelateC.RelateA);
                i++;
            }
        }


        [Fact]
        public void TestCase_Base_PageSize()
        {
            const int tol = 21;
            const int cnt = 8;

            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(20);

            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++)
            {
                {
                    var listEx = (from x in listA
                        join y in listB on x.Id equals y.RelateAId into ps
                        from p in ps.DefaultIfEmpty()
                        select new TeRelateA_B
                        {
                            Id = x.Id,
                            RelateBId = x.RelateBId,
                            RelateCId = x.RelateCId,
                            RelateDId = x.RelateDId,
                            RelateEId = x.RelateEId,
                            RelateFId = x.RelateFId,
                            DecimalField = x.DecimalField,
                            VarcharField = x.VarcharField,
                            DateTimeField = x.DateTimeField,
                            RelateB = p
                        }).Skip(cnt * i).Take(cnt).ToList();
                    var listAc = context.Query<TeRelateA_B>().PageSize(i + 1, cnt).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }
                {
                    var listEx = (from x in listA
                        join y in listB on x.Id equals y.RelateAId into ps
                        from p in ps.DefaultIfEmpty()
                        select new TeRelateA_B
                        {
                            Id = x.Id,
                            RelateBId = x.RelateBId,
                            RelateCId = x.RelateCId,
                            RelateDId = x.RelateDId,
                            RelateEId = x.RelateEId,
                            RelateFId = x.RelateFId,
                            DecimalField = x.DecimalField,
                            VarcharField = x.VarcharField,
                            DateTimeField = x.DateTimeField,
                            RelateB = p
                        }).Skip(cnt * i).Take(cnt).ToList();
                    var listAc = context.Query<TeRelateA_B>().Range(i * cnt, (i + 1) * cnt).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).Where(x => x.Id > cnt).Take(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().Where(x => x.Id > cnt).PageSize(1, cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).OrderByDescending(x => x.Id).Take(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().OrderByDescending(x => x.Id).PageSize(1, cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).Where(x => x.Id > cnt).Take(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().Where(x => x.Id > cnt).Range(0, cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).OrderByDescending(x => x.Id).Take(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().OrderByDescending(x => x.Id).Range(0, cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        [Fact]
        public void TestCase_Base_PageSkipTake()
        {
            const int tol = 21;
            const int cnt = 8;

            var listA = InitialRelateTableA(20);
            var listB = InitialRelateTableB(20);

            int times = tol / cnt;
            times++;

            for (int i = 0; i < times; i++)
            {
                {
                    var listEx = (from x in listA
                        join y in listB on x.Id equals y.RelateAId into ps
                        from p in ps.DefaultIfEmpty()
                        select new TeRelateA_B
                        {
                            Id = x.Id,
                            RelateBId = x.RelateBId,
                            RelateCId = x.RelateCId,
                            RelateDId = x.RelateDId,
                            RelateEId = x.RelateEId,
                            RelateFId = x.RelateFId,
                            DecimalField = x.DecimalField,
                            VarcharField = x.VarcharField,
                            DateTimeField = x.DateTimeField,
                            RelateB = p
                        }).Skip(cnt * i).ToList();
                    var listAc = context.Query<TeRelateA_B>().Skip(cnt * i).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }
                {
                    var listEx = (from x in listA
                        join y in listB on x.Id equals y.RelateAId into ps
                        from p in ps.DefaultIfEmpty()
                        select new TeRelateA_B
                        {
                            Id = x.Id,
                            RelateBId = x.RelateBId,
                            RelateCId = x.RelateCId,
                            RelateDId = x.RelateDId,
                            RelateEId = x.RelateEId,
                            RelateFId = x.RelateFId,
                            DecimalField = x.DecimalField,
                            VarcharField = x.VarcharField,
                            DateTimeField = x.DateTimeField,
                            RelateB = p
                        }).Take(cnt * i).ToList();
                    var listAc = context.Query<TeRelateA_B>().Take(cnt * i).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }
                {
                    var listEx = (from x in listA
                        join y in listB on x.Id equals y.RelateAId into ps
                        from p in ps.DefaultIfEmpty()
                        select new TeRelateA_B
                        {
                            Id = x.Id,
                            RelateBId = x.RelateBId,
                            RelateCId = x.RelateCId,
                            RelateDId = x.RelateDId,
                            RelateEId = x.RelateEId,
                            RelateFId = x.RelateFId,
                            DecimalField = x.DecimalField,
                            VarcharField = x.VarcharField,
                            DateTimeField = x.DateTimeField,
                            RelateB = p
                        }).Skip(cnt * i).Take(cnt).ToList();
                    var listAc = context.Query<TeRelateA_B>().Skip(cnt * i).Take(cnt).ToList();
                    AssertExtend.StrictEqual(listEx, listAc);
                }
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).Where(x => x.Id > cnt).Take(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().Where(x => x.Id > cnt).Take(cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).OrderByDescending(x => x.Id).Take(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().OrderByDescending(x => x.Id).Take(cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).Where(x => x.Id > cnt).Skip(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().Where(x => x.Id > cnt).Where(x => x.Id > cnt).Skip(cnt)
                    .ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).OrderByDescending(x => x.Id).Skip(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().OrderByDescending(x => x.Id).Skip(cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).Where(x => x.Id > cnt).Skip(cnt).Take(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().Where(x => x.Id > cnt).Where(x => x.Id > cnt).Skip(cnt)
                    .Take(cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }

            {
                var listEx = (from x in listA
                    join y in listB on x.Id equals y.RelateAId into ps
                    from p in ps.DefaultIfEmpty()
                    select new TeRelateA_B
                    {
                        Id = x.Id,
                        RelateBId = x.RelateBId,
                        RelateCId = x.RelateCId,
                        RelateDId = x.RelateDId,
                        RelateEId = x.RelateEId,
                        RelateFId = x.RelateFId,
                        DecimalField = x.DecimalField,
                        VarcharField = x.VarcharField,
                        DateTimeField = x.DateTimeField,
                        RelateB = p
                    }).OrderByDescending(x => x.Id).Skip(cnt).Take(cnt).ToList();
                var listAc = context.Query<TeRelateA_B>().OrderByDescending(x => x.Id).Skip(cnt).Take(cnt).ToList();
                AssertExtend.StrictEqual(listEx, listAc);
            }
        }

        #endregion
    }
}