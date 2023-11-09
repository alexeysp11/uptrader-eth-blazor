using System.Collections.Generic;
using System.Linq; 
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UptraderEth.Common
{
    /// <summary>
    /// Caching data associated with wallets.
    /// </summary>
    public class WalletCaching 
    {
        /// <summary>
        /// Cache model for wallet in database.
        /// </summary>
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

        /// <summary>
        /// Mongo client.
        /// </summary>
        private MongoClient Client; 
        /// <summary>
        /// Connection string.
        /// </summary>
        private string ConnectionString { get; } 
        /// <summary>
        /// Database name.
        /// </summary>
        private string DbName { get; } 
        /// <summary>
        /// Name of the wallet collection.
        /// </summary>
        private string WalletCollection { get; } 

        /// <summary>
        /// Construstor.
        /// </summary>
        public WalletCaching(string connectionString, string dbName, string walletCollection)
        {
            ConnectionString = connectionString; 
            DbName = dbName; 
            WalletCollection = walletCollection; 

            Client = new MongoClient(ConnectionString); 
        }

        /// <summary>
        /// Method for getting balance from cache.
        /// </summary>
        public string GetBalanceFromCache(string address)
        {
            string result = string.Empty; 
            try
            {
                var walletCollection = Client.GetDatabase(DbName).GetCollection<WalletCache>(WalletCollection);
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

        /// <summary>
        /// Method for saving balance in the cache.
        /// </summary>
        public void InsertBalanceToCache(string address, string balanceEth)
        {
            // Check balance for the address and its last update 
            if (GetBalanceFromCache(address) == balanceEth) 
                return; 
            
            // 
            var walletCollection = Client.GetDatabase(DbName).GetCollection<WalletCache>(WalletCollection);

            // Check if there's a wallet in the database 
            // If wallet exists, update it 
            // If wallet does not exist, insert it 
            walletCollection.InsertOne(new WalletCache 
            {
                Address = address, 
                BalanceEth = balanceEth, 
                UpdateDateTime = System.DateTime.Now.ToString()
            });
        }
    }
}
