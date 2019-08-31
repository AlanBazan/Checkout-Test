using Checkout.Core;
using Checkout.Core.Domain;
using Checkout.Messaging;
using Checkout.SqlServerConsumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Checkout.Consumer.SqlServer
{
    class Program
    {
        public static void Main(string[] args)
        {            
            Console.WriteLine("Listening for new messages...");

            var messageingService = new MessagingService();
            using (var context = new CheckoutContext())
            {
                context.Database.EnsureCreated();

                messageingService.ActivateMessageListener(ConfigurationDefaults.RABBITMQ_HOST, ConfigurationDefaults.RABBITMQ_EXCHANGENAME,
                    (nav) =>
                    {
                        var navObject = JsonConvert.DeserializeObject<Navigation>(nav);
                        context.Navigations.Add(navObject);
                        context.SaveChanges();

                        Console.WriteLine($"MSG: {navObject.ToString()}");
                    });

                while (Console.Read() != 13) { }

                messageingService.DeactivateMessageListener();
            }
        }

    }
}
