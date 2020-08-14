using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Capstone
{
    public class CapstoneCLI
    {
        const string Command_ParkMenu = "1";
        const string Command_About = "2";
        const string Command_Quit = "q";
        private ICampgroundDAO campgroundDAO;
        private IParkDAO parkDAO;
        private IReservationDAO reservationDAO;
        private ISiteDAO siteDAO;
        

        public CapstoneCLI(ICampgroundDAO campgroundDAO, IParkDAO parkDAO, IReservationDAO reservationDAO, ISiteDAO siteDAO)
        {
            this.campgroundDAO = campgroundDAO;
            this.parkDAO = parkDAO;
            this.reservationDAO = reservationDAO;
            this.siteDAO = siteDAO;
        }

        public void RunCLI()
        {
            Console.SetWindowSize(150, 40);
           
            while (true)
            {

                DisplayHeader();
                PrintMainMenu();
                string userInput = CLIHelper.GetString("Please enter a command: ");
                switch (userInput.ToLower())
                {
                    case Command_ParkMenu:
                        Console.Clear();
                        ParkMenu();
                        break;
                    case Command_About:
                        Console.Clear();
                        AboutNPS();
                        break;
                    case Command_Quit:
                        Console.WriteLine("Thank you! Have a nice day!");
                        return;
                    default:
                        Console.WriteLine("That's not a valid choice, please try again...");
                        break;
                }

            }


        }
        

        public void DisplayHeader()
        {
            Console.WriteLine(@"    _   __      __  _                   __   ____             __      _____                 _         ");
            Console.WriteLine(@"   / | / /___ _/ /_(_)___  ____  ____ _/ /  / __ \____ ______/ /__   / ___/___  ______   __(_)_______ ");
            Console.WriteLine(@"  /  |/ / __ `/ __/ / __ \/ __ \/ __ `/ /  / /_/ / __ `/ ___/ //_/   \__ \/ _ \/ ___/ | / / / ___/ _ \");
            Console.WriteLine(@" / /|  / /_/ / /_/ / /_/ / / / / /_/ / /  / ____/ /_/ / /  / ,<     ___/ /  __/ /   | |/ / / /__/  __/");
            Console.WriteLine(@"/_/ |_/\__,_/\__/_/\____/_/ /_/\__,_/_/  /_/    \__,_/_/  /_/|_|   /____/\___/_/    |___/_/\___/\___/ ");
            Console.WriteLine();
            Console.WriteLine();
        }

        public void PrintMainMenu()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine(" 1 - Parks Menu");
            Console.WriteLine(" 2 - About");
            Console.WriteLine(" Q - Quit");
        }
        public void PrintParkMenu()
        {
            Console.WriteLine("Park Menu");
            Console.WriteLine(" 1 - View Campgrounds");
            Console.WriteLine(" 2 - Search for Reservation");
            Console.WriteLine(" 3 - Advanced Menu");
            Console.WriteLine(" 4 - Return to Previous Screen");
        }

        
        public void ParkMenu()
        {
            const int Command_ViewCampgrounds = 1;
            const int Command_SearchReservation = 2;
            const int Command_AdvancedMenu = 3;
            const int Command_ReturnToPrevious = 4;
            bool isValidInput = false;
            Console.WriteLine("National Parks\n");
            IList<Park> parks = parkDAO.GetAllParks();
            foreach (Park park in parks)
            {
                Console.WriteLine($" {park.ID} - {park.Name}");
            }
            int choice = 0;
            while(!isValidInput)
            {
                choice = CLIHelper.GetInteger("Please choose a park: ");
                isValidInput = VerifyIntChoice(choice, parks.Count);
                if(!isValidInput)
                {
                    Console.WriteLine("That's not a valid choice, please try again...");
                }
            }
            
            Park chosenPark = parks.Where(park => park.ID == choice).FirstOrDefault();

            Console.Clear();
            while (true)
            {
                Console.WriteLine(chosenPark);
                PrintParkMenu();
                isValidInput = false;
                int userInput = 0;
                while (!isValidInput)
                {
                    userInput = CLIHelper.GetInteger("Please enter a command: ");
                    isValidInput = VerifyIntChoice(userInput, 4);
                    if(!isValidInput)
                    {
                        Console.WriteLine("That's not a valid command, please try again...");
                    }
                }
                switch(userInput)
                {
                    case Command_ViewCampgrounds:
                        Console.Clear();
                        ViewCampgrounds(chosenPark);
                        break;
                    case Command_SearchReservation:
                        Console.Clear();
                        SearchAvailableSitesForReservations(chosenPark);
                        break;
                    case Command_AdvancedMenu:
                        Console.Clear();
                        AdvancedMenu(chosenPark);
                        break;
                    case Command_ReturnToPrevious:
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("That's not a valid choice...");
                        break;
                }
            }

        }
        public void PrintAdvanceMenu()
        {
            Console.WriteLine($"Advanced Menu");
            Console.WriteLine(" 1 - View Next 30 Days of Reservations");
            Console.WriteLine(" 2 - View Campsite Availability");
            Console.WriteLine(" 3 - Advanced Search for Reservation");
            Console.WriteLine(" 4 - Return to Previous Screen");
        }
        public void AdvancedMenu(Park chosenPark)
        {
            const int Command_View30DaysReservations = 1;
            const int Command_ViewAllCampsiteAvailAcrossPark = 2;
            const int Command_AdvancedSearchMenu = 3;
            const int Command_ReturnToPrevious = 4;
            int userInput = 0;
            bool isValidChoice = false;
            PrintAdvanceMenu();
            while(!isValidChoice)
            {
                userInput = CLIHelper.GetInteger("Please enter a command: ");
                isValidChoice = VerifyIntChoice(userInput, 4);
                if(!isValidChoice)
                {
                    Console.WriteLine("That's not a valid choice, please try again...");
                }
            }
            switch(userInput)
            {
                case Command_View30DaysReservations:
                    Console.Clear();
                    Get30DaysReservations(chosenPark);
                    break;
                case Command_ViewAllCampsiteAvailAcrossPark:
                    Console.Clear();
                    SearchParkForReservations(chosenPark);
                    break;
                case Command_AdvancedSearchMenu:
                    Console.Clear();
                    Console.WriteLine("\n\nSorry, under construction! Taking you back...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;
                case Command_ReturnToPrevious:
                    return;
            }
        }

        public void Get30DaysReservations(Park chosenPark)
        {
            IDictionary<Reservation,string> reservations = reservationDAO.Get30DaysReservations(DateTime.Now, chosenPark.ID);
            if (!(reservations is null))
            {
                Console.WriteLine("|{0,-8}|{1,-8}|{2,-25}|{3,-40}|{4,-15}|{5,-15}|{6,-15}|", "Res. ID.", "Site No.","Camground","Res. Name","Start Date","End Date","Date Created");
                foreach(KeyValuePair<Reservation, string> kvp in reservations)
                {
                    Console.WriteLine($"|{kvp.Key.ID,8}|{kvp.Key.SiteID,8}|{kvp.Value,-25}|{kvp.Key.Name,-40}|{kvp.Key.FromDate.ToShortDateString(),-15}|{kvp.Key.ToDate.ToShortDateString(),15}|{kvp.Key.DateCreated.ToShortDateString(),15}|");
                }
            }
            else
            {
                Console.WriteLine("\n\nThere are no reservations...\n\n");
            }
        }
       
        public void ViewCampgrounds(Park chosenPark)
        {
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(chosenPark);
            PrintCampgrounds(campgrounds);

        }

        public void PrintCampgrounds(IList<Campground> campgrounds)
        {
            Console.WriteLine("|{0,-3}|{1,-25}|{2,-15}|{3,-15}|{4,-10}|", "ID", "Name", "Open", "Close", "Daily Fee");
            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine(campground);
            }
        }

        public void SearchAvailableSitesForReservations(Park chosenPark)
        {
            bool moveForward = true;
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(chosenPark);
            PrintCampgrounds(campgrounds);
            int choice = 0;
            bool isValidIntInput = false;
            //Gets user choice of campgroundID and verifies if it's a valid choice
            while(!isValidIntInput)
            {
                choice = CLIHelper.GetInteger("Please choose a campground: ");
                isValidIntInput = VerifyIntChoice(choice, campgrounds.Count);
                if(!isValidIntInput)
                {
                    Console.WriteLine("That's not a valid choice, please try again...");
                }
            }

            DateTime arrival = DateTime.MinValue;
            DateTime departure = DateTime.MaxValue;
            Campground chosenCampground = campgrounds.Where(camp => camp.ID == choice).FirstOrDefault();
            while (moveForward)
            {
                bool IsValidDates = false;
                while (!IsValidDates)
                {

                    arrival = CLIHelper.GetDateTime("When will you be arriving?: ");
                    departure = CLIHelper.GetDateTime("When will you be departing?: ");
                    IsValidDates = VerifyDates(arrival, departure);
                    if (!IsValidDates)
                    {
                        Console.WriteLine("Your departure date must be after your arrival date, try again...");
                    }
                }

                PrintSiteTableHeader();
                IList<Site> sites = siteDAO.GetTop5AvailableSites(chosenCampground, arrival, departure);
                if (sites.Count > 0)
                {
                    int selectedSiteNumber = -1;
                    foreach (Site site in sites)
                    {
                        decimal totalCost = CalculateTotalCostofStay(arrival,departure, chosenCampground.Daily_Fee);
                        Console.WriteLine(site + $"{totalCost,15:C2}|");
                    }
                    while (sites.Where(s => s.SiteNumber == selectedSiteNumber).FirstOrDefault() is null && selectedSiteNumber != 0)
                    {
                        selectedSiteNumber = CLIHelper.GetInteger("Which site would you like to reserve? (enter 0 to cancel)");
                        if (sites.Where(s => s.SiteNumber == selectedSiteNumber).FirstOrDefault() is null && selectedSiteNumber !=0)
                        {
                            Console.WriteLine("That's not a valid campsite, please try again...");
                        }
                    }
                    switch (selectedSiteNumber)
                    {
                        case 0:
                            return;
                        default:
                            MakeReservation(sites.Where(s => s.SiteNumber == selectedSiteNumber).FirstOrDefault().ID, arrival, departure);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("There are no available sites for that date range...");
                    moveForward = CLIHelper.GetBool("Would you like to enter a new date range? (yes/no): ");

                }
            }
        }
        public void SearchParkForReservations(Park chosenPark)
        {
            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(chosenPark);
            IList<Site> allSites = new List<Site>();
            DateTime arrival = DateTime.MinValue;
            DateTime departure = DateTime.MaxValue;
            bool isValidDates = false;
            while (!isValidDates)
            {
                arrival = CLIHelper.GetDateTime("When will you be arriving?: ");
                departure = CLIHelper.GetDateTime("When will you be departing?: ");
                isValidDates = VerifyDates(arrival, departure);
                if (!isValidDates)
                {
                    Console.WriteLine("Your departure date must be after your arrival date, try again...");
                }
            }
            
            foreach (Campground campground in campgrounds)
            {
                IList<Site> sites = siteDAO.GetTop5AvailableSites(campground, arrival, departure);
                decimal totalCost = CalculateTotalCostofStay(arrival, departure, campground.Daily_Fee);
                Console.WriteLine($"{campground.ID} - {campground.Name}");
                PrintSiteTableHeader();
                foreach (Site site in sites)
                {

                    allSites.Add(site);
                    Console.WriteLine(site + $"{totalCost,15:C2}|");
                }

            }
            bool isMakingReservation = CLIHelper.GetBool("Reserve a site?(yes/no)");
            if(isMakingReservation)
            {
                int userCampground = -1;
                int userSite = -1;

                while (allSites.Where(c => c.CampgroundID == userCampground && c.SiteNumber == userSite).FirstOrDefault() is null && userCampground != 0)
                {
                    userCampground = CLIHelper.GetInteger("Which campground?: ");
                    userSite = CLIHelper.GetInteger("Which site would you like to reserve? (enter 0 to cancel)");
                    if (allSites.Where(c => c.SiteNumber == userSite).FirstOrDefault() is null && userCampground != 0 && userSite != 0)
                    {
                        Console.WriteLine("That's not a valid campsite, please try again...");
                    }
                }
                switch (userCampground)
                {
                    case 0:
                        return;
                    default:
                        MakeReservation(allSites.Where(s => s.SiteNumber == userSite).FirstOrDefault().ID, arrival, departure);
                        break;
                }
            }
            
        }
        public void MakeReservation(int site_id,  DateTime arrival, DateTime departure)
        {
            string reservationName = CLIHelper.GetString("What name should the reservation be made under?: ");
            Reservation newReservation = new Reservation
            {
                SiteID = site_id,
                Name = reservationName,
                FromDate = arrival,
                ToDate = departure
            };
            int newReservationID = reservationDAO.AddReservation(newReservation);
            Console.WriteLine($"The reservation has been made. Confirmation ID: {newReservationID}");
        }

        public bool VerifyDates(DateTime arrival, DateTime departure)
        {
            bool result = false;
            int datecomparison = arrival.CompareTo(departure);
            if(datecomparison == -1)
            {
                result = true;
            }
            return result;
        }

        public bool VerifyIntChoice(int choice, int NumOfPossibleChoice)
        {
            bool result = false;
            if(choice <= NumOfPossibleChoice && choice > 0)
            {
                result = true;
            }
            return result;
        }
        public decimal CalculateTotalCostofStay(DateTime arrival, DateTime departure, decimal dailyfee)
        {
            return ((decimal)departure.Subtract(arrival).TotalDays + 1) * dailyfee;

        }
        public void AboutNPS()
        {
            Random r = new Random();
            string message = "National Park Reservation Management Software (C)2020\n\nCo-Authored by: Brad Azich, Bradley Babiack, and Mike Millen.";
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(r.Next(4,69));
            }
            Console.WriteLine("");
            Console.WriteLine("\nPress ENTER to return to the main menu...");
            Console.ReadLine();
            Console.Clear();
            
        }

        public void PrintSiteTableHeader()
        {
            Console.WriteLine("|{0,-8}|{1,-12}|{2,-12}|{3,-15}|{4,-10}|{5,-15}|", "Site No.", "Max Occup.", "Accessible?", "Max RV Length", "Utility?", "Cost");
        }

    }
}
