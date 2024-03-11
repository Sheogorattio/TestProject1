using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.EfContext
{
    public class Product
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }
        public DateTime? DeleteDt { get; set; }


        public IEnumerable<Sale> Sales { get; set;}
    }
}
