using CrowdRecipe.Website.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CrowdRecipe.Website.Services
{
    public class RecipesService
    {

        private readonly IMongoCollection<Recipe> recipeCollection;

        public RecipesService(IOptions<RecipeDatabaseSettings> dbSettings) {

            var mClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mDb = mClient.GetDatabase(dbSettings.Value.DatabaseName);
            recipeCollection = mDb.GetCollection<Recipe>(dbSettings.Value.CollectionName);

        }

        public async Task<List<Recipe>> GetAsync() =>
            await recipeCollection.Find(_ => true).ToListAsync();

        public async Task<Recipe?> GetAsync(ObjectId id) =>
            await recipeCollection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task CreateAsync(Recipe recipe) =>
            await recipeCollection.InsertOneAsync(recipe);

        public async Task UpdateAsync(ObjectId id, Recipe recipe) =>
            await recipeCollection.ReplaceOneAsync(x => x.Id.Equals(id), recipe);

        public async Task RemoveAsync(ObjectId id) =>
            await recipeCollection.DeleteOneAsync(x => x.Id.Equals(id));

        public List<Recipe> Get() => recipeCollection.Find(_ => true).ToList();

        public Recipe Get(ObjectId id) => recipeCollection.Find(x => x.Id.Equals(id)).FirstOrDefault();

        public async Task<List<Recipe>> GetByAuthorName(string name) => await recipeCollection.Find(x => x.AuthorName.Equals(name)).ToListAsync();


    }
}
