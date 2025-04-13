
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services
{
    public class MealService : IMealService
    {
        private readonly IMealDAO _mealDAO;

        public MealService(IMealDAO mealDAO)
        {
            _mealDAO = mealDAO;
        }

        public async Task<IEnumerable<Meal>> GetAllMealsAsync()
        {
            return await _mealDAO.GetAllMealsAsync();
        }

        public async Task<Meal> GetMealByIdAsync(int id)
        {
            return await _mealDAO.GetMealByIdAsync(id);
        }

        public async Task<IEnumerable<Meal>> GetMealsByCityIdAsync(int cityId)
        {
            return await _mealDAO.GetMealsByCityIdAsync(cityId);
        }
    }
}