//-- Data: 18/09/2013 15:11
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using BNE.BLL.Custom;

namespace BNE.BLL
{
    public partial class Campanha // Tabela: BNE_Campanha
    {

        #region Consultas

        #region Spconfirmarenvio
        private const string Spconfirmarenvio = "UPDATE BNE_Campanha SET Dta_Envio = GETDATE() WHERE Idf_Campanha = @Idf_Campanha";
        #endregion

        #region Sprecuperarcampanhaaenviar
        private const string Sprecuperarcampanhaaenviar = @"
        SELECT  Cam.* 
        FROM    BNE_Campanha Cam 
                INNER JOIN BNE_Celular_Selecionador CS ON Cam.Idf_Celular_Selecionador = CS.Idf_Celular_Selecionador
                INNER JOIN BNE_Celular Cel ON CS.Idf_Celular = Cel.Idf_Celular
        WHERE   Cam.Dta_Envio IS NULL
                AND Cel.Cod_Imei_Celular LIKE @imei
        ORDER BY Cam.Dta_Cadastro ASC";
        #endregion

        #region Sprecuperaridcampanha
        private const string Sprecuperaridcampanhaeimei = @"
        SELECT  Cam.* 
        FROM    BNE_Campanha Cam 
                INNER JOIN BNE_Celular_Selecionador CS ON Cam.Idf_Celular_Selecionador = CS.Idf_Celular_Selecionador
                INNER JOIN BNE_Celular Cel ON CS.Idf_Celular = Cel.Idf_Celular
        WHERE   Cam.Dta_Envio IS NULL
                AND Cel.Cod_Imei_Celular LIKE @imei
				AND Cam.Idf_Campanha = @Idf_Campanha
        ORDER BY Cam.Dta_Cadastro ASC";
        #endregion

        #region Spquantidadecurriculosenviadoshoje
        private const string Spquantidadecurriculosenviadoshoje = @"
        SELECT  COUNT(CC.Idf_Curriculo) 
        FROM    BNE_Campanha Cam 
                INNER JOIN BNE_Celular_Selecionador CS ON Cam.Idf_Celular_Selecionador = CS.Idf_Celular_Selecionador
                INNER JOIN BNE_Campanha_Curriculo CC ON Cam.Idf_Campanha = CC.Idf_Campanha
        WHERE   CONVERT(VARCHAR, Cam.Dta_Envio, 112) = CONVERT(VARCHAR, GETDATE(), 112)
                AND CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil";
        #endregion

        #region SpRecuperarMensagemCampanha
        private const string SpRecuperarMensagemCampanha = @"
        select top 1 c.Dta_Cadastro
	        , c.Des_Mensagem
            , c.Idf_Campanha
        from bne.bne_campanha c with(nolock)
	        join bne.BNE_Campanha_Curriculo ccv with(nolock) on c.Idf_Campanha = ccv.Idf_Campanha
	        join bne.BNE_Celular_Selecionador cls with(nolock) on c.Idf_Celular_Selecionador = cls.Idf_Celular_Selecionador
	        join bne.TAB_Usuario_Filial_Perfil ufp with(nolock) on cls.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
        where cls.Idf_Usuario_Filial_Perfil = @Idf_usuario_Filial_Perfil
	        and ccv.Idf_Curriculo = @Idf_Curriculo
        order by c.Idf_Campanha desc
        ";
        #endregion

        #endregion

        #region Métodos

        #region ConfirmarEnvio
        /// <summary>
        /// Confirma o envio da campanha informando a data do seu envio
        /// </summary>
        /// <returns></returns>
        public bool ConfirmarEnvio()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Campanha", SqlDbType = SqlDbType.Int, Size = 4, Value = this.IdCampanha }
                };

