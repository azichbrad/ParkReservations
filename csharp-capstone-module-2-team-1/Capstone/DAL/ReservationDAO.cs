using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO : IReservationDAO
    {
        public ReservationDAO(string dbConnectionString)
        {
            ConnectionString = dbConnectionString;
        }
        private string ConnectionString;

        public int AddReservation(Reservation newReservation)
        {
            int newReservationID = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string sqlstatement = "insert into reservation values (@site_id, @name, @from_date, @to_date, @create_date);select scope_identity();";
                    SqlCommand command = new SqlCommand(sqlstatement,connection);
                    command.Parameters.AddWithValue("@site_id", newReservation.SiteID);
                    command.Parameters.AddWithValue("@name", newReservation.Name);
                    command.Parameters.AddWithValue("@from_date", newReservation.FromDate.ToShortDateString());
                    command.Parameters.AddWithValue("@to_date", newReservation.ToDate.ToShortDateString());
                    command.Parameters.AddWithValue("@create_date", DateTime.Now);
                    newReservationID = Convert.ToInt32(command.ExecuteScalar());
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
            return newReservationID;
        }


        public IList<Reservation> GetReservations()
        {
            IList<Reservation> reservations = new List<Reservation>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    
                    string sqlStatement = "select * from reservation";
                    SqlCommand command = new SqlCommand(sqlStatement, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation()
                        {
                            ID = Convert.ToInt32(reader["reservation_id"]),
                            SiteID = Convert.ToInt32(reader["site_id"]),
                            Name = Convert.ToString(reader["name"]),
                            FromDate = Convert.ToDateTime(reader["from_date"]),
                            ToDate = Convert.ToDateTime(reader["to_date"]),
                            DateCreated = Convert.ToDateTime(reader["create_date"]),
                        };
                        reservations.Add(reservation);
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
            return reservations;
        }

        public IDictionary<Reservation, string> Get30DaysReservations(DateTime currentDate, int parkid)
        {
            IDictionary<Reservation, string> reservations = new Dictionary<Reservation, string>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string sqlStatement = "select reservation.reservation_id, reservation.site_id, site.site_number, campground.name as campgroundname, reservation.name, reservation.from_date, reservation.to_date, reservation.create_date from reservation join site on reservation.site_id = site.site_id join campground on site.campground_id = campground.campground_id where reservation.site_id in (select site.site_id from campground where park_id = @park_id) and from_date between @currentdate and @currentdate30;";
                    SqlCommand command = new SqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@currentdate", currentDate.ToShortDateString());
                    command.Parameters.AddWithValue("@currentdate30", currentDate.AddDays(30).ToShortDateString());
                    command.Parameters.AddWithValue("park_id", parkid);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation()
                        {
                            ID = Convert.ToInt32(reader["reservation_id"]),
                            SiteID = Convert.ToInt32(reader["site_id"]),
                            Name = Convert.ToString(reader["name"]),
                            FromDate = Convert.ToDateTime(reader["from_date"]),
                            ToDate = Convert.ToDateTime(reader["to_date"]),
                            DateCreated = Convert.ToDateTime(reader["create_date"]),
                        };
                        reservations.Add(reservation, Convert.ToString(reader["campgroundname"]));
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
            return reservations;
        }

        
    }
}

