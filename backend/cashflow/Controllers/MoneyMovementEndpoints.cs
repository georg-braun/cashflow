using System.Security.Claims;
using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.data.dbo;
using budget_backend.Domain.Commands;
using budget_backend.Domain.Queries;
using MediatR;
using DeleteMoneyMovementCommand = budget_backend.Controllers.apiDto.commands.DeleteMoneyMovementCommand;

namespace budget_backend.Controllers;

public static class MoneyMovementEndpoints
{
    public static async Task<ChangesContainerDto> GetAll(IMediator mediator, IUserService userService,
        ClaimsPrincipal claims)
    {
        Console.WriteLine("GetAll");
        var userId = await userService.GetUserIdAsync(claims);
        var moneyMovements = await mediator.Send(new GetMoneyMovementsQuery(userId));
        var categories = await mediator.Send(new GetCategoriesQuery(userId));
        return ChangesContainerDtoFactory.Create(moneyMovements, categories);
    }
    
    public static async Task<ChangesContainerDto> GetMoneyMovements(IMediator mediator,
        IUserService userService,
        ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var moneyMovements = await mediator.Send(new GetMoneyMovementsQuery(userId));
        return ChangesContainerDtoFactory.Create(moneyMovements);
    }
    
    public static async Task<ChangesContainerDto> DeleteMoneyMovement(DeleteMoneyMovementCommand command,
        IMediator mediator, IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var id = MoneyMovementIdFactory.Create(command.MoneyMovementId);
        var changes =
            await mediator.Send(new Domain.Commands.DeleteMoneyMovementCommand(id, userId));
        return ChangesContainerDtoFactory.Create(changes);
    }
    
    public static async Task<ChangesContainerDto> AddMoneyMovement(AddMoneyMovementCommand command,
        IMediator mediator,
        IUserService userService, ClaimsPrincipal claims)
    {
        var utcDate = command.Date.ToUniversalTime();
        var userId = await userService.GetUserIdAsync(claims);
        var categoryId = CategoryIdFactory.Create(command.CategoryId);
        var moneyMovement = MoneyMovementFactory.Create(command.Amount, utcDate, command.Note, categoryId, userId);
        var changes =
            await mediator.Send(new CreateMoneyMovementCommand(moneyMovement));
        return ChangesContainerDtoFactory.Create(changes);
    }
}