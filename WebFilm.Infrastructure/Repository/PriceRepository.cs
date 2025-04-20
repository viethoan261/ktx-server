using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Price;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class PriceRepository : BaseRepository<int, Price>, IPriceRepository
    {
        public PriceRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> Add(Price price)
        {
            return await Task.FromResult(base.Add(price));
        }

        public async Task<int> Delete(int id)
        {
            return await Task.FromResult(base.Delete(id));
        }

        public async Task<IEnumerable<Price>> GetAll()
        {
            return await Task.FromResult(base.GetAll());
        }
    }
} 