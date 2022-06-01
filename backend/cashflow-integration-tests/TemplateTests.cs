using System;
using System.Linq;
using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using Xunit;

namespace budet_backend_integration_tests;

public class TemplateTests
{
    [Fact]
    public async Task TemplatesAreAddedAndRetrievedCorrect()
    {
        // Arrange + Act
        var client = new ApiClient();
        var result = await client.AddCategory("car");
        var carCategoryId = result.Categories.First().Id;

        await client.AddTemplate(500, new TimeSpan(1,0,0,0), "Insurance", carCategoryId);
        await client.AddTemplate(500, new TimeSpan(30,0,0,0), "Fuel", carCategoryId);
  
        var allTemplates = await client.GetTemplates();

        // Assert
        allTemplates.Should().HaveCount(2);
    }
}