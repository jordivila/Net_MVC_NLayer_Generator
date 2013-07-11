using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
using $customNamespace$.Models.UserRequestModel;

namespace $safeprojectname$.Common.AspNetApplicationServices
{
    public class CookieManagerBehaviorExtension<TBinding> : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(CookieManagerEndpointBehavior<TBinding>); }
        }

        protected override object CreateBehavior()
        {
            return new CookieManagerEndpointBehavior<TBinding>();
        }
    }

    public class CookieManagerEndpointBehavior<TBinding> : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(CookieManagerMessageInspector<TBinding>.Instance);
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

    public class CookieManagerMessageInspector<TBinding> : IClientMessageInspector
    {
        private static CookieManagerMessageInspector<TBinding> instance;

        public CookieManagerMessageInspector()
        {
        }

        public static CookieManagerMessageInspector<TBinding> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CookieManagerMessageInspector<TBinding>();
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