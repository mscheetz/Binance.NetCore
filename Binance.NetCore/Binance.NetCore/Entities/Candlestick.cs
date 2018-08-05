using Binance.NetCore.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    [JsonConverter(typeof(Converter.ObjectToArrayConverter<Candlestick>))]
    public class Candlestick
    {
        [JsonProperty(Order = 1)]
        public long openTime { get; set; }
        [JsonProperty(Order = 2)]
        public decimal open { get; set; }
        [JsonProperty(Order = 3)]
        public decimal high { get; set; }
        [JsonProperty(Order = 4)]
        public decimal low { get; set; }
        [JsonProperty(Order = 5)]
        public decimal close { get; set; }
        [JsonProperty(Order = 6)]
        public decimal volume { get; set; }
        [JsonProperty(Order = 7)]
        public long closeTime { get; set; }
        [JsonProperty(Order = 8)]
        public double quoteAssetVolume { get; set; }
        [JsonProperty(Order = 9)]
        public long numberTrades { get; set; }
        [JsonProperty(Order = 10)]
        public double takerBase { get; set; }
        [JsonProperty(Order = 11)]
        public double takerQuote { get; set; }
    }
}
