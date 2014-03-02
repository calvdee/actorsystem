using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorSystemInfra.Actors
{
    public class ActorStatusResponse
    {
        private readonly String m_Status;
        private readonly String m_ThreadId;
        private readonly DateTime m_Timestamp;
        private readonly long m_MessageCount;
        public String StatusUpdate { get { return m_Status; } }
        public DateTime Timestamp { get { return m_Timestamp; } }
        public String ThreadId { get { return m_ThreadId; } }
        public long MessageCount { get { return m_MessageCount; } }

        public ActorStatusResponse(String status, String thread, long count)
        {
            m_Status = status;
            m_Timestamp = DateTime.Now;
            m_ThreadId = thread;
            m_MessageCount = count;
        }
    }
}
