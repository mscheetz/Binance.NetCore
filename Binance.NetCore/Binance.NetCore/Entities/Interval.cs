using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Binance.NetCore.Entities
{
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
