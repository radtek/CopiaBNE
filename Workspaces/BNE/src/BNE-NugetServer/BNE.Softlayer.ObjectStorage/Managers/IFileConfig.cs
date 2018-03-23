using BNE.StorageManager.Config;

namespace BNE.StorageManager.Managers
{
    interface IFileConfig
    {
        void LoadConfig(StorageConfiguration storage);
    }
}
