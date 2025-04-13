
using Airways.Domain.DataDB;
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airways.Repository
{
    public class MealDAO : IMealDAO
    {
        private readonly FlightBookingDBContext _context;

        public MealDAO(FlightBookingDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meal>> GetAllMealsAsync()
        {
            return await _context.Meals
                .Include(m => m.City)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Meal> GetMealByIdAsync(int id)
        {
            return await _context.Meals
                .Include(m => m.City)
                .FirstOrDefaultAsync(m => m.MealId == id);
        }

        public async Task<IEnumerable<Meal>> GetMealsByCityIdAsync(int cityId)
        {
            return await _context.Meals
                .Where(m => m.CityId == cityId || m.CityId == null)
                .Include(m => m.City)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }
    }
}