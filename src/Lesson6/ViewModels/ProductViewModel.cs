using Lesson6.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson6.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        [DataType(DataType.Currency), Range(0.1, 10000.0, ErrorMessage = "Hold your horses!"), Required(ErrorMessage ="The price field is required")]
        public decimal Price { get; set; }
        public ProductCategory ProductCategory { get; set; }
        [Display(Name = "Product name"), Required(ErrorMessage = "The product name field is required.")]
        public string ProductName { get; set; }
        [Display(Name ="Product Description"),Required(ErrorMessage ="The product description field is requried.")]
        public string ProductDescription { get; set; }
        public string PictureURL { get; set; }
   
    }
}
