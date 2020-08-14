using Capstone.Models;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ParkDAO : IParkDAO
    {
        private string ConnectionString;

        public ParkDAO (string databseconnectionString)
        {
            ConnectionString = databseconnectionString;
        }

         public IList<Park> GetAllParks()
        {
            IList<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    string sqlStatment = "select * from park order by name";
                    sqlCommand.CommandText = sqlStatment;
                    sqlCommand.Connection = connection;
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.ID = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.AnnualVisitorCount = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);
                        parks.Add(park);
                    }
                }


            }
            catch(SqlException se)
            {
                Console.WriteLine(se.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return parks;
        }
    }
}
