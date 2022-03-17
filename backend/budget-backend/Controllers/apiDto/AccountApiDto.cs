using budget_backend.domain;
using budget_backend.domain.account;

namespace budget_backend.Controllers.apiDto;

public class AccountApiDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public static class AccountApiDtoExtensions
{
    public static AccountApiDto ToApiDto(this Account account)
    {
        return new AccountApiDto()
        {
            Id = account.Id.Id,
            Name = account.Name
        };
    }
}