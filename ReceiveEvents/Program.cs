﻿using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;

namespace ReceiveEvents
{
    class Program
    {
        private const string ConnectionString = "your_event_hub_connection_string";
        private const string EventHubName = "your_event_hub_name";
        private const string BlobStorageConnectionString = "your_blob_connection_string";
        private const string BlobContainerName = "your_container_name";
        
        static async Task Main()
        {
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            // Event client uses a blob storage to save the checkpoint. 
            BlobContainerClient storageClient = new BlobContainerClient(BlobStorageConnectionString, BlobContainerName);
            EventProcessorClient processorClient = new EventProcessorClient(storageClient, consumerGroup, ConnectionString, EventHubName);

            // Registering the handlers
            processorClient.ProcessEventAsync += ProcessEventHandler;
            processorClient.ProcessErrorAsync += ProcessErrorHandler;

            await processorClient.StartProcessingAsync();
            await processorClient.StopProcessingAsync();
        }

        static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            Console.WriteLine("\tEvent received: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            // Important: make sure the save the checkpoint.
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"\tException in partition '{ eventArgs.PartitionId}': an unhandled exception has occurred.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        } 
    }
}
