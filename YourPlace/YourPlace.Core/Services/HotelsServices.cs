using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Core.Services
{
    public class HotelsServices //: IHotel
    {
        private readonly YourPlaceDbContext _dbContext;

        public HotelsServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Hotel>>  GetAllHotels()
        {
            return _dbContext.Hotels.ToList();
        }
        //public async Task<List<Hotel>> SortHotels(string location, string tourism, string atmosphere, string company, decimal pricing)
        //{
            //var suggestedHotels = await _dbContext.Hotels.Where(x => x.Location == location )
        //}
        

    }
}
