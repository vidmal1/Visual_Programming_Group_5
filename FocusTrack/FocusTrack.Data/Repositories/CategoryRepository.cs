using FocusTrack.Data.Context;
using FocusTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack.Data.Repositories
{
    public class CategoryRepository
    {
        public async Task<List<Category>> GetAllAsync()
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            return await context.Categories
                .OrderBy(category => category.Id)
                .ToListAsync();
        }
    }
}