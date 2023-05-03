# Issues 

1. When trying to connect to the ETH node, there're 

I'm trying to send request to the ETH node the following way: 
```C#
// ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

var web3 = new Web3("https://ropsten.infura.io"); 
var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
ethAmount = Web3.Convert.FromWei(balance.Value); 
```

But I get errors: 
```
System.AggregateException: One or more errors occurred. (One or more errors occurred. (Error occurred when trying to send rpc requests(s): eth_getBalance))
 ---> System.AggregateException: One or more errors occurred. (Error occurred when trying to send rpc requests(s): eth_getBalance)
 ---> Nethereum.JsonRpc.Client.RpcClientUnknownException: Error occurred when trying to send rpc requests(s): eth_getBalance
 ---> System.Net.Http.HttpRequestException: Response status code does not indicate success: 410 (Gone).
   at System.Net.Http.HttpResponseMessage.EnsureSuccessStatusCode()
   at Nethereum.JsonRpc.Client.RpcClient.SendAsync(RpcRequestMessage request, String route)
```

According to the answers [here](https://ethereum.stackexchange.com/questions/50482/error-occurred-when-trying-to-send-rpc-requestss) and info [here](https://docs.infura.io/infura/getting-started), I probably need to sign up to Infura. 
Is there any other ways to solve the issue without having to sign up? 
