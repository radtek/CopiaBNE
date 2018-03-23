using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using BNE.EL;

namespace BNE.BLL.Custom
{
    public class BufferBloquearEmpresa
    {
        private static Dictionary<UpdateType, SolrObjectBloqueio> Buffer = new Dictionary<UpdateType, SolrObjectBloqueio>();
        private static Thread _objThread;
        private static bool _atualizaBuffer = Convert.ToBoolean(Convert.ToInt16(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarSolrImediatamente)));
        private static readonly int TamanhoBuffer = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarSolrImediatamenteTamanhoBuffer));
        private static readonly int Timeout = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DeveAtualizarSolrImediatamenteTimeout));
        private enum UpdateType
        {
            BloquearEmpresa
        }
        ~BufferBloquearEmpresa()
        {
            foreach (var key in Buffer.Keys)
            {
                GravaBuffer(key);
            }
        }

        private static void Inicializar()
        {
            if (_objThread == null)
            {
                Buffer.Add(UpdateType.BloquearEmpresa, new SolrObjectBloqueio { Queue = new Queue<int>(), URL = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlSolrAtualizaBloqueioEmpresa) });

                _objThread = new Thread(TimeOutLog);
                _objThread.Start();
            }
        }


        public static void bloquear(int IdCurriculo)
        {
            Add(IdCurriculo, UpdateType.BloquearEmpresa);
        }

        private static void Add(int IdCurriculo, UpdateType enumerador)
        {
            try
            {
                lock (Buffer)
                {
                    Inicializar();

                    foreach (var key in Buffer.Keys)
                    {
                        if (Buffer[key].Queue.Count >= TamanhoBuffer)
                            GravaBuffer(key);
                    }

                    if (!Buffer[enumerador].Queue.Contains(IdCurriculo))
                        Buffer[enumerador].Queue.Enqueue(IdCurriculo);
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }

        public static void Stop()
        {
            lock (Buffer)
            {
                foreach (var key in Buffer.Keys)
                {
                    GravaBuffer(key);
                }
                _atualizaBuffer = false;
            }
            try
            {
                if (!_objThread.IsAlive || (_objThread.ThreadState & ThreadState.WaitSleepJoin) != ThreadState.WaitSleepJoin)
                    return;
                _objThread.Abort();
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }

        private static void TimeOutLog()
        {
            while (_atualizaBuffer)
            {
                Thread.Sleep(Timeout);
                lock (Buffer)
                {
                    foreach (var key in Buffer.Keys)
                    {
                        GravaBuffer(key);
                    }
                }
            }
        }

        private static void GravaBuffer(UpdateType enumerador)
        {
            lock (Buffer)
            {
                try
                {
                    DispararThread(Buffer[enumerador]);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
               
            }
        }

        #region EnviarSolr
        private static string EnviarSolr(SolrObjectBloqueio solrObject)
        {
            string retorno = string.Empty;

            var urlSolr = solrObject.URL + string.Join(",", solrObject.Queue.ToList());

            try
            {
                var request = WebRequest.Create(urlSolr);
                using (var response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream()) // Load the response stream
                using (StreamReader streamReader = new StreamReader(responseStream)) // Load the stream reader to read the response
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
        private static void DispararThread(SolrObjectBloqueio solrObject)
        {
            lock (Buffer)
            {
                if (solrObject.Queue.Count > 0)
                {
                    var threadObject = new SolrObjectBloqueio()
                    {
                        Queue = new Queue<int>(solrObject.Queue),
                        URL = solrObject.URL
                    };

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        var retorno = "busy";
                        while (retorno == "busy")
                        {
                            Thread.Sleep(1000);
                            retorno = EnviarSolr(threadObject);
                        }
                    }).Start();

                    solrObject.Queue.Clear();
                }
            }
        }
        #endregion

    }

    internal class SolrObjectBloqueio
    {
        internal Queue<int> Queue { get; set; }
        internal string URL { get; set; }
    }
}