using System.ComponentModel.DataAnnotations.Schema;

namespace FluentSqlBuilder.Data.DataModel
{
    [Table("Customer", Schema = "Customers")]
    public class CustomerDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CustomerType Type { get; set; }
    }
}