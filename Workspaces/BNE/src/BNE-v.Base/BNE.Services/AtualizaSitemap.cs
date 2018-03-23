using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using BNE.BLL.Custom;
using BNE.Services.Code;
using BNE.Services.Properties;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BNE.Services
{
    partial class AtualizaSitemap : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly int DelayExecucao = Settings.Default.AtualizaSitemapDelayMinutos;
        private const string EventSourceName = "AtualizaSitemap";
        #endregion

        #region Construtores
        public AtualizaSitemap()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarAtualizaSitemap);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #endregion

        #region Metodos

        #region IniciarAtualizaSitemap
        public void IniciarAtualizaSitemap()
        {
            try
            {
                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                    #region Geral

                    var objSitemapGeral = new SiteMapControl(TipoSitemap.Geral, "sitemap");

                    var listaEstaticaSitemap = new List<string>
                        {
                            "agradecimentos",
                            "anunciar-vaga-gratis",
                            "cadastro-de-curriculo-gratis",
                            "cadastro-de-empresa-gratis",
                            "fale-com-presidente",
                            "login-candidato",
                            "Login-selecionadora",
                            "onde-estamos",
                            "lista-de-curriculos",
                            "vagas-de-emprego",
                            "pesquisa-de-vagas",
                            "vip",
                            "cursos",
                            "R1-recrutamento",
                            "recrutamento-R1",
                            "simulacao-de-entrega-do-recrutamento-R1",
                            "detalhes-do-recrutamento-R1",
                            "emitir-proposta-de-recrutamento-R1"
                        };

                    Parallel.ForEach(listaEstaticaSitemap, new ParallelOptions { MaxDegreeOfParallelism = 4 }, listaEstatica =>
                    {
                        var url = SitemapHelper.MontarUrl(listaEstatica);

                        objSitemapGeral.AdicionarURL(url, SiteMapControl.changefreq.daily, "0.8");
                    });

                    objSitemapGeral.SaveFiles();

                    #endregion

                    #region Vaga

                    var objSitemapVaga = new SiteMapControl(TipoSitemap.Vaga, "sitemapVaga");

                    var listaVagaSitemap = BLL.Vaga.RecuperarVagasSiteMap();
                    var listaPalavrasSitemap = BLL.CloudTag.PalavraFuncaoVaga.RecuperarPalavrasSitemap();

                    var lista = listaVagaSitemap.Concat(listaPalavrasSitemap);

                    Parallel.ForEach(lista, new ParallelOptions { MaxDegreeOfParallelism = 4 }, vaga =>
                    {
                        var descricaoFuncao = vaga.DescricaoFuncao;
                        var nomeCidade = vaga.NomeCidade;
                        var siglaEstado = vaga.SiglaEstado;
                        var areaBNE = vaga.DescricaoAreaBNE;
                        var palavraChave = vaga.PalavraChave;

                        string url = string.Empty;

                        if (vaga.IdfVaga.HasValue)
                            url = SitemapHelper.MontarUrlVaga(descricaoFuncao, areaBNE, nomeCidade, siglaEstado, vaga.IdfVaga.Value);
                        else if (string.IsNullOrWhiteSpace(descricaoFuncao).Equals(false) && string.IsNullOrWhiteSpace(nomeCidade).Equals(false) && string.IsNullOrWhiteSpace(siglaEstado).Equals(false))
                            url = SitemapHelper.MontarUrlVagasPorFuncaoCidade(descricaoFuncao, nomeCidade, siglaEstado);
                        else if (string.IsNullOrWhiteSpace(descricaoFuncao).Equals(false) && string.IsNullOrWhiteSpace(nomeCidade).Equals(true) && string.IsNullOrWhiteSpace(siglaEstado).Equals(true))
                            url = SitemapHelper.MontarUrlVagasPorFuncao(descricaoFuncao);
                        else if (string.IsNullOrWhiteSpace(descricaoFuncao).Equals(true) && string.IsNullOrWhiteSpace(nomeCidade).Equals(false) && string.IsNullOrWhiteSpace(siglaEstado).Equals(false))
                            url = SitemapHelper.MontarUrlVagasPorCidade(nomeCidade, siglaEstado);
                        else if (string.IsNullOrWhiteSpace(palavraChave).Equals(false))
                            url = SitemapHelper.MontarUrlVagasPorPalavraChave(palavraChave);

                        if (!string.IsNullOrWhiteSpace(url))
                            objSitemapVaga.AdicionarURL(url, SiteMapControl.changefreq.daily, "0.8");
                    });

                    objSitemapVaga.SaveFiles();

                    #endregion

                    #region Currículo

                    var objSitemapCurriculo = new SiteMapControl(TipoSitemap.Curriculo, "sitemapCurriculo");

                    var listaCurriculoSitemap = BLL.Curriculo.RecuperarCurriculosSiteMap();

                    Parallel.ForEach(listaCurriculoSitemap, new ParallelOptions { MaxDegreeOfParallelism = 4 }, curriculo =>
                        {
                            var descricaoFuncao = curriculo.DescricaoFuncao;
                            var nomeCidade = curriculo.NomeCidade;
                            var siglaEstado = curriculo.SiglaEstado;

                            string url = string.Empty;

                            if (curriculo.IdfCurriculo.HasValue)
                                url = SitemapHelper.MontarUrlCurriculo(descricaoFuncao, nomeCidade, siglaEstado, curriculo.IdfCurriculo.Value);
                            else if (string.IsNullOrWhiteSpace(descricaoFuncao).Equals(false) && string.IsNullOrWhiteSpace(nomeCidade).Equals(false) && string.IsNullOrWhiteSpace(siglaEstado).Equals(false))
                                url = SitemapHelper.MontarUrlCurriculosPorFuncaoCidade(descricaoFuncao, nomeCidade, siglaEstado);
                            else if (string.IsNullOrWhiteSpace(descricaoFuncao).Equals(false) && string.IsNullOrWhiteSpace(nomeCidade).Equals(true) && string.IsNullOrWhiteSpace(siglaEstado).Equals(true))
                                url = SitemapHelper.MontarUrlCurriculosPorFuncao(descricaoFuncao);
                            else if (string.IsNullOrWhiteSpace(descricaoFuncao).Equals(true) && string.IsNullOrWhiteSpace(nomeCidade).Equals(false) && string.IsNullOrWhiteSpace(siglaEstado).Equals(false))
                                url = SitemapHelper.MontarUrlCurriculosPorCidade(nomeCidade, siglaEstado);

                            objSitemapCurriculo.AdicionarURL(url, SiteMapControl.changefreq.daily, "0.8");
                        });

                    objSitemapCurriculo.SaveFiles();

                    #endregion

                    #region Empresa

                    var objSitemapEmpresa = new SiteMapControl(TipoSitemap.Empresa, "sitemapEmpresa");

                    var listaOrigensSitemap = BLL.Origem.RecuperarOrigensSiteMap();

                    Parallel.ForEach(listaOrigensSitemap, new ParallelOptions { MaxDegreeOfParallelism = 4 }, origemSitemap =>
                        {
                            var url = SitemapHelper.MontarUrlEmpresa(origemSitemap.DesDiretorio);

                            objSitemapEmpresa.AdicionarURL(url, SiteMapControl.changefreq.daily, "0.8");
                        });

                    objSitemapEmpresa.SaveFiles();

                    #endregion

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);

                    AjustarThread(DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaFinal)
        {
            var tempoTotalExecucao = horaFinal - _dataHoraUltimaExecucao;
            var delay = new TimeSpan(0, DelayExecucao, 0);
            if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
            {
                EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
            }
            else
                EventLogWriter.LogEvent(EventSourceName, String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
        }
        #endregion

        #endregion

        #region SiteMapControl
        public class SiteMapControl
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

            public SiteMapControl(TipoSitemap tipoSitemap, string prefixoArquivo)
            {
                ArquivosGerados = new List<XmlControl>();
                TipoSitemap = tipoSitemap;
                PrefixoArquivo = prefixoArquivo;
                QuantidadeMaximaURLPorArquivo = Convert.ToInt32(BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.SitemapQuantidadeURLPorArquivo));
                DiretorioAplicacao = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.DiretorioAplicacao);
                DiretorioAplicacaoVagas = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.DiretorioAplicacaoWebVagas);
                string urlSite = string.Concat("http://", BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLAmbiente));
                string urlSiteVaga = string.Concat("http://", BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLVagas));

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
                        XmlControl xmlControl = GetXMLControl();

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
                    string message;
                    var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                    message = string.Format("{0} - {1}", id, message);
                    EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
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
                    string message;
                    var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                    message = string.Format("{0} - {1}", id, message);
                    EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                }
            }

            private XmlControl GetXMLControl()
            {
                try
                {

                    if (this.ArquivosGerados.Count <= 0 ||
                        this.ArquivosGerados[this.ArquivosGerados.Count - 1].QuantidadeURL >=
                        this.QuantidadeMaximaURLPorArquivo)
                    {
                        this.ArquivosGerados.Add(new XmlControl());

                        XmlDocument xmlDoc = this.ArquivosGerados[this.ArquivosGerados.Count - 1].xmlDoc;

                        XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                        XmlElement rootNode = xmlDoc.CreateElement("urlset");
                        rootNode.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                        xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                        xmlDoc.AppendChild(rootNode);
                    }
                    return this.ArquivosGerados[this.ArquivosGerados.Count - 1];
                }
                catch (Exception ex)
                {
                    string message;
                    var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                    message = string.Format("{0} - {1}", id, message);
                    EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                }
                return null;
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

        #endregion

        #region TipoSitemap
        public enum TipoSitemap
        {
            Curriculo,
            Vaga,
            Empresa,
            Geral
        }
        #endregion

    }
}
