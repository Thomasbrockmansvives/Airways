using Airways.Domain.EntitiesDB;
using System.Threading.Tasks;

namespace Airways.Repository.Interfaces
{
    public interface ICustomerPrefDAO
    {
        Task<CustomerPref> GetCustomerPrefAsync(int profileId, int cityId);
        Task<CustomerPref> CreateCustomerPrefAsync(int profileId, int cityId, int visitCount);
        Task<CustomerPref> IncrementVisitCountAsync(int prefId);
        Task<int> GenerateUniquePrefIdAsync();

        // Add this method to the ICustomerPrefDAO.cs file
        Task<IEnumerable<CustomerPref>> GetCustomerPrefsByProfileIdAsync(int profileId);
    }
}