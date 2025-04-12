using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectionDAO _connectionDAO;

        public ConnectionService(IConnectionDAO connectionDAO)
        {
            _connectionDAO = connectionDAO;
        }

        public async Task<Connection> GetConnectionByCitiesAsync(int departureCityId, int arrivalCityId)
        {
            return await _connectionDAO.GetConnectionByCitiesAsync(departureCityId, arrivalCityId);
        }

        public async Task<IEnumerable<Connection>> GetAllConnectionsAsync()
        {
            return await _connectionDAO.GetAllConnectionsAsync();
        }
    }
}