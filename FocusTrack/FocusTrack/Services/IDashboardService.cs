namespace FocusTrack.Services
{
    public interface IDashboardService
    {
        Task<DashboardReport> GetDailyReportAsync(DateTime date);
    }
}
