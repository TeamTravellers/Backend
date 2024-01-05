using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Contracts;

namespace YourPlace.Core.Services
{
    public class ReservationServices : IReservation
    {
        private readonly YourPlaceDbContext _dbContext;
        public ReservationServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MakeReservation(Reservation reservation)
        {
            var newReservation = new Reservation
            {
                FirstName = reservation.FirstName,
                Surname = reservation.Surname,
                ArrivalDate = reservation.ArrivalDate,
                LeavingDate = reservation.LeavingDate,
                PeopleCount = reservation.PeopleCount,
                Price = reservation.Price,
                RoomID = reservation.RoomID,  
            };
            await _dbContext.Reservations.AddAsync(newReservation);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteReservation(Reservation reservation)
        {
            _dbContext.Reservations.Remove(reservation);
            _dbContext.SaveChanges();
        }
        public async Task UpdateReservation(Reservation editedReservation)
        {
            var reservationToBeEdited = await _dbContext.Reservations.FindAsync(editedReservation.ReservationID);
            reservationToBeEdited.FirstName = editedReservation.FirstName;
            reservationToBeEdited.Surname = editedReservation.Surname;
            reservationToBeEdited.ArrivalDate = editedReservation.ArrivalDate;
            reservationToBeEdited.LeavingDate = editedReservation.LeavingDate;
            reservationToBeEdited.PeopleCount = editedReservation.PeopleCount;
            reservationToBeEdited.Price = editedReservation.Price;
        }
    }
}
