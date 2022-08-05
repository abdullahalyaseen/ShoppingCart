using System;
namespace ShoppingCart.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        public string Name { get; set; }



        public virtual ICollection<Product> Products { get; set; }
    }
}

