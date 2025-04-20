using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Price;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IPriceRepository
    {
        Task<int> Add(Price price);
        Task<int> Delete(int id);
        Task<IEnumerable<Price>> GetAll();
    }
} 