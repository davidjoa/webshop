using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Lesson6.Models
{
    public class Product
    {

        public int ProductId { get; set; }        
    
        public decimal Price { get; set; }
        
        // Foreign key
        public int ProductCategoryId { get; set; }

        public string PictureURL { get; set; }
        public ProductCategory ProductCategory {get; set;}

        [Required]

        public virtual ICollection<ProductTranslation> Translations { get; set; }

    }

    public class ProductTranslation
    {
        //Here we store properties changing with language
        public int ProductId {get; set;}

        public string Language { get; set; }   
         
        [Required]
        public string ProductName { get; set; }      
         
        [Required]
        public string ProductDescription { get; set; }
        
    }
}
