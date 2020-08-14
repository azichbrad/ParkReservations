using Capstone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class SiteDAO : ISiteDAO
    {
       private string ConnectionString;

   
        public SiteDAO(string dbConnectionString)
        {
            ConnectionString = dbConnectionString;
        }
        public IList<Site> GetSites()
        {
            IList<Site> output = new List<Site>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                   
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    string sqlStatement = "select * from site;";
                    sqlCommand.CommandText = sqlStatement;
                    sqlCommand.Connection = connection;
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = new Site();

                        site.ID = Convert.ToInt32(reader["site_id"]);
                        site.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);

                        output.Add(site);
                    }
                }
            }
            catch(SqlException se)
            {
                Console.WriteLine(se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return output;
        }
        public IList<Site> GetTop5AvailableSites(Campground selectedCampground, DateTime startDate, DateTime endDate)
        {

            IList<Site> sites = new List<Site>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string sqlCommand = @"select top 5 site.site_id, campground.campground_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, count(reservation.reservation_id) as num_reservations from site 
                        join campground on campground.campground_id = site.campground_id 
                        join reservation on reservation.site_id = site.site_id 
                        where campground.campground_id = @campgroundid 
                        and site.site_id not in (select site_id from reservation where (reservation.from_date >= @fromdate) 
                        and (reservation.to_date <= @enddate))and month(@fromdate) between campground.open_from_mm and campground.open_to_mm 
                        and month(@enddate) between campground.open_from_mm  and campground.open_to_mm 
                        group by site.site_id, campground.campground_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities 
                        order by num_reservations desc;";

                    SqlCommand command = new SqlCommand(sqlCommand, connection);
                    command.Parameters.AddWithValue("@campgroundid", selectedCampground.ID);
                    command.Parameters.AddWithValue("@fromdate", startDate.ToShortDateString());
                    command.Parameters.AddWithValue("@enddate", endDate.ToShortDateString());
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.ID = Convert.ToInt32(reader["site_id"]);
                        site.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);
                        sites.Add(site);
                    }
                }
                catch (SqlException se)
                {
                    Console.WriteLine(se.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return sites;
        }
        public IList<Site> SiteAdvancedSearch()
        {
            throw new NotImplementedException();
        }
    }
}

