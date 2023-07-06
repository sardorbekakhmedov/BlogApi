namespace BlogApi.CustomExceptions.UserExceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User not found!")
    { }
}