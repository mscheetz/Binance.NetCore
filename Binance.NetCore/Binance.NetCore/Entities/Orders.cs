using Binance.NetCore.Core;
using Newtonsoft.Json;

namespace Binance.NetCore.Entities
{
    [JsonConverter(typeof(Converter.ObjectToArrayConverter<Orders>))]
    public class Orders
    {
        [JsonProperty(Order = 1)]
        public decimal price { get; set; }
        [JsonProperty(Order = 2)]
        public decimal quantity { get; set; }
        [JsonProperty(Order = 3)]
        public string[] ignore { get; set; }
    }
}
