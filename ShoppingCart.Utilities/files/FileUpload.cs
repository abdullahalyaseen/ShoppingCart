using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace ShoppingCart.Utilities.files
{
    public class FileUpload : IDisposable
    {
        public FileUpload(IWebHostEnvironment env,IFormFile File,string path)
        {
            _env = env;
            _InRootPath = path;
            _File = File;
        }
        private bool isDisposed = false;

        private string _InRootPath;
        private IFormFile _File;
        private readonly IWebHostEnvironment _env;


        public string Upload()
        {
            string uploadPath = Path.Combine(_env.WebRootPath, _InRootPath);
            _File.CopyTo(new FileStream(uploadPath, FileMode.Create));
            return Path.Combine(uploadPath);
        }


        public void Dispose(){
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing){
            if(!isDisposed){
                if(disposing){

                }
            }
            isDisposed = true;
        }
    }
}