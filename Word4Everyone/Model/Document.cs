using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Word4Everyone.Model
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название документа")]
        [StringLength(100, MinimumLength = 1, 
            ErrorMessage = "Название должно содержать от 1 до 100 знаков")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите текст")]
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
