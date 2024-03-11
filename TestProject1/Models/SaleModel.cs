using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.EfContext;

namespace TestProject1.Models
{
    public class SaleModel
    {
        public Guid? Id {get; set;}
        public string Name { get; set;}
        public string ManagerName { get; set;}
        public int Quantity { get; set;}
        public double Price { get; set;}

        public static SaleModel Fromentity(Sale entity)
        {
            return new()
            {
                Id = entity.Id,
                Name = entity.Product.Name,
                ManagerName = entity.Manager.Name,
                Quantity = entity.Quantity,
                Price = entity.Product.Price
            };
        }
    }
}
