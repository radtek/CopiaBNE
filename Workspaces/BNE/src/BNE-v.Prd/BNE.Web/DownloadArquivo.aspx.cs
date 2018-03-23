using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class DownloadArquivo : BasePage
    {
        string extensao;

        #region Propriedades

        #region Path - Variável 1
        private string Path
        {
            get
            {
                return Request["path"].ToString();
            }


        }
        #endregion

        #region FileName - Variável 2
        private string FileName
        {
            get
            {
                return Request["filename"].ToString();
            }


        }
        #endregion


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Inicializar();
        }

        #region Inicializar

        private void Inicializar()
        {
            var filePath = Server.MapPath(Path);
            if(!File.Exists(filePath))return;

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = RetornarContentType();
            Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}.{1}", FileName, extensao));
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();
        }

        #endregion

        #region RetornarExtensao

        private string RetornarContentType()
        {
            string[] arquivo = Path.Split('.');
            extensao = arquivo[1];
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