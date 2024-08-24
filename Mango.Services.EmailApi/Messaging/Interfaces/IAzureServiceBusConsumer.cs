namespace Mango.Services.EmailApi.Messaging.Interfaces
{
    public interface IAzureServiceBusConsumer
    {

        Task Start();
        Task Stop();

    }
}
