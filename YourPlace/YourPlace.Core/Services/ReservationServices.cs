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
            catch(Exception)
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
        public List<Room> FreeRoomCheck(Reservation reservation, Hotel hotel)
        {
            //bool response;
            IQueryable<Room> roomsInHotel = _dbContext.Rooms.Where(x=>x.HotelID == hotel.HotelID);
            List<Reservation> reservations = new List<Reservation>();
            foreach (Room room in roomsInHotel)
            {
                reservations = _dbContext.Reservations.Where(x => x.RoomID == room.RoomID).ToList();
            }
            
            //IQueryable<Room> reservedRooms;
            List<Room> freeRooms = new List<Room>();
            foreach (var eachReservation in reservations)
            {
                if (reservation.ArrivalDate > eachReservation.LeavingDate || reservation.LeavingDate < eachReservation.ArrivalDate)
                {
                    //response = true;
                    Console.WriteLine("The room is free.");
                    freeRooms.AddRange(roomsInHotel.Where(x => x.RoomID == eachReservation.RoomID));
                    //CreateReservation(reservation);
                }
                else
                {
                    //response = false;
                    Console.WriteLine("The room is not free.");
                    //reservedRooms = rooms.Where(x => x.RoomID == eachReservation.RoomID);
                }
            }
            return freeRooms;
        }
        public List<Room> FreeRoomsAccordingToPeopleCount(Reservation reservation, Hotel hotel)
        {
            RoomTypes roomType = RoomTypesHelper.GetRoomTypeForPeopleCount(reservation.PeopleCount);
            List<Room> freeRooms = FreeRoomCheck(reservation, hotel).ToList();
            List<Room> appropriateFreeRooms = new List<Room>();  
            foreach (var room in freeRooms)
            {
                if(room.Type.ToString().ToLower() == roomType.ToString().ToLower())
                {
                    appropriateFreeRooms.Add(room);
                }
            }
            return appropriateFreeRooms;
        }
        public List<RoomTypes> GetRoomTypesForBiggerPeopleCount(Reservation reservation, Hotel hotel)
        {
            if (reservation.PeopleCount > 0 && reservation.PeopleCount <= 6)
            {
                RoomTypesHelper.GetRoomTypeForPeopleCount(reservation.PeopleCount);
            }
            else
            {
                //TO BE CONTINUED...
                //List<RoomTypes> roomTypes = new List<RoomTypes>();
                int peopleCount = reservation.PeopleCount;
                List<Room> freeRoomsInHotel = FreeRoomCheck(reservation, hotel).ToList();
                List<Room> suggestedRooms = new List<Room>();
                Dictionary<int, string> availableRooms = new Dictionary<int, string>();
                var roomID = reservation.RoomID;
                Room room = _dbContext.Rooms.Find(roomID);
                var sameTypeRoomsAvailability = freeRoomsInHotel.Select(x => x.Type == room.Type).Count();
                availableRooms.Add(sameTypeRoomsAvailability, room.Type.ToString());

                foreach(var availability in availableRooms.Keys)
                {
                    if(peopleCount >= availability)
                    {
                        
                    }
                }


            }
            return null;
        }
        public async Task<bool> CompleteReservation(Reservation reservation, Hotel hotel)
        {
            bool success;
            try
            {
                List<Room> appropriateFreeRooms = FreeRoomsAccordingToPeopleCount(reservation, hotel).ToList();
                List<int> roomsIDs = appropriateFreeRooms.Select(x => x.RoomID).ToList();
                Random random = new Random();
                int randomIndex = random.Next(roomsIDs.Count);
                int roomID = roomsIDs[randomIndex];
                reservation.RoomID = roomID;
                //await CreateAsync(reservation, roomID);
                success = true;
            }
            catch
            {
                success = false;
            }
            return success;
        }
   
    }
}
