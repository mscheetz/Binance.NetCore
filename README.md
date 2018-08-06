# Binance.NetCore
.Net Core library for accessing the [Binance Exchange](https://www.binance.com) api  
  
This library is available on NuGet for download: https://www.nuget.org/packages/Binance.NetCore  
```
PM> Install-Package Binance.NetCore -Version 1.0.0
```

  
To trade, log into your Binance account and create an api key with trading permissions:  
Account -> API -> Create (with Read Info & Enable Trading)  
Store your API Key & Secret Key  
  
Initialization:  
Non-secured endpoints only:  
```
var binance = new BinanceClient();
```  
  
Secure & non-secure endpoints:  
```
var binance = new BinanceClient("api-key", "api-secret");
```  
or
```
create config file config.json
{
  "apiKey": "api-key",
  "apiSecret": "api-secret"
}
var binance = new BinanceClient("/path-to/config.json");
```

Using an endpoint:  
```  
var binance = await binance.BinanceRepository.GetBalance();
```  
or  
```
var balance = binance.BinanceRepository.GetBalance().Result;
```

Non-secure endpoints:  
Get24HourStats() - Get 24hour stats for one or all trading pairs  
GetCryptos() - Get ticker for all pairs  
GetOrderBook() - Get current order book for a trading pair  
GetCandlestick() - Get charting candlesticks  
GetBinanceTime() - Get binance server unix time  

Secure endpoints:  
GetBalance() - Get current asset balances  
GetOrder() - Get information for an order  
GetOrders() - Get all current user order information  
GetOpenOrders() - Get all current user open orders   
GetTransactions() - Get all transactions for account  
PostTrade() - Post a new trade  
DeleteTrade() - Delete a current open trade  

BNB:  
0xdd061d572e94bb18b6b6ad7fec83d03225a200eb  
ETH:  
0x3c8e741c0a2Cb4b8d5cBB1ead482CFDF87FDd66F  
BTC:  
1MGLPvTzxK9argeNRTHJ9EZ3WtGZV6nxit  
XLM:  
GA6JNJRSTBV54W3EGWDAWKPEGGD3QCXIGEHMQE2TUYXUKKTNKLYWEXVV  
