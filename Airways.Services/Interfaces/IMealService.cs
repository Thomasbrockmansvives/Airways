using Airways.Domain.EntitiesDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface IMealService
    {
        Task<IEnumerable<Meal>> GetAllMealsAsync();
        Task<Meal> GetMealByIdAsync(int id);

        // get all standard meals plus the local meal
        Task<IEnumerable<Meal>> GetMealsByCityIdAsync(int cityId);
    }
}