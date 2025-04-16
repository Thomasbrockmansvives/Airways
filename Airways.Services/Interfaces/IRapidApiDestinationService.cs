using Airways.Domain.EntitiesDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface IRapidApiDestinationService
    {
        Task<List<RapidApiDestination>> SearchRapidApiDestinationsByQueryAsync(string cityname);
    }
}