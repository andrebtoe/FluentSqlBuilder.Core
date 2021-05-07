using FluentSqlBuilder.Core.Middlewares.Services.Interfaces;
using FluentSqlBuilder.Data.DataModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public void OnGet()
        {

        }
    }
}