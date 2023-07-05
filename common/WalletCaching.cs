using System.Collections.Generic;
using System.Linq; 
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UptraderEth.Common
{
    public class WalletCaching 
    {
        private string ConnectionString { get; } = "mongodb://127.0.0.1:27017"; 
        private string DbName { get; } = "uptrader_eth_blazor"; 
        private string WalletCollection { get; } = "wallets_cache"; 

        public WalletCaching(string connectionString, string dbName, string walletCollection)
        {
            ConnectionString = connectionString; 
            DbName = dbName; 
            WalletCollection = walletCollection; 
        }

        private class WalletCache
        {
            public ObjectId Id { get; set; }

            [BsonElement("address")]
            public string Address { get; set; }

            [BsonElement("balance_eth")]
            public string BalanceEth { get; set; }

            [BsonElement("update_datetime")]
            public string UpdateDateTime { get; set; }
        }

        public string GetBalanceFromCache(string address)
        {
            string result = string.Empty; 
            try
            {
                var client = new MongoClient(ConnectionString);
                var walletCollection = client.GetDatabase(DbName).GetCollection<WalletCache>(WalletCollection);
                var filter = Builders<WalletCache>.Filter.Eq("address", address);
                var wallets = walletCollection.Find(filter).ToList();
                return wallets.Count == 0 ? result : wallets.First().BalanceEth; 
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"ex: {ex.Message}"); 
            }
            return result; 
        }

        public void InsertBalanceToCache(string address, string balanceEth)
        {
            var client = new MongoClient(ConnectionString);
            var walletCollection = client.GetDatabase(DbName).GetCollection<WalletCache>(WalletCollection);
            if (GetBalanceFromCache(address) == balanceEth) 
                return; 
            walletCollection.InsertOne(new WalletCache 
            {
                Address = address, BalanceEth = balanceEth, UpdateDateTime = System.DateTime.Now.ToString()
            });
        }
    }
}
