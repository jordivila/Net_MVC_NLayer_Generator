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
using $customNamespace$.UI.Web.Areas.LogViewer.Models;
using $customNamespace$.UI.Web.Areas.UserAccount;
using $customNamespace$.UI.Web.Controllers;
using $customNamespace$.UI.Web.Common.Mvc.Html;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace $customNamespace$.UI.Web.Areas.LogViewer.Controllers
{
    [$customNamespace$.UI.Web.Common.Mvc.Attributes.Authorize(Roles = SiteRoles.Administrator)]
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
            model.BaseViewModelInfo.Title = GeneralTexts.LogViewer;
            model.LogTraceSources = (ConfigurationManager.GetSection(LogginConfigurationSectionName) as LoggingSettings).TraceSources;
            if (!string.IsNullOrEmpty(sourceName))
            {
                if (!model.LogTraceSources.Contains(sourceName))
                {
                    throw new Exception("Trace Data Source Not Found");
                }

                model.Filter = new DataFilterLogger()
                {
                    LogTraceSourceSelected = sourceName,
                    CreationDate = DateTime.Now,
                    //CreationDateTo = DateTime.Now,
                    IsClientVisible = true,
                    Page = 0,
                    PageSize = (int)PageSizesAvailable.RowsPerPage10,
                    SortAscending = true,
                    SortBy = string.Empty
                };

                model.LogTraceListeners = model.LogTraceSources.Where(x => x.Name == sourceName).First().TraceListeners;

                if (!string.IsNullOrEmpty(listenerName))
                {
                    model.Filter.LogTraceListenerSelected = listenerName;
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
            return View(LogViewerViewHelper.LogViewerDisplay, model);
        }

        public ActionResult LogViewerByModel(LogViewerModel model)
        {
            if (WebGrid<LogMessageModel, LogViewerModel, DataFilterLogger>.IsWebGridEvent())
            {
                this.ModelState.Clear();
                model.Filter = (DataFilterLogger)WebGrid<LogMessageModel, LogViewerModel, DataFilterLogger>.GetDataFilterFromPost();
                model.Filter.IsClientVisible = false;
            }


            model = this.LogViewerSetBreadcrumb(model, model.Filter.LogTraceSourceSelected, model.Filter.LogTraceListenerSelected);

            LogWriterFactory logWriterFactory = new LogWriterFactory();
            LogWriter logWriterInstance = logWriterFactory.Create();
            using (TraceListener traceListenerInstance = logWriterInstance.TraceSources[model.Filter.LogTraceSourceSelected]
                                                                          .Listeners.Where(p => p.Name == model.Filter.LogTraceListenerSelected).First())
            {
                if (traceListenerInstance is ICustomTraceListener)
                {
                    model.LogMessages = ((ICustomTraceListener)traceListenerInstance).SearchLogMessages(LogginConfigurationSectionName, model.Filter);
                    //model.Filter.NextContinuationToken = model.LogMessages.NextContinuationToken;
                    //model.Filter.PreviousContinuationToken = model.LogMessages.PreviousContinuationToken;
                    return View(LogViewerViewHelper.LogViewerDisplay, model);
                }
                else
                {
                    throw new Exception("TraceListener Not Supported");
                }
            }
        
        }


        public ActionResult LogViewerById(string guid)
        {
            LogViewerModel model = new LogViewerModel();

            using (IProviderLogging provider = DependencyFactory.Resolve<IProviderLogging>())
            {
                model.BaseViewModelInfo.Title = GeneralTexts.LogViewer;
                model.LogMessages = new DataResultLogMessageList()
                {
                    Data = provider.LoggingExceptionGetById(Guid.Parse(guid)).Data
                };
            }


            return View(LogViewerViewHelper.LogViewerById, model);
        }
    }
}
