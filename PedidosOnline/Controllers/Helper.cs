using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Models;


namespace PedidosOnline.Controllers
{

    public class CheckSessionOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();

            //VALIDACIONES DE LOGIN
            //if (!controllerName.Contains("Cuenta"))
            //{
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Usuario user = (Usuario)session["curUser"];
            if (((user == null ) && (!session.IsNewSession)) || (session.IsNewSession))
            {
                //send them off to the login page
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("~/Account/Login");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);

            }
            //if (DatosCliente.UsuarioLogeado.RowID == 0)
            //{
            //    //send them off to the login page
            //    var url = new UrlHelper(filterContext.RequestContext);
            //    var loginUrl = url.Content("~/Inicio/Index");
            //    filterContext.HttpContext.Response.Redirect(loginUrl, true);
            //}
            //}

        }
    }

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    
    //public class CheckSessionOutAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
            
    //        string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();

    //        HttpSessionStateBase session = filterContext.HttpContext.Session;
            
    //        if (!controllerName.Contains("account") && !controllerName.Contains("svc") && !controllerName.Contains("mobile") && !controllerName.Contains("ecommerce") && !controllerName.Contains("senddata"))
    //        {

    //            if (DatosCliente.UsuarioLogeado.RowID > 0)
    //            {
    //                //send them off to the login page
    //                var url = new UrlHelper(filterContext.RequestContext);
    //                var loginUrl = url.Content("~/Account/Login");                    
    //                filterContext.HttpContext.Response.Redirect(loginUrl, true);

    //            }
    //            //else if (user.PrimerInicio == true)
    //            //{
    //            //    var url = new UrlHelper(filterContext.RequestContext);
    //            //    var loginUrl = url.Content("~/Account/CambiarClave");
    //            //    filterContext.HttpContext.Response.Redirect(loginUrl, true);
    //            //}
    //            //else if (user.acepto_condiciones == null)
    //            //{
    //            //    var url = new UrlHelper(filterContext.RequestContext);
    //            //    var loginUrl = url.Content("~/Account/AceptarAcuerdo");
    //            //    filterContext.HttpContext.Response.Redirect(loginUrl, true);
    //            //}
    //        }


    //    }
    //}


    //public class NoCache : ActionFilterAttribute
    //{
    //    public override void OnResultExecuting(ResultExecutingContext filterContext)
    //    {
    //        filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
    //        filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
    //        filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
    //        filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //        filterContext.HttpContext.Response.Cache.SetNoStore();

    //        base.OnResultExecuting(filterContext);
    //    }
    //}


    //public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    //{
    //    public override void OnResultExecuting(ResultExecutingContext filterContext)
    //    {
    //        if (filterContext.HttpContext.Response != null)
    //            filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

    //        base.OnResultExecuting(filterContext);
    //    }
    //}

}