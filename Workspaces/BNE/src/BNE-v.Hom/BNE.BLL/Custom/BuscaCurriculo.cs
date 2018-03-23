using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BNE.Solr;
using Microsoft.SqlServer.Types;

namespace BNE.BLL.Custom
{
    public class BuscaCurriculo
    {
        public class Parametros
        {
            public Origem Origem { get; set; }
            public Filial Filial { get; set; }
            public UsuarioFilialPerfil UsuarioFilialPerfil { get; set; }
            public List<int> Funcoes { get; set; }
            public List<KeyValuePair<int, int>> Idiomas { get; set; }
            public List<int> Disponibilidades { get; set; }
            public Cidade Cidade { get; set; }
            public Cidade CidadeDisponibilidade { get; set; }
            public Estado Estado { get; set; }
            public SqlGeography GeoBuscaCidade { get; set; }
            public SqlGeography GeoBuscaBairro { get; set; }
            public Escolaridade Escolaridade { get; set; }
            public List<int> Deficiencia { get; set; }
            public Sexo Sexo { get; set; }
            public Raca Raca { get; set; }
            public EstadoCivil EstadoCivil { get; set; }
            public TipoVeiculo TipoVeiculo { get; set; }
            public CategoriaHabilitacao CategoriaHabilitacao { get; set; }
            public AreaBNE AreaBNE { get; set; }
            public short? IdadeMinima { get; set; }
            public short? IdadeMaxima { get; set; }
            public string Bairro { get; set; }
            public string Zona { get; set; }
            public string Logradouro { get; set; }
            public string NumeroCEPInicial { get; set; }
            public string NumeroCEPFinal { get; set; }
            public string CodigoCPFNome { get; set; }
            public string DDDTelefone { get; set; }
            public string Telefone { get; set; }
            public string Email { get; set; }
            public string PalavraChave { get; set; }
            public List<string> CamposPalavraChave { get; set; }
            public string FiltroExcludente { get; set; }
            public decimal? ValorSalarioMinimo { get; set; }
            public decimal? ValorSalarioMaximo { get; set; }
            public short? QuantidadeExperiencia { get; set; }
            public bool? Filhos { get; set; }
            public bool? Aprendiz { get; set; }
            public string EscolaridadesWebEstagios { get; set; }
            public string RazaoSocial { get; set; }
            public string Experiencia { get; set; }
            public Funcao FuncaoExercida { get; set; }
            public string DescricaoFuncaoExercida { get; set; }

            public string Avaliacao { get; set; }
            public List<int> Avaliacoes { get; set; }

            public Curso CursoTecnicoGraduacao { get; set; }
            public string DescricaoCursoTecnicoGraduacao { get; set; }
            public Fonte FonteTecnicoGraduacao { get; set; }

            public Curso CursoOutros { get; set; }
            public string DescricaoCursoOutros { get; set; }
            public Fonte FonteOutros { get; set; }

            public ParametrosFormacao Formacao { get; set; }

            public DateTime DataInicial { get; set; }
            public Tipo TipoBusca { get; set; }

            /// <summary>
            /// Escolaridade do filtro da modal dos candidatos a vaga
            /// </summary>

            public enum Tipo
            {
                Curriculo,
                Vaga,
                Rastreador
            }
        }

        public class ParametrosFormacao
        {
            /// <summary>
            /// Nível do curso. Um dos valores presentes na Tabela Escolaridades.
            /// </summary>
            public Escolaridade Escolaridade { get; set; }

            /// <summary>
            /// Nome do curso procurado
            /// </summary>
            public Curso Curso { get; set; }

            /// <summary>
            /// Instituição de ensino
            /// </summary>
            public Fonte Fonte { get; set; }

            /// <summary>
            /// Ano de conclusao do curso
            /// </summary>
            public int? AnoConclusao { get; set; }

            /// <summary>
            /// Período do curso
            /// </summary>
            public int? Periodo { get; set; }

            /// <summary>
            /// Situação do curso de nível Médio Incompleto, Técnico/Pós-Médio Incompleto, Tecnólogo Incompleto ou Superior Incompleto
            /// </summary>
            public SituacaoFormacao SituacaoCurso { get; set; }

            /// <summary>
            /// Cidade do curso
            /// </summary>
            public Cidade Cidade { get; set; }
        }

        #region Pesquisa Curriculo
        /// <summary>
        /// Query para paginação dos resultados na busca de currículo
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="objFilial"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objPesquisaCurriculo"></param>
        /// <returns></returns>
        public static string MontarQuery(int currentIndex, int pageSize, bool CidadeDisponibilidade, Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, bool buscaCurriculosMaisUmAno = false)
        {
            return MontarQueryBuscaCurriculo(currentIndex, pageSize, RecuperarParametros(objFilial, objUsuarioFilialPerfil, objPesquisaCurriculo, CidadeDisponibilidade), buscaCurriculosMaisUmAno);
        }
        #endregion

        #region Vaga
        public static string MontarQuery(int currentIndex, int pageSize, bool CidadeDisponibilidade, Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, BLL.Vaga objVaga)
        {
            return MontarQueryBuscaCurriculo(currentIndex, pageSize, RecuperarParametros(objVaga, CidadeDisponibilidade));
        }

        /// <summary>
        /// Query para recuperar apenas a quantidade de curriculos com perfil proximo ao da vaga 
        /// </summary>
        /// <param name="objVaga"></param>
        /// <returns></returns>
        public static string MontarQuery(BLL.Vaga objVaga, bool CidadeDisponibilidade)
        {
            return MontarQueryBuscaCurriculo(0, 0, RecuperarParametros(objVaga, CidadeDisponibilidade));
        }

        public static SolrResponse<CurriculoCompleto.Response> MontarQueryCandidatosVaga(int currentIndex, int pageSize, int idFilial, int? idUsuarioFilialPerfil, int? idVaga, FiltroCurriculoVaga filtroCurriucloVaga, bool? Visualizados = null)
        {

            #region [Listar Candidatos da Vaga de Acordo com os Filtros]
            ///
            var listaCand = BNE.Solr.VagaCandidato.EfetuarRequisicao(Solr.BuscaVagaCandidato.MontaUrlSolr(currentIndex, pageSize, idVaga.Value, Visualizados, filtroCurriucloVaga));
            List<int> listaCandidatos = null;
            if (listaCand != null)
            {
                listaCandidatos = new List<int>();

                foreach (var item in listaCand.response.docs)
                {
                    listaCandidatos.Add(item.Idf_Curriculo);
                }
            }
            #endregion

            var resultado = CurriculoCompleto.EfetuarRequisicao(MontarQueryBuscaCurriculoCandidato(0, pageSize, listaCandidatos));

            #region [Informar se esta dentro do perfil da vaga]
            foreach (var item in resultado.response.docs)
            {
                item.Dentro_Perfil = listaCand.response.docs.Find(s => s.Idf_Curriculo.Equals(item.Idf_Curriculo)).Perfil;
                item.Flg_Auto_Candidatura = listaCand.response.docs.Find(s => s.Idf_Curriculo.Equals(item.Idf_Curriculo)).Flg_Auto_Candidatura;
            }
            #endregion

            resultado.response.numFound = listaCand.response.numFound;
            return resultado;
        }

        public static string MontarQueryAutoCandidatura(int currentIndex, int pageSize, BLL.Vaga objVaga)
        {
            return MontarQueryBuscaCurriculo(currentIndex, pageSize, RecuperarParametrosAutoCandidatura(objVaga));
        }
        #endregion

