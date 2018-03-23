using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.StorageManager.Managers
{
    public class OSRegexPathManager : IFileManager
    {
        private 

        public void LoadConfig(string folderName)
        {
            try
            {
                StoragesConfigSection config = System.Configuration.ConfigurationManager.GetSection("StoragesConfig") as StoragesConfigSection;

                ObjectStorageConfiguration storage = config.Storages[config.Folders[folderName].Storages[StorageTypes.ObjectStorage].StorageName];

                _url = storage.Url;
                _account = storage.Account;
                _container = storage.Container;
                _containerUrl = Helper.CombineUrl(storage.Url, "v1", storage.Account, storage.Container);
                _user = storage.User;
                _apiKey = storage.ApiKey;
                _folder = config.Folders[folderName].Storages[StorageTypes.ObjectStorage].StorageFolder;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Ocorreu uma falha ao tentar configurar o Object Storage", ex);
            }
        }

        public bool Save(string objectPath, byte[] byteArray)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string objectPath)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string objectPath)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes(string objectPath)
        {
            throw new NotImplementedException();
        }

        public bool Copy(string objectOriginPath, string objectDestinationPath)
        {
            throw new NotImplementedException();
        }

        public bool Move(string objectOriginPath, string objectDestinationPath)
        {
            throw new NotImplementedException();
        }

        public string GetUrl(string objectPath)
        {
            throw new NotImplementedException();
        }
    }
}
