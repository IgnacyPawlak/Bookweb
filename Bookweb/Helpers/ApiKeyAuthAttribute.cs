using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookweb.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string key = "thisisaverysafekeythatnoonecanreproducecuzofnextcharacters*&&*(8941198**7&39414unioujnfi8233u21983u";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var possiblyApiKeyValue)
                || possiblyApiKeyValue != key)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
