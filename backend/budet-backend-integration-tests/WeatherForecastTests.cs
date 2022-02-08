using System.Threading.Tasks;
using Xunit;

namespace budet_backend_integration_tests;

public class WeatherForecastTests : IntegrationTest
{
    [Fact]
    public async Task Get_ReturnsSomeDate()
    {
        // Arrange
        
        // Act
        var result = await client.GetAsync("/weatherforecast");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
    }
}