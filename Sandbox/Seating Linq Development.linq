<Query Kind="Statements">
  <Connection>
    <ID>01fe9a26-8f6c-4446-9b9f-82ae63ab33d1</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>


var date = Bills.Max(eachBill => eachBill.BillDate).Date;

//adjust the time
var time = Bills.Max(eachBill => eachBill.BillDate).TimeOfDay.Add(new TimeSpan(0, 30, 0));

// Step 1 - Get the table info along with any walk-in bills and reservation bills for the specific time slot
var step1 = from data in Tables
             select new
             {
                Table = data.TableNumber,
                Seating = data.Capacity,
                // This sub-query gets the bills for walk-in customers
                WalkIns = from walkIn in data.Bills
                        where 
                               walkIn.BillDate.Year == date.Year
                            && walkIn.BillDate.Month == date.Month
                            && walkIn.BillDate.Day == date.Day
                            && walkIn.BillDate.TimeOfDay <= time
                            && (!walkIn.OrderPaid.HasValue || walkIn.OrderPaid.Value >= time)
  //                       	&& (!walkIn.PaidStatus || walkIn.OrderPaid >= time)
                        select walkIn,
                 // This sub-query gets the bills for reservations
                 Reservations = from booking in data.ReservationTables
									from reservationParty in booking.Reservation.Bills
									where 
										reservationParty.BillDate.Year == date.Year
										&& reservationParty.BillDate.Month == date.Month
										&& reservationParty.BillDate.Day == date.Day
										&& reservationParty.BillDate.TimeOfDay <= time
										&& (!reservationParty.OrderPaid.HasValue || reservationParty.OrderPaid.Value >= time)
			//                          && (!reservationParty.PaidStatus || reservationParty.OrderPaid >= time)
									select reservationParty
             };
//step1.Dump();

// Step 2 - Union the walk-in bills and the reservation bills while extracting the relevant bill info
// .ToList() helps resolve the "Types in Union or Concat are constructed incompatibly" error
var step2 = from data in step1.ToList() // .ToList() forces the first result set to be in memory
            select new
            {
                Table = data.Table,
                Seating = data.Seating,
                CommonBilling = from info in data.WalkIns.Union(data.Reservations)
                                select new // info
                                {
                                    BillID = info.BillID,
                                    BillTotal = info.BillItems.Sum(bi => bi.Quantity * bi.SalePrice),
                                    Waiter = info.Waiter.FirstName,
                                    Reservation = info.Reservation
                                }
            };
step2.Dump();