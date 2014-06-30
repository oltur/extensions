﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.Chart.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    
    #line 4 "..\..\Chart\Views\ChartResults.cshtml"
    using System.Web.Mvc;
    
    #line default
    #line hidden
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 3 "..\..\Chart\Views\ChartResults.cshtml"
    using Signum.Engine;
    
    #line default
    #line hidden
    
    #line 5 "..\..\Chart\Views\ChartResults.cshtml"
    using Signum.Entities;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Chart\Views\ChartResults.cshtml"
    using Signum.Entities.Chart;
    
    #line default
    #line hidden
    
    #line 1 "..\..\Chart\Views\ChartResults.cshtml"
    using Signum.Entities.DynamicQuery;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Chart\Views\ChartResults.cshtml"
    using Signum.Entities.Reflection;
    
    #line default
    #line hidden
    using Signum.Utilities;
    
    #line 6 "..\..\Chart\Views\ChartResults.cshtml"
    using Signum.Web;
    
    #line default
    #line hidden
    
    #line 8 "..\..\Chart\Views\ChartResults.cshtml"
    using Signum.Web.Chart;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Chart/Views/ChartResults.cshtml")]
    public partial class ChartResults : System.Web.Mvc.WebViewPage<TypeContext<ChartRequest>>
    {
        public ChartResults()
        {
        }
        public override void Execute()
        {
            
            #line 10 "..\..\Chart\Views\ChartResults.cshtml"
   
    ResultTable queryResult = (ResultTable)ViewData[ViewDataKeys.Results];
    var mode = (ChartRequestMode)ViewData["mode"];

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 14 "..\..\Chart\Views\ChartResults.cshtml"
 if(mode == ChartRequestMode.complete)
{
    using(var tabs = Html.Tabs(Model))
    {
        tabs.Tab("sfChartContainer", ChartMessage.Chart.NiceToString(), 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<div");

WriteLiteralTo(__razor_template_writer, " class=\"sf-chart-container\"");

WriteLiteralTo(__razor_template_writer, " \r\n                data-json=\"");

            
            #line 19 "..\..\Chart\Views\ChartResults.cshtml"
WriteTo(__razor_template_writer, Html.Json(ChartUtils.DataJson(Model.Value, queryResult)).ToString());

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\"");

WriteLiteralTo(__razor_template_writer, ">\r\n            </div>");

})
            
            #line 20 "..\..\Chart\Views\ChartResults.cshtml"
                  );

        tabs.Tab("sfChartData", ChartMessage.Data.NiceToString(), 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "\r\n");

            
            #line 23 "..\..\Chart\Views\ChartResults.cshtml"
            
            
            #line default
            #line hidden
            
            #line 23 "..\..\Chart\Views\ChartResults.cshtml"
              Html.RenderPartial(ChartClient.ChartResultsTableView, Model, ViewData);
            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n        ");

})
            
            #line 24 "..\..\Chart\Views\ChartResults.cshtml"
               );
    }
}
else if (mode == ChartRequestMode.chart)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"sf-chart-container\"");

WriteLiteral("data-json=\"");

            
            #line 29 "..\..\Chart\Views\ChartResults.cshtml"
                                         Write(Html.Json(ChartUtils.DataJson(Model.Value, queryResult)).ToString());

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n    </div>\r\n");

            
            #line 31 "..\..\Chart\Views\ChartResults.cshtml"
}
else if (mode == ChartRequestMode.data)
{
    Html.RenderPartial(ChartClient.ChartResultsTableView, Model, ViewData);
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
