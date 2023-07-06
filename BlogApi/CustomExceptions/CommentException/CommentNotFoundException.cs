namespace BlogApi.CustomExceptions.CommentException;

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException()
        : base("Comment not found!")
    { }
}