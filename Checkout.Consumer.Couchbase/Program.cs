using Checkout.Core;
using Checkout.Core.Domain;
using Checkout.Messaging;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Couchbase.N1QL;
using System;
using System.Collections.Generic;

namespace Checkout.Consumer.Couchbase
{
    class Program
    {
        static void Main(string[] args)
        {
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(ConfigurationDefaults.COUCHBASE_URI) }
            }, new PasswordAuthenticator(ConfigurationDefaults.COUCHBASE_USERNAME, ConfigurationDefaults.COUCHBASE_PASSWORD));

            var _bucket = ClusterHelper.GetBucket("navigation");

            var ipIndex = "CREATE INDEX ix_Ip ON navigation(Ip);";
            var browserIndex = "CREATE INDEX ix_Browser ON navigation(Browser);";

            var manager = _bucket.CreateManager();

            manager.CreateN1qlIndex("ix_Ip", false, new[] { "Ip" });
            manager.CreateN1qlIndex("ix_Browser", false, new[] { "Browser" });

            Console.WriteLine("Listening for new messages...");

            var messageingService = new MessagingService();
            messageingService.ActivateMessageListener(ConfigurationDefaults.RABBITMQ_HOST, ConfigurationDefaults.RABBITMQ_EXCHANGENAME,
                (nav) =>
                {
                    _bucket.Insert(Guid.NewGuid().ToString(), nav);

                    Console.WriteLine($"MSG: {nav.ToString()}");
                });

            while (Console.Read() != 13) { }

            messageingService.DeactivateMessageListener();

            ClusterHelper.Close();
        }
    }
}
