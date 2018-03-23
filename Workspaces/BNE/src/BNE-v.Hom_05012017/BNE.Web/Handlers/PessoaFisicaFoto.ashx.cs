using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using BNE.Web.Properties;
using BNE.BLL.Custom;

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
            if (context.Request.QueryString["origem"] == null || context.Request.QueryString["CPF"] == null)
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

            if (!Employer.Plataforma.Web.Componentes.CPF.Validar(numeroCpf))
            {
                NaoDisponivel(context);
                return;
            }

            byte[] byteArray = null;
            bool homem = true;

            var origem = context.Request.QueryString["origem"];

            OrigemFoto origemEnum;
            if (Enum.TryParse(origem, false, out origemEnum))
            {
                byteArray = RecuperarFoto(Convert.ToDecimal(numeroCpf), origemEnum, out homem);
            }

            if (byteArray == null || byteArray.Length == 0)
            {
                NaoDisponivel(context, homem);
                return;
            }

            string fileName = string.Format("/{0}.jpg", numeroCpf);

            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.AppendHeader("content-disposition", fileName);
            context.Response.ContentType = Helper.GetMIMEType(fileName);
            context.Response.BinaryWrite(byteArray);
            context.Response.Flush();
            context.Response.End();
        }
        #endregion

        #region NaoDisponivel
        private void NaoDisponivel(HttpContext context, bool homem = true)
        {
            context.Response.Redirect(homem ? "~/img/img-homem.jpg" : "~/img/img-mulher.jpg", false);
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
        public static byte[] RecuperarFoto(decimal numeroCPF, OrigemFoto origemFoto, out bool homem)
        {
            homem = true;
            byte[] byteArray = null;
            switch (origemFoto)
            {
                case OrigemFoto.Local:
                    #region Foto Local
                    try
                    {
                        byteArray = BLL.PessoaFisicaFoto.RecuperarArquivo(numeroCPF, out homem);
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
            bool homem;
            return RecuperarFoto(numeroCPF, OrigemFoto.Local, out homem) != null;
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