using System;
using System.Collections.Generic;
using System.Web.Mvc;
using $customNamespace$.Models;
using $customNamespace$.Resources.General;
using $customNamespace$.UI.Web.Areas.Home;

namespace $customNamespace$.UI.Web.Models
{
    public class Breadcrumb : baseModel, IDisposable
    {
        public List<KeyValuePair<string, string>> BreadcrumbPaths { get; set; }
        public bool IsVisible { get; set; }
        public Breadcrumb()
        {
            this.IsVisible = false;

            UrlHelper urlHelper = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
            this.BreadcrumbPaths = new List<KeyValuePair<string, string>>();
            this.BreadcrumbPaths.Add(new KeyValuePair<string, string>(GeneralTexts.Home, HomeUrlHelper.Home_Index(urlHelper)));
        }
        public void Dispose()
        {

        }
    }
}