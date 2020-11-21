using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging
{
    /// <summary>
    /// This class represents a petition in the RabbitMQ messaging system.
    /// 
    /// A petition is a request sent by the client side and received by the server side, and
    /// that must be handled by the database accesor.
    /// </summary>
    public class Petition
    {
        public PetitionType Type { get; set; }

        public object Data { get; set; }
    }

    public enum PetitionType
    {
        GET_LIST,
        ADD_DEVICE
    }
}
