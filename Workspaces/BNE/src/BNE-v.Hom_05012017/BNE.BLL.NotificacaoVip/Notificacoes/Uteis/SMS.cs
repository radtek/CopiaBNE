using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.NotificacoesVip.Notificacoes.Uteis
{
    internal class SMS
    {
        public string _nome { get; set; }
        public decimal _numero { get; set; }
        public string _mensagem { get; set; }
        public int _idCV { get; set; }

        public SMS(string nome, decimal numero, string mensagem, int idCurriculo)
        {
            _nome = nome;
            _numero = numero;
            _mensagem = mensagem;
            _idCV = idCurriculo;
        }

        public void Enviar()
        {
            using (var objWsTanque = new BNETanqueService.AppClient())
            {
                List<BNETanqueService.Mensagem> listaSMS = new List<BNETanqueService.Mensagem>();

                var mensagem = new BNETanqueService.Mensagem
                {
                    ci = _idCV.ToString(),
                    np = _nome,
                    nc = _numero,
                    dm = _mensagem
                };

                listaSMS.Add(mensagem);

                var receberMensagem = new BNETanqueService.InReceberMensagem
                {
                    l = listaSMS.ToArray(),
                    cu = "EnvioNotificacoesVIP",
                };

                try
                {
                    var retorno = objWsTanque.ReceberMensagem(receberMensagem);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
        }
    }
}
