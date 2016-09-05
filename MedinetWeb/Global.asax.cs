using MedinetWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace MedinetWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            Server.ClearError();

            int statusCode = 0;

            if (lastError != null)
            {
                if (lastError.GetType() == typeof(HttpException))
                {
                    statusCode = ((HttpException)lastError).GetHttpCode();
                }
                else
                {
                    // Not an HTTP related error so this is a problem in our code, set status to
                    // 500 (internal server error)
                    statusCode = 500;
                }

                HttpContextWrapper contextWrapper = new HttpContextWrapper(this.Context);

                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Index");
                routeData.Values.Add("statusCode", statusCode);
                routeData.Values.Add("exception", lastError);
                routeData.Values.Add("isAjaxRequet", contextWrapper.Request.IsAjaxRequest());

                IController controller = new ErrorController();

                RequestContext requestContext = new RequestContext(contextWrapper, routeData);

                controller.Execute(requestContext);
                Response.End();
            }
        }
    }

    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            
            // check if session is supported
            if (ctx.Session != null)
            {
                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];

                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        ctx.Response.Redirect("~/Acceso/Login");
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}