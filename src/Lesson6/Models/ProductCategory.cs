using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lesson6.Models
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }

        [Required]
        public string ProductCategoryName { get; set; }
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
