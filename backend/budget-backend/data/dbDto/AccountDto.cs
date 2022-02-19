using System.ComponentModel.DataAnnotations;
using budget_backend.domain;

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
            Id = account.Id,
            Name = account.Name
        };
    }

    public static Account ToDomain(this AccountDto accountDto)
    {
        return new Account(accountDto.Id, accountDto.Name);
    }
}