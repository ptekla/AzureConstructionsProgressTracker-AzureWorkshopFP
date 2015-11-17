using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AzureConstructionsProgressTracker.Common
{
    internal class ServiceBusManager
    {
        private readonly string _serviceBusNamespaceConnectionString;
        private string QueueName = "resizepicturesqueue";

        public ServiceBusManager(string serviceBusNamespaceConnectionString)
        {
            _serviceBusNamespaceConnectionString = serviceBusNamespaceConnectionString;
        }


        public void CreateQueue()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(_serviceBusNamespaceConnectionString);

            if (!namespaceManager.QueueExists(QueueName))
            {
                var queueDescription = new QueueDescription(QueueName);

                namespaceManager.CreateQueue(queueDescription);
            }
        }

        public async Task Enqueue(object message)
        {
            var brokeredMessage = new BrokeredMessage(message);

            var topicClient = QueueClient.CreateFromConnectionString(_serviceBusNamespaceConnectionString, QueueName);

            await topicClient.SendAsync(brokeredMessage);
        }
    }
}