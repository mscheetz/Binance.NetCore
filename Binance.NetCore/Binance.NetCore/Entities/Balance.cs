using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class Balance
    {
        public string asset { get; set; }
        public decimal free { get; set; }
        public decimal locked { get; set; }
    }
}
