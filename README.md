# uptrader-eth-blazor 

Blazor project that includes the page `Wallets`. 

On the page `Wallets` there is table with columns: `Id`, `Address`, `Balance` (data could be sorted by balance).  

## Requirements 

- `Netehereum.Web3`; 
- The page should work fast; 
- Balance is not stored in the database, and they could be retrieved from ETH node; 
- ETH testnet Sepolia ([alchemy](https://www.alchemy.com/) и [infura](https://www.infura.io/) are recomended); 
- Communication with the node should be implemented as a separate API service.  

## Code snippets 

Wallet is implemented as follows: 

```C#
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UptraderEthBlazor.Data
{
    /// <summary>
    /// Class that is mapped against database table which contains info about wallets
    /// </summary>
    [Table("Wallets")]
    public class Wallet
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
    }
}
```

<!-- 
```
dotnet tool install --global dotnet-ef --version 3.1.0 
dotnet ef dbcontext scaffold "Host=localhost;Database=postgres;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL
``` 
-->

## API 

Functionality: 

- Gets JSON in a form: 
```JSON
{
    "AppUid": "appuid632rbAbB325ao234", 
    "MethodName": "getbalance", 
    "WalletAddresses": [
        "0xE276Bc378A527A8792B353cdCA5b5E53263DfB9e",
        "0xb21c33DE1FAb3FA15499c62B59fe0cC3250020d1",
        "0xd7d76c58b3a519e9fA6Cc4D22dC017259BC49F1E",
        "0x25c4a76E7d118705e7Ea2e9b7d8C59930d8aCD3b",
        "0x10F5d45854e038071485AC9e402308cF80D2d2fE"
    ]
}
```

- Sends requests to ETH, or imitates it 

- Returns JSON in a form: 
```JSON
{
    "AppUid": "appuid632rbAbB325ao234", 
    "MethodName": "getbalance", 
    "Wallets": [
        {
            "Address": "0xE276Bc378A527A8792B353cdCA5b5E53263DfB9e",
            "Balance": "0.36452 ETH"
        }, 
        {
            "Address": "0xb21c33DE1FAb3FA15499c62B59fe0cC3250020d1",
            "Balance": "1.36452 ETH"
        }, 
        {
            "Address": "0xd7d76c58b3a519e9fA6Cc4D22dC017259BC49F1E",
            "Balance": "0.98452 ETH"
        }, 
        {
            "Address": "0x25c4a76E7d118705e7Ea2e9b7d8C59930d8aCD3b",
            "Balance": "2.20452 ETH"
        }, 
        {
            "Address": "0x10F5d45854e038071485AC9e402308cF80D2d2fE",
            "Balance": "0.65452 ETH"
        }
    ]
}
```