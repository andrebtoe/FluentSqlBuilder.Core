using System.ComponentModel.DataAnnotations.Schema;

namespace FluentSqlBuilder.Data.DataModel
{
    [Table("order", Schema = "checkout")]
    public class OrderDataModel
    {
        public int Id { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        public OrderStatus Status { get; set; }
    }
}