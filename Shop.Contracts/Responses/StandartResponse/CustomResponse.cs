namespace Shop.Contracts.Responses.StandartResponse;

public class CustomResponse<TResult>
{
    public bool IsSuccessful { get; set; }
    
    public string? Message { get; set; }
    
    public ResponseError? Error { get; set; }
    
    public TResult? Result { get; set; }

    public static CustomResponse<TResult> CreateSuccessResponse(TResult result)
    {
        return new CustomResponse<TResult>
        {
            IsSuccessful = true,
            Result = result
        };
    }
    
    public static CustomResponse<TResult> CreateErrorResponse(ResponseError error)
    {
        return new CustomResponse<TResult>
        {
            IsSuccessful = false,
            Error = error
        };
    }
}

public class CustomResponse : CustomResponse<object>
{
    
}