using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Data.Model
{
    public class SalespersonDistrict
    {
        public Guid SalespersonId { get; set; }
        public Guid DistrictId { get; set; }
        public bool IsPrimary { get; set; }

        public Salesperson Salesperson { get; set; }
        public District District { get; set; }
    }
}
