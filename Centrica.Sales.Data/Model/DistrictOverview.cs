using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Data.Model
{
    public class DistrictOverview
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Salesperson> Salespersons { get; set; }
        public List<Store> Stores { get; set; }
    }
}
