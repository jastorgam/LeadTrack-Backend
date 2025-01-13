using Xunit;
using LeadTrackApi.Application.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadTrack.API.Test;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using LeadTrack.API.Application.Extensions;
using LeadTrackApi.Application.Interfaces;

namespace LeadTrackApi.Application.Business.Tests
{
    public class AuthBusinessTests(TestConfiguration testConfiguration, ITestOutputHelper console) : IClassFixture<TestConfiguration>
    {
        private readonly IAuthBusiness _authBusiness = testConfiguration.ServiceProvider.GetService<IAuthBusiness>() ?? throw new ArgumentNullException();
        private readonly ITestOutputHelper _console = console;

        [Fact()]
        public async Task LoginTest()
        {
            var resp = await _authBusiness.Login("admin@leadtrack.cl", "123456");
            _console.WriteLine(resp.Dump());
            Assert.True(resp.UserName == "Admin");

        }
    }
}