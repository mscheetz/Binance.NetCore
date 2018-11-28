// -----------------------------------------------------------------------------
// <copyright file="BinanceWSRepositoryTests" company="Matt Scheetz">
//     Copyright (c) Matt Scheetz All Rights Reserved
// </copyright>
// <author name="Matt Scheetz" date="11/13/2018 9:11:51 PM" />
// -----------------------------------------------------------------------------

namespace Binance.NetCore.Tests.Tests
{
    #region Usings

    using Binance.NetCore.Data;
    using Binance.NetCore.Data.Interface;
    using Binance.NetCore.Entities;
    using FileRepository;
    using System;
    using System.Linq;
    using Xunit;

    #endregion Usings

    public class BinanceWSRepositoryTests : IDisposable
    {
        #region Properties

        private ApiInformation _exchangeApi = null;
        private IBinanceRepository _repo;
        private string configPath = "config.json";
        private string apiKey = string.Empty;
        private string apiSecret = string.Empty;

        #endregion Properties

        /// <summary>
        /// Constructor for tests
        /// </summary>
        public BinanceWSRepositoryTests()
        {
            IFileRepository _fileRepo = new FileRepository();
            if (_fileRepo.FileExists(configPath))
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

        public void TestWS()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}