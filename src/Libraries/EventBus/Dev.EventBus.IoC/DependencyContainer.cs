using Dev.EventBus.Abstractions;
using Dev.EventBus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Dev.EventBus.IoC
{
    public class DependencyContainer
    {
        public static void RegisterEventBusConntionServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:EventBusConnection"],
                    DispatchConsumersAsync = true
                };
                if (!string.IsNullOrEmpty(Configuration["EventBus:EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBus:EventBusUserName"];
                }
                if (!string.IsNullOrEmpty(Configuration["EventBus:EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBus:EventBusPassword"];
                }
                return new DefaultRabbitMQPersistentConnection(factory, logger);
            });
        }

        public static void RegisterEventBus(IServiceCollection services, IConfiguration Configuration)
        {
            var subscriptionClientName = Configuration["EventBus:SubscriptionClientName"];
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var provider = sp.GetRequiredService<IServiceProvider>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, provider, eventBusSubcriptionsManager, subscriptionClientName);
            });
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
