using System.Security.Authentication;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Portfolio.Api.MongoDb;

internal class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbOptions> mongoDbOptions)
    {
        var mongoConfig = mongoDbOptions.Value;
        var clientSettings = MongoClientSettings.FromConnectionString(mongoConfig.ConnectionString);
        
        clientSettings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };

        var client = new MongoClient(clientSettings);
        _database = client.GetDatabase(mongoConfig.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
        => _database.GetCollection<T>(collectionName);
}