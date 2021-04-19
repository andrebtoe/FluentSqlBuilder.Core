using SqlBuilderFluent.Types;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace SqlBuilderFluent.Playground
{
    public class Program
    {
        static void Main()
        {
            var watch = Stopwatch.StartNew();

            var sqlBuilder03 = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, "order_alias")
                                   .Projection((orderDataModel) => new { orderDataModel.CustomerId }, "order_alias")
                                   .OrderBy("order_alias", x => x.CustomerId)
                                   .Pagination(10, 1)
                                   .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1, "order_alias")
                                   .Or(x => x.Status == OrderStatus.Canceled && x.CustomerId == 1, "order_alias")
                                   .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, "customer_alias")
                                   //.LeftJoin<OrderItem>((order, orderItem) => order.OrderId == orderItem.OrderId)
                                   //.RightJoin<OrderItem>((order, orderItem) => order.OrderId == orderItem.OrderId)
                                   //.RightJoin<Order, OrderItem>((order, orderItem) => order.Id == orderItem.OrderId)
                                   //.Where(x => x.Name != "")
                                   .Limit(10);

            var p = sqlBuilder03.GetParameters();
            var sqlSelect = sqlBuilder03.ToString();
            Console.WriteLine(sqlSelect);


            //var sqlBuilder01 = new SqlBuilderFluent<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, "order_alias")
            //                       .Projection((orderDataModel) => new { orderDataModel.CustomerId }, "order_alias")
            //                       .Projection<CustomerDataModel>((customer) => customer, "customer_alias")
            //                       .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
            //                       .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, "customer_alias")
            //                       //.LeftJoin<OrderItem>((order, orderItem) => order.OrderId == orderItem.OrderId)
            //                       //.RightJoin<OrderItem>((order, orderItem) => order.OrderId == orderItem.OrderId)
            //                       //.RightJoin<Order, OrderItem>((order, orderItem) => order.Id == orderItem.OrderId)
            //                       //.Where(x => x.Name != "")
            //                       .OrderBy(x => x.Id)
            //                       .Limit(10);

            //var sqlSelect = sqlBuilder01.ToString();
            //Console.WriteLine(sqlSelect);

            //var sqlBuilder02 = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, "order_alias")
            //                       .Sum(x => x.CustomerId, "order_alias", "Quantity")
            //                       .GroupBy("order_alias", x => x.CustomerId)
            //                       .Having(SelectFunction.SUM, x => x.CustomerId >= 0, "order_alias");

            //var sqlSelect2 = sqlBuilder02.ToString();
            //Console.WriteLine(sqlSelect2);

            watch.Stop();
            var seconds = watch.Elapsed.TotalSeconds;

            Console.WriteLine($"Tempo: {seconds}");

            // netcore3.1 = 0.1713746 segundos
            // netcore5.0 = 0.1362718 segundos

            // SELECT [Order].Id, [Order].Name, [Order].LastName
            // FROM [Order]
            // INNER JOIN [OrderItem] ON ([Order].[Id] = [OrderItem].[OrderId])
            // WHERE [Order].[LastName] != @Param1
            // AND [OrderItem].[Name] != @Param2
        }
    }

    [Table("customer", Schema = "customers")]
    public class CustomerDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CustomerType Type { get; set; }
    }

    public enum CustomerType
    {
        B2C = 1,
        B2B = 2
    }

    public enum OrderStatus
    {
        AwaitingPayment = 1,
        Paid = 2,
        Canceled = 3
    }

    [Table("order", Schema = "checkout")]
    public class OrderDataModel
    {
        public int Id { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        public OrderStatus Status { get; set; }
    }

    [Table("product", Schema = "products")]
    public class ProductDataModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    [Table("order_item", Schema = "checkout")]
    public class OrderItemDataModel
    {
        public int Quantity { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("Product_id")]
        public int ProductId { get; set; }
    }
}