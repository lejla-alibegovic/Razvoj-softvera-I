#pragma checksum "C:\Users\Lejla\Desktop\RS1_Ispit_2020_02_20_aspnet_core\RS1_Ispit\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "39f3184532d12e5048483312dae8681827bab28d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"39f3184532d12e5048483312dae8681827bab28d", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c0909514ccbe15c9b46987fb6fc827edf50cf04a", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\Users\Lejla\Desktop\RS1_Ispit_2020_02_20_aspnet_core\RS1_Ispit\Views\Home\Index.cshtml"
  
    ViewBag.Title = "Index";



#line default
#line hidden
            BeginContext(43, 43, true);
            WriteLiteral("\r\n<h2>\r\n    IB170030\r\n</h2>\r\n\r\n\r\n\r\n<br />\r\n");
            EndContext();
            BeginContext(87, 49, false);
#line 15 "C:\Users\Lejla\Desktop\RS1_Ispit_2020_02_20_aspnet_core\RS1_Ispit\Views\Home\Index.cshtml"
Write(Html.ActionLink("Ajax text", "Index", "AjaxTest"));

#line default
#line hidden
            EndContext();
            BeginContext(136, 10, true);
            WriteLiteral("\r\n<br />\r\n");
            EndContext();
            BeginContext(147, 44, false);
#line 17 "C:\Users\Lejla\Desktop\RS1_Ispit_2020_02_20_aspnet_core\RS1_Ispit\Views\Home\Index.cshtml"
Write(Html.ActionLink("DB text", "TestDB", "Home"));

#line default
#line hidden
            EndContext();
            BeginContext(191, 111, true);
            WriteLiteral("   << -- Prije nego što kliknete prepravite Connection string. Upišite vaš broj indeksa u naziv DB-a.\r\n<br />\r\n");
            EndContext();
            BeginContext(303, 58, false);
#line 19 "C:\Users\Lejla\Desktop\RS1_Ispit_2020_02_20_aspnet_core\RS1_Ispit\Views\Home\Index.cshtml"
Write(Html.ActionLink("Takmicenje/Index", "Index", "Takmicenje"));

#line default
#line hidden
            EndContext();
            BeginContext(361, 31, true);
            WriteLiteral(" << -- Potrebno implementirati!");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
