using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    { 
        int AddReservation(Reservation newReservation);
        IList<Reservation> GetReservations();
        IDictionary<Reservation, string> Get30DaysReservations(DateTime currentDate, int parkid);
        //TODO add parameters
        
    }
}
