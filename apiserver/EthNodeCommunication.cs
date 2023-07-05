using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Nethereum.Web3; 
using UptraderEth.Common; 

namespace UptraderEth.EthApiServer
{
    /// <summary>
    /// Communication with ETH node (or imition of it)
    /// </summary>
    public class EthNodeCommunication
    {
        /// <summary>
        /// 
        /// </summary>
        private WalletCaching WalletCaching; 
        /// <summary>
        /// Address of the API node for retrieving ETH data 
        /// </summary>
        private string EthConnectionAddress { get; set; }
        /// <summary>
        /// Shows if connection with ETH node should be established or just imitated 
        /// </summary>
        private bool UseEthConnection { get; set; }
        /// <summary>
        /// Shows if the application is running in production or test mode. 
        /// If it is running in test mode, communication with ETH node will be imitated
        /// </summary>
        private string Environment { get; set; }

        /// <summary>
        /// Constructor of EthNodeCommunication
        /// </summary>
        public EthNodeCommunication() : this("", true, "production")
        {
        }
        /// <summary>
        /// Constructor of EthNodeCommunication 
        /// </summary>
        public EthNodeCommunication(string ethConnectionAddress, bool useEthConnection, string environment)
        {
            EthConnectionAddress = ethConnectionAddress; 
            UseEthConnection = useEthConnection; 
            Environment = environment; 

            WalletCaching = new WalletCaching("mongodb://127.0.0.1:27017", "uptrader_eth_blazor", "wallets_cache_apiserver"); 
        }

        /// <summary>
        /// Allows to get balance of a wallet 
        /// </summary>
        public Task<decimal> GetBalanceAsync(string address)
        {
            if (string.IsNullOrEmpty(address)) return Task.FromResult(0m); 

            decimal ethAmount = 0m; 
            Task task = Task.Run(async () => {
                // Get data from cache 
                string balanceCache = this.WalletCaching.GetBalanceFromCache(address); 
                bool converted = decimal.TryParse(balanceCache, out ethAmount); 

                // Get request if necessary 
                if (string.IsNullOrEmpty(balanceCache) && !converted) 
                {    
                    if (UseEthConnection && !string.IsNullOrEmpty(EthConnectionAddress))
                    {
                        // ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

                        var web3 = new Web3(EthConnectionAddress); 
                        var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
                        ethAmount = Web3.Convert.FromWei(balance.Value); 
                    }
                    else 
                    {
                        ethAmount = (new System.Random()).Next(0, 100); 
                        System.Threading.Thread.Sleep(1000); 
                    }
                    this.WalletCaching.InsertBalanceToCache(address, ethAmount.ToString()); 
                }
                System.Console.WriteLine($"ethAmount: {ethAmount}"); 
            });
            task.Wait();
            return Task.FromResult(ethAmount); 
        }
    }
}