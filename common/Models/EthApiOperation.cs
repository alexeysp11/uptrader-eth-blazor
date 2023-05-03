namespace UptraderEth.Common.Models 
{
    /// <summary>
    /// 
    /// </summary>
    public class EthApiOperation
    {
        /// <summary>
        /// 
        /// </summary>
        public string AppUid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string MethodName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string WalletAddress { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal WalletBalance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }
    }
}