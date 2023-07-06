namespace BlogApi.CustomExceptions.UserExceptions;

public class UsernameIncorrectException : Exception
{
    public UsernameIncorrectException(string username)
        : base($"{username}: Username incorrect!")
    { }
}