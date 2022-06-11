using System.Web.Mvc;
using FilmsStorage.Filters;

namespace FilmsStorage
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogItAttribute());
        }
    }
}