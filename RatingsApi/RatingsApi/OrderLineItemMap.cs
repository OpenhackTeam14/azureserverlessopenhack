using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace RatingsApi
{
    public class OrderLineItemMap: ClassMap<OrderLineItem>
    {
        public OrderLineItemMap()
        {
            Map(m => m.productid).Name("productid");
            Map(m => m.ponumber).Name("ponumber");
            Map(m => m.quantity).Name("quantity");
            Map(m => m.totalcost).Name("totalcost");
            Map(m => m.totaltax).Name("totaltax");
            Map(m => m.unitcost).Name("unitcost");
        }
    }
}
