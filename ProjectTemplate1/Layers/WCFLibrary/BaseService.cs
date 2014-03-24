using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;
using System.Collections.Generic;
using $customNamespace$.WCF.ServicesLibrary.AspNetApplicationServices;
using $customNamespace$.WCF.ServicesLibrary.AspNetApplicationServices.Admin;
using $customNamespace$.WCF.ServicesLibrary.LoggingServices;
using $customNamespace$.WCF.ServicesLibrary.SyndicationServices;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;


namespace $customNamespace$.WCF.ServicesLibrary
{
    public abstract class BaseService : IDisposable
    {
        public BaseService()
        {

        }

        public static List<Type> GetAllServiceTypes()
        {
            List<Type> allServices = BaseService.CheckInheritance(Assembly.GetAssembly(typeof(BaseService)),
                                            new List<Type>() { typeof(BaseServiceWithCustomMessageHeaders) },
                                            new List<Type>() { typeof(BaseService) });

            return allServices;
        }

        protected static List<Type> CheckInheritance(Assembly assemblyToCheck, List<Type> excludedTypes, List<Type> baseClasses)
        {
            List<TypeInfo> assemblyTypes = assemblyToCheck.DefinedTypes.ToList();
            List<TypeInfo> assemblyTypesToCheck = assemblyTypes.Where(x => excludedTypes.All(p => p.FullName != x.FullName) && !x.IsEnum).ToList();
            List<TypeInfo> assemblyTypesDevelopment = assemblyTypesToCheck.Where(x =>
                                                                                !(x.IsDefined(typeof(CompilerGeneratedAttribute), false)) // filter out <>_DisplayClasses or any compiler generated class
                                                                                ).ToList();

            List<TypeInfo> assemblyTypesUnInherited = new List<TypeInfo>();

            foreach (var item in assemblyTypesDevelopment)
            {
                int inheritanceChildLevelCounter = 0;
                int inheritanceChildLevelMax = 10;
                bool inheritanceOK = false;
                Func<TypeInfo, bool> inheritanceValid = delegate(TypeInfo type)
                {
                    bool validBaseClass = baseClasses.Select(p => p.FullName).Contains(type.BaseType.FullName);
                    return ((type.BaseType != null) && (validBaseClass));
                };
                TypeInfo typeToCheck = item;
                while ((!inheritanceOK) && (typeToCheck != null) && (typeToCheck != typeof(object)) && (inheritanceChildLevelCounter < inheritanceChildLevelMax))
                {
                    inheritanceOK = inheritanceValid(typeToCheck);
                    if (!inheritanceOK)
                    {
                        typeToCheck = typeToCheck.BaseType.GetTypeInfo();
                    }

                    inheritanceChildLevelCounter++;
                }

                if (!inheritanceOK)
                {
                    assemblyTypesUnInherited.Add(item);
                }
            }

            return assemblyTypesDevelopment.Except(assemblyTypesUnInherited).Cast<Type>().ToList();
        }


        public virtual void Dispose()
        { 
            
        }
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    [LoggingServiceBehavior]
    public abstract class BaseServiceWithCustomMessageHeaders : BaseService
    {
        public BaseServiceWithCustomMessageHeaders(): base()
        {
            Thread.CurrentThread.CurrentCulture = this.UserRequest.UserProfile.Culture;
            Thread.CurrentThread.CurrentUICulture = this.UserRequest.UserProfile.Culture;
        }

        private IUserRequestModel<OperationContext, MessageHeaders> _userRequest = null;

        internal IUserRequestModel<OperationContext, MessageHeaders> UserRequest
        {
            get
            {
                if (_userRequest == null)
                {
                    _userRequest = DependencyFactory.Resolve<IUserRequestModel<OperationContext, MessageHeaders>>();
                }

                return _userRequest;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}