using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Light.Data;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LightDataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, Action<DataContextOptionsBuilder<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            return AddDataContext<DataContext>(serviceCollection, optionsAction, contextLifetime);
        }

        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, Action<DataContextOptionsBuilder<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            var builder = new DataContextOptionsBuilder<TContext>();
            optionsAction?.Invoke(builder);
            var options = builder.Build();
            serviceCollection.AddSingleton(options);
            if (contextLifetime == ServiceLifetime.Transient) {
                serviceCollection.AddTransient<TContext>();
            } else if (contextLifetime == ServiceLifetime.Singleton) {
                serviceCollection.AddSingleton<TContext>();
            } else {
                serviceCollection.AddScoped<TContext>();
            }
            return serviceCollection;
        }

        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, IConfiguration config, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            return AddDataContext<DataContext>(serviceCollection, config, optionsAction, contextLifetime);
        }

        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, IConfiguration config, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            if (config == null) {
                throw new ArgumentNullException(nameof(config));
            }
            var configOptions = config.Get<LightDataOptions>();
            var configuration = new DataContextConfiguration(configOptions);
            return AddDataContext(serviceCollection, configuration, optionsAction, contextLifetime);
        }

        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, DataContextConfiguration configuration, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            return AddDataContext<DataContext>(serviceCollection, configuration, optionsAction, contextLifetime);
        }

        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, DataContextConfiguration configuration, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            var configurator = new DataContextOptionsConfigurator<TContext>();
            optionsAction?.Invoke(configurator);
            var options = configurator.Create(configuration);
            serviceCollection.AddSingleton(options);
            if (contextLifetime == ServiceLifetime.Transient) {
                serviceCollection.AddTransient<TContext>();
            } else if (contextLifetime == ServiceLifetime.Singleton) {
                serviceCollection.AddSingleton<TContext>();
            } else {
                serviceCollection.AddScoped<TContext>();
            }
            return serviceCollection;
        }

        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, string configFilePath, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            DataContextConfiguration configuration = new DataContextConfiguration(configFilePath);
            return AddDataContext<DataContext>(serviceCollection, configFilePath, optionsAction, contextLifetime);
        }

        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, string configFilePath, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            DataContextConfiguration configuration = new DataContextConfiguration(configFilePath);
            return AddDataContext(serviceCollection, configuration, optionsAction, contextLifetime);
        }
    }
}
