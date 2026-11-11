using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class TechnicDto
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int TechnicTypeId { get; set; }
    public int RoomNumber { get; set; }
}

public class CreateTechnicRequest
{
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

public class UpdateTechnicRequest : CreateTechnicRequest
{
    // id приходит в URL (/api/technic/{id}), в теле он не нужен
}