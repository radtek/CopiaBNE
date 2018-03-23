//-- Data: 18/09/2013 15:11
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class CelularSelecionador // Tabela: BNE_Celular_Selecionador
    {

        #region Consultas

        #region Spverificacelularliberadoporselecionador
        private const string Spverificacelularliberadoporselecionadorparatanque = @"
        SELECT  COUNT(*)
        FROM    BNE_Celular C WITH (NOLOCK)
                INNER JOIN BNE_Celular_Selecionador CS WITH (NOLOCK) ON C.Idf_Celular = CS.Idf_Celular
        WHERE   CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
		        AND CONVERT(VARCHAR, CS.Dta_Inicio_Utilizacao, 112) <= CONVERT(VARCHAR, GETDATE(), 112)
		        AND (CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) IS NULL OR CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) >= CONVERT(VARCHAR, GETDATE(), 112))
                AND CS.Flg_Utiliza_Servico_Tanque = 1";
        #endregion


        #region SpRecuperarCelularSelecionadorTanque
        private const string SpRecuperarCelularSelecionadorTanque = @"
        SELECT  CS.*
        FROM    BNE_Celular_Selecionador CS WITH (NOLOCK)
        WHERE   CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
		        AND CS.Flg_Utiliza_Servico_Tanque = 1";
        #endregion

        #region SpRecuperarCelularSelecionador
        private const string SpRecuperarCelularSelecionador = @"
        SELECT  TOP 1 CS.*
        FROM    BNE_Celular_Selecionador CS WITH (NOLOCK)
        WHERE   CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
		        AND CS.Flg_Utiliza_Servico_Tanque = 1
                AND (CS.Dta_Fim_Utilizacao IS NULL OR CS.Dta_Fim_Utilizacao > GetDate())
        ";
        #endregion

        #endregion

        #region VerificaCelularEstaLiberado
        public static bool VerificaCelularEstaLiberadoParaTanque(int IdUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = IdUsuarioFilialPerfil }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spverificacelularliberadoporselecionadorparatanque, parms)) > 0;
        }
        #endregion

        #region RecuperarCelularSelecionador
        public static CelularSelecionador RecuperarCelularSelecionadorTanque(int IdUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = IdUsuarioFilialPerfil }
                };

            var objCelularSelecionador = new CelularSelecionador();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarCelularSelecionadorTanque, parms))
            {
                if (!SetInstance(dr, objCelularSelecionador))
                    objCelularSelecionador = null;
            }

            return objCelularSelecionador;
        }
        #endregion

        #region RecuperarCelularSelecionadorByCodigo
        public static CelularSelecionador RecuperarCelularSelecionadorByCodigo(int UsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 8, Value = UsuarioFilialPerfil }
                };

            var objCelularSelecionador = new CelularSelecionador();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarCelularSelecionador, parms))
            {
                if (!SetInstance(dr, objCelularSelecionador))
                    objCelularSelecionador = null;
            }

            return objCelularSelecionador;
        }
        #endregion RecuperarCelularSelecionador

        #region RecuperarCotaDisponivel
        /// <summary>
        /// Recupera a quantidade de mensagens disponíveis do usuário
        /// </summary>
        /// <returns></returns>
        public static int RecuperarCotaDisponivel(int IdUsuarioFilialPerfil, int IdFilial)
        {
            try
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = IdUsuarioFilialPerfil }
                };
                int quantidadeCotaTanque = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "TANQUE_Recuperar_Cota_Disponivel", parms));

                Filial oFilial = Filial.LoadObject(IdFilial);
                int saldoVisualizacao = oFilial.SaldoVisualizacao();

                if (quantidadeCotaTanque < saldoVisualizacao)
                    return quantidadeCotaTanque;

                return saldoVisualizacao;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha ao retornar cota de SMS usuario - Tanque e Plano");
                return 0;
            }
        }
        public static int RecuperarCotaDisponivelEmpresa(int IdFilial)
        {
            try
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = IdFilial }
                };
                int quantidadeCotaTanque = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "TANQUE_Recuperar_Cota_Disponivel_Empresa", parms));

                Filial oFilial = Filial.LoadObject(IdFilial);
                int saldoVisualizacao = oFilial.SaldoVisualizacao();

                if (quantidadeCotaTanque < saldoVisualizacao)
                    return quantidadeCotaTanque;
                
                return saldoVisualizacao;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha ao retornar cota de SMS empresa - Tanque e Plano");
                return 0;
            }
        }
        #endregion

        #region RecuperarCotaPadrao
        /// <summary>
        /// Recupera a quantidade de mensagens disponíveis do usuário
        /// </summary>
        /// <returns></returns>
        public static int RecuperarCotaPadrao()
        {
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "TANQUE_Recuperar_Cota_Padrao", null));
        }
        #endregion

        #region Criar
        public static CelularSelecionador Criar(UsuarioFilialPerfil objUsuarioFilialPerfil, DateTime dataInicio, DateTime? dataFim)
        {
            var objCelularSelecionador = new CelularSelecionador()
            {
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Celular = Celular.LoadObject(1),
                DataInicioUtilizacao = dataInicio,
                DataFimUtilizacao = dataFim,
                FlagUtilizaServicoTanque = true
            };
            objCelularSelecionador.Insert();

            return objCelularSelecionador;
        }
        #endregion

        #region HabilitarUsuario
        private static void HabilitarUsuario(UsuarioFilialPerfil objUsuarioFilialPerfil, DateTime dataInicio, DateTime? dataFim, string numeroDDD)
        {
            HabilitarDesabilitarUsuario(objUsuarioFilialPerfil, dataInicio, dataFim, numeroDDD, true);
        }
        private static void DesabilitarUsuario(UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            HabilitarDesabilitarUsuario(objUsuarioFilialPerfil, null, null, string.Empty, false);
        }
        private static void HabilitarDesabilitarUsuario(UsuarioFilialPerfil objUsuarioFilialPerfil, DateTime? dataInicio, DateTime? dataFim, string numeroDDD, bool habilitar)
        {
            try
            {
                var objCelularSelecionador = RecuperarCelularSelecionadorTanque(objUsuarioFilialPerfil.IdUsuarioFilialPerfil);

                if (objCelularSelecionador != null)
                {
                    if (habilitar)
                    {
                        if (objCelularSelecionador.DataFimUtilizacao < dataFim)
                        {
                            objCelularSelecionador.DataFimUtilizacao = dataFim;
                            objCelularSelecionador.Update();
                        }
                    }
                    else
                    {
                        objCelularSelecionador.DataFimUtilizacao = DateTime.Today.AddDays(-1);
                        objCelularSelecionador.Update();
                    }
                }
                else
                {
                    if (habilitar)
                        Criar(objUsuarioFilialPerfil, dataInicio.Value, dataFim);
                }

                using (var objWsTanque = new BNETanqueService.AppClient())
                {
                    //objWsTanque.Endpoint.Address = new System.ServiceModel.EndpointAddress("http://wstg.apprh.com.br/celulardaselecionadora/App.svc/mex");
                    var objIn = new BNETanqueService.InHabilitarUsuario
                    {
                        n = objUsuarioFilialPerfil.PessoaFisica.NomeCompleto,
                        c = objUsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString(),
                        di = dataInicio,
                        df = dataFim,
                        ddd = numeroDDD.Trim(),
                        h = habilitar
                    };

                    var retorno = objWsTanque.HabilitarUsuario_New(objIn);
                    if (retorno.s == BNETanqueService.TipoRetorno.erro)
                    {
                        EL.GerenciadorException.GravarExcecao(new Exception(), "Tanque - Falha no processo de habilitar usuario tanque: " + retorno.erro);
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Tanque - HabilitarDesabilitarUsuario");
            }
        }
        /// <summary>
        /// Método utilizado para inserir novos usuário no tanque, para que possua cota mínima de envio de SMS a usuários VIP
        /// </summary>
        private static void HabilitarUsuarioCotaMinima(UsuarioFilialPerfil objUsuarioFilialPerfil, string numeroDDD)
        {
            try
            {
                int qtdCota = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeSMSDiarioEmpresaChupaVIP));
                DateTime dataInicio = DateTime.Now;
                DateTime dataFim = DateTime.Now.AddDays(30);

                var objCelularSelecionador = RecuperarCelularSelecionadorTanque(objUsuarioFilialPerfil.IdUsuarioFilialPerfil);

                if (objCelularSelecionador != null)
                    return;

                Criar(objUsuarioFilialPerfil, dataInicio, dataFim);

                using (var objWsTanque = new BNETanqueService.AppClient())
                {
                    var objIn = new BNETanqueService.InHabilitarUsuario
                    {
                        n = objUsuarioFilialPerfil.PessoaFisica.NomeCompleto,
                        c = objUsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString(),
                        di = dataInicio,
                        df = dataFim,
                        ddd = numeroDDD.Trim(),
                        h = true,
                        qtd = qtdCota
                    };

                    var retorno = objWsTanque.HabilitarUsuario_New(objIn);
                    if (retorno.s == BNETanqueService.TipoRetorno.erro)
                    {
                        EL.GerenciadorException.GravarExcecao(new Exception(), "Tanque - Falha no processo de habilitar usuario tanque cota minima: " + retorno.erro);
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Tanque - HabilitarUsuarioCotaMinima");
            }
        }
        #endregion

        #region DesabilitarUsuarios
        /// <summary>
        /// Desabilita todos os usuários do tanque para esta filial
        /// </summary>
        /// <param name="objFilial"></param>
        public static void DesabilitarUsuarios(Filial objFilial)
        {
            var listaUsuario = BLL.UsuarioFilialPerfil.ListarUsuariosFilial(objFilial);
            foreach (var objUsuarioFilialPerfil in listaUsuario)
            {
                DesabilitarUsuario(objUsuarioFilialPerfil);
            }
        }
        #endregion

        #region HabilitarUsuarios
        /// <summary>
        /// Habilita os usuários do tanque para esta filial
        /// Se não for altera a cota (usuario de empresa sem plano liberado) não passar valor mantera o default
        /// </summary>
        /// <param name="objFilial"></param>
        private static void HabilitarUsuarios(Filial objFilial, int? AlterarCotaValor)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido;
                if (PlanoAdquirido.CarregarPlanoVigentePessoaJuridicaPorSituacao(objFilial, new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado), out objPlanoAdquirido))
                {
                    //Empresa com plano, ativa os usuários ativo no tanque
                    objPlanoAdquirido.Plano.CompleteObject();
                    if (objPlanoAdquirido.Plano.FlagLiberaUsuariosTanque)
                    {
                        var listaUsuario = BLL.UsuarioFilialPerfil.ListarUsuariosFilial(objFilial);

                        foreach (var objUsuarioFilialPerfil in listaUsuario)
                        {
                            if (objUsuarioFilialPerfil.FlagInativo)
                                DesabilitarUsuario(objUsuarioFilialPerfil);
                            else
                            {
                                string numeroDDD = string.Empty;
                                UsuarioFilial objUsuarioFilial;
                                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                                    numeroDDD = objUsuarioFilial.NumeroDDDComercial;

                                HabilitarUsuario(objUsuarioFilialPerfil, objPlanoAdquirido.DataInicioPlano, objPlanoAdquirido.DataFimPlano, numeroDDD);

                                //se não for altera a cota (usuario de empresa sem plano liberado) não passar valor mantera o default
                                if (AlterarCotaValor.HasValue)
                                    AlterarCotaTanque(objUsuarioFilialPerfil, AlterarCotaValor.Value);

                            }
                        }
                    }
                }
                else
                {
                    //Empresa sem plano. 
                    // 1 - Usuários novos e ativos ganham acesso de 30 dias no tanque. 
                    // 2 - Usuário que já tem acesso ao tanque continua com o acesso já cadastrado

                    var listaUsuario = BLL.UsuarioFilialPerfil.ListarUsuariosFilial(objFilial);
                    foreach (var objUsuarioFilialPerfil in listaUsuario)
                    {
                        if (objUsuarioFilialPerfil.FlagInativo)
                            DesabilitarUsuario(objUsuarioFilialPerfil);
                        else
                        {
                            string numeroDDD = string.Empty;
                            UsuarioFilial objUsuarioFilial;
                            if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                                numeroDDD = objUsuarioFilial.NumeroDDDComercial;

                            HabilitarUsuarioCotaMinima(objUsuarioFilialPerfil, numeroDDD);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region HabilitarDesabilitarUsuarios
        public static void HabilitarDesabilitarUsuarios(Filial objFilial, int? cotaValor = null)
        {
            switch (objFilial.SituacaoFilial.IdSituacaoFilial)
            {
                case (int)Enumeradores.SituacaoFilial.PublicadoAgencia:
                case (int)Enumeradores.SituacaoFilial.PublicadoEmpresa:
                    HabilitarUsuarios(objFilial, cotaValor);
                    break;
                case (int)Enumeradores.SituacaoFilial.AguardandoPublicacao:
                case (int)Enumeradores.SituacaoFilial.ComCritica:
                case (int)Enumeradores.SituacaoFilial.Bloqueado:
                case (int)Enumeradores.SituacaoFilial.Cancelado:
                    DesabilitarUsuarios(objFilial);
                    break;
            }
        }
        #endregion


        #region [Alterar Cota Tanque]
        public static void AlterarCotaTanque(UsuarioFilialPerfil objUsuarioFilialPerfil, int cota)
        {

            using (var objWsTanque = new BNETanqueService.AppClient())
            {
                //objWsTanque.Endpoint.Address = new System.ServiceModel.EndpointAddress("http://wstg.apprh.com.br/celulardaselecionadora/App.svc/mex");
                var objIn = new BNETanqueService.InAtualizarCota
                {
                    n = objUsuarioFilialPerfil.PessoaFisica.NomeCompleto,
                    c = objUsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString(),
                    Cota = cota
                };
                
                var retorno = objWsTanque.AtualizarCota(objIn);
                if (retorno.s == BNETanqueService.TipoRetorno.erro)
                {
                    EL.GerenciadorException.GravarExcecao(new Exception(), "Tanque - Falha no processo de Atualizar Cota do usuario: " + retorno.erro);
                }
            }
        }
        #endregion
    }
}