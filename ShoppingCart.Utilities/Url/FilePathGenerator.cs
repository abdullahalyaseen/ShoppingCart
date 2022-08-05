using Microsoft.AspNetCore.Http;
namespace ShoppingCart.Utilities.Url
{
    public class FilePathGenerator
    {
        public static string GeneraterPath(IFormFile file,string? inRootPath = null,string? prefix = null, string? suffix = null,bool keepName = false,Func<IFormFile,bool>? filter = null){
            bool IsAccesptable = filter == null ? true : filter(file);
            if(IsAccesptable){
                string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                string FileName = keepName ? FileNameWithoutExtension : Guid.NewGuid().ToString();
                string Extension = Path.GetExtension(file.FileName);
                string FullFileName = (prefix == null ? "" : (prefix + "-")) + FileName + (suffix == null ? "" : ("-"+suffix)) + Extension;
                string FilePath = Path.Combine(inRootPath == null ? "" : inRootPath,FullFileName);
                return FilePath;
            }

            return "dropped";
        }
    }
}