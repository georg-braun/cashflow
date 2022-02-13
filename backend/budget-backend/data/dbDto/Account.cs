using System.ComponentModel.DataAnnotations;

namespace budget_backend.data.dbDto;

public class Account
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; }
}