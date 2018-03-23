using BNE.StorageManager.Config;
using BNE.StorageManager.Config.AzureStorage;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BNE.StorageManager.Managers
{
    public class AzureStorageManager : IFileManager, IFileConfig
    {
        private string _connectionString;
        private string _folder;
        private Regex _regexToPath;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _container;
        internal static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Copy(string objectOriginPath, string objectDestinationPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"Coping from '{objectOriginPath}' to {objectDestinationPath} exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            objectOriginPath = ResolvePath(objectOriginPath, false);
            objectDestinationPath = ResolvePath(objectDestinationPath, false);

            var sourceBlob = _container.GetBlockBlobReference(objectOriginPath);
            var targetBlob = _container.GetBlockBlobReference(objectDestinationPath);

            var result = targetBlob.StartCopy(sourceBlob);

            while (targetBlob.CopyState.Status == CopyStatus.Pending)
            {
                Thread.Sleep(500);
            }

            if (targetBlob.CopyState.Status != CopyStatus.Success)
                throw new Exception("Copy failed: " + targetBlob.CopyState.Status);

            if (log.IsDebugEnabled)
            {
                log.Debug($"'{objectOriginPath}' coppied to {objectDestinationPath} exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");
            }

            return true;
        }

        public bool Delete(string objectPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"Deleting '{objectPath}' in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            objectPath = ResolvePath(objectPath, false);

            var result = _container.GetBlockBlobReference(objectPath).DeleteIfExists();

            if (log.IsDebugEnabled)
            {
                if (result)
                    log.Debug($"'{objectPath}' deleted '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");
                else
                    log.Debug($"'{objectPath}' was not deleted '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");
            }

            return result;
        }

        public bool Exists(string objectPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"Checking if '{objectPath}' exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            objectPath = ResolvePath(objectPath, false);

            var result = _container.GetBlockBlobReference(objectPath).Exists();

            if (log.IsDebugEnabled)
            {
                if (result)
                    log.Debug($"'{objectPath}' exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");
                else
                    log.Debug($"'{objectPath}' does not exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");
            }

            return result;
        }

        public async Task<bool> ExistsAsync(string objectPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"Checking if '{objectPath}' exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            objectPath = ResolvePath(objectPath, false);

            var result = await _container.GetBlockBlobReference(objectPath).ExistsAsync();

            if (log.IsDebugEnabled)
            {
                if (result)
                    log.Debug($"'{objectPath}' exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");
                else
                    log.Debug($"'{objectPath}' does not exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");
            }

            return result;
        }

        public byte[] GetBytes(string objectPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"Reading bytes from '{objectPath}' in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            objectPath = ResolvePath(objectPath, false);

            var stream = _container.GetBlockBlobReference(objectPath).OpenRead();

            try
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }

                    var result = ms.ToArray();

                    if (log.IsDebugEnabled)
                    {
                        log.Debug($"{result.Length} bytes read from '{objectPath}' in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");
                    }

                    return result;
                }
            }
            finally { stream.Close(); stream.Dispose(); }
        }

        public string GetUrl(string objectPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"Geting url for '{objectPath}' in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            objectPath = ResolvePath(objectPath);

            var uri = new Uri(_container.Uri, objectPath);

            if (log.IsDebugEnabled)
                log.Debug($"Path resolved to '{uri.AbsoluteUri}' url for '{objectPath}' in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            return uri.AbsoluteUri;
        }

        public void LoadConfig(StorageConfiguration storage)
        {
            try
            {
                StoragesConfigSection config = System.Configuration.ConfigurationManager.GetSection("StoragesConfig") as StoragesConfigSection;

                AzureStorageConfiguration azStorage = config.AzureStorages[storage.StorageName];

                _folder = storage.StorageFolder;
                _connectionString = azStorage.ConnectionString;
                _storageAccount = CloudStorageAccount.Parse(_connectionString);
                _blobClient = _storageAccount.CreateCloudBlobClient();
                _container = _blobClient.GetContainerReference(azStorage.Container);
                _container.CreateIfNotExists();

                if (!String.IsNullOrEmpty(storage.RegexPath))
                    _regexToPath = new Regex(storage.RegexPath);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ConfigurationErrorsException("Ocorreu uma falha ao tentar configurar o Object Storage", ex);
            }
        }

        public bool Move(string objectOriginPath, string objectDestinationPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"Moving from '{objectOriginPath}' to {objectDestinationPath} exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            if (Copy(objectOriginPath, objectDestinationPath))
                Delete(objectOriginPath);

            if (log.IsDebugEnabled)
                log.Debug($"'{objectOriginPath}' moved to {objectDestinationPath} exists in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            return true;
        }

        public bool Save(string objectPath, byte[] byteArray)
        {
            if (log.IsDebugEnabled)
                log.Debug($"Saving '{objectPath}' in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            objectPath = ResolvePath(objectPath, false);

            var blob = _container.GetBlockBlobReference(objectPath);
            blob.Properties.ContentType = Helper.GetMIMEType(objectPath); ;

            blob.UploadFromByteArray(byteArray, 0, byteArray.Length);

            if (log.IsDebugEnabled)
                log.Debug($"'{objectPath}'({byteArray.Length} bytes) saved in container '{_container.Name}', account '{_storageAccount.BlobStorageUri.PrimaryUri}'");

            return true;
        }

        public int Count()
        {
            var directory = _container.GetDirectoryReference(_folder);

            return directory.ListBlobs(useFlatBlobListing:true).Count();
        }

        public IEnumerable<string> List()
        {
            var directory = _container.GetDirectoryReference(_folder);

            var blobs = directory.ListBlobs(useFlatBlobListing: true);

            foreach (var b in blobs)
            {
                yield return string.IsNullOrEmpty(_folder) ?
                        (b as CloudBlockBlob).Name :
                        (b as CloudBlockBlob).Name.Replace(_folder + "/", string.Empty);
            }
        }

        /// <summary>
        /// Implementa o tratamento ao tratamento da url incluindo as pastas a serem criadas com os grupos da Regex
        /// </summary>
        /// <param name="objectPath">Caminho do arquivo</param>
        /// <returns></returns>
        public String ResolvePath(String objectPath, bool includeURL = true)
        {
            //objectPath = HttpUtility.UrlEncode(objectPath);

            if (_regexToPath == null)
            {
                if (includeURL)
                    return Helper.CombineUrl(_container.Uri.AbsoluteUri, _folder, objectPath);
                else
                    return Helper.CombineUrl(_folder, objectPath);
            }

            String folderPath;
            if (includeURL)
                return Helper.CombineUrl(_container.Uri.AbsoluteUri, _folder, Helper.ResolvePath(objectPath, '/', _regexToPath, out folderPath));
            else
                return Helper.CombineUrl(_folder, Helper.ResolvePath(objectPath, '/', _regexToPath, out folderPath));
        }
    }
}
