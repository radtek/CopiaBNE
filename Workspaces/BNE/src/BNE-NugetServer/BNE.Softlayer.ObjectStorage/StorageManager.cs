using System;
using System.Collections.Generic;
using BNE.StorageManager.Config;

namespace BNE.StorageManager
{
    /// <summary>
    /// Classe responsável pelas iterações com a API do Object Layer da Softlayer
    /// </summary>
    public class StorageManager
    {
        public IFileManager FileManager { get; private set; }

        public StorageManager(String folderName)
        {
            StoragesConfigSection config = System.Configuration.ConfigurationManager.GetSection("StoragesConfig") as StoragesConfigSection;
            FileManager = new FolderManager();
            (FileManager as FolderManager).LoadConfig(folderName);
        }

        private static Dictionary<string, IFileManager> _managers = new Dictionary<string,IFileManager>();

        public static IFileManager GetFileManager(String folderName)
        {
            IFileManager ret;
            if (_managers.TryGetValue(folderName, out ret))
                return ret;

            StoragesConfigSection config = System.Configuration.ConfigurationManager.GetSection("StoragesConfig") as StoragesConfigSection;
            FolderManager fm = new FolderManager();
            fm.LoadConfig(folderName);
            _managers.Add(folderName, fm);
            return _managers[folderName];
        }
    }
}
