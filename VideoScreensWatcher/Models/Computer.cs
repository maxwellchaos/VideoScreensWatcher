using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VideoScreensWatcher.Data;

namespace VideoScreensWatcher.Models
{
    //Перерчисление статусов
    public enum Statuses
    {
        NotConnected,
        Stoped,
        Running,
        Blocked,
        Unblocked
    }
    public class Computer
    {
        /// <summary>
        /// Псевдоним компьютера, введенный пользователем в веб-интерфейсе
        /// </summary>
        public string? Alias { get; set; }

        /// <summary>
        /// Название компьютера, указанное в десктопной программе
        /// </summary>
        [Display(Name = "Название компьютера")]
        public string? ComputerName { get; set; }

        /// <summary>
        /// идентификатор компьютера в системе
        /// этот идентификатор генерит десктопная программа
        /// </summary>
        [Display(Name = "Индетификатор")]
        public string? ComputerId { get; set; }

        /// <summary>
        /// идентификатор компьютера в системе 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заблокирован ли компьютер, получать ли от него сообщения. 
        /// По умолчанию компьютер не заблокирован
        /// </summary>
        [Display(Name = "Блокировка")]
        public bool IsBlocked { get; set; } = false;

        /// <summary>
        /// Запущено ли сейчас видео на компьютере.
        /// </summary>
        [Display(Name = "Состояние")]
        public int Status { get; set; }

        /// <summary>
        /// Таймаут проверок на зависание, установленный на компьютере
        /// <b>по умолчанию:</b> 30 секунд
        /// </summary>
        [Display(Name = "Время обновления")]
        public int Timeout { get; set; } = 30;

        //Ссылка на журнал
        public List<OnlineLog>? Logs { get; set; }

        /// <summary>
        /// Дата последнего изменения статуса
        /// </summary>  

        public DateTime LastChangeStatusDate { get; set; }

        /// <summary>
        /// Обновляет статус компьютера в контексте и добавляет в журнал, не записывает в БД
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ComputerId"></param>
        /// <param name="NewStatus"></param>
        public static void UpdateStatus(VideoScreensWatcherContext context, Computer computer, Statuses NewStatus)
        {

            if (computer == null)
                return;
            if (computer.IsBlocked && NewStatus != Statuses.Unblocked)
                return;
            if (computer.Status != (int)NewStatus)
            {
                computer.LastChangeStatusDate = DateTime.Now;
                computer.Status = (int)NewStatus;
                if (computer.IsBlocked && NewStatus == Statuses.Unblocked)
                {
                    computer.IsBlocked = false;
                }
                if (!computer.IsBlocked && NewStatus == Statuses.Blocked)
                {
                    computer.IsBlocked = true;
                }
                context.Update(computer);

                OnlineLog log = new OnlineLog();
                log.ComputerId = computer.Id;
                log.OnlineDateTime = DateTime.Now;
                log.StatusChangedTo = (int)NewStatus;
                context.Add(log);
            }
        }
    }
}
