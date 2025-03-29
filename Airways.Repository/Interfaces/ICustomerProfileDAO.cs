using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airways.Domain.EntitiesDB;

namespace Airways.Repository.Interfaces
{
    public interface ICustomerProfileDAO
    {
        Task<CustomerProfile> CreateCustomerProfileAsync(string userId, string firstName, string lastName, string email);
        Task<CustomerProfile> UpdateEmailAsync(int profileId, string newEmail);
        Task<CustomerProfile> GetCustomerProfileByUserIdAsync(string userId);
        Task<bool> DeleteCustomerProfileAsync(int profileId);
    }
}
