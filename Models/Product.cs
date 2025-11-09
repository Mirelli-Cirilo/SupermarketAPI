using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SupermarketAPI.Models
{
    public class Product
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public CategoryProduct Category {get; set;}

        [Required]
        public int Stock {  get; set; }

        [Required]
        public DateTime RegistrationDate {  get; set; }
        
        

    }
}
