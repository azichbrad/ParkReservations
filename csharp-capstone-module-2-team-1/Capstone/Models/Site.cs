using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        public int ID { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumber{ get; set; }
        public int MaxOccupancy { get; set; }
        public bool IsAccessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool Utilities { get; set; }

        public override string ToString()
        {
            return $"|{SiteNumber,8}|{MaxOccupancy,12}|{(IsAccessible? "Yes" : "No"),12}|{(MaxRVLength>0? MaxRVLength.ToString() : "N/A"),15}|{(Utilities?"Yes" : "No"),10}|";
        }

    }
}
