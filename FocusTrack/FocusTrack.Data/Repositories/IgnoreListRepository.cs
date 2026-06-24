using FocusTrack.Data.Context;
using FocusTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack.Data.Repositories
{
    public class IgnoreListRepository
    {
        public async Task<List<IgnoreListItem>> GetAllAsync()
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            return await context.IgnoreListItems
                .OrderBy(item => item.ApplicationName)
                .ToListAsync();
        }

        public async Task AddAsync(string applicationName)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            bool alreadyExists = await context.IgnoreListItems
                .AnyAsync(item => item.ApplicationName.ToLower() == applicationName.ToLower());

            if (alreadyExists)
            {
                return;
            }

            IgnoreListItem item = new IgnoreListItem
            {
                ApplicationName = applicationName
            };

            await context.IgnoreListItems.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            IgnoreListItem? item = await context.IgnoreListItems.FindAsync(id);

            if (item == null)
            {
                return;
            }

            context.IgnoreListItems.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsIgnoredAsync(string applicationName)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            return await context.IgnoreListItems
                .AnyAsync(item => item.ApplicationName.ToLower() == applicationName.ToLower());
        }
    }
}