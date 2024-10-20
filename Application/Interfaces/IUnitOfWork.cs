﻿using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRestaurantRepository RestaurantRepository { get; }
        IGenericRepository<T> GetGenericRepository<T>() where T : class;
        Task<int> CompleteAsync();
    }
}
