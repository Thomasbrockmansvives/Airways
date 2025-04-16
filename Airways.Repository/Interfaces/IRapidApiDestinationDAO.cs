using Airways.Domain.EntitiesDB;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Airways.Repository.Interfaces
{
    public interface IRapidApiDestinationDAO
    {
        Task<List<RapidApiDestination>> SearchRapidApiDestinationsByQueryAsync(string cityname);
    }
}