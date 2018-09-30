using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public class Deposit
    {
        public long insertTime { get; set; }
        public decimal amount { get; set; }
        public string asset { get; set; }
        public string address { get; set; }
        public string addressTag { get; set; }
        public string txId { get; set; }
        public int status { get; set; }

        public DepositStatus depositStatus
        {
            get
            {
                return (DepositStatus) status;
            }
        }
    }
}
