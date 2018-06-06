using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RatingsApi
{
    public class OrderLineItem
    {
        public string ponumber
        {
            get;
            set;
        }

        public Guid productid
        {
            get;
            set;
        }

        public int quantity
        {
            get;
            set;
        }

        public double unitcost
        {
            get;
            set;
        }

        public double totalcost
        {
            get;
            set;
        }

        public double totaltax
        {
            get;
            set;
        }
    }
}
