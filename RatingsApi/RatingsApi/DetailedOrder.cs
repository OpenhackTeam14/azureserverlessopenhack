using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingsApi
{
    public class DetailedOrder
    {
        public DetailedOrder(ProductLine productline, OrderHeaderDetail detailline, OrderLineItem orderline)
        {
            productid = productline.productid;
            productname = productline.productname;
            productdescription = productline.productdescription;
            ponumber = detailline.ponumber;
            datetime = detailline.datetime;
            locationaddress = detailline.locationaddress;
            locationid = detailline.locationid;
            locationname = detailline.locationname;
            locationpostcode = detailline.locationpostcode;
            totalcost = detailline.totalcost;
            totaltax = detailline.totaltax;
            quantity = orderline.quantity;
        }

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
        public string productname
        {
            get;
            set;
        }

        public string productdescription
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
