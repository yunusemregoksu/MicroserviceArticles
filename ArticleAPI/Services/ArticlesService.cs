using ArticleAPI.Entities;
using ArticleAPI.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ArticleAPI.Services
{
    public class ArticlesService
    {
        private readonly IMongoCollection<Article> _articlesCollection;

        public ArticlesService(
            IOptions<ArticleDatabaseSettings> articleDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                articleDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                articleDatabaseSettings.Value.DatabaseName);

            _articlesCollection = mongoDatabase.GetCollection<Article>(
                articleDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Article>> GetAsync() =>
            await _articlesCollection.Find(_ => true).ToListAsync();

        public async Task<Article?> GetAsync(string id) =>
            await _articlesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public IQueryable<Article> AsQueryable() =>
            _articlesCollection.AsQueryable();

        public async Task CreateAsync(Article newArticle) =>
            await _articlesCollection.InsertOneAsync(newArticle);

        public async Task UpdateAsync(string id, Article updatedArticle) =>
            await _articlesCollection.ReplaceOneAsync(x => x.Id == id, updatedArticle);

        public async Task RemoveAsync(string id) =>
            await _articlesCollection.DeleteOneAsync(x => x.Id == id);
    }
}
