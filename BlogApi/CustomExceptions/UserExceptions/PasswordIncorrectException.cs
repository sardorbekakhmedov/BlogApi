namespace BlogApi.CustomExceptions.UserExceptions;

public class PasswordIncorrectException : Exception
{
    public string Password { get; set; } = "password01";

    public PasswordIncorrectException(string password)
        : base($"{password}:  Password incorrect!")
    {

    }
}