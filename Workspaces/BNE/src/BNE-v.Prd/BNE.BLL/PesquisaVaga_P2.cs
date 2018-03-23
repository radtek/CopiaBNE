//-- Data: 10/08/2010 15:09
//-- Autor: Gieyson Stelmak

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace BNE.BLL
{
    public partial class PesquisaVaga // Tabela: TAB_Pesquisa_Vaga
    {
        #region Propriedades
        public String DescricaoFuncao
        {
            get
            {
                if (this.Funcao != null && !string.IsNullOrEmpty(this.Funcao.DescricaoFuncao))
                    return this.Funcao.DescricaoFuncao;

                return String.Empty;
            }
        }
        public String DescricaoAreaBNE
        {
            get
            {
                if (this.AreaBNE != null && !string.IsNullOrEmpty(this.AreaBNE.DescricaoAreaBNE))
                    return this.AreaBNE.DescricaoAreaBNE;

                return String.Empty;
            }
        }
        public String NomeCidade
        {
            get
            {
                if (this.Cidade != null && !string.IsNullOrEmpty(this.Cidade.NomeCidade))
                    return this.Cidade.NomeCidade;

                return String.Empty;
            }
        }
        public String SiglaEstado
        {
            get
            {
                if (this.Cidade != null && this.Cidade.Estado != null && !string.IsNullOrEmpty(this.Cidade.Estado.SiglaEstado))
                    return this.Cidade.Estado.SiglaEstado;

                return String.Empty;
            }
        }
        public Int32 TotalRegistros { get; set; }
        #endregion Propriedades

        #region Consultas

        private const string Spatualizarquantidadevagasretornadas = @"
        UPDATE TAB_Pesquisa_Vaga SET Qtd_Vaga_Retorno = @Qtd_Vaga_Retorno WHERE Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga
        ";
        #endregion

        #region Salvar
        public void Salvar(List<PesquisaVagaDisponibilidade> listPesquisaVagaDisponibilidade, List<PesquisaVagaTipoVinculo> listPesquisaVagaTipoVinculo, PesquisaVagaCurso objPesquisaVagaCurso = null)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.Save(trans);

                        foreach (PesquisaVagaDisponibilidade objPesquisaVagaDisponibilidade in listPesquisaVagaDisponibilidade)
                        {
                            objPesquisaVagaDisponibilidade.PesquisaVaga = this;
                            objPesquisaVagaDisponibilidade.Save(trans);
                        }

                        foreach (PesquisaVagaTipoVinculo objPesquisaVagaTipoVinculo in listPesquisaVagaTipoVinculo)
                        {
                            objPesquisaVagaTipoVinculo.PesquisaVaga = this;
                            objPesquisaVagaTipoVinculo.Save(trans);
                        }
                        if (objPesquisaVagaCurso != null)
                        {
                            objPesquisaVagaCurso.PesquisaVaga = this;
                            objPesquisaVagaCurso.Save(trans);
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

        #region BuscaVagaFullText
        public static DataTable BuscaVagaFullText(BLL.PesquisaVaga objPesquisaVaga, int tamanhoPagina, int paginaAtual, int? idCurriculo, int? idFuncao, int? idCidade, string palavraChave, int? idOrigem, bool? empresaOfereceCursos, string sigEstado, int? idFilial, int? idFuncaoArea, OrdenacaoBuscaVaga ordenacao, out int totalRegistros, bool mostrarConfidencial = true)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@PaginaAtual", SqlDbType.Int, 4),
                    new SqlParameter("@TamanhoPagina", SqlDbType.Int, 4),
                    new SqlParameter("@TotalRegistros", SqlDbType.Int, 4),
                    new SqlParameter("@Idf_Limite_Resultado", SqlDbType.Int, 4),
                    new SqlParameter("@Idf_Origem", SqlDbType.Int, 4),
                    new SqlParameter("@Mostra_Vagas_BNE", SqlDbType.Bit, 4),
                    new SqlParameter("@Codigo_Ordenacao", SqlDbType.Int, 4)
                };

            parms[0].Value = paginaAtual;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = 0;
            parms[2].Direction = ParameterDirection.Output;

            if (idOrigem.HasValue)
                parms[4].Value = idOrigem;
            else
                parms[4].Value = DBNull.Value;

            if (empresaOfereceCursos.HasValue)
                parms[5].Value = empresaOfereceCursos;
            else
                parms[5].Value = DBNull.Value;

            parms[6].Value = (int)ordenacao;

            if (objPesquisaVaga != null)
            {
                if (objPesquisaVaga.Funcao != null || objPesquisaVaga.Cidade != null || !String.IsNullOrEmpty(objPesquisaVaga.DescricaoPalavraChave))
                    parms[3].Value = (int)Enumeradores.Parametro.LimiteResultadoPesquisa;
                else
                    parms[3].Value = (int)Enumeradores.Parametro.LimiteResultadoPesquisaSemValoresInformados;

                //Configurando parâmentros adicionais
                if (objPesquisaVaga.Funcao != null)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaVaga.Funcao.IdFuncao });

                if (objPesquisaVaga.AreaBNE != null)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Area_BNE", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaVaga.AreaBNE.IdAreaBNE });

                if (!String.IsNullOrEmpty(objPesquisaVaga.DescricaoPalavraChave))
                    parms.Add(new SqlParameter { ParameterName = "@Des_Metabusca_Rapida", SqlDbType = SqlDbType.VarChar, Size = 500, Value = objPesquisaVaga.DescricaoPalavraChave });

                if (objPesquisaVaga.Cidade != null)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaVaga.Cidade.IdCidade });

                if (objPesquisaVaga.Estado != null)
                    parms.Add(new SqlParameter { ParameterName = "@Sig_Estado", SqlDbType = SqlDbType.Char, Size = 2, Value = objPesquisaVaga.Estado.SiglaEstado });

                if (objPesquisaVaga.Escolaridade != null)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Escolaridade", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaVaga.Escolaridade.IdEscolaridade });

                if (objPesquisaVaga.NumeroSalarioMin.HasValue)
                    parms.Add(new SqlParameter { ParameterName = "@Vlr_Salario_Min", SqlDbType = SqlDbType.Decimal, Value = (decimal)objPesquisaVaga.NumeroSalarioMin });

                if (objPesquisaVaga.NumeroSalarioMax.HasValue)
                    parms.Add(new SqlParameter { ParameterName = "@Vlr_Salario_Max", SqlDbType = SqlDbType.Decimal, Value = (decimal)objPesquisaVaga.NumeroSalarioMax });

                string idfsDisponibilidade = PesquisaVagaDisponibilidade.ListarIdentificadoresConcatenadosPorPesquisa(objPesquisaVaga);
                if (!String.IsNullOrEmpty(idfsDisponibilidade))
                    parms.Add(new SqlParameter { ParameterName = "@Idfs_Disponibilidade", SqlDbType = SqlDbType.VarChar, Size = 100, Value = idfsDisponibilidade });

                string idfsTipoVinculo = PesquisaVagaTipoVinculo.ListarIdentificadoresConcatenadosPorPesquisa(objPesquisaVaga);
                if (!String.IsNullOrEmpty(idfsTipoVinculo))
                    parms.Add(new SqlParameter { ParameterName = "@Idfs_Tipo_Vinculo", SqlDbType = SqlDbType.VarChar, Size = 100, Value = idfsTipoVinculo });

                if (!String.IsNullOrEmpty(objPesquisaVaga.RazaoSocial))
                    parms.Add(new SqlParameter { ParameterName = "@Raz_Social", SqlDbType = SqlDbType.VarChar, Size = 50, Value = objPesquisaVaga.RazaoSocial });
                if (!String.IsNullOrEmpty(objPesquisaVaga.DescricaoCodVaga))
                {
                    try
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Idf_vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = Convert.ToInt32(objPesquisaVaga.DescricaoCodVaga) });
                    }
                    catch (Exception ex)
                    {
                        parms.Add(new SqlParameter { ParameterName = "@Cod_Vaga", SqlDbType = SqlDbType.VarChar, Size = 10, Value = objPesquisaVaga.DescricaoCodVaga });
                    }
                }

                if (objPesquisaVaga.Deficiencia != null)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Deficiencia", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaVaga.Deficiencia.IdDeficiencia });

                //if (objPesquisaVaga.DescricaoLocalizacao != null && objPesquisaVaga.NumeroRaio.HasValue && !buscaComGeolocalizacaoPorCidade)
                //{
                //    var sqlParam = new SqlParameter { ParameterName = "@Coordenada", UdtTypeName = "Geography", Value = objPesquisaVaga.DescricaoLocalizacao };
                //    parms.Add(sqlParam);

                //    sqlParam = new SqlParameter { ParameterName = "@Num_Raio", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaVaga.NumeroRaio.Value };
                //    parms.Add(sqlParam);
                //}

                #region [Curso]
                var obj = PesquisaVagaCurso.CarregarPesquisa(objPesquisaVaga.IdPesquisaVaga);
                if (obj.Curso != null)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Curso", SqlDbType = SqlDbType.Int, Size = 4, Value = obj.Curso.IdCurso });
                else if (obj != null && !string.IsNullOrEmpty(obj.DescricaoCurso))
                    parms.Add(new SqlParameter { ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 100, Value = obj.DescricaoCurso });
                #endregion
            }
            else
            {
                parms[3].Value = (int)Enumeradores.Parametro.LimiteResultadoPesquisaSemValoresInformados;

                if (idFilial.HasValue)
                {
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idFilial });
                    parms[3].Value = (int)Enumeradores.Parametro.LimiteResultadoPesquisa;
                }

                if (idCidade.HasValue)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idCidade });

                if (idFuncao.HasValue)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idFuncao });

                if (!String.IsNullOrEmpty(sigEstado))
                    parms.Add(new SqlParameter { ParameterName = "@Sig_Estado", SqlDbType = SqlDbType.Char, Size = 2, Value = sigEstado });

                if (!String.IsNullOrEmpty(palavraChave))
                    parms.Add(new SqlParameter { ParameterName = "@Des_Metabusca_Rapida", SqlDbType = SqlDbType.VarChar, Size = 500, Value = palavraChave });

                if (idFuncaoArea.HasValue)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Funcao_Area", SqlDbType = SqlDbType.Int, Size = 4, Value = idFuncaoArea });
            }

            if (idCurriculo.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idCurriculo });

            parms.Add(new SqlParameter { ParameterName = "@MostrarConfidencial", SqlDbType = SqlDbType.Bit, Value = mostrarConfidencial });

            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "SP_BUSCA_VAGA", parms).Tables[0];
            totalRegistros = Convert.ToInt32(parms[2].Value);

            if (paginaAtual.Equals(1) && objPesquisaVaga != null && !objPesquisaVaga.QuantidadeVagaRetorno.HasValue) //Se for a primeira página, para evitar salvar quando for paginação
                objPesquisaVaga.AtualizarQuantidadeRegistros(totalRegistros);

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
                    new SqlParameter { ParameterName = "@Idf_Pesquisa_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPesquisaVaga } ,
                    new SqlParameter { ParameterName = "@Qtd_Vaga_Retorno", SqlDbType = SqlDbType.Int, Size = 4, Value = quantidadeCurriculoEncontrado }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spatualizarquantidadevagasretornadas, parms);
        }
        #endregion

        #region RecuperarDadosPesquisaVaga
        public static PesquisaVaga RecuperarDadosPesquisaVaga(UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo, string descricaoIP, string descricaoFuncao, string descricaoCidade)
        {
            var objPesquisaVaga = new BLL.PesquisaVaga
            {
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Curriculo = objCurriculo,
                DescricaoIP = descricaoIP
            };

            Funcao objFuncao;
            if (!String.IsNullOrEmpty(descricaoFuncao) && Funcao.CarregarPorDescricao(descricaoFuncao, out objFuncao))
                objPesquisaVaga.Funcao = objFuncao;
            //bool usarTipoVinculo = false;
            //Funcao objFuncao;
            //if (!String.IsNullOrEmpty(descricaoFuncao))
            //{
            //    var funcaoSemAcentos = descricaoFuncao.Trim().NormalizarStringLINQ();
            //
            //    if (funcaoSemAcentos.Equals("Estagio", StringComparison.OrdinalIgnoreCase)
            //        || funcaoSemAcentos.Equals("Estagiario", StringComparison.OrdinalIgnoreCase)
            //        || funcaoSemAcentos.Equals("Estagiaria", StringComparison.OrdinalIgnoreCase)
            //        || funcaoSemAcentos.Equals("Aprendiz", StringComparison.OrdinalIgnoreCase))
            //    {
            //        usarTipoVinculo = true;
            //    }
            //    else
            //    {
            //        if (Funcao.CarregarPorDescricao(descricaoFuncao, out objFuncao))
            //            objPesquisaVaga.Funcao = objFuncao;
            //    }
            //}
            else
                objPesquisaVaga.DescricaoPalavraChave = descricaoFuncao;

            Cidade objCidade;
            if (Cidade.CarregarPorNome(descricaoCidade, out objCidade))
            {
                objPesquisaVaga.Cidade = objCidade;
                objPesquisaVaga.Estado = objCidade.Estado;
            }

            objPesquisaVaga.Save();

            return objPesquisaVaga;
        }
        #endregion

        #region RecuperarDadosPesquisaVaga
        public static PesquisaVaga RecuperarDadosPesquisaVaga(UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo, string descricaoIP, string descricaoAreaBNE)
        {
            var objPesquisaVaga = new BLL.PesquisaVaga
            {
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Curriculo = objCurriculo,
                DescricaoIP = descricaoIP
            };

            AreaBNE objAreaBNE;
            if (!String.IsNullOrEmpty(descricaoAreaBNE) && AreaBNE.CarregarPorDescricao(descricaoAreaBNE, out objAreaBNE))
                objPesquisaVaga.AreaBNE = objAreaBNE;

            objPesquisaVaga.Save();

            return objPesquisaVaga;
        }
        #endregion

        #region RecuperarDadosPesquisaVaga
        public static PesquisaVaga RecuperarDadosPesquisaVagaCandidato(PessoaFisica objPessoaFisica, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioFilialPerfil, string descricaoIP)
        {
            var objPesquisaVaga = new PesquisaVaga
            {
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Curriculo = objCurriculo,
                DescricaoIP = descricaoIP
            };

            FuncaoPretendida objFuncaoPretendida;
            if (FuncaoPretendida.CarregarPorCurriculo(objCurriculo.IdCurriculo, out objFuncaoPretendida))
                objPesquisaVaga.Funcao = objFuncaoPretendida.Funcao;

            Endereco objEndereco;
            if (Endereco.CarregarPorPessoaFisica(objPessoaFisica, out objEndereco))
            {
                objPesquisaVaga.Cidade = objEndereco.Cidade;
                objEndereco.Cidade.CompleteObject();
                objPesquisaVaga.Estado = objEndereco.Cidade.Estado;
            }

            objPesquisaVaga.Save();

            return objPesquisaVaga;
        }
        #endregion

        #region Recuperar Vagas por Estados
        public static Dictionary<string, int> RecuperarVagasPorEstado()
        {
            Dictionary<string, int> facets = Custom.Solr.Vaga.GetFacets("Sig_Estado", null, false);

            Dictionary<string, int> retorno = new Dictionary<string, int>();

            foreach (var item in facets)
            {
                Estado e = Estado.CarregarPorSiglaEstado(item.Key);
                retorno.Add(e.NomeEstado, item.Value);
            }

            return retorno;
        }
        #endregion Recuperar Vagas por Estados

        #region Recuperar Vagas por Area
        public static Dictionary<string, int> RecuperarVagasPorArea(Boolean shortByCount = false, int limit = -1)
        {
            return Custom.Solr.Vaga.GetFacets("Des_Area_BNE", null, shortByCount, limit);
        }
        #endregion Recuperar Vagas por Area

        #region Recuperar Vagas de Area Por Funcao
        public static Dictionary<string, int> RecuperarVagasAreaPorFuncao(string descArea, out AreaBNE objAreaBNE)
        {
            if (!AreaBNE.CarregarPorDescricao(descArea, out objAreaBNE))
                return null;

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("Des_Area_BNE", objAreaBNE.DescricaoAreaBNE);

            return Custom.Solr.Vaga.GetFacets("Des_Funcao", parametros, false);
        }
        #endregion Recuperar Vagas de Area Por Funcao

        #region Recuperar Vagas por Cidade de Estados
        public static Dictionary<string, int> RecuperarVagasPorCidadeDeEstado(string nomeEstado, out Estado objEstado)
        {
            objEstado = Estado.CarregarPorNome(nomeEstado);
            if (objEstado == null)
                return null;

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("Sig_Estado", objEstado.SiglaEstado);

            return Custom.Solr.Vaga.GetFacets("Nme_Cidade", parametros, false);
        }
        #endregion Recuperar Vagas por Cidade de Estados

        #region Recuperar Vagas por Cidade
        public static Dictionary<string, int> RecuperarVagasPorCidade(string nome, string siglaEstado, out Cidade objCidade)
        {
            if (!Cidade.CarregarPorNome(nome + "/" + siglaEstado, out objCidade))
                return null;

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("Nme_Cidade", objCidade.NomeCidade);
            parametros.Add("Sig_Estado", objCidade.Estado.SiglaEstado);

            Dictionary<string, int> facets = Custom.Solr.Vaga.GetFacets("Des_Funcao", parametros, false);

            Dictionary<string, int> retorno = new Dictionary<string, int>();

            foreach (var item in facets)
            {
                retorno.Add(item.Key, item.Value);
            }

            return retorno;
        }
        #endregion Recuperar Vagas por Cidade

        #region Recuperar Número de vagas Solr
        public static int RecuperarCountSolr(PesquisaVaga objPesquisaVaga)
        {
            Dictionary<String, String> parametrosCountSolr = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(objPesquisaVaga.DescricaoAreaBNE))
                parametrosCountSolr.Add("Des_Area_BNE", objPesquisaVaga.DescricaoAreaBNE);
            if (objPesquisaVaga.Funcao != null && objPesquisaVaga.Funcao.IdFuncao > 0)
                parametrosCountSolr.Add("Idf_Funcao", objPesquisaVaga.Funcao.IdFuncao.ToString());
            if (objPesquisaVaga.Cidade != null && objPesquisaVaga.Cidade.IdCidade > 0)
                parametrosCountSolr.Add("Idf_Cidade", objPesquisaVaga.Cidade.IdCidade.ToString());
            if (!String.IsNullOrEmpty(objPesquisaVaga.SiglaEstado))
                parametrosCountSolr.Add("Sig_Estado", objPesquisaVaga.SiglaEstado);
            if (objPesquisaVaga.Escolaridade != null && objPesquisaVaga.Escolaridade.IdEscolaridade > 0)
                parametrosCountSolr.Add("Idf_Escolaridade", objPesquisaVaga.Escolaridade.IdEscolaridade.ToString());
            if (!String.IsNullOrEmpty(objPesquisaVaga.RazaoSocial))
                parametrosCountSolr.Add("Raz_Social", objPesquisaVaga.RazaoSocial);

            if (parametrosCountSolr.Count <= 0)
                return -1;

            return Custom.Solr.Vaga.GetCount(parametrosCountSolr);
        }
        #endregion Recuperar Número de vagas Solr

        #region [BuscaVagaNoPerfil]
        /// <summary>
        /// Utilizado para trazer vagas no perfil do candidato vip.
        /// </summary>
        /// <param name="tamanhoPagina"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="idCurriculo"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable BuscaVagaNoPerfil(int tamanhoPagina, int paginaAtual, int idCurriculo, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Value = paginaAtual },
                new SqlParameter {ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Value = tamanhoPagina },
                new SqlParameter {ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo },
                new SqlParameter {ParameterName = "@Idf_Limite_Resultado", SqlDbType = SqlDbType.Int, Value = 1 },
                new SqlParameter {ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output }
            };

            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "[BNE].[SP_BUSCA_VAGA_PERFIL]", parms).Tables[0];
            totalRegistros = Convert.ToInt32(parms[4].Value);

            return dt;
        }
        #endregion

        #region [BuscaVagaAPI]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPesquisaVaga"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="idOrigemPesquisaVaga"></param>
        /// <param name="mostrarVagasBNEnoSTC"></param>
        /// <param name="ordenacao"></param>
        /// <param name="TotalRegistros"></param>
        /// <returns></returns>
        public static DataTable BuscaVagaAPIDT(PesquisaVaga objPesquisaVaga, int tamanhoPagina, int paginaAtual, int? codigoEmpresa, int? idOrigemPesquisaVaga, bool mostrarVagasBNEnoSTC, OrdenacaoBuscaVaga ordenacao, out int TotalRegistros)
        {
            DataTable dt = new DataTable();
            objPesquisaVaga = objPesquisaVaga == null ? objPesquisaVaga = new BLL.PesquisaVaga() : objPesquisaVaga;
            var listaVagaApi = BuscaVagaAPI(objPesquisaVaga, tamanhoPagina, paginaAtual, codigoEmpresa, idOrigemPesquisaVaga, mostrarVagasBNEnoSTC, ordenacao);

            dt.Columns.Add(new DataColumn("Des_Funcao", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Vlr_Salario_De", Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("Vlr_Salario_Para", Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("Nme_Cidade", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Sig_Estado", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Url_Vaga", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_Tipo_Vinculo", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Idf_Deficiencia", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Qtd_Vaga", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Nme_Bairro", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Flg_Vaga_Rapida", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("Des_Atribuicoes", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_Beneficio", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_Escolaridade", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_Disponibilidade", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Cod_Vaga", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Dta_Abertura", Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("Flg_Candidatou", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("Idf_Vaga", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Flg_BNE_Recomenda", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("idf_origem", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("idf_tipo_origem", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Des_requisito", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_deficiencia", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_Curso", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Flg_Vaga_Arquivada", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("Descricao", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_Area_BNE", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Perguntas", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Flg_Deficiencia", Type.GetType("System.Boolean")));


            if (objPesquisaVaga.Curriculo != null)
            {
                using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        VagasApi(ref dt, ref objPesquisaVaga, listaVagaApi, trans);
                    }
                    conn.Close();
                }
            }
            else
                VagasApi(ref dt, ref objPesquisaVaga, listaVagaApi);

            TotalRegistros = listaVagaApi.TotalRegistros;

            return dt;
        }

        private static void VagasApi(ref DataTable dt, ref PesquisaVaga objPesquisaVaga, DTO.VagaAPI listaVagaApi, SqlTransaction trans = null)
        {
            foreach (var item in listaVagaApi.Registros)
            {
                var vaga = dt.NewRow();
                var perguntas = string.Empty;
                foreach (var item2 in item.Perguntas)
                {
                    perguntas += $"{item2.Texto}<br>";
                }

                vaga["Des_Funcao"] = item.Funcao;
                if (item.SalarioMin.HasValue)
                {
                    vaga["Vlr_Salario_De"] = item.SalarioMin.Value;
                }
                else
                {
                    vaga["Vlr_Salario_De"] = DBNull.Value;
                }
                if (item.SalarioMax.HasValue)
                {
                    vaga["Vlr_Salario_Para"] = item.SalarioMax.Value;
                }
                else
                {
                    vaga["Vlr_Salario_Para"] = DBNull.Value;
                }
                vaga["Nme_Cidade"] = item.Cidade;
                vaga["Sig_Estado"] = item.SiglaEstado;
                vaga["Url_Vaga"] = item.Url;
                vaga["Des_Tipo_Vinculo"] = string.Join(" ", item.TipoVinculo);
                vaga["Idf_Deficiencia"] = DBNull.Value;// string.Join(" ",item.Deficiencia);
                vaga["Des_deficiencia"] = item.Deficiencia;
                vaga["Qtd_Vaga"] = item.Quantidade;
                vaga["Nme_Bairro"] = item.Bairro;
                vaga["Flg_Vaga_Rapida"] = false;
                vaga["Des_Atribuicoes"] = item.Atribuicoes;
                vaga["Des_Beneficio"] = item.Beneficios;
                vaga["Des_Escolaridade"] = item.Escolaridade;
                vaga["Des_Disponibilidade"] = string.Join(" ", item.Disponibilidade);
                vaga["Cod_Vaga"] = item.CodigoVaga;
                vaga["Dta_Abertura"] = item.DataAnuncio;
                vaga["Idf_Vaga"] = item.Id;
                vaga["Flg_BNE_Recomenda"] = item.BNERecomenda;
                vaga["idf_origem"] = item.TipoOrigem;
                vaga["idf_tipo_origem"] = item.TipoOrigem;
                vaga["Des_requisito"] = item.Requisitos != null ? item.Requisitos : string.Empty;
                vaga["Des_deficiencia"] = item.Deficiencia != null ? item.Deficiencia : string.Empty;
                vaga["Des_Curso"] = item.Cursos != null && item.Cursos.Length > 0 ? item.Cursos[0] : string.Empty;
                vaga["Flg_Candidatou"] = objPesquisaVaga.Curriculo != null && trans != null ? VagaCandidato.CurriculoJaCandidatouVaga(objPesquisaVaga.Curriculo, new BLL.Vaga(item.Id), trans) : false;
                vaga["Flg_Vaga_Arquivada"] = item.Status.Equals("Arquivada") ? true : false;
                vaga["Descricao"] = string.Empty;//campo que seria da descrição do curso retornado do sql, vai null so pra não dar erro.
                vaga["Des_Area_BNE"] = item.Area;

                if (!string.IsNullOrEmpty(perguntas))
                    vaga["Perguntas"] = perguntas;
                else
                    vaga["Perguntas"] = DBNull.Value;

                vaga["Flg_Deficiencia"] = (!string.IsNullOrEmpty(item.Deficiencia) && item.Deficiencia != "Nenhuma");
                dt.Rows.Add(vaga);
            }
        }




        public static DTO.VagaAPI BuscaVagaAPI(PesquisaVaga objPesquisaVaga,
            int tamanhoPagina,
            int paginaAtual,
            int? codigoEmpresa,
            int? idOrigemPesquisaVaga,
            bool mostrarVagasBNEnoSTC,
            OrdenacaoBuscaVaga ordenacao
            )
        {
            StringBuilder urlAPI = new StringBuilder();
            StringBuilder sortSolr = new StringBuilder();

            urlAPI.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlApiVagas));

            //Pesquisa avançada com o id da vaga ignora todos os outros filtros.
            if (objPesquisaVaga != null && !String.IsNullOrEmpty(objPesquisaVaga.DescricaoCodVaga))
            {
                urlAPI.Append($"/{objPesquisaVaga.DescricaoCodVaga}");
                return ConsultaApi(urlAPI.ToString());
            }

            urlAPI.Append($"?pagina={paginaAtual}&registrosPorPagina={tamanhoPagina}");

            if (idOrigemPesquisaVaga.HasValue)
            {//dentro do stc não mostra oportunidade
                urlAPI.Append($"&IdOrigem={idOrigemPesquisaVaga.Value}&Oportunidade=false");
                if (mostrarVagasBNEnoSTC)
                {//Vaga do bne origem 1 e sine origem 2.
                    urlAPI.Append($"&IdOrigem=1&IdOrigem=2");
                    sortSolr.Append("Idf_Origem desc,");
                }
            }
            else//Fora do STC não mostrar vaga de empresa que oferece curso
                urlAPI.Append($"&OfereceCurso=false");

            if (objPesquisaVaga != null)
            {
                if (!String.IsNullOrEmpty(objPesquisaVaga.DescricaoCodVaga))
                {
                    urlAPI.Append($"/{objPesquisaVaga.DescricaoCodVaga}");
                    return ConsultaApi(urlAPI.ToString());
                }

                if (!string.IsNullOrEmpty(objPesquisaVaga.DescricaoPalavraChave))
                    urlAPI.Append($"&query={objPesquisaVaga.DescricaoPalavraChave}");

                if (objPesquisaVaga.AreaBNE != null)
                {
                    objPesquisaVaga.AreaBNE.CompleteObject();
                    urlAPI.Append($"&area={objPesquisaVaga.AreaBNE.DescricaoAreaBNE}");
                }

                if (objPesquisaVaga.Cidade != null)
                {
                    //urlAPI.Append($"&nomeCidade={objPesquisaVaga.NomeCidade}");
                    urlAPI.Append($"&CidadeRegiao={objPesquisaVaga.Cidade.IdCidade}");
                    sortSolr.Append($"termfreq(Idf_Cidade, {objPesquisaVaga.Cidade.IdCidade}) desc,");
                }

                if (objPesquisaVaga.Estado != null)
                    urlAPI.Append($"&siglaEstado={objPesquisaVaga.Estado.SiglaEstado}");

                if (objPesquisaVaga.Funcao != null)
                {
                    urlAPI.Append($"&FuncaoAgrupadora={objPesquisaVaga.Funcao.IdFuncao}");
                    sortSolr.Append($"termfreq(Idf_Funcao, {objPesquisaVaga.Funcao.IdFuncao}) desc,");
                }

                if (objPesquisaVaga.Escolaridade != null)
                    urlAPI.Append($"&escolaridade={objPesquisaVaga.Escolaridade.IdEscolaridade}");

                if (objPesquisaVaga.NumeroSalarioMin.HasValue)
                    urlAPI.Append($"&salarioMinimo={objPesquisaVaga.NumeroSalarioMin}");

                if (objPesquisaVaga.NumeroSalarioMax.HasValue)
                    urlAPI.Append($"&salarioMaximo={objPesquisaVaga.NumeroSalarioMax}");



                else if (!String.IsNullOrEmpty(objPesquisaVaga.RazaoSocial))
                    urlAPI.Append($"&empresa={objPesquisaVaga.RazaoSocial}");

                if (objPesquisaVaga.Curriculo != null)
                    urlAPI.Append($"&curriculo={objPesquisaVaga.Curriculo.IdCurriculo}");

                #region [Curso]
                var obj = PesquisaVagaCurso.CarregarPesquisa(objPesquisaVaga.IdPesquisaVaga);
                if (obj.Curso != null)
                    urlAPI.Append($"&IdCurso={obj.Curso.IdCurso}");
                else if (obj != null && !string.IsNullOrEmpty(obj.DescricaoCurso))
                    urlAPI.Append($"&Curso={obj.DescricaoCurso}");
                #endregion

                var disponibilidades = PesquisaVagaDisponibilidade.ListarIdentificadores(objPesquisaVaga);
                foreach (var item in disponibilidades)
                    urlAPI.Append($"&Disponibilidade={item}");

                var vinculos = PesquisaVagaTipoVinculo.ListarIdentificadores(objPesquisaVaga);
                foreach (var item in vinculos)
                    urlAPI.Append($"&TipoVinculo={item}");

                if (objPesquisaVaga.Deficiencia != null)
                    urlAPI.Append($"&Deficiencia={objPesquisaVaga.Deficiencia.IdDeficiencia}");


            }
            if (codigoEmpresa.HasValue)
            {// não trazer as vagas confidenciais
                urlAPI.Append($"&idfFilial={codigoEmpresa.Value}");
                urlAPI.Append($"&Confidencial={true}");
            }

            //Não pegar vaga com data de abertura futura
            urlAPI.Append($"&DataFim={DateTime.Now.ToString("yyyy-MM-dd")}");

            switch (ordenacao)
            {
                case OrdenacaoBuscaVaga.Padrao:
                    urlAPI.Append($"&ordenacao={sortSolr.ToString()}Flg_Vaga_Arquivada+asc%2C+Dta_Abertura+desc");
                    break;
                case OrdenacaoBuscaVaga.SalarioDecrescente:
                    urlAPI.Append($"&ordenacao=Flg_Vaga_Arquivada+asc%2C+max(Vlr_Salario_De%2C+Vlr_Salario_Para)+desc,{sortSolr.ToString()}+Dta_Abertura+desc");
                    break;
                case OrdenacaoBuscaVaga.DataAbertura:
                    urlAPI.Append($"&ordenacao={sortSolr.ToString()}Dta_Abertura+desc%2CFlg_Vaga_Arquivada+asc%2C");
                    break;
                default:
                    urlAPI.Append($"&ordenacao={sortSolr.ToString()}Flg_Vaga_Arquivada+asc%2C+Dta_Abertura+desc");
                    break;
            }

            return ConsultaApi(urlAPI.ToString());
        }

        private static DTO.VagaAPI ConsultaApi(string urlAPI)
        {
            DTO.VagaAPI vagas = new DTO.VagaAPI();
            DTO.Registro[] vaga = new DTO.Registro[1];
            string apiKey = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ApiKeyVagas);

            var webRequest = WebRequest.Create(urlAPI.ToString());
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("apiKey", apiKey);

                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        vagas = JsonConvert.DeserializeObject<DTO.VagaAPI>(jsonResponse);
                        if (vagas.Registros == null)
                        {
                            vaga.SetValue(JsonConvert.DeserializeObject<DTO.Registro>(jsonResponse), 0);
                            vagas.Registros = vaga;
                            vagas.TotalRegistros = vaga.Length;
                        }
                    }
                }
            }
            return vagas;
        }
        #endregion

        #region [MinhasVagas]

        public static DataTable ListaMinhasVagas(int paginaAtual,
            int tamanhoPagina, int IdEmpresa,
            bool? Campanha, bool? oportunidade,
            List<string> ListUsuarioFilailPerfilFiltro, List<string> listFuncaoFiltro, out int TotalRegistros)
        {
            DataTable dt = new DataTable();
            var listaVagaApi = BuscaMinhasVagasAPI(paginaAtual, tamanhoPagina, IdEmpresa, Campanha, oportunidade, ListUsuarioFilailPerfilFiltro, listFuncaoFiltro);

            #region [Colunas do DT]
            dt.Columns.Add(new DataColumn("Des_Funcao", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Vlr_Salario_De", Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("Vlr_Salario_Para", Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("Nme_Cidade", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Sig_Estado", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Url_Vaga", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Idf_Deficiencia", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Des_Atribuicoes", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_Beneficio", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Cod_Vaga", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Idf_Vaga", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Des_requisito", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_deficiencia", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Flg_Vaga_Arquivada", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("Descricao", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Des_Area_BNE", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Perguntas", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Flg_Deficiencia", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("Dta_Abertura", Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("Num_Cvs_Recebidos", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Num_Cvs_Recebidos_Nao_Lidos", Type.GetType("System.String")));

            #endregion

            TotalRegistros = listaVagaApi.TotalRegistros;

            #region [Preencher o DataTable]
            foreach (var item in listaVagaApi.Registros)
            {
                var vaga = dt.NewRow();
                var perguntas = string.Empty;
                foreach (var item2 in item.Perguntas)
                {
                    perguntas += $"{item2.Texto}<br>";
                }

                vaga["Des_Funcao"] = item.Cursos !=null ? $"{item.Funcao} - {item.Cursos[0]}" : item.Funcao;
                if (item.SalarioMin.HasValue && item.SalarioMin.Value > 0)
                {
                    vaga["Vlr_Salario_De"] = item.SalarioMin.Value;
                }
                else
                {
                    vaga["Vlr_Salario_De"] = DBNull.Value;
                }
                if (item.SalarioMax.HasValue && item.SalarioMax.Value > 0)
                {
                    vaga["Vlr_Salario_Para"] = item.SalarioMax.Value;
                }
                else
                {
                    vaga["Vlr_Salario_Para"] = DBNull.Value;
                }
                vaga["Nme_Cidade"] = item.Cidade.Split('/')[0];
                vaga["Sig_Estado"] = item.SiglaEstado;
                vaga["Url_Vaga"] = item.Url;
                vaga["Idf_Deficiencia"] = DBNull.Value;// string.Join(" ",item.Deficiencia);
                vaga["Des_deficiencia"] = item.Deficiencia;
                vaga["Des_Atribuicoes"] = (item.Atribuicoes != null && item.Atribuicoes.Length > 100) ? $"{item.Atribuicoes.Substring(0,100)}...": item.Atribuicoes;
                vaga["Cod_Vaga"] = item.CodigoVaga;
                vaga["Dta_Abertura"] = item.DataAnuncio;
                vaga["Idf_Vaga"] = item.Id;
                vaga["Des_requisito"] = item.Requisitos != null ? item.Requisitos : string.Empty;
                vaga["Des_deficiencia"] = item.Deficiencia != null ? item.Deficiencia : string.Empty;
                vaga["Flg_Vaga_Arquivada"] = item.Status.Equals("Arquivada") ? true : false;
                vaga["Descricao"] = string.Empty;//campo que seria da descrição do curso retornado do sql, vai null so pra não dar erro.
                vaga["Des_Area_BNE"] = item.Area;

                if (!string.IsNullOrEmpty(perguntas))
                    vaga["Perguntas"] = perguntas;
                else
                    vaga["Perguntas"] = DBNull.Value;

                vaga["Flg_Deficiencia"] = (!string.IsNullOrEmpty(item.Deficiencia) && item.Deficiencia != "Nenhuma");
                var ResultCandidatos = BNE.Solr.VagaCandidato.EfetuarRequisicao(BLL.Custom.Solr.BuscaVagaCandidato.MontaUrlSolr(1, 1, item.Id,null,null));
                var ResultCandidatosVistos = BNE.Solr.VagaCandidato.EfetuarRequisicao(BLL.Custom.Solr.BuscaVagaCandidato.MontaUrlSolr(1, 1,item.Id,true,null));
                vaga["Num_Cvs_Recebidos"] = ResultCandidatos.response.numFound.ToString();
                vaga["Num_Cvs_Recebidos_Nao_Lidos"] = (ResultCandidatos.response.numFound - ResultCandidatosVistos.response.numFound).ToString();
                dt.Rows.Add(vaga);
            }
            #endregion

            return dt;
        }

        #region [BuscaMinhasVagasAPI]
        private static DTO.VagaAPI BuscaMinhasVagasAPI(int paginaAtual,
            int tamanhoPagina, int IdEmpresa,
            bool? Campanha, bool? oportunidade,
            List<string> ListUsuarioFilailPerfilFiltro, List<string> ListFuncaoFiltro)
        {
            StringBuilder urlAPI = new StringBuilder();
            StringBuilder sortSolr = new StringBuilder();

            urlAPI.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlApiVagas));

            urlAPI.Append($"?pagina={++paginaAtual}&registrosPorPagina={tamanhoPagina}");

            foreach(var item in ListFuncaoFiltro)
                urlAPI.Append($"&Funcao={item}");

            if (oportunidade.HasValue)
                urlAPI.Append($"&oportunidade={!oportunidade.Value}");
            if (Campanha.HasValue)
                urlAPI.Append($"&Campanha={Campanha.Value}");

            urlAPI.Append($"&idfFilial={IdEmpresa}");

            foreach(var ufp in ListUsuarioFilailPerfilFiltro)
                urlAPI.Append($"&UsuarioFilial={ufp}");

            urlAPI.Append($"&ordenacao=Flg_Vaga_Arquivada+asc%2C+Dta_Abertura+desc");

            return ConsultaApi(urlAPI.ToString());
        }
        #endregion

        #endregion

        public List<int> ListarIdentificadoresDisponibilidades()
        {
            return PesquisaVagaDisponibilidade.ListarIdentificadores(this);
        }

        public List<int> ListarIdentificadoresTiposDeVinculo()
        {
            return PesquisaVagaTipoVinculo.ListarIdentificadores(this);
        }
    }
}