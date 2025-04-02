using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services
{
    public class CityService : ICityService
    {
        private readonly ICityDAO _cityDAO;

        public CityService(ICityDAO cityDAO)
        {
            _cityDAO = cityDAO;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await _cityDAO.GetAllCitiesAsync();
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            return await _cityDAO.GetCityByIdAsync(id);
        }

        public async Task<City> GetCityByNameAsync(string name)
        {
            return await _cityDAO.GetCityByNameAsync(name);
        }
    }
}