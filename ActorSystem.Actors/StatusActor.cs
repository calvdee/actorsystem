using Akka.Actor;
using Akka.Event;
using Gushing.Interfaces;
using Loggly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace ActorSystemInfra.Actors
{


    public class StatusActor : TypedActor, IHandle<ActorStatusRequest>
    {
        private LoggingAdapter log = Logging.GetLogger(Context);
        private Logger Loggly = new Logger(ConfigurationManager.AppSettings["loggly"]);
        private String m_Status;
        private long m_MessageCount = 0;

        public void Handle(ActorStatusRequest request)
        {
            m_MessageCount += 1;
            m_Status = "This status was generated at " + DateTime.Now.ToString("MM-dd-yyyyTHH:mm:ss");
            
            var response = new ActorStatusResponse(m_Status, Thread.CurrentThread.ManagedThreadId.ToString(), m_MessageCount);
            
            var logResponse = Loggly.LogSync(JsonConvert.SerializeObject(response), true);
            if (!logResponse.Success)
                Console.WriteLine("Failed to send to loggly");
            
            Context.Sender.Tell(response);
        }
    }


}
