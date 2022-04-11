namespace budget_backend.Controllers.apiDto.commands;

public record DeleteAccountDto
{
    public Guid AccountId { get; set; }
}