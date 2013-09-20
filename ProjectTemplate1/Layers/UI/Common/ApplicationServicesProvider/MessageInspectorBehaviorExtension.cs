using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Web.Configuration;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;


namespace $safeprojectname$.Common.AspNetApplicationServices
{
    public class MessageInspectorBehaviorExtension : BehaviorExtensionElement
    {
        private static Type BehaviorTypeCurrent = null;
        private string currentBindingKey = "$customBindingConfigurationName$";
        private string currentVirtualPath = "/";

        public MessageInspectorBehaviorExtension()
            : base()
        {
            using (DependencyFactory dependencyFactory = new DependencyFactory())
            {
                if (MessageInspectorBehaviorExtension.BehaviorTypeCurrent == null)
                {
                    Binding b = this.ResolveBinding(this.currentBindingKey, this.currentVirtualPath);

                    if (b == null)
                    {
                        throw new Exception(string.Format("Could not find binding with name '{0}'", currentBindingKey));
                    }

                    MessageInspectorBehaviorExtension.BehaviorTypeCurrent = b.GetType();
                }
            }
        }

        private BindingsSection GetBindingsSection(string virtualPath)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(virtualPath);
            ServiceModelSectionGroup section = config.GetSectionGroup("system.serviceModel") as ServiceModelSectionGroup;
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(config);
            return serviceModel.Bindings;
        }

