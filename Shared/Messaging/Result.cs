using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging
{
    /// <summary>
    /// This class represents a result in the RabbitMQ messaging system.
    /// 
    /// A result is a request sent by the server side and received by the client side, and
    /// that must be handled by the specific client that sent the original petition.
    /// </summary>
    public class Result
    {
        public ResultType Type { get; set; }

        public object Data { get; set; }
    }

    public enum ResultType
    {
        GET_LIST,
        ADD_DEVICE
    }
}
