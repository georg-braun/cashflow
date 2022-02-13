using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace budet_backend_integration_tests;

public class AccountApiTests : IntegrationTest
{
    [Fact]
    public async Task Get_ReturnsSomeDate()
    {
        // Arrange

        // Act
        var result = await client.GetAsync("/account");
        var content = await result.Content.ReadAsStringAsync();

        // Assert
        content.Should().NotBeEmpty();
    }
}