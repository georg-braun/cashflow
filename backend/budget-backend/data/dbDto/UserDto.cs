namespace budget_backend.data;

internal class UserDto
{
    public UserDto(Guid id, string authProviderId, DateTime created)
    {
        Id = id;
        AuthProviderId = authProviderId;
        Created = created;
    }

    /// <summary>
    ///  Id that is associated with the datasets and independent of a provider
    /// </summary>
    public Guid Id { get; set; }
    
    public string AuthProviderId { get; set; }
    
    /// <summary>
    ///     Date of the user creation
    /// </summary>
    public DateTime Created { get; set; }
}