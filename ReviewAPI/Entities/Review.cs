using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReviewAPI.Entities
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string ArticleId { get; set; } = null!;
        public string Reviewer { get; set; } = null!;
        public string ReviewerContent { get; set; } = null!;
        public string Article { get; set; } = null!;
    }
}
