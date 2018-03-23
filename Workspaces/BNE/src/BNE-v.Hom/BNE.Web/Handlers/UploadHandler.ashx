<%@ WebHandler Language="C#" Class="UploadHandler" %>

using System;
using System.Web;

public class UploadHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            if (context.Request.Files != null && context.Request.Files.Count > 0)
            {
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    HttpPostedFile file = (HttpPostedFile)context.Request.Files[0];
                    String ext = System.IO.Path.GetExtension(file.FileName);
                    //String n = System.IO.Path.GetFileName(file.FileName);
                    String n = Guid.NewGuid().ToString() + ext;
                    String savePath = context.Server.MapPath("~/fotos");
                    
                    if (!System.IO.Directory.Exists(savePath))
                        System.IO.Directory.CreateDirectory(savePath);
                    
                    String targetPath = System.IO.Path.Combine(savePath, n);
                    String clientUrl = "~/fotos/" + n;
                    file.SaveAs(targetPath);
                    context.Response.Write(clientUrl);
                }
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("500: " + ex.Message);
            EL.GerenciadorException(ex);
        }
        finally
        {
            context.Response.End();
        }
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}