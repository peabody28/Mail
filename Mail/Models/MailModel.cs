using System.ComponentModel.DataAnnotations;

namespace Mail.Models
{
    public class MailModel
    {
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Не указан получатель")]
        public string Target { get; set; }
        public int TargetId { get; set; }

        [Required(ErrorMessage = "Не указан текст")]
        public string Text { get; set; }
    }
}
