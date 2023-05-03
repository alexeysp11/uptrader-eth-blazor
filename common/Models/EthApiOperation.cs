namespace UptraderEth.Common.Models 
{
    /// <summary>
    /// Represents any attempt to communcate with API server as an operation 
    /// </summary>
    public class EthApiOperation
    {
        /// <summary>
        /// Application UID (used for validating clients)
        /// </summary>
        public string AppUid { get; set; }
        
        /// <summary>
        /// Name of the method that should be executed to perform the operation 
        /// </summary>
        public string MethodName { get; set; }
        
        /// <summary>
        /// Address of a wallet 
        /// </summary>
        public string WalletAddress { get; set; }
        
        /// <summary>
        /// Balance acquired from the ETH node by wallet address 
        /// </summary>
        public decimal WalletBalance { get; set; }

        /// <summary>
        /// Status, or result, of the operation 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Additional information about the status of the operation 
        /// </summary>
        public string StatusDescription { get; set; }
    }
}