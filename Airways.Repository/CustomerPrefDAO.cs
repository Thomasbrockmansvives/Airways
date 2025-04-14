using Airways.Domain.DataDB;
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Airways.Repository
{
    public class CustomerPrefDAO : ICustomerPrefDAO
    {
        private readonly FlightBookingDBContext _context;

        public CustomerPrefDAO(FlightBookingDBContext context)
        {
            _context = context;
        }

        public async Task<CustomerPref> GetCustomerPrefAsync(int profileId, int cityId)
        {
            return await _context.CustomerPrefs
                .FirstOrDefaultAsync(cp => cp.ProfileId == profileId && cp.CityId == cityId);
        }

        public async Task<CustomerPref> CreateCustomerPrefAsync(int profileId, int cityId, int visitCount)
        {
            int newPrefId = await GenerateUniquePrefIdAsync();

            var customerPref = new CustomerPref
            {
                PrefId = newPrefId,
                ProfileId = profileId,
                CityId = cityId,
                VisitCount = visitCount
            };

            _context.CustomerPrefs.Add(customerPref);
            await _context.SaveChangesAsync();

            return customerPref;
        }

        public async Task<CustomerPref> IncrementVisitCountAsync(int prefId)
        {
            var customerPref = await _context.CustomerPrefs.FindAsync(prefId);
            if (customerPref != null)
            {
                customerPref.VisitCount += 1;
                await _context.SaveChangesAsync();
            }
            return customerPref;
        }

        public async Task<int> GenerateUniquePrefIdAsync()
        {
            int maxId = await _context.CustomerPrefs.MaxAsync(cp => (int?)cp.PrefId) ?? 0;
            return maxId + 1;
        }
    }
}