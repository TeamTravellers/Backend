using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace YourPlace.Core.Services
{
    public class HotelsServices :IHotel, IDbCRUD<Hotel, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;

        public HotelsServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<List<Hotel>> GetAllHotels()
        //{
        //    return await _dbContext.Hotels.ToListAsync();
        //}
        #region CRUD For Hotels
        public async Task CreateAsync(Hotel hotel)
        {
            try
            {
                _dbContext.Hotels.Add(hotel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Hotel> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Hotel> hotels = _dbContext.Hotels;
                if (isReadOnly)
                {
                    hotels.AsNoTrackingWithIdentityResolution();
                }
                return await hotels.SingleOrDefaultAsync(x => x.HotelID == key);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Image>> ShowHotelImages(int hotelID)
        {
            var hotelImages = _dbContext.Images.Where(x => x.HotelID == hotelID).ToList();
            return hotelImages;
        }

        public async Task<IEnumerable<Hotel>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Hotel> hotels = _dbContext.Hotels;
                if (isReadOnly)
                {
                    hotels.AsNoTrackingWithIdentityResolution();
                }
                return await hotels.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Hotel item)
        {
            try
            {
                _dbContext.Hotels.Update(item);
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
                Hotel hotel = await ReadAsync(key, false, false);
                if (hotel is null)
                {
                    throw new ArgumentException(string.Format($"Hotel with id {key} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Hotels.Remove(hotel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
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

        

    }
}
