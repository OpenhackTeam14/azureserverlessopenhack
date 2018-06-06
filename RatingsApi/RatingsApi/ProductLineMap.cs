using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace RatingsApi
{
    public class ProductLineMap :ClassMap<ProductLine>
    {
        public ProductLineMap()
        {
            Map(m => m.productid).Name("productid");
            Map(m => m.productname).Name("productname");
            Map(m => m.productdescription).Name("productdescription");
        }
    }
}
