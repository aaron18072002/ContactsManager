using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ResultFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Filters.OfType<SkipFilter>().Any())
            {
                return;
            }
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if(context.Filters.OfType<SkipFilter>().Any())
            {
                return;
            }
        }
    }
}
