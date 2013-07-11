using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Logging;
using $customNamespace$.Models.Unity;
using $customNamespace$.Resources.General;
using $safeprojectname$.Areas.LogViewer.Models;
using $safeprojectname$.Areas.UserAccount;
using $safeprojectname$.Controllers;
using $safeprojectname$.Common.Mvc.Html;

namespace $safeprojectname$.Areas.LogViewer.Controllers
{
    [$safeprojectname$.Common.Mvc.Attributes.Authorize(Roles = SiteRoles.Administrator)]
    public class LogViewerController : Controller, IControllerWithClientResources
    {
        public string[] GetControllerJavascriptResources
        {
            get { return new string[0]; }
        }
        public string[] GetControllerStyleSheetResources
        {
            get { return new string[0]; }
        }

        private const string LogginConfigurationSectionName = "loggingConfiguration";
        private LogViewerModel LogViewerSetBreadcrumb(LogViewerModel model, string traceSourceName, string traceListenerName)
        {
            model.BaseViewModelInfo.Breadcrumb.IsVisible = true;
            model.BaseViewModelInfo.Breadcrumb.BreadcrumbPaths.AddRange(new List<KeyValuePair<string, string>>() { 
                new KeyValuePair<string, string>(Resources.General.GeneralTexts.Dashboard, Url.Account_Dashboard()),
                new KeyValuePair<string, string>(Resources.General.GeneralTexts.LogViewer, Url.LogViewer()),
                new KeyValuePair<string, string>(string.Format("{0}", traceSourceName), Url.LogViewerBySourceName(traceSourceName)),
            });
            return model;
        }
        private LogViewerModel LogViewerModel_GetBaseModel(string sourceName, string listenerName)
        {
            LogViewerModel model = new LogViewerModel();
            model.BaseViewModelInfo.Title = $customNamespace$.Resources.General.GeneralTexts.LogViewer;
            model.LogTraceSources = (ConfigurationManager.GetSection(LogginConfigurationSectionName) as LoggingSettings).TraceSources;
            if (!string.IsNullOrEmpty(sourceName))
            {
                if (!model.LogTraceSources.Contains(sourceName))
                {
                    throw new Exception("Trace Data Source Not Found");
                }
                model.LogTraceSourceSelected = sourceName;
                model.LogTraceListeners = model.LogTraceSources.Where(x => x.Name == sourceName).First().TraceListeners;
                if (!string.IsNullOrEmpty(listenerName))
                {
                    model.LogTraceListenerSelected = listenerName;
                    if (!model.LogTraceListeners.Contains(listenerName))
                    {
                        throw new Exception("Trace Data Source Not Found");
                    }

                }
            }
            return model;
        }

