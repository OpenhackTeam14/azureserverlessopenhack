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

        public string quantity
        {
            get;
            set;
        }

        public string unitcost
        {
            get;
            set;
        }

        public string totalcost
        {
            get;
            set;
        }

        public string totaltax
        {
            get;
            set;
        }
    }
}
