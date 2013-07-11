using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Logging;

namespace $safeprojectname$
{
    
    public class LoggingServiceBehavior : Attribute, IErrorHandler, IServiceBehavior
    {
        private static Type BindingType = null;

        #region IErrorHandler Members

        public bool HandleError(Exception ex)
        {
            return true;
        }

        private void LogMessage(Exception error, LoggingExceptionDetails wrapperException)
        {
            try
            {
                Dictionary<string, object> extendedProperties = new Dictionary<string, object>();
                extendedProperties.Add("Guid", wrapperException.ExceptionId.ToString());
                if (BindingType == typeof(NetTcpBinding))
                {
                    extendedProperties.Add("RequestMessage", OperationContext.Current.RequestContext.RequestMessage);
                }
                if (BindingType == typeof(BasicHttpBinding) || BindingType == typeof(WSHttpBinding))
                {
                    extendedProperties.Add("RequestMessage", OperationContext.Current.RequestContext.RequestMessage);
                }
                LoggingHelper.Write(new LogEntry(error, LoggerCategories.WCFGeneral, 1, 1, TraceEventType.Error,string.Format("{0} (DetailException): {1}", Assembly.GetExecutingAssembly().GetName().Name, wrapperException.ExceptionId), extendedProperties));
                // LoggingHelper.Write(new LogEntry(error.GetBaseException(), LoggerCategories.General, 1, 1, TraceEventType.Error, string.Format("{0} (BaseException) : {1}", Assembly.GetExecutingAssembly().GetName().Name, wrapperException.ExceptionId), extendedProperties));
            }
            catch (Exception exII)
            {
                //TODO: si runtime llega aqui significa que el databasetracelistener no funciona. Grabar en fichero texto utilizando otro tracelistener
                System.Diagnostics.Debug.Write(exII.Message);
            }

        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (!(error is FaultException))
            {
                LoggingExceptionDetails guidException = new LoggingExceptionDetails();
                FaultException<LoggingExceptionDetails> faultException = new FaultException<LoggingExceptionDetails>(guidException, new FaultReason(guidException.Message));
                fault = Message.CreateMessage(version, faultException.CreateMessageFault(), faultException.Action);
                this.LogMessage(error, guidException);
            }
        }

        #endregion

        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            BindingType = serviceDescription.Endpoints[0].Binding.GetType();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(new LoggingServiceBehavior());
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            
        }

        #endregion
    }

    public class LoggingExceptionDetails
    {
        private Guid _guid = Guid.NewGuid();
        public Guid ExceptionId
        {
            get
            {
                return this._guid;
            }
        }

        [DataMember]
        public String Message { get; private set; }

        public LoggingExceptionDetails()
        {
            this.Message = string.Format("{0}", this._guid.ToString());
        }

        public LoggingExceptionDetails(String message)
        {
            this.Message = message;
        }
    }

    public class LoggingServiceBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(LoggingServiceBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new LoggingServiceBehavior();
        }
    }
}