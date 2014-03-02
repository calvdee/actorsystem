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
using ServiceStack.Text.Json;
using ServiceStack.Text;
using Newtonsoft.Json;

namespace ActorSystemInfra.Actors
{


    public class StatusActor : TypedActor, IHandle<ActorStatusRequest>
    {
        private LoggingAdapter log = Logging.GetLogger(Context);
        private Logger Loggly = new Logger("aad0a7e8-bafc-4405-8630-3b99d8953bd3");
        private String m_Status;
        private long m_MessageCount = 0;

        public void Handle(ActorStatusRequest request)
        {
            m_MessageCount += 1;
            m_Status = "This status was generated at " + DateTime.Now.ToString("MM-dd-yyyyTHH:mm:ss");
            log.Info("Reporting status {0}", m_Status);
            var response = new ActorStatusResponse(m_Status, Thread.CurrentThread.ManagedThreadId.ToString(), m_MessageCount);
            Loggly.LogSync(JsonConvert.SerializeObject(response), true);
            
            Context.Sender.Tell(response);
        }
    }


}
