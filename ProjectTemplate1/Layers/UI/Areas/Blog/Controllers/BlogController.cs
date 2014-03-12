using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Syndication;
using $customNamespace$.Models.Unity;
using $safeprojectname$.Areas.Blog.Models;
using $safeprojectname$.Controllers;
using $safeprojectname$.Common.Mvc.Html;

namespace $safeprojectname$.Areas.Blog.Controllers
{
    public class BlogController : Controller, IControllerWithClientResources
    {
        public string[] GetControllerJavascriptResources
        {
            get { return new string[0]; }
        }

        public string[] GetControllerStyleSheetResources
        {
            get { return new string[1] { "~/Areas/Blog/Content/blog.css" }; }
        }

        private ISyndicationProxy ProviderSyndication = null;

        public BlogController()
        {
            this.ProviderSyndication = DependencyFactory.Resolve<ISyndicationProxy>();
        }

        protected override void Dispose(bool disposing)
        {
            if (this.ProviderSyndication != null)
            {
                this.ProviderSyndication.Dispose();
            }

            base.Dispose(disposing);
        }

        public ActionResult Index(string title)
        {
            ActionResult result = null;
            BlogModel model = null;

            if (string.IsNullOrEmpty(title))
            {
                model = this.GetIndexModel();
            }
            else
            {
                model = this.GetIndexModelByTitle(title);

                if (model.FeedItems.TotalRows == 0)
                {
                    result = new RedirectResult(Areas.Error.ErrorUrlHelper.NotFound404(this.Url));
                }
            }

            if(result==null)
            {
                result = View(model);
            }

            return result;
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Category(string categoryName, BlogModel model)
        {
            ActionResult result = null;

            if (string.IsNullOrEmpty(categoryName))
            {
                model = this.GetCategoryModel();
            }
            else
            {
                model = this.GetCategoryModel(categoryName, model);
            }

            if (result == null)
            {
                result = View(model);
            }

            return result;
        }
        private BlogModel GetIndexModel()
        {
            BlogModel model = new BlogModel();
            model.FeedId = string.Empty;
            model.FeedFormatter = this.ProviderSyndication.GetInfoWithNoItems();
            model.FeedFilter = new DataFilterSyndication() { Page = 0, PageSize = (int)$customNamespace$.Models.Enumerations.PageSizesAvailable.RowsPerPage10 };
            model.FeedItems = this.ProviderSyndication.GetLast(model.FeedFilter);
            model.BaseViewModelInfo.Title = model.FeedFormatter.Feed.Title.Text;
            return model;
        }
        private BlogModel GetIndexModelByTitle(string title)
        {
            BlogModel model = new BlogModel();
            model.FeedId = string.Empty;
            model.FeedFormatter = this.ProviderSyndication.GetInfoWithNoItems();
            model.FeedFilter = new DataFilterSyndication() { Uri = title, Page = 0, PageSize = (int)$customNamespace$.Models.Enumerations.PageSizesAvailable.RowsPerPage10 };
            model.FeedItems = this.ProviderSyndication.GetByTitle(model.FeedFilter);
            model.BaseViewModelInfo.Title = model.FeedFormatter.Feed.Title.Text;
            if (model.FeedItems.TotalRows > 0)
            {
                model.BaseViewModelInfo.Title = model.FeedItems.Data.First().Item.Title.Text;
            }
            return model;
        }
        private BlogModel GetCategoryModel()
        {
            BlogModel model = new BlogModel();
            model.FeedId = string.Empty;
            model.FeedFormatter = this.ProviderSyndication.GetInfoWithNoItems();
            model.FeedFilter = new DataFilterSyndication() { Page = 0, PageSize = (int)$customNamespace$.Models.Enumerations.PageSizesAvailable.RowsPerPage10 };
            model.BaseViewModelInfo.Title = model.FeedFormatter.Feed.Title.Text;
            return model;
        }
        private BlogModel GetCategoryModel(string categoryName, BlogModel model)
        {
            model.FeedId = categoryName;

            if (this.RequestType() == HttpVerbs.Get)
            {
                model = new BlogModel()
                {
                    FeedId = categoryName,
                    FeedFilter = new DataFilterSyndication()
                    {
                        CategoryName = categoryName,
                        Page = 0,
                        PageSize = (int)$customNamespace$.Models.Enumerations.PageSizesAvailable.RowsPerPage10
                    }
                };
            }
            else
            {
                if (WebGrid<SyndicationItemFormatter, BlogModel, DataFilterSyndication>.IsWebGridEvent())
                {
                    this.ModelState.Clear();
                    model.FeedFilter = (DataFilterSyndication)WebGrid<SyndicationItemFormatter, BlogModel, DataFilterSyndication>.GetDataFilterFromPost();
                }
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                model.FeedItems = this.ProviderSyndication.GetByCategory(model.FeedFilter);
                model.FeedFormatter = this.ProviderSyndication.GetInfoWithNoItems();
                model.BaseViewModelInfo.Title = string.Format("{0} - {1}", categoryName, model.FeedFormatter.Feed.Title.Text);
            }

            return model;
        }
    }
}
