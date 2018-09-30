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
                _repo = new BinanceRepository(_exchangeApi.apiKey, _exchangeApi.apiSecret);
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
    }
}