        private Binding ResolveBinding(string bindingName, string virtualPath)
        {
            BindingsSection section = this.GetBindingsSection(virtualPath);

            foreach (var bindingCollection in section.BindingCollections)
            {
                if (bindingCollection.ConfiguredBindings.Count > 0 && bindingCollection.ConfiguredBindings[0].Name == bindingName)
                {
                    var bindingElement = bindingCollection.ConfiguredBindings[0];
                    var binding = (Binding)Activator.CreateInstance(bindingCollection.BindingType);
                    binding.Name = bindingElement.Name;
                    bindingElement.ApplyConfiguration(binding);
                    return binding;
                }
            }

            return null;
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(MessageInspectorEndpointBehavior<NetTcpBinding>);
            }
        }

        protected override object CreateBehavior()
        {
            object result = null;

            if (MessageInspectorBehaviorExtension.BehaviorTypeCurrent == typeof(NetTcpBinding))
            {
                result = new MessageInspectorEndpointBehavior<NetTcpBinding>();
            }

            if (MessageInspectorBehaviorExtension.BehaviorTypeCurrent == typeof(BasicHttpBinding))
            {
                result = new MessageInspectorEndpointBehavior<BasicHttpBinding>();
            }

            if (result == null)
            {
                throw new Exception("MessageInspector: this type of binding is not supported. Please add it yourself at MessageInspectorBehaviorExtension or use one of the supported types.");
            }

            return result;
        }
    }

    public class MessageInspectorEndpointBehavior<TBinding> : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(MessageInspectorClient<TBinding>.Instance);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            return;
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            return;
        }
    }

    public class MessageInspectorClient<TBinding> : IClientMessageInspector
    {
        private static MessageInspectorClient<TBinding> instance;

        public MessageInspectorClient()
        {
        }

        public static MessageInspectorClient<TBinding> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageInspectorClient<TBinding>();
                }

                return instance;
            }
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (typeof(NetTcpBinding) == typeof(TBinding))
            {
                this.AfterReceiveReply_NetTcp(ref reply, correlationState);
            }
            else
            {
                if (typeof(BasicHttpBinding) == typeof(TBinding))
                {
                    this.AfterReceiveReply_Http(ref reply, correlationState);
                }
                else
                {
                    throw new Exception(string.Format("{0} Does NOT support this type of binding", this.GetType().Name));
                }
            }

        }
        public void AfterReceiveReply_NetTcp(ref Message reply, object correlationState)
        {
            if (reply.Headers.FindHeader(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, UserRequestModel_Keys.WcfCustomBehaviourName) > 0)
            {
                try
                {
                    MvcApplication.UserRequest.WcfAuthenticationCookieValue = reply.Headers.GetHeader<string>(reply.Headers.FindHeader(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, UserRequestModel_Keys.WcfCustomBehaviourName));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //if (UserRequest != null)
                    //{
                    //UserRequest.Dispose();
                    //}
                }

            }
        }
        public void AfterReceiveReply_Http(ref Message reply, object correlationState)
        {
            HttpResponseMessageProperty httpResponse = reply.Properties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty;

            List<HttpCookie> cookieResultList = new List<HttpCookie>();

            if (httpResponse != null)
            {
                string cookie = httpResponse.Headers[HttpResponseHeader.SetCookie];

                if (!string.IsNullOrEmpty(cookie))
                {
                    // Wcf Responsed cookies  are separated by "," and values inside cookies are separated by ";"
                    // WARNING: when some cookie has been expired, its datetime value is like "Mon, 11 Oct 1999". That forces me to use ";" key value pairs

                    HttpCookie cookieCurrent = null;
                    foreach (string item in cookie.Split(";".ToCharArray()))
                    {
                        string[] itemPair = item.Split("=".ToCharArray());
                        string itemName = itemPair[0].Trim();
                        string itemValue = itemPair.Length > 1 ? itemPair[1].Trim() : string.Empty;

                        switch (itemName)
                        {
                            case "HttpOnly":
                                cookieCurrent.HttpOnly = true;
                                break;
                            case "path":
                                cookieCurrent.Path = itemValue;
                                break;
                            case "expires":
                                cookieCurrent.Expires = DateTime.Parse(itemValue);
                                break;
                            default:
                                if (cookieCurrent != null)
                                {
                                    cookieResultList.Add(cookieCurrent);
                                }
                                cookieCurrent = new HttpCookie(itemName, itemValue);
                                break;
                        }
                    }

                    cookieResultList.Add(cookieCurrent);

                    foreach (HttpCookie item in cookieResultList)
                    {
                        System.Web.HttpContext.Current.Response.Cookies.Add(item);
                    }

                    cookieResultList.Clear();

                }
            }
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (typeof(NetTcpBinding) == typeof(TBinding))
            {
                return this.BeforeSend_NetTCPRequest(ref request, channel);
            }
            else
            {
                if (typeof(BasicHttpBinding) == typeof(TBinding))
                {
                    return this.BeforeSend_HttpRequest(ref request, channel);
                }
                else
                {
                    throw new Exception(string.Format("{0} Does NOT support this type of binding", this.GetType().Name));
                }
            }
        }
        private object BeforeSend_NetTCPRequest(ref Message request, IClientChannel channel)
        {
            request.Headers.Add(MessageHeader.CreateHeader(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, UserRequestModel_Keys.WcfCustomBehaviourName, MvcApplication.UserRequest.WcfAuthenticationCookieValue));
            request.Headers.Add(MessageHeader.CreateHeader(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, UserRequestModel_Keys.WcfCustomBehaviourName, MvcApplication.UserRequest.UserProfile.Culture.ToString()));
            request.Headers.Add(MessageHeader.CreateHeader(UserRequestModel_Keys.WcfClientThemeSelectedCookieName, UserRequestModel_Keys.WcfCustomBehaviourName, MvcApplication.UserRequest.UserProfile.Theme.Value.ToString()));
            request.Headers.Add(MessageHeader.CreateHeader(UserRequestModel_Keys.WcfSessionIdKey, UserRequestModel_Keys.WcfCustomBehaviourName, MvcApplication.UserRequest.WcfSessionIdKeyValue));
            return null;
        }
        private object BeforeSend_HttpRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequest;

            if (!request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                request.Properties.Add(HttpRequestMessageProperty.Name, new HttpRequestMessageProperty());
            }

            httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            try
            {
                httpRequest.Headers.Add(HttpRequestHeader.Cookie, string.Format("{0}={1};{2}={3};{4}={5};{6}={7};",
                                                                                                            UserRequestModel_Keys.WcfFormsAuthenticationCookieName,
                                                                                                            MvcApplication.UserRequest.WcfAuthenticationCookieValue,

                                                                                                            UserRequestModel_Keys.WcfClientCultureSelectedCookieName,
                                                                                                            MvcApplication.UserRequest.UserProfile.Culture.Name,

                                                                                                            UserRequestModel_Keys.WcfClientThemeSelectedCookieName,
                                                                                                            MvcApplication.UserRequest.UserProfile.Theme.ToString(),

                                                                                                            UserRequestModel_Keys.WcfSessionIdKey,
                                                                                                            MvcApplication.UserRequest.WcfSessionIdKeyValue));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (UserRequest != null)
                //{
                //UserRequest.Dispose();
                //}
            }
            return null;
        }
    }
}