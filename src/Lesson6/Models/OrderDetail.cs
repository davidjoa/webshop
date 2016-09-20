using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson6.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int count { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }


    }
}
