namespace BNE.BLL.Custom.Solr.Buffer
{
    public static class BufferAtualizacaoCurriculo
    {
        private static BufferAtualizacao _buffer;

        private static BufferAtualizacao Buffer
        {
            get { return _buffer ?? (_buffer = new BufferAtualizacao(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrAtualizaCV))); }
        }

        public static void Update(Curriculo objCurriculo)
        {
            Buffer.Add(objCurriculo);
        }
    }
}