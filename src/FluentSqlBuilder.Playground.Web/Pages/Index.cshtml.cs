using FluentSqlBuilder.Core.Middlewares.Services.Interfaces;
using FluentSqlBuilder.Data.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FluentSqlBuilder.Playground.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(IFluentSqlBuilderService fluentSqlBuilderService)
        {
            var sqlBuilder = fluentSqlBuilderService.From<OrderDataModel>()
                                                    .Where(x => DateTime.Now >= x.DateTimeSave
                                                           && x.Id == 1);

            var selectToUse = sqlBuilder.ToString();
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }

    public class A
    {
        public string SqlSelect { get; set; }
    }
}