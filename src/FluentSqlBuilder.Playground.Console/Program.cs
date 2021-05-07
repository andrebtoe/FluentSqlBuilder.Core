using Dapper;
using FluentSqlBuilder.Core.Middlewares.Inputs;
using FluentSqlBuilder.Core.Middlewares.Services;
using FluentSqlBuilder.Data.DataModel;
using FluentSqlBuilder.Playground;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.Json;

namespace SqlBuilderFluent.Playground
{
    public class Program
    {
        private readonly static bool _executeInBD = true;
        private readonly static FluentSqlBuilderMiddlewareOptions _fluentSqlBuilderMiddlewareOptions = new FluentSqlBuilderMiddlewareOptions()
        {
            SqlAdapterType = SqlAdapterType.SqlServer2019,
            Formatting = SqlBuilderFormatting.Indented
        };

        static void Main()
        {
            PrintMenu();

            while (true)
            {
                Console.Write("\n\nCHOOSE AN OPTION: ");

                var numberMenu = Console.ReadLine();
                var numberMenuParsed = int.TryParse(numberMenu, out int numberMenuParse);

                if (numberMenuParsed)
                {
                    Console.Clear();

                    PrintMenu(numberMenuParse);

                    FluentSqlBuilder<OrderDataModel> fluentSqlBuilderOrder = null;
                    FluentSqlBuilder<CustomerDataModel> fluentSqlBuilderCustomer = null;

                    switch (numberMenuParse)
                    {
                        case 1:
                            fluentSqlBuilderOrder = SelectSimple01();
                            break;
                        case 2:
                            fluentSqlBuilderCustomer = SelectWithInnerJoin02();
                            break;
                        case 3:
                            fluentSqlBuilderCustomer = SelectWithLeftJoin03();
                            break;
                        case 4:
                            fluentSqlBuilderCustomer = SelectWithRightJoin04();
                            break;
                        case 5:
                            fluentSqlBuilderOrder = SelectWithWhere05();
                            break;
                        case 6:
                            fluentSqlBuilderCustomer = SelectWithWhereAndInnerJoin06_01();
                            break;
                        case 7:
                            fluentSqlBuilderCustomer = SelectWithWhereAndInnerJoin07_02();
                            break;
                        case 8:
                            fluentSqlBuilderOrder = SelectWithOrderBy08();
                            break;
                        case 9:
                            fluentSqlBuilderOrder = SelectWithOrderByDescendingBy09();
                            break;
                        case 10:
                            fluentSqlBuilderOrder = SelectWithGroupBy10();
                            break;
                        case 11:
                            fluentSqlBuilderOrder = SelectWithGroupByAndHaving11();
                            break;
                        case 12:
                            fluentSqlBuilderOrder = SelectWithProjection12();
                            break;
                        case 13:
                            fluentSqlBuilderOrder = SelectWithLimit13();
                            break;
                        case 14:
                            fluentSqlBuilderOrder = SelectWithPagination14();
                            break;
                        case 15:
                            fluentSqlBuilderOrder = SelectWithAlias15();
                            break;
                        case 16:
                            fluentSqlBuilderCustomer = SelectWithFull16();
                            break;
                        case 17:
                            fluentSqlBuilderOrder = SelectWithDistinct17();
                            break;
                        default:
                            throw new ArgumentException("Number invalid");
                    }

                    string sqlSelect = null;
                    object data = null;

                    if (fluentSqlBuilderOrder != null)
                    {
                        sqlSelect = fluentSqlBuilderOrder.ToString();
                        var parameters = fluentSqlBuilderOrder.GetParameters();
                        data = ExecuteQuery<OrderDataModel>(sqlSelect, parameters);
                    }
                    else if (fluentSqlBuilderCustomer != null)
                    {
                        sqlSelect = fluentSqlBuilderCustomer.ToString();
                        var parameters = fluentSqlBuilderCustomer.GetParameters();
                        data = ExecuteQuery<CustomerDataModel>(sqlSelect, parameters);
                    }

                    Console.WriteLine("\n\n----------- SQL -----------\n\n ");
                    Console.WriteLine(DefineColorInSelect(sqlSelect));
                    Console.WriteLine();

                    if (_executeInBD)
                    {
                        var optionsJson = new JsonSerializerOptions()
                        {
                            WriteIndented = true
                        };
                        var dataJson = JsonSerializer.Serialize(data, optionsJson);

                        Console.WriteLine("\n\n----------- JSON -----------\n\n ");
                        Console.WriteLine(dataJson);
                    }
                }
                else
                {
                    throw new ArgumentException("Number invalid");
                }
            }
        }

