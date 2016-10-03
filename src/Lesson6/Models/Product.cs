using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Lesson6.Models
{
    public class Product
    {

        public int ProductId { get; set; }

        //[Display(Name ="Product name"), Required(ErrorMessage ="The product name field is required.")]        
        //public string ProductName { get; set; }

        [DataType(DataType.Currency), Range(0.1, 10000.0, ErrorMessage = "Hold your horses!")]
        public decimal Price { get; set; }

        //[Display(Name ="Product Description"),Required(ErrorMessage ="The product description field is requried.")]
        //public string ProductDescription { get; set; }


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
