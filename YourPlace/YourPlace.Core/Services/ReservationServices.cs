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
using Microsoft.Identity.Client;
using System.Diagnostics;
using Microsoft.Identity.Client.Extensibility;

namespace YourPlace.Core.Services
{
    public class ReservationServices : IReservation, IDbCRUD<Reservation, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly HotelsServices _hotelsServices;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;
        private readonly Filters _filters;

        private List<Family> CreatedFamilies = new List<Family>();
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


        
        public async Task<bool> CompareTotalCountWithFamilyMembersCount(List<Family> families, int totalCount) //something like js function
        {
            try
            {
                bool success = false;
                int totalCountInFamilies = 0;
                foreach (var family in families)
                {
                    totalCountInFamilies += family.TotalCount;
                }
                if (totalCountInFamilies != totalCount)
                {
                    throw new Exception("The total number of people does not match the number of all members in the families!");
                    success = false;
                }
                else
                {
                    success = true;
                }
                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

      

        public async Task<bool> CheckForTotalRoomAvailability(int hotelID, int peopleCount) // == filters
        {
            try
            {
                bool result = false;
                Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                List<Hotel> appropriateHotels = await _filters.FilterByPeopleCount(peopleCount);
                if (appropriateHotels.Contains(hotel))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch(Exception)
            {
                throw;
            }
            
            //Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            ////int totalRooms = await _roomAvailabiltyServices.GetTotalCountOfRoomsInHotel(hotelID);
            //int maxPeopleInHotel = 0;
            //List<Room> roomsInHotel = await _dbContext.Rooms.Where(x => x.HotelID == hotelID).ToListAsync();
            ////int totalRooms = roomsInHotel.Count;
            //foreach (var room in roomsInHotel)
            //{
            //    maxPeopleInHotel += room.MaxPeopleCount;
            //}

            //if (peopleCount > maxPeopleInHotel)
            //{
            //    throw new Exception("Sorry, we do not have enough rooms!");
            //}
            //else
            //{
            //    //CALL SOME METHODS
            //}

        }
        public async Task<List<Room>> FreeRoomCheck(DateOnly arrivalDate, DateOnly leavingDate, int hotelID) // == filters
        {
            try
            {
                List<Hotel> hotelsWithFreeRooms = await _filters.FilterByDates(arrivalDate, leavingDate);
                Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                List<Room> roomsInHotel = await _roomAvailabiltyServices.GetAllRoomsInHotel(hotelID);
                List<Room> freeRoomsInHotel = new List<Room>();

                if (hotelsWithFreeRooms.Contains(hotel))
                {
                    foreach (var room in roomsInHotel)
                    {
                        freeRoomsInHotel.Add(room);
                    }

                }
                return freeRoomsInHotel;
            }
            catch (Exception)
            {
                throw;
            }
            //bool response;
            //IQueryable<Room> roomsInHotel = _dbContext.Rooms.Where(x => x.HotelID == hotel.HotelID);
            //List<Reservation> reservations = new List<Reservation>();
            //foreach (Room room in roomsInHotel)
            //{
            //    reservations = _dbContext.Reservations.Where(x => x.HotelID == hotel.HotelID).ToList();
            //}

            ////IQueryable<Room> reservedRooms;

            //foreach (var eachReservation in reservations)
            //{
            //    if (arrivalDate > eachReservation.LeavingDate || leavingDate < eachReservation.ArrivalDate)
            //    {
            //        //response = true;
            //        Console.WriteLine("The room is free.");
            //        freeRooms.AddRange(roomsInHotel.Where(x => x.HotelID == eachReservation.HotelID));
            //        //CreateReservation(reservation);
            //    }
            //    else
            //    {
            //        //response = false;
            //        Console.WriteLine("There are no free rooms.");
            //        //reservedRooms = rooms.Where(x => x.RoomID == eachReservation.RoomID);
            //    }
            //}

        }
        public async Task<Family> CreateFamily(int totalCount)
        {
            try
            {
                Family newFamily = new Family(totalCount);
                CreatedFamilies.Add(newFamily);
                return newFamily;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public async Task<List<Family>> CreateFamilies(int familyCount, int membersCount)
        //{
        //    List<Family> currentlyCreatedFamilies = new List<Family>();
        //    for (int i = 0; i < familyCount; i++)
        //    {
        //        Family family = await CreateFamily(membersCount);
        //        currentlyCreatedFamilies.Add(family);
        //    }
        //    return currentlyCreatedFamilies;
        //}
        public async Task<List<Room>> FreeRoomsAccordingToPeopleCount(DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, int hotelID)
        {
            try
            {
                Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                RoomTypes roomType = RoomTypesHelper.GetRoomTypeForPeopleCount(peopleCount);
                List<Room> freeRooms = await FreeRoomCheck(arrivalDate, leavingDate, hotelID);
                List<Room> appropriateFreeRooms = freeRooms.Where(x => x.Type.ToString().ToLower() == roomType.ToString().ToLower()).ToList();
                return appropriateFreeRooms.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        
        public async Task<int> CountOfFreeRoomsAccordingToType(int hotelID, Family family, DateOnly arrivalDate, DateOnly leavingDate)
        {
            try
            {
                Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                List<Room> freeRooms = await FreeRoomsAccordingToPeopleCount(arrivalDate, leavingDate, family.TotalCount, hotelID);
                //List<Room> roomsInHotel = freeRooms.Where(x => x.HotelID == hotelID).ToList();
                int count = freeRooms.Where(x => x.MaxPeopleCount == family.TotalCount).Count();
                
                if (count == 0)
                {
                    throw new Exception($"No free rooms for this number of people!");
                }
                else
                {
                    return count;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }    
        public async Task<Room> AccomodateFamily(int hotelID, Family family, DateOnly arrivalDate, DateOnly leavingDate)
        {
            Room room = new Room();
            List<Room> freeRooms = await FreeRoomsAccordingToPeopleCount(arrivalDate, leavingDate, family.TotalCount, hotelID);
            if (freeRooms is null)
            {
                throw new Exception("There are no free rooms for this family!");
            }
            else
            {
                int count = await CountOfFreeRoomsAccordingToType(hotelID, family, arrivalDate, leavingDate);
                List<int> roomsIDs = freeRooms.Select(x => x.RoomID).ToList();
                Random random = new Random();
                int randomIndex = random.Next(roomsIDs.Count);
                int finalRoomID = roomsIDs[randomIndex];
                room = await _dbContext.Rooms.FindAsync(finalRoomID);
            }
            return room;
        }
        
        public async Task<bool> CompleteReservation(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, decimal price, int hotelID, int familyCount)
        {
            bool success;
            try
            {
                CheckForTotalRoomAvailability(hotelID, peopleCount);
                CompareTotalCountWithFamilyMembersCount(CreatedFamilies, peopleCount); //more like js function
                List<Room> currentlyReservedRooms = new List<Room>();
                
                foreach (Family family in CreatedFamilies)
                {
                    //CreateFamily(peopleCount, family);
                    Room room = await AccomodateFamily(hotelID, family, arrivalDate, leavingDate);
                    currentlyReservedRooms.Add(room);
                }
                CreateAsync(new Reservation(firstName, surname, arrivalDate, leavingDate, peopleCount, price, hotelID, currentlyReservedRooms, CreatedFamilies));
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
