using Mango.Services.EmailApi.Messaging.Interfaces;

namespace Mango.Services.EmailApi.Extension
{
    public static class ApplicationBuilderExtension
    {
        private static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }   

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var HostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            HostApplicationLife.ApplicationStarted.Register(OnStart);
            HostApplicationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStart()
        {
            ServiceBusConsumer.Start();
        }

        private static void OnStop()
        {
            ServiceBusConsumer.Stop();
        }
    }
}
