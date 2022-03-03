using System.ComponentModel.DataAnnotations;
using budget_backend.domain.account;

namespace budget_backend.data.dbDto;

public class AccountDto
{
    [Key] 
    public Guid Id { get; set; }

    public string Name { get; set; }
}

public static class AccountDtoExtensions
{
    public static AccountDto ToDbDto(this Account account)
    {
        return new AccountDto()
        {
            Id = account.Id.Id,
            Name = account.Name
        };
    }

    public static Account ToDomain(this AccountDto accountDto)
    {
        return new Account(new AccountId {Id = accountDto.Id}, accountDto.Name);
    }
}