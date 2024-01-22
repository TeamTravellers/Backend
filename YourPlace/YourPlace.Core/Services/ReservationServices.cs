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
        private readonly HotelsServices _hotelsServices;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;
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

        #region another old code for checking rooms
                public List<Room> FreeRoomCheck(DateOnly arrivalDate, DateOnly leavingDate, Hotel hotel)
                {
                    //bool response;
                    IQueryable<Room> roomsInHotel = _dbContext.Rooms.Where(x => x.HotelID == hotel.HotelID);
                    List<Reservation> reservations = new List<Reservation>();
                    foreach (Room room in roomsInHotel)
                    {
                        reservations = _dbContext.Reservations.Where(x => x.RoomID == room.RoomID).ToList();
                    }

                    //IQueryable<Room> reservedRooms;
                    List<Room> freeRooms = new List<Room>();
                    foreach (var eachReservation in reservations)
                    {
                        if (arrivalDate > eachReservation.LeavingDate || leavingDate < eachReservation.ArrivalDate)
                        {
                            //response = true;
                            Console.WriteLine("The room is free.");
                            freeRooms.AddRange(roomsInHotel.Where(x => x.RoomID == eachReservation.RoomID));
                            //CreateReservation(reservation);
                        }
                        else
                        {
                            //response = false;
                            Console.WriteLine("There are no free rooms.");
                            //reservedRooms = rooms.Where(x => x.RoomID == eachReservation.RoomID);
                        }
                    }
                    return freeRooms;
                }
                public List<Room> FreeRoomsAccordingToPeopleCount(DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, Hotel hotel)
                {
                    RoomTypes roomType = RoomTypesHelper.GetRoomTypeForPeopleCount(peopleCount);
                    List<Room> freeRooms = FreeRoomCheck(arrivalDate, leavingDate, hotel).ToList();
                    List<Room> appropriateFreeRooms = new List<Room>();
                    foreach (var room in freeRooms)
                    {
                        if (room.Type.ToString().ToLower() == roomType.ToString().ToLower())
                        {
                            appropriateFreeRooms.Add(room);
                        }
                    }
                    return appropriateFreeRooms;
                }
                public List<RoomTypes> GetRoomTypesForBiggerPeopleCount(DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, int roomID, Hotel hotel)
                {
                    if (peopleCount > 0 && peopleCount <= 6)
                    {
                        RoomTypesHelper.GetRoomTypeForPeopleCount(peopleCount);
                    }
                    else
                    {
                        //TO BE CONTINUED...
                        //List<RoomTypes> roomTypes = new List<RoomTypes>();
                        //int peopleCount = reservation.PeopleCount;
                        List<Room> freeRoomsInHotel = FreeRoomCheck(arrivalDate, leavingDate, hotel).ToList();
                        List<Room> suggestedRooms = new List<Room>();
                        Dictionary<int, string> availableRooms = new Dictionary<int, string>();
                        Room room = _dbContext.Rooms.Find(roomID);
                        var sameTypeRoomsAvailability = freeRoomsInHotel.Select(x => x.Type == room.Type).Count();
                        availableRooms.Add(sameTypeRoomsAvailability, room.Type.ToString());

                        foreach (var availability in availableRooms.Keys)
                        {
                            if (peopleCount >= availability)
                            {

                            }
                        }


                    }
                    return null;
                }
                public async Task<bool> CompleteReservation(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, decimal price, int roomID, Hotel hotel)
                {
                    bool success;
                    try
                    {
                        List<Room> appropriateFreeRooms = FreeRoomsAccordingToPeopleCount(arrivalDate, leavingDate, peopleCount, hotel).ToList();
                        List<int> roomsIDs = appropriateFreeRooms.Select(x => x.RoomID).ToList();
                        Random random = new Random();
                        int randomIndex = random.Next(roomsIDs.Count);
                        int finalRoomID = roomsIDs[randomIndex];
                        CreateAsync(new Reservation(firstName, surname, arrivalDate, leavingDate, peopleCount, price, finalRoomID));
                        //reservation.RoomID = roomIDs;
                        //await CreateAsync(reservation, roomID);
                        success = true;
                    }
                    catch
                    {
                        success = false;
                    }
                    return success;
                }
                #endregion

        public async Task CheckForTotalRoomAvailability(int hotelID, int peopleCount)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            int totalRooms = await _roomAvailabiltyServices.GetTotalCountOfRoomsInHotel(hotelID);
            int maxPeopleInHotel = 0;
            List<Room> roomsInHotel = await _dbContext.Rooms.Where(x => x.HotelID == hotelID).ToListAsync();
            foreach(var room in roomsInHotel)
            {
                maxPeopleInHotel += room.MaxPeopleCount;
            }
            
            if(peopleCount > maxPeopleInHotel)
            {
                throw new Exception("Sorry, we do not have enough rooms!");
            }
            else
            {
                //CALL SOME METHODS
            }
        }
        public async Task<Family> CreateFamily(int totalCount, int adultsCount, int childrenCount, Family family)
        {
            Family newFamily = new Family(totalCount, adultsCount, childrenCount);
            return newFamily;
        }
        public async Task AccomodateFamily(int hotelID, Family family, RoomAvailability availability)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);

        }
        public async Task ManageRoomsForFamilies(int hotelID, Family family)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            List<RoomAvailability> availability = await _roomAvailabiltyServices.ReadAsync(hotelID);
            foreach(var item in availability)
            {

            }
        }
    }
}
