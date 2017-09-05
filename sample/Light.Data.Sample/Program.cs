using System;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Data.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            DataMapperConfiguration.AddConfigFilePath("lightdata.json");
            DataMapperConfiguration.AddConfigFilePath("mapper_relate.json");
            Test0();
        }

        static void Test0()
        {
            CommandOutput output = new CommandOutput();
            output.Enable = true;
            output.UseConsoleOutput = true;
            output.OutputFullCommand = true;


            DataContext context = new DataContext("test");
            context.SetCommandOutput(output);
            var list = context.Query<TeUser>().Where(x => x.Id == 10).ToList();

            //foreach (var item in list) {
            //    Console.WriteLine(item);
            //}
            Console.Read();
        }

        static void Test1()
        {
            CommandOutput output = new CommandOutput();
            output.UseConsoleOutput = true;
            output.OutputFullCommand = true;

            DataContextOptionsBuilder builder = new DataContextOptionsBuilder();
            builder.UseMssql("Data Source=192.168.210.130;User ID=sa;Password=qwerty;Initial Catalog=CM_TEST;");
            builder.SetCommandOutput(output);
            var options = builder.Build();

            DataContext context = new DataContext(options);

            var list = context.Query<TeUser>().ToList();

            foreach (var item in list) {
                Console.WriteLine(item);
            }
            Console.Read();
        }

        static void Test2()
        {
            CommandOutput output = new CommandOutput();
            output.UseConsoleOutput = true;
            output.OutputFullCommand = true;

            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(builder => {
                builder.UseMssql("Data Source=192.168.210.130;User ID=sa;Password=qwerty;Initial Catalog=CM_TEST;");
                builder.SetCommandOutput(output);
                builder.SetTimeout(2000);
                builder.SetVersion("11.0");
            }, ServiceLifetime.Transient);


            var provider = service.BuildServiceProvider();
            var context = provider.GetRequiredService<TestContext>();

            var context2 = provider.GetRequiredService<TestContext>();
            if (Object.Equals(context, context2)) {
                throw new Exception("error");
            }
            var list = context.Query<TeUser>().ToList();

            foreach (var item in list) {
                Console.WriteLine(item);
            }
            Console.Read();
        }

        static void Test3()
        {
            CommandOutput output = new CommandOutput();
            output.UseConsoleOutput = true;
            output.OutputFullCommand = true;


            DataContext context = new DataContext("test");
            context.SetCommandOutput(output);
            TeTagInfo taginfo = new TeTagInfo() {
                GroupCode = "01",
                TagCode = "02",
                TagName = "aa",
                Status = 1,
                Remark = new TeUserLevel() {
                    Id = 1,
                    LevelName = "2",
                    Status = 1,
                }
            };
            TeTagInfo taginfo1 = new TeTagInfo() {
                GroupCode = "01",
                TagCode = "03",
                TagName = "aa",
                Status = 2,
                Remark = null
            };

            context.TruncateTable<TeTagInfo>();

            context.Insert(taginfo);
            context.Insert(taginfo1);
            var list = context.Query<TeTagInfo>().Where(x => x.Remark == taginfo.Remark).ToList();

            foreach (var item in list) {
                Console.WriteLine(item);
            }

            var agg = context.Query<TeTagInfo>().GroupBy(x => new {
                Remark = x.Remark,
                Count = Function.Count()
            }).ToList();

            var join = context.Query<TeTagInfo>().Select(x => new {
                x.Remark,
                x.Status
            }).LeftJoin<TeUser>((x, y) => x.Status == y.Id).Select((x, y) => new {
                x.Remark,
                y.Id,
                y.LevelId,
                y.NickName
            }).ToList();

            Console.Read();
        }

        static void Test4()
        {
            CommandOutput output = new CommandOutput();
            output.UseConsoleOutput = true;
            output.OutputFullCommand = true;


            DataContext context = new DataContext("test_extend");
            context.SetCommandOutput(output);

            var list = context.Query<TeRelateMainExtendConfig>().ToList();
            Console.WriteLine(list.Count);
        }

    }
}