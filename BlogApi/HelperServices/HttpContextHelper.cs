namespace BlogApi.HelperServices;

public class HttpContextHelper
{
    public static IHttpContextAccessor Accessor;
    public static HttpContext? CurrentContext => Accessor.HttpContext;

    public static void AddResponseHeader(string key, string value)
    {
        if (CurrentContext is not null && CurrentContext.Response.Headers.Keys.Contains(key))
        {
            CurrentContext.Response.Headers.Remove(key);
        }

        if (CurrentContext is not null)
        {
            CurrentContext.Response.Headers.Add("Access-Control-Expose-Headers", key);
            CurrentContext.Response.Headers.Add(key, value);
        }
    }
}