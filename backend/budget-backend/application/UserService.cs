using budget_backend.data;

namespace budget_backend.application;

public interface IUserService
{
    Task<UserId> GetUserIdAsync(string AuthProviderId);
}

public class UserService : IUserService
{
    private readonly DataContext _dataContext;

    public UserService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    ///     Get a user id. If the user doesn't exist a new one is created.
    /// </summary>
    /// <param name="authProviderId"></param>
    /// <returns></returns>
    public async Task<UserId> GetUserIdAsync(string authProviderId)
    {
        var userId =  _dataContext.GetUserIdByAuthProviderId(authProviderId);
        if (userId.IsValid)
            return userId;

        return await AddUserAsync(authProviderId);
    }

    public Task<UserId> AddUserAsync(string authProviderId)
    {
        return _dataContext.AddUserAsync(authProviderId);
    }
}