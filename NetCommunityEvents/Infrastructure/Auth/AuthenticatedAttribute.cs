using System.Web.Mvc;
using System.Web.Routing;

namespace NetCommunityEvents.Infrastructure.Auth
{
    public class IsAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
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