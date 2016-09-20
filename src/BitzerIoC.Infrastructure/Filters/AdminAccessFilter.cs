using BitzerIoC.Infrastructure.AppConstants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BitzerIoC.Infrastructure.Filters
{
    /// <summary>
    /// ToDo: update role name
    /// </summary>
    public class AdminAccessFilter: ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.HasClaim("role", RoleConstants.AdminRoleId))
            {
                // do nothing
            }
            else
            {
                context.Result = new UnauthorizedResult(); // mark unauthorized
            }
        }

    }


}
