using System.ComponentModel.DataAnnotations.Schema;

namespace FluentSqlBuilder.DataModel
{
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