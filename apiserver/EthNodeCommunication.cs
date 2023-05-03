using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Nethereum.Web3; 

namespace UptraderEth.EthApiServer
{
    /// <summary>
    /// Communication with ETH node (or imition of it)
    /// </summary>
    public class EthNodeCommunication
    {
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
        public EthNodeCommunication()
        {
            UseEthConnection = true; 
            Environment = "production"; 
        }
        /// <summary>
        /// Constructor of EthNodeCommunication 
        /// </summary>
        public EthNodeCommunication(bool useEthConnection, string environment)
        {
            UseEthConnection = useEthConnection; 
            Environment = environment; 
        }

        /// <summary>
        /// Allows to get balance of a wallet 
        /// </summary>
        public Task<decimal> GetBalanceAsync(string address)
        {
            if (string.IsNullOrEmpty(address)) return Task.FromResult(0m); 

            decimal ethAmount = 0m; 
            Task task = Task.Run(async () => {
                if (UseEthConnection && Environment == "production")
                {
                    // ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

                    var web3 = new Web3("https://ropsten.infura.io"); 
                    var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
                    ethAmount = Web3.Convert.FromWei(balance.Value); 
                }
                else 
                {
                    ethAmount = (new System.Random()).Next(0, 100); 
                    System.Threading.Thread.Sleep(1000); 
                }
                System.Console.WriteLine($"ethAmount: {ethAmount}"); 
            });
            task.Wait();
            return Task.FromResult(ethAmount); 
        }
    }
}