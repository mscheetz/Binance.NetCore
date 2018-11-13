using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class ExchangeInfo
    {
        public string timezone { get; set; }
        public long serverTime { get; set; }
        public RateLimit[] rateLimits { get; set; }
        public object[] exchangeFilters { get; set; }
        public Symbol[] symbols { get; set; }
    }
}
