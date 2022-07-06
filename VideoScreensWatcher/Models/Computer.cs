using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
}
}
