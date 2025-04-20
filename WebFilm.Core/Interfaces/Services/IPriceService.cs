using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Price;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IPriceService
    {
        Task<Price> Create(Price price);
        Task<bool> Delete(int id);
        Task<IEnumerable<Price>> GetAll();
    }
} 