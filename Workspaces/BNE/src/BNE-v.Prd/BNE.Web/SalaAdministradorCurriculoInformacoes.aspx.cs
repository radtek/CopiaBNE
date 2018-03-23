using BNE.BLL;
using BNE.Web.Code;
using System;
using System.Data;
using System.Web.UI;

namespace BNE.Web
{
    public partial class SalaAdministradorCurriculoInformacoes : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                Redirect(GetRouteUrl(BLL.Enumeradores.RouteCollection.Default.ToString(), null));


            if (!Page.IsPostBack)
            {
                Iniciar();
            }
        }

        private void Iniciar()
        {


            var idCurriculo = RouteData.Values["Idf_Curriculo"];
            var objPessoaFisica = PessoaFisica.LoadObject(PessoaFisica.RecuperarIdPorCurriculo(new Curriculo(Convert.ToInt32(idCurriculo))));
            lblNome.Text = $"{objPessoaFisica.NomeCompleto} {BLL.Custom.Helper.FormatarCPF(objPessoaFisica.CPF)}";

            hdfCpf.Value = objPessoaFisica.CPF.ToString().PadLeft(11, '0');


            CarregarInformacoes();
        }

        protected void lnkFiltro_Click(object sender, EventArgs e)
        {
            CarregarInformacoes();
        }


        private void CarregarInformacoes()
        {
            var idCurriculo = RouteData.Values["Idf_Curriculo"];

            if (string.IsNullOrEmpty(hdfDtaInicio.Value))
                txtDataInicio.Text = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
            else
                txtDataInicio.Text = hdfDtaInicio.Value;
            if (string.IsNullOrEmpty(hdfDtaFim.Value))
                txtDataFim.Text = DateTime.Now.ToString("dd/MM/yyyy");
            else
                txtDataFim.Text = hdfDtaFim.Value;


            var info = Curriculo.InformacoesSA(Convert.ToInt32(idCurriculo),Convert.ToDateTime(txtDataInicio.Text), Convert.ToDateTime(txtDataFim.Text).AddHours(23).AddMinutes(59));
            hdfCV.Value = idCurriculo.ToString();
            foreach (DataRow row in info.Rows)
            {
                lblCandidaturas.Text = row["QtdCandidaturas"].ToString();
                lblEmpresaEnviadaCV.Text = row["QtdEmpresaCvEnviado"].ToString();
                lblEmpresaVisualizaram.Text = row["QtdQuemMeviu"].ToString();
                lblQtdCidades.Text = row["QtdCidadeAlertaVaga"].ToString();
                lblQtdFuncoes.Text = row["QtdFuncaoAlertaVaga"].ToString();

            }
            upInformacoes.Update();
            ScriptManager.RegisterStartupScript(this, GetType(), "dtpiker", "dtpiker();", true);

        }

        protected void lnkVoltar_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(BLL.Enumeradores.RouteCollection.AdministradorCurriculo.ToString(), new { cpf = hdfCpf.Value }));
        }
    }
}