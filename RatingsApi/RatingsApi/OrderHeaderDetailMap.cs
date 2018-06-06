using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace RatingsApi
{
    public class OrderHeaderDetailMap : ClassMap<OrderHeaderDetail>
    {
        public OrderHeaderDetailMap()
        {
            Map(m => m.ponumber).Name("ponumber");
            Map(m => m.datetime).Name("datetime");
            Map(m => m.locationaddress).Name("locationaddress");
            Map(m => m.locationid).Name("locationid");
            Map(m => m.locationname).Name("locationname");
            Map(m => m.locationpostcode).Name("locationpostcode");
            Map(m => m.totalcost).Name("totalcost");
            Map(m => m.totaltax).Name("totaltax");
        }
    }
}
