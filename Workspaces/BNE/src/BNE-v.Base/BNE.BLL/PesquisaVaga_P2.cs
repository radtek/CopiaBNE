//-- Data: 10/08/2010 15:09
//-- Autor: Gieyson Stelmak

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Web.UI;
using BNE.BLL.Custom;
using System.IO;
using System.Net;
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
        public void Salvar(List<PesquisaVagaDisponibilidade> listPesquisaVagaDisponibilidade, List<PesquisaVagaTipoVinculo> listPesquisaVagaTipoVinculo)
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
        public static DataTable BuscaVagaFullText(PesquisaVaga objPesquisaVaga, int tamanhoPagina, int paginaAtual, int? idCurriculo, int? idFuncao, int? idCidade, string palavraChave, int? idOrigem, bool? empresaOfereceCursos, string sigEstado, int? idFilial, int? idFuncaoArea, OrdenacaoBuscaVaga ordenacao, out int totalRegistros)
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

                string idfsDisponibilidade = PesquisaVagaDisponibilidade.ListarIdentificadoresConcatenadosPorPesquisa(objPesquisaVaga.IdPesquisaVaga);
                if (!String.IsNullOrEmpty(idfsDisponibilidade))
                    parms.Add(new SqlParameter { ParameterName = "@Idfs_Disponibilidade", SqlDbType = SqlDbType.VarChar, Size = 100, Value = idfsDisponibilidade });

                string idfsTipoVinculo = PesquisaVagaTipoVinculo.ListarIdentificadoresConcatenadosPorPesquisa(objPesquisaVaga.IdPesquisaVaga);
                if (!String.IsNullOrEmpty(idfsTipoVinculo))
                    parms.Add(new SqlParameter { ParameterName = "@Idfs_Tipo_Vinculo", SqlDbType = SqlDbType.VarChar, Size = 100, Value = idfsTipoVinculo });

                if (!String.IsNullOrEmpty(objPesquisaVaga.RazaoSocial))
                    parms.Add(new SqlParameter { ParameterName = "@Raz_Social", SqlDbType = SqlDbType.VarChar, Size = 50, Value = objPesquisaVaga.RazaoSocial });

                if (!String.IsNullOrEmpty(objPesquisaVaga.DescricaoCodVaga))
                    parms.Add(new SqlParameter { ParameterName = "@Cod_Vaga", SqlDbType = SqlDbType.VarChar, Size = 10, Value = objPesquisaVaga.DescricaoCodVaga });

                if (objPesquisaVaga.Deficiencia != null)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Deficiencia", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaVaga.Deficiencia.IdDeficiencia });

                //if (objPesquisaVaga.DescricaoLocalizacao != null && objPesquisaVaga.NumeroRaio.HasValue && !buscaComGeolocalizacaoPorCidade)
                //{
                //    var sqlParam = new SqlParameter { ParameterName = "@Coordenada", UdtTypeName = "Geography", Value = objPesquisaVaga.DescricaoLocalizacao };
                //    parms.Add(sqlParam);

                //    sqlParam = new SqlParameter { ParameterName = "@Num_Raio", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaVaga.NumeroRaio.Value };
                //    parms.Add(sqlParam);
                //}
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

            if (objPessoaFisica.Endereco != null)
            {
                objPessoaFisica.Endereco.CompleteObject();
                objPesquisaVaga.Cidade = objPessoaFisica.Endereco.Cidade;
                objPesquisaVaga.Estado = objPessoaFisica.Endereco.Cidade.Estado;
            }

            objPesquisaVaga.Save();

            return objPesquisaVaga;
        }
        #endregion

        #region Recuperar Vagas por Estados
        public static Dictionary<string, int> RecuperarVagasPorEstado()
        {
            Dictionary<string, int> facets = Custom.Solr.SolrVaga.GetFacets("Sig_Estado", null, false);

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
        public static Dictionary<string, int> RecuperarVagasPorArea()
        {
            return Custom.Solr.SolrVaga.GetFacets("Des_Area_BNE", null, false);
        }
        #endregion Recuperar Vagas por Area

        #region Recuperar Vagas de Area Por Funcao
        public static Dictionary<string, int> RecuperarVagasAreaPorFuncao(string descArea, out AreaBNE objAreaBNE)
        {
            if (!AreaBNE.CarregarPorDescricao(descArea, out objAreaBNE))
                return null;

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("Des_Area_BNE", objAreaBNE.DescricaoAreaBNE);

            return Custom.Solr.SolrVaga.GetFacets("Des_Funcao", parametros, false);
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

            return Custom.Solr.SolrVaga.GetFacets("Nme_Cidade", parametros, false);
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

            Dictionary<string, int> facets = Custom.Solr.SolrVaga.GetFacets("Des_Funcao", parametros, false);

            Dictionary<string, int> retorno = new Dictionary<string, int>();

            foreach (var item in facets)
            {
                retorno.Add(item.Key, item.Value);
            }

            return retorno;
        }
        #endregion Recuperar Vagas por Cidade


    }
}