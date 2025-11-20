using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace cl_be.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int ProductId { get; set; }

        public int CustomerId { get; set; }
        public double Rating { get; set; }
        public string Text { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
