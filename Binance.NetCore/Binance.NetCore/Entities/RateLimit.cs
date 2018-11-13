using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class RateLimit
    {
        public string rateLimitType { get; set; }
        public string interval { get; set; }
        public int limit { get; set; }
    }
}
