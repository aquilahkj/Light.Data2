using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Light.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LightDataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection serviceCollection, Action<DataContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            DataContextOptionsBuilder builder = new DataContextOptionsBuilder();
            if (optionsAction != null) {
                optionsAction(builder);
            }
            DataContextOptions options = builder.Build();
            serviceCollection.AddSingleton(options);
            if (contextLifetime == ServiceLifetime.Transient) {
                serviceCollection.AddTransient<DataContext>();
            }
            else if (contextLifetime == ServiceLifetime.Singleton) {
                serviceCollection.AddSingleton<DataContext>();
            }
            else {
                serviceCollection.AddScoped<DataContext>();
            }
            return serviceCollection;
        }

        public static IServiceCollection AddDataContext<TContext>(this IServiceCollection serviceCollection, Action<DataContextOptionsBuilder<TContext>> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DataContext
        {
            DataContextOptionsBuilder<TContext> builder = new DataContextOptionsBuilder<TContext>();
            if (optionsAction != null) {
                optionsAction(builder);
            }
            DataContextOptions<TContext> options = builder.Build();
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
    }
}
