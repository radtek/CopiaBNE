using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web.Promocoes.BlackFriday
{
    public partial class BlackFriday : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbTipo.SelectedValue == "0")//candidato
                {
                    PreCadastro oPre = new PreCadastro();
                    oPre.nome = txtNome.Text.Trim();
                    oPre.email = txtEmail.Text.Trim();
                    oPre.idOrigemPreCadastro = (int)BNE.BLL.Enumeradores.OrigemPreCadastro.BlackFriday;
                    oPre.Save();
                }
                else
                {
                    PreCadastroEmpresa oPre = new PreCadastroEmpresa();
                    oPre.nome = txtNome.Text.Trim();
                    oPre.email = txtEmail.Text.Trim();
                    oPre.idOrigemPreCadastro = (int)BNE.BLL.Enumeradores.OrigemPreCadastro.BlackFriday;
                    oPre.Save();
                }
                ScriptManager.RegisterStartupScript(upPagina, upPagina.GetType(), "modal", "modal();", true);
              

            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(upPagina, upPagina.GetType(), "modalErro", "modalErro();", true);
            }
           

        }
    }
}