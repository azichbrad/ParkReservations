using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int ID { get; set; }
        public int Park_ID { get; set; }
        public string Name { get; set; }
        public int Open_Month { get; set; }
        public int Close_Month { get; set; }
        public decimal Daily_Fee { get; set; }


        public override string ToString()
        {
            string str = $"|{ID,3}|{Name,-25}|{(months)Open_Month - 1,-15}|{(months)Close_Month - 1,-15}|{Daily_Fee,10:C2}|";
            return str;
        }

        enum months
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

    }
}
