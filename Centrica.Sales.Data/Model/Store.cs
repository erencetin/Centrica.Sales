using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Data.Model
{
    public class Store
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DistrictId { get; set; }
    }
}
