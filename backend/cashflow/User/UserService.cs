using System.Security.Claims;
using budget_backend.Controllers;
using budget_backend.data;

namespace budget_backend.application;

public interface IUserService
{
    /// <summary>
    ///     Get a user id. If the user doesn't exist a new one is created.
    /// </summary>
    /// <param name="authProviderId"></param>
    /// <returns></returns>
    Task<UserId> GetUserIdAsync(string authProviderId);

    /// <summary>
    ///     Get a user id. If the user doesn't exist a new one is created.
    /// </summary>
    /// <param name="authProviderId"></param>
    /// <returns></returns>
    Task<UserId> GetUserIdAsync(ClaimsPrincipal claims);
}

public class UserService : IUserService
{
    private readonly CashflowDataContext _cashflowDataContext;

    public UserService(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    /// <summary>
    ///     Get a user id. If the user doesn't exist a new one is created.
    /// </summary>
    /// <param name="authProviderId"></param>
    /// <returns></returns>
    public async Task<UserId> GetUserIdAsync(string authProviderId)
    {
        var userId = _cashflowDataContext.GetUserIdByAuthProviderId(authProviderId);
        if (userId.IsValid)
            return userId;

        return await AddUserAsync(authProviderId);
    }

    /// <summary>
    ///     Get a user id. If the user doesn't exist a new one is created.
    /// </summary>
    /// <param name="authProviderId"></param>
    /// <returns></returns>
    public async Task<UserId> GetUserIdAsync(ClaimsPrincipal claims)
    {
        return await GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
    }


    public Task<UserId> AddUserAsync(string authProviderId)
    {
        return _cashflowDataContext.AddUserAsync(authProviderId);
    }
}