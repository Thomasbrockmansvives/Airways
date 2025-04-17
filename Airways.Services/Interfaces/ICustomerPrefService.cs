using Airways.Domain.EntitiesDB;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface ICustomerPrefService
    {
        /* check whether this customer (profile id already has a record for this city
         * if so, increment it's visitcount by 1, if not, create a record
         */
        Task<CustomerPref> TrackCityVisitAsync(int profileId, int cityId);

        // Add this method to the ICustomerPrefService.cs file
        Task<IEnumerable<CustomerPref>> GetCustomerPrefsByProfileIdAsync(int profileId);
    }
}