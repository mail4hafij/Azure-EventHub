using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace SendEvents
{
    class Program
    {
        private const string ConnectionString = "your_event_hub_connection_string";
        private const string EventHubName = "your_event_hub_name";

        static async Task Main()
        {
            await using (var producerClient = new EventHubProducerClient(ConnectionString, EventHubName))
            {
                using (EventDataBatch eventBatch = await producerClient.CreateBatchAsync())
                {
                    eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Hello world one")));
                    eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Hello world two")));

                    await producerClient.SendAsync(eventBatch);
                    Console.WriteLine("Events are published.");
                }
            }
        }
    }
}
