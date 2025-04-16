using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services
{
    public class RapidApiDestinationService : IRapidApiDestinationService
    {
        private readonly IRapidApiDestinationDAO _rapidApiDestinationDAO;

        public RapidApiDestinationService(IRapidApiDestinationDAO rapidApiDestinationDAO)
        {
            _rapidApiDestinationDAO = rapidApiDestinationDAO;
        }

        public async Task<List<RapidApiDestination>> SearchRapidApiDestinationsByQueryAsync(string cityname)
        {
            return await _rapidApiDestinationDAO.SearchRapidApiDestinationsByQueryAsync(cityname);
        }
    }
}