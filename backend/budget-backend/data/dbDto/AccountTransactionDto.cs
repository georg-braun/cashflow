using System.ComponentModel.DataAnnotations;
using budget_backend.domain;

namespace budget_backend.data.dbDto;

public class AccountTransactionDto
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid AccountIdFrom { get; set; }
    
    public Guid AccountIdTo { get; set; }

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
            AccountIdFrom = accountTransaction.From.Id,
            AccountIdTo = accountTransaction.To.Id,
   
        };
    }

    public static AccountTransaction ToDomain(this AccountTransactionDto accountTransactionDto, AccountEntry from, AccountEntry to)
    {
        return new AccountTransaction(accountTransactionDto.Id, from, to);
    }
    
}