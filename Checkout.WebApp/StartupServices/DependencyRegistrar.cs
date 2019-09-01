using Checkout.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.WebApp.StartupServices
{
    public static class DependencyExtensions
    {
        public static IServiceCollection DependencyRegistrar(
               this IServiceCollection services)
        {
            services.AddScoped<IMessagingService, MessagingService>();

            return services;
        }
    }
}
