using BNE.StorageManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class Sitemap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String fileName = Request.Url.AbsolutePath.Remove(0, 1);
            IFileManager fm;
            if (fileName.StartsWith("curriculos"))
                fm = StorageManager.StorageManager.GetFileManager("siteMapsCurriculo");
            else if (fileName.StartsWith("empresas"))
                fm = StorageManager.StorageManager.GetFileManager("siteMapsEmpresas");
            else if (fileName.StartsWith("geral"))
                fm = StorageManager.StorageManager.GetFileManager("siteMapsGeral");
            else
                fm = StorageManager.StorageManager.GetFileManager("siteMapsVagas");

            //Recuperando nome do arquivo (entrada seá no formato <pasta>/nomedoarquivo.xml
            fileName = fileName.Split('/')[1];

            if (!fm.Exists(fileName))
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "File not found";
                Response.End();
            }

            Response.ContentType = "application/xml";
            byte[] xml = fm.GetBytes(fileName);
            Response.Write(System.Text.Encoding.UTF8.GetString(xml));
            Response.End();
        }
    }
}