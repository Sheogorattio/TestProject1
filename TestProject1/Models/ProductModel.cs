using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.EfContext;

namespace TestProject1.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public String? Name { get; set; }
        public double Price { get; set; }

        public static ProductModel FromEntity(Product entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
        };
    }
}
