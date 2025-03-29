using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airways.Domain.DataDB;
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Airways.Repository
{
    public class CustomerProfileDAO : ICustomerProfileDAO
    {
        private readonly FlightBookingDBContext _flightBookingDBContext;

        public CustomerProfileDAO(FlightBookingDBContext flightBookingDBContext)
        {
            _flightBookingDBContext = flightBookingDBContext;
        }

        public async Task<CustomerProfile> CreateCustomerProfileAsync(string userId, string firstName, string lastName, string email)
        {
            int newProfileId = await GenerateUniqueProfileIdAsync();

            var customerProfile = new CustomerProfile
            {
                ProfileId = newProfileId,
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            _flightBookingDBContext.CustomerProfiles.Add(customerProfile);
            await _flightBookingDBContext.SaveChangesAsync();

            return customerProfile;
        }

        private async Task<int> GenerateUniqueProfileIdAsync()
        {
            int maxId = await _flightBookingDBContext.CustomerProfiles.MaxAsync(cp => (int?)cp.ProfileId) ?? 0; ;
            return maxId + 1;
        }

        public async Task<CustomerProfile> GetCustomerProfileByUserIdAsync(string userId)
        {
            return await _flightBookingDBContext.CustomerProfiles.FirstOrDefaultAsync(cp => cp.UserId == userId);
        }

        public async Task<CustomerProfile> UpdateEmailAsync(int profileId, string newEmail)
        {
            var customerProfile = await _flightBookingDBContext.CustomerProfiles.FirstOrDefaultAsync(cp => cp.ProfileId == profileId);

            if (customerProfile == null)
            {
                throw new ArgumentException("Customer profile not found");
            }

            customerProfile.Email = newEmail;
            await _flightBookingDBContext.SaveChangesAsync();

            return customerProfile;
        }

        public async Task<bool> DeleteCustomerProfileAsync(int profileId)
        {
            try
            {
                var customerProfile = await _flightBookingDBContext.CustomerProfiles.FirstOrDefaultAsync(cp => cp.ProfileId == profileId);

                if (customerProfile == null)
                {
                    return false;
                }

                var customerPrefs = await _flightBookingDBContext.CustomerPrefs
                    .Where(cp => cp.ProfileId == profileId)
                    .ToListAsync();
                _flightBookingDBContext.CustomerPrefs.RemoveRange(customerPrefs);

                var bookings = await _flightBookingDBContext.Bookings
                    .Where(b => b.CustomerId == profileId)
                    .ToListAsync();
                _flightBookingDBContext.Bookings.RemoveRange(bookings);

                _flightBookingDBContext.CustomerProfiles.Remove(customerProfile);

                await _flightBookingDBContext.SaveChangesAsync();

                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting customer profile: {ex.Message}");
                return false;
            }
        }
    }
}
