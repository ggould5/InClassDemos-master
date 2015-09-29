using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestaurantSystem.DAL.DTOs
{
    public class ReservationByDate
    {
        public string Description { get; set; }
        public IEnumerable Reservations { get; set; }
    }
}
