//-- Data: 22/06/2010 12:18
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Solr;
using LoginAutomatico = BNE.BLL.Custom.LoginAutomatico;

namespace BNE.BLL
{
    public partial class RastreadorCurriculo // Tabela: BNE_Rastreador_Curriculo
    {
        #region Consultas

        #region Spselectporfilial

        private const string Spselectporfilial = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect NVARCHAR(MAX)
        DECLARE @iSelectCount NVARCHAR(MAX)
        DECLARE @iSelectPag NVARCHAR(MAX)
        SET @FirstRec = @CurrentPage * @PageSize + 1
        SET @LastRec = ( @CurrentPage * @PageSize ) + @PageSize
        SET @iSelect = '
            SELECT	ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC) AS RowID ,
					*
            FROM 
            (
                SELECT	RC.Idf_Rastreador_Curriculo ,
						F.Des_Funcao ,
						RC.Des_Palavra_Chave ,
						Quantidade.Qtd as Qtd_Curriculo ,
                        /*QuantidadeNaoVisualizado.Qtd as Qtd_Curriculo_Nao_Visualizados,*/
						RC.Dta_Cadastro,
						C.Nme_Cidade ,
						C.Sig_Estado ,
                        RC.Idf_Escolaridade_WebStagio                        
                FROM	BNE_Rastreador_Curriculo RC WITH(NOLOCK)
						OUTER APPLY ( SELECT    F.Des_Funcao + '' ''
                                      FROM      BNE_Rastreador_Curriculo_Funcao RCF
                                                WITH ( NOLOCK )
                                                INNER JOIN plataforma.TAB_Funcao F
                                                WITH ( NOLOCK ) ON F.Idf_Funcao = RCF.Idf_Funcao
                                      WHERE     RCF.idf_Rastreador_Curriculo = RC.idf_Rastreador_Curriculo
                                    FOR
                                      XML PATH('''')
                                    ) AS F (Des_Funcao)
						LEFT JOIN plataforma.TAB_Cidade C WITH(NOLOCK) ON C.Idf_Cidade = RC.Idf_Cidade
						OUTER APPLY ( SELECT COUNT (RCR.Idf_Rastreador_Curriculo) Qtd FROM BNE_Rastreador_Curriculo_Resultado RCR WITH(NOLOCK) WHERE RCR.Idf_Rastreador_Curriculo = RC.Idf_Rastreador_Curriculo ) AS Quantidade
						/*OUTER APPLY ( SELECT COUNT (RCR.Idf_Rastreador_Curriculo) Qtd FROM BNE_Rastreador_Curriculo_Resultado RCR WITH(NOLOCK) WHERE RCR.Idf_Rastreador_Curriculo = RC.Idf_Rastreador_Curriculo AND RCR.Dta_Cadastro > RC.Dta_Visualizacao ) AS QuantidadeNaoVisualizado*/
                WHERE	RC.Idf_Filial = @Idf_Filial
						AND RC.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
						AND RC.Flg_Inativo = 0
            ) as temp '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)
		
		DECLARE @ParmDefinition NVARCHAR(1000)
		SET @ParmDefinition = N'@Idf_Filial INT, @Idf_Usuario_Filial_Perfil INT'
                                         
        EXECUTE sp_executesql @iSelectCount, @ParmDefinition, @Idf_Filial = @Idf_Filial, @Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
        EXECUTE sp_executesql @iSelectPag, @ParmDefinition, @Idf_Filial = @Idf_Filial, @Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil";

        #endregion

        #region Sprecuperareatualizardata

        private const string Sprecuperareatualizardata = @" 
        SELECT Dta_Visualizacao FROM BNE_Rastreador_Curriculo WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo
        UPDATE BNE_Rastreador_Curriculo SET Dta_Visualizacao = GETDATE() WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo
        ";

        #endregion

        #region Spselectrastreadores

        private const string Spselectrastreadores = @"
        SELECT  RC.*
        FROM    BNE_Rastreador_Curriculo RC WITH ( NOLOCK )
                OUTER APPLY ( SELECT TOP 1
                                        Idf_Rastreador_Curriculo_Historico
                              FROM      BNE_Rastreador_Curriculo_Historico RCH
                                        WITH ( NOLOCK )
                              WHERE     RCH.Idf_Rastreador_Curriculo = RC.Idf_Rastreador_Curriculo
                                        AND Dta_Processamento > CONVERT(DATE, GETDATE())
                              ORDER BY  Dta_Cadastro DESC
                            ) AS RCH
                INNER JOIN TAB_Filial F WITH ( NOLOCK ) on f.idf_filial = rc.idf_filial
        WHERE   RC.Flg_Inativo = 0
                AND ( Flg_Notifica_Hora = 1
                      OR ( Flg_Notifica_Dia = 1
                           AND DATEPART(HOUR, GETDATE()) = DATEPART(HOUR, @HoraPadraoAlertaDiario)
                           AND RCH.Idf_Rastreador_Curriculo_Historico IS NULL
                         )
                    )
                and f.Idf_Situacao_Filial IN ( 1, 2, 3, 4, 7 )
        ";

        #endregion

        #region Spinativarrastreadorcurriculo

        private const string Spinativarrastreadorcurriculo = @"UPDATE BNE_Rastreador_Curriculo SET Flg_Inativo = 1 WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";

        #endregion

        #region Sprecuperarequantidadecurriculosnaovisualizados

        private const string Sprecuperarequantidadecurriculosnaovisualizados = @"
        SELECT  COUNT(Idf_Rastreador_Curriculo_Resultado)
        FROM    BNE_Rastreador_Curriculo RC WITH ( NOLOCK )
                INNER JOIN BNE_Rastreador_Curriculo_Resultado RCR ON RCR.Idf_Rastreador_Curriculo = RC.Idf_Rastreador_Curriculo
        WHERE   RC.Dta_Visualizacao < rcr.Dta_Cadastro
                AND rc.Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo
        ";

        #endregion

        private const string Spselectporrastreadorcurriculo = @"SELECT Idf_Curriculo FROM BNE_Rastreador_Curriculo_Resultado WITH(NOLOCK) WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";

        #endregion

        #region Métodos

        #region Salvar

        /// <summary>
        /// </summary>
        /// <param name="listaFuncao"></param>
        /// <param name="listaIdioma"></param>
        /// <param name="listaDisponibilidade"></param>
        /// <param name="listaPalavraChave"></param>
        /// <param name="objRastreadorCurriculoAntigo">
        ///     Rastreador antigo, se houver, uma edição por exemplo, tem que inatviar ele,
        ///     por causa dos perfis diferentes para currículos que já foram rastreados
        /// </param>
        /// <param name="queroContratarEstagiarios"></param>
        /// <param name="queroContratarAprendiz"></param>
        public void Salvar(List<RastreadorCurriculoFuncao> listaFuncao, List<RastreadorCurriculoIdioma> listaIdioma, List<RastreadorCurriculoDisponibilidade> listaDisponibilidade, List<CampoPalavraChaveRastreadorCurriculo> listaPalavraChave, RastreadorCurriculo objRastreadorCurriculoAntigo, bool queroContratarEstagiarios = false, bool queroContratarAprendiz = false)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (queroContratarEstagiarios)
                            IdEscolaridadeWebStagio = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfsEscolaridadeWebEstagiosQueroEstagiario, trans);

                        if (queroContratarAprendiz)
                            FlagAprendiz = true;

                        Save(trans);

                        foreach (var item in listaFuncao)
                        {
                            item.RastreadorCurriculo = this;
                            item.Save(trans);
                        }
                        foreach (var item in listaIdioma)
                        {
                            item.RastreadorCurriculo = this;
                            item.Save(trans);
                        }
                        foreach (var item in listaDisponibilidade)
                        {
                            item.RastreadorCurriculo = this;
                            item.Save(trans);
                        }
                        foreach (var item in listaPalavraChave)
                        {
                            item.RastreadorCurriculo = this;
                            item.Save(trans);
                        }

                        if (objRastreadorCurriculoAntigo != null)
                        {
                            objRastreadorCurriculoAntigo.FlagInativo = true;
                            objRastreadorCurriculoAntigo.Save();
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

        #region ListarRastreadores

        /// <summary>
        ///     Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarRastreadores(int idFilial, int idUsuarioFilialPerfil, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial},
                new SqlParameter {ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
                new SqlParameter {ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina},
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil}
            };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporfilial, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }

        #endregion

        #region RecuperarDataUltimaVisualizacao

        public DateTime? RecuperarDataUltimaVisualizacao()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idRastreadorCurriculo}
            };

            var result = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperareatualizardata, parms);
            if (result != DBNull.Value)
            {
                return Convert.ToDateTime(result);
            }

            return null;
        }

        #endregion

        #region RecuperarRastreadores

        public static List<RastreadorCurriculo> RecuperarRastreadores()
        {
            var lista = new List<RastreadorCurriculo>();

            var time = TimeSpan.Parse(Parametro.RecuperaValorParametro(Enumeradores.Parametro.AlertaCurriculoHoraProcessarDiario));
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@HoraPadraoAlertaDiario", SqlDbType = SqlDbType.Time, Value = time}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectrastreadores, parms))
            {
                var objRastreador = new RastreadorCurriculo();
                while (SetInstance(dr, objRastreador, false))
                {
                    lista.Add(objRastreador);
                    objRastreador = new RastreadorCurriculo();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }

        #endregion

        #region RastrearNovosCurriculos

        public void RastrearNovosCurriculos()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var curriculosjarastreados = CurriculosJaRastreados(trans);

                        var watch = new Stopwatch();
                        watch.Start();
                        var curriculosNoPerfil = RastrearCurriculos();
                        var quantidadeCurriculosNoPerfil = Convert.ToInt16(curriculosNoPerfil.Count);
                        watch.Stop();
                        var novosCurriculos = curriculosNoPerfil.Where(cr => curriculosjarastreados.All(cjr => cjr != cr.IdCurriculo)).ToList();
                        var curriculosForaPerfil = curriculosjarastreados.Where(cr => curriculosNoPerfil.All(cjr => cjr.IdCurriculo != cr)).ToList();

                        var quantidadeNovosCurriculos = Convert.ToInt16(novosCurriculos.Count);

                        if (novosCurriculos.Any())
                        {
                            DataTable dtCurriculos = null;

                            foreach(var cv in novosCurriculos)
                            {
                                var objRastreadorCurriculoResultado = new RastreadorCurriculoResultado
                                {
                                    Curriculo = new Curriculo(cv.IdCurriculo),
                                    RastreadorCurriculo = this,
                                    FlagInativo = false
                                };
                                objRastreadorCurriculoResultado.AddBulkTable(ref dtCurriculos);
                            }

                            RastreadorCurriculoResultado.SaveBulkTable(dtCurriculos, trans);
                        }
                        if (curriculosForaPerfil.Any())
                            RastreadorCurriculoResultado.DeletarCurriculosForaPerfil(this, curriculosForaPerfil, trans);

                        RastreadorCurriculoHistorico.SalvarHistorico(this, quantidadeCurriculosNoPerfil, quantidadeNovosCurriculos, watch.Elapsed, trans);

                        trans.Commit();

                        Notificar(this, novosCurriculos);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        GerenciadorException.GravarExcecao(ex, "Rastreador " + _idRastreadorCurriculo);
                    }
                }
            }
        }

        #endregion

        #region Notificar

        private void Notificar(RastreadorCurriculo objRastreadorCurriculo, List<DTO.CurriculoRastreador> listaCurriculos)
        {
            if (listaCurriculos.Any())
            {
                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objRastreadorCurriculo.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                {
                    objRastreadorCurriculo.UsuarioFilialPerfil.CompleteObject();
                    objRastreadorCurriculo.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                    var funcao = "Currículos";

                    var funcoes = RastreadorCurriculoFuncao.ListarIdentificadoresFuncaoPorRastreador(objRastreadorCurriculo).Select(x => Funcao.LoadObject(x).DescricaoFuncao).ToList();
                    if (funcoes.Any())
                    {
                        funcao = string.Join(", ", funcoes);
                    }

                    objRastreadorCurriculo.Filial.CompleteObject();

                    #region MontaCandidatos

                    var cartaCandidato = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.RastreadorCurriculoCurriculoEncontrado);

                    var sb = new StringBuilder();

                    foreach(var curriculo in listaCurriculos)
                    {
                        var urlVisualizacao = SitemapHelper.MontarCaminhoVisualizacaoCurriculo(curriculo.NomeFuncaoPretendida, curriculo.NomeCidade, curriculo.SiglaEstado, curriculo.IdCurriculo).ToLower();
                        var url = string.Format("http://{0}/logar/{1}{2}", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente), LoginAutomatico.GerarHashAcessoLogin(objRastreadorCurriculo.UsuarioFilialPerfil.PessoaFisica.CPF, objRastreadorCurriculo.UsuarioFilialPerfil.PessoaFisica.DataNascimento, urlVisualizacao.NormalizarURL()), "?utm_source=alerta-curriculo&utm_medium=email&utm_campaign=ver_curriculo");

                        var parametroCandidato = new
                        {
                            Candidato = curriculo.PrimeiroNome,
                            Funcao = TratarFuncaoCVRastreador(curriculo, funcoes),
                            curriculo.Idade,
                            Cidade = Helper.FormatarCidade(curriculo.NomeCidade, curriculo.SiglaEstado),
                            VerCurriculo = url
                        };
                        sb.Append(parametroCandidato.ToString(cartaCandidato));
                    }

                    var urlAcessoConta = string.Format("http://{0}/logar/{1}{2}", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente), LoginAutomatico.GerarHashAcessoLogin(objRastreadorCurriculo.UsuarioFilialPerfil.PessoaFisica.CPF, objRastreadorCurriculo.UsuarioFilialPerfil.PessoaFisica.DataNascimento, "/sala-selecionador"), "?utm_source=alerta-curriculo&utm_medium=email&utm_campaign=acesso_conta");

                    
                    var objCartaEmail = CartaEmail.LoadObject((int)Enumeradores.CartaEmail.RastreadorCurriculo);
                    //Task 46182 - Quando o rastreador não tiver função, trocar o layout da carta com outro texto.
                    if (funcoes.Count() <= 0)
                        objCartaEmail = CartaEmail.LoadObject((int)Enumeradores.CartaEmail.RastreadorCurriculoSemFuncao);

                    if(funcao.Count() <=0 && objRastreadorCurriculo.Cidade == null)
                    {
                        objCartaEmail.ValorCartaEmail = objCartaEmail.ValorCartaEmail.Replace("em {Cidade}", string.Empty).Replace("de {Funcao}", string.Empty);
                        objCartaEmail.DescricaoAssunto = "Rastreador de Currículos";
                    }
                    if (objRastreadorCurriculo.Cidade != null)
                        objRastreadorCurriculo.Cidade.CompleteObject();

                    var parametros = new
                    {
                        Empresa = objRastreadorCurriculo.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome,
                        Funcao = funcao,
                        AcessoConta = urlAcessoConta,
                        Candidatos = sb.ToString(),
                        Cidade = objRastreadorCurriculo.Cidade != null ? Helper.FormatarCidade(objRastreadorCurriculo.Cidade.NomeCidade, objRastreadorCurriculo.Estado.SiglaEstado) : string.Empty

                    };
                   

                    var mensagem = parametros.ToString(objCartaEmail.ValorCartaEmail);
                    var assunto = parametros.ToString(objCartaEmail.DescricaoAssunto);

                    #endregion

                    var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                    EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, Enumeradores.CartaEmail.RastreadorCurriculo, emailRemetente, objUsuarioFilial.EmailComercial);
                }
            }
        }

        #endregion

        #region TratarFuncaoCVRastreador

        protected string TratarFuncaoCVRastreador(DTO.CurriculoRastreador curriculo, List<string> FuncoesRastreador)
        {
            try
            {
                var funcoesCurriculoNoRastreador = (from funcoesCurriculo in curriculo.FuncoesPretendidas
                                                    join funRastreador in FuncoesRastreador on funcoesCurriculo.NomeFuncaoPretendida equals funRastreador
                                                    select funcoesCurriculo).ToList();
                return string.Join(" ,", funcoesCurriculoNoRastreador.Select(x => x.NomeFuncaoPretendida));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Falha ao buscar funções do CV para o rastreador");
                return curriculo.NomeFuncaoPretendida;
            }
        }

        #endregion

        #region QuantidadeCurriculoNaoVisualizado

        public int QuantidadeCurriculoNaoVisualizado()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idRastreadorCurriculo}
            };

            var result = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarequantidadecurriculosnaovisualizados, parms);
            if (result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        #endregion

        #region Inativar

        public void Inativar()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = IdRastreadorCurriculo}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spinativarrastreadorcurriculo, parms);
        }

        #endregion

        #region RastrearCurriculos

        /// <summary>
        ///     Lista com todos os currículos que estão dentro dos parametros do rastreador
        /// </summary>
        /// <returns></returns>
        public List<DTO.CurriculoRastreador> RastrearCurriculos()
        {
            var resultado = CurriculoRastreador.EfetuarRequisicao(BuscaCurriculo.MontarQuery(this));

            if (resultado != null)
            {
                var lista = new List<DTO.CurriculoRastreador>();
                foreach (var curriculo in resultado.response.docs)
                {
                    var curriculoRastreador = new DTO.CurriculoRastreador();
                    curriculoRastreador.MapearCurriculo(curriculo);
                    lista.Add(curriculoRastreador);
                }

                return lista;
            }
            return new List<DTO.CurriculoRastreador>();
        }

        #endregion

        #region CurriculosJaRastreados

        /// <summary>
        ///     Recupera uma lista com os currículos já rastreados
        /// </summary>
        /// <returns></returns>
        public List<int> CurriculosJaRastreados(SqlTransaction trans)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = IdRastreadorCurriculo}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectporrastreadorcurriculo, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Curriculo"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }

        #endregion

        #endregion

        public static void Processar()
        {
            try
            {
                var lista = RecuperarRastreadores();

                foreach(var objRastreadorCurriculo in lista)
                {
                    var watch = new Stopwatch();
                    watch.Start();

                    try
                    {
                        objRastreadorCurriculo.RastrearNovosCurriculos();
                    }
                    catch (Exception ex)
                    {
                        var customMessage = string.Format("Falha ao rastrear currículos para o Rastreador {0}", objRastreadorCurriculo.IdRastreadorCurriculo);
                        GerenciadorException.GravarExcecao(ex, customMessage);
                    }

                    watch.Stop();
                    GerenciadorException.GravarInformacao(string.Format("Rastreador {0} levou {1} para completar", objRastreadorCurriculo.IdRastreadorCurriculo, watch.Elapsed));
                    GerenciadorException.GravarExcecao(new Exception(string.Format("Rastreador {0} levou {1} para completar", objRastreadorCurriculo.IdRastreadorCurriculo, watch.Elapsed)));
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
    }
}