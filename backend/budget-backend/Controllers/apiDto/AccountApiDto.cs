using budget_backend.domain;

namespace budget_backend.Controllers.apiDto;

public class AccountApiDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}

public static class AccountApiDtoExtensions
{
    public static AccountApiDto ToApiDto(this Account account)
    {
        return new AccountApiDto()
        {
            Id = account.Id,
            Name = account.Name
        };
    }
}