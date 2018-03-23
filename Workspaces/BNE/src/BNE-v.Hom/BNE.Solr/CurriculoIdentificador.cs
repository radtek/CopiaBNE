namespace BNE.Solr
{
    public class CurriculoIdentificador : SolrResponse<CurriculoIdentificador.Response>
    {
        public class Response : Docs
        {
            public int Idf_Curriculo { get; set; }
        }
    }
}
