using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure;
using Microsoft.Extensions.Configuration;
using WebCrawler.Web.Actors;
using WebCrawler.Web.Models;
using WebCrawler.Web.Services;

namespace WebCrawler.Web.Hubs
{
    public class CrawlHub : Hub
    {
        string ConnectionString { get; }
        CrawlHubHelper _hubhelper { get; } 
        public CrawlHub(IConfiguration configuration, CrawlHubHelper chHelper)
        {
            ConnectionString = configuration.GetConnectionString("AzureStorageConnection");
            _hubhelper = chHelper;
        }
        public async Task<string> StartCrawl()
        {
            try
            {
                var queueService = new QueueService(ConnectionString);
                var messages = await queueService.GetAllMesseges();
                if (messages.Count > 5)
                    throw new Exception("Max allowed amount of messages per processing request is 5");
                for (int i = 0; i < messages.Count; i++)
                {
                   SystemActors.SignalRActors[i].Tell(messages[i], ActorRefs.Nobody);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return String.Format("Failed: {0}", ex.Message);
            }
        }

        public async Task<string> AddUrl(string mes)
        {
            try
            {
                var queueService = new QueueService(ConnectionString);
                await queueService.AddMessage(mes);
                return "Success";
            }
            catch (Exception ex)
            {
                return String.Format("Failed: {0}", ex.Message);
            }
        }

        public async Task GetMessageCount()
        {
            try
            {
                var queueService = new QueueService(ConnectionString);
                var result = await queueService.GetMessageCount();
                await this.Clients.All.SendAsync("CounterSubscription", new CountResponse(result));
            }
            catch (Exception ex)
            {
                await this.Clients.All.SendAsync("CounterSubscription", new CountResponse(0, "Falied", ex.Message));
            }
        }
    }
}
