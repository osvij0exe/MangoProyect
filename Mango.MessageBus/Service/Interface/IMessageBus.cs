using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus.Service.Interface
{
    // Nota se Requiere de una suscripicon de AZURE para que funcione
    public interface IMessageBus
    {
        Task PublishMessage(object message, string topic_queue_Name);

    }
}
