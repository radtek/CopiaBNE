using BNE.StorageManager;

namespace BNE.BLL
{
    public class StorageManager
    {
        #region ArquivoExiste
        /// <summary>
        /// Método responsável por checar se o arquivo existe no Storage
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="objectPath"></param>
        /// <returns></returns>
        public static bool ArquivoExiste(string folderName,string objectPath)
        {
            IFileManager fm = BNE.StorageManager.StorageManager.GetFileManager(folderName);
            return fm.Exists(objectPath);
        }
        #endregion

        #region CarregarAquivo
        /// <summary>
        /// Método responsável por carregar um arquivo do Storage
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="objectPath"></param>
        /// <returns></returns>
        public static byte[] CarregarArquivo(string folderName, string objectPath)
        {
            IFileManager fm = BNE.StorageManager.StorageManager.GetFileManager(folderName);

            if (fm.Exists(objectPath))
                return fm.GetBytes(objectPath);
            else
                return null;
        }
        #endregion

        #region SalvarArquivo
        /// <summary>
        /// Método responsável por salvar um arquivo no Storage
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="objectPath"></param>
        /// <param name="file"></param>
        public static void SalvarArquivo(string folderName,string objectPath, byte[] file)
        {
            IFileManager fm = BNE.StorageManager.StorageManager.GetFileManager(folderName);
            fm.Save(objectPath,file);
        }
        #endregion

        #region ApagarArquivo
        /// <summary>
        /// Método responsável por apagar um arquivo do Storage
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="objectPath"></param>
        /// <returns></returns>
        public static bool ApagarArquivo(string folderName,string objectPath)
        {
            IFileManager fm = BNE.StorageManager.StorageManager.GetFileManager(folderName);

            if (fm.Exists(objectPath))
            {
                fm.Delete(objectPath);
                return true;
            }

            return false;
        }
        #endregion

        #region RecuperarCurriculos

        //public static byte[] RecuperarCurriculos(List<int> idCurriculo)
        //{

        //}
        #endregion
    }
}
