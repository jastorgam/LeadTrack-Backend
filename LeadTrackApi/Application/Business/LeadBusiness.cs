using LeadTrackApi.Application.Extensions;
using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Application.Utils;
using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Entities;
using LeadTrackApi.Domain.Models;
using LeadTrackApi.Persistence.Service;

namespace LeadTrackApi.Application.Business
{
    public class LeadBusiness(MongoDBService mongoDBService, ILogger<LeadBusiness> logger) : ILeadBusiness
    {
        private readonly MongoDBService _mongoService = mongoDBService;

        public async Task<UserDto> AddUser(string email, string pass, string name, string idRole)
        {
            pass = SecurityUtils.HashPassword(pass);
            return await _mongoService.AddUser(email, pass, name, idRole);
        }

        public async Task<List<ProspectDTO>> GetProspects()
        {
            return await _mongoService.GetProspects();
        }

        public async Task<List<InteractionDTO>> GetInteractions(string idProspect)
        {
            return await _mongoService.GetInteractionsByProspect(idProspect);
        }

        public async Task<List<Dictionary<string, object>>> ProcessFile(Stream stream)
        {
            var schema = new FileSchema()
            {
                Type = "excel",
                BodyFields =
                 [
                    new () { Name = "nombre", Type = "string" },
                    new () { Name = "apellido", Type = "string" },
                    new () { Name = "email", Type = "string" },
                    new () { Name = "cargo", Type = "string" },
                    new () { Name = "industria", Type = "string" },
                    new () { Name = "nombre_company", Type = "string" },
                    new () { Name = "address", Type = "string" },
                    new () { Name = "size", Type = "string" },
                    new () { Name = "domain", Type = "string" },
                    new () { Name = "linkedin", Type = "string" },
                ]
            };


            var reader = new ExcelFileReader(schema);
            var resp = reader.ReadFile(stream);
            var ret = resp.ToList();
            ret.ForEach(item => logger.LogInformation(item.Dump(false)));
            return ret;
        }
    }
}
