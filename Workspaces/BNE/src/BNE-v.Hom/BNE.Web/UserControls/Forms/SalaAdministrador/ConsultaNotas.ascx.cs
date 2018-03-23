using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL.Integracoes.WFat;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Master;
using Telerik.Web.UI;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class ConsultaNotas : Code.BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"\s{2,}");
            var identificador = regex.Replace(txtIdentificador.Text, "").Split(',');
            
            string identificadores;
            identificadores = "'" + string.Join("','", identificador.ToArray()) + "'";
            
            if (ValidaCampos(identificadores))
            { 
                var notas = NotaFiscal.VerificadorDeNotas(identificadores);

                gvNotas.DataSource = notas;
                gvNotas.DataBind();
            }
        }

        public bool ValidaCampos(string identificador)
        {   
            if (string.IsNullOrEmpty(identificador))
            {
                ExibirMensagem("Informe o identificador de pagamento para realizar a pesquisa.", TipoMensagem.Erro);
                return false;
            }
            return true;
        }

        protected void ExibirMensagem(string mensagem, TipoMensagem tipo, bool aumentarTamanhoPainelMensagem = false)
        {
            var principal = Page.Master as Principal;
            if (principal != null)
                principal.ExibirMensagem(mensagem, tipo, aumentarTamanhoPainelMensagem);
        
        }
    }
}