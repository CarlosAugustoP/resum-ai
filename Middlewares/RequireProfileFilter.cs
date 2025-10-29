using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Resumai.Middlewares
{
    public class RequireProfileFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"];
            if (user == null)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(new { message = "Unauthorized" });
                return;
            }
        }
    }
}