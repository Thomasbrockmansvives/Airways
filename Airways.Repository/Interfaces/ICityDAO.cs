using Airways.Domain.EntitiesDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Repository.Interfaces
{
    public interface ICityDAO
    {
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<City> GetCityByIdAsync(int id);
        Task<City> GetCityByNameAsync(string name);

        Task<decimal?> GetDestIdByCityNameAsync(string cityName);
    }
}