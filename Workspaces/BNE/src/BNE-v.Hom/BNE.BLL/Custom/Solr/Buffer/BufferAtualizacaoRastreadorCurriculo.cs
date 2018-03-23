namespace BNE.BLL.Custom.Solr.Buffer
{
    public class BufferAtualizacaoRastreadorCurriculo : BufferAtualizacao
    {
        //        Buffer.Add(UpdateType.DelecaoResultadoRastreador, new SolrObject { Queue = new Queue<int>(), URL = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlSolrLimparResultadoRastreador) });

        public BufferAtualizacaoRastreadorCurriculo()
            : this(Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlSolrAtualizaRastreador))
        {
        }

        public BufferAtualizacaoRastreadorCurriculo(string url)
            : base(url)
        {
        }
    }
}