using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class Withdrawal
    {
        public string id { get; set; }
        public decimal amount { get; set; }
        public string address { get; set; }
        public string addressTag { get; set; }
        public string asset { get; set; }
        public string txId { get; set; }
        public long applyTime { get; set; }
        public int status { get; set; }

        public WithdrawalStatus withdrawalStatus
        {
            get
            {
                return (WithdrawalStatus)status;
            }
        }
    }
}
