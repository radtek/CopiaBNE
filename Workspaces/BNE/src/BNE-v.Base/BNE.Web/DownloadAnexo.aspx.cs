using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code;

namespace BNE.Web
{
    public partial class DownloadAnexo : BasePage
    {

        #region Propriedades

        #region ArquivoAnexo - Variável 1
        private byte[] ArquivoAnexo
        {
            get
            {
                return (byte[])(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
        }
        #endregion

        #region NomeAnexo - Variável 2
        private string NomeAnexo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Convert.ToString(ViewState[Chave.Temporaria.Variavel2.ToString()]);
                return string.Empty;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }

        #endregion

        #endregion

        #region Inicializar

        private void Inicializar()
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", NomeAnexo));
            Response.ContentType = RetornarContentType();
            Response.BinaryWrite(ArquivoAnexo);
            Response.Flush();
            Response.End();
        }

        #endregion

        #region RetornarExtensao

        private string RetornarContentType()
        {
            string[] arquivo = NomeAnexo.Split('.');
            string extensao = arquivo[1];
            string contentType = string.Empty;

            switch (extensao)
            {
                case "doc":
                    contentType = "application/ms-word"; break;
                case "docx":
                    contentType = "application/ms-word"; break;
                case "xls":
                    contentType = "application/vnd.ms-excel"; break;
                case "xlsx":
                    contentType = "application/vnd.ms-excel"; break;
                case "ppt":
                    contentType = "application/vnd.ms-powerpoint"; break;
                case "pdf":
                    contentType = "application/pdf"; break;
                case "jpg":
                    contentType = "image/jpeg"; break;
                case "jpeg":
                    contentType = "image/jpeg"; break;
                case "gif":
                    contentType = "image/gif"; break;
                case "ico":
                    contentType = "image/vnd.microsoft.icon"; break;
                case "zip":
                    contentType = "application/zip"; break;
                case "exe":
                    contentType = "application/x-msdownload"; break;
                case "rar":
                    contentType = "application/rar"; break; //application/x-rar-compressed

            }

            return contentType;
        }

        #endregion
    }
}