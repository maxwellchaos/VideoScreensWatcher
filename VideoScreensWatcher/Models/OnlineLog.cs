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
        [Display(Name = "Время изменениия статуса")]
        public DateTime OnlineDateTime { get; set; }

        /// <summary>
        /// статус десктопа изменен на
        /// </summary>
        [Display(Name = "Статус изменен на")]
        public int StatusChangedTo { get; set; }

    }
}
