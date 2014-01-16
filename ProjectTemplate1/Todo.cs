using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIX_MVC_Layered_ProjectTemplate
{
    class TodoList
    {
        ///TODO: Template tests should use testsettings solution file
        ///
        /*
         * Los metodos de UserReuqest qyue guardan valores para un Thread
         * pierden el contexto cuando se hace un proceso Asincrono
         * 
         * Implementar esto en las propiedades que sean "per Thread" o que dependan de la peticion
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
