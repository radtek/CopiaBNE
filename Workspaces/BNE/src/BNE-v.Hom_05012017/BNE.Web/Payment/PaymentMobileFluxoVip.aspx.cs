   using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom;
using BNE.Web.Master;
using System.Web.Services;
using BNE.BLL.Integracoes.Pagamento;
using System.Web;

namespace BNE.Web.Payment
{
    public partial class PaymentMobileFluxoVip : BasePagePagamento
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IdPessoaFisicaLogada.HasValue && base.IdCurriculo.HasValue && base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                if (PlanoAdquirido.ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC(new PessoaFisica(base.IdPessoaFisicaLogada.Value)))
                    Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileRegistered.aspx"));
                else if (PlanoAdquirido.CarregaListaPlanoAdquiridoLiberadoOuEmAberto(base.IdUsuarioFilialPerfilLogadoCandidato.Value)
                            .Count(u => u.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado) > 0)
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaVIP.ToString(), null));
                else
                {
                    
                    txtPessoaFisica.Value = base.IdPessoaFisicaLogada.Value.ToString();
                    txtCurriculo.Value = base.IdCurriculo.Value.ToString();
                    txtUsuarioFilialPerfil.Value = base.IdUsuarioFilialPerfilLogadoCandidato.Value.ToString();

                    //Valida origem STC
                    OrigemFilial objOrigemFilial = null;
                    if (base.STC.Value && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial) &&
                    (objOrigemFilial != null && objOrigemFilial.Filial.PossuiSTCUniversitario()))
                        txtUniversitarioSTC.Value = "1";
                    else
                        txtUniversitarioSTC.Value = "0";

                }
            }
        }
        #endregion
    }
}