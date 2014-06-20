using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using $customNamespace$.Models;

namespace $customNamespace$.UI.Web.Models
{
    public class MenuModel : baseModel
    {
        public MenuModel()
        {
            this.MenuItems = new List<MenuItemModel>();
        }
        public List<MenuItemModel> MenuItems { get; set; }
        public IHtmlString Render()
        {
            TagBuilder ul = new TagBuilder("ul");
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
        }
        public string DataAction { get; set; }
        public string Description { get; set; }
        public List<MenuItemModel> Childs { get; set; }
        public IHtmlString Render()
        {
            TagBuilder li = new TagBuilder("li");

            if (this.Childs.Count > 0)
            {
                TagBuilder ul = new TagBuilder("ul");
                foreach (var item in this.Childs)
                {
                    ul.InnerHtml += item.Render();
                }
                li.InnerHtml = ul.ToString(TagRenderMode.Normal);
            }

            TagBuilder label = new TagBuilder("a");
            label.SetInnerText(this.Description);
            
            

            if (!string.IsNullOrEmpty(this.DataAction))
            {
                li.Attributes.Add("data-action", this.DataAction);
                label.Attributes.Add("href", this.DataAction);
            }

            li.InnerHtml += label.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(li.ToString(TagRenderMode.Normal));
        }
    }
}