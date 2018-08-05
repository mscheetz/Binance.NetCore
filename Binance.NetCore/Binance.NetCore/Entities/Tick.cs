using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class Tick
    {
        public string symbol { get; set; }
        public string priceChange { get; set; }
        public string priceChangePercent { get; set; }
        public string weightedAvgPrice { get; set; }
        public string prevClosePrice { get; set; }
        public string lastPrice { get; set; }
        public string lastQty { get; set; }
        public string bidPrice { get; set; }
        public string bidQty { get; set; }
        public string askPrice { get; set; }
        public string askQty { get; set; }
        public string openPrice { get; set; }
        public string highPrice { get; set; }
        public string lowPrice { get; set; }
        public string volume { get; set; }
        public string quoteVolume { get; set; }
        public object openTime { get; set; }
        public object closeTime { get; set; }
        public int firstId { get; set; }
        public int lastId { get; set; }
        public int count { get; set; }
    }
}
