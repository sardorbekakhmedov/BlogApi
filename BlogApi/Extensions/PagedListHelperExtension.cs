using BlogApi.HelperEntities;
using BlogApi.HelperServices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlogApi.Extensions;

public static class PagedListHelperExtension
{
    public static async Task<IEnumerable<T>> ToPagedListAsync<T>(this List<T> source, PaginationParams pageParams)
    {
        pageParams ??= new PaginationParams();

        HttpContextHelper.AddResponseHeader("X-Pagination", 
            JsonConvert.SerializeObject(new Pagination(source.Count, pageParams.Size, pageParams.Page)));

        return  source.Skip(pageParams.Size * (pageParams.Page - 1)).Take(pageParams.Size);
    }
}