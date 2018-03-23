using System;

namespace BNE.BLL.DTO
{
    public class VagaAPI
    {

        public Registro[] Registros { get; set; }
        public object[] Facets { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }
    }

    public class Registro
    {
        public int Id { get; set; }
        public string[] TipoVinculo { get; set; }
        public string[] Cursos { get; set; }
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public int Quantidade { get; set; }
        public object Escolaridade { get; set; }
        public float? SalarioMin { get; set; }
        public float? SalarioMax { get; set; }
        public object Beneficios { get; set; }
        public object Requisitos { get; set; }
        public string Atribuicoes { get; set; }
        public object[] Disponibilidade { get; set; }
        public string NomeFantasia { get; set; }
        public bool Confidencial { get; set; }
        public Pergunta[] Perguntas { get; set; }
        public string Deficiencia { get; set; }
        public string Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public string SiglaEstado { get; set; }
        public DateTime DataAnuncio { get; set; }
        public string DesOrigem { get; set; }
        public int TipoOrigem { get; set; }
        public bool BNERecomenda { get; set; }
        public string CodigoVaga { get; set; }
        public int IdFilial { get; set; }
        public string Bairro { get; set; }
        public string Area { get; set; }
        public string Url { get; set; }
        public bool Plano { get; set; }
        public bool Oferece_Cursos { get; set; }
    }

    public class Rootobject
    {
        public Registro[] Registros { get; set; }
        public object[] Facets { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }
    }

    public class Pergunta
    {
        public int IdPergunta { get; set; }
        public string Texto { get; set; }
    }


}
