using LitNovel.Application.Common.Models;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace LitNovel.WebAPI.Common
{
    public static class ODataQueryResultFactory
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            IQueryable<T> source,
            ODataQueryOptions<T> options,
            Func<IQueryable<T>, IOrderedQueryable<T>> defaultOrderBy,
            int defaultPageSize,
            int maxTop,
            CancellationToken ct)
        {
            var settings = new ODataQuerySettings();
            var filtered = ApplyFilter(source, options, settings);
            var totalElements = await CountAsync(filtered, ct);
            var ordered = ApplyOrderBy(filtered, options, settings, defaultOrderBy);
            var skip = options.Skip?.Value ?? 0;
            var top = options.Top?.Value ?? defaultPageSize;
            var size = Math.Min(Math.Max(top, 1), maxTop);

            var itemsQuery = ordered.Skip(skip).Take(size);
            var items = await ToListAsync(itemsQuery, ct);

            return new PagedResult<T>
            {
                Items = items,
                Page = skip / size + 1,
                Size = size,
                TotalElements = totalElements,
                TotalPages = (int)Math.Ceiling(totalElements / (double)size)
            };
        }

        public static async Task<List<T>> ToListAsync<T>(
            IQueryable<T> source,
            ODataQueryOptions<T> options,
            Func<IQueryable<T>, IOrderedQueryable<T>> defaultOrderBy,
            int defaultTop,
            int maxTop,
            CancellationToken ct)
        {
            var settings = new ODataQuerySettings();
            var filtered = ApplyFilter(source, options, settings);
            var ordered = ApplyOrderBy(filtered, options, settings, defaultOrderBy);
            var skip = options.Skip?.Value ?? 0;
            var top = options.Top?.Value ?? defaultTop;
            var size = Math.Min(Math.Max(top, 1), maxTop);

            return await ToListAsync(ordered.Skip(skip).Take(size), ct);
        }

        private static IQueryable<T> ApplyFilter<T>(
            IQueryable<T> source,
            ODataQueryOptions<T> options,
            ODataQuerySettings settings)
        {
            return options.Filter == null
                ? source
                : (IQueryable<T>)options.Filter.ApplyTo(source, settings);
        }

        private static IQueryable<T> ApplyOrderBy<T>(
            IQueryable<T> source,
            ODataQueryOptions<T> options,
            ODataQuerySettings settings,
            Func<IQueryable<T>, IOrderedQueryable<T>> defaultOrderBy)
        {
            return options.OrderBy == null
                ? defaultOrderBy(source)
                : options.OrderBy.ApplyTo(source, settings);
        }

        private static Task<int> CountAsync<T>(IQueryable<T> source, CancellationToken ct)
        {
            return source.Provider is IAsyncQueryProvider
                ? EntityFrameworkQueryableExtensions.CountAsync(source, ct)
                : Task.FromResult(source.Count());
        }

        private static Task<List<T>> ToListAsync<T>(IQueryable<T> source, CancellationToken ct)
        {
            return source.Provider is IAsyncQueryProvider
                ? EntityFrameworkQueryableExtensions.ToListAsync(source, ct)
                : Task.FromResult(source.ToList());
        }
    }
}
