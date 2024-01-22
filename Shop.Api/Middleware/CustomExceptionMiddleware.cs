using System.Net;
using Newtonsoft.Json;
using Shop.Contracts.Responses;

namespace Shop.Api.Middleware;

public class CustomExceptionMiddleware : IMiddleware
{
    private readonly ILogger<CustomExceptionMiddleware> _logger;

    public CustomExceptionMiddleware(ILogger<CustomExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var responseError = new ResponseError
        {
            Code = "",
            Message = exception.Message
        };
        var result = JsonConvert.SerializeObject(Response.CreateErrorResponse(responseError));
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
        return context.Response.WriteAsync(result);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exceptionObj)
        {
            _logger.LogError("Failed with exception: {message}\n{stacktrace}", exceptionObj.Message, exceptionObj.StackTrace);
            await HandleExceptionAsync(context, exceptionObj);
        }
    }
}