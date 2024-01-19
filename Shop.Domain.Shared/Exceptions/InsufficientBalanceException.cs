namespace Shop.Api.Exceptions;

public class InsufficientBalanceException : Exception, IBusinessLogicException
{
    public InsufficientBalanceException() { }

    public InsufficientBalanceException(string message) : base(message) { }

    public InsufficientBalanceException(string message, Exception inner) : base(message, inner) { }
}