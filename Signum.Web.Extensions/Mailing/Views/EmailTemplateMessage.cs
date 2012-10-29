﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.544
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Signum.Utilities;
    using Signum.Entities;
    using Signum.Web;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using Signum.Entities.Mailing;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Mailing/Views/EmailTemplateMessage.cshtml")]
    public class _Page_Mailing_Views_EmailTemplateMessage_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {


        public _Page_Mailing_Views_EmailTemplateMessage_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {


 using (var ec = Html.TypeContext<EmailTemplateMessageDN>())
{

WriteLiteral("    <div class=\"sf-email-messageContainer\">\r\n        <input type=\"hidden\" class=\"" +
"sf-email-culture\" value=\"");


                                                         Write(ec.Value.GetCultureInfo.DisplayName);

WriteLiteral("\" />\r\n        ");


   Write(Html.ValueLine(ec, e => e.CultureInfo, vl =>
        {
            vl.ValueLineType = ValueLineType.Combo;
            vl.EnumComboItems = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures).OrderBy(
                ci => ci.Name).ThenBy(ci => ci.IsNeutralCulture).Select(ci =>
                    new SelectListItem { Text = ci.DisplayName, Value = ci.Name, Selected = ci.Name == ec.Value.CultureInfo }).ToList();
        }));

WriteLiteral("\r\n        <fieldset>\r\n            <legend>Replacement</legend>\r\n            <text" +
"area cols=\"30\" rows=\"6\"></textarea>\r\n            <input type=\"button\" class=\"sf-" +
"button sf-email-insertTokenTextButton\" value=\"");


                                                                                    Write(Signum.Web.Extensions.Properties.Resources.InsertInText);

WriteLiteral("\" />\r\n            <input type=\"button\" class=\"sf-button sf-email-insertIterationT" +
"extButton\" value=\"");


                                                                                        Write(Signum.Web.Extensions.Properties.Resources.InsertIterationInText);

WriteLiteral("\" />\r\n            <input type=\"button\" class=\"sf-button sf-email-insertTokenSubje" +
"ctButton\" value=\"");


                                                                                       Write(Signum.Web.Extensions.Properties.Resources.InsertInSubject);

WriteLiteral("\" />\r\n            <input type=\"button\" class=\"sf-button sf-email-insertIterationS" +
"ubjectButton\" value=\"");


                                                                                           Write(Signum.Web.Extensions.Properties.Resources.InsertIterationInSubject);

WriteLiteral("\" />\r\n        </fieldset>\r\n        ");


   Write(Html.ValueLine(ec, e => e.Subject));

WriteLiteral("\r\n        <div class=\"sf-email-messageEditContent\">\r\n            ");


       Write(Html.ValueLine(ec, e => e.Text, vl =>
            {
                vl.ValueLineType = ValueLineType.TextArea;
                vl.ValueHtmlProps["cols"] = "30";
                vl.ValueHtmlProps["rows"] = "6";
                vl.ValueHtmlProps["class"] = "sf-email-htmlwrite";
            }));

WriteLiteral("\r\n            <br />\r\n            <input type=\"button\" class=\"sf-button sf-email-" +
"messagePreviewContentButton\" value=\"");


                                                                                          Write(Signum.Web.Extensions.Properties.Resources.PreviewContent);

WriteLiteral(@""" />
        </div>
        <div class=""sf-email-messagePreviewContent"" style=""display: none;"">
            <fieldset>
                <legend>Message</legend>
                <iframe name=""frameNewImage"" src=""about:blank"" class=""sf-email-htmlBody""
                    frameborder=""0""></iframe>
                <br />
                <input type=""button"" class=""sf-button sf-email-messageEditContentButton"" value=""");


                                                                                           Write(Signum.Web.Extensions.Properties.Resources.EditContent);

WriteLiteral("\" />\r\n            </fieldset>\r\n        </div>\r\n    </div>\r\n");


}


Write(Html.ScriptsJs("~/Mailing/Scripts/SF_Mail.js"));

WriteLiteral("\r\n");


        }
    }
}
