using FluentSqlBuilder.Core.Middlewares.Services.Interfaces;
using FluentSqlBuilder.Data.DataModel;
using FluentSqlBuilder.Playground.Shared.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace FluentSqlBuilder.Playground.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(IFluentSqlBuilderService fluentSqlBuilderService)
        {
            var sqlBuilder = fluentSqlBuilderService.From<OrderDataModel>()
                                                    .LeftJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            var sqlSelect = sqlBuilder.ToString();
        }

        public IActionResult OnGet()
        {
            var selects = new List<A>()
            {
                new A() {
                    SqlSelect = FluentSqlBuilderRepository.SelectSimple01().ToString()
                }
            };

            base.ViewData.Add("Selects", selects);

            return Page();
        }
    }

    public class A
    {
        public string SqlSelect { get; set; }
    }
}