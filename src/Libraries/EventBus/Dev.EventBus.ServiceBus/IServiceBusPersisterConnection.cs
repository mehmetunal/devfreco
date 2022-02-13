using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System;

namespace Dev.EventBus.ServiceBus
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusClient TopicClient { get; }
        ServiceBusAdministrationClient AdministrationClient { get; }
    }
}
