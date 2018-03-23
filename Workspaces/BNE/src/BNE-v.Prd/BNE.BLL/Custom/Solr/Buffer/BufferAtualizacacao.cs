using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;

namespace BNE.BLL.Custom.Solr.Buffer
{
    public class BufferAtualizacao
    {
        private readonly Queue<int> _buffer = new Queue<int>();
        private Thread _objThread;
        private readonly bool AtualizaBuffer = Convert.ToBoolean(Convert.ToInt16(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarSolrImediatamente)));
        private readonly int TamanhoBuffer = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarSolrImediatamenteTamanhoBuffer));
        private readonly int Timeout = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarSolrImediatamenteTimeout));
        public string URL { get; set; }

        public BufferAtualizacao(string url)
        {
            if (_objThread != null)
            {
                return;
            }
            _objThread = new Thread(TimeOutLog);
            _objThread.Start();
            URL = url;
        }

        ~BufferAtualizacao()
        {
            GravaBuffer();
        }

        internal void Add(Curriculo objCurriculo)
        {
            lock (_buffer)
            {
                if (_buffer.Count >= TamanhoBuffer)
                    GravaBuffer();

                if (!_buffer.Contains(objCurriculo.IdCurriculo))
                    _buffer.Enqueue(objCurriculo.IdCurriculo);
            }
        }

        private void TimeOutLog()
        {
            while (AtualizaBuffer)
            {
                Thread.Sleep(Timeout);
                GravaBuffer();
            }
        }

        private void GravaBuffer()
        {
            lock (_buffer)
            {
                if (_buffer.Count > 0)
                {
                    DispararThread(_buffer.ToList());
                    _buffer.Clear();
                }
            }
        }

        #region EnviarSolr
        private string EnviarSolr(List<int> lista)
        {
            string retorno = string.Empty;

            var urlSolr = URL + string.Join(",", lista);

            try
            {
                var request = WebRequest.Create(urlSolr);
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var responseStream = response.GetResponseStream()) // Load the response stream
                using (var streamReader = new StreamReader(responseStream)) // Load the stream reader to read the response
                {
                    var siteContent = streamReader.ReadToEnd(); // Read the entire response and store it in the siteContent variable
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(siteContent);
                    var node = doc.SelectSingleNode("response/str[@name='status']");

                    if (node != null)
                        retorno = node.InnerText;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "URL Solr: " + urlSolr);
            }
            return retorno;
        }
        #endregion

        #region DispararThread
        private void DispararThread(List<int> lista)
        {
            try
            {
                new Thread(() =>
                {
                    try
                    {
                        Thread.CurrentThread.IsBackground = true;
                        var retorno = "busy";
                        while (retorno == "busy")
                        {
                            Thread.Sleep(10000);
                            retorno = EnviarSolr(lista);
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Inside thread Count: " + lista.Count);
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

    }

}
