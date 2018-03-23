using System;
using System.IO;

namespace BNE.Console.PessoaFisicaComplemento
{
    public static class Attachment
    {

        #region GerarLinkDownload
        /// <summary>
        /// Passa a string para gerar o arquivo para a tela de geração de arquivo (DownloadArquivo.aspx)
        /// </summary>
        /// <param name="arrayFinal">Byte array de dados que devem estar no arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gerado</param>
        public static void Save(int idPessoaFisica, byte[] arrayFinal, string nomeArquivo)
        {

            FileStream fs = null;
            try
            {
                fs = new FileStream(GerarLinkDownload(idPessoaFisica + "_" + DateTime.Now.Ticks + "_" + nomeArquivo), FileMode.CreateNew, FileAccess.Write);
                fs.Write(arrayFinal, 0, arrayFinal.Length);
                fs.Close();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        private static string GerarLinkDownload(string nomeArquivo)
        {
            string diretorioArquivo = Configuracao.PastaArquivoTemporario;

            if (!Directory.Exists(diretorioArquivo))
                Directory.CreateDirectory(diretorioArquivo);

            return String.Format("{0}\\{1}", diretorioArquivo, nomeArquivo);
        }
        #endregion

    }
}
