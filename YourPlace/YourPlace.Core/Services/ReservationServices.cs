using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Contracts;
using System.ComponentModel;
using System.Data.Entity;
using YourPlace.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;

namespace YourPlace.Core.Services
{
    public class ReservationServices : IReservation, IDbCRUD<Reservation, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        public ReservationServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region CRUD For Reservations
        public async Task CreateAsync(Reservation item)
        {
            try
            {
                _dbContext.Add(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Reservation> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Reservation> reservations = _dbContext.Reservations;
                if (isReadOnly)
                {
                    reservations.AsNoTrackingWithIdentityResolution();
                }
                return await reservations.SingleOrDefaultAsync(x => x.ReservationID == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Reservation> reservations = _dbContext.Reservations;
                if (isReadOnly)
                {
                    reservations.AsNoTrackingWithIdentityResolution();
                }
                return await reservations.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Reservation item)
        {
            try
            {
                _dbContext.Update(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                Reservation reservation = await ReadAsync(key);
                if (reservation == null)
                {
                    throw new ArgumentException(string.Format($"Computer with id {key} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Remove(reservation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Old code
        //public async Task CreateReservation(Reservation reservation, int roomID)
        //{
        //    var newReservation = new Reservation
        //    {
        //        FirstName = reservation.FirstName,
        //        Surname = reservation.Surname,
        //        ArrivalDate = reservation.ArrivalDate,
        //        LeavingDate = reservation.LeavingDate,
        //        PeopleCount = reservation.PeopleCount,
        //        Price = reservation.Price,
        //        RoomID = roomID,  
        //    };
        //    _dbContext.Reservations.Add(newReservation);
        //    await _dbContext.SaveChangesAsync();
        //}
        //public async Task DeleteReservation(Reservation reservation)
        //{
        //    _dbContext.Reservations.Remove(reservation);
        //    await _dbContext.SaveChangesAsync();
        //}
        //public async Task UpdateReservation(Reservation editedReservation)
        //{
        //    var reservationToBeEdited = await _dbContext.Reservations.FindAsync(editedReservation.ReservationID);
        //    reservationToBeEdited.FirstName = editedReservation.FirstName;
        //    reservationToBeEdited.Surname = editedReservation.Surname;
        //    reservationToBeEdited.ArrivalDate = editedReservation.ArrivalDate;
        //    reservationToBeEdited.LeavingDate = editedReservation.LeavingDate;
        //    reservationToBeEdited.PeopleCount = editedReservation.PeopleCount;
        //    reservationToBeEdited.Price = editedReservation.Price;
        //}
        #endregion
    }
}
