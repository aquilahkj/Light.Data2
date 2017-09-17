using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Data.Test
{
    public abstract class BaseTest
    {
        static BaseTest()
        {

            DataMapperConfiguration.AddConfigFilePath("Config/mapper.json");
            DataMapperConfiguration.AddConfigFilePath("Config/mapper_relate.json");
        }

        readonly protected DataContext context = null;

        readonly protected CommandOutput commandOutput = new CommandOutput();

        public const double DELTA = 0.0001;

        protected readonly ITestOutputHelper output;

        protected BaseTest(ITestOutputHelper output)
        {
            context = new DataContext("mssql");
            commandOutput.OutputFullCommand = true;
            commandOutput.OnCommandOutput += CommandOutput_OnCommandOutput;
            //output.UseConsoleOutput = true;
            context.SetCommandOutput(commandOutput);
            this.output = output;

        }

        public DataContext CreateBuilderContextByConnection()
        {
            DataContextOptionsBuilder builder = new DataContextOptionsBuilder();
            builder.UseMssql("Data Source=192.168.210.130;User ID=sa;Password=qwerty;Initial Catalog=LightData_Test;");
            builder.SetCommandOutput(commandOutput);
            var options = builder.Build();
            DataContext context = new DataContext(options);
            return context;
        }

        public DataContext CreateBuilderContextByConfig()
        {
            DataContextOptionsBuilder builder = new DataContextOptionsBuilder();
            builder.ConfigName = "mssql";
            builder.SetCommandOutput(commandOutput);
            var options = builder.Build();
            DataContext context = new DataContext(options);
            return context;
        }

        public TestContext CreateBuilderContextByDi()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(builder => {
                builder.UseMssql("Data Source=192.168.210.130;User ID=sa;Password=qwerty;Initial Catalog=LightData_Test;");
                builder.SetCommandOutput(commandOutput);
                builder.SetTimeout(2000);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContext context = provider.GetRequiredService<TestContext>();
            return context;
        }

        public DataContext CreateContext()
        {
            DataContext context = new DataContext("mssql");
            context.SetCommandOutput(commandOutput);
            return context;
        }

        public DataContext CreateContext(string configName)
        {
            DataContext context = new DataContext(configName);
            context.SetCommandOutput(commandOutput);
            return context;
        }

        private void CommandOutput_OnCommandOutput(object sender, CommandOutputEventArgs args)
        {
            this.output.WriteLine(args.RunnableCommand);
        }

        protected DateTime GetNow()
        {
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            return d;
        }
    }

    public class TestContext : DataContext
    {
        public TestContext(DataContextOptions<TestContext> options) : base(options)
        {

        }
    }
}
