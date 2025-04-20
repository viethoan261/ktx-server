using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Price;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class PriceService : IPriceService
    {
        private readonly IPriceRepository _priceRepository;

        public PriceService(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public async Task<Price> Create(Price price)
        {
            await _priceRepository.Add(price);
            return price;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _priceRepository.Delete(id);
            return result > 0;
        }

        public async Task<IEnumerable<Price>> GetAll()
        {
            return await _priceRepository.GetAll();
        }
    }
} 