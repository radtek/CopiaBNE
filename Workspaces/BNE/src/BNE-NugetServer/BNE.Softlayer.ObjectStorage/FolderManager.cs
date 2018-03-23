using BNE.StorageManager.Config;
using BNE.StorageManager.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BNE.StorageManager
{
    public class FolderManager : IFileManager
    {
        private List<IFileManager> _lstFileManagers = new List<IFileManager>();

        internal FolderManager() { }

        public void LoadConfig(string folderName)
        {
            StoragesConfigSection config = System.Configuration.ConfigurationManager.GetSection("StoragesConfig") as StoragesConfigSection;
            FolderManager ret = new FolderManager();

            foreach (StorageConfiguration storage in config.Folders[folderName].Storages)
            {
                if (!storage.Enabled)
                    continue;
                switch (storage.Type)
                {
                    case StorageTypes.Local:
                        LocalFolderManager lf = new LocalFolderManager();
                        lf.LoadConfig(storage);
                        this._lstFileManagers.Add(lf);
                        break;
                    case StorageTypes.ObjectStorage:
                        ObjectStorageManager os = new ObjectStorageManager();
                        os.LoadConfig(storage);
                        this._lstFileManagers.Add(os);
                        break;
                    case StorageTypes.AmazonS3:
                        AmazonS3Manager s3 = new AmazonS3Manager();
                        s3.LoadConfig(storage);
                        this._lstFileManagers.Add(s3);
                        break;
                    case StorageTypes.AzureStorage:
                        AzureStorageManager az = new AzureStorageManager();
                        az.LoadConfig(storage);
                        this._lstFileManagers.Add(az);
                        break;
                    default:
                        throw new Exception("Storage type inválido ou não definido");
                }
            }
        }

        public bool Save(string objectPath, byte[] byteArray)
        {
            bool success = false;
            foreach (IFileManager fm in _lstFileManagers)
                success = fm.Save(objectPath, byteArray) ? true : success;
            return success;
        }

        public bool Delete(string objectPath)
        {
            bool success = false;
            foreach (IFileManager fm in _lstFileManagers)
                success = fm.Delete(objectPath) ? true : success;
            return success;
        }

        public bool Exists(string objectPath)
        {
            foreach (IFileManager fm in _lstFileManagers)
            {
                if (fm.Exists(objectPath))
                    return true;
            }

            return false;
        }

        public async Task<bool> ExistsAsync(string objectPath)
        {
            var tasks = new List<Task<bool>>();
            foreach (IFileManager fm in _lstFileManagers)
            {
                tasks.Add(fm.ExistsAsync(objectPath));
            }

            await Task.WhenAll(tasks);

            return tasks.Any(t => t.Result);
        }

        public byte[] GetBytes(string objectPath)
        {
            Exception exception = null;
            foreach (IFileManager fm in _lstFileManagers)
            {
                try
                {
                    return fm.GetBytes(objectPath);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    continue;
                }
            }

            if (exception != null)
                throw exception;
            else
                throw new FileNotFoundException();
        }

        public bool Copy(string objectOriginPath, string objectDestinationPath)
        {
            bool success = false;
            foreach (IFileManager fm in _lstFileManagers)
                success = fm.Copy(objectOriginPath, objectDestinationPath) ? true : success;
            return success;
        }

        public bool Move(string objectOriginPath, string objectDestinationPath)
        {
            bool success = false;
            foreach (IFileManager fm in _lstFileManagers)
                success = fm.Move(objectOriginPath, objectDestinationPath) ? true : success;
            return success;
        }

        public string GetUrl(string objectPath)
        {
            return _lstFileManagers[0].GetUrl(objectPath);
        }

        public IEnumerable<string> List()
        {
            var result = new List<string>();
            foreach (IFileManager fm in _lstFileManagers)
            {
                foreach (var file in fm.List())
                {
                    if (result.Any(r => r == file))
                        continue;

                    result.Add(file);
                    yield return file;
                }
            }
        }

        public int Count()
        {
            int result = -1;
            foreach (IFileManager fm in _lstFileManagers)
            {
                var count = fm.Count();
                if (result < count)
                    result = count;
            }

            return result;
        }
    }
}
