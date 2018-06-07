using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingsApi
{
    public class OrderHeaderDetail
    {
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
