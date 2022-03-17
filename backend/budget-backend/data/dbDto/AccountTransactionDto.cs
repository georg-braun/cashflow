using System.ComponentModel.DataAnnotations;
using budget_backend.domain;
using budget_backend.domain.account;

namespace budget_backend.data.dbDto;

public class AccountTransactionDto
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid AccountEntryIdFrom { get; set; }
    
    public Guid AccountIdFrom { get; set; }
    public Guid AccountIdTo { get; set; }
    
    public Guid AccountEntryIdTo { get; set; }

    public DateOnly Timestamp  { get; set; }
    
    public double Amount { get; set; }
}

public static class AccountTransactionDtoExtensions
{
    public static AccountTransactionDto ToDbDto(this AccountTransaction accountTransaction)
    {
        return new AccountTransactionDto()
        {
            Id = accountTransaction.Id.Id,
            AccountEntryIdFrom = accountTransaction.FromAccountEntryId.Id,
            AccountEntryIdTo = accountTransaction.ToAccountEntryId.Id,
            AccountIdFrom = accountTransaction.FromAccountId.Id,
            AccountIdTo = accountTransaction.ToAccountId.Id
        };
    }

    public static AccountTransaction ToDomain(this AccountTransactionDto accountTransactionDto)
    {
        return new AccountTransaction(AccountTransactionIdFactory.Create(accountTransactionDto.Id), AccountIdFactory.Create(accountTransactionDto.AccountIdFrom), new AccountEntryId{Id = accountTransactionDto.AccountEntryIdFrom}, AccountIdFactory.Create(accountTransactionDto.AccountIdTo), new AccountEntryId{Id = accountTransactionDto.AccountEntryIdTo});
    }
    
}