using Lesson6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson6.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public decimal Price { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string PictureURL { get; set; }
   
    }
}
