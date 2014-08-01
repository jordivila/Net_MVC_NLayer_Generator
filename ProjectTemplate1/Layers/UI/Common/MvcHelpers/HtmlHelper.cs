using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using $customNamespace$.Models;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Resources.Account;
using $customNamespace$.Resources.General;
using $customNamespace$.Resources.Helpers;
using $customNamespace$.UI.Web.Areas.Blog;
using $customNamespace$.UI.Web.Areas.Home;
using $customNamespace$.UI.Web.Areas.Test;
using $customNamespace$.UI.Web.Areas.UserAccount;
using $customNamespace$.UI.Web.Areas.UserProfile;
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
            [EnumMember(Value = "")]
            None,

            [EnumMember(Value = "")]
            ui_icon_carat_1_n,
            
            [EnumMember(Value = "")]
            ui_icon_carat_1_ne,

            [EnumMember(Value = "")]
            ui_icon_carat_1_e,

            [EnumMember(Value = "")]
            ui_icon_carat_1_se,

            [EnumMember(Value = "")]
            ui_icon_carat_1_s,

            [EnumMember(Value = "")]
            ui_icon_carat_1_sw,

            [EnumMember(Value = "")]
            ui_icon_carat_1_w,

            [EnumMember(Value = "")]
            ui_icon_carat_1_nw,

            [EnumMember(Value = "")]
            ui_icon_carat_2_n_s,

            [EnumMember(Value = "")]
            ui_icon_carat_2_e_w,

            [EnumMember(Value = "")]
            ui_icon_triangle_1_n,

            [EnumMember(Value = "")]
            ui_icon_triangle_1_ne,

            [EnumMember(Value = "")]
            ui_icon_triangle_1_e,

            [EnumMember(Value = "")]
            ui_icon_triangle_1_se,

            [EnumMember(Value = "")]
            ui_icon_triangle_1_s,

            [EnumMember(Value = "")]
            ui_icon_triangle_1_sw,

            [EnumMember(Value = "")]
            ui_icon_triangle_1_w,

            [EnumMember(Value = "")]
            ui_icon_triangle_1_nw,

            [EnumMember(Value = "")]
            ui_icon_triangle_2_n_s,

            [EnumMember(Value = "")]
            ui_icon_triangle_2_e_w,

            [EnumMember(Value = "")]
            ui_icon_arrow_1_n,

            [EnumMember(Value = "")]
            ui_icon_arrow_1_ne,

            [EnumMember(Value = "")]
            ui_icon_arrow_1_e,

            [EnumMember(Value = "")]
            ui_icon_arrow_1_se,

            [EnumMember(Value = "")]
            ui_icon_arrow_1_s,

            [EnumMember(Value = "")]
            ui_icon_arrow_1_sw,

            [EnumMember(Value = "")]
            ui_icon_arrow_1_w,

            [EnumMember(Value = "")]
            ui_icon_arrow_1_nw,

            [EnumMember(Value = "")]
            ui_icon_arrow_2_n_s,

            [EnumMember(Value = "")]
            ui_icon_arrow_2_ne_sw,

            [EnumMember(Value = "")]
            ui_icon_arrow_2_e_w,

            [EnumMember(Value = "")]
            ui_icon_arrow_2_se_nw,

            [EnumMember(Value = "")]
            ui_icon_arrowstop_1_n,

            [EnumMember(Value = "")]
            ui_icon_arrowstop_1_e,

            [EnumMember(Value = "")]
            ui_icon_arrowstop_1_s,

            [EnumMember(Value = "")]
            ui_icon_arrowstop_1_w,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_1_n,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_1_ne,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_1_e,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_1_se,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_1_s,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_1_sw,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_1_w,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_1_nw,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_2_n_s,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_2_ne_sw,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_2_e_w,

            [EnumMember(Value = "")]
            ui_icon_arrowthick_2_se_nw,

            [EnumMember(Value = "")]
            ui_icon_arrowthickstop_1_n,

            [EnumMember(Value = "")]
            ui_icon_arrowthickstop_1_e,

            [EnumMember(Value = "")]
            ui_icon_arrowthickstop_1_s,

            [EnumMember(Value = "")]
            ui_icon_arrowthickstop_1_w,

            [EnumMember(Value = "")]
            ui_icon_arrowreturnthick_1_w,

            [EnumMember(Value = "")]
            ui_icon_arrowreturnthick_1_n,

            [EnumMember(Value = "")]
            ui_icon_arrowreturnthick_1_e,

            [EnumMember(Value = "")]
            ui_icon_arrowreturnthick_1_s,

            [EnumMember(Value = "")]
            ui_icon_arrowreturn_1_w,

            [EnumMember(Value = "")]
            ui_icon_arrowreturn_1_n,
            
            [EnumMember(Value = "")]
            ui_icon_arrowreturn_1_e,

            [EnumMember(Value = "")]
            ui_icon_arrowreturn_1_s,

            [EnumMember(Value = "")]
            ui_icon_arrowrefresh_1_w,

            [EnumMember(Value = "")]
            ui_icon_arrowrefresh_1_n,

            [EnumMember(Value = "")]
            ui_icon_arrowrefresh_1_e,

            [EnumMember(Value = "")]
            ui_icon_arrowrefresh_1_s,

            [EnumMember(Value = "")]
            ui_icon_arrow_4,

            [EnumMember(Value = "")]
            ui_icon_arrow_4_diag,

            [EnumMember(Value = "")]
            ui_icon_extlink,

            [EnumMember(Value = "")]
            ui_icon_newwin,

            [EnumMember(Value = "")]
            ui_icon_refresh,

            [EnumMember(Value = "")]
            ui_icon_shuffle,

            [EnumMember(Value = "")]
            ui_icon_transfer_e_w,

            [EnumMember(Value = "")]
            ui_icon_transferthick_e_w,

            [EnumMember(Value = "")]
            ui_icon_folder_collapsed,

            [EnumMember(Value = "")]
            ui_icon_folder_open,

            [EnumMember(Value = "")]
            ui_icon_document,

            [EnumMember(Value = "")]
            ui_icon_document_b,

            [EnumMember(Value = "")]
            ui_icon_note,

            [EnumMember(Value = "")]
            ui_icon_mail_closed,

            [EnumMember(Value = "")]
            ui_icon_mail_open,

            [EnumMember(Value = "")]
            ui_icon_suitcase,

            [EnumMember(Value = "")]
            ui_icon_comment,

            [EnumMember(Value="fa-male")]
            ui_icon_person,

            [EnumMember(Value = "")]
            ui_icon_print,

            [EnumMember(Value = "")]
            ui_icon_trash,

            [EnumMember(Value = "")]
            ui_icon_locked,

            [EnumMember(Value = "")]
            ui_icon_unlocked,

            [EnumMember(Value = "")]
            ui_icon_bookmark,

            [EnumMember(Value = "")]
            ui_icon_tag,

            [EnumMember(Value = "")]
            ui_icon_home,

            [EnumMember(Value = "")]
            ui_icon_flag,

            [EnumMember(Value = "")]
            ui_icon_calculator,

            [EnumMember(Value = "")]
            ui_icon_cart,

            [EnumMember(Value = "")]
            ui_icon_pencil,

            [EnumMember(Value = "")]
            ui_icon_clock,

            [EnumMember(Value = "")]
            ui_icon_disk,

            [EnumMember(Value = "")]
            ui_icon_calendar,

            [EnumMember(Value = "")]
            ui_icon_zoomin,

            [EnumMember(Value = "")]
            ui_icon_zoomout,

            [EnumMember(Value = "")]
            ui_icon_search,

            [EnumMember(Value = "")]
            ui_icon_wrench,

            [EnumMember(Value = "")]
            ui_icon_gear,

            [EnumMember(Value = "")]
            ui_icon_heart,

            [EnumMember(Value = "")]
            ui_icon_star,

            [EnumMember(Value = "")]
            ui_icon_link,

            [EnumMember(Value = "")]
            ui_icon_cancel,

            [EnumMember(Value = "")]
            ui_icon_plus,

            [EnumMember(Value = "")]
            ui_icon_plusthick,

            [EnumMember(Value = "")]
            ui_icon_minus,

            [EnumMember(Value = "")]
            ui_icon_minusthick,

            [EnumMember(Value = "")]
            ui_icon_close,

            [EnumMember(Value = "")]
            ui_icon_closethick,

            [EnumMember(Value = "fa-key")]
            ui_icon_key,

            [EnumMember(Value = "")]
            ui_icon_lightbulb,

            [EnumMember(Value = "")]
            ui_icon_scissors,

            [EnumMember(Value = "")]
            ui_icon_clipboard,

            [EnumMember(Value = "")]
            ui_icon_copy,

            [EnumMember(Value = "")]
            ui_icon_contact,

            [EnumMember(Value = "")]
            ui_icon_image,

            [EnumMember(Value = "")]
            ui_icon_video,

            [EnumMember(Value = "")]
            ui_icon_script,

            [EnumMember(Value = "")]
            ui_icon_alert,

            [EnumMember(Value = "")]
            ui_icon_info,

            [EnumMember(Value = "")]
            ui_icon_notice,

            [EnumMember(Value = "")]
            ui_icon_help,

            [EnumMember(Value = "")]
            ui_icon_check,

            [EnumMember(Value = "")]
            ui_icon_bullet,

            [EnumMember(Value = "")]
            ui_icon_radio_off,

            [EnumMember(Value = "")]
            ui_icon_radio_on,

            [EnumMember(Value = "")]
            ui_icon_pin_w,

            [EnumMember(Value = "")]
            ui_icon_pin_s,

            [EnumMember(Value = "")]
            ui_icon_play,

            [EnumMember(Value = "")]
            ui_icon_pause,

            [EnumMember(Value = "")]
            ui_icon_seek_next,

            [EnumMember(Value = "")]
            ui_icon_seek_prev,

            [EnumMember(Value = "")]
            ui_icon_seek_end,

            [EnumMember(Value = "")]
            ui_icon_seek_first,

            [EnumMember(Value = "")]
            ui_icon_stop,

            [EnumMember(Value = "")]
            ui_icon_eject,

            [EnumMember(Value = "")]
            ui_icon_volume_off,

            [EnumMember(Value = "")]
            ui_icon_volume_on,

            [EnumMember(Value = "")]
            ui_icon_power,

            [EnumMember(Value = "")]
            ui_icon_signal_diag,

            [EnumMember(Value = "")]
            ui_icon_signal,
            
            [EnumMember(Value = "")]
            ui_icon_battery_0,

            [EnumMember(Value = "")]
            ui_icon_battery_1,

            [EnumMember(Value = "")]
            ui_icon_battery_2,

            [EnumMember(Value = "")]
            ui_icon_battery_3,

            [EnumMember(Value = "")]
            ui_icon_circle_plus,

            [EnumMember(Value = "")]
            ui_icon_circle_minus,

            [EnumMember(Value = "")]
            ui_icon_circle_close,

            [EnumMember(Value = "")]
            ui_icon_circle_triangle_e,

            [EnumMember(Value = "")]
            ui_icon_circle_triangle_s,

            [EnumMember(Value = "")]
            ui_icon_circle_triangle_w,

            [EnumMember(Value = "")]
            ui_icon_circle_triangle_n,

            [EnumMember(Value = "")]
            ui_icon_circle_arrow_e,

            [EnumMember(Value = "")]
            ui_icon_circle_arrow_s,

            [EnumMember(Value = "")]
            ui_icon_circle_arrow_w,

            [EnumMember(Value = "")]
            ui_icon_circle_arrow_n,

            [EnumMember(Value = "")]
            ui_icon_circle_zoomin,

            [EnumMember(Value = "")]
            ui_icon_circle_zoomout,

            [EnumMember(Value = "")]
            ui_icon_circle_check,

            [EnumMember(Value = "")]
            ui_icon_circlesmall_plus,

            [EnumMember(Value = "")]
            ui_icon_circlesmall_minus,

            [EnumMember(Value = "")]
            ui_icon_circlesmall_close,

            [EnumMember(Value = "")]
            ui_icon_squaresmall_plus,

            [EnumMember(Value = "")]
            ui_icon_squaresmall_minus,

            [EnumMember(Value = "")]
            ui_icon_squaresmall_close,

            [EnumMember(Value = "")]
            ui_icon_grip_dotted_vertical,

            [EnumMember(Value = "")]
            ui_icon_grip_dotted_horizontal,

            [EnumMember(Value = "")]
            ui_icon_grip_solid_vertical,

            [EnumMember(Value = "")]
            ui_icon_grip_solid_horizontal,

            [EnumMember(Value = "")]
            ui_icon_gripsmall_diagonal_se,

            [EnumMember(Value = "")]
            ui_icon_grip_diagonal_se,
        }

        public static string IconToCssClass(Icon buttonIcon)
        {
            string result = buttonIcon.ToString().Replace("_", "-");
            return result;
        }

        public static string IconToFontawsomeClass(Icon buttonIcon)
        {
            return string.Format("{0} fa {1}", IconToCssClass(buttonIcon), buttonIcon.ToEnumMemberString());
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
            tagIconSpan.AddCssClass("ui-button-icon-primary ui-icon");
            tagIconSpan.AddCssClass(jQueryHelpers.IconToCssClass(buttonIcon));
            //tagIconSpan.AddCssClass(jQueryHelpers.IconToFontawsomeClass(buttonIcon));
            
            //tagIconSpan.AddCssClass("ui-button-icon-primary");
            //tagIconSpan.AddCssClass("fa fa-volume-up fa-lg ui-icon");

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
            baseViewModel viewModel = (baseViewModel)Html.ViewData.Model;
            UrlHelper url = new UrlHelper(Html.ViewContext.RequestContext);


            MenuModel result = new MenuModel();

            if (MvcApplication.UserRequest.UserIsLoggedIn)
            {
                string name = string.IsNullOrEmpty(viewModel.BaseViewModelInfo.UserProfile.FirstName) ?
                                                            viewModel.BaseViewModelInfo.UserFormsIdentityName :
                                                            viewModel.BaseViewModelInfo.UserProfile.FirstName;

                result.MenuItems.Add(new MenuItemModel(string.Empty, name, new List<SiteRoles>() { SiteRoles.Guest }, new List<MenuItemModel>() 
                { 
                    new MenuItemModel(UrlHelperUserProfile.UserProfile_Edit(url), AccountResources.ProfileEdit, new List<SiteRoles>(){  SiteRoles.Guest }, null),
                    new MenuItemModel(UserAccountUrlHelper.Account_ChangePassword(url), AccountResources.ChangePassword, new List<SiteRoles>(){  SiteRoles.Guest }, null),
                    new MenuItemModel(UserAccountUrlHelper.Account_LogOff(url), AccountResources.SignOut, new List<SiteRoles>(){  SiteRoles.Guest }, null)
                }));
            }
            else
            {
                result.MenuItems.Add(new MenuItemModel(UserAccountUrlHelper.Account_LogOn(url), GeneralTexts.LogOn, new List<SiteRoles>() { SiteRoles.Guest }, null));
            }


            result.MenuItems.AddRange(new List<MenuItemModel>()
                {
                    new MenuItemModel(HomeUrlHelper.Home_Index(url), GeneralTexts.Home, new List<SiteRoles>(){  SiteRoles.Guest }, null),
                    new MenuItemModel(BlogUrlHelper.IndexRoot(url), GeneralTexts.Blog, new List<SiteRoles>(){  SiteRoles.Guest }, null),
                    new MenuItemModel(HomeUrlHelper.Home_About(url),GeneralTexts.About, new List<SiteRoles>(){  SiteRoles.Guest }, null),
                    new MenuItemModel(UserAccountUrlHelper.Account_Dashboard(url),GeneralTexts.Dashboard, new List<SiteRoles>(){  SiteRoles.Administrator }, null),
                    new MenuItemModel(TestsUrlHelper.Index(url), "UI Tests", new List<SiteRoles>(){  SiteRoles.Guest }, null)
                });




            var cultureMenuItem = new MenuItemModel(string.Empty, GeneralTexts.Languages, new List<SiteRoles>() { SiteRoles.Guest }, new List<MenuItemModel>() { });
            foreach (var item in GlobalizationHelper.CultureInfoAvailableList())
            {
                cultureMenuItem.Childs.Add(new MenuItemModel(HomeUrlHelper.Home_CultureSet(url, item.Name), item.DisplayName, new List<SiteRoles>() { SiteRoles.Guest }, null));
            }
            result.MenuItems.Add(cultureMenuItem);




            var themesMenuItem = new MenuItemModel(string.Empty, GeneralTexts.SiteThemes, new List<SiteRoles>() { SiteRoles.Guest }, new List<MenuItemModel>() { });
            foreach (var item in viewModel.BaseViewModelInfo.UserProfile.Theme.ToSelectList(typeof(ThemesAvailable)).ToList())
            {
                themesMenuItem.Childs.Add(new MenuItemModel(HomeUrlHelper.Home_ThemeSet(url, item.Value), item.Text, new List<SiteRoles>() { SiteRoles.Guest }, null));
            }
            result.MenuItems.Add(themesMenuItem);



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