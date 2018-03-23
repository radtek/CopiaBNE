using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using BNE.Web.Properties;

namespace BNE.Web.Handlers
{
    /// <summary>
    /// Summary description for PessoaFisicaFoto
    /// </summary>
    public class PessoaFisicaFoto : IHttpHandler
    {

        #region ProcessRequest
        public void ProcessRequest(HttpContext context)
        {
            DateTime delay = DateTime.Now.AddMinutes(-Settings.Default.PessoaFisicaFotoDelayLimparCacheMinutos);

            if (context.Request.QueryString["origem"] == null)
            {
                NaoDisponivel(context);
                return;
            }

            if (context.Request.QueryString["CPF"] == null)
            {
                NaoDisponivel(context);
                return;
            }

            String numeroCpf = context.Request.QueryString["CPF"];

            numeroCpf = (new Regex("[^0-9]")).Replace(numeroCpf, "");

            if (numeroCpf.Length > 11)
            {
                NaoDisponivel(context);
                return;
            }

            numeroCpf = numeroCpf.PadLeft(11, '0');

            var pathFile = string.Format("/{0}.jpg", numeroCpf);
            var pathFolder = Settings.Default.PessoaFisicaFotoCaminho;
            var serverPathFolder = context.Server.MapPath(pathFolder);

            if (!Employer.Plataforma.Web.Componentes.CPF.Validar(numeroCpf))
            {
                NaoDisponivel(context);
                return;
            }

            var dir = new DirectoryInfo(serverPathFolder);
            if (!dir.Exists)
                Directory.CreateDirectory(serverPathFolder);

            //Excluir arquivos antigos
            (new DirectoryInfo(serverPathFolder))
                .GetFiles()
                .Where(file => file.CreationTime < delay)
                .ToList()
                .ForEach(delegate(FileInfo file)
                    {
                        if (!file.IsReadOnly)
                            file.Delete();
                    });

            if (File.Exists(serverPathFolder + pathFile) && File.GetCreationTime(serverPathFolder + pathFile) > delay)
            {
                Disponivel(context, pathFolder + pathFile);
                return;
            }

            byte[] byteArray = null;

            var origem = context.Request.QueryString["origem"];

            OrigemFoto origemEnum;
            if (Enum.TryParse(origem, false, out origemEnum))
            {
                byteArray = RecuperarFoto(Convert.ToDecimal(numeroCpf), origemEnum);
            }

            if (byteArray == null || byteArray.Length == 0)
            {
                NaoDisponivel(context);
                return;
            }

            //Criando novo arquivo
            using (FileStream sw = File.Create(serverPathFolder + pathFile))
            {
                using (var bw = new BinaryWriter(sw))
                {
                    bw.Write(byteArray);
                    bw.Flush();
                    bw.Close();
                }
            }
            Disponivel(context, pathFolder + pathFile);
        }
        #endregion

        #region NaoDisponivel
        private void NaoDisponivel(HttpContext context)
        {
            context.Response.Redirect("~/img/img_sem_foto.gif", false);
        }
        #endregion

        #region Disponivel
        private void Disponivel(HttpContext context, string pathFolderFile)
        {
            context.Response.Redirect(string.Format("~/{0}?nocache={1}", pathFolderFile, Guid.NewGuid()), false);
        }
        #endregion

        #region IsReusable
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region RemoverFotoCache
        public static void RemoverFotoCache(String numeroCpf)
        {
            numeroCpf = (new Regex("[^0-9]")).Replace(numeroCpf, "");

            if (numeroCpf.Length > 11)
                return;

            numeroCpf = numeroCpf.PadLeft(11, '0');

            String pathFile = String.Format("/{0}.jpg", numeroCpf);
            String pathFolder = HttpContext.Current.Server.MapPath(Settings.Default.PessoaFisicaFotoCaminho);

            if (File.Exists(pathFolder + pathFile))
            {
                File.Delete(pathFolder + pathFile);
            }
        }
        #endregion

        #region RecuperarFoto
        public static byte[] RecuperarFoto(decimal numeroCPF, OrigemFoto origemFoto)
        {
            byte[] byteArray = null;
            switch (origemFoto)
            {
                case OrigemFoto.Local:
                    #region Foto Local
                    try
                    {
                        byteArray = BLL.PessoaFisicaFoto.RecuperarArquivo(numeroCPF);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                    #endregion
                    break;
                case OrigemFoto.SouEuMesmo:
                    #region SouEuMesmo
                    try
                    {
                        //Recuperar do SouEuMesmo
                        using (var client = new IntegracaoSouEuMesmo.IntegracaoClient())
                        {
                            byteArray = client.RetornoFotoPessoaFisica(Convert.ToDecimal(numeroCPF));
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                    #endregion
                    break;
            }

            return byteArray;
        }
        #endregion

        #region ExisteFoto
        public static bool ExisteFoto(decimal numeroCPF)
        {
            return RecuperarFoto(numeroCPF, OrigemFoto.Local) != null;
        }
        #endregion

        #region OrigemFoto
        public enum OrigemFoto
        {
            Local,
            SouEuMesmo
        }
        #endregion

    }
}