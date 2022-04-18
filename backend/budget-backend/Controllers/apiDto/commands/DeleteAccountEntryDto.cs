namespace budget_backend.Controllers.apiDto.commands;

public record DeleteAccountEntryDto
{
    public Guid AccountEntryId { get; set; }
}