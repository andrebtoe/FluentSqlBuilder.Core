using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluentSqlBuilder.Data.DataModel
{
    [Table("Order_Item", Schema = "Checkout")]
    public class OrderItemDataModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("Product_id")]
        public int ProductId { get; set; }
        public DateTime DateTime { get; set; }
    }
}