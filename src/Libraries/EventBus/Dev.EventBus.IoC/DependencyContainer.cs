﻿using Autofac;
using Dev.EventBus.Abstractions;
using Dev.EventBus.RabbitMQ;
using Dev.EventBus.ServiceBus;
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
            var azureServiceBusEnabled = Configuration.GetSection("EventBus:AzureServiceBusEnabled");
            if (azureServiceBusEnabled != null && bool.Parse(azureServiceBusEnabled.Value) == true)
            {
                services.AddSingleton<IServiceBusPersisterConnection>(sp =>
                {
                    var serviceBusConnectionString = Configuration["EventBus:EventBusConnection"];

                    return new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
                });
            }
            else
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

                    var retryCount = 5;

                    if (!string.IsNullOrEmpty(Configuration["EventBus:EventBusRetryCount"]))
                    {
                        retryCount = int.Parse(Configuration["EventBus:EventBusRetryCount"]);
                    }

                    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
                });
            }
        }

        public static void RegisterEventBus(IServiceCollection services, IConfiguration Configuration)
        {
            var azureServiceBusEnabled = Configuration.GetSection("EventBus:AzureServiceBusEnabled");
            if (azureServiceBusEnabled != null && bool.Parse(azureServiceBusEnabled.Value) == true)
            {
                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                    string subscriptionName = Configuration["EventBus:SubscriptionClientName"];

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, iLifetimeScope, subscriptionName);
                });

            }
            else
            {
                var subscriptionClientName = Configuration["EventBus:SubscriptionClientName"];
                services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                {
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                    var provider = sp.GetRequiredService<IServiceProvider>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();

                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(Configuration["EventBus:EventBusRetryCount"]))
                    {
                        retryCount = int.Parse(Configuration["EventBus:EventBusRetryCount"]);
                    }

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, provider, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
                });
            }
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
