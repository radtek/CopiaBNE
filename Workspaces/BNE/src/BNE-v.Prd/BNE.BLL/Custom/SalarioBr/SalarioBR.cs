using BNE.BLL.Custom.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.Custom.SalarioBr
{
    public class SalarioBR
    {

        #region [Metodos]

        #region [EnviarPropaganda]
        public static void EnviarPropaganda(int idPessoaFisica, int idFilial)
        {
            
          string emailDestinatario = UsuarioFilialContato.EmailComercialPorIdPessoaFisica(idPessoaFisica, idFilial);
            if (!string.IsNullOrEmpty(emailDestinatario))
            {
                DateTime? ultimoEnvio = LogEnvioMensagem.UltimoEnvio(emailDestinatario, Enumeradores.CartaEmail.PropagandaSalarioBR);
                if (!ultimoEnvio.HasValue || ultimoEnvio.Value < DateTime.Now.AddDays(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DiasEnvioPropagandaSalarioBr))))
                {
                    Filial objFilial = Filial.LoadObject(idFilial);
                    PessoaFisica objPessoa = PessoaFisica.LoadObject(idPessoaFisica);

                    #region [MontarCarta]
                    String emailRemetente = objFilial.Vendedor().EmailVendedor;
                    var objCarta = CartaEmail.LoadObject((int)Enumeradores.CartaEmail.PropagandaSalarioBR);

                    objCarta.ValorCartaEmail = objCarta.ValorCartaEmail.Replace("{Nome_Completo}", objPessoa.NomeCompleto);
                    objCarta.ValorCartaEmail = objCarta.ValorCartaEmail.Replace("{Assinatura_Vendedor}", objFilial.Vendedor().NomeVendedor);
                    objCarta.DescricaoAssunto = objCarta.DescricaoAssunto.Replace("{Primeiro_Nome}", Helper.RetornarPrimeiroNome(objPessoa.NomeCompleto));
                    #endregion

                    EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(objCarta.DescricaoAssunto,
                            objCarta.ValorCartaEmail, Enumeradores.CartaEmail.PropagandaSalarioBR, emailRemetente,
                            emailDestinatario);

                }
            }
        }
        #endregion

        #endregion
    }
}
