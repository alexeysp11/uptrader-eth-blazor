using UptraderEth.Common.Models; 
using Microsoft.Extensions.Configuration; 

namespace UptraderEth.Common 
{
    public class Configurator 
    {
        /// <summary>
        /// Gets settings for server from config file 
        /// </summary>
        public T GetConfigSettings<T>(string filename, string sectionName)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(filename)
                .AddEnvironmentVariables()
                .Build();
            return config.GetSection(sectionName).Get<T>();
        }
    }
}