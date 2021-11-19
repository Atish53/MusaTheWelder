using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusaTheWelder.Models
{
    public class ProductCategory
    {
        [Key]
        public int ProductCatergoryId { get; set; }

        public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}