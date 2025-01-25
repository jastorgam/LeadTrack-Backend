using ICSharpCode.SharpZipLib.Core;
using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Entities;
using LeadTrackApi.Domain.Enums;
using LeadTrackApi.Domain.Models.Response;
using LeadTrackApi.Persistence.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LeadTrackApi.Persistence.Service;

public class MongoDBService
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Prospect> _prospectCollection;
    private readonly IMongoCollection<FullProspect> _fullProspectCollection;
    private readonly IMongoCollection<Interactions> _interactionsCollection;
    private readonly IMongoCollection<Industry> _industryCollection;
    private readonly IMongoCollection<Role> _roleCollection;
    private readonly IMongoCollection<Company> _companyCollection;
    private readonly List<Role> _roles;
    private readonly List<Industry> _industries;
    private readonly List<User> _users;
    private readonly ILogger<MongoDBService> logger;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

        _userCollection = database.GetCollection<User>("Users");
        _roleCollection = database.GetCollection<Role>("Roles");
        _companyCollection = database.GetCollection<Company>("Companies");
        _industryCollection = database.GetCollection<Industry>("Industries");
        _prospectCollection = database.GetCollection<Prospect>("Prospects");
        _fullProspectCollection = database.GetCollection<FullProspect>("FullProspects");
        _interactionsCollection = database.GetCollection<Interactions>("Interactions");


        //Para mantener en cache
        _roles = _roleCollection.Find(_ => true).ToList();
        _industries = _industryCollection.Find(_ => true).ToList();
        _users = _userCollection.Find(_ => true).ToList();

    }


    public async Task<UserDto> AddUser(string email, string password, string userName, string idRole)
    {
        var newUser = new User { Email = email, Password = password, UserName = userName, IdRole = idRole };
        var user = await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        if (user != null) return user.Adapt<UserDto>();

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
        try
        {
            var filter = Builders<Prospect>.Filter.Eq(prospect => prospect.Name, p.Name) &
                         Builders<Prospect>.Filter.Eq(prospect => prospect.LastName, p.LastName) &
                         Builders<Prospect>.Filter.ElemMatch(prospect => prospect.Emails, email => email.Address == p.Emails.FirstOrDefault().Address);

            var existingProspect = await _prospectCollection.Find(filter).FirstOrDefaultAsync();
            if (existingProspect != null) return existingProspect;

            await _prospectCollection.InsertOneAsync(p);
            return p;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public async Task<FullProspect> AddFullProspect(FullProspect p)
    {
        try
        {
            var filter = Builders<FullProspect>.Filter.Eq(prospect => prospect.FullName, p.FullName);
            var existingProspect = await _fullProspectCollection.Find(filter).FirstOrDefaultAsync();

            if (existingProspect != null) return existingProspect;

            await _fullProspectCollection.InsertOneAsync(p);
            return p;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<FullProspect>> GetFullProspects()
    {
        try
        {
            var resp = await _fullProspectCollection.Find(_ => true).ToListAsync();
            return resp;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<FullProspect> GetFullProspect(string id)
    {
        try
        {
            var resp = await _fullProspectCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            return resp;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public async Task<Interactions> AddInteraction(InteractionDTO p)
    {
        var interaction = p.Adapt<Interactions>();
        interaction.UserId = _users.Find(u => u.UserName == p.UserName).Id;
        await _interactionsCollection.InsertOneAsync(interaction);
        return interaction;
    }

    public async Task<FullProspect> AddFullInteraction(string idProspect, FullInteractions interactions)
    {
        var filter = Builders<FullProspect>.Filter.Eq(p => p.Id, idProspect);
        var update = Builders<FullProspect>.Update.Push(p => p.Interactions, interactions);
        var options = new FindOneAndUpdateOptions<FullProspect> { ReturnDocument = ReturnDocument.After };

        var result = await _fullProspectCollection.FindOneAndUpdateAsync(filter, update, options);
        return result;
    }

    public async Task<List<ProspectDTO>> GetProspects(int page = 1, int pageSize = 10)
    {
        var prospectDTOs = new List<ProspectDTO>();
        var skip = (page - 1) * pageSize;

        var prospects = await _prospectCollection.Find(_ => true)
                                                 //.Skip(skip)
                                                 //.Limit(pageSize)
                                                 .ToListAsync();

        try
        {
            foreach (var p in prospects)
            {
                var prospectDTO = p.Adapt<ProspectDTO>();
                var company = await _companyCollection.Find(i => i.Id == p.IdCompany).FirstOrDefaultAsync();
                if (company != null)
                {
                    var industry = _industries.FirstOrDefault(i => i.Id == company.IdIndustry);
                    prospectDTO.Company = company.Adapt<CompanyDTO>();
                    industry.Adapt(prospectDTO.Company);
                }
                prospectDTOs.Add(prospectDTO);
            };

            return prospectDTOs;
        }
        catch (Exception ex)
        {

            throw ex;
        }
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
        var rol = await _roleCollection.Find(i => i.Name == r.Name).FirstOrDefaultAsync();
        if (rol != null) return rol;

        await _roleCollection.InsertOneAsync(r);
        return r;
    }

    public async Task<Industry> AddIndustry(Industry r)
    {
        var existingIndustry = await _industryCollection.Find(i => i.Type == r.Type).FirstOrDefaultAsync();
        if (existingIndustry != null) return existingIndustry;

        await _industryCollection.InsertOneAsync(r);
        return r;
    }

    public async Task<Company> AddCompany(Company company)
    {
        var existingCompany = await _companyCollection.Find(c => c.Name == company.Name).FirstOrDefaultAsync();
        if (existingCompany != null) return existingCompany;

        await _companyCollection.InsertOneAsync(company);
        return company;
    }

    public async Task<InteractionDTO> GetLastInteractionsByProspectId(string id)
    {
        var filter = Builders<Interactions>.Filter.Eq(p => p.ProspectId, id);
        var sort = Builders<Interactions>.Sort.Descending(p => p.Date);
        var resp = await _interactionsCollection.Find(filter).Sort(sort).FirstOrDefaultAsync();

        if (resp == null) return null;
        var inte = resp.Adapt<InteractionDTO>();
        inte.UserName = _users.FirstOrDefault(u => u.Id == resp.UserId).UserName;
        return inte.Adapt<InteractionDTO>();
    }

    public async Task<long> GetTotalProspectsCount()
    {
        return await _prospectCollection.CountDocumentsAsync(_ => true);
    }

    public async Task<FullProspect> UpdateFullProspect(FullProspectDTO fullProspect)
    {
        var newProspect = fullProspect.Adapt<FullProspect>();
        var filter = Builders<FullProspect>.Filter.Eq(p => p.Id, fullProspect.Id);
        var update = Builders<FullProspect>.Update
            .Set(p => p.Name, newProspect.Name)
            .Set(p => p.LastName, newProspect.LastName)
            .Set(p => p.FullName, $"{newProspect.Name.Trim()} {newProspect.LastName.Trim()}")
            .Set(p => p.Position, newProspect.Position)
            .Set(p => p.Phones, newProspect.Phones)
            .Set(p => p.Emails, newProspect.Emails)
            .Set(p => p.Company, newProspect.Company)
            .Set(p => p.Status, newProspect.Status)
            .Set(p => p.UserModify, newProspect.UserModify)
            .Set(p => p.DateModify, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<FullProspect> { ReturnDocument = ReturnDocument.After };
        var updatedProspect = await _fullProspectCollection.FindOneAndUpdateAsync(filter, update, options);

        return updatedProspect;
    }


}
