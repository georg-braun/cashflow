using System.ComponentModel.DataAnnotations;
using budget_backend.domain;
using budget_backend.domain.account;

namespace budget_backend.data.dbDto;

public class AccountEntryDto
{
    [Key] 
    public Guid Id { get; init; }
    
    public double Amount { get; init; }
    
    public DateOnly Timestamp { get; init;}
    
    public Guid AccountId { get; init; }
}

public static class AccountEntryDtoExtensions
{
    public static AccountEntryDto ToDbDto(this AccountEntry accountEntry)
    {
        return new AccountEntryDto()
        {
            Id = accountEntry.Id,
            AccountId = accountEntry.AccountId,
            Amount = accountEntry.Amount,
            Timestamp = accountEntry.Timestamp
        };
    }

    public static AccountEntry ToDomain(this AccountEntryDto accountEntryDto)
    {
        return new AccountEntry(accountEntryDto.Id, accountEntryDto.AccountId, accountEntryDto.Amount, accountEntryDto.Timestamp);
    }
}