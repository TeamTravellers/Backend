﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Core.Contracts
{
    public interface IReservation
    {
        public Task MakeReservation(Reservation reservation);
        public Task DeleteReservation(Reservation reservation);

        public Task UpdateReservation(Reservation editedReservation);
    }
}
