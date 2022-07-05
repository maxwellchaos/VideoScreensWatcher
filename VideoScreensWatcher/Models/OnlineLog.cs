using System.ComponentModel.DataAnnotations;

namespace VideoScreensWatcher.Models
{
    //Журнал статусов компьютеров
    //Содержит компьютер, время изменения статуса и сам статус
    public class OnlineLog
    {
        /// <summary>
        /// Первыичный ключ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на компьютер
        /// </summary>
        public Computer? Computer { get; set; }
        [Required]
        public int ComputerId { get; set; }


        /// <summary>
        /// Дата и время пришедшего сообщения
        /// </summary>
        public DateTime OnlineDateTime { get; set; }

        /// <summary>
        /// статус десктопа изменен на
        /// </summary>
        public Statuses StatusChangedTo { get; set; }

    }
}
