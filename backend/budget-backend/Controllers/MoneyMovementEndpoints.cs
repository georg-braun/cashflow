using System.Security.Claims;
using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.domain;
using budget_backend.domain.budget;

namespace budget_backend.Controllers;

public static class MoneyMovementEndpoints
{
    
    public static async Task<ChangesContainerDto> GetAll(IMoneyService moneyService, IUserService userService,
        ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var moneyMovements = moneyService.GetMoneyMovements(userId);
        var categories = moneyService.GetCategories(userId);
        return ChangesContainerDtoFactory.Create(moneyMovements, categories);
    }
    
    public static async Task<ChangesContainerDto> GetMoneyMovements(IMoneyService moneyService, IUserService userService,
        ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var moneyMovements = moneyService.GetMoneyMovements(userId);
        return ChangesContainerDtoFactory.Create(moneyMovements);
    }

    public static async Task<ChangesContainerDto> DeleteMoneyMovement(DeleteMoneyMovementCommand command,
        IMoneyService moneyService, IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var id = MoneyMovementIdFactory.Create(command.MoneyMovementId);
        var changes = await moneyService.DeleteMoneyMovementAsync(id, userId);
        return ChangesContainerDtoFactory.Create(changes);
    }

    public static async Task<ChangesContainerDto> AddMoneyMovement(AddMoneyMovementCommand command, IMoneyService moneyService,
        IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var categoryId = CategoryIdFactory.Create(command.CategoryId);
        var changes = await moneyService.AddMoneyMovementAsync(command.Amount, command.Date, command.Note, categoryId, userId);
        return ChangesContainerDtoFactory.Create(changes);
    }
}