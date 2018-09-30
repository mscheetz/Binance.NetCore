using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public enum Side
    {
        BUY,
        SELL
    }

    public enum TradeType
    {
        [Description("NONE")]
        NONE,
        [Description("BUY")]
        BUY,
        [Description("VOLUMEBUY")]
        VOLUMEBUY,
        [Description("VOLUMEBUYSELLOFF")]
        VOLUMEBUYSELLOFF,
        [Description("SELL")]
        SELL,
        [Description("VOLUMESELL")]
        VOLUMESELL,
        [Description("VOLUMESELLBUYOFF")]
        VOLUMESELLBUYOFF,
        [Description("STOPLOSS")]
        STOPLOSS,
        [Description("ORDERBOOKBUY")]
        ORDERBOOKBUY,
        [Description("ORDERBOOKSELL")]
        ORDERBOOKSELL,
        [Description("CANCELTRADE")]
        CANCELTRADE
    }

    public enum OrderType
    {
        LIMIT,
        MARKET,
        STOP_LOSS,
        STOP_LOSS_LIMIT,
        TAKE_PROFIT,
        TAKE_PROFIT_LIMIT,
        LIMIT_MAKER
    }

    public enum OrderStatus
    {
        NEW,
        PARTIALLY_FILLED,
        FILLED,
        CANCELED,
        REJECTED,
        EXPIRED
    }

    public enum TimeInForce
    {
        GTC,
        IOC,
        FOK
    }

    public enum Interval
    {
        None,
        [Description("1m")]
        OneM,
        [Description("3m")]
        ThreeM,
        [Description("5m")]
        FiveM,
        [Description("15m")]
        FifteenM,
        [Description("30m")]
        ThirtyM,
        [Description("1h")]
        OneH,
        [Description("2h")]
        TwoH,
        [Description("4h")]
        FourH,
        [Description("6h")]
        SixH,
        [Description("8h")]
        EightH,
        [Description("12h")]
        TwelveH,
        [Description("1d")]
        OneD,
        [Description("3d")]
        ThredD,
        [Description("1w")]
        OneW,
        [Description("1M")]
        OneMo
    }
}
