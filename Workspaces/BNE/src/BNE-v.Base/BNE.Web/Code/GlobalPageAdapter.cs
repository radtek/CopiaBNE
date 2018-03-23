using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace BNE.Web.Code
{
    public class GlobalPageAdapter : System.Web.UI.Adapters.PageAdapter
    {
        public override PageStatePersister GetStatePersister()
        {
            string customPageState = ConfigurationManager.AppSettings["PageStatePersister"];
            
            bool boolCustomPageState;
            bool.TryParse(customPageState, out boolCustomPageState);

            if (boolCustomPageState)
                return new CustomPagePersister(Page);

            return new HiddenFieldPageStatePersister(Page);
        }
    }

    /// <summary>
    /// Esta classe grava o viewstate em uma pasta VIEWSTATE na raiz da aplicação deixando de enviar 
    /// p/ o cliente.
    /// </summary>
    public class CustomPagePersister : PageStatePersister
    {
        private const string FileExtension = "viewstate";

        public CustomPagePersister(Page page) : base(page) {  }

        #region Load
        public override void Load()
        {
            string strViewState = GetKey();
            string strFolder = GetFolder();

            string sFile = string.Format("{0}{1}.{2}", strFolder, strViewState, FileExtension);

            if (!File.Exists(sFile))
                return;

            string bytes;
            using (var sr = new StreamReader(sFile))
            {
                bytes = sr.ReadToEnd(); //Ler do arquivo 
            }

            var formatter = new LosFormatter();
            var statePair = (Pair)formatter.Deserialize(bytes);

            ViewState = statePair.First;
            ControlState = statePair.Second;
        }
        #endregion

        #region Save
        public override void Save()
        {
            if ((ViewState != null || ControlState != null))
            {
                string strViewState = GetKey();
                string strFolder = GetFolder();

                string strNomeArquivo;
                if (String.IsNullOrEmpty(strViewState))
                {
                    ClearOldFiles();
                    strNomeArquivo = Guid.NewGuid().ToString().Replace("-", "") + "." + FileExtension;
                }
                else
                    strNomeArquivo = string.Format("{0}.{1}", strViewState, FileExtension);

                var formatter = new LosFormatter();
                StringWriter sw = null;
                var statePair = new Pair(ViewState, ControlState);

                try
                {
                    sw = new StringWriter();
                    formatter.Serialize(sw, statePair);

                    using (var fWriter = new StreamWriter(strFolder + strNomeArquivo))
                    {
                        fWriter.Write(sw.ToString());
                    }
                }
                finally
                {
                    if (sw != null)
                        sw.Dispose();
                }

                Page.ClientScript.RegisterHiddenField("__VSTATE", Regex.Replace(strNomeArquivo, @"(.+)\." + FileExtension, "$1"));
            }
        }
        #endregion

        #region GetFolder
        private string GetFolder()
        {
            string strFolder = Page.Server.MapPath(@"\VIEWSTATE\");
            
            if (!Directory.Exists(strFolder))
                Directory.CreateDirectory(strFolder);

            return strFolder;
        }
        #endregion

        #region GetKey
        private string GetKey()
        {
            string strViewState = Page.Request.Form["__VSTATE"];

            if (!string.IsNullOrEmpty(strViewState) && strViewState.Contains(","))
                return strViewState.Substring(0, strViewState.IndexOf(",", StringComparison.Ordinal));

            return strViewState;
        }
        #endregion

        #region ClearOldFiles
        private void ClearOldFiles()
        {
            string clearCustomPageState = ConfigurationManager.AppSettings["AutoClearPageStatePersister"];

            bool boolClearCustomPageState;
            bool.TryParse(clearCustomPageState, out boolClearCustomPageState);

            if (boolClearCustomPageState)
            {
                const string key = "__TimeClear__";
                DateTime? dateRelease = null;

                if (Page.Session[key] != null)
                    dateRelease = (DateTime)Page.Session[key];

                if (dateRelease != null && dateRelease.Value > DateTime.Now)
                    return;

                string strFolder = GetFolder();

                var dic = new DirectoryInfo(strFolder);

                FileInfo[] fOlds = dic.GetFiles(String.Format("*.{0}", FileExtension));

                foreach (FileInfo file in fOlds)
                {
                    try
                    {
                        TimeSpan dif = DateTime.Now.Subtract(file.LastWriteTime);
                        if (dif.Hours > 4 && file.Exists)
                            file.Delete();
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                }

                Page.Session[key] = DateTime.Now.AddMinutes(30);
            }
        }
        #endregion

    }
}
