using FluentSqlBuilder.DataModel;
using FluentSqlBuilder.Playground;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Types;
using System;
using System.Collections.Generic;

namespace SqlBuilderFluent.Playground
{
    public class Program
    {
        private static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

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


                    var select = string.Empty;

                    switch (numberMenuParse)
                    {
                        case 1:
                            select = SelectSimple01();
                            break;
                        case 2:
                            select = SelectWithInnerJoin02();
                            break;
                        case 3:
                            select = SelectWithLeftJoin03();
                            break;
                        case 4:
                            select = SelectWithRightJoin04();
                            break;
                        case 5:
                            select = SelectWithWhere05();
                            break;
                        case 6:
                            select = SelectWithWhereAndInnerJoin06_01();
                            break;
                        case 7:
                            select = SelectWithWhereAndInnerJoin07_02();
                            break;
                        case 8:
                            select = SelectWithOrderBy08();
                            break;
                        case 9:
                            select = SelectWithOrderByDescendingBy09();
                            break;
                        case 10:
                            select = SelectWithGroupBy10();
                            break;
                        case 11:
                            select = SelectWithGroupByAndHaving11();
                            break;
                        case 12:
                            select = SelectWithProjection12();
                            break;
                        case 13:
                            select = SelectWithLimit13();
                            break;
                        case 14:
                            select = SelectWithPagination14();
                            break;
                        case 15:
                            select = SelectWithAlias15();
                            break;
                        case 16:
                            select = SelectWithFull16();
                            break;
                        case 17:
                            select = SelectWithDistinct17();
                            break;
                        default:
                            throw new ArgumentException("Number invalid");
                    }

                    Console.WriteLine("\n\n----------- START -----------\n\n ");
                    Console.WriteLine(DefineColorInSelect(select));
                    Console.WriteLine("\n\n----------- END -----------");
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

        private static string SelectSimple01()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithInnerJoin02()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithLeftJoin03()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .LeftJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithRightJoin04()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .RightJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithWhere05()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithWhereAndInnerJoin06_01()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                 .Where<CustomerDataModel>(x => x.Type == CustomerType.B2B)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithWhereAndInnerJoin07_02()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id)
                                 .Where(x => x.Type == CustomerType.B2B);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithOrderBy08()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .OrderBy(x => x.CustomerId);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithOrderByDescendingBy09()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .OrderByDescending(x => x.CustomerId);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithGroupBy10()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Min(x => x.CustomerId)
                                 .GroupBy(x => x.CustomerId);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithGroupByAndHaving11()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Min(x => x.CustomerId)
                                 .GroupBy(x => x.CustomerId)
                                 .Having(SelectFunction.Min, x => x.CustomerId > 1);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithProjection12()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Projection(x => new { x.Id, x.Status });

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithLimit13()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Limit(10);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithPagination14()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .OrderBy(x => x.Id)
                                 .Pagination(10, 1);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithAlias15()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault, "order_alias");

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithFull16()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault, "order_alias")
                                   .Projection((orderDataModel) => new { orderDataModel.CustomerId }, "order_alias")
                                   .Projection<CustomerDataModel>((customer) => customer, "customer_alias")
                                   .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                   .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, "customer_alias")
                                   .OrderBy(x => x.Id)
                                   .Limit(10);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }

        private static string SelectWithDistinct17()
        {
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Distinct(x => x.CustomerId);

            var parameters = sqlBuilder.GetParameters();
            var sqlSelect = sqlBuilder.ToString();

            return sqlSelect;
        }
    }
}