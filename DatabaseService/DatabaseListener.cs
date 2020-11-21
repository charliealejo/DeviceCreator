using DatabaseService.DB;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messaging;
using Shared.Models;
using System.Text;

namespace DatabaseService
{
    /// <summary>
    /// This class receives the petitions via RabbitMQ messages and sends the proper responses
    /// after accessing the database.
    /// </summary>
    internal class DatabaseListener
    {
        private readonly IDBAccess dbAccess;
        private readonly JsonSerializerSettings jsonSettings;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer;

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="dbAccess">The object to access the database.</param>
        internal DatabaseListener(IDBAccess dbAccess)
        {
            this.dbAccess = dbAccess;
            jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        }

        /// <summary>
        /// Initializes the objects needed for the RabbitMQ messaging system from the server side.
        /// </summary>
        internal void Start()
        {
            var factory = new ConnectionFactory() { HostName = Shared.Constants.RABBIT_HOST_NAME };

            connection = factory.CreateConnection();

            channel = connection.CreateModel();
            channel.QueueDeclare(Shared.Constants.QUEUE_NAME, false, false, false, null);
            channel.BasicQos(0, 1, false);
            consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(Shared.Constants.QUEUE_NAME, false, consumer);
            consumer.Received += (_, ea) =>
            {
                processMessage(ea);
            };
        }

        internal void Stop()
        {
            channel.Dispose();
            connection.Dispose();
        }

        /// <summary>
        /// Processes a message and sends the responses.
        /// </summary>
        /// <param name="ea">Event received via RabbitMQ.</param>
        private void processMessage(BasicDeliverEventArgs ea)
        {
            string response = null;

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            var message = Encoding.UTF8.GetString(body);
            var petition = (Petition)JsonConvert.DeserializeObject(message, jsonSettings);

            try
            {
                switch (petition.Type)
                {
                    case PetitionType.GET_LIST:
                        var result = new Result
                        {
                            Type = ResultType.GET_LIST,
                            Data = dbAccess.GetDevices()
                        };
                        response = JsonConvert.SerializeObject(result, jsonSettings);
                        break;
                    case PetitionType.ADD_DEVICE:
                        if (dbAccess.SaveDevice((IDevice)petition.Data))
                            response = JsonConvert.SerializeObject(new Result { Type = ResultType.ADD_DEVICE, Data = "true" }, jsonSettings);
                        else
                            response = JsonConvert.SerializeObject(new Result { Type = ResultType.ADD_DEVICE, Data = "false" }, jsonSettings);
                        break;
                }
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(response);
                channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }
    }
}