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
            Id = accountTransaction.Id,
            AccountEntryIdFrom = accountTransaction.FromAccountEntryId,
            AccountEntryIdTo = accountTransaction.ToAccountEntryId,
            AccountIdFrom = accountTransaction.FromAccountId,
            AccountIdTo = accountTransaction.ToAccountId
        };
    }

    public static AccountTransaction ToDomain(this AccountTransactionDto accountTransactionDto)
    {
        return new AccountTransaction(accountTransactionDto.Id, accountTransactionDto.AccountIdFrom, accountTransactionDto.AccountEntryIdFrom, accountTransactionDto.AccountIdTo, accountTransactionDto.AccountEntryIdTo);
    }
    
}