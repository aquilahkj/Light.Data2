using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Light.Data.Mysql.Test
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
            context = new DataContext("mysql");
            commandOutput.OutputFullCommand = true;
            commandOutput.OnCommandOutput += CommandOutput_OnCommandOutput;
            //output.UseConsoleOutput = true;
            context.SetCommandOutput(commandOutput);
            this.output = output;

        }

        public DataContext CreateBuilderContextByConnection()
        {
            var builder = new DataContextOptionsBuilder<DataContext>();
            builder.UseMysql("server=mysql_test;Port=3306;User Id=root;password=1qazxsw23edC;database=LightData_Test;Allow Zero Datetime=True;charset=utf8;Min Pool Size=1;Max Pool Size=5;SslMode=None;");
            builder.SetCommandOutput(commandOutput);
            var options = builder.Build();
            DataContext context = new DataContext(options);
            return context;
        }

        public DataContext CreateBuilderContextByConfig()
        {
            var builder = new DataContextOptionsConfigurator<DataContext>();
            builder.ConfigName = "mysql";
            builder.SetCommandOutput(commandOutput);
            var options = builder.Create(DataContextConfiguration.Global);
            DataContext context = new DataContext(options);
            return context;
        }

        public TestContext CreateBuilderContextByDi()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(builder =>
            {
                builder.UseMysql("server=mysql_test;Port=3306;User Id=root;password=1qazxsw23edC;database=LightData_Test;Allow Zero Datetime=True;charset=utf8;Min Pool Size=1;Max Pool Size=5;SslMode=None;");
                builder.SetCommandOutput(commandOutput);
                builder.SetTimeout(2000);
                builder.SetBatchInsertCount(10);
                builder.SetBatchUpdateCount(10);
                builder.SetBatchDeleteCount(10);
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
            service.AddDataContext<TestContext>(configuration.GetSection("lightData"), config =>
            {
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
            service.AddDataContext<TestContext>(configuration.GetSection("lightData"), config =>
            {
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContext context = provider.GetRequiredService<TestContext>();
            return context;
        }

        public TestContext CreateBuilderContextByDiConfigGlobal()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(DataContextConfiguration.Global, config =>
            {
                config.ConfigName = "mysql";
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContext context = provider.GetRequiredService<TestContext>();
            return context;
        }

        public TestContext CreateBuilderContextByDiConfigGlobalDefault()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>(DataContextConfiguration.Global, config =>
            {
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContext context = provider.GetRequiredService<TestContext>();
            return context;
        }

        public TestContext CreateBuilderContextByConfigFile()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContext<TestContext>("lightdata2.json", config =>
            {
                config.ConfigName = "mysql2";
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContext context = provider.GetRequiredService<TestContext>();
            return context;
        }

        public DataContext CreateContext()
        {
            DataContext context = new DataContext("mysql");
            context.SetCommandOutput(commandOutput);
            return context;
        }

        public DataContext CreateContext(string configName)
        {
            DataContext context = new DataContext(configName);
            context.SetCommandOutput(commandOutput);
            return context;
        }

        public DataContext CreateBuilderContextFactoryByConnection()
        {
            var builder = new DataContextOptionsBuilder<DataContext>();
            builder.UseMysql("server=mysql_test;Port=3306;User Id=root;password=1qazxsw23edC;database=LightData_Test;Allow Zero Datetime=True;charset=utf8;Min Pool Size=1;Max Pool Size=5;SslMode=None;");
            builder.SetCommandOutput(commandOutput);
            var options = builder.Build();
            LightDataContextFactory factory = new LightDataContextFactory(options);
            DataContext context = factory.CreateDataContext();
            return context;
        }

        public DataContext CreateBuilderContextFactoryByConfig()
        {
            var builder = new DataContextOptionsConfigurator<DataContext>();
            builder.ConfigName = "mysql";
            builder.SetCommandOutput(commandOutput);
            var options = builder.Create(DataContextConfiguration.Global);
            LightDataContextFactory factory = new LightDataContextFactory(options);
            DataContext context = factory.CreateDataContext();
            return context;
        }

        public TestContext CreateBuilderContextFactoryByDi()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContextFactory<TestContextFactory, TestContext>(builder =>
            {
                builder.UseMysql("server=mysql_test;Port=3306;User Id=root;password=1qazxsw23edC;database=LightData_Test;Allow Zero Datetime=True;charset=utf8;Min Pool Size=1;Max Pool Size=5;SslMode=None;");
                builder.SetCommandOutput(commandOutput);
                builder.SetTimeout(2000);
                builder.SetBatchInsertCount(10);
                builder.SetBatchUpdateCount(10);
                builder.SetBatchDeleteCount(10);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContextFactory factory = provider.GetRequiredService<TestContextFactory>();
            TestContext context = factory.CreateDataContext();
            return context;
        }

        public TestContext CreateBuilderContextFactoryByDiConfigSpecified()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            IServiceCollection service = new ServiceCollection();
            service.AddDataContextFactory<TestContextFactory, TestContext>(configuration.GetSection("lightData"), config =>
            {
                config.ConfigName = "app";
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContextFactory factory = provider.GetRequiredService<TestContextFactory>();
            TestContext context = factory.CreateDataContext();
            return context;
        }

        public TestContext CreateBuilderContextFactoryByDiConfigSpecifiedDefault()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            IServiceCollection service = new ServiceCollection();
            service.AddDataContextFactory<TestContextFactory, TestContext>(configuration.GetSection("lightData"), config =>
            {
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContextFactory factory = provider.GetRequiredService<TestContextFactory>();
            TestContext context = factory.CreateDataContext();
            return context;
        }

        public TestContext CreateBuilderContextFactoryByDiConfigGlobal()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContextFactory<TestContextFactory, TestContext>(DataContextConfiguration.Global, config =>
            {
                config.ConfigName = "mysql";
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContextFactory contextFactory = provider.GetRequiredService<TestContextFactory>();
            TestContext context = contextFactory.CreateDataContext();
            return context;
        }

        public TestContext CreateBuilderContextFactoryByDiConfigGlobalDefault()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContextFactory<TestContextFactory, TestContext>(DataContextConfiguration.Global, config =>
            {
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContextFactory contextFactory = provider.GetRequiredService<TestContextFactory>();
            TestContext context = contextFactory.CreateDataContext();
            return context;
        }

        public TestContext CreateBuilderContextFactoryByConfigFile()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddDataContextFactory<TestContextFactory, TestContext>("lightdata2.json", config =>
            {
                config.ConfigName = "mysql2";
                config.SetCommandOutput(commandOutput);
            }, ServiceLifetime.Transient);
            var provider = service.BuildServiceProvider();
            TestContextFactory contextFactory = provider.GetRequiredService<TestContextFactory>();
            TestContext context = contextFactory.CreateDataContext();
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

    public class TestContextFactory : DataContextFactory<TestContext>
    {
        public TestContextFactory(DataContextOptions<TestContext> options) : base(options)
        {

        }

        public override TestContext CreateDataContext()
        {
            return new TestContext(options);
        }
    }
}
