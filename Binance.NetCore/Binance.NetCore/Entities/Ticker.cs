using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class Ticker
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }
    }
}
