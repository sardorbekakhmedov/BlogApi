namespace BlogApi.CustomExceptions.PostLikeException;

public class PostLikeNotFoundException : Exception
{
    public PostLikeNotFoundException() 
        :base("PostLike not found!")
    { }
}