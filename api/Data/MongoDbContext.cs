using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FullStackPoc.Api.Models;

namespace FullStackPoc.Api.Data;
public sealed class MongoDbSettings { public string ConnectionString { get; set; } = ""; public string DatabaseName { get; set; } = ""; }
public sealed class MongoDbContext
{
    private readonly IMongoDatabase _db;
    public MongoDbContext(IOptions<MongoDbSettings> options) { var c = new MongoClient(options.Value.ConnectionString); _db = c.GetDatabase(options.Value.DatabaseName); }
    public IMongoCollection<User> Users => _db.GetCollection<User>("users");
    public IMongoCollection<Product> Products => _db.GetCollection<Product>("products");
}
