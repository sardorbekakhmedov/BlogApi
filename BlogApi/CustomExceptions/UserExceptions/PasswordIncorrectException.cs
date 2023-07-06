namespace BlogApi.CustomExceptions.UserExceptions;

public class PasswordIncorrectException : Exception
{
    public PasswordIncorrectException(string password)
        : base($"{password}:  Password incorrect!")
    { }
}