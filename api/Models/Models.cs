using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FullStackPoc.Api.Models;
public sealed class User { [BsonId] [BsonRepresentation(BsonType.ObjectId)] public string? Id { get; set; } public string Email { get; set; } = ""; public string PasswordHash { get; set; } = ""; public string Name { get; set; } = ""; }
public sealed class Product { [BsonId] [BsonRepresentation(BsonType.ObjectId)] public string? Id { get; set; } public string Name { get; set; } = ""; public decimal Price { get; set; } public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }
