using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using BNE.BLL;
using BNE.EL;
using BNE.Services.Hangfire.BLL.Enumeradores;

namespace BNE.Services.Hangfire.BLL
{
    public class SitemapControl
    {
        readonly object _locker = new object();
        private List<XmlControl> ArquivosGerados { get; set; }
        public TipoSitemap TipoSitemap { get; set; }
        public string Diretorio { get; set; }
        public string URL { get; set; }
        public string PrefixoArquivo { get; set; }
        public int QuantidadeMaximaURLPorArquivo { get; set; }
        public string DiretorioAplicacao { get; set; }
        public string DiretorioAplicacaoVagas { get; set; }

        public SitemapControl(TipoSitemap tipoSitemap, string prefixoArquivo)
        {
            ArquivosGerados = new List<XmlControl>();
            TipoSitemap = tipoSitemap;
            PrefixoArquivo = prefixoArquivo;
            QuantidadeMaximaURLPorArquivo = Convert.ToInt32(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.SitemapQuantidadeURLPorArquivo));
            DiretorioAplicacao = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.DiretorioAplicacao);
            DiretorioAplicacaoVagas = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.DiretorioAplicacaoWebVagas);
            string urlSite = string.Concat("http://", Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.URLAmbiente));
            string urlSiteVaga = string.Concat("http://", Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.URLVagas));

            switch (TipoSitemap)
            {
                case TipoSitemap.Curriculo:
                    Diretorio = string.Concat(DiretorioAplicacao, "\\curriculo");
                    URL = String.Format("{0}/curriculo", urlSite);
                    break;
                case TipoSitemap.Vaga:
                    Diretorio = DiretorioAplicacaoVagas;
                    URL = urlSiteVaga;
                    break;
                case TipoSitemap.Empresa:
                    Diretorio = string.Concat(DiretorioAplicacao, "\\empresa");
                    URL = String.Format("{0}/empresa", urlSite);
                    break;
                default:
                    Diretorio = DiretorioAplicacao;
                    URL = String.Format("{0}", urlSite);
                    break;
            }
        }

        public void AdicionarURL(string url, changefreq changefreq, string priority)
        {
            try
            {
                lock (_locker)
                {
                    XmlControl xmlControl = GetXmlControl();

                    XmlNode urlsetNode = xmlControl.xmlDoc.GetElementsByTagName("urlset")[0];

                    XmlElement urlNode = xmlControl.xmlDoc.CreateElement("url", null);
                    XmlElement locNode = xmlControl.xmlDoc.CreateElement("loc");
                    XmlElement changefreqNode = xmlControl.xmlDoc.CreateElement("changefreq");
                    XmlElement priorityNode = xmlControl.xmlDoc.CreateElement("priority");

                    locNode.AppendChild(xmlControl.xmlDoc.CreateTextNode(url));
                    changefreqNode.AppendChild(xmlControl.xmlDoc.CreateTextNode(changefreq.ToString()));
                    priorityNode.AppendChild(xmlControl.xmlDoc.CreateTextNode(priority));

                    urlNode.AppendChild(locNode);
                    urlNode.AppendChild(changefreqNode);
                    urlNode.AppendChild(priorityNode);

                    urlsetNode.AppendChild(urlNode);

                    xmlControl.QuantidadeURL++;
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }

        public void SaveFiles()
        {
            try
            {
                /*Criando o arquivo que contém todos os sitemaps*/
                var xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                XmlElement sitemapindex = xmlDoc.CreateElement("sitemapindex");
                sitemapindex.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                xmlDoc.AppendChild(sitemapindex);

                for (int i = 0; i < ArquivosGerados.Count; i++)
                {
                    XmlElement sitemap = xmlDoc.CreateElement("sitemap");
                    XmlElement loc = xmlDoc.CreateElement("loc");
                    XmlElement lastmod = xmlDoc.CreateElement("lastmod");

                    var sequenciaArquivo = (i + 1).ToString(CultureInfo.CurrentCulture).PadLeft(4, '0');

                    loc.AppendChild(xmlDoc.CreateTextNode(String.Format("{0}/{1}{2}.xml", URL, PrefixoArquivo, sequenciaArquivo)));
                    lastmod.AppendChild(xmlDoc.CreateTextNode((DateTime.Now.ToString("yyyy-MM-dd"))));

                    sitemap.AppendChild(loc);
                    sitemap.AppendChild(lastmod);

                    sitemapindex.AppendChild(sitemap);

                    ArquivosGerados[i].xmlDoc.Save(Path.Combine(Diretorio, String.Format("{0}{1}.xml", PrefixoArquivo, sequenciaArquivo)));
                }

                xmlDoc.Save(Path.Combine(Diretorio, String.Format("{0}.xml", PrefixoArquivo)));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }

        private XmlControl GetXmlControl()
        {
            try
            {

                if (ArquivosGerados.Count <= 0 ||
                    ArquivosGerados[ArquivosGerados.Count - 1].QuantidadeURL >=
                    QuantidadeMaximaURLPorArquivo)
                {
                    ArquivosGerados.Add(new XmlControl());

                    XmlDocument xmlDoc = ArquivosGerados[ArquivosGerados.Count - 1].xmlDoc;

                    XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    XmlElement rootNode = xmlDoc.CreateElement("urlset");
                    rootNode.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                    xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                    xmlDoc.AppendChild(rootNode);
                }
                return ArquivosGerados[ArquivosGerados.Count - 1];
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
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

        private class XmlControl
        {
            public XmlDocument xmlDoc = new XmlDocument();
            public int QuantidadeURL;
        }
    }
}
