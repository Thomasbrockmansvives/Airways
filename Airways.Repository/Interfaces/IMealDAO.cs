using Airways.Domain.EntitiesDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Repository.Interfaces
{
    public interface IMealDAO
    {
        Task<IEnumerable<Meal>> GetAllMealsAsync();
        Task<Meal> GetMealByIdAsync(int id);
        Task<IEnumerable<Meal>> GetMealsByCityIdAsync(int cityId);
    }
}