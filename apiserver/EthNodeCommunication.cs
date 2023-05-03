using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Nethereum.Web3; 

namespace UptraderEth.EthApiServer
{
    public class EthNodeCommunication
    {
        private string Environment { get; set; }

        public EthNodeCommunication()
        {
            Environment = "production"; 
        }
        public EthNodeCommunication(string environment)
        {
            Environment = environment; 
        }

        public Task<decimal> GetBalanceAsync(string address)
        {
            if (string.IsNullOrEmpty(address)) return Task.FromResult(0m); 

            decimal ethAmount = 0m; 
            Task task = Task.Run(async () => {
                if (Environment == "production")
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