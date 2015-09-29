using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.DAL;
using eRestaurantSystem.DAL.Entities;
using eRestaurantSystem.DAL.DTOs;
using eRestaurantSystem.DAL.POCOs;
using System.ComponentModel; //Object Data Source
#endregion

namespace eRestaurantSystem.BLL
{
    [DataObject]
    public class AdminController
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SpecialEvent> SpecialEvents_List()
        {
            //connect to our DbContext class in the DAL
            //create an instance of the class
            //we will use a transaction to hold our query
            using (var context = new eRestaurantContext())
            {
                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();

                //query syntax
                var results = from item in context.SpecialEvents
                              orderby item.Description
                              select item;
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Reservation> GetReservationsByEventCode(string eventcode)
        {
            using (var context = new eRestaurantContext())
            {
                //query syntax
                var results = from item in context.Reservations
                              where item.EventCode.Equals(eventcode)
                              orderby item.CustomerName, item.ReservationDate
                              select item;
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ReservationByDate> GetReservationsByDate(string reservationdate)
        {
            using (var context = new eRestaurantContext())
            {
                int theYear =(DateTime.Parse(reservationdate)).Year;
                int theMonth =(DateTime.Parse(reservationdate)).Month;
                int theDay =(DateTime.Parse(reservationdate)).Day;
                //query syntax
                var results = from item in context.SpecialEvents
                              orderby item.Description
                              select new ReservationByDate()
                              {
                                    Description = item.Description,
                                    Reservations = from r in item.Reservations
                                                   where r.ReservationDate.Year == theYear
                                                     && r.ReservationDate.Month == theMonth
                                                     && r.ReservationDate.Day == theDay
                                                    select new ReservationDetail() 
                                                    {
                                                        CustomerName = r.CustomerName,
                                                        ReservationDate = r.ReservationDate,
                                                        NumberInParty = r.NumberInParty,
                                                        ContactPhone = r.ContactPhone,
                                                        ReservationStatus = r.ReservationStatus,
                                                    }
                                                           

                              };
                return results.ToList();
            }
        }
      
    }
}
