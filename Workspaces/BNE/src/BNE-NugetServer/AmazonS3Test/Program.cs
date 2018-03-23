using BNE.StorageManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonS3Test
{
    class Program
    {
        static void Main(string[] args)
        {


            #region Download do Curriculo Anexo
            byte[] ArquivoAnexo = null;

            IFileManager fm = StorageManager.GetFileManager("curriculos");
            if (fm.Exists("nome_do_arquivo"))
            {
                Console.WriteLine("Exixste");
            }
            else {
                Console.WriteLine("Não existe");
            }

            #endregion

            Console.ReadKey();
        }
    }
}
