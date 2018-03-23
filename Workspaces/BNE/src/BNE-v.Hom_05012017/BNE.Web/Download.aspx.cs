using BNE.BLL;
using BNE.BLL.Custom;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class Download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            object oTipoArquivo = null;
            object oInfoArquivo = null;

            if (!RouteData.Values.TryGetValue("tipoArquivo", out oTipoArquivo) ||
                String.IsNullOrEmpty(oTipoArquivo.ToString()) ||
                !RouteData.Values.TryGetValue("infoArquivo", out oInfoArquivo) ||
                String.IsNullOrEmpty(oInfoArquivo.ToString())
                )
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                Response.End();
            }



            #region Download do Arquivo
            String tipoArquivo = oTipoArquivo.ToString();
            String infoArquivo = oInfoArquivo.ToString();
            String nomeArquivo = null;
            byte[] ArquivoAnexo = null;

            try
            {
                switch (tipoArquivo.ToLower())
                {
                    case "cv":
                        ArquivoAnexo = PessoaFisicaComplemento.CarregarAnexoPorPessoaFisicaDoStorage(Convert.ToInt32(Helper.Descriptografa(infoArquivo)), out nomeArquivo);
                        break;
                    case "imagemstc":
                        int idOrigem = Convert.ToInt32(Helper.Descriptografa(infoArquivo));
                        OrigemFilial objOrigemFilial;
                        if (OrigemFilial.CarregarPorOrigem(idOrigem, out objOrigemFilial))
                        {
                            string descricao = objOrigemFilial.DescricaoPaginaInicial;
                            if (!String.IsNullOrEmpty(descricao) && descricao.Contains("data:image"))
                            {
                                string imagem = Regex.Match(descricao, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value; //Recuperando o source
                                const string remove = "base64,";
                                imagem = imagem.Remove(0, imagem.IndexOf(remove, StringComparison.Ordinal) + remove.Length); //Removendo o encoding
                                ArquivoAnexo = Convert.FromBase64String(imagem);
                            }
                        }
                        nomeArquivo = "imagem.jpg";
                        break;
                    case "temp":
                        string path = Path.Combine(ConfigurationManager.AppSettings["ArquivosTemporarios"].ToString(), Helper.Descriptografa(infoArquivo));
                        nomeArquivo = "boleto.pdf";
                        if (File.Exists(path))
                            ArquivoAnexo = File.ReadAllBytes(path);
                        break;
                    default:
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        Response.End();
                        break;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }


            if (ArquivoAnexo == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                Response.End();
            }

            //Se os bytes dos arquivos foram encontrados, realizar o download
            if (ArquivoAnexo != null)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", nomeArquivo));
                Response.ContentType = Helper.GetMIMEType(nomeArquivo); //"application/pdf";
                Response.BinaryWrite(ArquivoAnexo);
                Response.Flush();
                Response.End();
            }
            #endregion
        }
    }
}