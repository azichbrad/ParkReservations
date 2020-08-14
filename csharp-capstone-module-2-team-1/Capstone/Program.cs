using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("npcampground");

            ICampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);
            IParkDAO parkDAO = new ParkDAO(connectionString);
            IReservationDAO reservationDAO = new ReservationDAO(connectionString);
            ISiteDAO siteDAO = new SiteDAO(connectionString);

            CapstoneCLI capstoneCLI = new CapstoneCLI(campgroundDAO, parkDAO, reservationDAO, siteDAO);
            capstoneCLI.RunCLI();


            
        }
    }
}
