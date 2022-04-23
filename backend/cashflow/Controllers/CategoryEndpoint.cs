using System.Security.Claims;
using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.domain.budget;

namespace budget_backend.Controllers;

public static class CategoryEndpoints
{
    public static async Task<ChangesContainerDto> GetCategories(IMoneyService moneyService, IUserService userService,
        ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var categories = moneyService.GetCategories(userId);
        return ChangesContainerDtoFactory.Create(categories);
    }

    public static async Task<ChangesContainerDto> DeleteCategory(DeleteCategoryCommand command,
        IMoneyService moneyService, IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var categoryId = CategoryIdFactory.Create(command.CategoryId);
        var changes = await moneyService.DeleteCategoryAsync(categoryId, userId);
        return ChangesContainerDtoFactory.Create(changes);
    }

    public static async Task<ChangesContainerDto> AddCategory(AddCategoryCommand command, IMoneyService moneyService,
        IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var changes = await moneyService.AddCategoryAsync(command.Name, userId);
        return ChangesContainerDtoFactory.Create(changes);
    }
}