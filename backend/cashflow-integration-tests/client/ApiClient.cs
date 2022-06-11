using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend.Controllers;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using Newtonsoft.Json;

namespace budget_backend_integration_tests.backend;

/// <summary>
///     Client for interaction with the backend api.
/// </summary>
public class ApiClient
{
    private HttpClient client;

    public ApiClient()
    {
        client = new IntegrationTestBackend().client;
    }

    private StringContent Serialze(object command)
    {
        var json = JsonConvert.SerializeObject(command);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }


    public async Task<ChangesContainerDto> AddCategory(string name)
    {
        var command = new AddCategoryCommand(name);
        var response = await client.PostAsync(Routes.AddCategory, Serialze(command));

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ChangesContainerDto>(responseJson);
    }

    public async Task AddTemplate(double amount, TimeSpan interval, string note, Guid categoryId)
    {
        var command = new AddTemplateCommand(amount, interval, note, categoryId);
        var response = await client.PostAsync(Routes.AddTemplate, Serialze(command));
    }
    
    public async Task<IEnumerable<TemplateDto>> GetTemplates()
    {
        var response = await client.GetAsync(Routes.GetTemplates);

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<TemplateDto>>(responseJson);
    }

    public async Task<ChangesContainerDto> AddMoneyMovement(double amount, DateTime date, string note, Guid categoryId)
    {
        var command = new AddMoneyMovementCommand(amount, date, note, categoryId);
        var response = await client.PostAsync(Routes.AddMoneyMovement, Serialze(command));

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ChangesContainerDto>(responseJson);
    }

    public async Task<ChangesContainerDto> GetAll()
    {
        var getAllResponse = await client.GetAsync(Routes.GetAll);
        var allDataAsJson = await getAllResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ChangesContainerDto>(allDataAsJson);
    }

    public async Task<ChangesContainerDto> DeleteMoneyMovement(Guid id)
    {
        var command = new DeleteMoneyMovementCommand(id);
        var response = await client.PostAsync(Routes.DeleteMoneyMovement, Serialze(command));
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ChangesContainerDto>(responseJson);
    }

    public async Task<ChangesContainerDto> DeleteCategory(Guid id)
    {
        var command = new DeleteCategoryCommand(id);
        var response = await client.PostAsync(Routes.DeleteCategory, Serialze(command));
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ChangesContainerDto>(responseJson);
    }
}