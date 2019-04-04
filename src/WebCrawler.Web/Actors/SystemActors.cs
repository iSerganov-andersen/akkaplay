using Akka.Actor;
using System.Collections.Generic;

namespace WebCrawler.Web.Actors
{
    /// <summary>
    /// Static class used to work around weird SignalR constructors
    /// 
    /// (need to learn how to wire this up properly in signalr)
    /// </summary>
    public static class SystemActors
    {
        public static ActorSystem ActorSystem;

        public static List<IActorRef> SignalRActors = new List<IActorRef>() { ActorRefs.Nobody };

        public static IActorRef CommandProcessor = ActorRefs.Nobody;
    }
}