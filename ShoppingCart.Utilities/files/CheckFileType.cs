using Microsoft.AspNetCore.Http;
namespace ShoppingCart.Utilities.files
{
    public static class CheckFileType
    {
        public static bool IsImage(this IFormFile file)
        {
            if(file.ContentType.Contains("image")){
                return true;
            }
            return false;
        }
    }

}

