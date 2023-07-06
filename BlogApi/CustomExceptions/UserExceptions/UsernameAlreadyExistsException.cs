namespace BlogApi.CustomExceptions.UserExceptions;

public class UsernameAlreadyExistsException : Exception
{
    public UsernameAlreadyExistsException(string username)
        : base($"{username}: Username already exists!")
    { }
}