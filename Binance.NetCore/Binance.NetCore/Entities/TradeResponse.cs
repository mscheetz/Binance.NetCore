using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class TradeResponse
    {
        public string symbol { get; set; }
        public long orderId { get; set; }
        public string clientOrderId { get; set; }
        public long transactTime { get; set; }
        public decimal price { get; set; }
        public decimal origQty { get; set; }
        public decimal executedQty { get; set; }
        public OrderStatus status { get; set; }
        public TimeInForce timeInForce { get; set; }
        public OrderType type { get; set; }
        public Side side { get; set; }
    }
}
