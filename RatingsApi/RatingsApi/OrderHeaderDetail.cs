using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingsApi
{
    public class OrderHeaderDetail
    {
        public OrderHeaderDetail()
        {
            id = Guid.NewGuid();
            lineItems = new List<OrderLineItem>();
        }

        public Guid id
        {
            get;
            set;
        }

        public List<OrderLineItem> lineItems
        {
            get;
            set;
        }

        public string ponumber
        {
            get;
            set;
        }

        public string datetime
        {
            get;
            set;
        }

        public string locationid
        {
            get;
            set;
        }

        public string locationname
        {
            get;
            set;
        }

        public string locationaddress
        {
            get;
            set;
        }

        public string locationpostcode
        {
            get;
            set;
        }
    }
}
