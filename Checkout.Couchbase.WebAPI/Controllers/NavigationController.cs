using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Core;
using Checkout.Core.Domain;
using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Couchbase.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationController : ControllerBase
    {
        private readonly IBucket _bucket;

        public NavigationController()
        {
            _bucket = ClusterHelper.GetBucket(ConfigurationDefaults.COUCHBASE_COLLECTIONNAME);
        }

        // GET api/navigation/ip/192.168.10.15
        [HttpGet("ip/{ip}")]
        public ActionResult GetByIp(string ip)
        {
            var n1ql = $"SELECT *, META().id FROM {ConfigurationDefaults.COUCHBASE_COLLECTIONNAME} WHERE Ip = $ip";
            var query = QueryRequest.Create(n1ql);
            query.AddNamedParameter("$ip", ip);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            var result = _bucket.Query<object>(query);

            return new JsonResult(result);
        }

        // GET api/navigation/browser/Chrome
        [HttpGet("browser/{browser}")]
        public ActionResult GetByBrowser(string browser)
        {
            var n1ql = $"SELECT *, META().id FROM {ConfigurationDefaults.COUCHBASE_COLLECTIONNAME} WHERE Browser = $browser";
            var query = QueryRequest.Create(n1ql);
            query.AddNamedParameter("$browser", browser);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            var result = _bucket.Query<object>(query);

            return new JsonResult(result);
        }
    }
}
