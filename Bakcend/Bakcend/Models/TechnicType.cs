using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("TechnicTypes")]
public class TechnicType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    private string _name = string.Empty;

    [Required]
    [MaxLength(128)]
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("Наименование не может быть пустым");

            if (value.Length > 128)
                throw new Exception("Наименование не должно превышать 128 символов");

            _name = value;
        }
    }
}