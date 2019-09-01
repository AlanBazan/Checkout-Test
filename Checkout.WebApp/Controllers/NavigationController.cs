using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Checkout.Core;
using Checkout.Core.Domain;
using Checkout.Messaging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.WebApp.Controllers
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
            model.Ip = GetIpAddress();
            model.Browser = GetBrowser();

            _messagingService.EnqueueMessage(ConfigurationDefaults.RABBITMQ_HOST, ConfigurationDefaults.RABBITMQ_EXCHANGENAME, model);
        }

        private string GetIpAddress()
        {
            var result = string.Empty;
            try
            {
                if (HttpContext.Request.Headers != null)
                {
                    //the X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a client
                    //connecting to a web server through an HTTP proxy or load balancer
                    var forwardedHttpHeaderKey = "X-FORWARDED-FOR";

                    var forwardedHeader = HttpContext.Request.Headers[forwardedHttpHeaderKey];
                    if (forwardedHeader.Any(x => !string.IsNullOrWhiteSpace(x)))
                        result = forwardedHeader.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
                }

                //if this header not exists try get connection remote IP address
                if (string.IsNullOrEmpty(result) && HttpContext.Connection.RemoteIpAddress != null)
                    result = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }

            //some of the validation
            if (result != null && result.Equals(IPAddress.IPv6Loopback.ToString(), StringComparison.InvariantCultureIgnoreCase))
                result = IPAddress.Loopback.ToString();

            //"TryParse" doesn't support IPv4 with port number
            if (IPAddress.TryParse(result ?? string.Empty, out var ip))
                //IP address is valid 
                result = ip.ToString();
            else if (!string.IsNullOrEmpty(result))
                //remove port
                result = result.Split(':').FirstOrDefault();

            return result;
        }

        private string GetBrowser()
        {
            return Request.Headers["User-Agent"].ToString();
        }
    }
}
