# Binance.NetCore
.Net Core library for accessing the [Binance Exchange](https://www.binance.com/?ref=12866217) api  
  
This library is available on NuGet for download: https://www.nuget.org/packages/Binance.NetCore  
```
PM> Install-Package Binance.NetCore
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
var binance = await binance.GetBalanceAsync();
```  
or  
```
var balance = binance.GetBalance();
```

Non-secure endpoints:  
Get24HourStats() | Get24HourStatsAsync() - Get 24hour stats for one or all trading pairs  
GetCryptos() | GetCryptosAsync() - Get ticker for all pairs  
GetOrderBook() | GetOrderBookAsync() - Get current order book for a trading pair  
GetCandlestick() | GetCandlestickAsync() - Get charting candlesticks  
Get24HourStats() | Get24HourStatsAsync() - Get 24hour stats for one or all trading pairs  
GetBinanceTime() - Get binance server unix time  

Secure endpoints:  
GetBalance() | GetBalanceAsync() - Get current asset balances  
GetDepositAddress() | GetDepositAddressAsync() - Get deposit address  
GetDepositHistory() | GetDepositHistoryAsync() - Get deposit history  
GetOrder() | GetOrderAsync() - Get information for an order  
GetOrders() | GetOrdersAsync() - Get all current user order information  
GetOpenOrders()  | GetOpenOrdersAsync() - Get all current user open orders  
GetTransactions() | GetTransactionsAsync() - Get all transactions for account  
GetWithdrawalHistory() | GetWithdrawalHistoryAsync() - Get withdrawal history  
DeleteTrade() | DeleteTradeAsync() - Delete a current open trade  
LimitOrder() | LimitOrderAsync() - Place Limit order  
LimitMakerOrder() | LimitMakerOrderAsync() - Place Limit Maker order  
MarketOrder() | MarketOrderAsync() - Place Market order  
StopLoss() | StopLossAsync() - Place Stop Loss order  
StopLossLimit() | StopLossLimitAsync() - Place Stop Loss Limit order  
TakeProfit() | TakeProfitAsync() - Place Take Profit order  
TakeProfitLimit() | TakeProfitLimitAsync() - Place Take Profit Limit order  
PostTrade() | PostTradeAsync() - Post a new trade  


BNB:  
0xdd061d572e94bb18b6b6ad7fec83d03225a200eb  
ETH:  
0x3c8e741c0a2Cb4b8d5cBB1ead482CFDF87FDd66F  
BTC:  
1MGLPvTzxK9argeNRTHJ9EZ3WtGZV6nxit  
XLM:  
GA6JNJRSTBV54W3EGWDAWKPEGGD3QCXIGEHMQE2TUYXUKKTNKLYWEXVV  
