using Binance.NetCore.Core;
using Binance.NetCore.Data.Interface;
using Binance.NetCore.Entities;
using DateTimeHelpers;
using FileRepository;
using RESTApiAccess;
using RESTApiAccess.Interface;
using System;
using System.Collections.Generic;
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
                for (int i = 0; i < queryString.Length; i++)
                {
                    qsValues += qsValues != string.Empty ? "&" : "";
                    qsValues += queryString[i];
                }
            }
            if (!secure)
            {
                url = baseUrl + $"{apiPath}";
                if (qsValues != string.Empty)
                    url += "?" + qsValues;

                return url;
            }
            var timestamp = _dtHelper.UTCtoUnixTimeMilliseconds();
            var timeStampQS = $"timestamp={timestamp}";
            if (qsValues != string.Empty)
            {
                qsValues += $"&{timeStampQS}";
            }
            else
            {
                qsValues = $"{timeStampQS}";
            }
            var hmac = security.GetBinanceHMACSignature(qsValues, _apiInfo.apiSecret);

            url = baseUrl + $"{apiPath}?{qsValues}&signature={hmac}";

            return url;
        }
    }
}
