using System.ComponentModel.DataAnnotations;
namespace Demo3DAPI.DTOs
{
    public class CreateCategoryDTO
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? CategoryDescription { get; set; }
    }

    public class UpdateCategoryDTO
    {
        [StringLength(100)]
        public string? CategoryName { get; set; }

        [StringLength(500)]
        public string? CategoryDescription { get; set; }
    }
}
