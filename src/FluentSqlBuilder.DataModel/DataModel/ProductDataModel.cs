using System.ComponentModel.DataAnnotations.Schema;

namespace FluentSqlBuilder.Data.DataModel
{
    [Table("product", Schema = "products")]
    public class ProductDataModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}