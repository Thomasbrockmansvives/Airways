using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;
using System.Threading.Tasks;

namespace Airways.Services
{
    public class CustomerPrefService : ICustomerPrefService
    {
        private readonly ICustomerPrefDAO _customerPrefDAO;

        public CustomerPrefService(ICustomerPrefDAO customerPrefDAO)
        {
            _customerPrefDAO = customerPrefDAO;
        }

        public async Task<CustomerPref> TrackCityVisitAsync(int profileId, int cityId)
        {
            // Check if a preference record already exists
            var existingPref = await _customerPrefDAO.GetCustomerPrefAsync(profileId, cityId);

            if (existingPref != null)
            {
                // Increment visit count for existing preference
                return await _customerPrefDAO.IncrementVisitCountAsync(existingPref.PrefId);
            }
            else
            {
                // Create new preference with visit count 1
                return await _customerPrefDAO.CreateCustomerPrefAsync(profileId, cityId, 1);
            }
        }

        // Add this method to the CustomerPrefService.cs file
        public async Task<IEnumerable<CustomerPref>> GetCustomerPrefsByProfileIdAsync(int profileId)
        {
            return await _customerPrefDAO.GetCustomerPrefsByProfileIdAsync(profileId);
        }
    }
}