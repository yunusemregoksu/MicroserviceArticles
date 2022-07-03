using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ArticleAPI.Entities
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Title { get; set; } = null!;
        public string ArticleContent { get; set; } = null!;
        public List<string>? Reviews { get; set; }
    }
}
