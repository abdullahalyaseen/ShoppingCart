using System;
namespace ShoppingCart.Models
{
    public class Image
    {
        public int ImageId { get; set; }

        public string Url { get; set; }

        public int ProductId { get; set; }


        public virtual Product Product { get; set; }
    }
}

