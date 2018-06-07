using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingsApi
{
    public class SentimentScores
    {
        public List<Sentiment> documents { get; set; }
        public List<dynamic> errors { get; set; }
    }

    public class Sentiment
    {
        public double score { get; set; }
        public string id { get; set; }
    }

}
