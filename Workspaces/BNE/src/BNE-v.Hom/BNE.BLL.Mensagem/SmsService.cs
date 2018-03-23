using BNE.BLL.Mensagem.Contracts;
using System;
using System.Collections.Generic;
using BNE.BLL.Mensagem.DTO;
using log4net;
using System.Reflection;
using BNE.BLL.Mensagem.Enumeradores;

namespace BNE.BLL.Mensagem
{
    public class SmsService : ISmsService
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CampanhaTanque _campanhaTanque = new CampanhaTanque();

        #region Enviar
        public void Enviar(DestinatarioSMS destinatario, string identificadorRemetente, bool agendar = false, int? idMesagemCampanha = null)
        {
            using (var objWsTanque = new BNETanqueService.AppClient())
            {
                List<BNETanqueService.Mensagem> listaSMS = new List<BNETanqueService.Mensagem>();

                var numeroCelular = Convert.ToDecimal(destinatario.DDDCelular.Trim() + destinatario.NumeroCelular.Trim());
                var mensagem = new BNETanqueService.Mensagem
                {
                    ci = Convert.ToString(destinatario.IdDestinatario),
                    np = destinatario.NomePessoa,
                    nc = numeroCelular,
                    dm = destinatario.Mensagem
                };

                listaSMS.Add(mensagem);

                var receberMensagem = new BNETanqueService.InReceberMensagem
                {
                    l = listaSMS.ToArray(),
                    cu = identificadorRemetente,
                    e = agendar,
                    idMsgCampanha = idMesagemCampanha
                };

                try
                {
                    var retorno = objWsTanque.ReceberMensagem(receberMensagem);


                    if (!string.IsNullOrEmpty(retorno.me))
                    {
                        throw new Exception(retorno.me);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Erro ao enviar SMS", ex);
                }

            }
        }
        #endregion

        #region Enviar
        /// <summary>
        /// Método utilizado para fazer o envio de sms via tanque
        /// </summary>
        /// <param name="destinatarios">Lista de destinatarios para envio de sms</param>
        /// <param name="idUsuarioFilialPerfil">Id usuário filiar perfil remetente</param>
        /// <param name="agendar">agendamento do sms</param>
        public void Enviar(List<DestinatarioSMS> destinatarios, string identificadorRemetente, bool agendar = false, int? idMesagemCampanha = null)
        {
            using (var objWsTanque = new BNETanqueService.AppClient())
            {
                List<BNETanqueService.Mensagem> listaSMS = new List<BNETanqueService.Mensagem>();

                foreach (var destinatario in destinatarios)
                {
                    var numeroCelular = Convert.ToDecimal(destinatario.DDDCelular.Trim() + destinatario.NumeroCelular.Trim());
                    var mensagem = new BNETanqueService.Mensagem
                    {
                        ci = Convert.ToString(destinatario.IdDestinatario),
                        np = destinatario.NomePessoa,
                        nc = numeroCelular,
                        dm = destinatario.Mensagem
                    };

                    listaSMS.Add(mensagem);
                }

                var receberMensagem = new BNETanqueService.InReceberMensagem
                {
                    l = listaSMS.ToArray(),
                    cu = identificadorRemetente,
                    e = agendar,
                    idMsgCampanha = idMesagemCampanha
                };

                try
                {
                    objWsTanque.ReceberMensagem(receberMensagem);
                }
                catch (Exception ex)
                {
                    _logger.Error("Erro ao enviar SMS", ex);
                }

            }
        }

        public CampanhaTanqueMensagemDTO GetTemplate(Enumeradores.CampanhaTanque campanha)
        {
            try
            {
                return _campanhaTanque.GetTextoCampanha(campanha);
            }
            catch (Exception ex)
            {
                _logger.Error("Erro GetTemplate()", ex);
                return null;
            }


        }
        #endregion Enviar
    }
}
