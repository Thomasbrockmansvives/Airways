using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airways.Domain.EntitiesDB;

//The connections table is used to lookup the different flights you need to connect to go from departure to arrival

namespace Airways.Services.Interfaces
{
    public interface IConnectionService
    {
        
        Task<Connection> GetConnectionByCitiesAsync(int departureCityId, int arrivalCityId);
        Task<IEnumerable<Connection>> GetAllConnectionsAsync();
    }
}
