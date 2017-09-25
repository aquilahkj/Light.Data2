using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
            var builder = new DataContextOptionsBuilder<DataContext>();
            builder.UseMssql("Data Source=192.168.210.130;User ID=sa;Password=qwerty;Initial Catalog=LightData_Test;");
            builder.SetCommandOutput(commandOutput);
            var options = builder.Build();
            DataContext context = new DataContext(options);
            return context;
        }

        public DataContext CreateBuilderContextByConfig()
        {
            var builder = new DataContextOptionsConfigurator<DataContext>();
            builder.ConfigName = "mssql";
            builder.SetCommandOutput(commandOutput);
            var options = builder.Create(DataContextConfiguration.Global);
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


        public TestContext CreateBuilderContextByDiConfigSpecified()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(configuration.GetSection("lightData"), config => {
                config.ConfigName = "app";
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContext context = provider.GetRequiredService<TestContext>();
            return context;
        }

        public TestContext CreateBuilderContextByDiConfigSpecifiedDefault()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(configuration.GetSection("lightData"), config => {
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContext context = provider.GetRequiredService<TestContext>();
            return context;
        }

        public TestContext CreateBuilderContextByDiConfigGlobal()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(DataContextConfiguration.Global, config => {
                config.ConfigName = "mssql";
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContext context = provider.GetRequiredService<TestContext>();
            return context;
        }

        public TestContext CreateBuilderContextByDiConfigGlobalDefault()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(DataContextConfiguration.Global, config => {
                config.SetCommandOutput(commandOutput);
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
