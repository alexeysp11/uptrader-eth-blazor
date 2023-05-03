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
        /// 
        /// </summary>
        public bool UseEthConnection { get; set; }
        
        /// <summary>
        /// HTTP paths that could be used for testing and debugging the server
        /// </summary>
        public string[] HttpPathsDbg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool PrintWebPaths { get; set; }
    }
}
