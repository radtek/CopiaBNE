using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;

namespace BNE.Web
{
    public partial class CIAProduto : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (base.IdFilial.HasValue && (new Filial(base.IdFilial.Value).EmpresaBloqueada()))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._505705);
                    Redirect("Default.aspx");
                }
            }

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "CIAProduto");
        }
    }
}