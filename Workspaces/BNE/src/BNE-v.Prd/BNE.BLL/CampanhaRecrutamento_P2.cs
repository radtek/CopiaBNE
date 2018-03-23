//-- Data: 13/07/2015 17:00
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Solr;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;

namespace BNE.BLL
{
    public partial class CampanhaRecrutamento // Tabela: BNE_Campanha_Recrutamento
    {

        #region Consultas

        #region Spselectcampanhasrecrutamentoemaberto
        private const string Spselectcampanhasrecrutamentoemaberto = @"
        SELECT  * 
        FROM    BNE_Campanha_Recrutamento 
        WHERE   Idf_Motivo_Campanha_Finalizada IS NULL
                AND Idf_Vaga IS NOT NULL
                AND Idf_Pesquisa_Curriculo IS NULL
                AND Dta_Cadastro < DATEADD(HOUR, -4, GETDATE())
        ";
        #endregion

        #region Spnotificarcandidaturacampanhacriada
        private const string Spnotificarcandidaturacampanhacriada = @"
        SELECT  COUNT(*) 
        FROM    BNE_Campanha_Recrutamento CR
                INNER JOIN BNE_Campanha_Recrutamento_Curriculo CRC ON CR.Idf_Campanha_Recrutamento = CRC.Idf_Campanha_Recrutamento AND CRC.Idf_Curriculo = @Idf_Curriculo
        WHERE   1=1
                AND Idf_Vaga = @Idf_Vaga
                AND Idf_Tipo_Retorno_Campanha_Recrutamento = @Idf_Tipo_Retorno_Campanha
                AND Idf_Pesquisa_Curriculo IS NULL
        ";
        #endregion

        #region Spcurriculorecebeucampanha
        private const string Spcurriculorecebeucampanha = @"
        SELECT  COUNT(*) 
        FROM    BNE_Campanha_Recrutamento CR
                INNER JOIN BNE_Campanha_Recrutamento_Curriculo CRC ON CR.Idf_Campanha_Recrutamento = CRC.Idf_Campanha_Recrutamento AND CRC.Idf_Curriculo = @Idf_Curriculo
		        INNER JOIN BNE_Vaga_Candidato VC ON VC.Idf_Vaga = CR.Idf_Vaga AND VC.Idf_Curriculo = CRC.Idf_Curriculo
        WHERE   1=1
                AND CR.Idf_Vaga = @Idf_Vaga
                AND CR.Idf_Pesquisa_Curriculo IS NULL
        ";
        #endregion

        #region [spSelectCampanhaPorVaga]
        private const string spSelectCampanhaPorVaga = "SELECT cr.* FROM bne.BNE_Campanha_Recrutamento cr WITH(NOLOCK) WHERE idf_vaga = @Idf_Vaga";
        #endregion

        #region [spGetTelefoneUltimaCampanhaEnviada]
        private const string spGetTelefoneUltimaCampanhaEnviada = @"select top 1 cr.Num_DDD_Telefone_Contato, cr.NUM_Telefone_Contato from bne.bne_campanha_recrutamento cr with(nolock)
                                    join bne.BNE_Vaga v with(nolock) on v.Idf_Vaga = cr.Idf_Vaga
                                    where cr.Num_DDD_Telefone_Contato is not null and v.Idf_Usuario_Filial_Perfil = @Idf_Usuario_filial_Perfil 
                                    order by cr.Dta_Cadastro desc ";
        #endregion

        #endregion

        #region Métodos

        #region Salvar
        /// <summary>
        /// Fluxo para salvar uma campanha de recrutamento
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="valorSalario"></param>
        /// <param name="objFilial"></param>
        public void Salvar(string nomeFuncao, string nomeCidade, decimal valorSalario, Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {//, decimal ddd, decimal telefone, 
            Funcao objFuncao;
            if (!Funcao.CarregarPorDescricao(nomeFuncao, out objFuncao))
                throw new Exception("Função inválida");
            else
            {
                if (objFuncao.FuncaoCategoria.IdFuncaoCategoria == (int)Enumeradores.FuncaoCategoria.Gestao || objFuncao.FuncaoCategoria.IdFuncaoCategoria == (int)Enumeradores.FuncaoCategoria.Especialista)
                    throw new Exception("Esta funcionalidade está disponível apenas para funções Operacionais ou de Apoio");
            }

            Cidade objCidade;
            if (!Cidade.CarregarPorNome(nomeCidade, out objCidade))
                throw new Exception("Cidade inválida");

            if (objFilial.SaldoCampanha() == 0)
                throw new Exception("Saldo insuficiente para enviar campanha.");

            var salarioMinimo = Convert.ToDecimal(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioMinimoNacional));

            if (valorSalario < salarioMinimo)
                throw new Exception(string.Format("O salário deve ser maior que o Salário Mínimo Nacional R$ {0}", salarioMinimo));

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        UsuarioFilial objUsuarioFilial;
                        UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial, trans);

