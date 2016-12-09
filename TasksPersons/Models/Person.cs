using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TasksPersons.Models
{
    public class Person
    {
        //TODO: дополнительные DataAnnotation для "красивого" именования и вывода сообщений об ошибках
        public int PersonId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string MiddleName { get; set; }

        public string FullName { get { return Surname + " " + Name + " " + (MiddleName ?? ""); } }
    }
}