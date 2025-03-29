using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;


namespace Airways.Services.Interfaces
{
    public interface ICustomerProfileService
    {

        Task<CustomerProfile> RegisterNewCustomerProfileAsync(string userId, string firstName, string lastName, string email);
        Task<CustomerProfile> UpdateCustomerProfileAsync(int profileId, string firstName = null, string lastName = null, string email = null);
        Task<bool> DeleteCustomerProfileAsync(int profileId);
        Task<CustomerProfile> GetCustomerProfileByUserIdAsync(string userId);

    }
}
