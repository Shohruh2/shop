using System.Net;

namespace Shop.Contracts.Responses;

public class Response<TResult>
{
    public bool IsSuccessful { get; set; }
    
    public string? Message { get; set; }
    
    public ResponseError? Error { get; set; }
    
    public TResult? Result { get; set; }

    public static Response<TResult> CreateSuccessResponse(TResult result)
    {
        return new Response<TResult>
        {
            IsSuccessful = true,
            Result = result
        };
    }
    
    public static Response<TResult> CreateErrorResponse(ResponseError error)
    {
        return new Response<TResult>
        {
            IsSuccessful = false,
            Error = error
        };
    }
}

public class Response : Response<object>
{
    
}