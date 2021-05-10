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
            //var sqlBuilder01 = fluentSqlBuilderService.From<OrderDataModel>()
            //                                          .Where(x => DateTime.Now >= x.DateTimeSave
            //                                                 && x.Id == 1);

            var sqlBuilder02 = fluentSqlBuilderService.From<OrderDataModel>()
                                                      .Where(x => x.DateTimeSave.AddHours(10) >= DateTime.Now);

            var selectToUse02 = sqlBuilder02.ToString();
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}