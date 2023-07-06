namespace BlogApi.CustomExceptions.PostException;

public class PostNotFoundException : Exception
{
    public PostNotFoundException() : base("Post not found!")
    { }
}