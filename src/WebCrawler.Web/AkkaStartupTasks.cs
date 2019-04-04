using Akka.Actor;
using Akka.Bootstrap.Docker;
using Akka.Routing;
using System.Collections.Generic;
using WebCrawler.Shared.Config;
using WebCrawler.Web.Actors;

namespace WebCrawler.Web
{
    public static class AkkaStartupTasks
    {
        public static ActorSystem StartAkka()
        {
            var config = HoconLoader.ParseConfig("web.hocon");
            SystemActors.ActorSystem = ActorSystem.Create("webcrawler", config.BootstrapFromDocker());
            var router = SystemActors.ActorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "tasker");
            var processor = SystemActors.CommandProcessor = SystemActors.ActorSystem.ActorOf(Props.Create(() => new CommandProcessor(router)),"commands");
            SystemActors.SignalRActors = new List<IActorRef>() {
                SystemActors.ActorSystem.ActorOf(Props.Create(() => new SignalRActor(processor)), "signalr0"),
                SystemActors.ActorSystem.ActorOf(Props.Create(() => new SignalRActor(processor)), "signalr1"),
                SystemActors.ActorSystem.ActorOf(Props.Create(() => new SignalRActor(processor)), "signalr2"),
                SystemActors.ActorSystem.ActorOf(Props.Create(() => new SignalRActor(processor)), "signalr3"),
                SystemActors.ActorSystem.ActorOf(Props.Create(() => new SignalRActor(processor)), "signalr4")
            };
            return SystemActors.ActorSystem;
        }
    }
}