                        var objVaga = new Vaga
                        {
                            Funcao = objFuncao,
                            Cidade = objCidade,
                            ValorSalarioDe = valorSalario,
                            ValorSalarioPara = valorSalario,
                            Filial = objFilial,
                            UsuarioFilialPerfil = objUsuarioFilialPerfil,
                            FlagLiberada = !objFilial.PossuiPlanoAtivo(),
                            QuantidadeVaga = 1,
                            DescricaoAtribuicoes = objFuncao.DescricaoJob,
                            Origem = new Origem((int)Enumeradores.Origem.BNE),
                            DescricaoFuncao = objFuncao.DescricaoFuncao,
                            FlagAuditada = true,
                            DataAbertura = DateTime.Now,
                            DataPrazo = DateTime.Now.AddDays(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasPrazoVaga))),
                            FlagConfidencial = true,
                            EmailVaga = objUsuarioFilial.EmailComercial,
                            NumeroDDD = objUsuarioFilial.NumeroDDDComercial,
                            NumeroTelefone = objUsuarioFilial.NumeroComercial
                        };
                        objVaga.Save(trans, objUsuarioFilialPerfil.IdUsuarioFilialPerfil, null);

                        //Salvar Tipo Vinculo para a Vaga - Fixo Efetivo
                        VagaTipoVinculo objVagaTipoVinculo = new VagaTipoVinculo
                        {
                            TipoVinculo = TipoVinculo.LoadObject((int) Enumeradores.TipoVinculo.Efetivo),
                            Vaga = objVaga
                        };
                        objVagaTipoVinculo.Save(trans);

                        this.Vaga = objVaga;
                        this.Save(trans);
                        this.Vaga.Filial.DescontarCampanha(trans);

                        trans.Commit();

                        #region DispararCampanha
                        try
                        {
                            this.DispararGatilhoParaAssincrono();
                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex);
                        }
                        #endregion

                    }
                    catch
                    {
                        trans.Rollback();
                        throw new Exception("Houve um erro ao processar a sua campanha!");
                    }
                }
            }
        }
        #endregion

        #region QuantidadeCurriculosNaBase
        /// <summary>
        /// Recupera a quantidade estimada de curriculos para a campanha atual
        /// </summary>
        /// <returns></returns>
        private int QuantidadeCurriculosNaBase()
        {
            var url = PesquisaCurriculo.MontaURLSolrBuscaCVsCampanha(this, null);

            url = url.Replace("{pagina}", "0");
            url = url.Replace("{tamanhoPagina}", "0");

            return CurriculoIdentificador.EfetuarRequisicao(url).response.numFound;
        }
        #endregion

        #region QuantidadeCurriculosEnviados
        /// <summary>
        /// Retorna a quantidade de currículos enviados para determinada campanha
        /// </summary>
        /// <returns></returns>
        public int QuantidadeCurriculosEnviados()
        {
            return CampanhaRecrutamentoCurriculo.RecuperarQuantidadeCurriculosEnviados(this);
        }
        public int QuantidadeSMSEnviados()
        {
            return CampanhaRecrutamentoCurriculo.RecuperarQuantidadeCurriculosEnviadosSMS(this);
        }
        public int QuantidadeEmailEnviados()
        {
            return CampanhaRecrutamentoCurriculo.RecuperarQuantidadeCurriculosEnviadosEmail(this);
        }
        #endregion

        #region DispararGatilhoParaAssincrono
        public void DispararGatilhoParaAssincrono()
        {
            #region Envio para o Assíncrono
            try
            {
                var parametros = new ParametroExecucaoCollection
                            {
                                {"idCampanha", "idCampanha", this._idCampanhaRecrutamento.ToString(), this._idCampanhaRecrutamento.ToString()}
                            };

                ProcessoAssincrono.IniciarAtividade(TipoAtividade.Campanha, parametros);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            #endregion Envio para o Assincrono
        }
        public void DispararGatilhoParaAssincronoPesquisaCV()
        {
            #region Envio para o Assíncrono
            try
            {
                var parametros = new ParametroExecucaoCollection
                            {
                                {"idCampanha", "idCampanha", this._idCampanhaRecrutamento.ToString(), this._idCampanhaRecrutamento.ToString()}
                            };

                ProcessoAssincrono.IniciarAtividade(TipoAtividade.CampanhaPesquisaCurriculo, parametros);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            #endregion Envio para o Assincrono
        }
        #endregion

        #region EnvioCampanhaRecrutamento
        public bool EnvioCampanhaRecrutamento()
        {
            return this._vaga != null && this._pesquisaCurriculo == null;
        }
        #endregion

        #region CampanhasEmAberto
        public static List<CampanhaRecrutamento> CampanhasEmAberto()
        {
            var lista = new List<CampanhaRecrutamento>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcampanhasrecrutamentoemaberto, null))
            {
                var objCampanhaRecrutamento = new CampanhaRecrutamento();
                while (SetInstance(dr, objCampanhaRecrutamento, false))
                {
                    lista.Add(objCampanhaRecrutamento);
                    objCampanhaRecrutamento = new CampanhaRecrutamento();
                }
            }

            return lista;
        }
        #endregion

        #region NotificarCandidatura
        public static bool NotificarCandidatura(Vaga objVaga, Curriculo objCurriculo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga },
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                    new SqlParameter { ParameterName = "@Idf_Tipo_Retorno_Campanha", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)Enumeradores.TipoRetornoCampanhaRecrutamento.ReceberLigacao }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spnotificarcandidaturacampanhacriada, parms)) > 0;
        }
        #endregion

        #region CurriculoRecebeuCampanha
        public static bool CurriculoRecebeuCampanha(Curriculo objCurriculo, Vaga objVaga)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga },
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcurriculorecebeucampanha, parms)) > 0;
        }
        #endregion

        #region EnviarCampanhaPesquisaCV
        /// <summary>
        /// Envio de campanha de curriculos com pase na pesquisa de CV. Regras:
        ///     - Apenas pesquisa utilizando a função
        ///     - Apenas empresas com plano
        /// </summary>
        public static void EnviarCampanhaPesquisaCV(PesquisaCurriculo objPesquisaCurriculo)
        {
            try
            {
                if (objPesquisaCurriculo.UsuarioFilialPerfil == null)
                    return; //Usuário não logado, sai do processo

                if (objPesquisaCurriculo.Cidade == null)
                    return; //Caso a pesquisa não esteja filtrando função e cidade, não dispara gatilho para envio de campanha para os CVs

                if (Parametro.RecuperaValorParametro(Enumeradores.Parametro.HabilitaEnvioCampanhaPesquisaCV) != "1")
                    return; //Processo desligado

                var listaFuncao = PesquisaCurriculoFuncao.ListarIdentificadoresFuncaoPorPesquisa(objPesquisaCurriculo);

                if (listaFuncao.Count == 0)
                    return; //Nenhuma função informada

                if (!CelularSelecionador.VerificaCelularEstaLiberadoParaTanque(objPesquisaCurriculo.UsuarioFilialPerfil.IdUsuarioFilialPerfil)) //Valida se o usuário está ativo no tanque
                    return; //Usuário não usa tanque

                objPesquisaCurriculo.UsuarioFilialPerfil.CompleteObject();
                if (!objPesquisaCurriculo.UsuarioFilialPerfil.Filial.PossuiPlanoAtivo())
                    return; //Caso a filial do usuário não possua plano, não dispara gatilho para envio de campanha para os CVs

                CampanhaRecrutamento objCampanha = SalvarCampanha(objPesquisaCurriculo);

                objCampanha.DispararGatilhoParaAssincronoPesquisaCV();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na criação de campanha a partir da busca de CVs");
            }
        }

        private static CampanhaRecrutamento SalvarCampanha(PesquisaCurriculo objPesquisaCurriculo, SqlTransaction trans = null)
        {
            //Grava a Campanha de Recrutamento
            var objCampanhaRecrutamento = new CampanhaRecrutamento
            {
                QuantidadeRetorno = Convert.ToInt16(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CampanhaPesquisaCurriculoLotePadraoParaEnvio)),
                PesquisaCurriculo = objPesquisaCurriculo
            };

            if (trans != null)
                objCampanhaRecrutamento.Save(trans);
            else
                objCampanhaRecrutamento.Save();

            return objCampanhaRecrutamento;
        }
        #endregion

        #region [GetCampanhaPorVaga]
        public static CampanhaRecrutamento getCampanhaPorVaga(int idVaga)
        {
            CampanhaRecrutamento objCampanha = new CampanhaRecrutamento();
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size= 4, Value = idVaga }
            };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectCampanhaPorVaga, parms))
            {
                SetInstance(dr, objCampanha);
            }

            return objCampanha;
        }
        #endregion

        #region [GetTelefoneUltimaCampanhaEnviada]
        public static string GetTelefoneUltimaCampanhaEnviada(int idUsuarioFilialperfil, out string telefone)
        {
            string ddd = telefone = string.Empty;
            try
            {
                var parm = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Usuario_filial_Perfil", SqlDbType = SqlDbType.Int, Size=4, Value= idUsuarioFilialperfil }
            };

                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spGetTelefoneUltimaCampanhaEnviada, parm))
                {
                    if (dr.Read())
                    {
                        telefone = dr["Num_Telefone_Contato"].ToString();
                        ddd = dr["Num_DDD_Telefone_Contato"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "GetTelefoneUltimaCampanhaEnviada");
            }

            return ddd;
        }
        #endregion

        #endregion

    }
}