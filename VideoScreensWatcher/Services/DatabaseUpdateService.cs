using VideoScreensWatcher.Data;
using VideoScreensWatcher.Models;

namespace VideoScreensWatcher.Services
{
    public class DatabaseUpdateService
    {
        readonly int interval = 3000;
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
                //Это можно оптимизировать написав один запрос к базе данных без цикла
                //но сделаю это позже
                foreach (Computer computer in _context.Computer)
                {
                    //Если компьютер не заблокирован и его статус не NotConnected
                    if (!computer.IsBlocked && computer.Status != Statuses.NotConnected)
                    {
                        var log = _context.OnlineLog.LastOrDefault(log => log.ComputerId == computer.Id);
                        //Если интервал превысил четырехкратный интервал обновления
                        if (log != null
                            && (log.OnlineDateTime - DateTime.Now).Seconds > computer.Timeout * 4)
                        {
                            computer.Status = Statuses.NotConnected;
                            _context.Computer.Update(computer);

                            //Если статус изменился - создать новую запись
                            var newLog = new OnlineLog();
                            newLog.ComputerId = computer.Id;
                            newLog.OnlineDateTime = DateTime.Now;
                            newLog.StatusChangedTo = computer.Status;
                            _context.Add(newLog);
                            hasStatusesUpdate = true;
                        }
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
