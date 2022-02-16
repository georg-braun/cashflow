using System.ComponentModel.DataAnnotations;

namespace budget_backend.data.dbDto;

public class TransactionDto
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid AccountId { get; set; }
    
    public DateOnly Timestamp  { get; set; }
    
    public double Amount { get; set; }
}