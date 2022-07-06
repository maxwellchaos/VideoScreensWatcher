using Microsoft.AspNetCore.Mvc;
using VideoScreensWatcher.Data;
using VideoScreensWatcher.Messages;
using VideoScreensWatcher.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VideoScreensWatcher.Controllers
{
    //Этот контроллер принимает данные с десктопов и кладет в базу данных
    [Route("api/[controller]")]
    [ApiController]
    public class FromDesktopController : ControllerBase
    {
        private readonly VideoScreensWatcherContext _context;

        public FromDesktopController(VideoScreensWatcherContext context)
        {
            _context = context;
        }

        // GET: api/<FromDesktopController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FromDesktopController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FromDesktopController>
        [HttpPost]
        public async Task Post(ComputerWork message)
        {
            //Что-то не то. не трогать базу данных
            if (message.ComputerId == "")
                return;

            //Ищем компьютер с нужным идентификатором
            var computer = _context.Computer?.FirstOrDefault(c => c.ComputerId == message.ComputerId);
            if (computer == null)
            {
                //Если нет такого компьютера в базе данных
                //то создаем его
                Computer newComputer = new Computer();
                newComputer.ComputerId = message.ComputerId;
                newComputer.ComputerName = message.ComputerName;
                newComputer.Timeout = message.Timeout;
                if (message.IsRunning)
                {
                    newComputer.Status = (int)Statuses.Running;
                }
                else
                {
                    newComputer.Status = (int)Statuses.Stoped;
                }
                _context.Computer.Add(newComputer);
                await _context.SaveChangesAsync();

                //Добавление записи в журнал
                OnlineLog log = new OnlineLog();
                log.ComputerId = newComputer.Id;
                log.OnlineDateTime = DateTime.Now;
                log.StatusChangedTo = newComputer.Status;
                _context.OnlineLog.Add(log);
            }
            else
            {
                //Если запись о компьютере есть

                //Если компьютер заблокирован
                if (computer.IsBlocked)
                {
                    //Выйти
                    return;
                }

                //Обновление данных
                computer.Timeout = message.Timeout;
                computer.ComputerName = message.ComputerName;
                if (message.IsRunning)
                {
                    computer.Status = (int)Statuses.Running;
                }
                else
                {
                    computer.Status = (int)Statuses.Stoped;
                }
                _context.Computer.Update(computer);
                //Получение последней записи в журнале
                OnlineLog? lastLog = _context.OnlineLog?.OrderBy(ol=>ol.OnlineDateTime).LastOrDefault(ol => ol.ComputerId == computer.Id);
                if (lastLog == null)
                {
                    //Если нет зписей в журнале - создать запись
                    var newLog = new OnlineLog();
                    newLog.ComputerId = computer.Id;
                    newLog.OnlineDateTime = DateTime.Now;
                    newLog.StatusChangedTo = computer.Status;
                    _context.OnlineLog.Add(newLog);
                }
                else
                {
                    //Если eсть зпись в журнале
                    if (lastLog.StatusChangedTo != computer.Status)
                    {
                        //Если статус изменился - создать новую запись
                        var newLog = new OnlineLog();
                        newLog.ComputerId = computer.Id;
                        newLog.OnlineDateTime = DateTime.Now;
                        newLog.StatusChangedTo = computer.Status;
                        _context.OnlineLog.Add(newLog);

                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        // PUT api/<FromDesktopController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FromDesktopController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
