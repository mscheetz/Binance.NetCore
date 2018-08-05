using Binance.NetCore.Data;
using Binance.NetCore.Data.Interface;

namespace Binance.NetCore
{
    public class BinanceApiClient
    {
        public IBinanceRepository BinanceRepository;

        /// <summary>
        /// Constructor - no authentication
        /// </summary>
        public BinanceApiClient()
        {
            BinanceRepository = new BinanceRepository();
        }

        /// <summary>
        /// Constructor - with authentication
        /// </summary>
        /// <param name="apiKey">Api key</param>
        /// <param name="apiSecret">Api secret</param>
        public BinanceApiClient(string apiKey, string apiSecret)
        {
            BinanceRepository = new BinanceRepository(apiKey, apiSecret);
        }

        /// <summary>
        /// Constructor - with authentication
        /// </summary>
        /// <param name="configPath">Path to config file</param>
        public BinanceApiClient(string configPath)
        {
            BinanceRepository = new BinanceRepository(configPath);
        }
    }
}
