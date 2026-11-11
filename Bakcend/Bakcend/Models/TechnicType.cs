using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("TechnicTypes")]
public class TechnicType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(128, ErrorMessage = "Наименование не должно превышать 128 символов")]
    public string Name { get; set; } = string.Empty;
}