using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Web.Services
{
    public class QueueService
    {
        CloudStorageAccount StorageAccount { get; }

        public QueueService(string connectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(connectionString);
        }
        public async Task<List<string>> GetAllMesseges()
        {

            // Create the queue client
            CloudQueueClient queueClient = StorageAccount.CreateCloudQueueClient();

            List<string> result = new List<string>();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("aprilqueue");
            await queue.FetchAttributesAsync();

            if (queue.ApproximateMessageCount.HasValue)
            {
                // Get the next message
                IEnumerable<CloudQueueMessage> retrievedMessages = await queue.GetMessagesAsync(queue.ApproximateMessageCount.Value);

                result = retrievedMessages.Select(m => m.AsString).ToList();

                //Process the message in less than 30 seconds, and then delete the message
                await queue.ClearAsync();
            }

            return result;
        }

        public async Task AddMessage(string mes)
        {
            CloudQueueClient queueClient = StorageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("aprilqueue");

            // Create the queue if it doesn't already exist.
            await queue.CreateIfNotExistsAsync();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(mes);
            await queue.AddMessageAsync(message);
        }

        public async Task<int> GetMessageCount()
        {
            CloudQueueClient queueClient = StorageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("aprilqueue");
            await queue.FetchAttributesAsync();
            return queue.ApproximateMessageCount.HasValue ? queue.ApproximateMessageCount.Value : 0;
        }
    }
}
