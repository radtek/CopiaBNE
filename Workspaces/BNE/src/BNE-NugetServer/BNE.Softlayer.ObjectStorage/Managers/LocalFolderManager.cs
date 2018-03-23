using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BNE.StorageManager.Config;

namespace BNE.StorageManager.Managers
{
    public class LocalFolderManager : IFileManager, IFileConfig
    {
        private string _folder;
        private string _publicUrl;
        private Regex _regexToPath;

        public void LoadConfig(StorageConfiguration storage)
        {
            try
            {
                StoragesConfigSection config = System.Configuration.ConfigurationManager.GetSection("StoragesConfig") as StoragesConfigSection;
                string folder = storage.Path;

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                _folder = storage.Path;
                _publicUrl = storage.PublicUrl;

                if (!String.IsNullOrEmpty(storage.RegexPath))
                {
                    _regexToPath = new Regex(storage.RegexPath);
                }
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Ocorreu uma falha ao tentar configurar o Object Storage", ex);
            }
        }

        public bool Save(String objectPath, byte[] byteArray)
        {
            objectPath = ResolvePath(objectPath);
            File.WriteAllBytes(objectPath, byteArray);
            return true;
        }

        public bool Delete(String objectPath)
        {
            objectPath = ResolvePath(objectPath);
            File.Delete(objectPath);
            return true;
        }
        public bool Exists(String objectPath)
        {
            objectPath = ResolvePath(objectPath);
            return File.Exists(objectPath);
        }
        public async Task<bool> ExistsAsync(String objectPath)
        {
            objectPath = ResolvePath(objectPath);
            return await Task.FromResult(File.Exists(objectPath));
        }

        public byte[] GetBytes(String objectPath)
        {
            objectPath = ResolvePath(objectPath);
            return System.IO.File.ReadAllBytes(objectPath);
        }

        public bool Copy(String objectOriginPath, String objectDestinationPath)
        {
            objectOriginPath = ResolvePath(objectOriginPath);
            objectDestinationPath = ResolvePath(objectOriginPath);

            File.Copy(objectOriginPath, objectDestinationPath);
            return true;
        }

        public bool Move(String objectOriginPath, String objectDestinationPath)
        {
            objectOriginPath = ResolvePath(objectOriginPath);
            objectDestinationPath = ResolvePath(objectDestinationPath);

            if (File.Exists(objectDestinationPath))
                File.Delete(objectDestinationPath);

            File.Move(objectOriginPath, objectDestinationPath);
            return true;
        }

        public String GetUrl(String objectPath)
        {
            objectPath = ResolvePath(objectPath);

            if (String.IsNullOrEmpty(_publicUrl))
                throw new Exception("PublicUrl não definida para a folder");

            return Helper.CombineUrl(_publicUrl, objectPath.Replace('\\', '/'));
        }

        /// <summary>
        /// Implementa o tratamento ao tratamento da url incluindo as pastas a serem criadas com os grupos da Regex
        /// </summary>
        /// <param name="objectPath">Caminho do arquivo</param>
        /// <returns></returns>
        public String ResolvePath(String objectPath)
        {
            if (_regexToPath == null)
                return Path.Combine(_folder, objectPath);

            String folderPath;
            String filePath = Path.Combine(_folder, Helper.ResolvePath(objectPath, '\\', _regexToPath, out folderPath));
            folderPath =  Path.Combine(_folder, folderPath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return filePath;
        }

        public IEnumerable<string> List()
        {
            var dirPath = ResolvePath(string.Empty);

            string[] allfiles = System.IO.Directory.GetFiles(dirPath, "*.*", System.IO.SearchOption.AllDirectories);

            return allfiles.Select(fp => fp.Replace(dirPath + "\\", string.Empty)).ToArray();
        }

        public int Count()
        {
            var dirPath = ResolvePath(string.Empty);

            string[] allfiles = System.IO.Directory.GetFiles(dirPath, "*.*", System.IO.SearchOption.AllDirectories);

            return allfiles.Count();
        }
    }
}
