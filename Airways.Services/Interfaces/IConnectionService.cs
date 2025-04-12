using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airways.Domain.EntitiesDB;

namespace Airways.Services.Interfaces
{
    public interface IConnectionService
    {
        Task<Connection> GetConnectionByCitiesAsync(int departureCityId, int arrivalCityId);
        Task<IEnumerable<Connection>> GetAllConnectionsAsync();
    }
}
