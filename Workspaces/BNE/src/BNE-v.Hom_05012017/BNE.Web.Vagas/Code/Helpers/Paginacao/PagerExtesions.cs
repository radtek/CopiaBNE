using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace BNE.Web.Vagas.Code.Helpers.Paginacao
{
    public static class PagerExtensions
    {
        #region HtmlHelper extensions

        public static Pager Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, NameValueCollection QueryString)
        {
            return new Pager(htmlHelper, pageSize, currentPage, totalItemCount, QueryString);
        }

        public static Pager Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, NameValueCollection QueryString, AjaxOptions ajaxOptions)
        {
            return new Pager(htmlHelper, pageSize, currentPage, totalItemCount, QueryString).Options(o => o.AjaxOptions(ajaxOptions));
        }

        #endregion

        #region IQueryable<T> extensions

        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }

        #endregion

        #region IEnumerable<T> extensions

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }

        public static IPagedList<T> ToLimitLessPagedCollection<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            return new LimitLessPagedCollection<T>(source, pageIndex, pageSize, totalCount);
        }
        #endregion
    }

  
}