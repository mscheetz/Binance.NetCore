using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class TradeParams
    {
        public string symbol { get; set; }
        public string side { get; set; }
        public string type { get; set; }
        public string timeInForce { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }
        public decimal stopPrice { get; set; }
        public decimal icebergQty { get; set; }
        public long timestamp { get; set; }
    }
}
