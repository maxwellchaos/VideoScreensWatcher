using VideoScreensWatcher.Data;
using VideoScreensWatcher.Models;

namespace VideoScreensWatcher.Services
{
    public class DatabaseUpdateService
    {
        readonly int interval = 30000;
        Timer timer;
        private IServiceScopeFactory _serviceScopeFactory;
        
        bool hasStatusesUpdate = false;
        public DatabaseUpdateService(IServiceScopeFactory pServiceScopeFactory)
        {
            _serviceScopeFactory = pServiceScopeFactory;
            timer = new Timer(new TimerCallback(UpdateDatabase), null, 0, interval);
        }
        private void UpdateDatabase(object? obj)
        {
            using (var _context = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<VideoScreensWatcherContext>())
            {
                var AllComputers = _context.Computer.Where(c => c.IsBlocked == false && c.Status != (int)Statuses.NotConnected);
                foreach (var computer in AllComputers)
                {
                    if ((DateTime.Now - computer.LastChangeStatusDate).Seconds > Math.Max(computer.Timeout * 4, interval / 1000))
                    {
                        Computer.UpdateStatus(_context, computer, Statuses.NotConnected);
                    }
                }
                _context.SaveChanges();
            }
            //Console.WriteLine("DB Updated");
        }

        public bool HasNewStatuses()
        {
            return hasStatusesUpdate;
        }
    }
}
