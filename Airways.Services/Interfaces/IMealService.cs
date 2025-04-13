
using Airways.Domain.EntitiesDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface IMealService
    {
        Task<IEnumerable<Meal>> GetAllMealsAsync();
        Task<Meal> GetMealByIdAsync(int id);
        Task<IEnumerable<Meal>> GetMealsByCityIdAsync(int cityId);
    }
}