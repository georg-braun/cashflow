namespace budget_backend.Controllers.apiDto.commands;

public record AddNewAccountDto
{
    public string Name { get; set; } = string.Empty;
}