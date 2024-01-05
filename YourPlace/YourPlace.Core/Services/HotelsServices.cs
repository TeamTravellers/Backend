using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Contracts;

namespace YourPlace.Core.Services
{
    public class HotelsServices :IHotel
    {
        private readonly YourPlaceDbContext _dbContext;

        public HotelsServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Hotel>> GetAllHotels()
        {
            return await _dbContext.Hotels.ToListAsync();
        }
        public async Task<List<Hotel>> SortHotels(string location, string tourism, string atmosphere, string company, decimal pricing)
        {
            var filteredHotels = new List<Hotel>();
            var filters = await _dbContext.Categories.Where(x => 
                x.Location == location ||
                x.Tourism == tourism ||
                x.Atmosphere == atmosphere ||
                x.Company == company ||
                x.Pricing == pricing)
                .ToListAsync();
            foreach(var filter in filters)
            {
                filteredHotels = await _dbContext.Hotels.Where(x => x.HotelID == filter.HotelID).ToListAsync();
            }
            return filteredHotels.ToList(); //TO BE CHANGED
        }
        public Hotel ShowHotelInfo(int hotelID)
        {
            Hotel hotel = _dbContext.Hotels.Find(hotelID);
            return hotel;
        }
        public List<Image> ShowHotelImages(int hotelID)
        {
            var hotelImages = _dbContext.Images.Where(x => x.HotelID ==  hotelID).ToList();
            return hotelImages;
        }

        //FILTERS
        public async Task<List<Hotel>> FilterByCountry(string country)
        {
            var hotelsByCountry = await _dbContext.Hotels.Where(x=>x.Country == country).ToListAsync();
            return hotelsByCountry;
        }
        public async Task<List<Hotel>> FilterByPeopleCount(int count)
        {
            var rooms = await _dbContext.Rooms.Where(x => x.MaxPeopleCount >= count).ToListAsync();
            var filteredHotels = new List<Hotel>();
            foreach(var room in rooms)
            {
                filteredHotels = await _dbContext.Hotels.Where(x => x.HotelID == room.HotelID).Distinct().ToListAsync();
            }
            return filteredHotels;
        }
        
        public async Task<List<Hotel>> FilterByPrice(decimal price)
        {
            var rooms = await _dbContext.Rooms.Where(x => x.Price <= price).ToListAsync();
            var filteredHotels = new List<Hotel>();
            foreach(var room in rooms)
            {
                filteredHotels = await _dbContext.Hotels.Where(x=>x.HotelID == room.HotelID).Distinct().ToListAsync();
            }
            return filteredHotels;
        }
        public async Task<List<Hotel>> FilterByDates(DateOnly arrivingDate, DateOnly leavingDate)
        {
            var reservations = await _dbContext.Reservations.ToListAsync();
            var freeRooms = new List<Room>();
            var filteredHotels = new List<Hotel>();

            foreach(var reservation in reservations)
            {
                if(leavingDate < reservation.ArrivalDate && arrivingDate < leavingDate || arrivingDate > reservation.LeavingDate && leavingDate > arrivingDate)
                {
                    freeRooms = await _dbContext.Rooms.Where(x => x.RoomID == reservation.RoomID).ToListAsync();
                }
            }
            foreach(var room in freeRooms)
            {
                filteredHotels = await _dbContext.Hotels.Where(x => x.HotelID == room.RoomID).Distinct().ToListAsync();
            }
            return filteredHotels;
        }



    }
}
