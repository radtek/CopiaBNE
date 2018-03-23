namespace BNE.BLL.Custom.Solr.Buffer
{
    public class BufferAtualizacaoSMSCurriculo
    {
        private static BufferAtualizacao _buffer;

        private static BufferAtualizacao Buffer
        {
            get { return _buffer ?? (_buffer = new BufferAtualizacao(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrAtualizaSms))); }
        }

        public static void Update(Curriculo objCurriculo)
        {
            Buffer.Add(objCurriculo);
        }
    }
}