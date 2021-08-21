using System.Collections.Generic;

namespace Dev.Core.Model.Pagination
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T>
    {
        /// <summary>
        /// Skip
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Take
        /// </summary>
        int Take { get; }

        /// <summary>
        /// Total count
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Total pages
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Next Page
        /// </summary>
        int? NextPage { get; set; }

        /// <summary>
        /// Previous Page
        /// </summary>
        int? PreviousPage { get; set; }

        /// <summary>
        /// Has previous page
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Has next age
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Data
        /// </summary>
        IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Data Filter
        /// </summary>
        List<Filter> Filters { get; set; }

        /// <summary>
        /// Data Sort OrderBy and OrderBy Desc
        /// </summary>
        List<Sort> Sorts { get; set; }
    }
}
