using System.ComponentModel.DataAnnotations.Schema;

namespace FluentSqlBuilder.DataModel
{
    [Table("customer", Schema = "customers")]
    public class CustomerDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CustomerType Type { get; set; }
    }
}