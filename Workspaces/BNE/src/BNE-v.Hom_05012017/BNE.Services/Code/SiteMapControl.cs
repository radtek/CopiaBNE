using BNE.BLL;
using BNE.Services.Properties;
using BNE.StorageManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace BNE.Services.Code
{
    public class SiteMapControl
    {
        private object _lock = new object();

        public List<String> SavedFiles;
        public List<SiteMapItem> Itens;
        public TipoSitemap TipoSitemap { get; set; }
        public string PrefixoArquivo { get; set; }
        public int QuantidadeMaximaURLPorArquivo { get; set; }

        public string StorageFolder { get; set; }
        public string SitemapsRelativePath { get; set; }

        private readonly object _locker = new object();

        public SiteMapControl(TipoSitemap tipoSitemap, string prefixoArquivo)
        {
            TipoSitemap = tipoSitemap;
            PrefixoArquivo = prefixoArquivo;
            QuantidadeMaximaURLPorArquivo = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.SitemapQuantidadeURLPorArquivo));
            SavedFiles = new List<String>();
            Itens = new List<SiteMapItem>();

            switch (TipoSitemap)
            {
                case TipoSitemap.Curriculo:
                    StorageFolder = "siteMapsCurriculo";
                    SitemapsRelativePath = "curriculos";
                    break;
                case TipoSitemap.Vaga:
                    StorageFolder = "siteMapsVagas";
                    SitemapsRelativePath = "vagas";
                    break;
                case TipoSitemap.Empresa:
                    StorageFolder = "siteMapsEmpresas";
                    SitemapsRelativePath = "empresas";
                    break;
                default:
                    StorageFolder = "siteMapsGeral";
                    SitemapsRelativePath = "geral";
                    break;
            }
        }

        public void AdicionarURL(string url, changefreq changefreq, string priority)
        {
            try
            {
                List<SiteMapItem> itensB = null;
                lock (_lock)
                {
                    Itens.Add(new SiteMapItem() { URL = url, Changefreq = changefreq, Priority = priority });

                    if (Itens.Count >= QuantidadeMaximaURLPorArquivo)
                    {
                        itensB = Itens;
                        Itens = new List<SiteMapItem>();
                    }
                }

                if (itensB != null)
                {
                    SaveSiteMapItens(itensB);
                    itensB.Clear();
                    itensB.TrimExcess();
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        public void EndSiteMapGeneration()
        {
            try
            {
                /* Salvando registros no xmlDoc. Último arquivo de sitemap criado */
                if (Itens.Count > 0)
                    SaveSiteMapItens();

                SaveSiteMapIndex();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        private void SaveSiteMapItens()
        {
            SaveSiteMapItens(Itens);
        }

        private void SaveSiteMapItens(List<SiteMapItem> itens)
        {
            using (MemoryStream ms = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(ms, new XmlWriterSettings() { Encoding = new UTF8Encoding(false), OmitXmlDeclaration = false }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                foreach (var item in itens)
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", item.URL);
                    writer.WriteElementString("changefreq", item.Changefreq.ToString());
                    writer.WriteElementString("priority", item.Priority);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();

                SavedFiles.Add(string.Format("{0}{1}.xml", PrefixoArquivo, SavedFiles.Count + 1));
                SaveXmlDocument(ms, SavedFiles.Last());
            }
        }

        private void SaveSiteMapIndex()
        {
            using (MemoryStream ms = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(ms, new XmlWriterSettings() { Encoding = new UTF8Encoding(false), OmitXmlDeclaration = false }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");

                foreach (var file in SavedFiles)
                {
                    writer.WriteStartElement("sitemap");
                    writer.WriteElementString("loc", String.Format("{0}/{1}/{2}", Settings.Default.SiteMapsUrl, SitemapsRelativePath, file));
                    writer.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();

                SaveXmlDocument(ms, PrefixoArquivo + ".xml");
            }
        }

        private void SaveXmlDocument(MemoryStream ms, string fileName)
        {
            try
            {
                IFileManager fm = StorageManager.StorageManager.GetFileManager(StorageFolder);
                fm.Save(fileName, ms.ToArray());
            }
            catch (Exception ex)
            {

                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
    }

    public enum changefreq
    {
        always,
        hourly,
        daily,
        weekly,
        monthly,
        yearly,
        never
    }

    public enum TipoSitemap
    {
        Curriculo,
        Vaga,
        Empresa,
        Geral
    }

    public class SiteMapItem
    {
        public string URL;
        public changefreq Changefreq;
        public string Priority;
    }
}
