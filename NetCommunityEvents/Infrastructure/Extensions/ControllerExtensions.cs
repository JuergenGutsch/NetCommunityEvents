using System;
using System.Web.Mvc;
using System.Web.Routing;
using NetCommunityEvents.Controllers;
using NetCommunityEvents.Infrastructure.Auth;

namespace NetCommunityEvents.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static string Action(this ControllerBase controller, string controllerName, string actionName)
        {
            RouteValueDictionary rvd = new RouteValueDictionary
                                           {
                                               {"controller", controllerName}, 
                                               {"action", actionName}
                                           };
            return RouteTable.Routes.GetVirtualPath(controller.ControllerContext.RequestContext, rvd).VirtualPath;
        }

        public static RedirectToRouteResult RedirectToActionWithId(this Controller controller, string controllerName, string actionName, string id)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary
                                                  {
                                                      {"controller", controllerName}, 
                                                      {"action", actionName}, 
                                                      {"id", id}
                                                  };
            return new RedirectToRouteResult(dictionary);
        }

        public static RedirectToRouteResult RedirectToActionWithId(this Controller controller, string controllerName, string actionName, Guid id)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary
                                                  {
                                                      {"controller", controllerName}, 
                                                      {"action", actionName}, 
                                                      {"id", id}
                                                  };
            return new RedirectToRouteResult(dictionary);
        }

        public static void CreateValidationMessage(this ControllerBase controller, string key, string message)
        {
            controller.ViewData.ModelState.Remove(key);
            ModelState modalState = new ModelState
                                        {
                                            Value = controller.ValueProvider.GetValue(key)
                                        };
            modalState.Errors.Add(new ModelError(message));
            controller.ViewData.ModelState.Add(key, modalState);
        }

        public static bool IsAuthenticated(this Controller controller)
        {
            return controller.HttpContext.User.Identity.IsAuthenticated;
        }

        public static bool IsCurrentUserId(this Controller controller, Guid userId)
        {
            CustomIdentity identity = new CustomIdentity(controller.HttpContext.User.Identity);
            return identity.User.Id == userId;
        }

        public static bool IsInRole(this Controller controller, Role role)
        {
            CustomPrincipal prinzipal = new CustomPrincipal(controller.HttpContext.User.Identity);
            return prinzipal.IsInRole(role);
        }

    }
}