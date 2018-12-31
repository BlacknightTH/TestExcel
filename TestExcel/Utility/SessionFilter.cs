using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestExcel.Utility
{
    public class SessionTimeout : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["status"] == null)
            {
                filterContext.Result = new RedirectResult("~/login/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class adminauthen : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            //if (HttpContext.Current.Session["status"] == null || HttpContext.Current.Session["status"].ToString() != "admin")
            if (HttpContext.Current.Session["status"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}