using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.EfContext;

namespace TestProject1.Models
{
    public class ManagerModel
    {
        public Guid Id { get; private set; }
        public String Surname { get; set; }
        public String Name { get; set; }
        public String Secname { get; set; }
        public IdName MainDep { get; set; }
        public IdName SecDep { get; set; }
        public IdName Chief { get; set; }
        public List<IdName> Departments { get; set; }
        public List<IdName> Chiefs { get; set; }

        public ManagerModel(Manager entity) 
        {
            Id = entity.Id;
            Surname = entity.Surname;
            Name = entity.Name;
            Secname = entity.Secname;

            MainDep = new IdName
            {
                Id = entity.MainDepartment.Id,
                Name = entity.MainDepartment.Name
            };

            SecDep = entity.SecondaryDepartment == null ? null
                : new IdName
                {
                    Id = entity.SecondaryDepartment.Id,
                    Name = entity.SecondaryDepartment.Name
                };
            Chief = entity.Chief == null ? null :
                new IdName
                {
                    Id = entity.Chief.Id,
                    Name = $"{entity.Chief.Surname} {entity.Chief.Name[0]}. {entity.Chief.Secname[0]}"
                };
        }
    }
}
