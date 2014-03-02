using ActorSystemInfra.Actors;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Loggly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorSystemInfra
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new actor system (a container for your actors)
            var config = ConfigurationFactory.ParseString(SystemConfiguration.Config);
            var system = ActorSystem.Create("status-system", config);

            // Create your actor and get a reference (proxy) to it
            var actor = system.ActorOf<StatusActor>();

            // Create workers
            system.ActorOf<StatusActor>("status-1");
            system.ActorOf<StatusActor>("status-2");
            system.ActorOf<StatusActor>("status-3");
            system.ActorOf<StatusActor>("status-4");

            // Create the round robin router
            var routerConfig = new RoundRobinGroup("user/status-1", "user/status-2", "user/status-3", "user/status-4");
            var props = new Props().WithRouter(routerConfig);
            var router = system.ActorOf(props);

            // Save the actor handles
            var futures = new List<Task>();


            // Send a whole bunch of messages
            long[] messages = new long[] { 1, 10, 100, 1000, 10000, 100000 };
            long sum = 0;
            foreach(long i in messages)
            {
                sum += i;
                Stopwatch sw = Stopwatch.StartNew();
                for(long j = 0; j < i; j++) futures.Add(router.Ask<ActorStatusResponse>(new ActorStatusRequest()));
                Console.WriteLine("Sent {0} messages in {1} seconds", messages, sw.ElapsedMilliseconds / 1000.0);
            }

            // Wait on the handles
            Stopwatch sw2 = Stopwatch.StartNew();
            futures.ForEach(_ => _.Wait());
            Console.WriteLine("Processed {0} messages in {1}", sum, sw2.ElapsedMilliseconds / 1000.0);
            Console.ReadKey();
        }
    }
}
