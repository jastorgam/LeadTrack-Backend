using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Entities;
using LeadTrackApi.Domain.Enums;
using LeadTrackApi.Domain.Models.Response;
using LeadTrackApi.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LeadTrackApi.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Prospect> _prospectCollection;
        private readonly IMongoCollection<Interacctions> _interactionsCollection;
        private readonly IMongoCollection<Industry> _industryCollection;
        private readonly IMongoCollection<Role> _roleCollection;
        private readonly IMongoCollection<Company> _companyCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            _userCollection = database.GetCollection<User>("Users");
            _prospectCollection = database.GetCollection<Prospect>("Prospects");
            _interactionsCollection = database.GetCollection<Interacctions>("Interactions");
            _industryCollection = database.GetCollection<Industry>("Industries");
            _roleCollection = database.GetCollection<Role>("Roles");
            _companyCollection = database.GetCollection<Company>("Companies");

        }


        public async Task<UserDto> AddUser(string email, string password, string userName, string idRole)
        {
            var newUser = new User { Email = email, Password = password, UserName = userName, IdRole = idRole };
            await _userCollection.InsertOneAsync(newUser);
            return newUser.Adapt<UserDto>();
        }


        public async Task<UserDto> Login(string email, string password)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email) & Builders<User>.Filter.Eq(u => u.Password, password);
            var resp = await _userCollection.Find(filter).FirstOrDefaultAsync();
            return resp?.Adapt<UserDto>();
        }

        public async Task<Prospect> AddProspect(Prospect p)
        {
            await _prospectCollection.InsertOneAsync(p);
            return p;
        }

        public async Task<Interacctions> AddInteraction(Interacctions p)
        {
            await _interactionsCollection.InsertOneAsync(p);
            return p;
        }

        public async Task<List<Prospect>> GetProspects()
        {
            return await _prospectCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Interacctions>> GetInteractions()
        {
            return await _interactionsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Industry>> GetIndustries()
        {
            return await _industryCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _roleCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Prospect> GetProspectById(string id)
        {
            var filter = Builders<Prospect>.Filter.Eq(p => p.Id, id);
            return await _prospectCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Industry> GetIndustryById(string id)
        {
            var filter = Builders<Industry>.Filter.Eq(p => p.Id, id);
            return await _industryCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Role> GetRoleById(string id)
        {
            var filter = Builders<Role>.Filter.Eq(p => p.Id, id);
            return await _roleCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Interacctions> GetInteractionById(string id)
        {
            var filter = Builders<Interacctions>.Filter.Eq(p => p.Id, id);
            return await _interactionsCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Prospect> UpdateProspect(Prospect p)
        {
            var filter = Builders<Prospect>.Filter.Eq(p => p.Id, p.Id);
            await _prospectCollection.ReplaceOneAsync(filter, p);
            return p;
        }

        public async Task<Interacctions> UpdateInteraction(Interacctions p)
        {
            var filter = Builders<Interacctions>.Filter.Eq(p => p.Id, p.Id);
            await _interactionsCollection.ReplaceOneAsync(filter, p);
            return p;
        }

        public async Task<Industry> UpdateIndustry(Industry p)
        {
            var filter = Builders<Industry>.Filter.Eq(p => p.Id, p.Id);
            await _industryCollection.ReplaceOneAsync(filter, p);
            return p;
        }

        public async Task<Role> UpdateRole(Role p)
        {
            var filter = Builders<Role>.Filter.Eq(p => p.Id, p.Id);
            await _roleCollection.ReplaceOneAsync(filter, p);
            return p;
        }

        public async Task DeleteProspect(string id)
        {
            var filter = Builders<Prospect>.Filter.Eq(p => p.Id, id);
            await _prospectCollection.DeleteOneAsync(filter);
        }

        public async Task DeleteInteraction(string id)
        {
            var filter = Builders<Interacctions>.Filter.Eq(p => p.Id, id);
            await _interactionsCollection.DeleteOneAsync(filter);
        }

        public async Task DeleteIndustry(string id)
        {
            var filter = Builders<Industry>.Filter.Eq(p => p.Id, id);
            await _industryCollection.DeleteOneAsync(filter);
        }

        public async Task DeleteRole(string id)
        {
            var filter = Builders<Role>.Filter.Eq(p => p.Id, id);
            await _roleCollection.DeleteOneAsync(filter);
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var filter = Builders<User>.Filter.Eq(p => p.Id, id);
            var resp = await _userCollection.Find(filter).FirstOrDefaultAsync();
            return resp?.Adapt<UserDto>();
        }

        public async Task DeleteUser(string id)
        {
            var filter = Builders<User>.Filter.Eq(p => p.Id, id);
            await _userCollection.DeleteOneAsync(filter);
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(p => p.Email, email);
            var resp = await _userCollection.Find(filter).FirstOrDefaultAsync();
            return resp?.Adapt<UserDto>();
        }

        public async Task<Role> AddRole(Role r)
        {
            await _roleCollection.InsertOneAsync(r);
            return r;
        }

        public async Task<Industry> AddIndustry(Industry r)
        {
            await _industryCollection.InsertOneAsync(r);
            return r;
        }

        public async Task<Company> AddCompany(Company company)
        {
            await _companyCollection.InsertOneAsync(company);
            return company;
        }
    }
}
