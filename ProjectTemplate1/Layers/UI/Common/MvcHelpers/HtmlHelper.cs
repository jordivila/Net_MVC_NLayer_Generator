using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using $customNamespace$.Models;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Resources.General;
using $customNamespace$.Resources.Helpers;
using $customNamespace$.UI.Web.Areas.Blog;
using $customNamespace$.UI.Web.Areas.Home;
using $customNamespace$.UI.Web.Areas.UserAccount;
using $customNamespace$.UI.Web.Models;

namespace $customNamespace$.UI.Web.Common.Mvc.Html
{
    public static class HtmlHelperExtensions
    {
        public readonly static string SectionScriptsToAdd = "ScriptsToAdd";
        public readonly static string SectionInlineStyles = "InlineStyles";

        public static MvcHtmlString PartialMenuTopNav(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_MenuTopNavPartial.cshtml");
        }
        public static MvcHtmlString PartialMenuPreferences(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_MenuPreferencesPartial.cshtml");
        }
        public static MvcHtmlString PartialMenuPreferencesSwitchers(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_MenuPreferencesSwitchersPartial.cshtml");
        }
        public static MvcHtmlString PartialBreadcrumb(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_Breadcrumb.cshtml");
        }
        public static MvcHtmlString PartialResourcesScriptsCommonRender(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_ResourcesScriptsCommonRender.cshtml");
        }
        public static MvcHtmlString PartialResourcesStylesheetsCommonRender(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_ResourcesStylesheetCommonRender.cshtml");
        }

        #region Login/LogOut Helpers
        public static MvcHtmlString PartialLogOnPartial(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_LogOnPartial.cshtml");
        }
        public static MvcHtmlString LogOnButton(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_LogOnButton.cshtml");
        }
        public static MvcHtmlString LogOffButton(this HtmlHelper htmlHelper)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(htmlHelper, "~/Views/Shared/_LogOffButton.cshtml");
        }
        #endregion

