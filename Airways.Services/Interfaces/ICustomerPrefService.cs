// ICustomerPrefService.cs
using Airways.Domain.EntitiesDB;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface ICustomerPrefService
    {
        Task<CustomerPref> TrackCityVisitAsync(int profileId, int cityId);
    }
}