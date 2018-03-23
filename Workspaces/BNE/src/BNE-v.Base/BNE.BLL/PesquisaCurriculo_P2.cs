//-- Data: 10/08/2010 15:09
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using BNE.BLL.Custom;
using Newtonsoft.Json;
using RestSharp.Contrib;

namespace BNE.BLL
{
    public partial class PesquisaCurriculo // Tabela: TAB_Pesquisa_Curriculo
    {

        public static bool BuscarSolr
        {
            get
            {
                return Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.BuscaCVSolr));
            }
        }

        #region Consultas

        private const string Spatualizarquantidadecurriculosretornados = @"
        UPDATE TAB_Pesquisa_Curriculo SET Qtd_Curriculo_Retorno = @Qtd_Curriculo_Retorno WHERE Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo
        ";
        #endregion

        #region BuscaCurriculo
        public static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, out int numeroRegistros, out decimal mediaSalarial)
        {
            if (BuscarSolr)
                return BuscaCurriculoSolr(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, objPesquisaCurriculo, out numeroRegistros, out mediaSalarial);

            return BuscaCurriculoSQL(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, objPesquisaCurriculo, out numeroRegistros, out mediaSalarial);
        }

        private static DataTable BuscaCurriculoSQL(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, out int numeroRegistros, out decimal mediaSalarial)
        {
            var criteriosBusca = new ParametroBuscaCV
                {
                    FlagIdfOrigem = true
                };

            DataTable dt;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output },
                    new SqlParameter { ParameterName = "@MediaSalarial", SqlDbType = SqlDbType.Decimal, Value = 0M, Direction = ParameterDirection.Output } ,
                    new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = idOrigem }
                };

            if (idFilial.HasValue)
            {
                parms.Add(new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idFilial });
                criteriosBusca.FlagIdfFilial = true;
            }

            if (idUsuarioFilialPerfil.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idUsuarioFilialPerfil });

            #region Pesquisa Avançada - Pesquisas Específicas
            if (objPesquisaCurriculo != null && objPesquisaCurriculo.FlagPesquisaAvancada)
            {
                #region CPF e Idf_Curriculo
                if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCodCPFNome))
                {
                    //Verificando a possibilidade de ser um CPF formatado XXX.XXX.XXX-XX    
                    string busca;
                    string possivelCPF = busca = objPesquisaCurriculo.DescricaoCodCPFNome;

                    if (Regex.IsMatch(possivelCPF, @"\d{3}.\d{3}.\d{3}-\d{2}"))
                    {
                        const string sPadrao = @"[.\\/-]";
                        var oRegEx = new Regex(sPadrao);
                        busca = oRegEx.Replace(possivelCPF, "");
                    }

                    if (Regex.IsMatch(busca, @"^\d{11}$"))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = Convert.ToDecimal(busca) });
                        criteriosBusca.FlagNumCPF = true;
                    }

                    Int32 aux;
                    if (Int32.TryParse(busca, out aux) || busca.Contains(","))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.VarChar, Value = busca });
                        criteriosBusca.FlagIdfCurriculo = true;
                    }
                }
                #endregion

                #region DDD e Telefone
                if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroDDDTelefone) || !String.IsNullOrEmpty(objPesquisaCurriculo.NumeroTelefone))
                {
                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroDDDTelefone))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Num_DDD", SqlDbType = SqlDbType.Char, Size = 2, Value = objPesquisaCurriculo.NumeroDDDTelefone });
                        criteriosBusca.FlagNumDDD = true;
                    }

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroTelefone))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Num_Telefone", SqlDbType = SqlDbType.Char, Size = 10, Value = objPesquisaCurriculo.NumeroTelefone });
                        criteriosBusca.FlagNumTelefone = true;
                    }
                }
                #endregion

                #region Email Pessoa
                if (!String.IsNullOrEmpty(objPesquisaCurriculo.EmailPessoa))
                {
                    parms.Add(new SqlParameter { ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 100, Value = objPesquisaCurriculo.EmailPessoa });
                    criteriosBusca.FlagEmlPessoa = true;
                }
                #endregion

            }
            #endregion

            #region Parametros Básicos

            Object valueLimiteResultado;

            if (objPesquisaCurriculo != null && (objPesquisaCurriculo.Funcao != null || objPesquisaCurriculo.Cidade != null || !String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoPalavraChave)))
                valueLimiteResultado = Parametro.RecuperaValorParametro(Enumeradores.Parametro.LimiteResultadoPesquisa);
            else
                valueLimiteResultado = Parametro.RecuperaValorParametro(Enumeradores.Parametro.LimiteResultadoPesquisaSemValoresInformados);

            if (idFilial.HasValue)
            {
                //Libera o máximo de currículos para a contax
                var idfContax = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CodigoIdentificadorFilialContax));
                if (idFilial.Value.Equals(idfContax))
                    valueLimiteResultado = Parametro.RecuperaValorParametro(Enumeradores.Parametro.CodigoIdentificadorFilialContax);
            }

            parms.Add(new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual });
            parms.Add(new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina });
            parms.Add(new SqlParameter { ParameterName = "@LimiteResultado", SqlDbType = SqlDbType.Int, Size = 4, Value = valueLimiteResultado });

            #endregion

            if (objPesquisaCurriculo != null)
            {
                //Configurando parâmentros adicionais
                if (objPesquisaCurriculo.Funcao != null)
                {
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.Funcao.IdFuncao });
                    criteriosBusca.FlagIdfFuncao = true;
                }

                if (objPesquisaCurriculo.Cidade != null)
                {
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.Cidade.IdCidade });
                    criteriosBusca.FlagIdfCidade = true;
                }

                if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoPalavraChave))
                {
                    if (objPesquisaCurriculo.FlagPesquisaAvancada) //Caso seja PesquisaAvancada a variável é Des_Metabusca
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Des_MetaBusca", SqlDbType = SqlDbType.VarChar, Size = 500, Value = objPesquisaCurriculo.DescricaoPalavraChave });
                        criteriosBusca.FlagDesMetaBusca = true;
                    }
                    else //Do contrário é Des_Metabusca_Rapida
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Des_Metabusca_Rapida", SqlDbType = SqlDbType.VarChar, Size = 500, Value = objPesquisaCurriculo.DescricaoPalavraChave });
                        criteriosBusca.FlagDesMetabuscaRapida = true;
                    }
                }

                #region Parametros da Pesquisa Avançada - Não específico
                if (objPesquisaCurriculo.FlagPesquisaAvancada)
                {
                    if (objPesquisaCurriculo.Estado != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Sig_Estado", SqlDbType = SqlDbType.Char, Size = 2, Value = objPesquisaCurriculo.Estado.SiglaEstado });
                        criteriosBusca.FlagSigEstado = true;
                    }

                    if (objPesquisaCurriculo.Escolaridade != null)
                    {
                        objPesquisaCurriculo.Escolaridade.CompleteObject();
                        if (objPesquisaCurriculo.Escolaridade.SequenciaPeso.HasValue)
                        {
                            parms.Add(new SqlParameter { ParameterName = "@Peso_Escolaridade", SqlDbType = SqlDbType.SmallInt, Value = objPesquisaCurriculo.Escolaridade.SequenciaPeso.Value });
                            criteriosBusca.FlagPesoEscolaridade = true;
                        }
                    }

                    if (objPesquisaCurriculo.Sexo != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Sexo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.Sexo.IdSexo });
                        criteriosBusca.FlagIdfSexo = true;
                    }

                    if (objPesquisaCurriculo.DataIdadeMax.HasValue)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idade_Max", SqlDbType = SqlDbType.SmallInt, Value = DateTime.Today.Year - objPesquisaCurriculo.DataIdadeMax.Value.Year });
                        criteriosBusca.FlagIdadeMax = true;
                    }

                    if (objPesquisaCurriculo.DataIdadeMin.HasValue)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idade_Min", SqlDbType = SqlDbType.SmallInt, Value = DateTime.Today.Year - objPesquisaCurriculo.DataIdadeMin.Value.Year });
                        criteriosBusca.FlagIdadeMin = true;
                    }

                    if (objPesquisaCurriculo.NumeroSalarioMin.HasValue)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Salario_Min", SqlDbType = SqlDbType.Decimal, Size = 11, Value = objPesquisaCurriculo.NumeroSalarioMin.Value });
                        criteriosBusca.FlagSalarioMin = true;
                    }

                    if (objPesquisaCurriculo.NumeroSalarioMax.HasValue)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Salario_Max", SqlDbType = SqlDbType.Decimal, Size = 11, Value = objPesquisaCurriculo.NumeroSalarioMax.Value });
                        criteriosBusca.FlagSalarioMax = true;
                    }

                    if (objPesquisaCurriculo.QuantidadeExperiencia.HasValue)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Meses_Exp", SqlDbType = SqlDbType.SmallInt, Value = objPesquisaCurriculo.QuantidadeExperiencia.Value });
                        criteriosBusca.FlagMesesExp = true;
                    }

                    string idfsIdioma = PesquisaCurriculoIdioma.ListarIdentificadoresConcatenadosPorPesquisa(objPesquisaCurriculo.IdPesquisaCurriculo);
                    if (!String.IsNullOrEmpty(idfsIdioma))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idfs_Idioma", SqlDbType = SqlDbType.VarChar, Size = 100, Value = idfsIdioma });
                        criteriosBusca.FlagIdfsIdioma = true;
                    }

                    string idfsDisponibilidade = PesquisaCurriculoDisponibilidade.ListarIdentificadoresConcatenadosPorPesquisa(objPesquisaCurriculo);
                    if (!String.IsNullOrEmpty(idfsDisponibilidade))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idfs_Disponibilidade", SqlDbType = SqlDbType.VarChar, Size = 100, Value = idfsDisponibilidade });
                        criteriosBusca.FlagIdfsDisponibilidade = true;
                    }

                    if (objPesquisaCurriculo.EstadoCivil != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Estado_Civil", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.EstadoCivil.IdEstadoCivil });
                        criteriosBusca.FlagIdfEstadoCivil = true;
                    }

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoBairro))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Des_Bairro", SqlDbType = SqlDbType.VarChar, Size = 80, Value = objPesquisaCurriculo.DescricaoBairro });
                        criteriosBusca.FlagDesBairro = true;
                    }

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoLogradouro))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Des_Logradouro", SqlDbType = SqlDbType.VarChar, Size = 80, Value = objPesquisaCurriculo.DescricaoLogradouro });
                        criteriosBusca.FlagDesLogradouro = true;
                    }

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroCEPMin))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Num_CEP_Min", SqlDbType = SqlDbType.Char, Size = 8, Value = objPesquisaCurriculo.NumeroCEPMin });
                        criteriosBusca.FlagNumCEPMin = true;
                    }

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroCEPMax))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Num_CEP_Max", SqlDbType = SqlDbType.Char, Size = 8, Value = objPesquisaCurriculo.NumeroCEPMax });
                        criteriosBusca.FlagNumCEPMax = true;
                    }

                    if (objPesquisaCurriculo.CursoTecnicoGraduacao != null || !String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao))
                    {
                        object valueCurso;
                        if (objPesquisaCurriculo.CursoTecnicoGraduacao != null)
                        {
                            objPesquisaCurriculo.CursoTecnicoGraduacao.CompleteObject();
                            valueCurso = objPesquisaCurriculo.CursoTecnicoGraduacao.DescricaoCurso;
                        }
                        else
                            valueCurso = objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao;

                        parms.Add(new SqlParameter { ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 100, Value = valueCurso });
                        criteriosBusca.FlagDesCurso = true;
                    }

                    if (objPesquisaCurriculo.FonteTecnicoGraduacao != null)
                    {
                        objPesquisaCurriculo.FonteTecnicoGraduacao.CompleteObject();

                        parms.Add(new SqlParameter { ParameterName = "@Nme_Fonte", SqlDbType = SqlDbType.VarChar, Size = 100, Value = objPesquisaCurriculo.FonteTecnicoGraduacao.NomeFonte });
                        criteriosBusca.FlagNmeFonte = true;
                    }

                    if (objPesquisaCurriculo.CursoOutrosCursos != null || !String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoOutrosCursos))
                    {
                        object valueOutrosCursos;

                        if (objPesquisaCurriculo.CursoOutrosCursos != null)
                        {
                            objPesquisaCurriculo.CursoOutrosCursos.CompleteObject();
                            valueOutrosCursos = objPesquisaCurriculo.CursoOutrosCursos.DescricaoCurso;
                        }
                        else
                            valueOutrosCursos = objPesquisaCurriculo.DescricaoCursoOutrosCursos;

                        parms.Add(new SqlParameter { ParameterName = "@Des_Curso_Outros", SqlDbType = SqlDbType.VarChar, Size = 100, Value = valueOutrosCursos });
                        criteriosBusca.FlagDesCursoOutros = true;
                    }

                    if (objPesquisaCurriculo.FonteOutrosCursos != null)
                    {
                        objPesquisaCurriculo.FonteOutrosCursos.CompleteObject();
                        parms.Add(new SqlParameter { ParameterName = "@Nme_Fonte_Outros", SqlDbType = SqlDbType.VarChar, Size = 100, Value = objPesquisaCurriculo.FonteOutrosCursos.NomeFonte });
                        criteriosBusca.FlagNmeFonteOutros = true;
                    }

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoExperiencia))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Experiencia_Em", SqlDbType = SqlDbType.VarChar, Size = 100, Value = objPesquisaCurriculo.DescricaoExperiencia });
                        criteriosBusca.FlagExperienciaEm = true;
                    }

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.RazaoSocial))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Nme_Empresa", SqlDbType = SqlDbType.VarChar, Size = 100, Value = objPesquisaCurriculo.RazaoSocial });
                        criteriosBusca.FlagNmeEmpresa = true;
                    }

                    if (objPesquisaCurriculo.AreaBNE != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Area_Bne", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.AreaBNE.IdAreaBNE });
                        criteriosBusca.FlagIdfAreaBNE = true;
                    }

                    if (objPesquisaCurriculo.CategoriaHabilitacao != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Categoria_Habilitacao", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.CategoriaHabilitacao.IdCategoriaHabilitacao });
                        criteriosBusca.FlagIdfCategoriaHabilitacao = true;
                    }

                    if (objPesquisaCurriculo.TipoVeiculo != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Tipo_Veiculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.TipoVeiculo.IdTipoVeiculo });
                        criteriosBusca.FlagIdfTipoVeiculo = true;
                    }

                    if (objPesquisaCurriculo.Deficiencia != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Deficiencia", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.Deficiencia.IdDeficiencia });
                        criteriosBusca.FlagIdfDeficiencia = true;
                    }

                    if (objPesquisaCurriculo.Raca != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Raca", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.Raca.IdRaca });
                        criteriosBusca.FlagIdfRaca = true;
                    }

                    if (objPesquisaCurriculo.FlagFilhos != null)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Flg_Filhos", SqlDbType = SqlDbType.Bit, Size = 1, Value = objPesquisaCurriculo.FlagFilhos.Value });
                        criteriosBusca.FlagFlgFilhos = true;
                    }

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCodCPFNome))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Nme_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 100, Value = objPesquisaCurriculo.DescricaoCodCPFNome });
                        criteriosBusca.FlagNmePessoa = true;
                    }

                    if (!string.IsNullOrWhiteSpace(objPesquisaCurriculo.IdEscolaridadeWebStagio))
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_Escolaridade_WebStagio", SqlDbType = SqlDbType.VarChar, Size = 50, Value = objPesquisaCurriculo.IdEscolaridadeWebStagio });
                        // não foi criada uma flag para o critério
                    }

                    if (objPesquisaCurriculo.FlagAprendiz.HasValue)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Flg_Aprendiz", SqlDbType = SqlDbType.Bit, Size = 1, Value = objPesquisaCurriculo.FlagAprendiz.Value });
                        criteriosBusca.FlagAprendiz = true;
                    }
                }
                #endregion

            }

            //Recupera todas as buscas
            var buscas = ParametroBuscaCV.ListarParametros(); //TODO - Cachear resultado

            //Varre todas as buscas parametrizadas no banco para determinar qual é a melhor a ser usada.
            ParametroBuscaCV buscaEspecifica = null;
            var excluirPropriedades = new[]
                    {
                        "IdParametroBuscaCV",
                        "FlagInativo",
                        "NomeSPBusca"
                    };

            int i = 0;
            do
            {
                if (CompareObject.Compare(criteriosBusca, buscas[i], excluirPropriedades))
                    buscaEspecifica = buscas[i];

                i++;
            } while (buscaEspecifica == null && buscas.Count > i);

            if (buscaEspecifica != null && (objPesquisaCurriculo != null && string.IsNullOrWhiteSpace(objPesquisaCurriculo.IdEscolaridadeWebStagio)))
            {
                dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, buscaEspecifica.NomeSPBusca, parms).Tables[0];
                numeroRegistros = Convert.ToInt32(parms[0].Value);
                mediaSalarial = parms[1].Value != DBNull.Value ? Convert.ToDecimal(parms[1].Value) : 0M;
            }
            else //Se não achou busca específica vai pro geralzão
            {
                dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_GERAL", parms).Tables[0];
                numeroRegistros = Convert.ToInt32(parms[0].Value);
                mediaSalarial = parms[1].Value != DBNull.Value ? Convert.ToDecimal(parms[1].Value) : 0M;
            }

            if (paginaAtual.Equals(0) && objPesquisaCurriculo != null) //Se for a primeira página, para evitar salvar quando for paginação
                objPesquisaCurriculo.AtualizarQuantidadeRegistros(numeroRegistros);

            dt.Columns.Add("Des_SMS", typeof(string));

            System.Data.DataColumn idDeficiencia = new DataColumn("Idf_Deficiencia", typeof(int));
            idDeficiencia.DefaultValue = 0;
            dt.Columns.Add(idDeficiencia);

            return dt;
        }

        private static DataTable BuscaCurriculoSolr(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, out int numeroRegistros, out decimal mediaSalarial)
        {
            //URL padrão Solr
            var urlCVsSolr = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCV);
            var urlCVsClassificacaoSolr = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCVClassificacao);
            var urlCVsSms = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCVSms);

            //Pegar stats da amostra
            //&stats=true&stats.field=Vlr_Pretensao_Salarial
            var qSolr = "?q={!boost=recip(ms(NOW,Dta_Atualizacao),3.16e-11,1,1)}";
            urlCVsSolr += "&wt=json"; //retorno JSON            
            urlCVsSolr += "&start=" + ((paginaAtual * tamanhoPagina)); //Primeiro registro
            urlCVsSolr += "&rows=" + tamanhoPagina; //Número de registros retornados

            //Quando logado add -1 year : -3 year
            //DateTime dataLimiteInferior = DateTime.Now;
            //dataLimiteInferior = idFilial.HasValue ? dataLimiteInferior.AddYears(-Int32.Parse(Parametro.RecuperaValorParametro(Enumeradores.Parametro.AnosLimiteBuscaSolrLogado))) : dataLimiteInferior.AddYears(-Int32.Parse(Parametro.RecuperaValorParametro(Enumeradores.Parametro.AnosLimiteBuscaSolrDeslogado)));
            //urlCVsSolr += "&fq=Dta_Atualizacao%3A%5B" + dataLimiteInferior.Year + "-" + dataLimiteInferior.Month.ToString().PadLeft(2, '0') + "-" + dataLimiteInferior.Day.ToString().PadLeft(2, '0') + "T18%3A31%3A28.833Z+TO+*%5D";

            /*CLASSIFICACAO DO CV AINDA NÃO ESTÁ FEITA*/
            if (idFilial.HasValue)
            {
                urlCVsClassificacaoSolr += "?q=*%3A*&wt=json&indent=true";
                urlCVsClassificacaoSolr += "&fq=Idf_Filial%3A" + idFilial;

                urlCVsSms += "?q=*%3A*&wt=json&indent=true&sort=Dta_Cadastro+desc&rows=3";
                urlCVsSms += "&fq=Idf_Filial%3A" + idFilial;


                //Se passado usuario associacao add só comentarios do usuario
                if (idUsuarioFilialPerfil.HasValue)
                {
                    urlCVsClassificacaoSolr += "&fq=Idf_Usuario_Filial_Perfil%3A" + idUsuarioFilialPerfil;
                    urlCVsSms += "&fq=Idf_Usuario_Filial_Perfil%3A" + idUsuarioFilialPerfil;
                }
            }

            #region Pesquisa Avançada - Pesquisas Específicas
            if (objPesquisaCurriculo != null && objPesquisaCurriculo.FlagPesquisaAvancada)
            {
                //campo solr -> buscaBNE
                #region CPF e Idf_Curriculo
                if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCodCPFNome))
                {

                    //Verificando a possibilidade de ser um CPF formatado XXX.XXX.XXX-XX    
                    string busca;
                    string possivelCPF = busca = objPesquisaCurriculo.DescricaoCodCPFNome;

                    if (Regex.IsMatch(possivelCPF, @"\d{3}.\d{3}.\d{3}-\d{2}"))
                    {
                        const string sPadrao = @"[.\\/-]";
                        var oRegEx = new Regex(sPadrao);
                        busca = oRegEx.Replace(possivelCPF, "");
                    }

                    if (busca.Contains(",") || busca.Contains(";"))
                    {
                        urlCVsSolr += "&fq=Idf_Curriculo%3A(" + busca.Replace(",", " ") + ")";
                    }
                    else
                    {
                        urlCVsSolr += "&fq=buscaBNE%3A\"" + busca + "\"~4";
                    }
                }
                #endregion

                //campos solr -> telefones e ddds -> copyfields stored false
                #region DDD e Telefone
                if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroDDDTelefone) || !String.IsNullOrEmpty(objPesquisaCurriculo.NumeroTelefone))
                {
                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroDDDTelefone))
                        urlCVsSolr += "&fq=ddds%3A" + objPesquisaCurriculo.NumeroDDDTelefone;

                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroTelefone))
                        urlCVsSolr += "&fq=telefones%3A" + objPesquisaCurriculo.NumeroTelefone;
                }
                #endregion

                //campo solr -> Eml_Pessoa
                #region Email Pessoa
                if (!String.IsNullOrEmpty(objPesquisaCurriculo.EmailPessoa))
                    urlCVsSolr += "&fq=Eml_Pessoa%3A" + objPesquisaCurriculo.EmailPessoa;
                #endregion

            }
            #endregion

            if (objPesquisaCurriculo != null)
            {
                //Configurando parâmentros adicionais
                if (objPesquisaCurriculo.Funcao != null)
                {
                    //Campo solr -> Idfs_Funcoes
                    urlCVsSolr += "&fq=Idfs_Funcoes%3A" + objPesquisaCurriculo.Funcao.IdFuncao;
                }

                if (objPesquisaCurriculo.Cidade != null)
                {
                    //Campo solr -> Idfs_Cidades                    
                    urlCVsSolr += "&fq=Idfs_Cidades%3A" + objPesquisaCurriculo.Cidade.IdCidade;
                }

                if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoPalavraChave))
                {
                    //Campos Solr -> allsearch
                    if (objPesquisaCurriculo.FlagPesquisaAvancada) //Caso seja PesquisaAvancada a variável é Des_Metabusca
                        qSolr += HttpUtility.UrlEncode(objPesquisaCurriculo.DescricaoPalavraChave.Replace(",", "").Replace(";", ""));
                    else //Do contrário é Des_Metabusca_Rapida    
                        qSolr += HttpUtility.UrlEncode(objPesquisaCurriculo.DescricaoPalavraChave.Replace(",", "").Replace(";", ""));
                }

                if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoFiltroExcludente))
                {
                    if (objPesquisaCurriculo.DescricaoFiltroExcludente.Contains(",") || objPesquisaCurriculo.DescricaoFiltroExcludente.Contains(";"))
                        urlCVsSolr += "&fq=!Raz_Social%3A(" + objPesquisaCurriculo.DescricaoFiltroExcludente.Replace(",", " ") + ")";
                    else
                        urlCVsSolr += "&fq=!Raz_Social%3A\"" + objPesquisaCurriculo.DescricaoFiltroExcludente + "\"~4";
                }

                #region Parametros da Pesquisa Avançada - Não específico
                if (objPesquisaCurriculo.FlagPesquisaAvancada)
                {
                    //Campos Solr -> Sig_Estado
                    if (objPesquisaCurriculo.Estado != null)
                        urlCVsSolr += "&fq=Sig_Estado%3A" + objPesquisaCurriculo.Estado.SiglaEstado;

                    //Campo Solr -> Idf_Escolaridade [X TO *]
                    if (objPesquisaCurriculo.Escolaridade != null)
                    {
                        objPesquisaCurriculo.Escolaridade.CompleteObject();
                        if (objPesquisaCurriculo.Escolaridade.SequenciaPeso.HasValue)
                            urlCVsSolr += "&fq=Idf_Escolaridade:[" + objPesquisaCurriculo.Escolaridade.IdEscolaridade + " TO *]";
                    }

                    //Campo Solr -> Idf_Sexo
                    if (objPesquisaCurriculo.Sexo != null)
                        urlCVsSolr += "&fq=Idf_Sexo%3A" + objPesquisaCurriculo.Sexo.IdSexo;

                    //Campo Solr -> Num_Idade
                    if (objPesquisaCurriculo.DataIdadeMax.HasValue && objPesquisaCurriculo.DataIdadeMin.HasValue)
                        urlCVsSolr += "&fq=Dta_Nascimento%3A%5B" + (objPesquisaCurriculo.DataIdadeMax.Value.Year - 1) + "-" + objPesquisaCurriculo.DataIdadeMax.Value.Month.ToString().PadLeft(2, '0') + "-" + (objPesquisaCurriculo.DataIdadeMax.Value.Day - 1).ToString().PadLeft(2, '0') + "T00:00:00.000Z+TO+" + objPesquisaCurriculo.DataIdadeMin.Value.Year + "-" + objPesquisaCurriculo.DataIdadeMin.Value.Month.ToString().PadLeft(2, '0') + "-" + objPesquisaCurriculo.DataIdadeMin.Value.Day.ToString().PadLeft(2, '0') + "T23:59:59.999Z%5D&";
                    else
                    {
                        if (objPesquisaCurriculo.DataIdadeMin.HasValue)
                            urlCVsSolr += "&fq=Dta_Nascimento%3A%5B*+TO+" + objPesquisaCurriculo.DataIdadeMin.Value.Year + "-" + objPesquisaCurriculo.DataIdadeMin.Value.Month.ToString().PadLeft(2, '0') + "-" + objPesquisaCurriculo.DataIdadeMin.Value.Day.ToString().PadLeft(2, '0') + "T23:59:59.999Z%5D&";


                        if (objPesquisaCurriculo.DataIdadeMax.HasValue)
                            urlCVsSolr += "&fq=Dta_Nascimento%3A%5B" + (objPesquisaCurriculo.DataIdadeMax.Value.Year - 1) + "-" + objPesquisaCurriculo.DataIdadeMax.Value.Month.ToString().PadLeft(2, '0') + "-" + (objPesquisaCurriculo.DataIdadeMax.Value.Day - 1).ToString().PadLeft(2, '0') + "T00:00:00.000Z+TO+*%5D&";

                    }

                    //Campos Solr -> Vlr_Pretensao_Salarial
                    if (objPesquisaCurriculo.NumeroSalarioMin.HasValue && objPesquisaCurriculo.NumeroSalarioMax.HasValue)
                        urlCVsSolr += "&fq=Vlr_Pretensao_Salarial%3A%5B" + objPesquisaCurriculo.NumeroSalarioMin.Value.ToString().Replace(",", ".") + "+TO+" + objPesquisaCurriculo.NumeroSalarioMax.Value.ToString().Replace(",", ".") + "%5D&";
                    else
                    {
                        if (objPesquisaCurriculo.NumeroSalarioMin.HasValue)
                            urlCVsSolr += "&fq=Vlr_Pretensao_Salarial%3A%5B" + objPesquisaCurriculo.NumeroSalarioMin.Value.ToString().Replace(",", ".") + "+TO+*%5D&";

                        if (objPesquisaCurriculo.NumeroSalarioMax.HasValue)
                            urlCVsSolr += "&fq=Vlr_Pretensao_Salarial%3A%5B*+TO+" + objPesquisaCurriculo.NumeroSalarioMax.Value.ToString().Replace(",", ".") + "%5D&";
                    }

                    //Campo Solr -> Total_Experiencia
                    if (objPesquisaCurriculo.QuantidadeExperiencia.HasValue)
                        urlCVsSolr += "&fq=Total_Experiencia%3A%5B" + objPesquisaCurriculo.QuantidadeExperiencia.Value + "+TO+*%5D&";

                    //Campo Solr -> Idf_Idioma
                    string idfsIdioma = PesquisaCurriculoIdioma.ListarIdentificadoresConcatenadosPorPesquisa(objPesquisaCurriculo.IdPesquisaCurriculo);
                    if (!String.IsNullOrEmpty(idfsIdioma))
                        urlCVsSolr += "&fq=Idf_Idioma%3A(" + idfsIdioma.Replace(",", " ") + ")";

                    //Campo Solr -> Idf_Disponibilidade                    
                    string idfsDisponibilidade = PesquisaCurriculoDisponibilidade.ListarIdentificadoresConcatenadosPorPesquisa(objPesquisaCurriculo);
                    if (!String.IsNullOrEmpty(idfsDisponibilidade))
                        urlCVsSolr += "&fq=Idf_Disponibilidade%3A(" + idfsDisponibilidade.Replace(",", " ") + ")";

                    //Campo Solr -> Idf_Estado_Civil                    
                    if (objPesquisaCurriculo.EstadoCivil != null)
                        urlCVsSolr += "&fq=Idf_Estado_Civil%3A" + objPesquisaCurriculo.EstadoCivil.IdEstadoCivil;

                    //Campo Solr -> Des_Bairro
                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoBairro))
                        urlCVsSolr += "&fq=Des_Bairro%3A(\"" + objPesquisaCurriculo.DescricaoBairro.Replace(",", "\" \"") + "\")";


                    //Campo Solr -> Des_Logradouro                    
                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoLogradouro))
                        urlCVsSolr += "&fq=Des_Logradouro%3A\"" + objPesquisaCurriculo.DescricaoLogradouro + "\"~2";

                    //Campo Solr -> Num_CEP                                        
                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroCEPMin) && !String.IsNullOrEmpty(objPesquisaCurriculo.NumeroCEPMax))
                        urlCVsSolr += "&fq=Num_CEP%3A%5B" + objPesquisaCurriculo.NumeroCEPMin + "+TO+" + objPesquisaCurriculo.NumeroCEPMax + "%5D&";
                    else
                    {
                        if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroCEPMin))
                            urlCVsSolr += "&fq=Num_CEP%3A%5B" + objPesquisaCurriculo.NumeroCEPMin + "+TO+*%5D";

                        if (!String.IsNullOrEmpty(objPesquisaCurriculo.NumeroCEPMax))
                            urlCVsSolr += "&fq=Num_CEP%3A%5B*+TO+" + objPesquisaCurriculo.NumeroCEPMax + "%5D";
                    }

                    //Campo Solr -> Des_Curso
                    if (objPesquisaCurriculo.CursoTecnicoGraduacao != null || !String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao))
                    {
                        object valueCurso;
                        if (objPesquisaCurriculo.CursoTecnicoGraduacao != null)
                        {
                            objPesquisaCurriculo.CursoTecnicoGraduacao.CompleteObject();
                            valueCurso = objPesquisaCurriculo.CursoTecnicoGraduacao.DescricaoCurso;

                        }
                        else
                            valueCurso = objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao;

                        urlCVsSolr += "&fq=Des_Curso%3A" + valueCurso;
                    }

                    //Campo Solr -> Des_Fonte
                    if (objPesquisaCurriculo.FonteTecnicoGraduacao != null)
                    {
                        objPesquisaCurriculo.FonteTecnicoGraduacao.CompleteObject();

                        urlCVsSolr += "&fq=Idf_Fonte%3A" + objPesquisaCurriculo.FonteTecnicoGraduacao.IdFonte;
                    }

                    //Campo Solr -> Des_Curso
                    if (objPesquisaCurriculo.CursoOutrosCursos != null || !String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoOutrosCursos))
                    {
                        object valueOutrosCursos;

                        if (objPesquisaCurriculo.CursoOutrosCursos != null)
                        {
                            objPesquisaCurriculo.CursoOutrosCursos.CompleteObject();
                            valueOutrosCursos = objPesquisaCurriculo.CursoOutrosCursos.DescricaoCurso;
                        }
                        else
                            valueOutrosCursos = objPesquisaCurriculo.DescricaoCursoOutrosCursos;

                        urlCVsSolr += "&fq=Des_Curso%3A" + valueOutrosCursos;
                    }

                    //Campo Solr -> Des_Fonte                    
                    if (objPesquisaCurriculo.FonteOutrosCursos != null)
                    {
                        objPesquisaCurriculo.FonteOutrosCursos.CompleteObject();
                        urlCVsSolr += "&fq=Des_Fonte%3A" + objPesquisaCurriculo.FonteOutrosCursos.NomeFonte;
                    }

                    //Campo Solr -> Des_Atividade
                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoExperiencia))
                        urlCVsSolr += "&fq=Des_Atividade%3A" + objPesquisaCurriculo.DescricaoExperiencia;

                    //Campo Solr -> Raz_Social                    
                    if (!String.IsNullOrEmpty(objPesquisaCurriculo.RazaoSocial))
                        urlCVsSolr += "&fq=Raz_Social%3A\"" + objPesquisaCurriculo.RazaoSocial + "\"~2";

                    //Campo Solr -> Idf_Area_BNE
                    if (objPesquisaCurriculo.AreaBNE != null)
                        urlCVsSolr += "&fq=Idf_Area_BNE%3A" + objPesquisaCurriculo.AreaBNE.IdAreaBNE;

                    //Campo Solr -> Idf_Categoria_Habilitacao
                    if (objPesquisaCurriculo.CategoriaHabilitacao != null)
                    {
                        switch (objPesquisaCurriculo.CategoriaHabilitacao.IdCategoriaHabilitacao)
                        {
                            case 1:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A(1+6+7+8+9)";
                                break;
                            case 2:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A[2 TO 9]";
                                break;
                            case 3:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A(3+4+5+7+8+9)";
                                break;
                            case 4:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A(4+5+8+9)";
                                break;
                            case 5:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A(5+9)";
                                break;
                            case 6:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A[6 TO 9]";
                                break;
                            case 7:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A[7 TO 9]";
                                break;
                            case 8:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A[8 TO 9]";
                                break;
                            default:
                                urlCVsSolr += "&fq=Idf_Categoria_Habilitacao%3A" + objPesquisaCurriculo.CategoriaHabilitacao.IdCategoriaHabilitacao;
                                break;
                        }
                    }

                    //Campo Solr -> Idf_Tipo_Veiculo
                    if (objPesquisaCurriculo.TipoVeiculo != null)
                        urlCVsSolr += "&fq=Idf_Tipo_Veiculo%3A" + objPesquisaCurriculo.TipoVeiculo.IdTipoVeiculo;

                    //Campo Solr -> Idf_Deficiencia
                    if (objPesquisaCurriculo.Deficiencia != null)
                        urlCVsSolr += "&fq=Idf_Deficiencia%3A" + objPesquisaCurriculo.Deficiencia.IdDeficiencia;

                    //Campo Solr -> Idf_Raca
                    if (objPesquisaCurriculo.Raca != null)
                        urlCVsSolr += "&fq=Idf_Raca%3A" + objPesquisaCurriculo.Raca.IdRaca;

                    //Campo Solr -> Flg_Filhos
                    if (objPesquisaCurriculo.FlagFilhos != null)
                        urlCVsSolr += "&fq=Flg_Filhos%3A" + objPesquisaCurriculo.FlagFilhos;


                    if (!string.IsNullOrWhiteSpace(objPesquisaCurriculo.IdEscolaridadeWebStagio))
                        urlCVsSolr += "&fq=Idf_Escolaridade%3A(" + objPesquisaCurriculo.IdEscolaridadeWebStagio.Replace(",", " ") + ")";

                    //Campo Solr -> Flg_Filhos
                    if (objPesquisaCurriculo.FlagAprendiz != null)
                        urlCVsSolr += "&fq=Num_Idade%3A%5B14+TO+24%5D&fq=Idf_Escolaridade%3A(5+6+8)";

                }
                #endregion

            }

            //Sort

            urlCVsSolr += "&sort=termfreq(Idf_Origem," + idOrigem.ToString() + ")+desc";

            if (objPesquisaCurriculo != null && objPesquisaCurriculo.Funcao != null)
            {
                urlCVsSolr += "%2Ctermfreq(Idf_Funcoes,"
                + objPesquisaCurriculo.Funcao.IdFuncao + ")+desc";
            }
            if (objPesquisaCurriculo != null && objPesquisaCurriculo.Cidade != null)
            {
                urlCVsSolr += "%2Ctermfreq(Idf_Cidade,"
                + objPesquisaCurriculo.Cidade.IdCidade + ")+desc";
            }
            /*if (objPesquisaCurriculo != null && objPesquisaCurriculo.Funcao != null)
            {
                urlCVsSolr += "%2Ctermfreq(Idfs_Funcoes,"
                + objPesquisaCurriculo.Funcao.IdFuncao + ")+desc";
            }
            if (objPesquisaCurriculo != null && objPesquisaCurriculo.Cidade != null)
            {
                urlCVsSolr += "%2Ctermfreq(Idfs_Cidades,"
                + objPesquisaCurriculo.Cidade.IdCidade + ")+desc";
            }*/

            urlCVsSolr += "%2CFlg_VIP+desc%2CDta_Atualizacao+desc";
            urlCVsSolr = urlCVsSolr.Replace("/browse", "/browse" + qSolr);
            //Efetuando busca Solr
            ResultadoBuscaCVSolr resultado = EfetuarRequisicao(urlCVsSolr);

            var tableCvsSolr = new DataTable();
            tableCvsSolr.Columns.Add("Idf_Curriculo", typeof(int));
            tableCvsSolr.Columns.Add("Nme_Pessoa", typeof(string));
            tableCvsSolr.Columns.Add("Des_Genero", typeof(string));
            tableCvsSolr.Columns.Add("Des_Estado_Civil", typeof(string));
            tableCvsSolr.Columns.Add("Num_Idade", typeof(int));
            tableCvsSolr.Columns.Add("Idf_Escolaridade", typeof(int));
            tableCvsSolr.Columns.Add("Sig_Escolaridade", typeof(string));
            tableCvsSolr.Columns.Add("Vlr_Pretensao_Salarial", typeof(Decimal));
            tableCvsSolr.Columns.Add("Des_Bairro", typeof(string));
            tableCvsSolr.Columns.Add("Nme_Cidade", typeof(string));
            tableCvsSolr.Columns.Add("Des_Funcao", typeof(string));
            tableCvsSolr.Columns.Add("Des_Experiencia", typeof(string));
            tableCvsSolr.Columns.Add("Des_Categoria_Habilitacao", typeof(string));
            tableCvsSolr.Columns.Add("Des_Atualizacao", typeof(string));
            tableCvsSolr.Columns.Add("Vip", typeof(int));
            tableCvsSolr.Columns.Add("Valor", typeof(float));
            tableCvsSolr.Columns.Add("Mensagem", typeof(string));
            tableCvsSolr.Columns.Add("Avaliacao", typeof(int));
            tableCvsSolr.Columns.Add("Des_avaliacao", typeof(string));
            tableCvsSolr.Columns.Add("Dentro_Perfil", typeof(int));
            tableCvsSolr.Columns.Add("Pertence_origem", typeof(int));
            tableCvsSolr.Columns.Add("Dta_Atualizacao", typeof(DateTime));
            tableCvsSolr.Columns.Add("Sig_Estado", typeof(string));
            tableCvsSolr.Columns.Add("Num_CPF", typeof(string));
            tableCvsSolr.Columns.Add("Vlr_Ultimo_Salario", typeof(Decimal));
            tableCvsSolr.Columns.Add("Idf_Grau_Escolaridade_Formacao", typeof(int));
            tableCvsSolr.Columns.Add("Idf_Escolaridade_Formacao", typeof(int));
            tableCvsSolr.Columns.Add("Des_Grau_Escolaridade_Formacao", typeof(string));
            tableCvsSolr.Columns.Add("Des_Curso_Formacao", typeof(string));
            tableCvsSolr.Columns.Add("Des_Fonte_Formacao", typeof(string));
            tableCvsSolr.Columns.Add("Dta_Conclusao_Formacao", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Raz_Social_1", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Admissao_1", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Demissao_1", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Des_Funcao_1", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Atividade_1", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Area_BNE_1", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Raz_Social_2", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Admissao_2", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Demissao_2", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Des_Funcao_2", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Atividade_2", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Area_BNE_2", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Raz_Social_3", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Admissao_3", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Demissao_3", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Des_Funcao_3", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Atividade_3", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Area_BNE_3", typeof(string));
            tableCvsSolr.Columns.Add("Idf_Deficiencia", typeof(int));
            tableCvsSolr.Columns.Add("Nme_Anexo", typeof(string));
            tableCvsSolr.Columns.Add("Des_SMS", typeof(string));

            ResultadoBuscaCVSolr resultadoCvClassificacao = null;
            ResultadoBuscaCVSolr resultadoCvSms = null;
            //Busca Observacao da Filial
            if (idFilial.HasValue)
            {
                //Prepara URL Solr para buscar classificacao 
                urlCVsClassificacaoSolr += "&fq=Idf_Curriculo%3A(";
                urlCVsClassificacaoSolr += String.Join("+", resultado.response.docs.Select(item => item.Idf_Curriculo.ToString(CultureInfo.CurrentCulture)).ToArray());
                urlCVsClassificacaoSolr += ")";

                urlCVsSms += "&fq=Idf_Curriculo%3A(";
                urlCVsSms += String.Join("+", resultado.response.docs.Select(item => item.Idf_Curriculo.ToString(CultureInfo.CurrentCulture)).ToArray());
                urlCVsSms += ")";

                //Efetuando busca Solr cvclassificacao
                resultadoCvClassificacao = EfetuarRequisicao(urlCVsClassificacaoSolr);
                resultadoCvSms = EfetuarRequisicao(urlCVsSms);
            }

            foreach (CvsSolr document in resultado.response.docs)
            {
                int? idfAvaliacao = null;
                string desAvaliacao = string.Empty;
                string desFuncoes = string.Empty;
                string descricaoSMS = string.Empty;
                string estadoCivil = string.Empty;

                int pertenceOrigem = 0;

                //Ajusta campo funcao
                if (document.Des_Funcao != null)
                    desFuncoes = document.Des_Funcao.Aggregate(desFuncoes, (current, funcoes) => current + (funcoes + ";"));

                //Se filial preenchida verifica se tem classificacao
                if (idFilial.HasValue && resultadoCvClassificacao != null)
                {
                    var classificacao = resultadoCvClassificacao.response.docs.OrderByDescending(c => c.Dta_Cadastro).FirstOrDefault(c => c.Idf_Curriculo == document.Idf_Curriculo);
                    if (classificacao != null)
                    {
                        idfAvaliacao = classificacao.Idf_Avaliacao;
                        desAvaliacao = string.IsNullOrWhiteSpace(classificacao.Des_Observacao) ? string.Empty : classificacao.Des_Observacao.Trim();
                    }
                }
                if (idFilial.HasValue && resultadoCvSms != null)
                {
                    var sms = resultadoCvSms.response.docs.Where(c => c.Idf_Curriculo == document.Idf_Curriculo).OrderByDescending(o => o.Dta_Cadastro).Take(3);
                    if (sms != null)
                        descricaoSMS = sms.Aggregate(descricaoSMS, (current, mensagemSMS) => current + string.Format("{0} enviado em {1}<br>", mensagemSMS.Des_Mensagem, mensagemSMS.Dta_Cadastro.AddHours(-3)));
                }

                if (document.Idfs_Origens != null)
                {
                    if (document.Idfs_Origens.Any(origens => idOrigem == origens))
                        pertenceOrigem = 1;
                }

                if (document.Des_Estado_Civil != null)
                {
                    estadoCivil = document.Des_Estado_Civil;
                    estadoCivil = estadoCivil.Substring(0, 1);
                }

                #region Tratamento Maior Formacao

                int idMaiorEscolaridade;
                string descricaoMaiorEscolaridade, descricaoGrauEscolaridadeFormacao, descricaoCursoFormacao, descricaoFonteFormacao, dataConclusaoFormacao;
                int? identificadorGrauEscolaridadeFormacao, identificadorEscolaridadeFormacao;

                document.RecuperarMaiorFormacao(out idMaiorEscolaridade, out descricaoMaiorEscolaridade, out identificadorGrauEscolaridadeFormacao, out identificadorEscolaridadeFormacao, out descricaoGrauEscolaridadeFormacao, out descricaoCursoFormacao, out descricaoFonteFormacao, out dataConclusaoFormacao);
                #endregion

                #region Tratamento Experiencia

                string razaoSocial1, descricaoFuncao1, descricaoAtividade1, descricaoAtividadeEmpresa1, razaoSocial2, descricaoFuncao2, descricaoAtividade2, descricaoAtividadeEmpresa2, razaoSocial3, descricaoFuncao3, descricaoAtividade3, descricaoAtividadeEmpresa3;
                razaoSocial1 = descricaoFuncao1 = descricaoAtividade1 = descricaoAtividadeEmpresa1 = razaoSocial2 = descricaoFuncao2 = descricaoAtividade2 = descricaoAtividadeEmpresa2 = razaoSocial3 = descricaoFuncao3 = descricaoAtividade3 = descricaoAtividadeEmpresa3 = string.Empty;
                DateTime dataAdmissao1, dataAdmissao2, dataAdmissao3;
                dataAdmissao1 = dataAdmissao2 = dataAdmissao3 = new DateTime();
                DateTime? dataDemissao1, dataDemissao2, dataDemissao3;
                dataDemissao1 = dataDemissao2 = dataDemissao3 = null;

                if (document.Raz_Social != null)
                {
                    if (document.Raz_Social.Count >= 1)
                    {
                        razaoSocial1 = document.Raz_Social[0];
                        dataAdmissao1 = document.Dta_Admissao[0];

                        if (document.Dta_Demissao[0] != null && document.Dta_Demissao[0].ToShortDateString() != "01/01/1900")
                            dataDemissao1 = Convert.ToDateTime(document.Dta_Demissao[0]);

                        descricaoFuncao1 = document.Des_Funcao_Exercida[0];
                        descricaoAtividade1 = document.Des_Atividade[0];
                        descricaoAtividadeEmpresa1 = document.Des_Atividade_empresa[0];
                    }
                    if (document.Raz_Social.Count >= 2)
                    {
                        razaoSocial2 = document.Raz_Social[1];
                        dataAdmissao2 = document.Dta_Admissao[1];

                        if (document.Dta_Demissao[1] != null && document.Dta_Demissao[1].ToShortDateString() != "01/01/1900")
                            dataDemissao2 = Convert.ToDateTime(document.Dta_Demissao[1]);

                        descricaoFuncao2 = document.Des_Funcao_Exercida[1];
                        descricaoAtividade2 = document.Des_Atividade[1];
                        descricaoAtividadeEmpresa2 = document.Des_Atividade_empresa[1];
                    }
                    if (document.Raz_Social.Count >= 3)
                    {
                        razaoSocial3 = document.Raz_Social[2];
                        dataAdmissao3 = document.Dta_Admissao[2];

                        if (document.Dta_Demissao[2] != null && document.Dta_Demissao[2].ToShortDateString() != "01/01/1900")
                            dataDemissao3 = Convert.ToDateTime(document.Dta_Demissao[2]);

                        descricaoFuncao3 = document.Des_Funcao_Exercida[2];
                        descricaoAtividade3 = document.Des_Atividade[2];
                        descricaoAtividadeEmpresa3 = document.Des_Atividade_empresa[2];
                    }
                }
                #endregion

                tableCvsSolr.Rows.Add(
                            document.Idf_Curriculo,
                            idFilial.HasValue ? document.Nme_Pessoa : PessoaFisica.RetornarPrimeiroNome(document.Nme_Pessoa),
                            document.Sig_Sexo,
                            estadoCivil,
                            Helper.CalcularIdade(document.Dta_Nascimento),
                            idMaiorEscolaridade,
                            descricaoMaiorEscolaridade,
                            document.Vlr_Pretensao_Salarial,
                            document.Des_Bairro,
                            document.Nme_Cidade,
                            desFuncoes,
                            document.Total_Experiencia + " m",
                            document.Des_Categoria_Habilitacao,
                            0, //Des_atualizacao ver se campo é utilizado
                            document.Flg_VIP ? 1 : 0,
                            0,//score
                            " ",//string msg
                            idfAvaliacao,//int Avaliacao
                            desAvaliacao,//string Des_avaliacao
                            1,//int dentro do perfil
                            pertenceOrigem,//int pertence origem
                            document.Dta_Atualizacao,
                            document.Sig_Estado,
                            document.Num_CPF,
                            document.Vlr_Ultimo_Salario,
                            identificadorGrauEscolaridadeFormacao,
                            identificadorEscolaridadeFormacao,
                            descricaoGrauEscolaridadeFormacao,
                            descricaoCursoFormacao,
                            descricaoFonteFormacao,
                            dataConclusaoFormacao,
                            razaoSocial1,
                            dataAdmissao1,
                            dataDemissao1,
                            descricaoFuncao1,
                            descricaoAtividade1,
                            descricaoAtividadeEmpresa1,
                            razaoSocial2,
                            dataAdmissao2,
                            dataDemissao2,
                            descricaoFuncao2,
                            descricaoAtividade2,
                            descricaoAtividadeEmpresa2,
                            razaoSocial3,
                            dataAdmissao3,
                            dataDemissao3,
                            descricaoFuncao3,
                            descricaoAtividade3,
                            descricaoAtividadeEmpresa3,
                            document.Idf_Deficiencia,
                            document.Nme_Anexo,
                            descricaoSMS
                            );
            }

            //Numero de registros
            numeroRegistros = resultado.response.numFound;

            //RecuperarMedia
            if (objPesquisaCurriculo != null && objPesquisaCurriculo.Funcao != null)
            {
                PesquisaSalarial objPesquisaSalarial = new PesquisaSalarial();
                PesquisaSalarial.RecuperarPesquisaSalarial(objPesquisaCurriculo.Funcao.IdFuncao, out objPesquisaSalarial);
                mediaSalarial = objPesquisaSalarial.ValorMedia;
            }
            else
            {
                mediaSalarial = 0;
            }


            // Media dos cvs com perfil            
            //mediaSalarial = resultado.stats.stats_fields.Vlr_Pretensao_Salarial.mean;
            // Media da paginacao
            //mediaSalarial = resultado.response.docs.Sum(cvs => cvs.Vlr_Pretensao_Salarial) / resultado.response.docs.Count;

            return tableCvsSolr;
        }
        #endregion

        #region EfetuarRequisicao
        public static ResultadoBuscaCVSolr EfetuarRequisicao(String url)
        {
            Stream dataStream = null;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = true;
                ResultadoBuscaCVSolr objRetorno = null;
                // Get the response.
                var response = request.GetResponse();
                dataStream = response.GetResponseStream();

                if (dataStream != null)
                {
                    var reader = new StreamReader(dataStream);

                    objRetorno = (ResultadoBuscaCVSolr)new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaCVSolr));
                }
                return objRetorno;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, url);
            }
            finally
            {
                if (dataStream != null)
                    dataStream.Dispose();
            }
            return null;
        }
        #endregion

        #region BuscaCurriculoQuePertenceAOrigem
        public static DataTable BuscaCurriculoQuePertenceAOrigem(int tamanhoPagina, int paginaAtual, int idOrigem, int idFilial, int? idUsuarioFilialPerfil, List<int> listIdFuncoes, string descricaoCurso, out int numeroRegistros)
        {
            Object valueFuncoes = DBNull.Value;
            Object valueCurso = DBNull.Value;

            if (listIdFuncoes != null && listIdFuncoes.Count > 0)
                valueFuncoes = String.Join(",", listIdFuncoes.Select(item => item.ToString(CultureInfo.CurrentCulture)).ToArray());

            if (!string.IsNullOrEmpty(descricaoCurso))
                valueCurso = descricaoCurso;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output },
                    new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = idOrigem } ,
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial } ,
                    new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                    new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                    new SqlParameter { ParameterName = "@Funcoes", SqlDbType = SqlDbType.VarChar, Size = 2000, Value = valueFuncoes } ,
                    new SqlParameter { ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 100, Value = valueCurso }
                };

            if (idUsuarioFilialPerfil.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil });

            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "SP_BUSCA_CURRICULO_PERTENCE_ORIGEM", parms).Tables[0];
            numeroRegistros = Convert.ToInt32(parms[0].Value);

            return dt;
        }
        #endregion

        #region AtualizarQuantidadeRegistros
        /// <summary>
        /// Se a quantidade mínima de currículos não foi atingida pela busca, envia um e-mail com a busca realizada.
        /// </summary>
        /// <param name="numeroRegistros"></param>
        private void AtualizarQuantidadeRegistros(int numeroRegistros)
        {
            try
            {
                //Iniciando uma thread para preencher a localização do endereço
                var thread = new Thread(AtualizarNumeroRetornos);
                thread.Start(numeroRegistros);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region AtualizarNumeroRetornos
        private void AtualizarNumeroRetornos(object quantidadeCurriculoEncontrado)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPesquisaCurriculo } ,
					new SqlParameter { ParameterName = "@Qtd_Curriculo_Retorno", SqlDbType = SqlDbType.Int, Size = 4, Value = quantidadeCurriculoEncontrado }
				};

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spatualizarquantidadecurriculosretornados, parms);
        }
        #endregion

        #region RecuperarCurriculosNaoVisualisadosVaga
        /// <summary>
        /// Método utilizado na tela de Pesquisa de Curriculo, retorna todos os currículos relacionados a um rastreador de CV
        /// </summary>
        /// <param name="paginaAtual"> </param>
        /// <param name="idFilial"></param>
        /// <param name="idVaga"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <param name="tamanhoPagina"> </param>
        /// <returns></returns>
        public static DataTable RecuperarCurriculosNaoVisualisadosVaga(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, out int numeroRegistros, out decimal mediaSalarial)
        {
            //Configurando parametros basicos da pesquisa
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                                new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                                new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output} ,
                                new SqlParameter { ParameterName = "@MediaSalarial", SqlDbType = SqlDbType.Decimal, Value = 0, Direction = ParameterDirection.Output } ,
                                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                            };

            if (idUsuarioFilialPerfil.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idUsuarioFilialPerfil });

            var dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_NAO_VISUALIZADO", parms).Tables[0];

            numeroRegistros = Convert.ToInt32(parms[2].Value);

            mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            return dt;
        }
        #endregion

        #region RecuperarCurriculosRelacionadosVaga
        /// <summary>
        /// Método utilizado na tela de Pesquisa de Curriculo, retorna todos os currículos relacionados a um rastreador de CV
        /// </summary>
        /// <param name="paginaAtual"> </param>
        /// <param name="idFilial"></param>
        /// <param name="idVaga"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <param name="tamanhoPagina"> </param>
        /// <returns></returns>
        public static DataTable RecuperarCurriculosRelacionadosVaga(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, out int numeroRegistros, out decimal mediaSalarial)
        {
            //Configurando parametros basicos da pesquisa
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                                new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                                new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output} ,
                                new SqlParameter { ParameterName = "@MediaSalarial", SqlDbType = SqlDbType.Decimal, Value = 0, Direction = ParameterDirection.Output } ,
                                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                            };

            if (idUsuarioFilialPerfil.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idUsuarioFilialPerfil });

            var dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_VISUALIZADO", parms).Tables[0];

            numeroRegistros = Convert.ToInt32(parms[2].Value);

            mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            return dt;
        }
        #endregion

        #region RecuperarCurriculosNoPerfilVaga
        /// <summary>
        /// Método utilizado na tela de Pesquisa de Curriculo, retorna todos os currículos relacionados a um rastreador de CV
        /// </summary>
        /// <param name="paginaAtual"> </param>
        /// <param name="idFilial"></param>
        /// <param name="idVaga"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <param name="tamanhoPagina"> </param>
        /// <returns></returns>
        public static DataTable RecuperarCurriculosNoPerfilVaga(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, out int numeroRegistros, out decimal mediaSalarial)
        {
            //Configurando parametros basicos da pesquisa
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                                new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                                new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output} ,
                                new SqlParameter { ParameterName = "@MediaSalarial", SqlDbType = SqlDbType.Decimal, Value = 0, Direction = ParameterDirection.Output } ,
                                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                            };

            if (idUsuarioFilialPerfil.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idUsuarioFilialPerfil });

            var dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_NO_PERFIL", parms).Tables[0];

            numeroRegistros = Convert.ToInt32(parms[2].Value);

            mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            return dt;
        }
        #endregion

        #region BuscaCurriculoRastreador
        /// <summary>
        /// Método que realiza uma busca de currículos de acordo com os parametros do Rastreador de Curriuclos.
        /// </summary>
        /// <param name="objRastreador"></param>
        /// <returns></returns>
        public static List<int> BuscaCurriculoRastreador(Rastreador objRastreador)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = objRastreador.Origem.IdOrigem } ,
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objRastreador.Filial.IdFilial }
                };

            //Configurando parâmentros adicionais
            if (objRastreador.Funcao != null)
            {
                var sqlParam = new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4) { Value = objRastreador.Funcao.IdFuncao };
                parms.Add(sqlParam);
            }

            if (objRastreador.Cidade != null)
            {
                var sqlParam = new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4) { Value = objRastreador.Cidade.IdCidade };
                parms.Add(sqlParam);
            }

            if (objRastreador.Estado != null)
            {
                var sqlParam = new SqlParameter("@Sig_Estado", SqlDbType.Char, 2) { Value = objRastreador.Estado.SiglaEstado };
                parms.Add(sqlParam);
            }

            if (!String.IsNullOrEmpty(objRastreador.DescricaoPalavraChave))
            {
                var sqlParam = new SqlParameter("@Des_Metabusca", SqlDbType.VarChar, 500) { Value = objRastreador.DescricaoPalavraChave };
                parms.Add(sqlParam);
            }

            if (objRastreador.ValorSalarioInicio.HasValue)
            {
                var sqlParam = new SqlParameter("@Salario_Min", SqlDbType.Decimal, 11) { Value = objRastreador.ValorSalarioInicio.Value };
                parms.Add(sqlParam);
            }

            if (objRastreador.ValorSalarioFim.HasValue)
            {
                var sqlParam = new SqlParameter("@Salario_Max", SqlDbType.Decimal, 11) { Value = objRastreador.ValorSalarioFim.Value };
                parms.Add(sqlParam);
            }

            if (objRastreador.QuantidadeExperiencia.HasValue)
            {
                var sqlParam = new SqlParameter("@Meses_Exp", SqlDbType.SmallInt) { Value = objRastreador.QuantidadeExperiencia.Value };
                parms.Add(sqlParam);
            }

            string idfsIdioma = RastreadorIdioma.ListarIdentificadoresConcatenadosPorRastreador(objRastreador);
            if (!String.IsNullOrEmpty(idfsIdioma))
            {
                var sqlParamIdioma = new SqlParameter("@Idfs_Idioma", SqlDbType.VarChar, 100) { Value = idfsIdioma };
                parms.Add(sqlParamIdioma);
            }

            string idfsDisponibilidade = RastreadorDisponibilidade.ListarIdentificadoresConcatenadosPorRastreador(objRastreador);
            if (!String.IsNullOrEmpty(idfsDisponibilidade))
            {
                var sqlParamDisponibilidade = new SqlParameter("@Idfs_Disponibilidade", SqlDbType.VarChar, 100) { Value = idfsDisponibilidade };
                parms.Add(sqlParamDisponibilidade);
            }

            if (objRastreador.Escolaridade != null)
            {
                objRastreador.Escolaridade.CompleteObject();
                if (objRastreador.Escolaridade.SequenciaPeso.HasValue)
                {
                    var sqlParam = new SqlParameter("@Peso_Escolaridade", SqlDbType.SmallInt) { Value = objRastreador.Escolaridade.SequenciaPeso.Value };
                    parms.Add(sqlParam);
                }
            }

            if (objRastreador.DescricaoIdadeFim.HasValue)
            {
                var sqlParam = new SqlParameter("@Idade_Max", SqlDbType.SmallInt) { Value = objRastreador.DescricaoIdadeFim.Value };
                parms.Add(sqlParam);
            }

            if (objRastreador.DescricaoIdadeInicio.HasValue)
            {
                var sqlParam = new SqlParameter("@Idade_Min", SqlDbType.SmallInt) { Value = objRastreador.DescricaoIdadeInicio.Value };
                parms.Add(sqlParam);
            }

            if (objRastreador.Sexo != null)
            {
                var sqlParam = new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4) { Value = objRastreador.Sexo.IdSexo };
                parms.Add(sqlParam);
            }

            if (objRastreador.EstadoCivil != null)
            {
                var sqlParam = new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4) { Value = objRastreador.EstadoCivil.IdEstadoCivil };
                parms.Add(sqlParam);
            }

            if (!String.IsNullOrEmpty(objRastreador.DescricaoBairro))
            {
                var sqlParam = new SqlParameter("@Des_Bairro", SqlDbType.VarChar, 80) { Value = objRastreador.DescricaoBairro };
                parms.Add(sqlParam);
            }

            if (!String.IsNullOrEmpty(objRastreador.DescricaoCEPInicio))
            {
                var sqlParam = new SqlParameter("@Num_CEP_Min", SqlDbType.Char, 8) { Value = objRastreador.DescricaoCEPInicio };
                parms.Add(sqlParam);
            }

            if (!String.IsNullOrEmpty(objRastreador.DescricaoCEPFim))
            {
                var sqlParam = new SqlParameter("@Num_CEP_Max", SqlDbType.Char, 8) { Value = objRastreador.DescricaoCEPFim };
                parms.Add(sqlParam);
            }

            if (objRastreador.Curso != null)
            {
                objRastreador.Curso.CompleteObject();
                var sqlParam = new SqlParameter("@Des_Curso", SqlDbType.VarChar, 100) { Value = objRastreador.Curso.DescricaoCurso };
                parms.Add(sqlParam);
            }

            if (objRastreador.Fonte != null)
            {
                objRastreador.Fonte.CompleteObject();
                var sqlParam = new SqlParameter("@Nme_Fonte", SqlDbType.VarChar, 100) { Value = objRastreador.Fonte.NomeFonte };
                parms.Add(sqlParam);
            }

            if (objRastreador.CursoOutrosCursos != null)
            {
                objRastreador.CursoOutrosCursos.CompleteObject();
                var sqlParam = new SqlParameter("@Des_Curso_Outros", SqlDbType.VarChar, 100) { Value = objRastreador.CursoOutrosCursos.DescricaoCurso };
                parms.Add(sqlParam);
            }

            if (objRastreador.FonteOutrosCursos != null)
            {
                objRastreador.FonteOutrosCursos.CompleteObject();
                var sqlParam = new SqlParameter("@Nme_Fonte_Outros", SqlDbType.VarChar, 100) { Value = objRastreador.FonteOutrosCursos.NomeFonte };
                parms.Add(sqlParam);
            }

            if (!String.IsNullOrEmpty(objRastreador.RazaoSocial))
            {
                var sqlParam = new SqlParameter("@Nme_Empresa", SqlDbType.VarChar, 100) { Value = objRastreador.RazaoSocial };
                parms.Add(sqlParam);
            }

            if (objRastreador.AreaBNE != null)
            {
                var sqlParam = new SqlParameter("@Idf_Area_Bne", SqlDbType.Int, 4) { Value = objRastreador.AreaBNE.IdAreaBNE };
                parms.Add(sqlParam);
            }

            if (objRastreador.CategoriaHabilitacao != null)
            {
                var sqlParam = new SqlParameter("@Idf_Categoria_Habilitacao", SqlDbType.Int, 4) { Value = objRastreador.CategoriaHabilitacao.IdCategoriaHabilitacao };
                parms.Add(sqlParam);
            }

            if (objRastreador.TipoVeiculo != null)
            {
                var sqlParam = new SqlParameter("@Idf_Tipo_Veiculo", SqlDbType.Int, 4) { Value = objRastreador.TipoVeiculo.IdTipoVeiculo };
                parms.Add(sqlParam);
            }

            if (objRastreador.Deficiencia != null)
            {
                var sqlParam = new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4) { Value = objRastreador.Deficiencia.IdDeficiencia };
                parms.Add(sqlParam);
            }

            if (objRastreador.Raca != null)
            {
                var sqlParam = new SqlParameter("@Idf_Raca", SqlDbType.Int, 4) { Value = objRastreador.Raca.IdRaca };
                parms.Add(sqlParam);
            }

            if (objRastreador.FlagFilhos != null)
            {
                var sqlParam = new SqlParameter("@Flg_Filhos", SqlDbType.Bit, 1) { Value = objRastreador.FlagFilhos.Value };
                parms.Add(sqlParam);
            }

            List<int> idfsCurriculo;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BUSCA_CURRICULO_RASTREADOR_IDFS", parms))
            {
                idfsCurriculo = new List<int>();

                while (dr.Read())
                    idfsCurriculo.Add(Convert.ToInt32(dr["Idf_Curriculo"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return idfsCurriculo;
        }
        #endregion

        #region Salvar
        public void Salvar(List<PesquisaCurriculoIdioma> listPesquisaCurriculoIdioma, List<PesquisaCurriculoDisponibilidade> listPesquisaCurriculoDisponibilidade, bool queroContratarEstagiarios = false, bool queroContratarAprendiz = false)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (queroContratarEstagiarios)
                        {
                            //this.IdEscolaridadeWebStagio = ((int)Enumeradores.Escolaridade.EnsinoMedioIncompleto).ToString(CultureInfo.InvariantCulture) + "," +
                            //    ((int)Enumeradores.Escolaridade.TecnicoPosMedioIncompleto).ToString(CultureInfo.InvariantCulture) + "," +
                            //    ((int)Enumeradores.Escolaridade.TecnologoIncompleto).ToString(CultureInfo.InvariantCulture) + "," +
                            //    ((int)Enumeradores.Escolaridade.SuperiorIncompleto).ToString(CultureInfo.InvariantCulture);

                            this.IdEscolaridadeWebStagio = Parametro.RecuperaValorParametro(
                                Enumeradores.Parametro.IdfsEscolaridadeWebEstagiosQueroEstagiario, trans);
                        }

                        if (queroContratarAprendiz)
                            this.FlagAprendiz = true;

                        Save(trans);

                        foreach (PesquisaCurriculoIdioma objPesquisaCurriculoIdioma in listPesquisaCurriculoIdioma)
                        {
                            objPesquisaCurriculoIdioma.PesquisaCurriculo = this;
                            objPesquisaCurriculoIdioma.Save(trans);
                        }
                        foreach (PesquisaCurriculoDisponibilidade objPesquisaCurriculoDisponibilidade in listPesquisaCurriculoDisponibilidade)
                        {
                            objPesquisaCurriculoDisponibilidade.PesquisaCurriculo = this;
                            objPesquisaCurriculoDisponibilidade.Save(trans);
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region [ RecuperaUltimaPesquisa ]

        public static bool RecuperarUltimaPesquisa(int usuarioFilialPerfil, out PesquisaCurriculo pesquisaCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = usuarioFilialPerfil }
                };

            const string spSelectUltimaPesquisa = @"SELECT TOP 1 * FROM BNE.TAB_Pesquisa_Curriculo AS pesq
              WHERE  pesq.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
              ORDER BY Idf_Pesquisa_Curriculo DESC";

            using (IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, spSelectUltimaPesquisa, parms)
                                                : DataAccessLayer.ExecuteReader(trans, CommandType.Text, spSelectUltimaPesquisa, parms))
            {
                pesquisaCurriculo = new PesquisaCurriculo();
                if (SetInstance(dr, pesquisaCurriculo))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            pesquisaCurriculo = null;
            return false;
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPesquisaCurriculo">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PesquisaCurriculo objPesquisaCurriculo)
        {
            try
            {
                if (dr.Read())
                {
                    objPesquisaCurriculo._idPesquisaCurriculo = Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]);
                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objPesquisaCurriculo._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    if (dr["Idf_Curriculo"] != DBNull.Value)
                        objPesquisaCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    if (dr["Idf_Funcao"] != DBNull.Value)
                        objPesquisaCurriculo._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
                    if (dr["Des_IP"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoIP = Convert.ToString(dr["Des_IP"]);
                    if (dr["Idf_Cidade"] != DBNull.Value)
                        objPesquisaCurriculo._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
                    if (dr["Des_Palavra_Chave"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoPalavraChave = Convert.ToString(dr["Des_Palavra_Chave"]);
                    if (dr["Sig_Estado"] != DBNull.Value)
                        objPesquisaCurriculo._estado = new Estado(dr["Sig_Estado"].ToString());
                    if (dr["Idf_Escolaridade"] != DBNull.Value)
                        objPesquisaCurriculo._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
                    if (dr["Idf_Sexo"] != DBNull.Value)
                        objPesquisaCurriculo._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
                    if (dr["Dta_Idade_Min"] != DBNull.Value)
                        objPesquisaCurriculo._dataIdadeMin = Convert.ToDateTime(dr["Dta_Idade_Min"]);
                    if (dr["Dta_Idade_Max"] != DBNull.Value)
                        objPesquisaCurriculo._dataIdadeMax = Convert.ToDateTime(dr["Dta_Idade_Max"]);
                    if (dr["Num_Salario_Min"] != DBNull.Value)
                        objPesquisaCurriculo._numeroSalarioMin = Convert.ToDecimal(dr["Num_Salario_Min"]);
                    if (dr["Num_Salario_Max"] != DBNull.Value)
                        objPesquisaCurriculo._numeroSalarioMax = Convert.ToDecimal(dr["Num_Salario_Max"]);
                    if (dr["Qtd_Experiencia"] != DBNull.Value)
                        objPesquisaCurriculo._quantidadeExperiencia = Convert.ToInt64(dr["Qtd_Experiencia"]);
                    if (dr["Idf_Idioma"] != DBNull.Value)
                        objPesquisaCurriculo._idioma = new Idioma(Convert.ToInt32(dr["Idf_Idioma"]));
                    if (dr["Des_Cod_CPF_Nome"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoCodCPFNome = Convert.ToString(dr["Des_Cod_CPF_Nome"]);
                    if (dr["Idf_Estado_Civil"] != DBNull.Value)
                        objPesquisaCurriculo._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
                    if (dr["Des_Bairro"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
                    if (dr["Des_Logradouro"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoLogradouro = Convert.ToString(dr["Des_Logradouro"]);
                    if (dr["Num_CEP_Min"] != DBNull.Value)
                        objPesquisaCurriculo._numeroCEPMin = Convert.ToString(dr["Num_CEP_Min"]);
                    if (dr["Num_CEP_Max"] != DBNull.Value)
                        objPesquisaCurriculo._numeroCEPMax = Convert.ToString(dr["Num_CEP_Max"]);
                    if (dr["Des_Experiencia"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoExperiencia = Convert.ToString(dr["Des_Experiencia"]);
                    if (dr["Idf_Curso_Tecnico_Graduacao"] != DBNull.Value)
                        objPesquisaCurriculo._cursoTecnicoGraduacao = new Curso(Convert.ToInt32(dr["Idf_Curso_Tecnico_Graduacao"]));
                    if (dr["Idf_Fonte_Tecnico_Graduacao"] != DBNull.Value)
                        objPesquisaCurriculo._fonteTecnicoGraduacao = new Fonte(Convert.ToInt32(dr["Idf_Fonte_Tecnico_Graduacao"]));
                    if (dr["Raz_Social"] != DBNull.Value)
                        objPesquisaCurriculo._razaoSocial = Convert.ToString(dr["Raz_Social"]);
                    if (dr["Idf_Area_BNE"] != DBNull.Value)
                        objPesquisaCurriculo._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
                    if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
                        objPesquisaCurriculo._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
                    if (dr["Num_DDD_Telefone"] != DBNull.Value)
                        objPesquisaCurriculo._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
                    if (dr["Num_Telefone"] != DBNull.Value)
                        objPesquisaCurriculo._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
                    if (dr["Eml_Pessoa"] != DBNull.Value)
                        objPesquisaCurriculo._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
                    if (dr["Idf_Deficiencia"] != DBNull.Value)
                        objPesquisaCurriculo._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
                    if (dr["Dta_Cadastro"] != DBNull.Value)
                        objPesquisaCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Idf_Tipo_Veiculo"] != DBNull.Value)
                        objPesquisaCurriculo._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
                    if (dr["Idf_Curso_Outros_Cursos"] != DBNull.Value)
                        objPesquisaCurriculo._cursoOutrosCursos = new Curso(Convert.ToInt32(dr["Idf_Curso_Outros_Cursos"]));
                    if (dr["Idf_Fonte_Outros_Cursos"] != DBNull.Value)
                        objPesquisaCurriculo._fonteOutrosCursos = new Fonte(Convert.ToInt32(dr["Idf_Fonte_Outros_Cursos"]));
                    if (dr["Idf_Raca"] != DBNull.Value)
                        objPesquisaCurriculo._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
                    if (dr["Flg_Filhos"] != DBNull.Value)
                        objPesquisaCurriculo._flagFilhos = Convert.ToBoolean(dr["Flg_Filhos"]);
                    objPesquisaCurriculo._flagPesquisaAvancada = Convert.ToBoolean(dr["Flg_Pesquisa_Avancada"]);
                    if (dr["Des_Curso_Tecnico_Graduacao"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoCursoTecnicoGraduacao = Convert.ToString(dr["Des_Curso_Tecnico_Graduacao"]);
                    if (dr["Des_Curso_Outros_Cursos"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoCursoOutrosCursos = Convert.ToString(dr["Des_Curso_Outros_Cursos"]);
                    if (dr["Qtd_Curriculo_Retorno"] != DBNull.Value)
                        objPesquisaCurriculo._quantidadeCurriculoRetorno = Convert.ToInt32(dr["Qtd_Curriculo_Retorno"]);
                    if (dr["Idf_Escolaridade_WebStagio"] != DBNull.Value)
                        objPesquisaCurriculo._idEscolaridadeWebStagio = Convert.ToString(dr["Idf_Escolaridade_WebStagio"]);
                    if (dr["Flg_Aprendiz"] != DBNull.Value)
                        objPesquisaCurriculo._flagAprendiz = Convert.ToBoolean(dr["Flg_Aprendiz"]);
                    if (dr["Des_Filtro_Excludente"] != DBNull.Value)
                        objPesquisaCurriculo._descricaoFiltroExcludente = Convert.ToString(dr["Des_Filtro_Excludente"]);

                    objPesquisaCurriculo._persisted = true;
                    objPesquisaCurriculo._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

    }
}