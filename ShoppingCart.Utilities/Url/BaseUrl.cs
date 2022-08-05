using Microsoft.AspNetCore.Http;
namespace ShoppingCart.Utilities.Url
{
    public static class BaseUrl
    {
        public static string GetBaseUrl(HttpContext Context){
            return Context.Request.Scheme + "://" + Context.Request.Host.Value.ToString() + "/";
        }
    }
}