        //public static ResourceMvcHelper ResourceTexts(this HtmlHelper htmlHelper)
        //{
        //    if (System.Web.HttpContext.Current.Application["ResourceMvcHelper"] == null)
        //    {
        //        System.Web.HttpContext.Current.Application["ResourceMvcHelper"] = new ResourceMvcHelper();
        //    }
        //    return (ResourceMvcHelper)System.Web.HttpContext.Current.Application["ResourceMvcHelper"];
        //}
    }

    public static class FormsExtension
    {
        public static MvcForm BeginWidgetForm(this HtmlHelper htmlHelper)
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("class", "ui-widgetForm");
            return htmlHelper.BeginForm((string)htmlHelper.ViewContext.RouteData.Values["action"],
                                        (string)htmlHelper.ViewContext.RouteData.Values["controller"],
                                        FormMethod.Post,
                                        htmlAttributes);
        }

        public static MvcForm BeginWidgetForm(this HtmlHelper htmlHelper, object attributes)
        {
            IDictionary<string, object> htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            htmlAttributes.Add("class", "ui-widgetForm");
            return htmlHelper.BeginForm((string)htmlHelper.ViewContext.RouteData.Values["action"],
                                        (string)htmlHelper.ViewContext.RouteData.Values["controller"],
                                        FormMethod.Post,
                                        htmlAttributes);
        }

        public static MvcHtmlString CheckBoxMultiple<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<string>>> expression, IEnumerable<SelectListItem> listOfValues)
        {
            return htmlHelper.EditorFor(expression, "IEnumerable_CheckboxMultiple", new { listItems = listOfValues });
        }

        public static MvcHtmlString HiddenFor_IEnumerable<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.EditorFor(expression, "IEnumerable_HiddenFor");
        }

        public static MvcHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> listOfValues, Position position = Position.Horizontal)
        {
            return htmlHelper.EditorFor(expression, "IEnumerable_RadioButtons", new { listItems = listOfValues, position = position });
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> listOfValues, object htmlAttributes = null)
        {
            return htmlHelper.EditorFor(expression, "IEnumerable_DropdownList", new { listItems = listOfValues, htmlAttributes = htmlAttributes });
        }

        public static String GetId<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression));
        }

    }

    public static class EnumExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectList(this Enum valueSelected, Type enumType)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum item in enumValues)
            {
                list.Add(new SelectListItem()
                {
                    Selected = item.Equals(valueSelected),
                    Text = EnumExtension.EnumDescription(item),
                    Value = item.ToString()
                });
            }
            return list;
        }
        public static IEnumerable<SelectListItem> ToSelectList(this Enum valueSelected, Type enumType, Func<Enum, SelectListItem> forEachItem)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum item in enumValues)
            {
                list.Add(forEachItem(item));
            }
            return list;
        }
    }

    public static class GlobalizationExtension
    {
        public static List<SelectListItem> ToSelectList(this CultureInfo c)
        {
            var x = (from cAvailble in GlobalizationHelper.CultureInfoAvailableList()
                     select new SelectListItem()
                     {
                         Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(cAvailble.NativeName),
                         Value = cAvailble.Name
                     }).ToList();

            if (x.Any(p => p.Value == c.Name))
            {
                x.Where(p => p.Value == c.Name).First().Selected = true;
            }

            return x;
        }
    }

    #region jQuery Helper
    public class jQueryHelpers
    {
        public enum Icon : int
        {
            None,
            ui_icon_carat_1_n,
            ui_icon_carat_1_ne,
            ui_icon_carat_1_e,
            ui_icon_carat_1_se,
            ui_icon_carat_1_s,
            ui_icon_carat_1_sw,
            ui_icon_carat_1_w,
            ui_icon_carat_1_nw,
            ui_icon_carat_2_n_s,
            ui_icon_carat_2_e_w,
            ui_icon_triangle_1_n,
            ui_icon_triangle_1_ne,
            ui_icon_triangle_1_e,
            ui_icon_triangle_1_se,
            ui_icon_triangle_1_s,
            ui_icon_triangle_1_sw,
            ui_icon_triangle_1_w,
            ui_icon_triangle_1_nw,
            ui_icon_triangle_2_n_s,
            ui_icon_triangle_2_e_w,
            ui_icon_arrow_1_n,
            ui_icon_arrow_1_ne,
            ui_icon_arrow_1_e,
            ui_icon_arrow_1_se,
            ui_icon_arrow_1_s,
            ui_icon_arrow_1_sw,
            ui_icon_arrow_1_w,
            ui_icon_arrow_1_nw,
            ui_icon_arrow_2_n_s,
            ui_icon_arrow_2_ne_sw,
            ui_icon_arrow_2_e_w,
            ui_icon_arrow_2_se_nw,
            ui_icon_arrowstop_1_n,
            ui_icon_arrowstop_1_e,
            ui_icon_arrowstop_1_s,
            ui_icon_arrowstop_1_w,
            ui_icon_arrowthick_1_n,
            ui_icon_arrowthick_1_ne,
            ui_icon_arrowthick_1_e,
            ui_icon_arrowthick_1_se,
            ui_icon_arrowthick_1_s,
            ui_icon_arrowthick_1_sw,
            ui_icon_arrowthick_1_w,
            ui_icon_arrowthick_1_nw,
            ui_icon_arrowthick_2_n_s,
            ui_icon_arrowthick_2_ne_sw,
            ui_icon_arrowthick_2_e_w,
            ui_icon_arrowthick_2_se_nw,
            ui_icon_arrowthickstop_1_n,
            ui_icon_arrowthickstop_1_e,
            ui_icon_arrowthickstop_1_s,
            ui_icon_arrowthickstop_1_w,
            ui_icon_arrowreturnthick_1_w,
            ui_icon_arrowreturnthick_1_n,
            ui_icon_arrowreturnthick_1_e,
            ui_icon_arrowreturnthick_1_s,
            ui_icon_arrowreturn_1_w,
            ui_icon_arrowreturn_1_n,
            ui_icon_arrowreturn_1_e,
            ui_icon_arrowreturn_1_s,
            ui_icon_arrowrefresh_1_w,
            ui_icon_arrowrefresh_1_n,
            ui_icon_arrowrefresh_1_e,
            ui_icon_arrowrefresh_1_s,
            ui_icon_arrow_4,
            ui_icon_arrow_4_diag,
            ui_icon_extlink,
            ui_icon_newwin,
            ui_icon_refresh,
            ui_icon_shuffle,
            ui_icon_transfer_e_w,
            ui_icon_transferthick_e_w,
            ui_icon_folder_collapsed,
            ui_icon_folder_open,
            ui_icon_document,
            ui_icon_document_b,
            ui_icon_note,
            ui_icon_mail_closed,
            ui_icon_mail_open,
            ui_icon_suitcase,
            ui_icon_comment,
            ui_icon_person,
            ui_icon_print,
            ui_icon_trash,
            ui_icon_locked,
            ui_icon_unlocked,
            ui_icon_bookmark,
            ui_icon_tag,
            ui_icon_home,
            ui_icon_flag,
            ui_icon_calculator,
            ui_icon_cart,
            ui_icon_pencil,
            ui_icon_clock,
            ui_icon_disk,
            ui_icon_calendar,
            ui_icon_zoomin,
            ui_icon_zoomout,
            ui_icon_search,
            ui_icon_wrench,
            ui_icon_gear,
            ui_icon_heart,
            ui_icon_star,
            ui_icon_link,
            ui_icon_cancel,
            ui_icon_plus,
            ui_icon_plusthick,
            ui_icon_minus,
            ui_icon_minusthick,
            ui_icon_close,
            ui_icon_closethick,
            ui_icon_key,
            ui_icon_lightbulb,
            ui_icon_scissors,
            ui_icon_clipboard,
            ui_icon_copy,
            ui_icon_contact,
            ui_icon_image,
            ui_icon_video,
            ui_icon_script,
            ui_icon_alert,
            ui_icon_info,
            ui_icon_notice,
            ui_icon_help,
            ui_icon_check,
            ui_icon_bullet,
            ui_icon_radio_off,
            ui_icon_radio_on,
            ui_icon_pin_w,
            ui_icon_pin_s,
            ui_icon_play,
            ui_icon_pause,
            ui_icon_seek_next,
            ui_icon_seek_prev,
            ui_icon_seek_end,
            ui_icon_seek_first,
            ui_icon_stop,
            ui_icon_eject,
            ui_icon_volume_off,
            ui_icon_volume_on,
            ui_icon_power,
            ui_icon_signal_diag,
            ui_icon_signal,
            ui_icon_battery_0,
            ui_icon_battery_1,
            ui_icon_battery_2,
            ui_icon_battery_3,
            ui_icon_circle_plus,
            ui_icon_circle_minus,
            ui_icon_circle_close,
            ui_icon_circle_triangle_e,
            ui_icon_circle_triangle_s,
            ui_icon_circle_triangle_w,
            ui_icon_circle_triangle_n,
            ui_icon_circle_arrow_e,
            ui_icon_circle_arrow_s,
            ui_icon_circle_arrow_w,
            ui_icon_circle_arrow_n,
            ui_icon_circle_zoomin,
            ui_icon_circle_zoomout,
            ui_icon_circle_check,
            ui_icon_circlesmall_plus,
            ui_icon_circlesmall_minus,
            ui_icon_circlesmall_close,
            ui_icon_squaresmall_plus,
            ui_icon_squaresmall_minus,
            ui_icon_squaresmall_close,
            ui_icon_grip_dotted_vertical,
            ui_icon_grip_dotted_horizontal,
            ui_icon_grip_solid_vertical,
            ui_icon_grip_solid_horizontal,
            ui_icon_gripsmall_diagonal_se,
            ui_icon_grip_diagonal_se,
        }

        public static string IconToCssClass(Icon buttonIcon)
        {
            string result = buttonIcon.ToString().Replace("_", "-");
            return result;
        }
    }
    #endregion

    #region ValidationExtensions
    public static class ValidationExtensions
    {

        public static MvcHtmlString ValidationMessageCustomizedFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            MvcHtmlString mvcHtmlString = htmlHelper.ValidationMessageFor(expression);

            if (mvcHtmlString != null)
            {
                TagBuilder containerDivBuilder = new TagBuilder("div");
                //containerDivBuilder.AddCssClass("ui-state-error ui-corner-all");
                containerDivBuilder.InnerHtml = mvcHtmlString == null ? string.Empty : mvcHtmlString.ToString();
                return MvcHtmlString.Create(containerDivBuilder.ToString(TagRenderMode.Normal));
            }
            else
            {
                return mvcHtmlString;
            }
        }

        public static MvcHtmlString ValidationSummaryCustomized(this HtmlHelper htmlHelper)
        {
            if (!htmlHelper.ViewData.ModelState.IsValid)
            {


                TagBuilder tgErrList = new TagBuilder("ul");
                foreach (string item in htmlHelper.ViewData.ModelState.Keys)
                {
                    if (htmlHelper.ViewData.ModelState[item].Errors.Count > 0)
                    {
                        TagBuilder tgErrorItem = new TagBuilder("li");
                        tgErrorItem.InnerHtml = htmlHelper.ViewData.ModelState[item].Errors[0].ErrorMessage;
                        tgErrorItem.Attributes.Add("modelKey", TagBuilder.CreateSanitizedId(item));
                        tgErrList.InnerHtml = string.Format("{0} {1}", tgErrList.InnerHtml, tgErrorItem.ToString(TagRenderMode.Normal));
                    }
                }

                TagBuilder tgDivTitle = new TagBuilder("span");
                tgDivTitle.InnerHtml = $customNamespace$.Resources.General.GeneralTexts.PleaseReviewForm; //htmlHelper.ResourceTexts().ResourceTextsGeneral.PleaseReviewForm;

                TagBuilder tgDivBox = new TagBuilder("div");
                tgDivBox.Attributes.Add("data-widget", "widgetFormSummary");
                tgDivBox.AddCssClass("ui-widgetForm-ValidationSummary ui-state-error ui-corner-all");
                tgDivBox.InnerHtml = string.Format("{0} {1}", tgDivTitle.ToString(TagRenderMode.Normal), tgErrList.ToString(TagRenderMode.Normal));

                return MvcHtmlString.Create(tgDivBox.ToString(TagRenderMode.Normal));
            }
            else
            {
                return null;
            }

            //return htmlHelper.ValidationSummary( htmlHelper.ResourceTexts().ResourceTextsGeneral.PleaseReviewForm, new { @class = "ui-widgetForm-ValidationSummary ui-state-error ui-corner-all" });
        }


    }
    #endregion

    #region Button Extensions
    public enum ButtonType : int
    {
        Button,
        Submit
    }

    public static class ButtonExtensions
    {

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string htmlContent, jQueryHelpers.Icon buttonIcon, ButtonType buttonType, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagTextSpan = new TagBuilder("span");
            tagTextSpan.AddCssClass("ui-button-text");
            tagTextSpan.InnerHtml = string.IsNullOrEmpty(htmlContent) ? "&nbsp;" : htmlContent;

            TagBuilder tagIconSpan = new TagBuilder("span");
            //tagIconSpan.AddCssClass("ui-button-icon-primary ui-icon");
            //tagIconSpan.AddCssClass(jQueryHelpers.IconToCssClass(buttonIcon));
            tagIconSpan.AddCssClass("ui-button-icon-primary");
            tagIconSpan.AddCssClass("fa fa-volume-up fa-lg ui-icon");

            TagBuilder tagButton = new TagBuilder("button");
            tagButton.MergeAttributes(htmlAttributes);
            tagButton.AddCssClass("ui-widgetButton ui-button ui-widget ui-state-default ui-corner-all");
            tagButton.Attributes.Add("data-widget", "widgetButton");
            tagButton.Attributes.Add("type", buttonType == ButtonType.Button ? "button" : "submit");
            tagButton.InnerHtml += tagTextSpan.ToString(TagRenderMode.Normal);
            if (string.IsNullOrEmpty(htmlContent))
            {
                tagButton.AddCssClass("ui-button-icon-only");
            }
            else
            {
                tagButton.AddCssClass("ui-button-text-icon-primary");
            }

            if (buttonIcon != jQueryHelpers.Icon.None)
            {
                tagButton.InnerHtml += tagIconSpan.ToString(TagRenderMode.Normal);
            }

            return MvcHtmlString.Create(tagButton.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string htmlContent, jQueryHelpers.Icon buttonIcon, ButtonType buttonType, object htmlAttributes = null)
        {
            return ButtonExtensions.Button(htmlHelper, htmlContent, buttonIcon, buttonType, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string htmlContent, jQueryHelpers.Icon buttonIcon, ButtonType buttonType, bool requiresConfirmation, object htmlAttributes = null)
        {
            IDictionary<string, object> attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (requiresConfirmation)
            {
                attrs.Add("data-widget-confirmButton", true);
            }
            return ButtonExtensions.Button(htmlHelper, htmlContent, buttonIcon, buttonType, attrs);
        }

    }
    #endregion

    #region FormItem Extensions
    public static class FormItemExtensions
    {
        public static MvcHtmlString FormItem<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            ModelMetadata meta = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            bool IsNullable = meta.ModelType.IsGenericType && meta.ModelType.GetGenericTypeDefinition() == typeof(Nullable<>);
            bool IsEnum = meta.ModelType.IsEnum;

            if (IsNullable)
            {
                IsEnum = Nullable.GetUnderlyingType(meta.ModelType).IsEnum;
            }

            if (IsEnum)
            {
                IEnumerable<SelectListItem> listedValues = ((Enum)meta.Model).ToSelectList(Nullable.GetUnderlyingType(meta.ModelType));
                if (listedValues.Count() < 6)
                {
                    return FormItemExtensions.FormItem(htmlHelper,
                                                    expression,
                                                    FormsExtension.RadioButtonFor(htmlHelper, expression, listedValues, Position.Horizontal),
                                                    string.Empty);
                }
                else
                {
                    return FormItemExtensions.FormItem(htmlHelper,
                                                    expression,
                                                    FormsExtension.DropDownListFor(htmlHelper, expression, listedValues, htmlAttributes),
                                                    string.Empty);
                }
            }
            else
            {
                return FormItemExtensions.FormItem(htmlHelper, expression, meta.DisplayName);
            }
        }

        public static MvcHtmlString FormItem<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string customLabel)
        {
            ModelMetadata meta = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            MvcHtmlString inputObject = htmlHelper.EditorFor(expression);
            return FormItem(htmlHelper, expression, inputObject, customLabel);
        }

        public static MvcHtmlString FormItem<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, MvcHtmlString inputObject, string customLabel)
        {
            ModelMetadata meta = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string displayName = string.IsNullOrEmpty(meta.DisplayName) ? meta.PropertyName : meta.DisplayName;
            string displayValue = string.IsNullOrEmpty(customLabel) ? displayName : customLabel;
            bool hasModelError = !htmlHelper.ViewData.ModelState.IsValidField(meta.PropertyName);
            string htmlString = string.Format(@"<div data-widget='widgetModelItem' class='ui-widgetFormItem {3} {4}'>
                                                                            <div class='ui-widgetForm-inputLabel'>{0} </div>
                                                                            <div class='ui-widgetForm-inputValue'>{1}</div>
                                                                            <div class='ui-widgetForm-inputError {5}'>{2}</div>
                                                                            <div class='ui-carriageReturn'></div>
                                                                        </div>"
                                                                        , displayValue
                                                                        , inputObject
                                                                        , hasModelError ? htmlHelper.ValidationMessageCustomizedFor(expression).ToString() : string.Empty
                                                                        , string.Format("ui-widgetForm-{0}", meta.PropertyName)
                                                                        , hasModelError ? "ui-state-error ui-corner-all" : string.Empty
                                                                        , hasModelError ? string.Empty : "ui-hidden"
                                                                        );
            return MvcHtmlString.Create(htmlString);
        }

    }
    #endregion

    #region Menu Extensions
    public static class MenuExtensions
    {
        public static MenuModel MenuGet(this HtmlHelper Html)
        {
            UrlHelper url = new UrlHelper(Html.ViewContext.RequestContext);
            MenuModel result = new MenuModel()
            {
                MenuItems = new List<MenuItemModel>() { 
                    new MenuItemModel(){DataAction = HomeUrlHelper.Home_Index(url),Description = GeneralTexts.Home},
                    new MenuItemModel(){DataAction = BlogUrlHelper.IndexRoot(url),Description = GeneralTexts.Blog},
                    new MenuItemModel(){DataAction = HomeUrlHelper.Home_About(url),Description = GeneralTexts.About},
                    new MenuItemModel(){DataAction = UserAccountUrlHelper.Account_Dashboard(url),Description = GeneralTexts.Dashboard}
                }
            };

            bool isLoggedIn = MvcApplication.UserRequest.UserIsLoggedIn;
            if (!isLoggedIn)
            {
                result.MenuItems.RemoveAll(x => x.DataAction == UserAccountUrlHelper.Account_Dashboard(url));
            }

            return result;
        }
    }
    #endregion

    #region WidgetMessage
    public static class WidgetMessageExtensions
    {
        private static TagBuilder WidgetMessageInit(HtmlHelper htmlHelper, DataResultMessageType messageType, string content, bool allowClose, bool autoHide)
        {
            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("data-widget", "widgetMsg");
            div.Attributes.Add("data-widget-msgType", ((int)messageType).ToString());
            if (messageType != DataResultMessageType.Confirm)
            {
                div.Attributes.Add("data-widget-allowClose", allowClose.ToString().ToLower());
                div.Attributes.Add("data-widget-autoHide", autoHide.ToString().ToLower());
            }
            div.InnerHtml = content;
            return div;
        }

        public static MvcHtmlString WidgetMessage(this HtmlHelper htmlHelper, DataResultMessageType messageType, string content, bool allowClose, bool autoHide = false)
        {
            TagBuilder div = WidgetMessageExtensions.WidgetMessageInit(htmlHelper, messageType, content, allowClose, autoHide);
            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString WidgetMessageConfirm(this HtmlHelper htmlHelper, string content, string jsOnAccept, string jsOnCancel)
        {
            TagBuilder div = WidgetMessageExtensions.WidgetMessageInit(htmlHelper, DataResultMessageType.Confirm, content, false, false);
            div.Attributes.Add("data-widget-onAccept", jsOnAccept);
            div.Attributes.Add("data-widget-onCancel", jsOnCancel);
            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString WidgetMessage(this HtmlHelper htmlHelper, DataResultMessageType messageType, string content)
        {
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("ui-widget-content ui-corner-all ui-message-textOnly");
            switch (messageType)
            {
                case DataResultMessageType.Success:
                    div.AddCssClass("ui-state-active");
                    break;
                case DataResultMessageType.Warning:
                    div.AddCssClass("ui-state-highlight");
                    break;
                case DataResultMessageType.Error:
                    div.AddCssClass("ui-state-error");
                    break;
                case DataResultMessageType.Confirm:
                    break;
                default:
                    break;
            }
            div.InnerHtml = content;
            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

    }
    #endregion

    #region WebGrid Extensions
    public class WebGrid<TListOf, TModel, TDataFilter> : WebGrid
    {
        public static string WebGrid_HiddenFieldFilterSerializedKey = "WebGrid_filterSerialized";
        public static string WebGrid_HiddenFieldEventKey = "WebGrid_event";
        public static string WebGrid_HiddenFieldPageSizeKey = "WebGrid_PageSize";
        public static string WebGrid_HiddenFieldPageSizeEventKey = "WebGrid_PageSizeChanged";
        public static string WebGrid_HiddenFieldPaginationEventKey = "WebGrid_Pagination";

        public static bool IsWebGridEvent()
        {
            return HttpContext.Current.Request.Params.AllKeys.Contains(WebGrid_HiddenFieldEventKey);
        }

        public static TDataFilter GetDataFilterFromPost()
        {
            TDataFilter dataFilter = baseModel.DeserializeFromJson<TDataFilter>(HttpUtility.HtmlDecode((string)System.Web.HttpContext.Current.Request[WebGrid_HiddenFieldFilterSerializedKey]));
            if (System.Web.HttpContext.Current.Request[WebGrid_HiddenFieldEventKey] == WebGrid_HiddenFieldPageSizeEventKey)
            {
                ((IDataFilter)dataFilter).Page = 0;
                ((IDataFilter)dataFilter).PageSize = Convert.ToInt32(System.Web.HttpContext.Current.Request["WebGrid_PageSize"]);
            }
            return dataFilter;
        }

        public WebGrid(IEnumerable<TListOf> source = null, IEnumerable<string> columnNames = null, string defaultSort = null, int rowsPerPage = 10, bool canPage = true, bool canSort = true, string ajaxUpdateContainerId = null, string ajaxUpdateCallback = null, string fieldNamePrefix = null, string pageFieldName = null, string selectionFieldName = null, string sortFieldName = null, string sortDirectionFieldName = null)
            : base(source == null ? null : source.Cast<object>(), columnNames, defaultSort, rowsPerPage, canPage, canSort, ajaxUpdateContainerId, ajaxUpdateCallback, fieldNamePrefix, pageFieldName, selectionFieldName, sortFieldName, sortDirectionFieldName)
        {

        }

        public WebGrid<TListOf, TModel, TDataFilter> Bind(IEnumerable<TListOf> source, IEnumerable<string> columnNames = null, bool autoSortAndPage = true, int rowCount = -1)
        {
            base.Bind(source == null ? null : source.Cast<object>(), columnNames, autoSortAndPage, rowCount);
            return this;
        }

        public WebGridColumn Column(string columnName = null, string header = null, Func<TListOf, object> format = null, string style = null, bool canSort = true)
        {
            Func<dynamic, object> wrappedFormat = null;
            if (format != null)
            {
                wrappedFormat = o => format((TListOf)o.Value);
            }
            WebGridColumn column = base.Column(columnName, header, wrappedFormat, style, canSort);
            return column;
        }

        public string EmptyResultsMessage { get; set; }
        public IEnumerable<WebGridColumn> columns { get; set; }
        public HtmlHelper<TModel> htmlHelper { get; set; }
        public Expression<Func<TModel, IDataResultPaginatedModel<TListOf>>> expressionToPaginatedResults { get; set; }
        public Expression<Func<TModel, IDataFilter>> expressionIDataFiler { get; set; }

        public WebGridStyle webGridStyle { get; set; }

        private IHtmlString RenderHeader()
        {
            return htmlHelper.Display(this.FieldNamePrefix,
                                                        "WebGridHeader",
                                                        new
                                                        {
                                                            Rows = this.Rows,
                                                            Columns = this.columns,
                                                            WebGridStyle = this.webGridStyle,
                                                            dataFilter = (IDataFilter)ModelMetadata.FromLambdaExpression(this.expressionIDataFiler, htmlHelper.ViewData).Model
                                                        });
        }

        private IHtmlString RenderBody()
        {
            return htmlHelper.Display(this.FieldNamePrefix,
                                                        "WebGridBody",
                                                        new
                                                        {
                                                            Rows = this.Rows,
                                                            Columns = this.columns,
                                                            WebGridStyle = this.webGridStyle
                                                        });
        }

        private IHtmlString RenderPagination()
        {
            IDataResultPaginatedModel<TListOf> dataPaginated = (IDataResultPaginatedModel<TListOf>)ModelMetadata.FromLambdaExpression(expressionToPaginatedResults, htmlHelper.ViewData).Model;
            IDataFilter dataFilter = (IDataFilter)ModelMetadata.FromLambdaExpression(this.expressionIDataFiler, htmlHelper.ViewData).Model;

            if (!dataPaginated.Page.HasValue)
            {
                dataPaginated.Page = 0;
            }

            return htmlHelper.Display(this.FieldNamePrefix,
                                                        "WebGridPager",
                                                        new
                                                        {
                                                            dataFilter = dataFilter,
                                                            dataPaginated = dataPaginated
                                                        });
        }

        public MvcHtmlString Render()
        {
            IDataResultPaginatedModel<TListOf> dataPaginated = (IDataResultPaginatedModel<TListOf>)ModelMetadata.FromLambdaExpression(expressionToPaginatedResults, htmlHelper.ViewData).Model;

            bool isEmptyGrid = dataPaginated.Data.Count() == 0;
            IHtmlString listHeader = isEmptyGrid ? MvcHtmlString.Empty : this.RenderHeader(/*this.columns, "ui-widget-header ui-priority-secondary"*/);
            IHtmlString listBody = isEmptyGrid ? MvcHtmlString.Empty : this.RenderBody();
            IHtmlString listPager = isEmptyGrid ? MvcHtmlString.Empty : this.RenderPagination();
            IHtmlString emptyResultsMessage = MvcHtmlString.Create(string.IsNullOrEmpty(this.EmptyResultsMessage) ? $customNamespace$.Resources.General.GeneralTexts.NoDataFound : this.EmptyResultsMessage);
            WebGridStyle webGridStyle = this.webGridStyle;
            int columnsCount = this.columns.Count();


            return htmlHelper.Display(this.FieldNamePrefix,
                                                        "WebGrid",
                                                        new
                                                        {
                                                            listHeader = listHeader,
                                                            listBody = listBody,
                                                            listPager = listPager,
                                                            emptyResultsMessage = emptyResultsMessage,
                                                            webGridStyle = webGridStyle,
                                                            columnsCount = columnsCount,
                                                            isEmptyGrid = isEmptyGrid
                                                        });
        }
    }
    public enum WebGridStyle : int
    {
        Table = 0,
        TableLess = 1
    }
    public static class WebGridExtensions
    {
        public static WebGrid<T, TModel, TDataFilter> WebGrid<T, TModel, TDataFilter>(
                                                                    this HtmlHelper<TModel> htmlHelper,
                                                                    Expression<Func<TModel, IDataResultPaginatedModel<T>>> expressionToPaginatedResults,
                                                                    Expression<Func<TModel, IDataFilter>> expressionToFilter,
                                                                    IEnumerable<WebGridColumn> columns,
                                                                    WebGridStyle webGridStyle = WebGridStyle.Table,
                                                                    string emptyResultsMessage = null)
        {
            IDataFilter dataFilter = (IDataFilter)ModelMetadata.FromLambdaExpression(expressionToFilter, htmlHelper.ViewData).Model;
            IDataResultPaginatedModel<T> dataPaginated = (IDataResultPaginatedModel<T>)ModelMetadata.FromLambdaExpression(expressionToPaginatedResults, htmlHelper.ViewData).Model;

            WebGrid<T, TModel, TDataFilter> result = new WebGrid<T, TModel, TDataFilter>(null, rowsPerPage: dataFilter.PageSize, canSort: true, canPage: true)
                                                                        .Bind(dataPaginated.Data, rowCount: dataPaginated.TotalRows, autoSortAndPage: false);

            result.htmlHelper = htmlHelper;

            result.expressionToPaginatedResults = expressionToPaginatedResults;
            result.expressionIDataFiler = expressionToFilter;
            result.columns = columns;
            result.EmptyResultsMessage = emptyResultsMessage;
            result.webGridStyle = webGridStyle;
            return result;
        }
    }
    #endregion

    #region WidgetFieldSet
    public class MvcWidgetFieldSet : IDisposable
    {
        private bool _disposed;
        private readonly TextWriter _writer;

        public MvcWidgetFieldSet(ViewContext viewContext, string legend)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            _writer = viewContext.Writer;
            if (!string.IsNullOrEmpty(legend))
            {
                _writer.Write(string.Format("<legend>{0}</legend>", legend));
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Write("</fieldset>");
            }
        }

        public void EndForm()
        {
            Dispose(true);
        }
    }
    public static class MvcFieldSetExtensions
    {
        public static MvcWidgetFieldSet BeginWidgetFieldSet(this HtmlHelper htmlHelper, string legend, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("fieldset");
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tagBuilder.AddCssClass("ui-widget-content");
            tagBuilder.AddCssClass("ui-corner-all");
            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcWidgetFieldSet(htmlHelper.ViewContext, legend);
        }
    }
    #endregion

    #region WidgetContent
    public class MvcWidgetBox : IDisposable
    {
        private bool _disposed;
        private readonly FormContext _originalFormContext;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;



        public MvcWidgetBox(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            _viewContext = viewContext;
            _writer = viewContext.Writer;

            // push the new FormContext
            _originalFormContext = viewContext.FormContext;
            viewContext.FormContext = new FormContext();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Write("</div>");

                // output client validation and restore the original form context
                if (_viewContext != null)
                {
                    _viewContext.OutputClientValidation();
                    _viewContext.FormContext = _originalFormContext;
                }
            }
        }

        public void EndForm()
        {
            Dispose(true);
        }
    }
    public class MvcWidgetBoxHeader : MvcWidgetBox
    {
        public MvcWidgetBoxHeader(ViewContext viewContext) : base(viewContext) { }
    }
    public class MvcWidgetBoxContent : MvcWidgetBox
    {
        public MvcWidgetBoxContent(ViewContext viewContext) : base(viewContext) { }
    }
    public class MvcWidgetButtonsBox : MvcWidgetBox
    {
        public MvcWidgetButtonsBox(ViewContext viewContext) : base(viewContext) { }
    }
    public static class WidgetContentExtensions
    {
        private static TagBuilder BeginWidgetBoxInit<TModel>(this HtmlHelper<TModel> helper, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tagBuilder.AddCssClass("ui-widget-content");
            tagBuilder.AddCssClass("ui-corner-all");
            return tagBuilder;
        }
        public static MvcWidgetBox BeginWidgetBox<TModel>(this HtmlHelper<TModel> helper, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = WidgetContentExtensions.BeginWidgetBoxInit(helper, htmlAttributes);
            helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcWidgetBox(helper.ViewContext);
        }
        public static MvcWidgetBox BeginWidgetBox<TModel>(this HtmlHelper<TModel> helper, bool allowClose, bool allowCollapse, bool isCollapsed, string jsOnCollapse, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = WidgetContentExtensions.BeginWidgetBoxInit(helper, htmlAttributes);
            tagBuilder.Attributes.Add("data-widget", "widgetBase");
            tagBuilder.Attributes.Add("data-widget-allowClose", allowClose.ToString().ToLower());
            tagBuilder.Attributes.Add("data-widget-allowCollapse", allowCollapse.ToString().ToLower());
            tagBuilder.Attributes.Add("data-widget-isCollapsed", isCollapsed.ToString().ToLower());
            tagBuilder.Attributes.Add("data-widget-jsOnCollapse", jsOnCollapse);
            helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcWidgetBox(helper.ViewContext);
        }
        public static MvcWidgetBoxHeader BeginWidgetBoxHeader<TModel>(this HtmlHelper<TModel> helper, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tagBuilder.AddCssClass("ui-widget-header");
            tagBuilder.AddCssClass("ui-corner-top");
            //tagBuilder.AddCssClass("ui-state-active");
            helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcWidgetBoxHeader(helper.ViewContext);
        }
        public static MvcWidgetBoxContent BeginWidgetBoxContent<TModel>(this HtmlHelper<TModel> helper, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tagBuilder.AddCssClass("ui-widgetForm-content");
            tagBuilder.AddCssClass("ui-widget-content");
            tagBuilder.AddCssClass("ui-corner-bottom");
            helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcWidgetBoxContent(helper.ViewContext);
        }
        public static MvcWidgetButtonsBox BeginWidgetButtonsBox<TModel>(this HtmlHelper<TModel> helper, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tagBuilder.AddCssClass("ui-widgetFormItem");
            tagBuilder.AddCssClass("ui-widgetForm-buttons");
            helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcWidgetButtonsBox(helper.ViewContext);
        }

    }
    #endregion
}
