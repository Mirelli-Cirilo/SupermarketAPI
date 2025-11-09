using SupermarketAPI.Models;

namespace SupermarketAPI.DTOs
{
    public class ResponseProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        public int Stock { get; set; }

        public DateTime RegistrationDate { get; set; }

    }
}
