using System;
using System.Collections.Generic;
using System.Linq;

namespace AoApi.Services.Data.Helpers
{
    /// <summary>
    /// Pagedlist class that extends List<T> with extra properties that contain pagination information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        /// Create a paged list isntance
        /// </summary>
        /// <param name="source">List of items to add to the paged list</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">the size of items in pages</param>
        /// <returns>a new instance of PagedList<T></returns>
        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
