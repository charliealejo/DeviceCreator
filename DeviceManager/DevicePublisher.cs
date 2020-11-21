using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messaging;
using Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager
{
    /// <summary>
    /// This class is the entry point to access the database from the client side.
    /// </summary>
    public class DevicePublisher : IDevicePublisher
    {
        private readonly IConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> addDeviceResults = new BlockingCollection<string>();
        private readonly BlockingCollection<IEnumerable<IDevice>> getDevicesResults = new BlockingCollection<IEnumerable<IDevice>>();
        private readonly IBasicProperties props;
        private readonly JsonSerializerSettings jsonSettings;

        /// <summary>
        /// Creates a new instance of this class, setting up everything needed
        /// for the RabbitMQ messaging system from the client side.
        /// </summary>
        public DevicePublisher()
        {
            factory = new ConnectionFactory() { HostName = Shared.Constants.RABBIT_HOST_NAME };
            connection = factory.CreateConnection();
            var correlationId = Guid.NewGuid().ToString();

            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);
            props = channel.CreateBasicProperties();
            props.ContentEncoding = "application/json";

            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var res = (Result)JsonConvert.DeserializeObject(response, jsonSettings);
                    if (res.Type == ResultType.ADD_DEVICE)
                        addDeviceResults.Add((string)res.Data);
                    else if (res.Type == ResultType.GET_LIST)
                        getDevicesResults.Add((IEnumerable<IDevice>)res.Data);
                }
            };

            jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        }

        /// <summary>
        /// Saves a device into the database.
        /// </summary>
        /// <param name="device">The instance of the device to save.</param>
        /// <returns>A boolean value indicating if the device could be inserted or not.</returns>
        public bool SaveDevice(IDevice device)
        {
            var petition = new Petition { Type = PetitionType.ADD_DEVICE, Data = device };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(petition, jsonSettings));
            channel.BasicPublish(
                exchange: "",
                routingKey: Shared.Constants.QUEUE_NAME,
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var res = addDeviceResults.Take();
            return bool.Parse(res);
        }

        /// <summary>
        /// Gets the list of devices stored in the database.
        /// </summary>
        /// <returns>The list of devices already present in the database.</returns>
        public IEnumerable<IDevice> GetDevices()
        {
            var petition = new Petition { Type = PetitionType.GET_LIST };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(petition, jsonSettings));
            channel.BasicPublish(
                exchange: "",
                routingKey: Shared.Constants.QUEUE_NAME,
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var res = getDevicesResults.Take();
            return res;
        }

        /// <summary>
        /// Shuts down the messaging system and all the disposable objects.
        /// </summary>
        public void Close()
        {
            addDeviceResults.CompleteAdding();
            addDeviceResults.Dispose();
            getDevicesResults.CompleteAdding();
            getDevicesResults.Dispose();
            channel.Dispose();
            connection.Close();
            connection.Dispose();
        }
    }
}
