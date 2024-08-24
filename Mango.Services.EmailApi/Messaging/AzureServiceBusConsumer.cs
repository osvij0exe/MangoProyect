using Azure.Messaging.ServiceBus;
using Mango.Services.EmailApi.Data.Dtos;
using Mango.Services.EmailApi.Message;
using Mango.Services.EmailApi.Messaging.Interfaces;
using Mango.Services.EmailApi.Services;
using Mango.Services.EmailApi.Services.Implementation;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailApi.Messaging
{

    //pasamos los mensajes del servic bus
    //NOTA: Se requeire una membresia de Azure y crear un servicebus
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string registerUserQueue;
        private readonly string orderCreated_Topic;
        private readonly string orderCreated_Email_Subscription;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private ServiceBusProcessor _emailOrderPlacedProcessor;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration,
            EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");
            orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated");
            orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");

            var clietn = new ServiceBusClient(serviceBusConnectionString);

            _emailCartProcessor = clietn.CreateProcessor(emailCartQueue);
            _registerUserProcessor = clietn.CreateProcessor(registerUserQueue);
            _emailOrderPlacedProcessor = clietn.CreateProcessor(orderCreated_Topic,orderCreated_Email_Subscription);

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestRecived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _registerUserProcessor.ProcessMessageAsync += OnRegisterRequestRecived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registerUserProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestRecived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();

        }


        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();

            await _emailOrderPlacedProcessor.StopProcessingAsync();
            await _emailOrderPlacedProcessor.DisposeAsync();
        }

        private async Task OnEmailCartRequestRecived(ProcessMessageEventArgs args)
        {
            //aqui se resive el mesaje
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            //NOTA: se deserializa el Dto que se encuentra en el service bus de azure
            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                //TODO: try to log email
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task OnOrderPlacedRequestRecived(ProcessMessageEventArgs args)
        {
            //aqui se resive el mesaje
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            //NOTA: se deserializa el Dto que se encuentra en el service bus de azure
            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                //TODO: try to log email
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        private async Task OnRegisterRequestRecived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            //NOTA: se deserializa el Dto que se encuentra en el service bus de azure
            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                //TODO: try to log email
                await _emailService.RegisterUserEmailAndLog(email);
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
