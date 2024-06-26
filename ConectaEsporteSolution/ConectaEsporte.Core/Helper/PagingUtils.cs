using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Helper
{
   
    static class PagingUtils
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> en, int pageSize, int page)
        {
            return en.Skip(page * pageSize).Take(pageSize);
        }
        public static IQueryable<T> Page<T>(this IQueryable<T> en, int pageSize, int page)
        {
            return en.Skip(page * pageSize).Take(pageSize);
        }
    }
}
