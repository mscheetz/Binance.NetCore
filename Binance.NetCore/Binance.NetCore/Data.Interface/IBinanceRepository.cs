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
        /// Get 24hour ticker statistics
        /// </summary>
        /// <param name="symbol">Trading symbol (default = "")</param>
        /// <returns>Array of Tick objects</returns>
        Task<Tick[]> Get24HourStats(string symbol = "");

        /// <summary>
        /// Get BinanceTime
        /// </summary>
        /// <returns>long of timestamp</returns>
        long GetBinanceTime();

        /// <summary>
        /// Withdraw funds from exchange
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="amount">Decimal of amount</param>
        /// <returns>Withdrawal response</returns>
        Task<WithdrawalResponse> WithdrawFunds(string symbol, string address, decimal amount);

        /// <summary>
        /// Withdraw funds from exchange
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="amount">Decimal of amount</param>
        /// <param name="description">Description of address</param>
        /// <returns>Withdrawal response</returns>
        Task<WithdrawalResponse> WithdrawFunds(string symbol, string address, decimal amount, string description);

        /// <summary>
        /// Withdraw funds from exchange
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="addressTag">Secondary address identifier</param>
        /// <param name="amount">Decimal of amount</param>
        /// <returns>Withdrawal response</returns>
        Task<WithdrawalResponse> WithdrawFunds(string symbol, string address, string addressTag, decimal amount);

        /// <summary>
        /// Withdraw funds from exchange
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="addressTag">Secondary address identifier</param>
        /// <param name="amount">Decimal of amount</param>
        /// <param name="description">Description of address</param>
        /// <returns>Withdrawal response</returns>
        Task<WithdrawalResponse> WithdrawFunds(string symbol, string address, string addressTag, decimal amount, string description);

        /// <summary>
        /// Get all deposit history
        /// </summary>
        /// <param name="status">deposit status (default all)</param>
        /// <returns>Array of deposits</returns>
        Task<Deposit[]> GetDepositHistory(DepositStatus status = DepositStatus.all);

        /// <summary>
        /// Get deposit history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">deposit status (default all)</param>
        /// <returns>Array of deposits</returns>
        Task<Deposit[]> GetDepositHistory(string asset, DepositStatus status = DepositStatus.all);

        /// <summary>
        /// Get deposit history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">deposit status (default all)</param>
        /// <param name="startTime">Start of date range</param>
        /// <param name="endTime">End of date range</param>
        /// <returns>Array of deposits</returns>
        Task<Deposit[]> GetDepositHistory(string asset, DepositStatus status, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Get all withdrawal history
        /// </summary>
        /// <param name="status">withdrawal status (default all)</param>
        /// <returns>Array of withdrawal</returns>
        Task<Withdrawal[]> GetWithdrawalHistory(WithdrawalStatus status = WithdrawalStatus.all);

        /// <summary>
        /// Get withdrawal history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">withdrawal status (default all)</param>
        /// <returns>Array of withdrawal</returns>
        Task<Withdrawal[]> GetWithdrawalHistory(string asset, WithdrawalStatus status = WithdrawalStatus.all);

        /// <summary>
        /// Get withdrawal history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">withdrawal status (default all)</param>
        /// <param name="startTime">Start of date range</param>
        /// <param name="endTime">End of date range</param>
        /// <returns>Array of withdrawal</returns>
        Task<Withdrawal[]> GetWithdrawalHistory(string asset, WithdrawalStatus status, DateTime startTime, DateTime endTime);
        
        /// <summary>
        /// Get deposit address for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">Account status</param>
        /// <param name="recvWindow">Recieving window?</param>
        /// <returns>String of address</returns>
        Task<Dictionary<string, string>> GetDepositAddress(string asset, bool? status = null, long recvWindow = 0);
    }
}