        private static string DefineColorInSelect(string select)
        {
            var wordsToColorBlue = new List<string>(){
                "SELECT",
                "AS ",
                "FROM",
                "HAVING",
                "SUM",
                "AND ",
                "OR ",
                "ON ",
                "OFFSET",
                "ROWS",
                "FETCH",
                "NEXT",
                "ROWS",
                "ONLY",
                "WHERE"
            };

            foreach (var item in wordsToColorBlue)
                select = select.Replace(item, item.Blue().Bold());

            return select;
        }

        private static void PrintMenu(int? itemMenu = null)
        {
            Func<string, bool?, bool> printItem = (string text, bool? showWithColor) =>
            {
                if (showWithColor.GetValueOrDefault())
                    Console.WriteLine(text.Cyan());
                else
                    Console.WriteLine(text);

                return true;
            };

            var menuItemSelected = itemMenu.GetValueOrDefault();

            printItem("01 - Select simple", menuItemSelected == 1);
            printItem("02 - Select with INNER JOIN", menuItemSelected == 2);
            printItem("03 - Select with LEFT JOIN", menuItemSelected == 3);
            printItem("04 - Select with RIGHT JOIN", menuItemSelected == 4);
            printItem("05 - Select with WHERE simple", menuItemSelected == 5);
            printItem("06 - Select with WHERE simple and INNER JOIN -  01", menuItemSelected == 6);
            printItem("07 - Select with WHERE simple and INNER JOIN -  02", menuItemSelected == 7);
            printItem("08 - Select with ORDER BY ASC", menuItemSelected == 8);
            printItem("09 - Select with ORDER BY DESC", menuItemSelected == 9);
            printItem("10 - Select with MIN AND GROUP BY", menuItemSelected == 10);
            printItem("11 - Select with GROUP BY and HAVING", menuItemSelected == 11);
            printItem("12 - Select with Projection", menuItemSelected == 12);
            printItem("13 - Select with Limit", menuItemSelected == 13);
            printItem("14 - Select with Pagination", menuItemSelected == 14);
            printItem("15 - Select with alias", menuItemSelected == 15);
            printItem("16 - Select with Projection, WHERE, INNER JOIN ORDER BY and Lmit", menuItemSelected == 16);
            printItem("17 - Select with DISTINT", menuItemSelected == 17);
        }

        private static IEnumerable<TTable> ExecuteQuery<TTable>(string sqlSelect, IDictionary<string, object> parameters)
        {
            if (!_executeInBD)
                return null;

            IEnumerable<TTable> data;

            using (var connection = new SqlConnection("Initial Catalog=FluentSqlBuilder;User Id=sa;Password=b6WTRgh6;Data source=127.0.0.1,1434"))
            {
                connection.Open();

                data = connection.Query<TTable>(sqlSelect, parameters);

                connection.Close();
            }

            return data;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectSimple01()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions).From<OrderDataModel>();

            return sqlBuilder;
        }

        private static FluentSqlBuilder<CustomerDataModel> SelectWithInnerJoin02()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<CustomerDataModel> SelectWithLeftJoin03()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .LeftJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<CustomerDataModel> SelectWithRightJoin04()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .RightJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithWhere05()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<CustomerDataModel> SelectWithWhereAndInnerJoin06_01()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                 .Where<CustomerDataModel>(x => x.Type == CustomerType.B2B)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<CustomerDataModel> SelectWithWhereAndInnerJoin07_02()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id)
                                 .Where(x => x.Type == CustomerType.B2B);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithOrderBy08()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .OrderBy(x => x.CustomerId);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithOrderByDescendingBy09()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .OrderByDescending(x => x.CustomerId);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithGroupBy10()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Min(x => x.CustomerId)
                                 .GroupBy(x => x.CustomerId);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithGroupByAndHaving11()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Min(x => x.CustomerId)
                                 .GroupBy(x => x.CustomerId)
                                 .Having(SelectFunction.Min, x => x.CustomerId > 1);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithProjection12()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Projection(x => new { x.Id, x.Status });

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithLimit13()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Limit(10);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithPagination14()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .OrderBy(x => x.Id)
                                 .Pagination(10, 1);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithAlias15()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>("order_alias");

            return sqlBuilder;
        }

        private static FluentSqlBuilder<CustomerDataModel> SelectWithFull16()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                   .From<OrderDataModel>("order_alias")
                                   .Projection((orderDataModel) => new { orderDataModel.CustomerId }, "order_alias")
                                   .Projection<CustomerDataModel>((customer) => customer, "customer_alias")
                                   .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                   .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, "customer_alias")
                                   .OrderBy(x => x.Id)
                                   .Limit(10);

            return sqlBuilder;
        }

        private static FluentSqlBuilder<OrderDataModel> SelectWithDistinct17()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Distinct(x => x.CustomerId);

            return sqlBuilder;
        }
    }
}