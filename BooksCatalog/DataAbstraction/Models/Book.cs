using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BooksCatalog
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;

        public string Category { get; set; } = "Other";

        public string Author { get; set; } = null!;


        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
