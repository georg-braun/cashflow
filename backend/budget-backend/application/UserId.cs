namespace budget_backend.application;

public class UserId
{
    public Guid Id { get; init; } = Guid.Empty;

    public bool IsEmpty => Id.Equals(Guid.Empty);
    public bool IsValid => !IsEmpty;
    public static UserId Empty => new();
    public static UserId New(Guid id) => new() {Id= id};

    
}
