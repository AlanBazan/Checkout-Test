using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Core;
using Checkout.Core.Domain;
using Checkout.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationController : ControllerBase
    {
        private readonly IMessagingService _messagingService;

        public NavigationController(IMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Navigation model)
        {
            model.CreatedOnUtc = DateTime.UtcNow;

            _messagingService.EnqueueMessage(ConfigurationDefaults.RABBITMQ_HOST, ConfigurationDefaults.RABBITMQ_EXCHANGENAME, model);
        }
    }
}
