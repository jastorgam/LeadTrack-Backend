﻿using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadTrackApi.Domain.Entities;
using LeadTrack.API.Test;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using LeadTrackApi.Domain.Enums;
using LeadTrackApi.Application.Utils;
using LeadTrackApi.Persistence.Service;
using LeadTrackApi.Application.Extensions;
using System.Diagnostics;

namespace LeadTrackApi.Persistence.Service.Tests;

public class MongoDBServiceTests(TestConfiguration testConfiguration, ITestOutputHelper console) : IClassFixture<TestConfiguration>
{
    public MongoDBService _mongoDBService = testConfiguration.ServiceProvider.GetService<MongoDBService>() ?? throw new ArgumentNullException();
    private readonly ITestOutputHelper _console = console;


    [Fact()]
    public async Task AddRoles()
    {
        var rol = new Role { Name = "Admin", Active = true, Description = "Administrador del sistema" };
        var result = await _mongoDBService.AddRole(rol);
        _console.WriteLine(result.Dump());
        Assert.True(result.Name == "Admin");

        rol = new Role { Name = "Executive", Active = true, Description = "Ejecutivo de Leads" };
        result = await _mongoDBService.AddRole(rol);
        _console.WriteLine(result.Dump());
        Assert.True(result.Name == "Executive");
    }

    [Fact()]
    public async Task AddIndustries()
    {
        var industry = new Industry { Type = "Computer Hardware", Description = "Computer Hardware" };

        var result = await _mongoDBService.AddIndustry(industry);
        _console.WriteLine(result.Dump());
        Assert.True(result.Type == "Computer Hardware");

        industry = new Industry { Type = "Government Administration", Description = "Government Administration" };
        result = await _mongoDBService.AddIndustry(industry);
        _console.WriteLine(result.Dump());
        Assert.True(result.Type == "Government Administration");

        industry = new Industry { Type = "TI", Description = "Tecnología de la información" };
        result = await _mongoDBService.AddIndustry(industry);
        _console.WriteLine(result.Dump());
        Assert.True(result.Type == "TI");
    }

    [Fact()]
    public async Task AddCompany()
    {
        var company = new Company { Name = "Company", Address = "Address", Domain = "Domain.cl", Size = "10-50", IdIndustry = "677db84f187b44707abbfdc6" };
        var result = await _mongoDBService.AddCompany(company);
        _console.WriteLine(result.Dump());
        Assert.True(result.Name == company.Name);
    }


    [Fact()]
    public async Task AddProspectTest()
    {
        var prospect = new Prospect
        {
            Name = "John",
            LastName = "Doe",
            Position = "Manager",
            IdCompany = "677dbbc7e932115f41f2ca16",
            Phones = [new Phone { PhoneNumber = "123456789", Type = PhoneType.Personal.ToString() }],
            Emails = [new Email
                {
                    Address = "jd@gmail.com",
                    Type = EmailType.Personal.ToString(),
                    Valid = true
                }],
            SocialNetworks = [new SocialNetwork
                {
                    Url = "https://www.linkedin.com/in/johndoe",
                    Type = SocialNetworkType.LinkedIn.ToString()
                }],
        };

        var result = await _mongoDBService.AddProspect(prospect);
        _console.WriteLine(result.Dump());
        Assert.True(result.Name == prospect.Name);
    }

    [Fact()]
    public async Task AddUserTest()
    {
        var email = "admin@leadtrack.cl";
        var password = SecurityUtils.HashPassword("123456");
        var name = "Admin";
        var idRole = "677db6caa30aeee7cb519c28";

        var result = await _mongoDBService.AddUser(email, password, name, idRole);
        _console.WriteLine(result.Dump());
        Assert.True(result.Email == email);
    }

    [Fact()]
    public async Task AddInteraction()
    {
        var interaction = new Domain.DTOs.InteractionDTO
        {
            ProspectId = "677db948bc09b835dee770a6",
            UserName = "Admin",
            Type = InteractionType.Call.ToString(),
            Notes = "Test call"
        };
        var result = await _mongoDBService.AddInteraction(interaction);
        _console.WriteLine(result.Dump());
        Assert.True(result.ProspectId == interaction.ProspectId);
    }

    [Fact()]
    public async Task GetOldProspectsTest()
    {
        var resp = await _mongoDBService.GetProspects();
        _console.WriteLine(resp.Dump());
        Assert.NotEmpty(resp);
    }



    [Fact()]
    public async Task GetFullProspectsTest()
    {
        var sw = new Stopwatch();
        sw.Start();
        var resp = await _mongoDBService.GetFullProspects();
        sw.Stop();

        _console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
        _console.WriteLine(resp.Dump());
        Assert.NotEmpty(resp);
    }

    //[Fact()]
    //public async Task AddFullInteractionTest()
    //{
    //    var fi = new FullInteractions()
    //    {
    //        Date = DateTime.UtcNow,
    //        Type = InteractionType.Call.ToString(),
    //        UserName = "Admin",
    //        Answer = true,
    //        Notes = "Test"
    //    };

    //    var resp = await _mongoDBService.AddFullInteraction("678dbbfdd509724fbf82e647", fi);
    //    _console.WriteLine(resp.Dump());
    //    Assert.True(resp.Interactions.Count > 0);
    //}
}