using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class OrderBook
    {
        public long lastUpdateId { get; set; }
        public Orders[] bids { get; set; }
        public Orders[] asks { get; set; }
    }
}
