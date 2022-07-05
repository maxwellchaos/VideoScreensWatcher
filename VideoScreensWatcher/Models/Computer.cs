using System.ComponentModel.DataAnnotations.Schema;

namespace VideoScreensWatcher.Models
{
    //Перерчисление статусов
    public enum Statuses
    {
        NotConnected,
        Stoped,
        Running,
        Blocked
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
        public string? ComputerName { get; set; }

        /// <summary>
        /// идентификатор компьютера в системе
        /// этот идентификатор генерит десктопная программа
        /// </summary>
        public string? ComputerId { get; set; }

        /// <summary>
        /// идентификатор компьютера в системе 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заблокирован ли компьютер, получать ли от него сообщения
        /// по умолчанию компьютер не заблокирован
        /// </summary>
        public bool IsBlocked { get; set; } = false;

        /// <summary>
        /// Запущено ли сейчас видео на компьютере
        /// это поле не хранится в базе, оно вычисляется на основе приходящих данных
        /// </summary>
        [NotMapped]
        public Statuses Status;

        /// <summary>
        /// Таймаут проверок на зависание, установленный на компьютере
        /// это поле не хранится в базе, оно вычисляется на основе приходящих данных
        /// <b>по умолчанию:</b> 30 секунд
        /// </summary>
        [NotMapped]
        public int Timeout = 30;
    }
}
