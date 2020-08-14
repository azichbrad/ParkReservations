using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISiteDAO
    {
        IList<Site> GetSites();
        IList<Site> GetTop5AvailableSites(Campground selectedCampground, DateTime startDate, DateTime endDate);
        IList<Site> SiteAdvancedSearch();
    }
}
