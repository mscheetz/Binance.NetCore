using Binance.NetCore.Data;
using Binance.NetCore.Data.Interface;
using Binance.NetCore.Entities;
using FileRepository;
using System;
using System.Linq;
using Xunit;

namespace Binance.NetCore.Tests
{
    public class BinanceRepositoryTests : IDisposable
    {
        private ApiInformation _exchangeApi = null;
        private IBinanceRepository _repo;
        private string configPath = "config.json";
        private string apiKey = string.Empty;
        private string apiSecret = string.Empty;

        /// <summary>
        /// Constructor for tests
        /// </summary>
        public BinanceRepositoryTests()
        {
            IFileRepository _fileRepo = new FileRepository.FileRepository();
            if(_fileRepo.FileExists(configPath))
            {
                _exchangeApi = _fileRepo.GetDataFromFile<ApiInformation>(configPath);
            }
            if (_exchangeApi != null || !string.IsNullOrEmpty(apiKey))
            {
                _repo = new BinanceRepository(_exchangeApi.apiKey, _exchangeApi.apiSecret, true);
            }
            else
            {
                _repo = new BinanceRepository();
            }
        }

        public void Dispose()
        {

        }

        [Fact]
        public void GetExchangeInfo()
        {
            var info = _repo.GetExchangeInfo().Result;

            Assert.NotNull(info);
        }

        [Fact]
        public void GetTradingPairs()
        {
            var pairs = _repo.GetTradingPairs().Result;

            Assert.NotNull(pairs);
        }

        [Fact]
        public void GetTradingPairsByBase()
        {
            var baseSymbol = "BTC";
            var pairs = _repo.GetTradingPairs(baseSymbol).Result;

            Assert.NotNull(pairs);
        }

        [Fact]
        public void GetTradingPairDetail()
        {
            var pair = "NANOBTC";
            var detail = _repo.GetTradingPairDetail(pair).Result;

            Assert.NotNull(detail);
        }

        [Fact]
        public void GetTradingPairDetails()
        {
            var details = _repo.GetTradingPairDetails().Result;

            Assert.NotNull(details);
        }

        [Fact]
        public void GetAccountTest()
        {
            var account = _repo.GetBalance();

            Assert.NotNull(account.Result);
        }

        [Fact]
        public void GetBinanceTimeTest()
        {
            var binanceTime = _repo.GetBinanceTime();

            Assert.True(binanceTime > 0);
        }

        [Fact]
        public void GetBinanceCandlestick()
        {
            var pair = "BTCUSDT";
            var interval = Interval.OneM;

            var candleSticks = _repo.GetCandlestick(pair, interval).Result.ToList();

            Assert.True(candleSticks.Count > 0);
        }

        [Fact]
        public void PostTrade()
        {
            var tradeParams = new TradeParams
            {
                price = 0.00001500M,
                quantity = 1000,
                side = Side.SELL.ToString(),
                symbol = "TRXBTC",
                type = OrderType.LIMIT.ToString()
            };

            var order = _repo.PostTrade(tradeParams).Result;

            Assert.NotNull(order);
        }

        [Fact]
        public void PostStopLossLimit()
        {
            var tradeParams = new TradeParams
            {
                price = 0.00001500M,
                stopPrice = 0.00000150M,
                quantity = 1000,
                side = Side.SELL.ToString(),
                symbol = "TRXBTC",
                type = OrderType.STOP_LOSS_LIMIT.ToString()
            };

            var order = _repo.PostTrade(tradeParams).Result;

            Assert.NotNull(order);
        }

        [Fact]
        public void GetAllOrdersTest()
        {
            var pair = "BTCUSDT";

            var orders = _repo.GetOrders(pair).Result.ToArray();

            Assert.True(orders.Length > 0);
        }

        [Fact]
        public void GetOpenOrdersTest()
        {
            var pair = "BTCUSDT";

            var orders = _repo.GetOpenOrders(pair).Result.ToArray();

            Assert.True(orders.Length > 0);
        }

        [Fact]
        public void GetOrderBookTest()
        {
            var pair = "BTCUSDT";

            var orders = _repo.GetOrderBook(pair).Result;

            Assert.True(orders != null);
        }

        [Fact]
        public void Get24hrBTCUSDTTest()
        {
            var pair = "BTCUSDT";

            var stats = _repo.Get24HourStats(pair).Result;

            Assert.True(stats != null);
        }

        [Fact]
        public void Get24hrTest()
        {
            var stats = _repo.Get24HourStats().Result;

            Assert.True(stats != null);
        }

        [Fact]
        public void GetDepositHistory()
        {
            var asset = "ETH";

            var history = _repo.GetDepositHistory(asset).Result;

            Assert.NotNull(history);
        }

        [Fact]
        public void GetWithdrawalHistory()
        {
            var asset = "ETH";

            var history = _repo.GetWithdrawalHistory(asset).Result;

            Assert.NotNull(history);
        }

        [Fact]
        public void GetDepositAddress()
        {
            var asset = "ETH";

            var address = _repo.GetDepositAddress(asset).Result;

            Assert.NotNull(address);
        }

        [Fact]
        public void WithdrawFundsTest()
        {
            var asset = "XLM";
            var address = "GB6YPGW5JFMMP2QB2USQ33EUWTXVL4ZT5ITUNCY3YKVWOJPP57CANOF3";
            var addressTag = "086b22f1e5604f18a47";
            var amount = 100.0M;

            var withdrawResponse = _repo.WithdrawFunds(asset, address, addressTag, amount).Result;

            Assert.NotNull(withdrawResponse);
            Assert.True(withdrawResponse.success);
        }
    }
}