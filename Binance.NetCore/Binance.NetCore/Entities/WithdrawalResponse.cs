using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class WithdrawalResponse
    {
        public string msg { get; set; }
        public bool success { get; set; }
        public string id { get; set; }
    }
}
