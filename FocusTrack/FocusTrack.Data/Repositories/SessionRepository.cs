using FocusTrack.Data.Context;
using FocusTrack.Data.Entities;
using FocusTrack.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack.Data.Repositories
{
    public class SessionRepository : IRepository<AppSession>
    {
        public async Task<List<AppSession>> GetAllAsync()
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            return await context.AppSessions
                .Include(session => session.Category)
                .OrderByDescending(session => session.StartTime)
                .ToListAsync();
        }

        public async Task<AppSession?> GetByIdAsync(int id)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            return await context.AppSessions
                .Include(session => session.Category)
                .FirstOrDefaultAsync(session => session.Id == id);
        }

        public async Task AddAsync(AppSession entity)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            await context.AppSessions.AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppSession entity)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            context.AppSessions.Update(entity);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppSession entity)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            context.AppSessions.Remove(entity);

            await context.SaveChangesAsync();
        }

        public async Task<List<AppSession>> GetByDateAsync(DateTime date)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            return await context.AppSessions
                .Include(session => session.Category)
                .Where(session => session.StartTime.Date == date.Date)
                .OrderByDescending(session => session.StartTime)
                .ToListAsync();
        }

        public async Task EndSessionAsync(int sessionId, DateTime endTime)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            AppSession? session = await context.AppSessions.FindAsync(sessionId);

            if (session == null)
            {
                return;
            }

            session.EndTime = endTime;
            session.DurationSeconds = (int)(endTime - session.StartTime).TotalSeconds;

            await context.SaveChangesAsync();
        }
    }
}