using Xunit;
using LeadTrackApi.Application.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadTrack.API.Test;
using LeadTrackApi.Application.Interfaces;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using LeadTrackApi.Application.Extensions;
using Microsoft.Extensions.Logging;

namespace LeadTrackApi.Application.Business.Tests;

public class LeadBusinessTests(TestConfiguration testConfiguration, ITestOutputHelper console) : IClassFixture<TestConfiguration>
{
    private readonly ILeadBusiness _business = testConfiguration.ServiceProvider.GetService<ILeadBusiness>() ?? throw new NullReferenceException();

    [Fact()]
    public async Task GetFullProspectsTest()
    {
        var sw = new Stopwatch();
        sw.Start();
        var resp = await _business.GetFullProspects();
        sw.Stop();

        console.WriteLine($"Elapsed={sw.ElapsedMilliseconds} ms");
        console.WriteLine(resp.Dump());
        Assert.NotNull(resp);
    }
}
