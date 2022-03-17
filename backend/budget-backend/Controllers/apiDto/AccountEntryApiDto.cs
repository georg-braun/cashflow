using budget_backend.domain.account;

namespace budget_backend.Controllers.apiDto;

public record AccountEntryApiDto
{
    public Guid Id { get; init; }
    public double Amount { get; init; }
    public DateTime Date { get; init; }
    public AccountId AccountId { get; init; } = null!;
}

public static class AccountEntryApiDtoExtensions
{
    public static AccountEntryApiDto ToApiDto(this AccountEntry accountEntry)
    {
        return new AccountEntryApiDto()
        {
            Id = accountEntry.Id.Id,
            Amount = accountEntry.Amount,
            Date = accountEntry.Date.ToDateTime(TimeOnly.MinValue), 
            AccountId = accountEntry.AccountId
        };
    }
}