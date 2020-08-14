using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishedDate { get; set; }
        public int Area { get; set; }
        public int AnnualVisitorCount { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            string fName = $"{Name} National Park";
            string fLocation = string.Format("\n{0,-15}{1}", "Location:", Location);
            string fEstablished = string.Format("\n{0,-15}{1}", "Established:", EstablishedDate.ToShortDateString());
            string fArea = string.Format("\n{0,-15}{1}km\xB2", "Area:", Area);
            string fAnnualVistorCount = string.Format("\n{0,-15}{1}", "Annual Vistors:", AnnualVisitorCount);
            string fDescription = string.Format($"\n\nAbout:\n{Description}\n");
            return fName + fLocation + fEstablished + fArea + fAnnualVistorCount + fDescription;
        }


    }
}
