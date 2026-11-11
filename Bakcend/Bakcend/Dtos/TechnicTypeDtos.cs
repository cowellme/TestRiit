using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class TechnicTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CreateTechnicTypeRequest
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
    }
}
