namespace Shop.Application;

public class CurrentUser
{
    public required Guid Id { get; set; }
    
    public required string UserName { get; set; }
    
    public required string GivenName { get; set; }
    
    public required string MiddleName { get; set; }
    
    public required DateTime Birthdate { get; set; }
    
    public required string Gender { get; set; }
    
    public required string? Email { get; set; }
}