using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class ResponseWrapper<T>
    {
        public string success { get; set; }

        public T depositList { get; set; }

        public T withdrawList { get; set; }
    }
}