        public ActionResult Index()
        {
            return View(LogViewerViewHelper.Index, this.LogViewerModel_GetBaseModel(string.Empty, string.Empty));
        }
        public ActionResult LogViewerBySourceName(string sourceName)
        {
            LogViewerModel model = this.LogViewerModel_GetBaseModel(sourceName, string.Empty);
            model = this.LogViewerSetBreadcrumb(model, sourceName, string.Empty);
            return View(LogViewerViewHelper.LogViewerBySourceName, model);
        }
        public ActionResult LogViewerByListenerName(string sourceName, string listenerName)
        {
            LogViewerModel model = this.LogViewerModel_GetBaseModel(sourceName, listenerName);

            model = this.LogViewerSetBreadcrumb(model, sourceName, listenerName);

            TraceListenerReferenceData traceListenerReference = model.LogTraceListeners.Where(x => x.Name == listenerName).First();
            TraceListenerData traceListener = (ConfigurationManager.GetSection(LogginConfigurationSectionName) as LoggingSettings).TraceListeners.Where(x => x.Name == traceListenerReference.Name).First();
            if (traceListener.Type == typeof(ProxiedWcfTraceListener))
            {
                return RedirectPermanent(LogViewerUrlHelper.LogViewerByProxiedWcfTraceListener(this.Url, model.LogTraceSourceSelected, model.LogTraceListenerSelected));
            }

            if (traceListener.Type == typeof(RollingXmlTraceListener))
            {
                return RedirectPermanent(LogViewerUrlHelper.LogViewerByRollingXmlFileTraceListener(this.Url, model.LogTraceSourceSelected, model.LogTraceListenerSelected));
            }

            throw new Exception("TraceListener Not Supperted");

        }
        public ActionResult LogViewerById(string guid)
        {
            LogViewerModel model = new LogViewerModel();

            using (DependencyFactory dependencyFactory = new DependencyFactory())
            {
                using (IProviderLogging provider = dependencyFactory.Unity.Resolve<IProviderLogging>())
                {
                    model.BaseViewModelInfo.Title = $customNamespace$.Resources.General.GeneralTexts.LogViewer;
                    model.LogMessages = new DataResultLogMessageList()
                    {
                        Data = provider.LoggingExceptionGetById(Guid.Parse(guid)).Data
                    };
                }
            }

            return View(LogViewerViewHelper.LogViewerById, model);
        }
        public ActionResult LogViewerByProxiedWcfTraceListener(string sourceName, string listenerName, LogViewerModel model)
        {
            model = this.LogViewerModel_GetBaseModel(sourceName, listenerName);
            model = this.LogViewerSetBreadcrumb(model, sourceName, listenerName);

            if (this.RequestType() == HttpVerbs.Get)
            {
                model.Filter = new DataFilterLogger()
                {
                    LogTraceSourceSelected = sourceName,
                    Page = 0,
                    PageSize = (int)PageSizesAvailable.RowsPerPage10
                };
            }


            if (WebGrid<LogMessageModel, LogViewerModel, DataFilterLogger>.IsWebGridEvent())
            {
                this.ModelState.Clear();
                model.Filter = (DataFilterLogger)WebGrid<LogMessageModel, LogViewerModel, DataFilterLogger>.GetDataFilterFromPost();
            }

            using (DependencyFactory dependencyFactory = new DependencyFactory())
            {
                using (IProviderLogging provider = dependencyFactory.Unity.Resolve<IProviderLogging>())
                {
                    DataResultLogMessageList resultSearch = provider.LoggingExceptionGetAll(model.Filter);
                    model.BaseViewModelInfo.Title = GeneralTexts.LogViewer;
                    model.LogMessages = new DataResultLogMessageList()
                    {
                        Data = resultSearch.Data,
                        Page = resultSearch.Page,
                        PageSize = resultSearch.PageSize,
                        SortAscending = resultSearch.SortAscending,
                        SortBy = resultSearch.SortBy,
                        TotalPages = resultSearch.TotalPages,
                        TotalRows = resultSearch.TotalRows
                    };
                }
            }

            return View(LogViewerViewHelper.LogViewerDisplay, model);
        }
        public ActionResult LogViewerByRollingXmlFileTraceListener(string sourceName, string listenerName, LogViewerModel model)
        {
            model = this.LogViewerModel_GetBaseModel(sourceName, listenerName);
            model = this.LogViewerSetBreadcrumb(model, sourceName, listenerName);

            if (ControllerHelper.RequestType(this.ControllerContext) == HttpVerbs.Get)
            {
                model.Filter = new DataFilterLogger()
                {
                    Page = 0,
                    PageSize = (int)PageSizesAvailable.RowsPerPage10,
                    IsClientVisible = false
                };
            }

            if (WebGrid<LogMessageModel, LogViewerModel, DataFilterLogger>.IsWebGridEvent())
            {
                this.ModelState.Clear();
                model.Filter = (DataFilterLogger)WebGrid<LogMessageModel, LogViewerModel, DataFilterLogger>.GetDataFilterFromPost();
            }

            List<LogMessageModel> messages = RollingXmlTraceListener.RollingXmlFileListenerToList(model.LogTraceListenerSelected, LogginConfigurationSectionName);
            model.LogMessages = new DataResultLogMessageList()
            {
                Page = model.Filter.Page,
                PageSize = model.Filter.PageSize,
                Data = new List<LogMessageModel>(),
                TotalRows = messages.Count,
            };

            int rowStartIndex = model.Filter.Page.Value * model.Filter.PageSize;
            int rowEndIndex = (int)(model.Filter.Page.Value * model.Filter.PageSize) + model.Filter.PageSize;

            for (int i = rowStartIndex; i < rowEndIndex; i++)
            {
                if (i < messages.Count)
                {
                    model.LogMessages.Data.Add(messages[i]);
                }
            }

            return View(LogViewerViewHelper.LogViewerDisplay, model);
        }
    }
}
