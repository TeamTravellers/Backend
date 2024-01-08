using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Contracts;
using System.ComponentModel;
using YourPlace.Core.Enums;
using System.Data.Entity;

namespace YourPlace.Core.Services
{
    public class ReservationServices : IReservation
    {
        private readonly YourPlaceDbContext _dbContext;
        public ReservationServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task CreateReservation(Reservation reservation, int roomID)
        {
            var newReservation = new Reservation
            {
                FirstName = reservation.FirstName,
                Surname = reservation.Surname,
                ArrivalDate = reservation.ArrivalDate,
                LeavingDate = reservation.LeavingDate,
                PeopleCount = reservation.PeopleCount,
                Price = reservation.Price,
                RoomID = roomID,  
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

        public List<Room> FreeRoomCheck(Reservation reservation)
        {
            //bool response;
            IQueryable<Room> rooms = _dbContext.Rooms;
            IQueryable<Reservation> reservations = _dbContext.Reservations;
            //IQueryable<Room> reservedRooms;
            List<Room> freeRooms = new List<Room>();
            foreach (var eachReservation in reservations)
            {
                if (reservation.ArrivalDate > eachReservation.LeavingDate || reservation.LeavingDate < eachReservation.ArrivalDate)
                {
                    //response = true;
                    Console.WriteLine("The room is free.");
                    freeRooms.AddRange(rooms.Where(x => x.RoomID == eachReservation.RoomID));
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
        public List<Room> FreeRoomsAccordingToPeopleCount(Reservation reservation)
        {
            RoomTypes roomType = RoomTypesHelper.GetRoomTypeForPeopleCount(reservation.PeopleCount);
            List<Room> freeRooms = FreeRoomCheck(reservation).ToList();
            List<Room> appropriateFreeRooms = new List<Room>();  
            foreach (var room in freeRooms)
            {
                if(room.Type.ToLower() == roomType.ToString().ToLower())
                {
                    appropriateFreeRooms.Add(room);
                }
            }
            return appropriateFreeRooms;
        }
        public List<RoomTypes> GetRoomTypesForBiggerPeopleCount(Reservation reservation)
        {
            if (reservation.PeopleCount > 0 && reservation.PeopleCount <= 6)
            {
                RoomTypesHelper.GetRoomTypeForPeopleCount(reservation.PeopleCount);
            }
            else
            {
                List<Room> freeRooms = FreeRoomsAccordingToPeopleCount(reservation);
                //TO BE CONTINUED...
                List<RoomTypes> roomTypes = new List<RoomTypes>();
                
            }
            return null;
        }
        public async Task<bool> CompleteReservation(Reservation reservation)
        {
            bool success;
            try
            {
                List<Room> appropriateFreeRooms = FreeRoomsAccordingToPeopleCount(reservation).ToList();
                List<int> roomsIDs = appropriateFreeRooms.Select(x => x.RoomID).ToList();
                Random random = new Random();
                int randomIndex = random.Next(roomsIDs.Count);
                int roomID = roomsIDs[randomIndex];
                reservation.RoomID = roomID;
                await CreateReservation(reservation, roomID);
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
