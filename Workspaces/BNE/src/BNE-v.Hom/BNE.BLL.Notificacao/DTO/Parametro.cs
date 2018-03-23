namespace BNE.BLL.Notificacao.DTO
{
    public struct Parametro
    {
        public string nome { get; set; }
        public int num_vagas { get; internal set; }
        public string link_escolha_plano { get; set; }
        public string url_salavip { get; set; }
        public string link_propaganda { get; set; }
        public string url_vagas_no_perfil { get; set; }
        public string link_quemmeviu { get; set; }
        public int quemmeviu_qtd { get; set; }
        public string quemmeviu { get; set; }
        public string funcao_cidade { get; set; }
        public int qtd_funcao_cidade { get; set; }
        public string facets_funcao_cidade { get; set; }
        public string funcao_area { get; set; }
        public int qtd_funcao_area { get; set; }
        public string facets_funcao_area { get; set; }
        public string link_cadastro { get; set; }
        public string quantidade { get; set; }
        public string quantidade_vagas { get; set; }
    }
}
