using Xunit;
using LeadTrackApi.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadTrack.API.Test;
using Xunit.Abstractions;

namespace LeadTrackApi.Application.Utils.Tests
{
    public class SecurityUtilsTests(
        TestConfiguration testConfiguration,
        ITestOutputHelper console
    ) : IClassFixture<TestConfiguration>
    {


        [Fact]
        public void HashPassword_ShouldReturnHashedString()
        {
            // Arrange
            string password = "123456";
            string expectedHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            // Act
            string actualHash = SecurityUtils.HashPassword(password);
            console.WriteLine($"Actual Hash: {actualHash}");
            // Assert
            Assert.Equal(expectedHash, actualHash);
        }
    }
}