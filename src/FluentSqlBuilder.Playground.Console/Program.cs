using Dapper;
using FluentSqlBuilder.Data.DataModel;
using FluentSqlBuilder.Playground;
using FluentSqlBuilder.Playground.Shared.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.Json;

namespace SqlBuilderFluent.Playground
{
    public class Program
    {
        private readonly static bool _executeInBD = true;

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
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectSimple01();
                            break;
                        case 2:
                            fluentSqlBuilderCustomer = FluentSqlBuilderRepository.SelectWithInnerJoin02();
                            break;
                        case 3:
                            fluentSqlBuilderCustomer = FluentSqlBuilderRepository.SelectWithLeftJoin03();
                            break;
                        case 4:
                            fluentSqlBuilderCustomer = FluentSqlBuilderRepository.SelectWithRightJoin04();
                            break;
                        case 5:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithWhere05();
                            break;
                        case 6:
                            fluentSqlBuilderCustomer = FluentSqlBuilderRepository.SelectWithWhereAndInnerJoin06_01();
                            break;
                        case 7:
                            fluentSqlBuilderCustomer = FluentSqlBuilderRepository.SelectWithWhereAndInnerJoin07_02();
                            break;
                        case 8:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithOrderBy08();
                            break;
                        case 9:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithOrderByDescendingBy09();
                            break;
                        case 10:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithGroupBy10();
                            break;
                        case 11:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithGroupByAndHaving11();
                            break;
                        case 12:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithProjection12();
                            break;
                        case 13:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithLimit13();
                            break;
                        case 14:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithPagination14();
                            break;
                        case 15:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithAlias15();
                            break;
                        case 16:
                            fluentSqlBuilderCustomer = FluentSqlBuilderRepository.SelectWithFull16();
                            break;
                        case 17:
                            fluentSqlBuilderOrder = FluentSqlBuilderRepository.SelectWithDistinct17();
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
    }
}