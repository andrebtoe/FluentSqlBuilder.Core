#pragma checksum "C:\develop\Publicos\fluent-sql-builder\src\FluentSqlBuilder.Playground.Web\Pages\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "456b9551becfbf1543332424bfa994dabc995a56"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(FluentSqlBuilder.Playground.Web.Pages.Pages_Index), @"mvc.1.0.razor-page", @"/Pages/Index.cshtml")]
namespace FluentSqlBuilder.Playground.Web.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\develop\Publicos\fluent-sql-builder\src\FluentSqlBuilder.Playground.Web\Pages\_ViewImports.cshtml"
using FluentSqlBuilder.Playground.Web;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"456b9551becfbf1543332424bfa994dabc995a56", @"/Pages/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c0458dea1c516fb4f094c6dbb8596030a75397cc", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\develop\Publicos\fluent-sql-builder\src\FluentSqlBuilder.Playground.Web\Pages\Index.cshtml"
  
    ViewData["Title"] = "Home page";
    var selects = ViewData["Selects"] as List<A>;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center\">\r\n");
#nullable restore
#line 9 "C:\develop\Publicos\fluent-sql-builder\src\FluentSqlBuilder.Playground.Web\Pages\Index.cshtml"
     foreach (var itemSql in selects)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"card\" style=\"width: 18rem;\">\r\n            <div class=\"card-body\">\r\n                <h5 class=\"card-title\">Card title</h5>\r\n                <p class=\"card-text\">");
#nullable restore
#line 14 "C:\develop\Publicos\fluent-sql-builder\src\FluentSqlBuilder.Playground.Web\Pages\Index.cshtml"
                                Write(itemSql.SqlSelect);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                <a href=\"#\" class=\"btn btn-primary\">Go somewhere</a>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 18 "C:\develop\Publicos\fluent-sql-builder\src\FluentSqlBuilder.Playground.Web\Pages\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel>)PageContext?.ViewData;
        public IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
