using LeadTrackApi.Domain.Models;
using LeadTrackApi.Persistence.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LeadTrackApi.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<User> _userCollection;
        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<User>> GetAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync();
        }

        public async Task<User> AddUser(string email, string pass)
        {
            var newUser = new User { Email = email, Password = pass };
            await _userCollection.InsertOneAsync(newUser);
            return newUser;
        }


        public async Task<User> Login(string email, string pass)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email) & Builders<User>.Filter.Eq(u => u.Password, pass);
            return await _userCollection.Find(filter).FirstOrDefaultAsync();
        }


    }
}
