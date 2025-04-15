using Airways.Domain.EntitiesDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface ICityService
    {
        
        Task<IEnumerable<City>> GetAllCitiesAsync();
        
        Task<City> GetCityByIdAsync(int id);
        Task<City> GetCityByNameAsync(string name);
    }
}