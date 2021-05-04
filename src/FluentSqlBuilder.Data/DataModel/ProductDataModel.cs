using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluentSqlBuilder.Data.DataModel
{
    [Table("Product", Schema = "Products")]
    public class ProductDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }
}