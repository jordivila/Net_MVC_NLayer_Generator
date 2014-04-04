using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIX_MVC_Layered_ProjectTemplate
{
    class TodoList
    {
        ///TODO: When deploy to azure WCF Host Common configuration throws exception: unauthroized when ConfigurationManager.SaveAs when trying to merge configuration
        ///TODO: Use jQuery.globalize from microsoft cdn. Remove from Template.UI.Web jquery.globalize files
        ///TODO: Check Session outdated can re-login
        ///TODO: Asp Net Mvc assembly should be load by Nuget
        ///TODO: LoggingHelper.Write(Exception ex)
        
        

        ///TODO: Los metodos de UserReuqest qyue guardan valores para un Thread pierden el contexto cuando se hace un proceso Asincrono
        /** 
                * Implementar esto en las propiedades que dependan del ContextItem de la peticion
                * 
                public int? AuditoriaLogId
                {
                    get
                    {
                        return (int?)Thread.GetData(Thread.GetNamedDataSlot(SepaRequestModel_Keys.WcfFormsAuditoriaKey));
                        //return this.Context.Items[SepaRequestModel_Keys.WcfFormsAuditoriaKey] == null ? null : (int?)this.Context.Items[SepaRequestModel_Keys.WcfFormsAuditoriaKey];
                    }
                    set
                    {
                        Thread.SetData(Thread.GetNamedDataSlot(SepaRequestModel_Keys.WcfFormsAuditoriaKey), value);
                        //this.Context.Items[SepaRequestModel_Keys.WcfFormsAuditoriaKey] = value;
                    }
                }         
        */
    }
}
