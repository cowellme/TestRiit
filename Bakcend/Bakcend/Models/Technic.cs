using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Backend.Models
{
    [Table("Equipment")]
    public class Technic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string _accountNumber = string.Empty;

        [Required]
        [MaxLength(32)]
        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Учетный номер не может быть пустым");

                if (value.Length > 32)
                    throw new Exception("Учетный номер не должен превышать 32 символа");

                if (!Regex.IsMatch(value, @"^[a-zA-Z0-9]+$"))
                    throw new Exception("Учетный номер должен содержать только цифры и латинские буквы");

                _accountNumber = value;
            }
        }

        private string _name = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Наименование не может быть пустым");

                if (value.Length > 256)
                    throw new Exception("Наименование не должно превышать 256 символов");

                _name = value;
            }
        }

        [Required]
        public int TechnicTypeId { get; set; }

        private int _roomNumber;

        [Required]
        [Range(1, 1000)]
        public int RoomNumber
        {
            get => _roomNumber;
            set
            {
                if (value < 1 || value > 1000)
                    throw new Exception("Комната размещения должна быть от 1 до 1000");

                _roomNumber = value;
            }
        }
    }
}