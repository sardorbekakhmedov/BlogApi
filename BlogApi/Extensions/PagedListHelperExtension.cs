using BlogApi.HelperEntities.Pagination;
using BlogApi.HelperServices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlogApi.Extensions;

public static class PagedListHelperExtension
{
    public static async Task<IEnumerable<T>> ToPagedListAsync<T>(this IQueryable<T> source, PaginationParams? pageParams, HttpContextHelper httpContextHelper)
    {
        pageParams ??= new PaginationParams();

        httpContextHelper.AddResponseHeader("X-Pagination",
            JsonConvert.SerializeObject(new PaginationMetaData(source.Count(), pageParams.Size, pageParams.Page)));

        return await source.Skip(pageParams.Size * (pageParams.Page - 1)).Take(pageParams.Size).ToListAsync();
    }


    public static async Task<IEnumerable<T>> ToPagedListAsync<T>(this List<T> source, PaginationParams? pageParams, HttpContextHelper httpContextHelper)
    {
        pageParams ??= new PaginationParams();

        httpContextHelper.AddResponseHeader("X-Pagination",
            JsonConvert.SerializeObject(new PaginationMetaData(source.Count, pageParams.Size, pageParams.Page)));

        return await Task.FromResult(source.Skip(pageParams.Size * (pageParams.Page - 1)).Take(pageParams.Size));
    }
}