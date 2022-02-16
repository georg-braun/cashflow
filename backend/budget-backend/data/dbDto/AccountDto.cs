using System.ComponentModel.DataAnnotations;

namespace budget_backend.data.dbDto;

public class AccountDto
{
    [Key] 
    public Guid Id { get; set; }

    public string Name { get; set; }
}