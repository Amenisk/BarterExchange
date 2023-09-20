using BarterExchange.Data.Classes;
using MongoDB.Driver;

namespace BarterExchange.Data.Services
{
    public class FileSystemService
    {
        public async Task UploadImage(Stream fs, string name)
        {
           await Database.UploadImageToDbAsync(fs, name);
        }

        public string DownloadImage(string name)
        {
            return Database.DownloadToLocal(name);
        }
    }
}
