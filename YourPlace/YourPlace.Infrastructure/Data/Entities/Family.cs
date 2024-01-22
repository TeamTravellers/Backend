using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Family
    {
        public int TotalCount { get; set; }
        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }

        public Family(int totalCount, int adultsCount, int childrenCount)
        {
            TotalCount = totalCount;
            AdultsCount = adultsCount;
            ChildrenCount = childrenCount;
        }
    }
}
