namespace Shop.Contracts.Responses;

public class Result<TResponse>
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
    public TResponse Response { get; set; }
}