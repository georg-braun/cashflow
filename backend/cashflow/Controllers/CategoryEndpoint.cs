using System.Security.Claims;
using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.data.dbo;
using budget_backend.Domain.Commands;
using budget_backend.Domain.Queries;
using MediatR;
using DeleteCategoryCommand = budget_backend.Domain.Commands.DeleteCategoryCommand;

namespace budget_backend.Controllers;

public static class CategoryEndpoints
{
    public static async Task<ChangesContainerDto> GetCategories(IMediator mediator, IUserService userService,
        ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var categories = await mediator.Send(new GetCategoriesQuery(userId));
        return ChangesContainerDtoFactory.Create(categories);
    }
    
    public static async Task<ChangesContainerDto> DeleteCategory(apiDto.commands.DeleteCategoryCommand command,
        IMediator mediator, IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var categoryId = CategoryIdFactory.Create(command.CategoryId);
        var changes = await mediator.Send(new DeleteCategoryCommand(categoryId, userId));
        
        return ChangesContainerDtoFactory.Create(changes);
    }

    public static async Task<ChangesContainerDto> AddCategory(AddCategoryCommand command, IMediator mediator,
        IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var category = CategoryFactory.Create(command.Name, userId);
        var changes = await mediator.Send(new CreateCategoryCommand(category));
        return ChangesContainerDtoFactory.Create(changes);
    }
}