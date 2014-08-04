using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using $customNamespace$.Models;
using $customNamespace$.Models.Enumerations;

namespace $customNamespace$.UI.Web.Models
{
    public class MenuModel : baseModel
    {
        public MenuModel()
        {
            this.MenuItems = new List<MenuItemModel>();
        }
        public List<MenuItemModel> MenuItems { get; set; }
        public IHtmlString Render(object htmlAttributes = null)
        {
            TagBuilder ul = new TagBuilder("ul");
            ul.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            foreach (var item in this.MenuItems)
            {
                ul.InnerHtml += item.Render();
            }
            return MvcHtmlString.Create(ul.ToString(TagRenderMode.Normal));
        }

    }

    public class MenuItemModel : baseModel
    {
        public MenuItemModel()
        {
            this.Childs = new List<MenuItemModel>();
            this.RolesAllowed = new List<SiteRoles>();
        }

        public MenuItemModel(string dataAction, string description, List<SiteRoles> rolesAllowed, List<MenuItemModel> childs)
        {
            this.DataAction = dataAction;
            this.Description = description;
            this.RolesAllowed = rolesAllowed == null? new List<SiteRoles>(): rolesAllowed;
            this.Childs = childs == null ? new List<MenuItemModel>() : childs;
        }

        public string DataAction { get; set; }
        public string Description { get; set; }
        public List<SiteRoles> RolesAllowed { get; set; }
        public List<MenuItemModel> Childs { get; set; }
        public IHtmlString Render()
        {
            TagBuilder li = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");
            link.SetInnerText(this.Description);
            if (!string.IsNullOrEmpty(this.DataAction))
            {
                li.Attributes.Add("data-action", this.DataAction);
                link.Attributes.Add("href", this.DataAction);
            }
            else
            {
                link.Attributes.Add("href", "javascript:void(0);");
            }

            li.InnerHtml += link.ToString(TagRenderMode.Normal);

            if (this.Childs.Count > 0)
            {
                TagBuilder ul = new TagBuilder("ul");
                foreach (var item in this.Childs)
                {
                    ul.InnerHtml += item.Render();
                }
                li.InnerHtml += ul.ToString(TagRenderMode.Normal);
            }

            return MvcHtmlString.Create(li.ToString(TagRenderMode.Normal));
        }
    }
}