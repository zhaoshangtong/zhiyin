using Zhiyin.Filter;
using System.Web;
using System.Web.Mvc;

namespace Zhiyin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
