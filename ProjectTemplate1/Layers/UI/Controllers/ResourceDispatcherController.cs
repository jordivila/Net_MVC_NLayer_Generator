using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.Cryptography;
using $customNamespace$.Models.Enumerations;
using $safeprojectname$.Common.Mvc.Attributes;
using $safeprojectname$.Common.T4;


namespace $safeprojectname$.Controllers
{
    [CacheFilterAttribute(Duration=9000000)]
    public class ResourceDispatcherController : Controller
    {
        public const string TTSessionContext_ControllerType = "SessionContext_ControllerType";
        public const string ResourceDispatchParamCommonKey = "Common";
        public const string ResourceDispatchCryptoPasswordKey = "ResourceDispatchCryptoPasswordKey";
        public const string ResourceDispatchParamControllerKey = "controller";
        public const string ResourceDispatchParamVersionKey = "version";
        public const string ResourceDispatchParamCultureKey = "culture";
        public const string ResourceDispatchParamIsMobileDevice = "IsMobileDevice";
        private ObjectCache _objCacheManager = new MemoryCache("CacheManagerFoeClientResources");
        private CacheItemPolicy _objCachePolicy = new CacheItemPolicy();

        private string GenerateCacheKey(string id, MediaType mediaType)
        {
            string controller = id;
            string cacheKey = string.Empty;
            if (mediaType == MediaType.javascript)
            {
                cacheKey = string.Format("{0}.{1}.{2}", controller,
                                                        MvcApplication.UserRequest.UserProfile.Culture.Name,
                                                        mediaType == MediaType.javascript ? "js" : "css");
            }
            else
            {
                cacheKey = string.Format("{0}.{1}.{2}", controller,
                                                        string.Format("{0}{1}", Request.Browser.Browser, Request.Browser.MajorVersion),
                                                        mediaType == MediaType.javascript ? "js" : "css");
            }
            return cacheKey;
        }
        private void FileSetCache(StringBuilder lFilesContent, string cacheKey, MediaType media)
        {
            Minifier m = new Minifier();
            if (media == MediaType.javascript)
            {
                CodeSettings c = new CodeSettings();
                this._objCacheManager.Add(cacheKey,
                                        ApplicationConfiguration.IsDebugMode ? lFilesContent.ToString() : m.MinifyJavaScript(lFilesContent.ToString(), c)
                                        , _objCachePolicy);
            }
            else
            {
                this._objCacheManager.Add(cacheKey,
                                        ApplicationConfiguration.IsDebugMode ? lFilesContent.ToString() : m.MinifyStyleSheet(lFilesContent.ToString())
                                        , _objCachePolicy);
            }
        }
        private bool FileIsCached(string cacheKey)
        {
            if (ApplicationConfiguration.IsDebugMode)
            {
                this._objCacheManager.Remove(cacheKey);
            }

            return this._objCacheManager.Contains(cacheKey);
        }
        public JavaScriptResult Javascript()
        {
            string controller = Crypto.Decrypt(Request.Params[ResourceDispatchParamControllerKey], ResourceDispatcherController.ResourceDispatchCryptoPasswordKey);
            string cacheKeyJs = this.GenerateCacheKey(controller, MediaType.javascript);

            if (!this.FileIsCached(cacheKeyJs))
            {
                StringBuilder sb = new StringBuilder();

                if (controller == ResourceDispatcherController.ResourceDispatchParamCommonKey)
                {
                    sb.Append(System.IO.File.ReadAllText(Server.MapPath(string.Format("{0}{1}",
                                                                        ApplicationConfiguration.ClientResourcesSettingsSection.CDN_JS_VirtualRoot,
                                                                        ApplicationConfiguration.ClientResourcesSettingsSection.CDN_JS_CommonFileName($customNamespace$.UI.Web.MvcApplication.UserRequest.UserProfile.Culture, MvcApplication.Version)))));
                }
                else
                {
                    sb.Append(new Template_T4_Runtime_JS_ControllerScripts()
                    {
                        Session = new Dictionary<string, object>() 
                        { 
                            { ResourceDispatcherController.TTSessionContext_ControllerType, Type.GetType(controller) } 
                        }
                    }.TransformText());

                    foreach (var item in ApplicationConfiguration.ClientResourcesSettingsSection.WebSitePageInitScripts)
                    {
                        sb.Append(System.IO.File.ReadAllText(Server.MapPath(item)));
                    }
                }

                this.FileSetCache(sb, cacheKeyJs, MediaType.javascript);
            }

            JavaScriptResult jsResult = new JavaScriptResult();
            jsResult.Script = (string)this._objCacheManager.Get(cacheKeyJs);
            return jsResult;
        }
        public FileStreamResult StyleSheet()
        {
            string controller = Crypto.Decrypt(Request.Params[ResourceDispatchParamControllerKey], ResourceDispatcherController.ResourceDispatchCryptoPasswordKey);
            string cacheKeyCss = this.GenerateCacheKey(controller, MediaType.stylesheet);

            if (controller == ResourceDispatcherController.ResourceDispatchParamCommonKey)
            {
                cacheKeyCss = "Common.css";
            }

            if (!this.FileIsCached(cacheKeyCss))
            {
                StringBuilder sb = new StringBuilder();
                if (controller == ResourceDispatcherController.ResourceDispatchParamCommonKey)
                {
                    sb.Append(System.IO.File.ReadAllText(Server.MapPath(string.Format("{0}{1}",
                                                                        ApplicationConfiguration.ClientResourcesSettingsSection.CDN_CSS_VirtualRoot,
                                                                        ApplicationConfiguration.ClientResourcesSettingsSection.CDN_CSS_CommonFileName(MvcApplication.Version)))));
                }
                else
                {
                    sb.Append(new Template_T4_Runtime_CSS_ControllerStylesheets()
                    {
                        Session = new Dictionary<string, object>() 
                        { 
                            { ResourceDispatcherController.TTSessionContext_ControllerType, Type.GetType(controller) }
                        }
                    }.TransformText());
                }

                this.FileSetCache(sb, cacheKeyCss, MediaType.stylesheet);
            }

            FileStreamResult fsr = new FileStreamResult(new MemoryStream(UTF8Encoding.UTF8.GetBytes((string)this._objCacheManager.Get(cacheKeyCss))), "text/css");
            return fsr;
        }
    }
}