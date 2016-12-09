using System;
using System.ComponentModel.DataAnnotations;

namespace TasksPersons.Models
{
    public class Task
    {
        //TODO: дополнительные DataAnnotation для "красивого" именования и вывода сообщений об ошибках
        public int TaskId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime BeginDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public TaskState State { get; set; }
        [Required]
        public int PersonId { get; set; }
    }

    public enum TaskState
    {
        Plan, Process, Completed, Hold
    };
}
