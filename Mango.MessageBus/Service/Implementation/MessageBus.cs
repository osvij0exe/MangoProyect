using Azure.Messaging.ServiceBus;
using Mango.MessageBus.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus.Service.Implementation
{
    // ser requiere de una suscripcion en AZURE para que funcion y un SERVICEBUS creado
    public class MessageBus : IMessageBus
    {
        // Shared acces policies - obtenida del service bus de azure
        //Este solo es un ejemplo
        private string connectionstring = "Endpont=sb://mangoweb.servicebus.windoes.net/;sharedAccesKeyName=RootManageSaherdAccesKey;SharedAccessKey=Hjpsls58pPHtAULb0tay/jx4Ys0+Mo5/R+ASbCcFTG0=";
        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionstring);

            ServiceBusSender sender = client.CreateSender(topic_queue_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);

            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };
            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();

        }
    }
}
