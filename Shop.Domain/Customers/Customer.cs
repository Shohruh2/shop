namespace Shop.Domain.Customers;

public class Customer
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Surname { get; set; }
    
    public required string Gender { get; set; }
    
    public required DateTime Birthday { get; set; }
    
    public required decimal Balance { get; set; }
}