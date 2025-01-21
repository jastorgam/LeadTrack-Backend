using LeadTrackApi.Application.Extensions;
using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Application.Utils;
using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Entities;
using LeadTrackApi.Domain.Enums;
using LeadTrackApi.Domain.Models;
using LeadTrackApi.Persistence.Service;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

        public async Task<List<ProspectDTO>> GetProspects(int page = 1, int pageSize = 10)
        {
            var resp = await _mongoService.GetProspects(page, pageSize);
            foreach (var item in resp)
            {
                var interaction = await _mongoService.GetLastInteractionsByProspectId(item.Id.ToString());
                item.LastInteraction = interaction;
            }
            return resp;
        }

        public async Task<IEnumerable<FullProspect>> GetFullProspects()
        {
            var resp = await _mongoService.GetFullProspects();
            return resp;
        }

        public async Task<List<InteractionDTO>> GetInteractions(string idProspect)
        {
            return await _mongoService.GetInteractionsByProspect(idProspect);
        }

        public async Task<List<Dictionary<string, object>>> ProcessFilex(Stream stream)
        {
            var schema = new FileSchema()
            {
                Type = "excel",
                BodyFields =
                 [
                    new () { Name = "name", Type = "string" },
                    new () { Name = "last_name", Type = "string" },
                    new () { Name = "email", Type = "string" },
                    new () { Name = "job_title", Type = "string" },
                    new () { Name = "industry", Type = "string" },
                    new () { Name = "name_company", Type = "string" },
                    new () { Name = "address", Type = "string" },
                    new () { Name = "size", Type = "string" },
                    new () { Name = "domain", Type = "string" },
                    new () { Name = "linkedin", Type = "string" },
                ]
            };

            try
            {
                var reader = new ExcelFileReader(schema);
                var resp = reader.ReadFile(stream);
                var ret = resp.ToList();

                var remover = new List<Dictionary<string, object>>();

                foreach (var item in ret)
                {
                    if (string.IsNullOrEmpty(item["name"].ToString()) && string.IsNullOrEmpty(item["name"].ToString()))
                    {
                        remover.Add(item);
                        continue;
                    }


                    logger.LogInformation(item.Dump(false));

                    var industry = new Industry()
                    {
                        Type = item["industry"].ToString(),
                        Description = item["industry"].ToString()
                    };

                    industry = await _mongoService.AddIndustry(industry);

                    var company = new Company()
                    {
                        Name = item["name_company"].ToString(),
                        Address = item["address"].ToString(),
                        Size = item["size"].ToString(),
                        Domain = item["domain"].ToString(),
                        IdIndustry = industry.Id
                    };

                    company = await _mongoService.AddCompany(company);

                    var prospects = new Prospect()
                    {
                        Name = item["name"].ToString(),
                        LastName = item["last_name"].ToString(),
                        Emails = [new Email() { Address = item["email"].ToString(), Type = EmailType.Personal.ToString(), Valid = true }],
                        Position = item["job_title"].ToString(),
                        IdCompany = company.Id,
                        SocialNetworks = [new SocialNetwork() { Url = item["linkedin"].ToString(), Type = SocialNetworkType.LinkedIn.ToString() }]
                    };

                    await _mongoService.AddProspect(prospects);
                };

                remover.ForEach(e => ret.Remove(e));

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el proceso de lectura", ex);
            }


        }


        public async Task<List<Dictionary<string, object>>> ProcessFile(Stream stream)
        {
            var schema = new FileSchema()
            {
                Type = "excel",
                BodyFields =
                 [
                    new () { Name = "name", Type = "string" },
                    new () { Name = "last_name", Type = "string" },
                    new () { Name = "email", Type = "string" },
                    new () { Name = "job_title", Type = "string" },
                    new () { Name = "industry", Type = "string" },
                    new () { Name = "name_company", Type = "string" },
                    new () { Name = "address", Type = "string" },
                    new () { Name = "size", Type = "string" },
                    new () { Name = "domain", Type = "string" },
                    new () { Name = "linkedin", Type = "string" },
                ]
            };
            logger.LogWarning(schema.Dump());

            try
            {
                var reader = new ExcelFileReader(schema);
                var resp = reader.ReadFile(stream);
                var ret = resp.ToList();

                var remover = new List<Dictionary<string, object>>();
                var i = 1;

                foreach (var item in ret)
                {
                    if (string.IsNullOrEmpty(item["name"].ToString()) && string.IsNullOrEmpty(item["name"].ToString()))
                    {
                        remover.Add(item);
                        continue;
                    }

                    var prospects = new FullProspect()
                    {
                        Name = item["name"].ToString(),
                        LastName = item["last_name"].ToString(),
                        FullName = $"{item["name"].ToString().Trim()} {item["last_name"].ToString().Trim()}",
                        Emails = [new Email() { Address = item["email"].ToString(), Type = EmailType.Personal.ToString(), Valid = true }],
                        Phones = i++ == 1 ? [new Phone() { PhoneNumber = "123456", Type = PhoneType.Personal.ToString(), Valid = true }] : [],
                        Interactions = [],
                        Position = item["job_title"].ToString(),
                        Company = new FullCompany()
                        {
                            Name = item["name_company"].ToString(),
                            Address = item["address"].ToString(),
                            Size = item["size"].ToString(),
                            Domain = item["domain"].ToString(),
                            Description = item["industry"].ToString(),
                            Type = item["industry"].ToString(),
                        },
                        SocialNetworks = [new SocialNetwork() { Url = item["linkedin"].ToString(), Type = SocialNetworkType.LinkedIn.ToString() }],
                        Status = false
                    };

                    await _mongoService.AddFullProspect(prospects);
                };

                remover.ForEach(e => ret.Remove(e));

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el proceso de lectura", ex);
            }


        }

        public async Task<long> GetProspectsCount()
        {
            return await _mongoService.GetTotalProspectsCount();
        }

        public async Task<FullProspect> SaveInteraction(InteractionDTO interaction)
        {
            var fi = new FullInteractions()
            {
                Notes = interaction.Notes,
                UserName = interaction.UserName,
                Answer = interaction.Answer,
                Date = DateTime.UtcNow,
                Type = interaction.Type
            };

            return await _mongoService.AddFullInteraction(interaction.ProspectId, fi);
        }

        public async Task<FullProspect> GetFullProspect(string id)
        {
            return await _mongoService.GetFullProspect(id);
        }
    }
}
