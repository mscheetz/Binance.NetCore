// -----------------------------------------------------------------------------
// <copyright file="IBinanceWSRepository" company="Matt Scheetz">
//     Copyright (c) Matt Scheetz All Rights Reserved
// </copyright>
// <author name="Matt Scheetz" date="11/13/2018 8:49:13 PM" />
// -----------------------------------------------------------------------------

namespace Binance.NetCore.Data.Interface
{
    using Binance.NetCore.Entities;
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    #endregion Usings

    public interface IBinanceWSRepository
    {
        Task SocketTest(string symbol, Interval interval);
    }
}