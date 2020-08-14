using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class CampgroundDAO : ICampgroundDAO
    {
        private string ConnectionString;

        
        public CampgroundDAO(string dbConnectionString)
        {
            ConnectionString = dbConnectionString;
        }
        public IList<Campground> GetCampgrounds(Park park)
        {
            IList<Campground> output = new List<Campground>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                   
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    string sqlStatement = "select * from campground where park_id = @park_id order by campground_id;";
                    sqlCommand.CommandText = sqlStatement;
                    sqlCommand.Parameters.AddWithValue("@park_id", park.ID);
                    sqlCommand.Connection = connection;
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.ID = Convert.ToInt32(reader["campground_id"]);
                        campground.Park_ID = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.Open_Month = Convert.ToInt32(reader["open_from_mm"]);
                        campground.Close_Month = Convert.ToInt32(reader["open_to_mm"]);
                        campground.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);

                        output.Add(campground);
                    }
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
            return output;
        }
    }
}