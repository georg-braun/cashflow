using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend.Controllers.apiDto;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace budet_backend_integration_tests;

public class AccountApiTests : IntegrationTest
{
    [Fact]
    public async Task CreateAndGet_OfAccount_IsCorrect()
    {
        // Arrange
        await AddNewAccountAsync("cash");
      
        // Act
        var accounts = await GetAllAccountsAsync();

        // Assert
        var accountFromServer = accounts.First();
        accountFromServer.Name.Should().Be("cash");
        accountFromServer.Id.Should().NotBe(Guid.Empty);
    }
    
    


    private async Task<IEnumerable<AccountApiDto>> GetAllAccountsAsync()
    {
        var getAccountResult = await client.GetAsync("/account");
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountApiDto[]>(accountsJson);
    }
    
    private async Task AddNewAccountAsync(string name)
    {
        var newAccount = new AddNewAccountDto()
        {
            Name = name
        };
        var json = JsonConvert.SerializeObject(newAccount);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync("/account", data);

    }
}