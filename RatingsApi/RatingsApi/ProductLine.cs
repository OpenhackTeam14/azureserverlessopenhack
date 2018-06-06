using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingsApi
{
    public class ProductLine
    {
        public Guid productid
        {
            get;
            set;
        }

        public string productname
        {
            get;
            set;
        }

        public string productdescription { get; set; }
    }
}
