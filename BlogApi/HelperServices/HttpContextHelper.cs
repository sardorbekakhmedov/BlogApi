namespace BlogApi.HelperServices;

public class HttpContextHelper
{
    private readonly IHttpContextAccessor _accessor;
    public HttpContext? CurrentContext => _accessor.HttpContext;

    public HttpContextHelper(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public void AddResponseHeader(string key, string value)
    {
        if (CurrentContext is not null && CurrentContext.Response.Headers.Keys.Contains(key))
        {
            CurrentContext.Response.Headers.Remove(key);
        }

        if (CurrentContext is null) 
            return;

        CurrentContext.Response.Headers.Add("Access-Control-Expose-Headers", key);
        CurrentContext.Response.Headers.Add(key, value);
    }
}