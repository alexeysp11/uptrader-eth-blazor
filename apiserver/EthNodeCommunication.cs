using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Nethereum.Web3; 

namespace UptraderEth.EthApiServer
{
    public class EthNodeCommunication
    {
        public Task<string> GetBalanceAsync(string address)
        {
            // ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            // var web3 = new Web3("https://ropsten.infura.io"); 
            // var balance = web3.Eth.GetBalance.SendRequestAsync(wallets[i].Address).Result; 
            // decimal ethAmount = Web3.Convert.FromWei(balance.Value);
            decimal ethAmount = 0m; 
            Task task = Task.Run(() => {
                // var balance = await web3.Eth.GetBalance.SendRequestAsync(wallets[i].Address);
                // ethAmount = Web3.Convert.FromWei(balance.Value); 
                ethAmount = (new System.Random()).Next(0, 100); 
                // System.Threading.Thread.Sleep(id % 10 == 0 ? 1000 : 0); 
                System.Threading.Thread.Sleep(1000); 
                System.Console.WriteLine($"ethAmount: {ethAmount}"); 
            });
            task.Wait();
            return Task.FromResult($"{ethAmount} ETH"); 
        }
    }
}