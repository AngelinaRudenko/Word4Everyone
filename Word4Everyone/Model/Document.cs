using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Word4Everyone.Model
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название документа.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Название документа должно содержать от {2} до {1} символов.")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Документ должен содержать текст.")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string UserId { get; set; }
        //public virtual User User { get; set; }

        public void ChangeDateCreated() => DateCreated = DateTime.UtcNow;

        public void ChangeDateModified() => DateModified = DateTime.UtcNow;
    }
}
