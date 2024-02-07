using MongoDB.Bson;
using MongoDB.Driver;
using System.Numerics;
using Zync.Api.Models.Output;
using Zync.Api.Utilities;

namespace Zync.Api.Middleware
{
    public class MongoConnection
    {
        public string country { get; set; }
        public MongoClient mongoClient = new MongoClient();
        public BsonDocument currentDocument { get; set; }

        public MongoConnection(string country)
        {
            this.country = country;
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
            var connectionString = configBuilder.GetConnectionString(country);
            this.mongoClient = new MongoClient(connectionString);
        }


        public Creative filterCreativeById(string creativeId)
        {

            var mongoDatabase = this.mongoClient.GetDatabase("creatividades");
            var mongoCollection = mongoDatabase.GetCollection<BsonDocument>("creativities");
            var objectIdToFind = creativeId.All(c => char.IsDigit(c) || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F') ? new ObjectId(creativeId) : new ObjectId();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectIdToFind);
            var results = mongoCollection.Find(filter).ToList();
            var foundDocument = new BsonDocument();
            Creative creative = new Creative();
            if (results.Count == 0)
            {
                foundDocument["_id"] = new ObjectId();
                foundDocument["_result"] = "No creatives found";
            }
            else
            {
                foundDocument = mongoCollection.Find(filter).FirstOrDefault();
                DataBuilder builder = new DataBuilder(foundDocument);
                creative = builder.buildCreative();
                foundDocument["_result"] = "1 creative found";
            }
            return creative;
        }

        public Campaign filterCampaignById(string campaignId)
        {
            var mongoDatabase = this.mongoClient.GetDatabase("creatividades");
            var mongoCollection = mongoDatabase.GetCollection<BsonDocument>("campaigns");
            var objectIdToFind = campaignId.All(c => char.IsDigit(c) || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F') ? new ObjectId(campaignId) : new ObjectId();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectIdToFind);
            var results = mongoCollection.Find(filter).ToList();
            var foundDocument = new BsonDocument();
            Campaign campaign = new Campaign();
            if (results.Count == 0)
            {
                foundDocument["_id"] = new ObjectId();
                foundDocument["_result"] = "No creatives found";
            }
            else
            {
                foundDocument = mongoCollection.Find(filter).FirstOrDefault();
                DataBuilder builder = new DataBuilder(foundDocument);
                campaign = builder.buildCampaign();
                foundDocument["_result"] = "1 creative found";
            }
            return campaign;
        }
        public Format filterFormatById(string formatId)
        {
            var mongoDatabase = this.mongoClient.GetDatabase("creatividades");
            var mongoCollection = mongoDatabase.GetCollection<BsonDocument>("formats");
            var objectIdToFind = formatId.All(c => char.IsDigit(c) || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F') ? new ObjectId(formatId) : new ObjectId();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectIdToFind);
            var results = mongoCollection.Find(filter).ToList();
            var foundDocument = new BsonDocument();
            Format format = new Format();
            if (results.Count == 0)
            {
                foundDocument["_id"] = new ObjectId();
                foundDocument["_result"] = "No creatives found";
            }
            else
            {
                foundDocument = mongoCollection.Find(filter).FirstOrDefault();
                DataBuilder builder = new DataBuilder(foundDocument);
                format = builder.buildFormat();
                foundDocument["_result"] = "1 creative found";
            }
            return format;
        }
        public Player filterPlayerById(string playerId)
        {
            var mongoDatabase = this.mongoClient.GetDatabase("creatividades");
            var mongoCollection = mongoDatabase.GetCollection<BsonDocument>("players");
            var objectIdToFind = playerId.All(c => char.IsDigit(c) || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F') ? new ObjectId(playerId) : new ObjectId();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectIdToFind);
            var results = mongoCollection.Find(filter).ToList();
            var foundDocument = new BsonDocument();
            Player player = new Player();
            if (results.Count == 0)
            {
                foundDocument["_id"] = new ObjectId();
                foundDocument["_result"] = "No creatives found";
            }
            else
            {
                foundDocument = mongoCollection.Find(filter).FirstOrDefault();
                DataBuilder builder = new DataBuilder(foundDocument);
                player = builder.buildPlayer();
                foundDocument["_result"] = "1 creative found";
            }
            return player;
        }

        public Players getAllPlayers()
        {
            var mongoDatabase = this.mongoClient.GetDatabase("creatividades");
            var mongoCollection = mongoDatabase.GetCollection<BsonDocument>("players");
            var filter = Builders<BsonDocument>.Filter.Ne("_id", "");
            //var results = mongoCollection.Find(FilterDefinition<BsonDocument>.Empty).ToList();
            var results = mongoCollection.Find(filter).ToList();
            Console.WriteLine(results.Count);

            foreach (BsonDocument document in results)
            {
                Console.WriteLine(document.GetElement("name").Value.ToString());
            }


            //var foundDocument = new BsonDocument();
            Players playersList = new Players();

            /*
            if (results.Count == 0)
            {
                //foundDocument["_id"] = new ObjectId();
                //foundDocument["_result"] = "No creatives found";
            }
            else
            {
                //foundDocument = mongoCollection.Find(filter).FirstOrDefault();
                DataBuilder builder = new DataBuilder(results);
                playersList = builder.buildPlayerList();
                //foundDocument["_result"] = "1 creative found";
            }
            */
            return playersList;
        }
    }
}
