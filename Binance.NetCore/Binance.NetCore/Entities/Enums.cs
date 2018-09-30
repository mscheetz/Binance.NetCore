using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.NetCore.Entities
{
    public enum DepositStatus
    {
        pending = 0,
        success = 1,
        all = 2,
        not_set = 3
    }

    public enum WithdrawalStatus
    {
        email_sent = 0,
        canceled = 1,
        awating_approval = 2,
        rejected = 3,
        processing = 4,
        failure = 5,
        completed = 6,
        all = 7,
        not_set = 8
    }
}
