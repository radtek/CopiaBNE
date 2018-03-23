using System;
using System.IO;

namespace BNE.Log.Base
{
    public static class Helper
    {
        public static void GravarLogDisco(Exception ex)
        {
            //Recuperando o diretório do arquivo de log.
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"logErro");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(path + "\\error-" + DateTime.Now.ToString("dd-MM-yy") + "-" + Environment.TickCount + ".txt",
                string.Format("{0} {1}{2}MENSAGEM: {3}{2}STACKTRACE: {2}{4}{2}INNER EXCEPTION: {5}{2}HELP LINK: {6}{2}SOURCE: {7}{2}{2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, ex.Message, ex.StackTrace, ex.InnerException, ex.HelpLink, ex.Source));
        }
    }
}
