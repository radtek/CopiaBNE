using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.StorageManager
{
    public interface IFileManager
    {
        bool Save(String objectPath, byte[] byteArray);
        bool Delete(String objectPath);
        bool Exists(String objectPath);
        byte[] GetBytes(String objectPath);
        IEnumerable<string> List();
        int Count();

        bool Copy(String objectOriginPath, String objectDestinationPath);
        bool Move(String objectOriginPath, String objectDestinationPath);
        String GetUrl(String objectPath);

        Task<bool> ExistsAsync(String objectPath);
    }
}
