/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      struct
 *          Pagination
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common
{
    using System;


    /// <summary>
    /// 分页信息。
    /// </summary>
    public ref struct Pagination
    {
        /// <summary>
        /// 初始化分页信息。
        /// </summary>
        /// <param name="pageNumber">页码。</param>
        /// <param name="pageSize">期望每页的记录数。</param>
        public Pagination(int pageNumber, int pageSize)
            : this(pageNumber, pageSize, 0) { }
        /// <summary>
        /// 初始化分页信息。
        /// </summary>
        /// <param name="pageNumber">页码。</param>
        /// <param name="pageSize">期望每页的记录数。</param>
        /// <param name="totalItems">总记录数。</param>
        public Pagination(int pageNumber, int pageSize, int totalItems)
        {
            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, Properties.Localization.the_page_number_must_be_greater_than_zero);
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, Properties.Localization.the_page_size_must_be_greater_than_zero);
            if (totalItems < 0) totalItems = 0;

            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        /// <summary>
        /// 页码。
        /// </summary>
        public int PageNumber { get; }
        /// <summary>
        /// 期望每页的记录数。
        /// </summary>
        public int PageSize { get; }
        /// <summary>
        /// 总记录数。
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// 总页数。
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (TotalItems <= 0)
                    return 0;
                else if (PageSize >= TotalItems)
                    return 1;
                else
                {
                    int result = Math.DivRem(TotalItems, PageSize, out int remainder);
                    if (remainder > 0) result++;
                    return result;
                }
            }
        }
        /// <summary>
        /// 要跳过的记录数。
        /// </summary>
        public int Skip => (PageNumber - 1) * PageSize;
    }
}
