using System.Collections.Generic;
using System.Linq;

namespace BNE.Web.LanHouse.Code.Paginacao
{
    public class LimitLessPagedCollection<T> : PagedList<T>
    {
        public LimitLessPagedCollection(IEnumerable<T> source, int index, int pageSize, int? totalCount = null)
            : base(source, index, pageSize, totalCount)
        {
        }

        public LimitLessPagedCollection(IQueryable<T> source, int index, int pageSize, int? totalCount = null)
            : base(source, index, pageSize, totalCount)
        {
        }

        public override void Populate(IQueryable<T> source, int realTotalPages, int realTotalCount)
        {
            AddRange(source);
        }
    }
}