        #region Rastreador de Curriculo
        /// <summary>
        /// Query para busca de currículos de um rastreador
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="objRastreadorCurriculo"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public static string MontarQuery(int currentIndex, int pageSize, bool CidadeDisponibilidade, RastreadorCurriculo objRastreadorCurriculo, DateTime? ultimaVisualizacao = null)
        {
            var parametros = RecuperarParametros(objRastreadorCurriculo, CidadeDisponibilidade);
            if (ultimaVisualizacao.HasValue)
                parametros.DataInicial = ultimaVisualizacao.Value;
            else
                parametros.DataInicial = objRastreadorCurriculo.DataCadastro.Value;

            if (CidadeDisponibilidade)
                parametros.CidadeDisponibilidade = parametros.Cidade;

            return MontarQueryBuscaCurriculo(currentIndex, pageSize, parametros);
        }
        public static string MontarQuery(RastreadorCurriculo objRastreadorCurriculo)
        {
            var parametros = RecuperarParametros(objRastreadorCurriculo);

            //TODO: Poderia pega a data do ultimo curriculo rastreado para diminuir o range de data e assim trazer menos currículos do SOLR.
            parametros.DataInicial = objRastreadorCurriculo.DataCadastro.Value;

            return MontarQueryBuscaCurriculo(0, Int32.MaxValue, parametros);
        }
        public static string MontarQueryCurriculosNaoVisulizados(RastreadorCurriculo objRastreadorCurriculo)
        {
            objRastreadorCurriculo.CompleteObject();

            var parametros = RecuperarParametros(objRastreadorCurriculo);
            if (objRastreadorCurriculo.DataVisualizacao != null)
                parametros.DataInicial = objRastreadorCurriculo.DataVisualizacao.Value;

            return MontarQueryBuscaCurriculo(0, Int32.MaxValue, parametros);
        }
        #endregion

        #region RecuperarParametros
        private static Parametros RecuperarParametros(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, bool CidadeDisponibilidade, BLL.Custom.FiltroCurriculoVaga FiltroCurriculoVaga = null)
        {
            var ret = new Parametros();
            if (objPesquisaCurriculo != null)
            {

                ret = new Parametros
                {
                    TipoBusca = Parametros.Tipo.Curriculo,

                    Origem = objPesquisaCurriculo.Origem,
                    Filial = objFilial,
                    UsuarioFilialPerfil = objUsuarioFilialPerfil,
                    Funcoes = PesquisaCurriculoFuncao.ListarIdentificadoresFuncaoPorPesquisa(objPesquisaCurriculo),
                    Idiomas = PesquisaCurriculoIdioma.ListarIdentificadoresIdiomaPorPesquisa(objPesquisaCurriculo),
                    Disponibilidades = PesquisaCurriculoDisponibilidade.ListarIdentificadoresDisponibilidadePorPesquisa(objPesquisaCurriculo),
                    Cidade = objPesquisaCurriculo.Cidade,
                    CidadeDisponibilidade = CidadeDisponibilidade ? objPesquisaCurriculo.Cidade : null,
                    Estado = objPesquisaCurriculo.Estado,
                    GeoBuscaCidade = objPesquisaCurriculo.GeoBuscaCidade,
                    GeoBuscaBairro = objPesquisaCurriculo.GeoBuscaBairro,
                    Escolaridade = objPesquisaCurriculo.Escolaridade,
                    Deficiencia = PesquisaCurriculoDeficiencia.ListarIdentificadoresDeficienciaPorPesquisa(objPesquisaCurriculo),
                    Sexo = objPesquisaCurriculo.Sexo,
                    Raca = objPesquisaCurriculo.Raca,
                    EstadoCivil = objPesquisaCurriculo.EstadoCivil,
                    TipoVeiculo = objPesquisaCurriculo.TipoVeiculo,
                    CategoriaHabilitacao = objPesquisaCurriculo.CategoriaHabilitacao,
                    AreaBNE = objPesquisaCurriculo.AreaBNE,

                    IdadeMinima = objPesquisaCurriculo.NumeroIdadeMin,
                    IdadeMaxima = objPesquisaCurriculo.NumeroIdadeMax,
                    ValorSalarioMinimo = objPesquisaCurriculo.NumeroSalarioMin.HasValue && objPesquisaCurriculo.NumeroSalarioMin > 0 ? objPesquisaCurriculo.NumeroSalarioMin : null,
                    ValorSalarioMaximo = objPesquisaCurriculo.NumeroSalarioMax.HasValue && objPesquisaCurriculo.NumeroSalarioMax > 0 ? objPesquisaCurriculo.NumeroSalarioMax : null,

                    //PalavraChave
                    PalavraChave = objPesquisaCurriculo.DescricaoPalavraChave,
                    CamposPalavraChave = CampoPalavraChavePesquisaCurriculo.ListarPalavraChave(objPesquisaCurriculo),
                    FiltroExcludente = objPesquisaCurriculo.DescricaoFiltroExcludente,

                    //Endereço
                    Bairro = objPesquisaCurriculo.DescricaoBairro,
                    Zona = objPesquisaCurriculo.DescricaoZona,
                    Logradouro = objPesquisaCurriculo.DescricaoLogradouro,
                    NumeroCEPInicial = objPesquisaCurriculo.NumeroCEPMin,
                    NumeroCEPFinal = objPesquisaCurriculo.NumeroCEPMax,

                    //Formação
                    CursoTecnicoGraduacao = objPesquisaCurriculo.CursoTecnicoGraduacao,
                    DescricaoCursoTecnicoGraduacao = objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao,
                    FonteTecnicoGraduacao = objPesquisaCurriculo.FonteTecnicoGraduacao,
                    CursoOutros = objPesquisaCurriculo.CursoOutrosCursos,
                    DescricaoCursoOutros = objPesquisaCurriculo.DescricaoCursoOutrosCursos,
                    FonteOutros = objPesquisaCurriculo.FonteOutrosCursos,

                    //Específicos
                    CodigoCPFNome = objPesquisaCurriculo.DescricaoCodCPFNome,
                    DDDTelefone = objPesquisaCurriculo.NumeroDDDTelefone,
                    Telefone = objPesquisaCurriculo.NumeroTelefone,
                    Email = objPesquisaCurriculo.EmailPessoa,

                    //Experiencia
                    RazaoSocial = objPesquisaCurriculo.RazaoSocial,
                    Experiencia = objPesquisaCurriculo.DescricaoExperiencia,
                    FuncaoExercida = objPesquisaCurriculo.FuncaoExercida,
                    DescricaoFuncaoExercida = objPesquisaCurriculo.DescricaoFuncaoExercida,
                    QuantidadeExperiencia = objPesquisaCurriculo.QuantidadeExperiencia,

                    Filhos = objPesquisaCurriculo.FlagFilhos,
                    Aprendiz = objPesquisaCurriculo.FlagAprendiz,
                    EscolaridadesWebEstagios = objPesquisaCurriculo.IdEscolaridadeWebStagio,

                    Avaliacao = objPesquisaCurriculo.DescricaoAvaliacao,
                    Avaliacoes = objPesquisaCurriculo.Avaliacoes.Select(x => x.Avaliacao.IdAvaliacao).ToList()
                };

                #region  Pesquisa de formação
                if (objPesquisaCurriculo.EscolaridadeFormacao != null)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.Escolaridade = objPesquisaCurriculo.EscolaridadeFormacao;
                }

                if (objPesquisaCurriculo.CursoFormacao != null)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.Curso = objPesquisaCurriculo.CursoFormacao;
                }
                else if (objPesquisaCurriculo.DescricaoCursoFormacao != null)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.Curso = new Curso() { DescricaoCurso = objPesquisaCurriculo.DescricaoCursoFormacao };
                }

