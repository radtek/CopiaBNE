using Bne.Web.Services.API.DTO.Enum;
using System;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.Query
{
    /// <summary>
    /// Parâmetros para a pesquisa de vaga
    /// </summary>
    [DataContract]
    public class QueryVagas
    {
        /// <summary>
        /// Página a ser retornada. Iniciando em 1.
        /// </summary>
        [DataMember(Name = "Pagina")]
        public int Pagina { get; set; } = 1;

        /// <summary>
        /// Número de registros por página
        /// </summary>
        [DataMember(Name = "RegistrosPorPagina")]
        public int RegistrosPorPagina { get; set; }

        /// <summary>
        /// Pesquisa textual
        /// </summary>
        [DataMember(Name = "Query")]
        public string Query { get; set; }

        /// <summary>
        /// Salário mínimo oferecido pela vaga
        /// </summary>
        [DataMember(Name = "SalarioMinimo")]
        public decimal? SalarioMinimo { get; set; }

        /// <summary>
        /// Salário máximo oferecido pela vaga
        /// </summary>
        [DataMember(Name = "SalarioMaximo")]
        public decimal? SalarioMaximo { get; set; }

        /// <summary>
        /// Tipo de vínculo desejado
        /// </summary>
        [DataMember(Name = "TipoVinculo")]
        public TipoVinculo[] TipoVinculo { get; set; }

        /// <summary>
        /// Funcão da vaga
        /// </summary>
        [DataMember(Name = "Funcao")]
        public string[] Funcao { get; set; }

        /// <summary>
        /// Área de atuação
        /// </summary>
        [DataMember(Name = "Area")]
        public string[] Area { get; set; }

        /// <summary>
        /// Escolaridade
        /// </summary>
        [DataMember(Name = "Escolaridade")]
        public string[] Escolaridade { get; set; }

        /// <summary>
        /// Deficiência para vagas BNE
        /// </summary>
        [DataMember(Name = "Deficiencia")]
        public Deficiencia[] Deficiencia { get; set; }

        /// <summary>
        /// Disponibilidade requerida para a vaga
        /// </summary>
        /// @Todo Tem que indexar no Solr
        [DataMember(Name = "Disponibilidade")]
        public Disponibilidade[] Disponibilidade { get; set; }

        /// <summary>
        /// Nomes das cidades desejadas
        /// </summary>
        [DataMember(Name = "SiglaEstado")]
        public string[] SiglaEstado { get; set; }

        /// <summary>
        /// Nomes das cidades desejadas
        /// </summary>
        [DataMember(Name = "NomeCidade")]
        public string[] NomeCidade { get; set; }

        /// <summary>
        /// Nomes das empresas anunciantes
        /// </summary>
        [DataMember(Name = "Empresa")]
        public string[] Empresa { get; set; }

        /// <summary>
        /// Limite inferior para a abertura da vaga
        /// </summary>
        [DataMember(Name = "DataInicio")]
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// Limite superior para a abertura da vaga
        /// </summary>
        [DataMember(Name = "DataFim")]
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Origem das vagas, empresas que possuem STC
        /// </summary>
        [DataMember(Name = "IdOrigem")]
        public int[] IdOrigem { get; set; }
        
        /// <summary>
        /// Trazer oportunidade (vagas abertas a mais de 60 dias)
        /// </summary>
        [DataMember(Name = "Oportunidade")]
        public bool? Oportunidade { get; set; }

        /// <summary>
        /// Ordenação ex: "Dta_Abertura asc, Des_Funcao desc"
        /// </summary>
        [DataMember(Name = "Ordenacao")]
        public string Ordenacao { get; set; }

        /// <summary>
        /// Cursos pesquisados com like
        /// </summary>
        [DataMember(Name = "Curso")]
        public string[] Curso { get; set; }

        /// <summary>
        /// Pesquisa acertiva dos curso 
        /// </summary>
        [DataMember(Name = "IdCurso")]
        public int[] IdCurso { get; set; }
        /// <summary>
        /// Curriculo do BNE - para não trazer as vagas da empresa que o candidato bloqueou.
        /// </summary>
        [DataMember(Name = "Curriculo")]
        public int? Curriculo { get; set; }

        /// <summary>
        /// Vagas de empresa que oferecem curso
        /// </summary>
        [DataMember(Name ="OfereceCurso")]
        public bool? OfereceCurso { get; set; }

        /// <summary>
        /// Vai retoranar as vagas com funções similares a pesquisada.
        /// </summary>
        [DataMember(Name = "FuncaoAgrupadora")]
        public int? FuncaoAgrupadora { get; set; }

        /// <summary>
        /// Alem das vaga da cidade ira trazer as vagas da região metropolitana.
        /// </summary>
        [DataMember(Name = "CidadeRegiao")]
        public int? CidadeRegiao { get; set; }

        /// <summary>
        /// Buscar vagas da filial 
        /// </summary>
        [DataMember(Name ="IdfFilial")]
        public int? IdfFilial { get; set; }
        /// <summary>
        /// Mostrar vagas Confidenciais (vaga de Empresas que optaram a não aparecer como anunciante na vaga)
        /// </summary>
        [DataMember(Name = "Confidencial")]
        public bool? Confidencial { get; set; }

        /// <summary>
        /// Vaga de campanha
        /// </summary>
        [DataMember(Name = "Campanha")]
        public bool? Campanha { get; set; }

        /// <summary>
        /// Quem Anunciou a vaga.
        /// </summary>
        [DataMember(Name = "UsuarioFilial")]
        public int[] UsuarioFilial { get; set; }

        /// <summary>
        /// Campos para os quais a totalização deve ser retornada
        /// </summary>
        [DataMember(Name = "FacetField")]
        public VagaFacetField[] FacetField { get; set; }
    }
}