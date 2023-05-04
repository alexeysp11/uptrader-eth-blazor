namespace UptraderEth.Common.Models
{
    /// <summary>
    /// Class for storing confidurations related to the core server
    /// </summary>
    public sealed class EthApiServerSettings
    {
        /// <summary>
        /// HTTP address of the core server 
        /// </summary>
        public string ServerAddress { get; set; }
        
        /// <summary>
        /// Environment (test, production)
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Allows to use ETH connection (if this parameter is set to false, imitation of EHT node will be executed)
        /// </summary>
        public bool UseEthConnection { get; set; }

        /// <summary>
        /// Address of the API node for retrieving ETH data 
        /// </summary>
        public string EthConnectionAddress { get; set; }
        
        /// <summary>
        /// HTTP paths that could be used for testing and debugging the server
        /// </summary>
        public string[] HttpPathsDbg { get; set; }

        /// <summary>
        /// Allows to print web paths that API server uses to listen to requests 
        /// </summary>
        public bool PrintWebPaths { get; set; }

        /// <summary>
        /// Allows to print debug information about processing HTTP requests 
        /// </summary>
        public bool PrintHttpRequestProcInfo { get; set; }
    }
}
