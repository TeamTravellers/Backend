﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Core.Services
{
    public class RoomAvailabiltyServices
    {
        private readonly YourPlaceDbContext _dbContext;
        public RoomAvailabiltyServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region old code

        //TO DO: see if the hotels has these number of rooms


        
        #endregion
        
        public async Task CreateAsync(int hotelID, RoomTypes type, int count)
        {
            try
            {
                
                _dbContext.RoomsAvailability.Add(new RoomAvailability(hotelID, type, count)); 
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task FillAvailability(int hotelID)
        {
            try { 
            
                List<Room> roomsInHotel = _dbContext.Rooms.Where(x=>x.HotelID == hotelID).ToList();
                List<RoomTypes> roomTypes = new List<RoomTypes>();
                List<Room> roomsOfTheSameType = new List<Room>();
                int count = 0;
                foreach(Room room in roomsInHotel)
                {
                    roomTypes.Add(room.Type);
                    roomTypes.Distinct();

                }
                foreach(var type in roomTypes)
                {
                    roomsOfTheSameType = roomsInHotel.Where(x => x.Type.ToString().ToLower() == type.ToString().ToLower()).ToList();
                    count++;
                    CreateAsync(hotelID, type, count);
                }
                
            }
            catch(Exception)
            {
                throw;
            }
        }
        //public Task CompleteSomething(int hotelID, int peopleCount)
        //{

        //}

    }
}