            try
            {
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spconfirmarenvio, parms);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return false;
            }
            return true;
        }
        #endregion

        #region RecuperaCampanhaAEnviar
        /// <summary>
        /// Recupera a primeira campanha a enviar
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static Campanha RecuperaCampanhaAEnviar(string imei)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@imei", SqlDbType = SqlDbType.VarChar, Size = 200, Value = imei }
                };

            var objCampanha = new Campanha();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarcampanhaaenviar, parms))
            {
                if (!SetInstance(dr, objCampanha))
                    objCampanha = null;
            }

            return objCampanha;
        }
        #endregion

        #region RecuperaCampanhaAEnviarIdCampanha
        /// <summary>
        /// Recupera a primeira campanha a enviar
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static Campanha RecuperaCampanhaAEnviarIdCampanha(string imei, int idCampanha)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@imei", SqlDbType = SqlDbType.VarChar, Size = 200, Value = imei },
                    new SqlParameter{ ParameterName = "@Idf_Campanha", SqlDbType = SqlDbType.Int, Value = idCampanha }
                };

            var objCampanha = new Campanha();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperaridcampanhaeimei, parms))
            {
                if (!SetInstance(dr, objCampanha))
                    objCampanha = null;
            }

            return objCampanha;
        }
        #endregion

        #region QuantidadeCurriculosEnviadosHoje
        public static int QuantidadeCurriculosEnviadosHoje(UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadecurriculosenviadoshoje, parms));
        }
        #endregion

        #region SalvarCampanha
        public static bool SalvarCampanha(UsuarioFilialPerfil objUsuarioFilialPerfil, string nomeCampanha, string mensagem, List<CampanhaCurriculo> listaCampanhaCurriculo, out int idCampanha, out string erroOuObservacao)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var objCampanha = new Campanha
                        {
                            NomeCampanha = nomeCampanha,
                            DescricaoMensagem = mensagem
                        };

                        var objCelularSelecionador = CelularSelecionador.RecuperarCelularSelecionador(objUsuarioFilialPerfil.IdUsuarioFilialPerfil);
                        if (objCelularSelecionador == null)
                            throw new Exception("Selecionador não possui celular liberado!");

                        objCampanha.CelularSelecionador = objCelularSelecionador;
                        objCampanha.Save(trans);

                        var token = objCelularSelecionador.RecuperarToken();

                        foreach (var objCampanhaCurriculo in listaCampanhaCurriculo)
                        {
                            objCampanhaCurriculo.Campanha = objCampanha;
                            objCampanhaCurriculo.Save(trans);
                        }

                        trans.Commit();

                        idCampanha = objCampanha.IdCampanha;

                        if (!objCelularSelecionador.FlagUtilizaServicoTanque)
                        {
                            objCampanha.NotificarCelular(token);
                            erroOuObservacao = "";
                            return true;
                        }

                        return EnviaMensagemTanque(objUsuarioFilialPerfil, mensagem, listaCampanhaCurriculo, out erroOuObservacao);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }
            }
        }

        private static bool EnviaMensagemTanque(UsuarioFilialPerfil objUsuarioFilialPerfil, string mensagem,
            List<CampanhaCurriculo> listaCampanhaCurriculo, out string erroOuObservacao)
        {
            var dto = new BNETanqueService.InReceberMensagem();
            dto.cu = objUsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString(CultureInfo.InvariantCulture);
            dto.e = true;
            
            Func<string, string, decimal> parseCelular = (a, b) =>
            {
                decimal ddd;
                if (!decimal.TryParse(a, out ddd))
                {
                    throw new InvalidCastException("ddd");
                }

                decimal celular;
                if (!decimal.TryParse(b, out celular))
                {
                    throw new InvalidCastException("celular");
                }

                if (ddd > celular ||
                    celular <= 0)
                    return Convert.ToDecimal(ddd.ToString(CultureInfo.InvariantCulture) + celular.ToString(CultureInfo.InvariantCulture));

                int countDecimal = 1;
                decimal aux = celular;
                while ((aux = aux / 10) >= 1)
                {
                    countDecimal++;
                }

                var toSum = Math.Pow(10, countDecimal);
                decimal totalNumber = ddd * Convert.ToDecimal(toSum);
                totalNumber = totalNumber + celular;
                return totalNumber;
            };

            var mensagens = listaCampanhaCurriculo.Select(
                a =>
                    new BNETanqueService.Mensagem
                    {
                        ci = a.Curriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture),
                        dcm = DateTime.Now,
                        dm = new { nome = a.NomePessoa }.ToString(mensagem),
                        nc = parseCelular(a.NumeroDDDCelular, a.NumeroCelular),
                        np = a.NomePessoa
                    }).ToArray();

            dto.l = mensagens;

            BNETanqueService.OutReceberMensagem envioMensagemResult;
            using (var client = new BNETanqueService.AppClient())
            {
                envioMensagemResult = client.ReceberMensagem(dto);
            }

            if (envioMensagemResult == null)
            {
                EL.GerenciadorException.GravarExcecao(new NullReferenceException("envioMensagemResult"));
                erroOuObservacao = "Falha na comunicação com o serviço de envio de SMS, contate administrador.";
                return false;
            }

            if (envioMensagemResult.l != null)
            {
                for (int index = 0; index < mensagens.Length; index++)
                {
                    //Tratamento para o chat da selecionadora com cada CV
                    var item = mensagens[index];
                    int idCurriculo = Convert.ToInt32(item.ci);
                    var res = envioMensagemResult.l.ElementAtOrDefault(index);

                    var pair = BNE.Chat.Core.ChatService.Instance.ChatStore.AddPrivateMessage(idCurriculo,
                                                                     objUsuarioFilialPerfil.IdUsuarioFilialPerfil,
                                                                     res == 0
                                                                         ? Guid.NewGuid().ToString()
                                                                         : res.ToString(CultureInfo.InvariantCulture),
                                                                     true);
                    if (pair.Value != null)
                    {
                        pair.Value.StatusTypeValue = -2;
                        pair.Value.MessageContent = item.dm;
                    }

                    //Atualizando o Solr sobre o envio de SMS para o CV
                    MensagemCS.EnviarSmsSolr(idCurriculo);
                }
            }

            if (!string.IsNullOrEmpty(envioMensagemResult.me))
            {
                erroOuObservacao = envioMensagemResult.me;
                return false;
            }

            if (envioMensagemResult.qtdDisp > 0)
            {
                erroOuObservacao = string.Empty;
                return true;
            }

            if (listaCampanhaCurriculo.Count <= (envioMensagemResult.qtdDisp * -1))
            {
                if (listaCampanhaCurriculo.Count == 1)
                    erroOuObservacao =
                        string.Format(
                            "A mensagem foi agendada para envio amanhã. Atenção! A cota diária já foi excedida. Observação: Existem {0} mensagens agendadas.",
                            envioMensagemResult.qtdAgendadaAmanha);
                else
                    erroOuObservacao =
                        string.Format(
                            "As mensagens foram agendadas para envio amanhã. Atenção! A cota diária já foi excedida. Observação: Existem {0} mensagens agendadas.",
                            envioMensagemResult.qtdAgendadaAmanha);

                return true;
            }

            var totalEnviado = listaCampanhaCurriculo.Count - (envioMensagemResult.qtdDisp * -1);
            if (totalEnviado == 1)
            {
                erroOuObservacao =
                    string.Format(
                        "1 mensagem foi enviada com sucesso. Observação: A cota diária foi excedida, {0} mensagens estão agendadas para serem enviadas amanhã.",
                        envioMensagemResult.qtdAgendadaAmanha);
            }
            else
            {
                erroOuObservacao =
                    string.Format(
                        "{0} mensagens foram enviadas com sucesso. Observação: A cota diária foi excedida, {1} mensagens estão agendadas para serem enviadas amanhã.",
                        totalEnviado,
                        envioMensagemResult.qtdAgendadaAmanha);
            }
            return true;

        }

        #endregion

        #region NotificarCelular
        public void NotificarCelular(string dispositivo)
        {
            try
            {
                var listaParametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.NotificacaoPushAndroidURL,
                        Enumeradores.Parametro.NotificacaoPushAndroidKey
                    };

                var parametros = Parametro.ListarParametros(listaParametros);

                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

                var parametrosRequisicao = "{ \"registration_ids\": [ \"" + dispositivo + "\" ], \"data\": {\"idCampanha\":\"" + _idCampanha + "\"}}";

                var requestHeader = string.Format("Authorization: key={0}", parametros[Enumeradores.Parametro.NotificacaoPushAndroidKey]);
                Custom.Helper.EfetuarRequisicao(parametros[Enumeradores.Parametro.NotificacaoPushAndroidURL], parametrosRequisicao, "application/json", requestHeader);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        public static bool RecuperarMensagemCampanha(int idUsuarioFilialPerfil, int idCurriculo, out int idCampanha, out DateTime dataCadastro, out string descricaoMensagem)
        {
            dataCadastro = DateTime.Now;
            descricaoMensagem = string.Empty;
            idCampanha = 0;

            bool retorno = false;

            var parms = new List<SqlParameter>()
            {
                new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 8, Value = idCurriculo},
                new SqlParameter{ ParameterName= "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 8, Value = idUsuarioFilialPerfil}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarMensagemCampanha, parms))
            {
                if (dr.Read())
                {
                    dataCadastro = Convert.ToDateTime(dr["dta_cadastro"]);
                    descricaoMensagem = dr["Des_Mensagem"].ToString();
                    idCampanha = Convert.ToInt32(dr["Idf_Campanha"].ToString());
                    retorno = true;
                }
            }

            return retorno;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion


    }
}