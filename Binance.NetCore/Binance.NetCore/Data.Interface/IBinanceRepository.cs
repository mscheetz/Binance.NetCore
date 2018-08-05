using Binance.NetCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Binance.NetCore.Data.Interface
{
    public interface IBinanceRepository
    {
        /// <summary>
        /// Check if the Exchange Repository is ready for trading
        /// </summary>
        /// <returns>Boolean of validation</returns>
        bool ValidateExchangeConfigured();

        /// <summary>
        /// Get Transactions for account
        /// </summary>
        /// <returns>Collection of Transactions</returns>
        Task<IEnumerable<Transaction>> GetTransactions();

        /// <summary>
        /// Get account balance
        /// </summary>
        /// <returns>Account object</returns>
        Task<Account> GetBalance();

        /// <summary>
        /// Get order information
        /// </summary>
        /// <param name="symbol">string of symbol</param>
        /// <param name="orderId">long of orderId</param>
        /// <returns>OrderResponse object</returns>
        Task<OrderResponse> GetOrder(string symbol, long orderId);

        /// <summary>
        /// Get all order information
        /// </summary>
        /// <param name="symbol">string of symbol</param>
        /// <param name="limit">Int of orders count to return, default 20</param>
        /// <returns>Array OrderResponse object</returns>
        Task<OrderResponse[]> GetOrders(string symbol, int limit = 20);

        /// <summary>
        /// Get all open orders
        /// </summary>
        /// <param name="symbol">string of symbol</param>
        /// <returns>Array OrderResponse object</returns>
        Task<OrderResponse[]> GetOpenOrders(string symbol);

        /// <summary>
        /// Get Order Book for a pair
        /// </summary>
        /// <param name="symbol">string of trading pair</param>
        /// <param name="limit">Number of orders to return</param>
        /// <returns>OrderBook object</returns>
        Task<OrderBook> GetOrderBook(string symbol, int limit = 100);

        /// <summary>
        /// Post/Place a trade
        /// </summary>
        /// <param name="tradeParams">Trade to place</param>
        /// <returns>TradeResponse object</returns>
        Task<TradeResponse> PostTrade(TradeParams tradeParams);

        /// <summary>
        /// Delete/Cancel a trade
        /// </summary>
        /// <param name="tradeParams">Trade to delete</param>
        /// <returns>TradeResponse object</returns>
        Task<TradeResponse> DeleteTrade(CancelTradeParams tradeParams);

        /// <summary>
        /// Get Ticker for all pairs
        /// </summary>
        /// <returns>Collection of BinanceTick objects</returns>
        Task<IEnumerable<Tick>> GetCrytpos();

        /// <summary>
        /// Get Candlesticks for a symbol
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <param name="interval">Time interval</param>
        /// <param name="limit">Time limit</param>
        /// <returns>Array of Candlestick objects</returns>
        Task<Candlestick[]> GetCandlestick(string symbol, Interval interval, int limit = 500);

        /// <summary>
        /// Get BinanceTime
        /// </summary>
        /// <returns>long of timestamp</returns>
        long GetBinanceTime();
    }
}
