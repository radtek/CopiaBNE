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
            public Estado Estado { get; set; }
            public SqlGeography GeoBuscaCidade { get; set; }
            public SqlGeography GeoBuscaBairro { get; set; }
            public Escolaridade Escolaridade { get; set; }
            public Deficiencia Deficiencia { get; set; }
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

            public DateTime DataInicial { get; set; }
            public Tipo TipoBusca { get; set; }

            public enum Tipo
            {
                Curriculo,
                Vaga,
                Rastreador
            }
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
        public static string MontarQuery(int currentIndex, int pageSize, Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, bool buscaCurriculosMaisUmAno = false)
        {
            return MontarQueryBuscaCurriculo(currentIndex, pageSize, RecuperarParametros(objFilial, objUsuarioFilialPerfil, objPesquisaCurriculo), buscaCurriculosMaisUmAno);
        }
        #endregion

        #region Vaga
        public static string MontarQuery(int currentIndex, int pageSize, Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, BLL.Vaga objVaga)
        {
            return MontarQueryBuscaCurriculo(currentIndex, pageSize, RecuperarParametros(objVaga));
        }

        /// <summary>
        /// Query para recuperar apenas a quantidade de curriculos com perfil proximo ao da vaga 
        /// </summary>
        /// <param name="objVaga"></param>
        /// <returns></returns>
        public static string MontarQuery(BLL.Vaga objVaga)
        {
            return MontarQueryBuscaCurriculo(0, 0, RecuperarParametros(objVaga));
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
        public static string MontarQuery(int currentIndex, int pageSize, RastreadorCurriculo objRastreadorCurriculo, DateTime? ultimaVisualizacao = null)
        {
            var parametros = RecuperarParametros(objRastreadorCurriculo);
            if (ultimaVisualizacao.HasValue)
                parametros.DataInicial = ultimaVisualizacao.Value;
            else
                parametros.DataInicial = objRastreadorCurriculo.DataCadastro.Value;

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
        private static Parametros RecuperarParametros(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo)
        {
            return new Parametros
            {
                TipoBusca = Parametros.Tipo.Curriculo,

                Origem = objPesquisaCurriculo.Origem,
                Filial = objFilial,
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Funcoes = PesquisaCurriculoFuncao.ListarIdentificadoresFuncaoPorPesquisa(objPesquisaCurriculo),
                Idiomas = PesquisaCurriculoIdioma.ListarIdentificadoresIdiomaPorPesquisa(objPesquisaCurriculo),
                Disponibilidades = PesquisaCurriculoDisponibilidade.ListarIdentificadoresDisponibilidadePorPesquisa(objPesquisaCurriculo),
                Cidade = objPesquisaCurriculo.Cidade,
                Estado = objPesquisaCurriculo.Estado,
                GeoBuscaCidade = objPesquisaCurriculo.GeoBuscaCidade,
                GeoBuscaBairro = objPesquisaCurriculo.GeoBuscaBairro,
                Escolaridade = objPesquisaCurriculo.Escolaridade,
                Deficiencia = objPesquisaCurriculo.Deficiencia,
                Sexo = objPesquisaCurriculo.Sexo,
                Raca = objPesquisaCurriculo.Raca,
                EstadoCivil = objPesquisaCurriculo.EstadoCivil,
                TipoVeiculo = objPesquisaCurriculo.TipoVeiculo,
                CategoriaHabilitacao = objPesquisaCurriculo.CategoriaHabilitacao,
                AreaBNE = objPesquisaCurriculo.AreaBNE,

                IdadeMinima = objPesquisaCurriculo.NumeroIdadeMin,
                IdadeMaxima = objPesquisaCurriculo.NumeroIdadeMax,
                ValorSalarioMinimo = objPesquisaCurriculo.NumeroSalarioMin,
                ValorSalarioMaximo = objPesquisaCurriculo.NumeroSalarioMax,

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
        }

        private static Parametros RecuperarParametros(RastreadorCurriculo objRastreadorCurriculo)
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
                Estado = objRastreadorCurriculo.Estado,
                GeoBuscaCidade = objRastreadorCurriculo.GeoBuscaCidade,
                GeoBuscaBairro = objRastreadorCurriculo.GeoBuscaBairro,
                Escolaridade = objRastreadorCurriculo.Escolaridade,
                Deficiencia = objRastreadorCurriculo.Deficiencia,
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

        private static Parametros RecuperarParametros(BLL.Vaga objVaga)
        {
            return new Parametros
            {
                TipoBusca = Parametros.Tipo.Vaga,
                Filial = objVaga.Filial,
                UsuarioFilialPerfil = objVaga.UsuarioFilialPerfil,
                Funcoes = new List<int> { objVaga.Funcao.IdFuncao },
                Disponibilidades = VagaDisponibilidade.ListarIdentificadoresDisponibilidadePorVaga(objVaga),
                Cidade = objVaga.Cidade,
                Escolaridade = objVaga.Escolaridade,
                Deficiencia = objVaga.Deficiencia,
                Sexo = objVaga.Sexo,
                IdadeMinima = objVaga.NumeroIdadeMinima,
                IdadeMaxima = objVaga.NumeroIdadeMaxima,
                ValorSalarioMinimo = objVaga.ValorSalarioDe,
                ValorSalarioMaximo = objVaga.ValorSalarioPara
            };
        }
        #endregion

        #region Query de Busca de Currículo
        private static string MontarQueryBuscaCurriculo(int currentIndex, int pageSize, Parametros objParametros, bool buscaCurriculosMaisUmAno = false)
        {
            var sbFilters = new StringBuilder();
            var sbQuery = new StringBuilder();
            var sbExtraParameters = new StringBuilder();

            sbQuery.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCV));
            sbQuery.Append("?q={!boost=recip(ms(NOW,Dta_Atualizacao),3.16e-11,1,1)}");

            sbExtraParameters.Append("&wt=json"); //retorno JSON            
            sbExtraParameters.AppendFormat("&start={0}", ((currentIndex * pageSize))); //Primeiro registro
            sbExtraParameters.AppendFormat("&rows={0}", pageSize); //Número de registros retornados


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
                    sbFilters.AppendFormat("&fq=(Num_DDD_Celular:{0} OR Num_DDD_Telefone:{0} OR Num_DDD_Celular_Contato:{0} OR Num_DDD_Telefone_Contato:{0})", objParametros.DDDTelefone);

                if (!String.IsNullOrEmpty(objParametros.Telefone))
                    sbFilters.AppendFormat("&fq=(Num_Celular:{0} OR Num_Telefone:{0} OR Num_Celular_Contato:{0} OR Num_Telefone_Contato:{0})", objParametros.Telefone);
            }

            if (!string.IsNullOrEmpty(objParametros.Email))
                sbFilters.AppendFormat("&fq=Eml_Pessoa:{0}", objParametros.Email);

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
                if (objParametros.GeoBuscaCidade != null && objParametros.GeoBuscaBairro == null)
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
                            sbQuery.Append("(" + objParametros.CamposPalavraChave[i] + ":" + '"' + HttpUtility.UrlEncode(objParametros.PalavraChave.Replace(",", "").Replace(";", "")) + '"');
                        else
                            sbQuery.Append(" OR " + objParametros.CamposPalavraChave[i] + ":" + '"' + HttpUtility.UrlEncode(objParametros.PalavraChave.Replace(",", "").Replace(";", "")) + '"');
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
                    sbFilters.AppendFormat("&fq=!Raz_Social:({0})", objParametros.FiltroExcludente.Replace(",", " "));
                else
                    sbFilters.AppendFormat("&fq=!Raz_Social:\"{0}\"~4", objParametros.FiltroExcludente);
            }

            if (objParametros.Estado != null)
                sbFilters.AppendFormat("&fq=Sig_Estado:{0}", objParametros.Estado.SiglaEstado);

            if (objParametros.Escolaridade != null)
            {
                objParametros.Escolaridade.CompleteObject();
                if (objParametros.Escolaridade.SequenciaPeso.HasValue)
                    sbFilters.AppendFormat("&fq=Idf_Escolaridade:[{0} TO *]", objParametros.Escolaridade.IdEscolaridade);
            }

            if (objParametros.Sexo != null)
                sbFilters.AppendFormat("&fq=Idf_Sexo:{0}", objParametros.Sexo.IdSexo);

            if (objParametros.IdadeMinima.HasValue && objParametros.IdadeMaxima.HasValue)
                sbFilters.AppendFormat("&fq=Dta_Nascimento:[{0} TO {1}]", DateTime.Today.AddYears(-objParametros.IdadeMaxima.Value - 1).ToString("yyyy-MM-ddT00:00:00.000Z"), DateTime.Today.AddYears(-objParametros.IdadeMinima.Value).ToString("yyyy-MM-ddT00:00:00.000Z"));
            else
            {
                if (objParametros.IdadeMinima.HasValue)
                    sbFilters.AppendFormat("&fq=Dta_Nascimento:[* TO {0}]", DateTime.Today.AddYears(-objParametros.IdadeMinima.Value + 1).ToString("yyyy-MM-ddT00:00:00.000Z"));

                if (objParametros.IdadeMaxima.HasValue)
                    sbFilters.AppendFormat("&fq=Dta_Nascimento:[{0} TO *]", DateTime.Today.AddYears(-objParametros.IdadeMaxima.Value - 1).ToString("yyyy-MM-ddT00:00:00.000Z"));
            }

            if (objParametros.ValorSalarioMinimo.HasValue && objParametros.ValorSalarioMaximo.HasValue)
                sbFilters.AppendFormat("&fq=Vlr_Pretensao_Salarial:[{0}+TO+{1}]", objParametros.ValorSalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture), objParametros.ValorSalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture));
            else
            {
                if (objParametros.ValorSalarioMinimo.HasValue)
                    sbFilters.AppendFormat("&fq=Vlr_Pretensao_Salarial:[{0}+TO+*]", objParametros.ValorSalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture));

                if (objParametros.ValorSalarioMaximo.HasValue)
                    sbFilters.AppendFormat("&fq=Vlr_Pretensao_Salarial:[*+TO+{0}]", objParametros.ValorSalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture));
            }

            //Campo Solr -> Total_Experiencia
            if (objParametros.QuantidadeExperiencia.HasValue)
                sbFilters.AppendFormat("&fq=Total_Experiencia:[{0}+TO+*]&", objParametros.QuantidadeExperiencia.Value);

            if (objParametros.Disponibilidades != null && objParametros.Disponibilidades.Count > 0)
                sbFilters.AppendFormat("&fq=Idf_Disponibilidade:({0})", string.Join(" AND ", objParametros.Disponibilidades));

            if (objParametros.EstadoCivil != null)
                sbFilters.AppendFormat("&fq=Idf_Estado_Civil:{0}", objParametros.EstadoCivil.IdEstadoCivil);

            if (!String.IsNullOrEmpty(objParametros.Bairro))
            {
                if (objParametros.GeoBuscaBairro != null)
                {
                    sbFilters.Append("&fq=%7b!geofilt%7d&sfield=Geo_Localizacao");
                    sbFilters.AppendFormat("&pt={0},{1}", objParametros.GeoBuscaBairro.Lat.ToString().Replace(',', '.'), objParametros.GeoBuscaBairro.Long.ToString().Replace(',', '.'));
                    sbFilters.AppendFormat("&d={0}", objParametros.Cidade != null && objParametros.Cidade.IdCidade == 5345 /* São Paulo */ ? Parametro.RecuperaValorParametro(Enumeradores.Parametro.DistanciaPadraoBuscaCVCidadeSaoPaulo) : Parametro.RecuperaValorParametro(Enumeradores.Parametro.DistanciaPadraoBuscaCVCidade));
                }
                else
                    sbFilters.AppendFormat("&fq=Des_Bairro:(\"{0}\")", objParametros.Bairro.Replace(",", "\" \""));
            }

            if (!String.IsNullOrEmpty(objParametros.Zona))
                sbFilters.AppendFormat("&fq=Des_Zona:(\"{0}\")", objParametros.Zona.Replace(",", "\" \""));

            if (!String.IsNullOrEmpty(objParametros.Logradouro))
                sbFilters.AppendFormat("&fq=Des_Logradouro:\"{0}\"~2", objParametros.Logradouro);

            if (!String.IsNullOrEmpty(objParametros.NumeroCEPInicial) && !String.IsNullOrEmpty(objParametros.NumeroCEPFinal))
                sbFilters.AppendFormat("&fq=Num_CEP:[{0}+TO+{1}]&", objParametros.NumeroCEPInicial, objParametros.NumeroCEPFinal);
            else
            {
                if (!String.IsNullOrEmpty(objParametros.NumeroCEPInicial))
                    sbFilters.AppendFormat("&fq=Num_CEP:[{0}+TO+*]&", objParametros.NumeroCEPInicial);

                if (!String.IsNullOrEmpty(objParametros.NumeroCEPFinal))
                    sbFilters.AppendFormat("&fq=Num_CEP:[*+TO+{0}]&", objParametros.NumeroCEPFinal);
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

                sbFilters.AppendFormat("&fq=Des_Curso:\"{0}\"", valueCurso);
            }

            //Campo Solr -> Des_Fonte
            if (objParametros.FonteTecnicoGraduacao != null)
            {
                objParametros.FonteTecnicoGraduacao.CompleteObject();

                sbFilters.AppendFormat("&fq=Idf_Fonte:{0}", objParametros.FonteTecnicoGraduacao.IdFonte);
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

                sbFilters.AppendFormat("&fq=Des_Curso:\"{0}\"", valueOutrosCursos);
            }

            if (objParametros.FonteOutros != null)
            {
                objParametros.FonteOutros.CompleteObject();
                sbFilters.AppendFormat("&fq=Des_Fonte:{0}", objParametros.FonteOutros.NomeFonte);
            }

            if (!String.IsNullOrEmpty(objParametros.Experiencia))
                sbFilters.AppendFormat("&fq=Des_Atividade:{0}", objParametros.Experiencia);

            if (objParametros.FuncaoExercida != null)
            {
                //Verificando se a descrição da funcao esta presente no objeto. Se nao, completa para pesquisa por descricao.
                if (String.IsNullOrEmpty(objParametros.FuncaoExercida.DescricaoFuncao))
                    objParametros.FuncaoExercida.CompleteObject();

                sbFilters.AppendFormat("&fq=Des_Funcao_Exercida:{0}", objParametros.FuncaoExercida.DescricaoFuncao);
            }
            else if (!String.IsNullOrEmpty(objParametros.DescricaoFuncaoExercida))
                sbFilters.AppendFormat("&fq=Des_Funcao_Exercida:{0}", objParametros.DescricaoFuncaoExercida);

            if (!String.IsNullOrEmpty(objParametros.RazaoSocial))
            {
                sbFilters.AppendFormat("&fq=Raz_Social:\"{0}\"~2", objParametros.RazaoSocial);
            }

            if (objParametros.AreaBNE != null)
                sbFilters.AppendFormat("&fq=Idf_Area_BNE:{0}", objParametros.AreaBNE.IdAreaBNE);

            if (objParametros.CategoriaHabilitacao != null)
            {
                switch (objParametros.CategoriaHabilitacao.IdCategoriaHabilitacao)
                {
                    case 1:
                        sbFilters.Append("&fq=Idf_Categoria_Habilitacao:(1+6+7+8+9)");
                        break;
                    case 2:
                        sbFilters.Append("&fq=Idf_Categoria_Habilitacao:[2 TO 9]");
                        break;
                    case 3:
                        sbFilters.Append("&fq=Idf_Categoria_Habilitacao:(3+4+5+7+8+9)");
                        break;
                    case 4:
                        sbFilters.Append("&fq=Idf_Categoria_Habilitacao:(4+5+8+9)");
                        break;
                    case 5:
                        sbFilters.Append("&fq=Idf_Categoria_Habilitacao:(5+9)");
                        break;
                    case 6:
                        sbFilters.Append("&fq=Idf_Categoria_Habilitacao:[6 TO 9]");
                        break;
                    case 7:
                        sbFilters.Append("&fq=Idf_Categoria_Habilitacao:[7 TO 9]");
                        break;
                    case 8:
                        sbFilters.Append("&fq=Idf_Categoria_Habilitacao:[8 TO 9]");
                        break;
                    default:
                        sbFilters.AppendFormat("&fq=Idf_Categoria_Habilitacao:{0}", objParametros.CategoriaHabilitacao.IdCategoriaHabilitacao);
                        break;
                }
            }

            if (objParametros.TipoVeiculo != null)
                sbFilters.AppendFormat("&fq=Idf_Tipo_Veiculo:{0}", objParametros.TipoVeiculo.IdTipoVeiculo);

            if (objParametros.Deficiencia != null)
            {
                if (objParametros.Deficiencia.IdDeficiencia == 0)
                    sbFilters.Append("&fq=-(-Idf_Deficiencia:[* TO *] AND Idf_Deficiencia:0)");
                else if (objParametros.Deficiencia.IdDeficiencia == 7)
                    sbFilters.Append("&fq=Idf_Deficiencia:(1+2+3+4+5+6+7)");
                else
                    sbFilters.AppendFormat("&fq=Idf_Deficiencia:{0}", objParametros.Deficiencia.IdDeficiencia);
            }
            else
                sbFilters.Append("&fq=-(-Idf_Deficiencia:[* TO *] AND Idf_Deficiencia:0)");

            if (objParametros.Raca != null)
                sbFilters.AppendFormat("&fq=Idf_Raca:{0}", objParametros.Raca.IdRaca);

            if (objParametros.Filhos != null)
                sbFilters.AppendFormat("&fq=Flg_Filhos:{0}", objParametros.Filhos);

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
                        sbFilters.Append("&fq={!join from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=cv_idioma}Idf_Idioma:" + idioma.Key + "%20AND%20Idf_Nivel_Idioma:[" + idioma.Value + "%20TO%20*]");
                    else
                        idiomaSemNivel.Add(idioma.Key);

                }
                if (idiomaSemNivel.Count > 0)
                    sbFilters.AppendFormat("&fq=Idf_Idioma:({0})", string.Join(" ", idiomaSemNivel));
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
                    sbFilters.AppendFormat("termfreq(Idfs_Origens,{0})+desc,", objParametros.Origem.IdOrigem);
            }
            //Cidade Preenchida
            if (objParametros.Cidade != null)
            {
                if (objParametros.GeoBuscaCidade != null && objParametros.GeoBuscaBairro != null)
                {
                    string[] partsBairro = objParametros.Bairro.Trim().Replace(",", "").Split(' ');
                    foreach (string part in partsBairro)
                        sbFilters.AppendFormat("termfreq(Des_Bairro,{0})+desc,", part.Trim());

                    sbFilters.AppendFormat("termfreq(Idf_Cidade,{0})+desc,", objParametros.Cidade.IdCidade);
                }
                else
                {
                    if (objParametros.GeoBuscaCidade != null && objParametros.GeoBuscaBairro == null)
                        sbFilters.AppendFormat("termfreq(Idf_Cidade,{0})+desc,", objParametros.Cidade.IdCidade);
                    else
                        sbFilters.AppendFormat("termfreq(Idf_Cidade,{0})+desc,", objParametros.Cidade.IdCidade);
                }
            }
            //Funcao
            if (objParametros.Funcoes.Count > 0)
            {
                foreach (int funcao in objParametros.Funcoes)
                {
                    sbFilters.AppendFormat("termfreq(Idf_Funcao,{0})+desc,", funcao);
                }
            }



            //Bairro preenchido
            if (objParametros != null && objParametros.Cidade == null && (objParametros.GeoBuscaBairro != null || (!String.IsNullOrEmpty(objParametros.Bairro))))
            {
                if (objParametros.GeoBuscaBairro != null)
                    sbFilters.Append("geodist()+asc,");
                else
                {
                    if (!String.IsNullOrEmpty(objParametros.Bairro))
                        sbFilters.AppendFormat("termfreq(Des_Bairro,'{0}')+desc,", objParametros.Bairro.Trim().Substring(0, objParametros.Bairro.Length - 1));
                }

            }

            //Se pesquisado estag
            if (!string.IsNullOrWhiteSpace(objParametros.EscolaridadesWebEstagios))
            {
                sbFilters.Append("Flg_CurriculoAceitaEstagio desc,");
            }

            sbFilters.Append("Flg_VIP+desc,Dta_Atualizacao+desc");

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



    }
}
