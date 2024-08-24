namespace Mango.Services.RewardApi.Messaging.Interfaces
{
    public interface IAzureServiceBusConsumer
    {

        Task Start();
        Task Stop();

    }
}
