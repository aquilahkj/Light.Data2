using System;
using Light.Data;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service Extension
    /// </summary>
    public static class LightDataServiceCollectionExtensions
    {
        /// <summary>
        /// Add DataContext Service
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, Action<DataContextOptionsBuilder<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            return AddDataContext<DataContext>(serviceCollection, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Service
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, Action<DataContextOptionsBuilder<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            var builder = new DataContextOptionsBuilder<TContext>();
            optionsAction?.Invoke(builder);
            var options = builder.Build();
            serviceCollection.AddSingleton(options);
            if (contextLifetime == ServiceLifetime.Transient) {
                serviceCollection.AddTransient<TContext>();
            }
            else if (contextLifetime == ServiceLifetime.Singleton) {
                serviceCollection.AddSingleton<TContext>();
            }
            else {
                serviceCollection.AddScoped<TContext>();
            }
            return serviceCollection;
        }
        /// <summary>
        /// Add DataContext Service
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="config"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, IConfiguration config, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            return AddDataContext<DataContext>(serviceCollection, config, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Service
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="config"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, IConfiguration config, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            if (config == null) {
                throw new ArgumentNullException(nameof(config));
            }
            var configOptions = config.Get<LightDataOptions>();
            var configuration = new DataContextConfiguration(configOptions);
            return AddDataContext(serviceCollection, configuration, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Service
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, DataContextConfiguration configuration, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            return AddDataContext<DataContext>(serviceCollection, configuration, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Service
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, DataContextConfiguration configuration, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            var configurator = new DataContextOptionsConfigurator<TContext>();
            optionsAction?.Invoke(configurator);
            var options = configurator.Create(configuration);
            serviceCollection.AddSingleton(options);
            if (contextLifetime == ServiceLifetime.Transient) {
                serviceCollection.AddTransient<TContext>();
            }
            else if (contextLifetime == ServiceLifetime.Singleton) {
                serviceCollection.AddSingleton<TContext>();
            }
            else {
                serviceCollection.AddScoped<TContext>();
            }
            return serviceCollection;
        }
        /// <summary>
        /// Add DataContext Service
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configFilePath"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, string configFilePath, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            var configuration = new DataContextConfiguration(configFilePath);
            return AddDataContext<DataContext>(serviceCollection, configFilePath, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Service
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="configFilePath"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, string configFilePath, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            var configuration = new DataContextConfiguration(configFilePath);
            return AddDataContext(serviceCollection, configuration, optionsAction, contextLifetime);
        }

        /// <summary>
        /// Add DataContext Factory Service
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContextFactory(this IServiceCollection serviceCollection, Action<DataContextOptionsBuilder<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Singleton)
        {
            return AddDataContextFactory<LightDataContextFactory, DataContext>(serviceCollection, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Factory Service
        /// </summary>
        /// <typeparam name="TContextFactory"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContextFactory<TContextFactory, TContext>(this IServiceCollection serviceCollection, Action<DataContextOptionsBuilder<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Singleton) where TContextFactory : DataContextFactory<TContext> where TContext : DataContext
        {
            var builder = new DataContextOptionsBuilder<TContext>();
            optionsAction?.Invoke(builder);
            var options = builder.Build();
            serviceCollection.AddSingleton(options);
            if (contextLifetime == ServiceLifetime.Transient) {
                serviceCollection.AddTransient<TContextFactory>();
            }
            else if (contextLifetime == ServiceLifetime.Singleton) {
                serviceCollection.AddSingleton<TContextFactory>();
            }
            else {
                serviceCollection.AddScoped<TContextFactory>();
            }
            return serviceCollection;
        }
        /// <summary>
        /// Add DataContext Factory Service
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="config"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContextFactory(this IServiceCollection serviceCollection, IConfiguration config, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Singleton)
        {
            return AddDataContextFactory<LightDataContextFactory, DataContext>(serviceCollection, config, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Factory Service
        /// </summary>
        /// <typeparam name="TContextFactory"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="config"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContextFactory<TContextFactory, TContext>(this IServiceCollection serviceCollection, IConfiguration config, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Singleton) where TContextFactory : DataContextFactory<TContext> where TContext : DataContext
        {
            if (config == null) {
                throw new ArgumentNullException(nameof(config));
            }
            var configOptions = config.Get<LightDataOptions>();
            var configuration = new DataContextConfiguration(configOptions);
            return AddDataContextFactory<TContextFactory, TContext>(serviceCollection, configuration, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Factory Service
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContextFactory(this IServiceCollection serviceCollection, DataContextConfiguration configuration, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Singleton)
        {
            return AddDataContextFactory<LightDataContextFactory, DataContext>(serviceCollection, configuration, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Factory Service
        /// </summary>
        /// <typeparam name="TContextFactory"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContextFactory<TContextFactory, TContext>(this IServiceCollection serviceCollection, DataContextConfiguration configuration, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Singleton) where TContextFactory : DataContextFactory<TContext> where TContext : DataContext
        {
            var configurator = new DataContextOptionsConfigurator<TContext>();
            optionsAction?.Invoke(configurator);
            var options = configurator.Create(configuration);
            serviceCollection.AddSingleton(options);
            if (contextLifetime == ServiceLifetime.Transient) {
                serviceCollection.AddTransient<TContextFactory>();
            }
            else if (contextLifetime == ServiceLifetime.Singleton) {
                serviceCollection.AddSingleton<TContextFactory>();
            }
            else {
                serviceCollection.AddScoped<TContextFactory>();
            }
            return serviceCollection;
        }
        /// <summary>
        /// Add DataContext Factory Service
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configFilePath"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContextFactory(this IServiceCollection serviceCollection, string configFilePath, Action<DataContextOptionsConfigurator<DataContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Singleton)
        {
            var configuration = new DataContextConfiguration(configFilePath);
            return AddDataContextFactory<LightDataContextFactory, DataContext>(serviceCollection, configFilePath, optionsAction, contextLifetime);
        }
        /// <summary>
        /// Add DataContext Factory Service
        /// </summary>
        /// <typeparam name="TContextFactory"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="configFilePath"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataContextFactory<TContextFactory, TContext>(this IServiceCollection serviceCollection, string configFilePath, Action<DataContextOptionsConfigurator<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Singleton) where TContextFactory : DataContextFactory<TContext> where TContext : DataContext
        {
            var configuration = new DataContextConfiguration(configFilePath);
            return AddDataContextFactory<TContextFactory, TContext>(serviceCollection, configuration, optionsAction, contextLifetime);
        }

    }
}
