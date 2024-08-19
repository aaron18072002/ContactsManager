using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ResourceFilters
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisabledResourceFilter> _logger;
        private bool _isDisabled;
        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled)
        {
            this._logger = logger;
            this._isDisabled = isDisabled;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method before",
                nameof(FeatureDisabledResourceFilter), nameof(this.OnResourceExecutionAsync));

            if (this._isDisabled)
            {
                context.Result = new StatusCodeResult(501); //not implemented
            }
            else
            {
                await next();
            }

            this._logger.LogInformation("{FilterName}.{MethodName} method after",
                nameof(FeatureDisabledResourceFilter), nameof(this.OnResourceExecutionAsync));
        }
    }
}
