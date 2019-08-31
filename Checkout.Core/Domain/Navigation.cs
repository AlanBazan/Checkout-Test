using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Core.Domain
{
    public class Navigation
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string PageName { get; set; }
        public string Browser { get; set; }
        public string Parameters { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public override string ToString()
        {
            return $"IP:{Ip}; Page:{PageName}; Browser:{Browser}; Params:{Parameters};";
        }
    }
}
