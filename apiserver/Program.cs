namespace UptraderEth.EthApiServer
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Start API server"); 

            string configFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\apiserver\\appsettings.json"); 
            (new EthApiHttpServer(configFile)).CreateWebServer();
            System.Console.ReadLine();
        }
    }
}
