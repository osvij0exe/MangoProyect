using Azure.Messaging.ServiceBus;
using Mango.Services.RewardApi.Message;
using Mango.Services.RewardApi.Messaging.Interfaces;
using Mango.Services.RewardApi.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.RewardApi.Messaging
{

    //pasamos los mensajes del servic bus
    //NOTA: Se requeire una membresia de Azure y crear un servicebus
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderCreatedTopic;
        private readonly string orderCreatedRewardSuscription;
        private readonly IConfiguration _configuration;
        private readonly RewardsServices _rewardsService;

        private ServiceBusProcessor _rewardProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration,
            RewardsServices rewardsService)
        {
            _configuration = configuration;
            _rewardsService = rewardsService;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreateTopic");
            orderCreatedRewardSuscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");

            var clietn = new ServiceBusClient(serviceBusConnectionString);

            _rewardProcessor = clietn.CreateProcessor(orderCreatedTopic,orderCreatedRewardSuscription);

        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnOrderRewardsRequestRecived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();

        }


        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();

        }

        private async Task OnOrderRewardsRequestRecived(ProcessMessageEventArgs args)
        {
            //aqui se resive el mesaje
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            //NOTA: se deserializa el Dto que se encuentra en el service bus de azure
            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                //TODO: try to log email
                await _rewardsService.UpdateRewards(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }

            
        }


        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //generalmente se manda un emal.investigar
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask; ;
        }


    }
}
