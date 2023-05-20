using System.Collections.Generic; 
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UptraderEth.Common.Models; 

namespace UptraderEthBlazor.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class WalletService
    {
        private string AppUid; 
        private string ApiServerAddress;
        public bool UsePlaceholders { get; private set; } 
        
        public WalletService(string appUid, string apiServerAddress, bool usePlaceholders)
        {
            AppUid = appUid; 
            ApiServerAddress = apiServerAddress; 
            UsePlaceholders = usePlaceholders; 
        }

        /// <summary>
        /// Retrieves an array of wallets from the database 
        /// </summary> 
        public Task<UptraderEth.Common.Models.Wallet[]> GetWalletsAsync()
        {
            using var context = new WalletContext();
            var wallets = context.Wallets.ToArray(); 
            var result = new UptraderEth.Common.Models.Wallet[wallets.Length]; 
            for (int i = 0; i < wallets.Length; i++)
            {
                result[i] = new UptraderEth.Common.Models.Wallet(); 
                result[i].Id = wallets[i].Id; 
                result[i].Address = wallets[i].Address; 
                result[i].Balance = "Loading...";
            }
            return Task.FromResult(result);
        }

        /// <summary>
        /// Retrieves a balance of the wallet from API server 
        /// </summary>
        public Task<string> GetBalanceAsync(string address)
        {
            if (string.IsNullOrEmpty(address)) return Task.FromResult("N/D"); 
            
            decimal ethAmount = 0m; 
            Task task = Task.Run(async () => {
                // Send request to the API server 
                var values = new Dictionary<string, string>
                {
                    { "AppUid", AppUid },
                    { "MethodName", "getbalance" },
                    { "WalletAddress", address }
                };
                string response = EthHttpClient.Post(ApiServerAddress, values); 

                // Parse response 
                EthApiOperation operation = System.Text.Json.JsonSerializer.Deserialize<EthApiOperation>(response);
                ethAmount = operation.WalletBalance; 
            });
            task.Wait();
            return Task.FromResult($"{ethAmount} ETH"); 
        }
    }
}
