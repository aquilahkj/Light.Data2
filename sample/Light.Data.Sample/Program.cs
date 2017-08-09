using System;
using Light.Data.Mssql;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Data.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Test0();
        }

        static void Test0()
        {
            CommandOutput output = new CommandOutput();
            output.UseConsoleOutput = true;
            output.OutputFullCommand = true;


            DataContext context = new DataContext("test");
            context.SetCommanfOutput(output);
            var list = context.Query<TeUser>().ToList();

            foreach (var item in list) {
                Console.WriteLine(item);
            }
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
    }
}