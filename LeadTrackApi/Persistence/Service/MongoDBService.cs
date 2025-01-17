using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Entities;
using LeadTrackApi.Domain.Models.Response;
using LeadTrackApi.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LeadTrackApi.Persistence.Service;

public class MongoDBService
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Prospect> _prospectCollection;
    private readonly IMongoCollection<Interactions> _interactionsCollection;
    private readonly IMongoCollection<Industry> _industryCollection;
    private readonly IMongoCollection<Role> _roleCollection;
    private readonly IMongoCollection<Company> _companyCollection;
    private readonly List<Role> _roles;
    private readonly List<Industry> _industries;
    private readonly List<User> _users;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

        _userCollection = database.GetCollection<User>("Users");
        _roleCollection = database.GetCollection<Role>("Roles");
        _companyCollection = database.GetCollection<Company>("Companies");
        _industryCollection = database.GetCollection<Industry>("Industries");
        _prospectCollection = database.GetCollection<Prospect>("Prospects");
        _interactionsCollection = database.GetCollection<Interactions>("Interactions");


        //Para mantener en cache
        _roles = _roleCollection.Find(_ => true).ToList();
        _industries = _industryCollection.Find(_ => true).ToList();
        _users = _userCollection.Find(_ => true).ToList();

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

        if (resp == null) await Task.FromException<UserDto>(new Exception("User not found"));

        var adapt = resp.Adapt<UserDto>();
        adapt.Role = _roles.FirstOrDefault(r => r.Id == resp.IdRole).Name;
        adapt.UserId = resp.Id;

        return adapt;
    }

    public async Task<Prospect> AddProspect(Prospect p)
    {
        await _prospectCollection.InsertOneAsync(p);
        return p;
    }

    public async Task<Interactions> AddInteraction(Interactions p)
    {
        await _interactionsCollection.InsertOneAsync(p);
        return p;
    }

    public async Task<List<ProspectDTO>> GetProspects()
    {
        var prospectDTOs = new List<ProspectDTO>();
        var prospects = await _prospectCollection.Find(_ => true).ToListAsync();

        foreach (var p in prospects)
        {
            var prospectDTO = p.Adapt<ProspectDTO>();
            var company = await _companyCollection.Find(i => i.Id == p.IdCompany).FirstOrDefaultAsync();
            var industry = _industries.FirstOrDefault(i => i.Id == company.IdIndustry);
            prospectDTO.Company = company.Adapt<CompanyDTO>();
            industry.Adapt(prospectDTO.Company);
            prospectDTOs.Add(prospectDTO);
        };
        return prospectDTOs;
    }

    public async Task<List<InteractionDTO>> GetInteractionsByProspect(string idProspect)
    {
        var interactions = new List<InteractionDTO>();
        var resp = await _interactionsCollection.Find(e => e.ProspectId == idProspect).ToListAsync();
        foreach (var i in resp)
        {
            var iDto = i.Adapt<InteractionDTO>();
            iDto.UserName = _users.FirstOrDefault(u => u.Id == i.UserId).UserName;
            interactions.Add(iDto);
        }

        return interactions;
    }

    public async Task<Interactions> GetInteractionById(string id)
    {
        var filter = Builders<Interactions>.Filter.Eq(p => p.Id, id);
        return await _interactionsCollection.Find(filter).FirstOrDefaultAsync();
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
