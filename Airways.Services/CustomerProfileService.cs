using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;

namespace Airways.Services

    // when a new user is created, also a customerprofile should be created and synced to it
{
    public class CustomerProfileService : ICustomerProfileService
    {

        private readonly ICustomerProfileDAO _customerProfileDAO;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerProfileService(ICustomerProfileDAO customerProfileDAO, UserManager<IdentityUser> userManager)
        {
            _customerProfileDAO = customerProfileDAO;
            _userManager = userManager;
        }

        public async Task<CustomerProfile> RegisterNewCustomerProfileAsync(string userId, string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be empty");
            }

            var existingProfile = await _customerProfileDAO.GetCustomerProfileByUserIdAsync(userId);
            if (existingProfile != null)
            {
                throw new InvalidOperationException("Profile already exists for this user");
            }

            return await _customerProfileDAO.CreateCustomerProfileAsync(userId, firstName, lastName, email);
        }

        public async Task<CustomerProfile> UpdateCustomerProfileAsync(int profileId, string firstName = null, string lastName = null, string email = null)
        {
            var profile = await _customerProfileDAO.GetCustomerProfileByUserIdAsync((await _userManager.FindByIdAsync(profileId.ToString())).Id);

            if (profile == null)
            {
                throw new ArgumentException("Profile not found");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                await _customerProfileDAO.UpdateEmailAsync(profileId, email);
            }

            return profile;
        }

        public async Task<bool> DeleteCustomerProfileAsync(int profileId)
        {
            var profile = await _customerProfileDAO.GetCustomerProfileByUserIdAsync((await _userManager.FindByIdAsync(profileId.ToString())).Id);

            if (profile == null)
            {
                throw new ArgumentException("Profile not found");
            }

            return await _customerProfileDAO.DeleteCustomerProfileAsync(profileId);
        }

        public async Task<CustomerProfile> GetCustomerProfileByUserIdAsync(string userId)
        {
            return await _customerProfileDAO.GetCustomerProfileByUserIdAsync(userId);
        }
    }
}
