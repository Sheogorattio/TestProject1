using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.EfContext;

namespace TestProject1.Models
{
    public class DepartmentModel
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string? InternationalName { get; set; }


        public static DepartmentModel FromEntity(Department entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            InternationalName = entity.InternationalName,
        };
    }
}
