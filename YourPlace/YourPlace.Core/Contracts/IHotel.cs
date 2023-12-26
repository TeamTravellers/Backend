using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Core.Contracts
{
    public interface IHotel
    {
        public Task GetAllHotels();
        public Task SortHotels(string location, string tourism, string atmosphere, string company, decimal pricing);
        public Task ShowHotelInfo();

        public Task Filter();
    }
}
