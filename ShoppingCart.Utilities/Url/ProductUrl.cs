using System;
using Microsoft.AspNetCore.Http;
namespace ShoppingCart.Utilities.Url
{
    public static class ProductUrl{
        public static string GenerateProductUrl(string ProductTitle, int ProductId){
            string TitleId = string.Concat(ProductTitle," ",ProductId.ToString());
            return TitleId.Replace(' ','_');

        }

        public static int GetProductId(string ProductIdentifier){
            int GeneratedId;
            try{
                int id = int.Parse(ProductIdentifier.Split('_').Last());
                GeneratedId = id;
            }catch{
                GeneratedId = 0;
            }
            return GeneratedId;
        }
    }
}