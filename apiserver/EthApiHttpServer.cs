using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json; 
using System.Threading;
using System.Threading.Tasks;
using UptraderEth.Common.Models; 

namespace UptraderEth.EthApiServer
{
    public class EthApiHttpServer
    {
        #region Private properties
        /// <summary>
        /// Confidurations related to the core server
        /// </summary>
        private EthApiServerSettings Settings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private EthNodeCommunication EthNodeCommunication { get; } 

        /// <summary>
        /// Web site's file system delimiter characters 
        /// </summary>
        private char[] WebSiteFSDelimiterChars { get; } = { '/', '\\' }; 
        /// <summary>
        /// Allowed web site's paths 
        /// </summary>
        private List<string> WebPaths = new List<string>();
        #endregion  // Private properties

        #region Constructors
        /// <summary>
        /// Default constructor 
        /// </summary>
        public EthApiHttpServer(string configFile)
        {
            var configurator = new UptraderEth.Common.Configurator(); 
            Settings = configurator.GetConfigSettings<EthApiServerSettings>(configFile, "EthApiServerSettings"); 

            EthNodeCommunication = new EthNodeCommunication(Settings.Environment); 

            AddWebPaths(); 
        }
        #endregion  // Constructors

        #region Public methods
        /// <summary>
        /// Create web server as HttpListener
        /// </summary>
        public void CreateWebServer()
        {
            // Start HttpListener 
            HttpListener listener = new HttpListener();
            AddPrefixes(listener); 
            listener.Start();

            // 
            System.Console.WriteLine("Start to listen...");
            System.Console.WriteLine("ServerAddress: " + Settings.ServerAddress);
            System.Console.WriteLine("Environment: " + Settings.Environment.ToLower()); 

            // Start the thread 
            new Thread(() => {
                while (true)
                {
                    HttpListenerContext ctx = listener.GetContext();
                    ThreadPool.QueueUserWorkItem((_) => ProcessRequest(ctx));
                }
            }).Start();
        }
        #endregion  // Public methods

        #region Request processing 
        /// <summary>
        /// Processes request and sends response back 
        /// </summary>
        /// <param name="ctx"></param>
        private void ProcessRequest(HttpListenerContext ctx)
        {
            try
            {
                // Decode request 
                string url = ctx.Request.Url.ToString(); 
                string body = (new System.IO.StreamReader(ctx.Request.InputStream, ctx.Request.ContentEncoding)).ReadToEnd(); 

                // 
                System.Console.WriteLine("body: " + body); 
                System.Console.WriteLine(ctx.Response.StatusCode + " " + ctx.Response.StatusDescription + ": " + ctx.Request.Url);

                // Create response body 
                string responseText = GetResponseText(url, body); 
                byte[] buf = Encoding.UTF8.GetBytes(responseText);

                // Envelope response 
                ctx.Response.ContentEncoding = Encoding.UTF8;
                ctx.Response.ContentType = "text/html";
                ctx.Response.ContentLength64 = buf.Length;
                ctx.Response.OutputStream.Write(buf, 0, buf.Length);

                ctx.Response.Close();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex); 
            }
        }

        /// <summary>
        /// Gets response text depending on a given URL 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetResponseText(string url, string body)
        {
            // Check if path is valid 
            if (!IsPathValid(url)) return "Path is not valid"; 

            // Process product path request 
            if (url.Contains("/p/")) return ProcessProd(url, body); 

            // Process debug path request 
            if (Settings.Environment.ToLower() == "test") 
            {
                foreach (string path in Settings.HttpPathsDbg) 
                    if (url.Contains(path)) return ProcessDbg(url);
            }
            
            return "Page is not found";
        }

        /// <summary>
        /// 
        /// </summary>
        private string ProcessProd(string url, string body)
        {
            // 
            bool status = false; 
            string response = string.Empty; 
            
            // Process request 
            EthApiOperation operation = JsonSerializer.Deserialize<EthApiOperation>(body);
            operation.Status = GetStatusString(true); 
            Task task = Task.Run(async () => {
                operation.WalletBalance = await EthNodeCommunication.GetBalanceAsync(operation.WalletAddress); 
            }); 
            task.Wait(); 
            return System.Text.Json.JsonSerializer.Serialize(operation); 
        }

        /// <summary>
        /// 
        /// </summary>
        private string ProcessDbg(string url)
        {
            return "ProcessDbg"; 
        }

        /// <summary>
        /// 
        /// </summary>
        private string GetStatusString(bool status)
        {
            return status ? "SUCCESS" : "FAILED";
        }
        #endregion  // Request processing 

        #region Web site's folder structure
        /// <summary>
        /// Adds all the possible paths into WebPaths dictionary 
        /// </summary>
        private void AddWebPaths()
        {
            // Add production path
            WebPaths.Add("/p/");

            // Add dbg paths if necessary
            if (Settings.Environment.ToLower() == "test") 
            { 
                foreach (string path in Settings.HttpPathsDbg) 
                    WebPaths.Add(path);
            }

            // Print dbg paths if necessary
            if (Settings.PrintWebPaths) 
            {
                foreach (string path in WebPaths) System.Console.WriteLine("webpath: " + path); 
            }
        }

        /// <summary>
        /// Adds all the possible paths from WebPaths dictionary into HttpListener.Prefixes 
        /// </summary>
        /// <param name="listener"></param>
        private void AddPrefixes(HttpListener listener)
        {
            string purePath = string.Empty; 
            foreach (string path in WebPaths)
            {
                purePath = path.TrimStart(WebSiteFSDelimiterChars).TrimEnd(WebSiteFSDelimiterChars); 
                if (!string.IsNullOrEmpty(purePath)) listener.Prefixes.Add(Settings.ServerAddress + purePath + "/");
            }
        }

        /// <summary>
        /// Checks if web site path is valid 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsPathValid(string url)
        {
            string pureUrl = url.TrimStart(WebSiteFSDelimiterChars).TrimEnd(WebSiteFSDelimiterChars); 
            string purePath = string.Empty; 
            foreach (string path in WebPaths) 
            {
                purePath = path.TrimStart(WebSiteFSDelimiterChars).TrimEnd(WebSiteFSDelimiterChars); 
                if (!string.IsNullOrEmpty(purePath) && pureUrl.Contains(purePath)) return true; 
            }
            return false;
        }
        #endregion  // Web site's folder structure
    }
}
