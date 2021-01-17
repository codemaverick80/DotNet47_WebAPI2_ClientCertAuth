using System.ComponentModel.DataAnnotations;

namespace WebAPI2.Models
{
    public class ProductPostRequest
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
