using System.Web.Mvc;
using System.Web.Routing;
using NetCommunityEvents.Controllers;

namespace NetCommunityEvents.Infrastructure.Auth
{
    public class UserInRoleAttribute : ActionFilterAttribute
    {
        public Role Role { get; private set; }

        public UserInRoleAttribute(Role role)
        {
            Role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CustomPrincipal prinzipal = new CustomPrincipal(filterContext.HttpContext.User.Identity);
            if (prinzipal.IsInRole(Role))
            {
                return;
            }

            RouteValueDictionary dictionary = new RouteValueDictionary
                                                  {
                                                      {"controller", "Content"}, 
                                                      {"action", "Show"}, 
                                                      {"id", "AccessDenied"}
                                                  };
            filterContext.Result = new RedirectToRouteResult(dictionary);
        }
    }
}