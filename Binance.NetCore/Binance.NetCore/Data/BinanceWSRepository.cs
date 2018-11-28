// -----------------------------------------------------------------------------
// <copyright file="BinanceWSRepository" company="Matt Scheetz">
//     Copyright (c) Matt Scheetz All Rights Reserved
// </copyright>
// <author name="Matt Scheetz" date="11/13/2018 8:49:01 PM" />
// -----------------------------------------------------------------------------

namespace Binance.NetCore.Data
{
    using Binance.NetCore.Core;
    using Binance.NetCore.Data.Interface;
    using Binance.NetCore.Entities;
    using DateTimeHelpers;
    using FileRepository;
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    //using WebSocket4Net;

    #endregion Usings

    public class BinanceWSRepository : IBinanceWSRepository
    {
        private Security security;
        private string baseUrl;
        private ApiInformation _apiInfo = null;
        private DateTimeHelper _dtHelper;
        private bool testApi = false;

        #region Constructor/Destructor

        /// <summary>
        /// Constructor for non-signed endpoints
        /// </summary>
        public BinanceWSRepository()
        {
            LoadRepository();
        }

        /// <summary>
        /// Constructor for signed endpoints
        /// </summary>
        /// <param name="apiKey">Api key</param>
        /// <param name="apiSecret">Api secret</param>
        public BinanceWSRepository(string apiKey, string apiSecret)
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
        public BinanceWSRepository(string configPath)
        {
            IFileRepository _fileRepo = new FileRepository();

            if (_fileRepo.FileExists(configPath))
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
        public BinanceWSRepository(string apiKey, string apiSecret, bool test)
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
        public BinanceWSRepository(string configPath, bool test)
        {
            IFileRepository _fileRepo = new FileRepository();

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

        #endregion Constructor/Destructor

        #region Methods

        /// <summary>
        /// Load repository
        /// </summary>
        /// <param name="key">Api key value (default = "")</param>
        /// <param name="secret">Api secret value (default = "")</param>
        private void LoadRepository(string key = "", string secret = "")
        {
            security = new Security();
            baseUrl = "wss://stream.binance.com:9443";
            _dtHelper = new DateTimeHelper();
        }

        public async Task SocketTest(string symbol, Interval interval)
        {
            var url = baseUrl + $"{symbol}@aggTrade";
            ClientWebSocket socket = new ClientWebSocket();
            Task task = socket.ConnectAsync(new Uri("wss://ws-feed.gdax.com"), CancellationToken.None);
            task.Wait();
            Thread readThread = new Thread(
                delegate (object obj)
                {
                    byte[] recBytes = new byte[1024];
                    while (true)
                    {
                        ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                        Task<WebSocketReceiveResult> receiveAsync = socket.ReceiveAsync(t, CancellationToken.None);
                        receiveAsync.Wait();
                        string jsonString = Encoding.UTF8.GetString(recBytes);
                        Console.Out.WriteLine("jsonString = {0}", jsonString);
                        recBytes = new byte[1024];
                    }

                });
            readThread.Start();
            string json = "{\"product_ids\":[\"btc-usd\"],\"type\":\"subscribe\"}";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
            socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            Console.ReadLine();
        }


        #endregion Methods
    }
}