                if (objPesquisaCurriculo.FonteFormacao != null)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.Fonte = objPesquisaCurriculo.FonteFormacao;
                }
                else if (objPesquisaCurriculo.DescricaoFonteFormacao != null)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.Fonte = new Fonte() { NomeFonte = objPesquisaCurriculo.DescricaoFonteFormacao };
                }

                if (objPesquisaCurriculo.AnoConclusaoFormacao.HasValue)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.AnoConclusao = objPesquisaCurriculo.AnoConclusaoFormacao;
                }

                if (objPesquisaCurriculo.NumeroPeriodoFormacao.HasValue)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.Periodo = objPesquisaCurriculo.NumeroPeriodoFormacao;
                }

                if (objPesquisaCurriculo.SituacaoCursoFormacao != null)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.SituacaoCurso = objPesquisaCurriculo.SituacaoCursoFormacao;
                }

                if (objPesquisaCurriculo.CidadeFormacao != null)
                {
                    if (ret.Formacao == null) ret.Formacao = new ParametrosFormacao();

                    ret.Formacao.Cidade = objPesquisaCurriculo.CidadeFormacao;
                }

                #endregion  Pesquisa de formação
            }

            return ret;
        }

        private static Parametros RecuperarParametros(RastreadorCurriculo objRastreadorCurriculo, bool CidadeDisponibilidade = false)
        {

            return new Parametros
            {
                TipoBusca = Parametros.Tipo.Rastreador,

                Origem = objRastreadorCurriculo.Origem,
                Filial = objRastreadorCurriculo.Filial,
                UsuarioFilialPerfil = objRastreadorCurriculo.UsuarioFilialPerfil,
                Funcoes = RastreadorCurriculoFuncao.ListarIdentificadoresFuncaoPorRastreador(objRastreadorCurriculo),
                Idiomas = RastreadorCurriculoIdioma.ListarIdentificadoresIdiomaPorRastreador(objRastreadorCurriculo),
                Disponibilidades = RastreadorCurriculoDisponibilidade.ListarIdentificadoresDisponibilidadePorRastreador(objRastreadorCurriculo),
                Cidade = objRastreadorCurriculo.Cidade,
                CidadeDisponibilidade = CidadeDisponibilidade ? objRastreadorCurriculo.Cidade : null,
                Estado = objRastreadorCurriculo.Estado,
                GeoBuscaCidade = objRastreadorCurriculo.GeoBuscaCidade,
                GeoBuscaBairro = objRastreadorCurriculo.GeoBuscaBairro,
                Escolaridade = objRastreadorCurriculo.Escolaridade,
                //Deficiencia = objRastreadorCurriculo.Deficiencia,
                Sexo = objRastreadorCurriculo.Sexo,
                Raca = objRastreadorCurriculo.Raca,
                EstadoCivil = objRastreadorCurriculo.EstadoCivil,
                TipoVeiculo = objRastreadorCurriculo.TipoVeiculo,
                CategoriaHabilitacao = objRastreadorCurriculo.CategoriaHabilitacao,
                AreaBNE = objRastreadorCurriculo.AreaBNE,

                IdadeMinima = objRastreadorCurriculo.NumeroIdadeMin,
                IdadeMaxima = objRastreadorCurriculo.NumeroIdadeMax,
                ValorSalarioMinimo = objRastreadorCurriculo.NumeroSalarioMin,
                ValorSalarioMaximo = objRastreadorCurriculo.NumeroSalarioMax,

                //PalavraChave
                PalavraChave = objRastreadorCurriculo.DescricaoPalavraChave,
                CamposPalavraChave = CampoPalavraChaveRastreadorCurriculo.ListarPalavraChave(objRastreadorCurriculo),
                FiltroExcludente = objRastreadorCurriculo.DescricaoFiltroExcludente,

                //Endereço
                Bairro = objRastreadorCurriculo.DescricaoBairro,
                Zona = objRastreadorCurriculo.DescricaoZona,
                Logradouro = objRastreadorCurriculo.DescricaoLogradouro,
                NumeroCEPInicial = objRastreadorCurriculo.NumeroCEPMin,
                NumeroCEPFinal = objRastreadorCurriculo.NumeroCEPMax,

                //Formação
                CursoTecnicoGraduacao = objRastreadorCurriculo.CursoTecnicoGraduacao,
                DescricaoCursoTecnicoGraduacao = objRastreadorCurriculo.DescricaoCursoTecnicoGraduacao,
                FonteTecnicoGraduacao = objRastreadorCurriculo.FonteTecnicoGraduacao,
                CursoOutros = objRastreadorCurriculo.CursoOutrosCursos,
                DescricaoCursoOutros = objRastreadorCurriculo.DescricaoCursoOutrosCursos,
                FonteOutros = objRastreadorCurriculo.FonteOutrosCursos,

                //Experiencia
                Experiencia = objRastreadorCurriculo.DescricaoExperiencia,
                FuncaoExercida = objRastreadorCurriculo.FuncaoExercida,
                DescricaoFuncaoExercida = objRastreadorCurriculo.DescricaoFuncaoExercida,

                Filhos = objRastreadorCurriculo.FlagFilhos,
                Aprendiz = objRastreadorCurriculo.FlagAprendiz,
                EscolaridadesWebEstagios = objRastreadorCurriculo.IdEscolaridadeWebStagio,

                DataInicial = objRastreadorCurriculo.DataCadastro ?? DateTime.Now
            };
        }

        private static Parametros RecuperarParametros(BLL.Vaga objVaga, bool CidadeDisponibilidade)
        {
            List<int> listFunc = new List<int>();
            if (objVaga.Funcao != null)
                listFunc.Add(objVaga.Funcao.IdFuncao);
            var cidades = new List<int>();
            if (objVaga.Cidade != null)
                cidades.Add(objVaga.Cidade.IdCidade);

            var listPcd = new List<int>();
            if (objVaga.Deficiencia != null)
                listPcd.Add(objVaga.Deficiencia.IdDeficiencia);

            return new Parametros
            {
                TipoBusca = Parametros.Tipo.Vaga,
                Filial = objVaga.Filial,
                UsuarioFilialPerfil = objVaga.UsuarioFilialPerfil,
                Funcoes = listFunc,
                Disponibilidades = VagaDisponibilidade.ListarIdentificadoresDisponibilidadePorVaga(objVaga),
                Cidade = objVaga.Cidade,
                CidadeDisponibilidade = CidadeDisponibilidade ? objVaga.Cidade : null,
                Escolaridade = objVaga.Escolaridade,
                Deficiencia = listPcd,
                Sexo = objVaga.Sexo,
                IdadeMinima = objVaga.NumeroIdadeMinima,
                IdadeMaxima = objVaga.NumeroIdadeMaxima,
                ValorSalarioMinimo = objVaga.ValorSalarioDe,
                ValorSalarioMaximo = objVaga.ValorSalarioPara
            };
        }

        private static Parametros RecuperarParametrosAutoCandidatura(BLL.Vaga objVaga)
        {
            return new Parametros
            {
                TipoBusca = Parametros.Tipo.Vaga,
                Funcoes = new List<int> { objVaga.Funcao.IdFuncao },
                Cidade = objVaga.Cidade,
                Escolaridade = objVaga.Escolaridade,
                Sexo = objVaga.Sexo,
                IdadeMinima = objVaga.NumeroIdadeMinima,
                IdadeMaxima = objVaga.NumeroIdadeMaxima,
                ValorSalarioMinimo = objVaga.ValorSalarioDe,
                ValorSalarioMaximo = objVaga.ValorSalarioPara
            };
        }
        #endregion

        #region Query de Busca de Currículo
        private static string MontarQueryBuscaCurriculo(int currentIndex, int pageSize, Parametros objParametros, bool buscaCurriculosMaisUmAno = false, int? IdVaga = null, List<int> ListaCandidatos = null)
        {
            var sbFilters = new StringBuilder();
            var sbQuery = new StringBuilder();
            var sbExtraParameters = new StringBuilder();

            sbQuery.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCV));
            sbQuery.Append("?q={!boost=recip(ms(NOW,Dta_Atualizacao),3.16e-11,1,1)}");

            sbExtraParameters.Append("&wt=json"); //retorno JSON            
            sbExtraParameters.AppendFormat("&start={0}", ((currentIndex * pageSize))); //Primeiro registro
            sbExtraParameters.AppendFormat("&rows={0}", pageSize); //Número de registros retornados
            sbExtraParameters.Append("&indent=true&facet=true");


            if (!String.IsNullOrEmpty(objParametros.CodigoCPFNome))
            {
                //Verificando a possibilidade de ser um CPF formatado XXX.XXX.XXX-XX    
                string busca;
                string possivelCPF = busca = objParametros.CodigoCPFNome;

                if (Regex.IsMatch(possivelCPF, @"\d{3}.\d{3}.\d{3}-\d{2}"))
                {
                    const string sPadrao = @"[.\\/-]";
                    var oRegEx = new Regex(sPadrao);
                    busca = oRegEx.Replace(possivelCPF, "");
                }

                if (busca.Contains(",") || busca.Contains(";"))
                    sbFilters.AppendFormat("&fq=Idf_Curriculo:({0})", busca.Replace(",", " "));
                else
                    sbFilters.AppendFormat("&fq=buscaBNE:\"{0}\"~4", busca);
            }

            if (!String.IsNullOrEmpty(objParametros.DDDTelefone) || !String.IsNullOrEmpty(objParametros.Telefone))
            {
                if (!String.IsNullOrEmpty(objParametros.DDDTelefone))
                    sbFilters.Append($"&fq={{!tag=Num_DDD_Celular}}(Num_DDD_Celular:{objParametros.DDDTelefone} OR Num_DDD_Telefone:{objParametros.DDDTelefone} OR Num_DDD_Celular_Contato:{objParametros.DDDTelefone} OR Num_DDD_Telefone_Contato:{objParametros.DDDTelefone})&facet.query={{!ex=Num_DDD_Telefone key=\"Num_DDD_Telefone\"}}!Num_DDD_Telefone:{objParametros.DDDTelefone} OR Num_DDD_Telefone:{objParametros.DDDTelefone} OR Num_DDD_Celular_Contato:{objParametros.DDDTelefone} OR Num_DDD_Telefone_Contato:{objParametros.DDDTelefone}");

                if (!String.IsNullOrEmpty(objParametros.Telefone))
                    sbFilters.Append($"&fq={{!tag=Num_Celular}}(Num_Celular:{objParametros.Telefone} OR Num_Telefone:{objParametros.Telefone} OR Num_Celular_Contato:{objParametros.Telefone} OR Num_Telefone_Contato:{objParametros.Telefone})&facet.query={{!ex=Num_Telefone key=\"Num_Telefone\"}}!Num_Celular:{objParametros.Telefone} OR Num_Telefone:{objParametros.Telefone} OR Num_Celular_Contato:{objParametros.Telefone} OR Num_Telefone_Contato:{objParametros.Telefone}");
            }

            if (!string.IsNullOrEmpty(objParametros.Email))
                sbFilters.Append($"&fq={{!tag=Eml_Pessoa}}Eml_Pessoa:{objParametros.Email}&facet.query={{!ex=Eml_Pessoa key=\"Eml_Pessoa\"}}!Eml_Pessoa:{objParametros.Email}");

            if (objParametros.Funcoes.Count > 0)
            {
                sbFilters.Append("&fq=Idfs_Funcoes:(");
                for (int i = 0; i < objParametros.Funcoes.Count; i++)
                {
                    if (i == 0)
                        sbFilters.Append(objParametros.Funcoes[i].ToString());
                    else
                        sbFilters.AppendFormat("+{0}", objParametros.Funcoes[i]);
                }
                sbFilters.Append(")");
            }

            if (objParametros.Cidade != null)
            {
                if (objParametros.CidadeDisponibilidade != null)
                {
                    sbFilters.AppendFormat("&fq=(Idf_Cidade_Disponibilidade:{0}", objParametros.CidadeDisponibilidade.IdCidade + ")");
                    sbFilters.Append($"&fq=!Idfs_Cidades:{objParametros.CidadeDisponibilidade.IdCidade}&fq=!Idf_Cidade:{objParametros.CidadeDisponibilidade.IdCidade}");
                }
                else if (objParametros.GeoBuscaCidade != null && objParametros.GeoBuscaBairro == null)
                {
                    sbFilters.AppendFormat("&fq=(Idfs_Cidades:{0}", objParametros.Cidade.IdCidade + ")");
                    sbFilters.Append(@" OR _query_:""{!geofilt}""&sfield=Geo_Localizacao");
                    sbFilters.AppendFormat("&pt={0},{1}", objParametros.GeoBuscaCidade.Lat.ToString().Replace(',', '.'), objParametros.GeoBuscaCidade.Long.ToString().Replace(',', '.'));
                    sbFilters.AppendFormat("&d={0}", objParametros.Cidade.IdCidade == 5345 /* São Paulo */ ? Parametro.RecuperaValorParametro(Enumeradores.Parametro.DistanciaPadraoBuscaCVCidadeSaoPaulo) : Parametro.RecuperaValorParametro(Enumeradores.Parametro.DistanciaPadraoBuscaCVCidade));
                }
                else
                    sbFilters.AppendFormat("&fq=Idfs_Cidades:{0}", objParametros.Cidade.IdCidade);
            }

            if (!String.IsNullOrEmpty(objParametros.PalavraChave))
            {
                if (objParametros.CamposPalavraChave != null && objParametros.CamposPalavraChave.Count > 0)
                {
                    for (int i = 0; i < objParametros.CamposPalavraChave.Count; i++)
                    {
                        if (i == 0)
                            sbQuery.Append("(" + objParametros.CamposPalavraChave[i] + ":" + '"' + HttpUtility.UrlEncode(objParametros.PalavraChave.Replace(",", "").Replace(";", "")) + "\"&facet.query={!ex=" + objParametros.CamposPalavraChave[i] + " key=\"" + i + "\"}(" + objParametros.CamposPalavraChave[i] + ":\"" + HttpUtility.UrlEncode(objParametros.PalavraChave.Replace(",", "").Replace(";", "")) + "\"");
                        else
                            sbQuery.Append(" OR " + objParametros.CamposPalavraChave[i] + ":" + '"' + HttpUtility.UrlEncode(objParametros.PalavraChave.Replace(",", "").Replace(";", "")) + "\"&facet.query={!ex=" + objParametros.CamposPalavraChave[i] + " key=\"" + i + "\"}(" + objParametros.CamposPalavraChave[i] + ":\"" + HttpUtility.UrlEncode(objParametros.PalavraChave.Replace(",", "").Replace(";", "")) + "\"");
                    }

                    sbQuery.Append(")");
                }
                else
                {
                    sbQuery.Append(HttpUtility.UrlEncode(objParametros.PalavraChave.Replace(",", "").Replace(";", "")));
                }
            }

            if (!String.IsNullOrEmpty(objParametros.FiltroExcludente))
            {
                if (objParametros.FiltroExcludente.Contains(",") || objParametros.FiltroExcludente.Contains(";"))
                    sbFilters.Append($"&fq={{!tag=Raz_Social}}!Raz_Social:({objParametros.FiltroExcludente.Replace(",", " ")})&facet.query={{!ex=Raz_Social key=\"Raz_Social\"}}Raz_Social:{objParametros.FiltroExcludente.Replace(",", " ")}");
                else
                    sbFilters.Append($"&fq={{!tag=Raz_Social}}!Raz_Social:\"{objParametros.FiltroExcludente}\"~4&facet.query={{!ex=Raz_Social key=\"FiltroExcludente\"}}Raz_Social:\"{objParametros.FiltroExcludente}\"~4");
            }

            if (objParametros.Estado != null && objParametros.CidadeDisponibilidade == null)
                sbFilters.AppendFormat("&fq=Sig_Estado:{0}", objParametros.Estado.SiglaEstado);

            if (objParametros.Escolaridade != null)
            {
                objParametros.Escolaridade.CompleteObject();
                if (objParametros.Escolaridade.SequenciaPeso.HasValue)
                    sbFilters.Append($"&fq={{!tag=Idf_Escolaridade}}Idf_Escolaridade:[{objParametros.Escolaridade.IdEscolaridade} TO *]&facet.query={{!ex=Idf_Escolaridade key=\"Idf_Escolaridade\"}}!Idf_Escolaridade:[{objParametros.Escolaridade.IdEscolaridade} TO *]");
            }

            if (objParametros.Sexo != null)
                sbFilters.Append($"&fq={{!tag=Idf_Sexo}}Idf_Sexo:{objParametros.Sexo.IdSexo}&facet.query={{!ex=Idf_Sexo key=\"!Idf_Sexo:{objParametros.Sexo.IdSexo}\"}}!Idf_Sexo:{objParametros.Sexo.IdSexo}");

            if (objParametros.IdadeMinima.HasValue && objParametros.IdadeMaxima.HasValue)
                sbFilters.Append($"&fq={{!tag=Dta_Nascimento}}Dta_Nascimento:[{DateTime.Today.AddYears(-objParametros.IdadeMaxima.Value - 1).ToString("yyyy-MM-ddT00:00:00.000Z")} TO {DateTime.Today.AddYears(-objParametros.IdadeMinima.Value).ToString("yyyy-MM-ddT00:00:00.000Z")}]&facet.query={{!ex=Dta_Nascimento key=\"Idade_Min\"}}!Dta_Nascimento:[* TO {DateTime.Today.AddYears(-objParametros.IdadeMinima.Value).ToString("yyyy-MM-ddT00:00:00.000Z")}]&facet.query={{!ex=Dta_Nascimento key=\"Idade_Max\"}}!Dta_Nascimento:[{DateTime.Today.AddYears(-objParametros.IdadeMaxima.Value).ToString("yyyy-MM-ddT00:00:00.000Z")} TO *]");
            else
            {
                if (objParametros.IdadeMinima.HasValue)
                    sbFilters.Append($"&fq={{!tag=Dta_Nascimento}}Dta_Nascimento:[* TO {DateTime.Today.AddYears(-objParametros.IdadeMinima.Value + 1).ToString("yyyy-MM-ddT00:00:00.000Z")}]&facet.query={{!ex=Dta_Nascimento key=\"Idade_Min\"}}!Dta_Nascimento:[* TO {DateTime.Today.AddYears(-objParametros.IdadeMinima.Value + 1).ToString("yyyy-MM-ddT00:00:00.000Z")}]");

                if (objParametros.IdadeMaxima.HasValue)
                    sbFilters.Append($"&fq={{!tag=Dta_Nascimento}}Dta_Nascimento:[{DateTime.Today.AddYears(-objParametros.IdadeMaxima.Value - 1).ToString("yyyy-MM-ddT00:00:00.000Z")} TO *]&facet.query={{!ex=Dta_Nascimento key=\"Idade_Max\"}}!Dta_Nascimento:[{DateTime.Today.AddYears(-objParametros.IdadeMaxima.Value - 1).ToString("yyyy-MM-ddT00:00:00.000Z")} TO *]");
            }
            //sbFilters.AppendFormat("&fq=Vlr_Pretensao_Salarial:[{0}+TO+{1}]", objParametros.ValorSalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture), objParametros.ValorSalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture));

            if (objParametros.ValorSalarioMinimo.HasValue && objParametros.ValorSalarioMaximo.HasValue)
            {
                sbFilters.Append($"&fq={{!tag=Salario}}Vlr_Pretensao_Salarial:[{objParametros.ValorSalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture)}+TO+{objParametros.ValorSalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture)}]&facet.query={{!ex=Salario key=\"Vlr_Pretensao_Salarial_Min\"}}Vlr_Pretensao_Salarial:[0 TO {(objParametros.ValorSalarioMinimo.Value - 1).ToString("#.##", CultureInfo.InvariantCulture)}]&facet.query={{!ex=Salario key=\"Vlr_Pretensao_Salarial_Max\"}}Vlr_Pretensao_Salarial:[{(objParametros.ValorSalarioMaximo.Value + 1).ToString("#.##", CultureInfo.InvariantCulture)} TO *]");
            }
            else
            {
                if (objParametros.ValorSalarioMinimo.HasValue)
                    sbFilters.Append($"&fq={{!tag=Salario}}Vlr_Pretensao_Salarial:[{objParametros.ValorSalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture)}+TO+*]&facet.query={{!ex=Salario key=\"Vlr_Pretensao_Salarial_Min\"}}Vlr_Pretensao_Salarial:[0 TO {(objParametros.ValorSalarioMinimo.Value - 1).ToString("#.##", CultureInfo.InvariantCulture)}]");

                if (objParametros.ValorSalarioMaximo.HasValue)
                    sbFilters.Append($"&fq={{!tag=Salario}}Vlr_Pretensao_Salarial:[*+TO+{objParametros.ValorSalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture)}]&facet.query={{!ex=Salario key=\"Vlr_Pretensao_Salarial_Max\"}}Vlr_Pretensao_Salarial:[{(objParametros.ValorSalarioMaximo.Value + 1).ToString("#.##", CultureInfo.InvariantCulture)} TO *]");
            }

            //Campo Solr -> Total_Experiencia
            if (objParametros.QuantidadeExperiencia.HasValue)
                sbFilters.AppendFormat("&fq=Total_Experiencia:[{0}+TO+*]&", objParametros.QuantidadeExperiencia.Value);

            if (objParametros.Disponibilidades != null && objParametros.Disponibilidades.Count > 0)
            {
                sbFilters.Append($"&fq={{!tag=Idf_Disponibilidade}}Idf_Disponibilidade:({string.Join(" AND ", objParametros.Disponibilidades)})");
                if (objParametros.Disponibilidades.Count.Equals(1))
                {
                    sbFilters.Append("&facet.query={!ex=Idf_Disponibilidade key=\"Idf_Disponibilidade" + objParametros.Disponibilidades[0].ToString() + "\"}!Idf_Disponibilidade:(" + string.Join(" AND ", objParametros.Disponibilidades) + ")");
                }
                else
                {
                    foreach (var item in objParametros.Disponibilidades)
                    {
                        var itens = objParametros.Disponibilidades.ToList();
                        itens.Remove(item);
                        sbFilters.Append("&facet.query={!ex=Idf_Disponibilidade key=\"Idf_Disponibilidade" + item + "\"}Idf_Disponibilidade:(" + string.Join(" AND ", itens) + ")");
                    }
                }
            }

            if (objParametros.EstadoCivil != null)
                sbFilters.Append($"&fq={{!tag=Idf_Estado_Civil}}Idf_Estado_Civil:{objParametros.EstadoCivil.IdEstadoCivil}&facet.query={{!ex=Idf_Estado_Civil key=\"Idf_Estado_Civil\"}}!Idf_Estado_Civil:{objParametros.EstadoCivil.IdEstadoCivil}");

            if (!String.IsNullOrEmpty(objParametros.Bairro))
            {
                if (objParametros.GeoBuscaBairro != null)
                {
                    sbFilters.Append("&fq=%7b!geofilt%7d&sfield=Geo_Localizacao");
                    sbFilters.AppendFormat("&pt={0},{1}", objParametros.GeoBuscaBairro.Lat.ToString().Replace(',', '.'), objParametros.GeoBuscaBairro.Long.ToString().Replace(',', '.'));
                    sbFilters.AppendFormat("&d={0}", objParametros.Cidade != null && objParametros.Cidade.IdCidade == 5345 /* São Paulo */ ? Parametro.RecuperaValorParametro(Enumeradores.Parametro.DistanciaPadraoBuscaCVCidadeSaoPaulo) : Parametro.RecuperaValorParametro(Enumeradores.Parametro.DistanciaPadraoBuscaCVCidade));
                }
                else
                    sbFilters.Append($"&fq={{!tag=Des_Bairro}}Des_Bairro:(\"{objParametros.Bairro.Replace(",", "\" \"")}\")&facet.query={{!ex=Des_Bairro key=\"Des_Bairro\"}}!Des_Bairro:(\"{objParametros.Bairro.Replace(",", "\" \"")}\")");
            }

            if (!String.IsNullOrEmpty(objParametros.Zona))
                sbFilters.AppendFormat("&fq=Des_Zona:(\"{0}\")", objParametros.Zona.Replace(",", "\" \""));

            if (!String.IsNullOrEmpty(objParametros.Logradouro))
                sbFilters.Append($"&fq={{!tag=Des_Logradouro}}Des_Logradouro:\"{objParametros.Logradouro}\"~2&facet.query={{!ex=Des_Logradouro key=\"Des_Logradouro\"}}!Des_Logradouro:\"{objParametros.Logradouro}\"");

            if (!String.IsNullOrEmpty(objParametros.NumeroCEPInicial) && !String.IsNullOrEmpty(objParametros.NumeroCEPFinal))
                sbFilters.Append($"&fq={{!tag=Num_CEP}}Num_CEP:[{objParametros.NumeroCEPInicial}+TO+{objParametros.NumeroCEPFinal}]&facet.query={{!ex=Num_CEP key=\"Num_CEP_Inicio\"}}Num_CEP:[*+TO+{objParametros.NumeroCEPInicial}]&facet.query={{!ex=Num_CEP key=\"Num_CEP_Fim\"}}Num_CEP:[{objParametros.NumeroCEPFinal} TO *]");
            else
            {
                if (!String.IsNullOrEmpty(objParametros.NumeroCEPInicial))
                    sbFilters.Append($"&fq={{!tag=Num_CEP}}Num_CEP:[{objParametros.NumeroCEPInicial}+TO+*]&facet.query={{!ex=Num_CEP key=\"Num_CEP_Inicio\"}}Num_CEP[*+TO+{objParametros.NumeroCEPInicial}]");

                if (!String.IsNullOrEmpty(objParametros.NumeroCEPFinal))
                    sbFilters.Append($"&fq={{!tag=Num_CEP}}Num_CEP:[*+TO+{objParametros.NumeroCEPFinal}]&facet.query={{!ex=Num_CEP key=\"Num_CEP_Fim\"}}Num_Cep[{objParametros.NumeroCEPFinal}+TO*]");
            }

            //Campo Solr -> Des_Curso
            if (objParametros.CursoTecnicoGraduacao != null || !String.IsNullOrEmpty(objParametros.DescricaoCursoTecnicoGraduacao))
            {
                object valueCurso;
                if (objParametros.CursoTecnicoGraduacao != null)
                {
                    objParametros.CursoTecnicoGraduacao.CompleteObject();
                    valueCurso = objParametros.CursoTecnicoGraduacao.DescricaoCurso;
                }
                else
                    valueCurso = objParametros.DescricaoCursoTecnicoGraduacao;

                sbFilters.Append($"&fq={{!tag=Des_Curso}}Des_Curso:\"{valueCurso}\"&facet.query={{!ex=Des_Curso key=\"Des_Curso\"}}!Des_Curso:\"{valueCurso}\"");
            }

            //Campo Solr -> Des_Fonte
            if (objParametros.FonteTecnicoGraduacao != null)
            {
                objParametros.FonteTecnicoGraduacao.CompleteObject();

                sbFilters.Append($"&fq={{!tag=Idf_Fonte}}Idf_Fonte:{objParametros.FonteTecnicoGraduacao.IdFonte}&facet.query={{!ex=Idf_Fonte key=\"Idf_Fonte\"}}!Idf_Fonte:{objParametros.FonteTecnicoGraduacao.IdFonte}");
            }

            //Campo Solr -> Des_Curso
            if (objParametros.CursoOutros != null || !String.IsNullOrEmpty(objParametros.DescricaoCursoOutros))
            {
                object valueOutrosCursos;

                if (objParametros.CursoOutros != null)
                {
                    objParametros.CursoOutros.CompleteObject();
                    valueOutrosCursos = objParametros.CursoOutros.DescricaoCurso;
                }
                else
                    valueOutrosCursos = objParametros.DescricaoCursoOutros;

                sbFilters.Append($"&fq={{!tag=Des_Curso}}Des_Curso:\"{valueOutrosCursos}\"&facet.query={{!ex=Des_Curso key=\"Des_Curso_Outro\"}}!Des_Curso:\"{valueOutrosCursos}\" ");
            }

            if (objParametros.FonteOutros != null)
            {
                objParametros.FonteOutros.CompleteObject();
                sbFilters.Append($"&fq={{!tag=Des_Fonte}}Des_Fonte:{objParametros.FonteOutros.NomeFonte}&facet.query={{!ex=Des_Fonte key=\"Des_Fonte\"}}!Des_Fonte:{objParametros.FonteOutros.NomeFonte}");
            }

            #region Formacoes

            if (objParametros.Formacao != null)
            {
                var _formacoes_query = string.Empty;

                if (objParametros.Formacao.AnoConclusao.HasValue)
                {
                    _formacoes_query += !string.IsNullOrEmpty(_formacoes_query) ? "%20AND%20" : string.Empty;
                    _formacoes_query += "Ano_Conclusao:" + objParametros.Formacao.AnoConclusao.Value.ToString();
                }

                if (objParametros.Formacao.Cidade != null)
                {
                    _formacoes_query += !string.IsNullOrEmpty(_formacoes_query) ? "%20AND%20" : string.Empty;
                    _formacoes_query += "Idf_Cidade:" + objParametros.Formacao.Cidade.IdCidade.ToString();
                }

                if (objParametros.Formacao.Curso != null)
                {
                    _formacoes_query += !string.IsNullOrEmpty(_formacoes_query) ? "%20AND%20" : string.Empty;
                    _formacoes_query += objParametros.Formacao.Curso.IdCurso > 0 ?
                        "Idf_Curso:" + objParametros.Formacao.Curso.IdCurso :
                        "Des_Curso:%22" + objParametros.Formacao.Curso.DescricaoCurso + "%22";
                }

                if (objParametros.Formacao.Escolaridade != null)
                {
                    _formacoes_query += !string.IsNullOrEmpty(_formacoes_query) ? "%20AND%20" : string.Empty;
                    _formacoes_query += "Idf_Escolaridade:" + objParametros.Formacao.Escolaridade.IdEscolaridade.ToString();
                }

                if (objParametros.Formacao.Fonte != null)
                {
                    _formacoes_query += !string.IsNullOrEmpty(_formacoes_query) ? "%20AND%20" : string.Empty;
                    _formacoes_query += objParametros.Formacao.Fonte.IdFonte > 0 ?
                        "Idf_Fonte:" + objParametros.Formacao.Fonte.IdFonte :
                        "Des_Fonte:%22" + objParametros.Formacao.Fonte.NomeFonte + "%22";
                }

                if (objParametros.Formacao.Periodo.HasValue)
                {
                    _formacoes_query += !string.IsNullOrEmpty(_formacoes_query) ? "%20AND%20" : string.Empty;
                    _formacoes_query += "Num_Periodo:" + objParametros.Formacao.Periodo.Value.ToString();
                }

                if (objParametros.Formacao.SituacaoCurso != null)
                {
                    _formacoes_query += !string.IsNullOrEmpty(_formacoes_query) ? "%20AND%20" : string.Empty;
                    _formacoes_query += "Idf_Situacao_Formacao:" + objParametros.Formacao.SituacaoCurso.IdSituacaoFormacao.ToString();
                }

                if (!string.IsNullOrEmpty(_formacoes_query))
                    sbFilters.Append("&fq={!join%20from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=cv_formacao}" + _formacoes_query);
            }

            #endregion Formacoes

            if (!String.IsNullOrEmpty(objParametros.Experiencia))
                sbFilters.Append($"&fq={{!tag=Des_Atividade}}Des_Atividade:{objParametros.Experiencia}&facet.query={{!ex=Des_Atividade key=\"Des_Atividade\"}}!Des_Atividade:{objParametros.Experiencia}");

            if (objParametros.FuncaoExercida != null)
            {
                //Verificando se a descrição da funcao esta presente no objeto. Se nao, completa para pesquisa por descricao.
                if (String.IsNullOrEmpty(objParametros.FuncaoExercida.DescricaoFuncao))
                    objParametros.FuncaoExercida.CompleteObject();

                sbFilters.Append($"&fq={{!tag=Des_Funcao_Exercida}}Des_Funcao_Exercida:{objParametros.FuncaoExercida.DescricaoFuncao}&facet.query={{!ex=Des_Funcao_Exercida key=\"Des_Funcao_Exercida\"}}!Des_Funcao_Exercida:{objParametros.FuncaoExercida.DescricaoFuncao}");
            }

            if (!String.IsNullOrEmpty(objParametros.RazaoSocial))
            {
                sbFilters.Append($"&fq={{!tag=Raz_Social}}Raz_Social:\"{objParametros.RazaoSocial}\"~2&facet.query={{!ex=Raz_Social key=\"Raz_Social\"}}!Raz_Social:\"{objParametros.RazaoSocial}\"");
            }

            if (objParametros.AreaBNE != null)
                sbFilters.Append($"&fq={{!tag=Idf_Area_BNE}}Idf_Area_BNE:{objParametros.AreaBNE.IdAreaBNE}&facet.query={{!ex=Idf_Area_BNE key=\"Idf_Area_BNE\"}}!Idf_Area_BNE:{objParametros.AreaBNE.IdAreaBNE}");

            if (objParametros.CategoriaHabilitacao != null)
            {
                switch (objParametros.CategoriaHabilitacao.IdCategoriaHabilitacao)
                {
                    case 1:
                        sbFilters.Append("&fq={!tag=Idf_Categoria_Habilitacao}Idf_Categoria_Habilitacao:(1+6+7+8+9)&facet.query={!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}!Idf_Categoria_Habilitacao:(1+6+7+8+9)");
                        break;
                    case 2:
                        sbFilters.Append("&fq={!tag=Idf_Categoria_Habilitacao}Idf_Categoria_Habilitacao:[2 TO 9]&facet.query={!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}!Idf_Categoria_Habilitacao:[2 TO 9]");
                        break;
                    case 3:
                        sbFilters.Append("&fq={!tag=Idf_Categoria_Habilitacao}Idf_Categoria_Habilitacao:(3+4+5+7+8+9)&facet.query={!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}!Idf_Categoria_Habilitacao:(3+4+5+7+8+9)");
                        break;
                    case 4:
                        sbFilters.Append("&fq={!tag=Idf_Categoria_Habilitacao}Idf_Categoria_Habilitacao:(4+5+8+9)&facet.query={!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}!Idf_Categoria_Habilitacao:(4+5+8+9)");
                        break;
                    case 5:
                        sbFilters.Append("&fq={!tag=Idf_Categoria_Habilitacao}Idf_Categoria_Habilitacao:(5+9)&facet.query={!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}!Idf_Categoria_Habilitacao:(5+9)");
                        break;
                    case 6:
                        sbFilters.Append("&fq={!tag=Idf_Categoria_Habilitacao}Idf_Categoria_Habilitacao:[6 TO 9]&facet.query={!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}!Idf_Categoria_Habilitacao:[6 TO 9]");
                        break;
                    case 7:
                        sbFilters.Append("&fq={!tag=Idf_Categoria_Habilitacao}Idf_Categoria_Habilitacao:[7 TO 9]&facet.query={!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}!Idf_Categoria_Habilitacao:[7 TO 9]");
                        break;
                    case 8:
                        sbFilters.Append("&fq={!tag=Idf_Categoria_Habilitacao}Idf_Categoria_Habilitacao:[8 TO 9]&facet.query={!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}!Idf_Categoria_Habilitacao:[8 TO 9]");
                        break;
                    default:
                        sbFilters.Append($"&fq={{!tag=Idf_Categoria_Habilitacao}}Idf_Categoria_Habilitacao:{objParametros.CategoriaHabilitacao.IdCategoriaHabilitacao}&facet.query={{!ex=Idf_Categoria_Habilitacao key=\"Idf_Categoria_Habilitacao\"}}!Idf_Categoria_Habilitacao:{objParametros.CategoriaHabilitacao.IdCategoriaHabilitacao}");
                        break;
                }

            }

            if (objParametros.TipoVeiculo != null)
                sbFilters.Append($"&fq={{!tag=Idf_Tipo_Veiculo}}Idf_Tipo_Veiculo:{objParametros.TipoVeiculo.IdTipoVeiculo}&facet.query={{!ex=Idf_Tipo_Veiculo key=\"Idf_tipo_Veiculo\"}}!Idf_Tipo_Veiculo:{objParametros.TipoVeiculo.IdTipoVeiculo}");

            if (objParametros.Deficiencia != null && objParametros.Deficiencia.Count() > 0)
            {
                if (objParametros.Deficiencia.Any(x => x.Equals((int)Enumeradores.Deficiencia.Nenhuma)))
                    sbFilters.Append("&fq=-(-Idf_Deficiencia:[* TO *] AND Idf_Deficiencia:0)");
                else if (objParametros.Deficiencia.Any(x => x.Equals((int)Enumeradores.Deficiencia.Qualquer)))
                    sbFilters.Append("&fq=Idf_Deficiencia:(1+2+3+4+5+6+7)");
                else
                    sbFilters.AppendFormat("&fq=Idf_Deficiencia:({0})", string.Join(" + ", objParametros.Deficiencia));
            }

            if (objParametros.Raca != null)
                sbFilters.Append($"&fq={{!tag=Idf_Raca}}Idf_Raca:{objParametros.Raca.IdRaca}&facet.query={{!ex=Idf_Raca key=\"Idf_Raca\"}}!Idf_Raca:{objParametros.Raca.IdRaca}");

            if (objParametros.Filhos != null)
                sbFilters.Append($"&fq={{!tag=Flg_Filhos}}Flg_Filhos:{objParametros.Filhos}&facet.query={{!ex=Flg_Filhos key=\"Flg_Filhos\"}}!Flg_Filhos:{objParametros.Filhos}");

            if (!string.IsNullOrWhiteSpace(objParametros.EscolaridadesWebEstagios))
                sbFilters.AppendFormat("&fq=Idf_Escolaridade:({0})", objParametros.EscolaridadesWebEstagios.Replace(",", " "));

            if (objParametros.Aprendiz != null)
                sbFilters.Append("&fq=Num_Idade:[14+TO+24]&fq=Idf_Escolaridade:(5+6+8)");

            if (objParametros.Origem != null)
                sbFilters.AppendFormat("&fq=Idfs_Origens:{0}", objParametros.Origem.IdOrigem);

            if (objParametros.TipoBusca == Parametros.Tipo.Rastreador && !buscaCurriculosMaisUmAno)
            {
                //TODO: Convertendo para utc por causa da indexação do solr.
                sbFilters.AppendFormat("&fq=Dta_Atualizacao:[{0} TO *]", TimeZoneInfo.ConvertTimeToUtc(objParametros.DataInicial).ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }

            if (buscaCurriculosMaisUmAno)
            {
                sbFilters.AppendFormat("&fq=Dta_Atualizacao:[* TO {0}]", TimeZoneInfo.ConvertTimeToUtc(DateTime.Today.AddYears(-1)).ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }

            if (ListaCandidatos != null)
            {
                sbFilters.Append($"&fq=Idf_Curriculo:({string.Join(" OR ", ListaCandidatos)})");
            }

            //Empresa logada, não trazer os curriculos que a bloqueou.
            if (objParametros.Filial != null)
                sbFilters.Append("&fq=-({!join%20from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=bloqueio_empresa}Idf_Filial:" + objParametros.Filial.IdFilial + ")");

            #region [Idiomas]
            if (objParametros.Idiomas != null && objParametros.Idiomas.Count > 0)
            {
                List<int> idiomaSemNivel = new List<int>();
                foreach (var idioma in objParametros.Idiomas)
                {
                    if (idioma.Value > 0)
                    {
                        //sbFilters.Append("&fq={!join from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=cv_idioma}{!tag=Idf_Idioma}Idf_Idioma:" + idioma.Key + "%20AND%20Idf_Nivel_Idioma:[" + idioma.Value + "%20TO%20*]&facet.query={!ex=Idf_Idioma key=\"Idf_Idioma"+idioma.Key+ "\" join from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=cv_idioma}!Idf_Idioma:" + idioma.Key + "%20AND%20Idf_Nivel_Idioma:[" + idioma.Value + "%20TO%20*] ");
                        sbFilters.Append("&fq={!join from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=cv_idioma}{!tag=Idf_Idioma}Idf_Idioma:" + idioma.Key + "%20AND%20Idf_Nivel_Idioma:[" + idioma.Value + "%20TO%20*]&facet.query={!ex=Idf_Idioma key=\"Idf_Idioma" + idioma.Key + "\"}Idf_Idioma:[*%20TO%20*]");

                    }
                    else
                        idiomaSemNivel.Add(idioma.Key);

                }
                if (idiomaSemNivel.Count > 0)
                    sbFilters.Append($"&fq={{!tag=Idf_Idioma}}Idf_Idioma:({string.Join(" ", idiomaSemNivel)})");
                foreach (var item in idiomaSemNivel)
                {
                    sbFilters.Append($"&facet.query={{!ex=Idf_Idioma key=\"Idf_Idioma{item}\"}}!Idf_Idioma:({item})");
                }
            }
            #endregion

            //Se possui curriculo avaliado e foi pesquisado por avaliação
            if ((objParametros.Avaliacoes != null && objParametros.Avaliacoes.Any()) || !string.IsNullOrEmpty(objParametros.Avaliacao))
            {


                var _carinhas_query = "&fq={!join%20from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=cv_classificacao}Idf_Filial:" + objParametros.Filial.IdFilial;

                _carinhas_query += (objParametros.UsuarioFilialPerfil != null) ? "%20AND%20Idf_Usuario_Filial_Perfil:" + objParametros.UsuarioFilialPerfil.IdUsuarioFilialPerfil : "%20AND%20Idf_Usuario_Filial_Perfil:[%22%22%20TO%20*]";


                if (objParametros.Avaliacoes != null && objParametros.Avaliacoes.Any())
                {
                    _carinhas_query += "%20AND%20Idf_Avaliacao:(" + string.Join(" ", objParametros.Avaliacoes) + ")";
                }

                if (!string.IsNullOrEmpty(objParametros.Avaliacao))
                {
                    _carinhas_query += String.Format("%20AND%20Des_Observacao%3A%22{0}%22", objParametros.Avaliacao);
                }

                sbFilters.Append(_carinhas_query);
            }


            //Sort
            sbFilters.Append("&sort=");

            if (objParametros.Origem != null)
            {
                if (objParametros.Origem.IdOrigem != (int)Enumeradores.Origem.BNE)
                    sbFilters.AppendFormat("termfreq(Idfs_Origens,{0})+desc%2C", objParametros.Origem.IdOrigem);
            }
            //Cidade Preenchida
            if (objParametros.Cidade != null)
            {
                if (objParametros.GeoBuscaCidade != null && objParametros.GeoBuscaBairro != null)
                {
                    string[] partsBairro = objParametros.Bairro.Trim().Replace(",", "").Split(' ');
                    foreach (string part in partsBairro)
                        sbFilters.AppendFormat("termfreq(Des_Bairro,{0})+desc%2C", part.Trim());

                    sbFilters.AppendFormat("termfreq(Idf_Cidade,{0})+desc%2C", objParametros.Cidade.IdCidade);
                }
                else
                {
                    if (objParametros.GeoBuscaCidade != null && objParametros.GeoBuscaBairro == null)
                        sbFilters.AppendFormat("termfreq(Idf_Cidade,{0})+desc%2C", objParametros.Cidade.IdCidade);
                    else
                        sbFilters.AppendFormat("termfreq(Idf_Cidade,{0})+desc%2C", objParametros.Cidade.IdCidade);
                }
            }
            //Funcao
            if (objParametros.Funcoes.Count > 0)
            {//Task 49380 - Embaralhar as funções do resultado de pesquisa de currículo.
                switch (objParametros.Funcoes.Count())
                {
                    case 1:
                        sbFilters.AppendFormat("termfreq(Idf_Funcao,{0})+desc%2C", objParametros.Funcoes.First());
                        break;
                    default:
                        if (objParametros.Funcoes.Count > 0)
                        {
                            foreach (int funcao in objParametros.Funcoes)
                            {
                                sbFilters.Append($"if(termfreq(Idf_Funcao,{funcao}),1,");
                            }
                            sbFilters.Append($"0){(objParametros.Funcoes.Count() > 2 ? "))" : ")")} +desc%2C");
                        }
                        break;
                }
                //foreach (int funcao in objParametros.Funcoes)
                //{
                //    sbFilters.AppendFormat("if(termfreq(Idf_Funcao,{0}),1,+desc%2C", funcao);
                //}
            }

            //Bairro preenchido
            if (objParametros != null && objParametros.Cidade == null && (objParametros.GeoBuscaBairro != null || (!String.IsNullOrEmpty(objParametros.Bairro))))
            {
                if (objParametros.GeoBuscaBairro != null)
                    sbFilters.Append("geodist()+asc,");
                else
                {
                    if (!String.IsNullOrEmpty(objParametros.Bairro))
                        sbFilters.AppendFormat("termfreq(Des_Bairro,'{0}')+desc%2C", objParametros.Bairro.Trim().Substring(0, objParametros.Bairro.Length - 1));
                }

            }

            //Se pesquisado estag
            if (!string.IsNullOrWhiteSpace(objParametros.EscolaridadesWebEstagios))
            {
                sbFilters.Append("Flg_CurriculoAceitaEstagio desc,");
            }

            //TODO: Comentado até correção
            //sbFilters.Append("Flg_VIP+desc%2CQtd_Pesquisas+asc%2CDta_Atualizacao+desc");
            sbFilters.Append("Flg_VIP+desc%2Cmap(Qtd_Pesquisas,0,0,9999)+asc%2CDta_Atualizacao+desc");

            return string.Concat(sbQuery, sbExtraParameters, sbFilters);
        }
        #endregion

        #region Query de Busca de Classificacao
        public static string MontarQueryBuscaClassificacao(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, List<int> curriculos)
        {
            var sb = new StringBuilder();

            sb.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCVClassificacao));
            sb.Append("?q=*:*&wt=json&indent=true&rows=999999");
            sb.AppendFormat("&fq=Idf_Filial:{0}", objFilial.IdFilial);

            //Se passado usuario associacao add só comentarios do usuario
            if (objUsuarioFilialPerfil != null)
            {
                sb.AppendFormat("&fq=Idf_Usuario_Filial_Perfil:{0}", objUsuarioFilialPerfil.IdUsuarioFilialPerfil);
            }

            sb.AppendFormat("&fq=Idf_Curriculo:({0})", String.Join("+", curriculos));

            return sb.ToString();
        }
        #endregion

        #region Query de Busca de SMS
        public static string MontarQueryBuscaSMS(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, List<int> curriculos, int pageSize)
        {
            var sb = new StringBuilder();

            sb.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCVSms));
            sb.AppendFormat("?q=*:*&wt=json&indent=true&sort=Dta_Cadastro+desc&rows={0}", pageSize * 100);
            sb.AppendFormat("&fq=Idf_Filial:{0}", objFilial.IdFilial);

            //Se passado usuario associacao add só comentarios do usuario
            if (objUsuarioFilialPerfil != null)
            {
                sb.AppendFormat("&fq=Idf_Usuario_Filial_Perfil:{0}", objUsuarioFilialPerfil.IdUsuarioFilialPerfil);
            }

            sb.AppendFormat("&fq=Idf_Curriculo:({0})", String.Join("+", curriculos));

            return sb.ToString();
        }
        #endregion

        #region Query de Busca de Currículo
        /// <summary>
        /// Especifico para buscar os cv dos candidatos a vaga
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ListaCandidatos"></param>
        /// <returns></returns>
        private static string MontarQueryBuscaCurriculoCandidato(int currentIndex, int pageSize, List<int> ListaCandidatos)
        {
            var sbFilters = new StringBuilder();
            var sbQuery = new StringBuilder();
            var sbExtraParameters = new StringBuilder();

            sbQuery.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCV));
            sbQuery.Append("?q={!boost=recip(ms(NOW,Dta_Atualizacao),3.16e-11,1,1)}");

            sbExtraParameters.Append("&wt=json"); //retorno JSON            
            sbExtraParameters.AppendFormat("&start={0}", ((currentIndex * pageSize))); //Primeiro registro
            sbExtraParameters.AppendFormat("&rows={0}", pageSize); //Número de registros retornados
            sbExtraParameters.Append("&indent=true&facet=true");

            if (ListaCandidatos.Count > 0)
                sbFilters.Append($"&fq=Idf_Curriculo:({string.Join(" OR ", ListaCandidatos)})");
            else
                sbFilters.Append($"&fq=Idf_Curriculo:0");

            sbFilters.Append($"&sort=Flg_VIP+desc");

            return string.Concat(sbQuery, sbExtraParameters, sbFilters);
        }
        #endregion


        #region [MontarQueryCandidatoVaga]
        public static SolrResponse<BNE.Solr.VagaCandidato.Response> MontarQueryCandidatoVaga(int idVaga, int idCurriculo)
        {
            return BNE.Solr.VagaCandidato.EfetuarRequisicao(Solr.BuscaVagaCandidato.MontaUrlSolrCandidatura(0, 1, idVaga, idCurriculo));
        }
        #endregion

    }
}
