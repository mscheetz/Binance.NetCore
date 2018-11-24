using Binance.NetCore.Core;
using Binance.NetCore.Data.Interface;
using Binance.NetCore.Entities;
using DateTimeHelpers;
using FileRepository;
using RESTApiAccess;
using RESTApiAccess.Interface;
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
        private bool testApi = false;

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
        /// Constructor for signed endpoints
        /// </summary>
        /// <param name="apiKey">Api key</param>
        /// <param name="apiSecret">Api secret</param>
        /// <param name="test">Testing api?</param>
        public BinanceRepository(string apiKey, string apiSecret, bool test)
        {
            _apiInfo = new ApiInformation
            {
                apiKey = apiKey,
                apiSecret = apiSecret
            };
            testApi = test;
            LoadRepository();
        }

        /// <summary>
        /// Constructor for signed endpoints
        /// </summary>
        /// <param name="configPath">String of path to configuration file</param>
        /// <param name="test">Testing api?</param>
        public BinanceRepository(string configPath, bool test)
        {
            IFileRepository _fileRepo = new FileRepository.FileRepository();

            if (_fileRepo.FileExists(configPath))
            {
                _apiInfo = _fileRepo.GetDataFromFile<ApiInformation>(configPath);
                testApi = test;
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
        /// Get exchange and symbol information
        /// </summary>
        /// <returns>ExchangeInfo object</returns>
        public async Task<ExchangeInfo> GetExchangeInfo()
        {
            string url = CreateUrl("/api/v1/exchangeInfo");

            var response = await _restRepo.GetApiStream<ExchangeInfo>(url);

            return response;
        }

        /// <summary>
        /// Get exchange trading pairs
        /// </summary>
        /// <returns>Collection of trading pairs</returns>
        public async Task<string[]> GetTradingPairs()
        {
            string url = CreateUrl("/api/v1/exchangeInfo");

            var response = await _restRepo.GetApiStream<ExchangeInfo>(url);

            var pairs = response.symbols.Where(s => s.status.Equals("TRADING")).Select(s => s.symbol).ToArray();

            return pairs;
        }

        /// <summary>
        /// Get exchange trading pairs by base pair
        /// </summary>
        /// <param name="baseSymbol">Base symbol of trading pair</param>
        /// <returns>Collection of trading pairs</returns>
        public async Task<string[]> GetTradingPairs(string baseSymbol)
        {
            string url = CreateUrl("/api/v1/exchangeInfo");

            var response = await _restRepo.GetApiStream<ExchangeInfo>(url);

            var pairs = response.symbols.Where(s => s.quoteAsset.Equals(baseSymbol) && s.status.Equals("TRADING")).Select(s => s.symbol).ToArray();

            return pairs;
        }

        /// <summary>
        /// Get details of trading pair
        /// </summary>
        /// <param name="pair">Trading pair to find</param>
        /// <returns>Symbol object</returns>
        public async Task<Symbol> GetTradingPairDetail(string pair)
        {
            string url = CreateUrl("/api/v1/exchangeInfo");

            var response = await _restRepo.GetApiStream<ExchangeInfo>(url);

            var symbol = response.symbols.Where(s => s.symbol.Equals(pair)).FirstOrDefault();

            return symbol;
        }

        /// <summary>
        /// Get details of all trading pairs
        /// </summary>
        /// <returns>Collection of Symbol objects</returns>
        public async Task<Symbol[]> GetTradingPairDetails()
        {
            string url = CreateUrl("/api/v1/exchangeInfo");

            var response = await _restRepo.GetApiStream<ExchangeInfo>(url);

            var symbols = response.symbols;

            return symbols;
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
            if (!TradeParamsValidator(tradeParams))
                return null;

            var queryString = new List<string>();

            queryString.Add($"symbol={tradeParams.symbol}");
            queryString.Add($"side={tradeParams.side}");
            queryString.Add($"type={tradeParams.type}");

            OrderType orderType = (OrderType)Enum.Parse(typeof(OrderType), tradeParams.type);
            if ((orderType == OrderType.LIMIT || orderType == OrderType.STOP_LOSS_LIMIT 
                || orderType == OrderType.TAKE_PROFIT_LIMIT) && string.IsNullOrEmpty(tradeParams.timeInForce))
                tradeParams.timeInForce = TimeInForce.GTC.ToString();

            if (!string.IsNullOrEmpty(tradeParams.timeInForce))
                queryString.Add($"timeInForce={tradeParams.timeInForce}");

            queryString.Add($"quantity={DecimalToString(tradeParams.quantity)}");

            if (tradeParams.price > 0.0M)
                queryString.Add($"price={DecimalToString(tradeParams.price)}");

            if (tradeParams.stopPrice > 0.0M)
                queryString.Add($"stopPrice={DecimalToString(tradeParams.stopPrice)}");

            if (tradeParams.icebergQty > 0.0M)
                queryString.Add($"iceburgQty={DecimalToString(tradeParams.icebergQty)}");

            queryString.Add($"recvWindow=5000");

            string url = CreateUrl("/api/v3/order", true, queryString.ToArray(), true);

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
        /// Get latest price for all trading pairs
        /// </summary>
        /// <returns>Array of Tickers</returns>
        public async Task<Ticker[]> GetTickers()
        {
            return await OnGetTicker(string.Empty);
        }

        /// <summary>
        /// Get latest price for a trading pair
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>A Ticker object</returns>
        public async Task<Ticker> GetTicker(string pair)
        {
            var tickers = await OnGetTicker(pair);

            return tickers[0];
        }

        /// <summary>
        /// Get latest price for one or all trading pairs
        /// </summary>
        /// <returns>Array of Tickers</returns>
        private async Task<Ticker[]> OnGetTicker(string pair)
        {
            var endpoint = "/api/v1/ticker/price";
            var queryString = new Dictionary<string, object>();
            queryString.Add("symbol", pair);

            var url = string.IsNullOrEmpty(pair) ? CreateUrl(endpoint, false) : CreateUrl(endpoint, false, queryString);

            try
            {
                Ticker[] response;
                if (string.IsNullOrEmpty(pair))
                {
                    var ticker = await _restRepo.GetApiStream<Ticker>(url);

                    response = new Ticker[] { ticker };
                }
                else
                {
                    response = await _restRepo.GetApiStream<Ticker[]>(url);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region WAPI

        /// <summary>
        /// Withdraw funds from exchange
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="amount">Decimal of amount</param>
        /// <returns>Withdrawal response</returns>
        public async Task<WithdrawalResponse> WithdrawFunds(string symbol, string address, decimal amount)
        {
            return await OnPostWithdrawal(symbol, address, amount);
        }

        /// <summary>
        /// Withdraw funds from exchange
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="amount">Decimal of amount</param>
        /// <param name="description">Description of address</param>
        /// <returns>Withdrawal response</returns>
        public async Task<WithdrawalResponse> WithdrawFunds(string symbol, string address, decimal amount, string description)
        {
            return await OnPostWithdrawal(symbol, address, amount, description);
        }

        /// <summary>
        /// Withdraw funds from exchange
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="addressTag">Secondary address identifier</param>
        /// <param name="amount">Decimal of amount</param>
        /// <returns>Withdrawal response</returns>
        public async Task<WithdrawalResponse> WithdrawFunds(string symbol, string address, string addressTag, decimal amount)
        {
            return await OnPostWithdrawal(symbol, address, amount, "", addressTag);
        }

        /// <summary>
        /// Withdraw funds from exchange
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="addressTag">Secondary address identifier</param>
        /// <param name="amount">Decimal of amount</param>
        /// <param name="description">Description of address</param>
        /// <returns>Withdrawal response</returns>
        public async Task<WithdrawalResponse> WithdrawFunds(string symbol, string address, string addressTag, decimal amount, string description)
        {
            return await OnPostWithdrawal(symbol, address, amount, description, addressTag);
        }

        /// <summary>
        /// Post a withdrawal
        /// </summary>
        /// <param name="symbol">Symbol of asset</param>
        /// <param name="address">Address to send funds to</param>
        /// <param name="amount">Decimal of amount</param>
        /// <param name="name">Description of address</param>
        /// <param name="addressTag">Secondary address identifier</param>
        /// <param name="recvWindow">Recieving window?</param>
        /// <returns>Withdrawal response</returns>
        private async Task<WithdrawalResponse> OnPostWithdrawal(string symbol, string address, decimal amount, string name = "", string addressTag = "", long recvWindow = 0)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new Exception("Asset type cannot be empty!");

            var parameters = new Dictionary<string, object>();
            parameters.Add("asset", symbol);
            parameters.Add("address", address);
            if (!string.IsNullOrEmpty(addressTag))
                parameters.Add("addressTag", addressTag);

            parameters.Add("amount", amount);
            if(!string.IsNullOrEmpty(name))
                parameters.Add("name", name);
            if (recvWindow > 0)
                parameters.Add("recvWindow", recvWindow);
            parameters.Add("timestamp", _dtHelper.UTCtoUnixTimeMilliseconds());

            var url = CreateUrl("/wapi/v3/withdraw.html", true, parameters);

            try
            {
                var response = await _restRepo.PostApi<WithdrawalResponse>(url, GetRequestHeaders());

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

#endregion

        /// <summary>
        /// Get BinanceTime
        /// </summary>
        /// <returns>long of timestamp</returns>
        public long GetBinanceTime()
        {
            string url = CreateUrl("/api/v1/time", false);

            var response = _restRepo.GetApi<ServerTime>(url).Result;

            return response.serverTime;
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
        /// <param name="testable">Does the endpoint have a test api</param>
        /// <returns>String of url</returns>
        private string CreateUrl(string apiPath, bool secure, Dictionary<string, object> parameters, bool testable = false)
        {
            var qsValues = StringifyDictionary(parameters);

            return CreateUrl(apiPath, secure, qsValues, testable);
        }

        /// <summary>
        /// Create a Binance url
        /// </summary>
        /// <param name="apiPath">String of path to endpoint</param>
        /// <param name="secure">Boolean if secure endpoin (default = true)</param>
        /// <param name="queryString">String[] of querystring values</param>
        /// <param name="testable">Does the endpoint have a test api</param>
        /// <returns>String of url</returns>
        private string CreateUrl(string apiPath, bool secure = true, string[] queryString = null, bool testable = false)
        {
            var qsValues = string.Empty;
            var url = string.Empty;
            if (queryString != null)
            {
                qsValues = string.Join("&", queryString);
            }

            return CreateUrl(apiPath, secure, qsValues, testable);
        }

        /// <summary>
        /// Create a Binance url
        /// </summary>
        /// <param name="apiPath">String of path to endpoint</param>
        /// <param name="secure">Boolean if secure endpoin (default = true)</param>
        /// <param name="queryString">String of querystring values</param>
        /// <param name="testable">Does the endpoint have a test api</param>
        /// <returns>String of url</returns>
        private string CreateUrl(string apiPath, bool secure, string queryString, bool testable = false)
        {
            var url = string.Empty;
            if(testable)
                apiPath = testApi ? $"{apiPath}/test" : apiPath;
            if (!secure)
            {
                url = baseUrl + $"{apiPath}";
                if (queryString != "")
                    url += "?" + queryString;

                return url;
            }

            if (string.IsNullOrEmpty(queryString))
                return baseUrl + apiPath;

            var timestamp = _dtHelper.UTCtoUnixTimeMilliseconds();
            var timeStampQS = $"timestamp={timestamp}";
            queryString = queryString != "" ? $"{queryString}&{timeStampQS}" : $"{timeStampQS}";

            var hmac = security.GetBinanceHMACSignature(queryString, _apiInfo.apiSecret);

            url = baseUrl + $"{apiPath}?{queryString}&signature={hmac}";

            return url;
        }

        /// <summary>
        /// Get signature from dictionary of paramters
        /// </summary>
        /// <param name="parameters">paramters to sign</param>
        /// <returns>String of signature</returns>
        private string GetSignature(Dictionary<string, object> parameters)
        {
            var queryString = StringifyDictionary(parameters);

            return security.GetBinanceHMACSignature(queryString, _apiInfo.apiSecret);
        }

        /// <summary>
        /// Convert dictionary to querystring
        /// </summary>
        /// <param name="parameters">Dictionary to convert</param>
        /// <returns>String of values</returns>
        private string StringifyDictionary(Dictionary<string, object> parameters)
        {
            var qsValues = string.Empty;

            if (parameters != null)
            {
                qsValues = string.Join("&", parameters.Select(p => p.Key + "=" + p.Value));
            }

            return qsValues;
        }

        /// <summary>
        /// Cultural neutral decimal converter
        /// </summary>
        /// <param name="decimalVal">Decimal to convert</param>
        /// <returns>String of decimal</returns>
        private string DecimalToString(decimal decimalVal)
        {
            var decimalString = decimalVal.ToString();

            return decimalString.Replace(",", ".");
        }

        /// <summary>
        /// Validate trade parameters
        /// </summary>
        /// <param name="tradeParams">TradeParams object to validate</param>
        /// <returns>True if valid</returns>
        private bool TradeParamsValidator(TradeParams tradeParams)
        {
            var errorMessage = string.Empty;
            if (string.IsNullOrEmpty(tradeParams.symbol))
                errorMessage = "Trading pair required.";

            if (tradeParams.quantity == 0.0M)
                errorMessage = "Quantity is required.";

            if (string.IsNullOrEmpty(tradeParams.side))
                errorMessage = "Trade side is required.";

            if (string.IsNullOrEmpty(tradeParams.type))
                errorMessage = "Trade Type is required.";

            var orderType = (OrderType)Enum.Parse(typeof(OrderType), tradeParams.type);

            if ((orderType == OrderType.LIMIT || orderType == OrderType.STOP_LOSS_LIMIT
                || orderType == OrderType.TAKE_PROFIT_LIMIT || orderType == OrderType.LIMIT_MAKER)
                && tradeParams.price == 0.0M)
                    errorMessage = $"Price is required for {tradeParams.type} orders.";
            
            if ((orderType == OrderType.STOP_LOSS || orderType == OrderType.STOP_LOSS_LIMIT
                || orderType == OrderType.TAKE_PROFIT || orderType == OrderType.TAKE_PROFIT_LIMIT)
                && (tradeParams.stopPrice == 0.0M))
            {
                errorMessage = $"Stop price required for {tradeParams.type} orders";
            }

            if (!string.IsNullOrEmpty(errorMessage))
                throw new Exception(errorMessage);

            return true;
        }
    }
}
