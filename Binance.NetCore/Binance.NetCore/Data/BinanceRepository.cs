using Binance.NetCore.Core;
using Binance.NetCore.Data.Interface;
using Binance.NetCore.Entities;
using DateTimeHelpers;
using FileRepository;
//using RESTApiAccess;
//using RESTApiAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.NetCore.Data
{
    public class BinanceRepository : IBinanceRepository
    {
        private Security security;
        private IRESTRepository _restRepo;
        private string baseUrl;
        private ApiInformation _apiInfo = null;
        private DateTimeHelper _dtHelper;

        /// <summary>
        /// Constructor for non-signed endpoints
        /// </summary>
        public BinanceRepository()
        {
            LoadRepository();
        }

        /// <summary>
        /// Constructor for signed endpoints
        /// </summary>
        /// <param name="apiKey">Api key</param>
        /// <param name="apiSecret">Api secret</param>
        public BinanceRepository(string apiKey, string apiSecret)
        {
            _apiInfo = new ApiInformation
            {
                apiKey = apiKey,
                apiSecret = apiSecret
            };
            LoadRepository();
        }

        /// <summary>
        /// Constructor for signed endpoints
        /// </summary>
        /// <param name="configPath">String of path to configuration file</param>
        public BinanceRepository(string configPath)
        {
            IFileRepository _fileRepo = new FileRepository.FileRepository();

            if(_fileRepo.FileExists(configPath))
            {
                _apiInfo = _fileRepo.GetDataFromFile<ApiInformation>(configPath);
                LoadRepository();
            }
            else
            {
                throw new Exception("Config file not found");
            }
        }

        /// <summary>
        /// Load repository
        /// </summary>
        /// <param name="key">Api key value (default = "")</param>
        /// <param name="secret">Api secret value (default = "")</param>
        private void LoadRepository(string key = "", string secret = "")
        {
            security = new Security();
            _restRepo = new RESTRepository();
            baseUrl = "https://api.binance.com";
            _dtHelper = new DateTimeHelper();
        }

        /// <summary>
        /// Check if the Exchange Repository is ready for trading
        /// </summary>
        /// <returns>Boolean of validation</returns>
        public bool ValidateExchangeConfigured()
        {
            var ready = _apiInfo == null || string.IsNullOrEmpty(_apiInfo.apiKey) ? false : true;
            if (!ready)
                return false;

            return string.IsNullOrEmpty(_apiInfo.apiSecret) ? false : true;
        }

        /// <summary>
        /// Get Transactions for account
        /// </summary>
        /// <returns>Collection of Transactions</returns>
        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            string url = CreateUrl("/api/v3/allOrders");

            var response = await _restRepo.GetApiStream<IEnumerable<Transaction>>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get account balance
        /// </summary>
        /// <returns>Account object</returns>
        public async Task<Account> GetBalance()
        {
            string url = CreateUrl("/api/v3/account");

            try
            {
                var response = await _restRepo.GetApiStream<Account>(url, GetRequestHeaders());

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get order information
        /// </summary>
        /// <param name="symbol">string of symbol</param>
        /// <param name="orderId">long of orderId</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> GetOrder(string symbol, long orderId)
        {
            var queryString = new List<string>
            {
                $"symbol={symbol}",
                $"orderId={orderId}"
            };

            string url = CreateUrl($"/api/v3/order", true, queryString.ToArray());

            var response = await _restRepo.GetApiStream<OrderResponse>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get all current user order information
        /// </summary>
        /// <param name="symbol">string of symbol</param>
        /// <param name="limit">Int of orders count to return, default 20</param>
        /// <returns>Array OrderResponse object</returns>
        public async Task<OrderResponse[]> GetOrders(string symbol, int limit = 20)
        {
            var queryString = new List<string>
            {
                $"symbol={symbol}",
                $"limit={limit}"
            };

            string url = CreateUrl($"/api/v3/allOrders", true, queryString.ToArray());

            var response = await _restRepo.GetApiStream<OrderResponse[]>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get all open orders
        /// </summary>
        /// <param name="symbol">string of symbol</param>
        /// <returns>Array OrderResponse object</returns>
        public async Task<OrderResponse[]> GetOpenOrders(string symbol)
        {
            var queryString = new List<string>
            {
                $"symbol={symbol}"
            };

            string url = CreateUrl($"/api/v3/openOrders", true, queryString.ToArray());

            var response = await _restRepo.GetApiStream<OrderResponse[]>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get Order Book for a pair
        /// </summary>
        /// <param name="symbol">string of trading pair</param>
        /// <param name="limit">Number of orders to return</param>
        /// <returns>OrderBook object</returns>
        public async Task<OrderBook> GetOrderBook(string symbol, int limit = 100)
        {
            var queryString = new List<string>
            {
                $"symbol={symbol}",
                $"limit={limit}"
            };

            string url = CreateUrl($"/api/v1/depth", false, queryString.ToArray());

            var response = await _restRepo.GetApiStream<OrderBook>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Post/Place a trade
        /// </summary>
        /// <param name="tradeParams">Trade to place</param>
        /// <returns>TradeResponse object</returns>
        public async Task<TradeResponse> PostTrade(TradeParams tradeParams)
        {
            var queryString = new List<string>
            {
                $"symbol={tradeParams.symbol}",
                $"side={tradeParams.side}",
                $"type={tradeParams.type}",
                $"timeInForce={tradeParams.timeInForce}",
                $"quantity={tradeParams.quantity}",
                $"price={tradeParams.price}"
            };
            string url = CreateUrl("/api/v3/order", true, queryString.ToArray());

            var response = await _restRepo.PostApi<TradeResponse>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Delete/Cancel a trade
        /// </summary>
        /// <param name="tradeParams">Trade to delete</param>
        /// <returns>TradeResponse object</returns>
        public async Task<TradeResponse> DeleteTrade(CancelTradeParams tradeParams)
        {
            string url = CreateUrl("/api/v3/order");

            var headers = GetRequestHeaders();

            headers.Add("symbol", tradeParams.symbol);
            if (tradeParams.orderId != 0)
            {
                headers.Add("orderId", tradeParams.orderId.ToString());
            }
            else if (!string.IsNullOrEmpty(tradeParams.origClientOrderId))
            {
                headers.Add("origClientOrderId", tradeParams.origClientOrderId);
            }
            headers.Add("timestamp", _dtHelper.UTCtoUnixTime().ToString());

            var response = await _restRepo.DeleteApi<TradeResponse>(url, headers);

            return response;
        }

        /// <summary>
        /// Get Ticker for all pairs
        /// </summary>
        /// <returns>Collection of BinanceTick objects</returns>
        public async Task<IEnumerable<Tick>> GetCrytpos()
        {
            string url = "v1/open/tick";

            var response = await _restRepo.GetApiStream<List<Tick>>(url);

            return response;
        }

        /// <summary>
        /// Get Candlesticks for a symbol
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <param name="interval">Time interval</param>
        /// <param name="limit">Time limit</param>
        /// <returns>Array of Candlestick objects</returns>
        public async Task<Candlestick[]> GetCandlestick(string symbol, Interval interval, int limit = 500)
        {
            var intervalDescription = EnumHelper.GetEnumDescription((Interval)interval);

            var queryString = new List<string>
            {
                $"symbol={symbol}",
                $"interval={intervalDescription}",
                $"limit={limit.ToString()}"
            };

            string url = CreateUrl($"/api/v1/klines", false, queryString.ToArray());

            try
            {
                var response = await _restRepo.GetApiStream<Candlestick[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get 24hour ticker statistics
        /// </summary>
        /// <param name="symbol">Trading symbol (default = "")</param>
        /// <returns>Array of Tick objects</returns>
        public async Task<Tick[]> Get24HourStats(string symbol = "")
        {
            return symbol == "" 
                ? await OnGet24HourStats() 
                : await OnGet24HourStat(symbol);
        }

        /// <summary>
        /// Get 24hour ticker stats for all trading pairs
        /// </summary>
        /// <returns>Array of Tick Objects</returns>
        private async Task<Tick[]> OnGet24HourStats()
        {
            var url = CreateUrl("/api/v1/ticker/24hr", false);

            try
            {
                var response = await _restRepo.GetApiStream<Tick[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get 24hour ticker stats for a trading pair
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <returns>Array of Tick objects</returns>
        private async Task<Tick[]> OnGet24HourStat(string symbol)
        {
            var queryString = new List<string>
            {
                $"symbol={symbol}"
            };

            var url = CreateUrl("/api/v1/ticker/24hr", false, queryString.ToArray());

            try
            {
                var response = await _restRepo.GetApiStream<Tick>(url);

                var array = new Tick[1] { response };

                return array;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get all deposit history
        /// </summary>
        /// <param name="status">deposit status (default all)</param>
        /// <returns>Array of deposits</returns>
        public async Task<Deposit[]> GetDepositHistory(DepositStatus status = DepositStatus.all)
        {
            return await OnGetDepositHistory("", (int) status, 0, 0, 0);
        }

        /// <summary>
        /// Get deposit history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">deposit status (default all)</param>
        /// <returns>Array of deposits</returns>
        public async Task<Deposit[]> GetDepositHistory(string asset, DepositStatus status = DepositStatus.all)
        {
            return await OnGetDepositHistory(asset, (int) status, 0, 0, 0);
        }

        /// <summary>
        /// Get deposit history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">deposit status (default all)</param>
        /// <param name="startTime">Start of date range</param>
        /// <param name="endTime">End of date range</param>
        /// <returns>Array of deposits</returns>
        public async Task<Deposit[]> GetDepositHistory(string asset, DepositStatus status, DateTime startTime, DateTime endTime)
        {
            var start = _dtHelper.UTCtoUnixTime(startTime);
            var end = _dtHelper.UTCtoUnixTime(endTime);

            return await OnGetDepositHistory(asset, (int)status, start, end, 0);
        }

        /// <summary>
        /// Get deposit history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">deposit status (default all)</param>
        /// <param name="startTime">Start of date range</param>
        /// <param name="endTime">End of date range</param>
        /// <param name="recvWindow">Recieving window?</param>
        /// <returns>Array of deposits</returns>
        private async Task<Deposit[]> OnGetDepositHistory(string asset, int status, long startTime, long endTime, long recvWindow)
        {
            if (string.IsNullOrEmpty(asset))
                throw new Exception("Asset type cannot be empty!");

            var parameters = new Dictionary<string, object>();
            if (asset != "")
                parameters.Add("asset", asset);
            if (endTime > 0)
                parameters.Add("endTime", endTime);
            if (recvWindow > 0)
                parameters.Add("recvWindow", recvWindow);
            if (startTime > 0)
                parameters.Add("startTime", startTime);
            if (status < 2)
                parameters.Add("status", status);

            var url = CreateUrl("/wapi/v3/depositHistory.html", true, parameters);

            try
            {
                var response = await _restRepo.GetApiStream<ResponseWrapper<Deposit[]>>(url, GetRequestHeaders());

                return response.depositList;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get all withdrawal history
        /// </summary>
        /// <param name="status">withdrawal status (default all)</param>
        /// <returns>Array of withdrawal</returns>
        public async Task<Withdrawal[]> GetWithdrawalHistory(WithdrawalStatus status = WithdrawalStatus.all)
        {
            return await OnGetWithdrawalHistory("", (int)status, 0, 0, 0);
        }

        /// <summary>
        /// Get withdrawal history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">withdrawal status (default all)</param>
        /// <returns>Array of withdrawal</returns>
        public async Task<Withdrawal[]> GetWithdrawalHistory(string asset, WithdrawalStatus status = WithdrawalStatus.all)
        {
            return await OnGetWithdrawalHistory(asset, (int)status, 0, 0, 0);
        }

        /// <summary>
        /// Get withdrawal history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">withdrawal status (default all)</param>
        /// <param name="startTime">Start of date range</param>
        /// <param name="endTime">End of date range</param>
        /// <returns>Array of withdrawal</returns>
        public async Task<Withdrawal[]> GetWithdrawalHistory(string asset, WithdrawalStatus status, DateTime startTime, DateTime endTime)
        {
            var start = _dtHelper.UTCtoUnixTime(startTime);
            var end = _dtHelper.UTCtoUnixTime(endTime);

            return await OnGetWithdrawalHistory(asset, (int)status, start, end, 0);
        }

        /// <summary>
        /// Get withdrawal history for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">withdrawal status (default all)</param>
        /// <param name="startTime">Start of date range</param>
        /// <param name="endTime">End of date range</param>
        /// <param name="recvWindow">Recieving window?</param>
        /// <returns>Array of withdrawal</returns>
        private async Task<Withdrawal[]> OnGetWithdrawalHistory(string asset, int status, long startTime, long endTime, long recvWindow)
        {
            if (string.IsNullOrEmpty(asset))
                throw new Exception("Asset type cannot be empty!");

            var parameters = new Dictionary<string, object>();
            if (asset != "")
                parameters.Add("asset", asset);
            if (endTime > 0)
                parameters.Add("endTime", endTime);
            if (recvWindow > 0)
                parameters.Add("recvWindow", recvWindow);
            if (startTime > 0)
                parameters.Add("startTime", startTime);
            if (status < 7)
                parameters.Add("status", status);

            var url = CreateUrl("/wapi/v3/withdrawHistory.html", true, parameters);

            try
            {
                var response = await _restRepo.GetApiStream<ResponseWrapper<Withdrawal[]>>(url, GetRequestHeaders());

                return response.withdrawList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get deposit address for an asset
        /// </summary>
        /// <param name="asset">string of asset</param>
        /// <param name="status">Account status</param>
        /// <param name="recvWindow">Recieving window?</param>
        /// <returns>String of address</returns>
        public async Task<Dictionary<string, string>> GetDepositAddress(string asset, bool? status = null, long recvWindow = 0)
        {
            if (string.IsNullOrEmpty(asset))
                throw new Exception("Asset type cannot be empty!");

            var parameters = new Dictionary<string, object>();
            if (asset != "")
                parameters.Add("asset", asset);
            if (recvWindow > 0)
                parameters.Add("recvWindow", recvWindow);
            if (status != null)
                parameters.Add("status", status);

            var url = CreateUrl("/wapi/v3/depositAddress.html", true, parameters);

            try
            {
                var response = await _restRepo.GetApiStream<Dictionary<string, object>>(url, GetRequestHeaders());

                var addresses = new Dictionary<string, string>();
                addresses.Add("address", response["address"].ToString());

                if(response["addressTag"].ToString() != "")
                    addresses.Add("address tag", response["addressTag"].ToString());

                return addresses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get BinanceTime
        /// </summary>
        /// <returns>long of timestamp</returns>
        public long GetBinanceTime()
        {
            string url = CreateUrl("/api/v1/time", false);

            var response = _restRepo.GetApi<ServerTime>(url);

            response.Wait();

            return response.Result.serverTime;
        }

        /// <summary>
        /// Get Request Headers
        /// </summary>
        /// <returns>Dictionary of header values</returns>
        private Dictionary<string, string> GetRequestHeaders()
        {
            var headers = new Dictionary<string, string>();
            headers.Add("X-MBX-APIKEY", _apiInfo.apiKey);

            return headers;
        }

        /// <summary>
        /// Create a CoinEx url
        /// </summary>
        /// <param name="apiPath">String of path to endpoint</param>
        /// <param name="secure">Boolean if secure endpoin (default = true)</param>
        /// <param name="parameters">Dictionary of querystring values</param>
        /// <returns>String of url</returns>
        private string CreateUrl(string apiPath, bool secure, Dictionary<string, object> parameters)
        {
            var qsValues = StringifyDictionary(parameters);

            return CreateUrl(apiPath, secure, qsValues);
        }

        /// <summary>
        /// Create a Binance url
        /// </summary>
        /// <param name="apiPath">String of path to endpoint</param>
        /// <param name="secure">Boolean if secure endpoin (default = true)</param>
        /// <param name="queryString">String[] of querystring values</param>
        /// <returns>String of url</returns>
        private string CreateUrl(string apiPath, bool secure = true, string[] queryString = null)
        {
            var qsValues = string.Empty;
            var url = string.Empty;
            if (queryString != null)
            {
                qsValues = string.Join("&", queryString);
            }

            return CreateUrl(apiPath, secure, qsValues);
        }

        /// <summary>
        /// Create a Binance url
        /// </summary>
        /// <param name="apiPath">String of path to endpoint</param>
        /// <param name="secure">Boolean if secure endpoin (default = true)</param>
        /// <param name="queryString">String of querystring values</param>
        /// <returns>String of url</returns>
        private string CreateUrl(string apiPath, bool secure, string queryString)
        {
            var url = string.Empty;
            if (!secure)
            {
                url = baseUrl + $"{apiPath}";
                if (queryString != "")
                    url += "?" + queryString;

                return url;
            }
            var timestamp = _dtHelper.UTCtoUnixTimeMilliseconds();
            var timeStampQS = $"timestamp={timestamp}";
            queryString = queryString != "" ? $"{queryString}&{timeStampQS}" : $"{timeStampQS}";

            var hmac = security.GetBinanceHMACSignature(queryString, _apiInfo.apiSecret);

            url = baseUrl + $"{apiPath}?{queryString}&signature={hmac}";

            return url;
        }

        private string StringifyDictionary(Dictionary<string, object> parameters)
        {
            var qsValues = string.Empty;

            if (parameters != null)
            {
                qsValues = string.Join("&", parameters.Select(p => p.Key + "=" + p.Value));
            }

            return qsValues;
        }
    }
}
