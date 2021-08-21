using Dev.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dev.Core.Model.Pagination
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="skip">Skip</param>
        /// <param name="take">Take</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <param name="filters">Data Filter </param>
        /// <param name="sorts">Data Sort OrderBy and OrderBy Desc</param>
        public PagedList(IQueryable<T> source, int skip, int take, List<Filter> filters = null, List<Sort> sorts = null, bool getOnlyTotalCount = false)
        {
            var total = source.Count();
            TotalCount = total;
            TotalPages = total / take;

            if (total % take > 0)
                TotalPages++;

            Take = take;
            Skip = skip;
            if (getOnlyTotalCount)
                return;

            Filters = filters;
            Sorts = sorts;

            #region dxGrid
            Data = source.AddFilterQuery(Filters).Skip(skip).Take(take).AddSortQuery(Sorts).ToList();
            #endregion

            #region NormalPage
            //            Data = source.AddFilterQuery(Filters).Skip(skip * take).Take(take).AddSortQuery(Sorts).ToList();
            #endregion
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="skip">Skip</param>
        /// <param name="take">Take</param>
        /// <param name="filters">Data Filter </param>
        /// <param name="sorts">Data Sort OrderBy and OrderBy Desc</param>
        public PagedList(IList<T> source, int skip, int take, List<Filter> filters = null, List<Sort> sorts = null)
        {
            TotalCount = source.Count;
            TotalPages = TotalCount / take;

            if (TotalCount % take > 0)
                TotalPages++;

            Take = take;
            Skip = skip;

            Filters = filters;
            Sorts = sorts;

            Data = source.AsQueryable().AddFilterQuery(Filters).Skip(skip * take).Take(take).AddSortQuery(Sorts).ToList();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="skip">Skip</param>
        /// <param name="take">Take</param>
        /// <param name="totalCount">Total count</param>
        /// <param name="filters">Data Filter </param>
        /// <param name="sorts">Data Sort OrderBy and OrderBy Desc</param>
        public PagedList(IEnumerable<T> source, int skip, int take, int totalCount, List<Filter> filters = null, List<Sort> sorts = null)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / take;

            if (TotalCount % take > 0)
                TotalPages++;

            Take = take;
            Skip = skip;

            Filters = filters;
            Sorts = sorts;

            Data = source;
        }


        /// <summary>
        /// Skip
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Take
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Total count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Next Page
        /// </summary>
        public int? NextPage { get; set; }

        /// <summary>
        /// Previous Page
        /// </summary>
        public int? PreviousPage { get; set; }

        /// <summary>
        /// Has previous page
        /// </summary>
        public bool HasPreviousPage => PreviousPage != null && Take > 0;

        /// <summary>
        /// Has next page
        /// </summary>
        public bool HasNextPage => Take + 1 < TotalPages;

        /// <summary>
        /// DB Data
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Data Filter
        /// </summary>
        public List<Filter> Filters { get; set; }

        /// <summary>
        /// Data Sort OrderBy and OrderBy Desc
        /// </summary>
        public List<Sort> Sorts { get; set; }
    }
}
