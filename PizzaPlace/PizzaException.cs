namespace PizzaPlace;

public class PizzaException : Exception
{
    public PizzaException()
    {
    }

    public PizzaException(string? message) : base(message)
    {
    }

    public PizzaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
