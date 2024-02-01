using MongoDB.Bson;
using MongoDB.Driver;
using Zync.Api.Models.Output;
using Zync.Api.Utilities;

namespace Zync.Api.Middleware
{
    public class MongoConnection
    {
        public string country { get; set; }
        public BsonDocument currentDocument { get; set; }

        public MongoConnection(string country)
        {
            this.country = country;
        }


        public Creative filterCreativeById(string creativeId)
        {
            var configBuilder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();
            var connectionString = configBuilder.GetConnectionString(country);
            MongoClient mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase("creatividades");
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
    }
}
