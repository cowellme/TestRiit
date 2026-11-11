using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Equipment")]
    public class Technic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Учётный номер должен содержать только цифры и латинские буквы")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;


        [Required]
        public int TechnicTypeId { get; set; }

        [Range(1, 1000, ErrorMessage = "Комната размещения должна быть от 1 до 1000")]
        public int RoomNumber { get; set; }
    }
}