using System.ComponentModel.DataAnnotations;
using budget_backend.domain;

namespace budget_backend.data.dbDto;

public class TransactionDto
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid AccountId { get; set; }
    
    public DateOnly Timestamp  { get; set; }
    
    public double Amount { get; set; }
}

public static class TransactionDtoExtensions
{
    public static TransactionDto ToDbDto(this Transaction transaction)
    {
        return new TransactionDto()
        {
            Id = transaction.Id,
            AccountId = transaction.Account.Id,
            Timestamp = transaction.Timestamp,
            Amount = transaction.Amount
        };
    }
    
}