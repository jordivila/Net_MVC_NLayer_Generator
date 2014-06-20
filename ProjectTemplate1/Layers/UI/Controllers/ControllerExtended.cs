
namespace $customNamespace$.UI.Web.Controllers
{
    //public abstract class ControllerUserIntended : Controller
    //{
    //    public abstract string[] GetControllerJavascriptResources { get; }
    //    public abstract string[] GetControllerStyleSheetResources { get; }
    //}


    public interface IControllerWithClientResources 
    {
        string[] GetControllerJavascriptResources { get; }
        string[] GetControllerStyleSheetResources { get; }
    }
    

}