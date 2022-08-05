using Microsoft.AspNetCore.Hosting;
namespace ShoppingCart.Utilities.files
{
    public class FileDelete : IDisposable
    {
        public FileDelete(IWebHostEnvironment env,string filePath){

            _fileInfo = new FileInfo(Path.Combine(env.WebRootPath.ToString(),filePath));
        }
        private FileInfo _fileInfo;
        private bool isDisposed = false;

        public bool Delete(){
            _fileInfo.Delete();
            if(_fileInfo.Exists){
                return false;
            }
            return true;
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