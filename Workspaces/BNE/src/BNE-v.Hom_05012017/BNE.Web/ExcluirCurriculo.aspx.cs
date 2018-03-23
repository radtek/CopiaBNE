using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using System;
using System.Data;

namespace BNE.Web
{
    public partial class ExcluirCurriculo : BasePage
    {

        #region Eventos

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "OnLoad");
                CarregarEstatisticasdoCurriculo();
            }
        }
        #endregion

        #region BtnIrParaExcluirCV_Click
        protected void btnIrParaExcluirCV_Click(Object sender, EventArgs e)
        {
            etapa01.Visible = false;
            etapa02.Visible = true;
        }
        #endregion

        #region BtnExcluirCV_Click
        protected void btnExcluirCV_Click(Object sender, EventArgs e)
        {
            //gravar motivo de exclusao
            //mudar status do CV para Hibernar
            bool result = false;

            /*Valida se algum motivo foi selecionado*/
            if (!chkEmpregado.Checked && !chkPoucasVagas.Checked && !chkMuitosEmails.Checked && !chkOutros.Checked)
            {
                lblAvisoErro.Visible = true;
                lblAvisoErro.Text = "Por favor, selecione um ou mais motivos.";
                return;
            }

            if (chkEmpregado.Checked)
            {
                result = BLL.CurriculoMotivoExclusao.RegistrarCurriculoMotivoExclusao(base.IdCurriculo.Value, "", rblEmpregoBNE.SelectedValue == "1",
                    (byte)BLL.Enumeradores.MotivoExclusao.JaEstouEmpregado);
            }

            if (chkPoucasVagas.Checked)
            {
                result = BLL.CurriculoMotivoExclusao.RegistrarCurriculoMotivoExclusao(base.IdCurriculo.Value, "", false, (byte)BLL.Enumeradores.MotivoExclusao.PoucasVagas);
            }

            if (chkMuitosEmails.Checked)
            {
                result = BLL.CurriculoMotivoExclusao.RegistrarCurriculoMotivoExclusao(base.IdCurriculo.Value, "", false, (byte)BLL.Enumeradores.MotivoExclusao.ReceboMuitosEmails);
            }

            if (chkOutros.Checked)
            {
                result = BLL.CurriculoMotivoExclusao.RegistrarCurriculoMotivoExclusao(base.IdCurriculo.Value, txtOutroMotivo.InnerText, false,
                    (byte)BLL.Enumeradores.MotivoExclusao.Outros);
            }


            if (result)
            {
                //Hibernar o CV
                bool hibernado = BLL.Curriculo.HibernarCurriculo(base.IdCurriculo.Value);

                if (hibernado)
                {
                    etapa01.Visible = false;
                    etapa02.Visible = false;
                    etapa03.Visible = true;

                    lblAvisoErro.Visible = true;

                    /*Efetuar Logoff Usuario*/
                    Auth.BNEAutenticacao.DeslogarPadrao();
                    AjustarLogin();
                }
                else
                {
                    lblAvisoErro.Visible = true;
                    lblAvisoErro.Text = "Ocorreu um erro ao excluir o currículo, por favor tente novamente.";
                }
            }
            else
            {
                lblAvisoErro.Visible = true;
                lblAvisoErro.Text = "Ocorreu um erro ao excluir o currículo, por favor tente novamente.";
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region CarregarEstatisticasdoCurriculo
        private void CarregarEstatisticasdoCurriculo()
        {
            if (base.IdCurriculo.HasValue)
            {
                DataRow dr = null;
                DataTable dtEstatisticas = BLL.Curriculo.CarregarMetricasCurriculo(base.IdCurriculo.Value);

                if (dtEstatisticas.Rows.Count > 0)
                    dr = dtEstatisticas.Rows[0];

                if (dr != null)
                {
                    int qtdVagasRecebidasJornal = Convert.ToInt32(dr["QtdVagasRecebidasJornal"].ToString());
                    int qtdEmpresasPesquisaramnoPerfil = Convert.ToInt32(dr["QtdEmpresasPesquisaramnoPerfil"].ToString());
                    int qtdQuemMeViu = Convert.ToInt32(dr["QtdQuemMeViu"].ToString());
                    int qtdVezesApareciNabusca = Convert.ToInt32(dr["QtdVezesApareciNabusca"].ToString());
                    int qtdVagasPerfil = Convert.ToInt32(dr["QtdVagasPerfil"].ToString());
                    int qtdCandidaturas = Convert.ToInt32(dr["QtdCandidaturas"].ToString());
                    int qtdVagasVisualizadas = Convert.ToInt32(dr["QtdVagasVisualizadas"].ToString());
                    int qtdBuscasRealizadas = Convert.ToInt32(dr["QtdBuscasRealizadas"].ToString());
                    int qtdLoginsRealizados = Convert.ToInt32(dr["QtdLoginsRealizados"].ToString());
                    int qtdBuscaPerfil = Convert.ToInt32(dr["QtdBuscaPerfil"].ToString());
                    int qtdVagasNaCidadeERegiao = Convert.ToInt32(dr["QtdVagasNaCidadeERegiao"].ToString());
                    int qtdVagasNaoVisualizada = Convert.ToInt32(dr["QtdVagasNaoVisualizada"].ToString());
                    int qtdVagasNaArea = Convert.ToInt32(dr["QtdVagasNaArea"].ToString());
                    int qtdSmsRecebidos = Convert.ToInt32(dr["QtdSmsRecebidos"].ToString());

                    if (qtdQuemMeViu > 0)
                    {
                        cardUm.Visible = qtdQuemMeViu > 0;
                        ltQuemMeViu.Text = string.Format("<h6>seu cv foi visualizado</h6><h3>{0} {1}</h3>", qtdQuemMeViu, qtdQuemMeViu == 1 ? "vez" : "vezes");
                    }
                    else
                    {
                        cardUm.Visible = qtdVagasNaoVisualizada > 0;
                        ltVagasNaoVisualizadas.Text = string.Format("<h6>vagas não visualizadas</h6><h3>{0} {1}</h3>", qtdVagasNaoVisualizada, qtdVagasNaoVisualizada == 1 ? "vaga" : "vagas");
                    }

                    if (qtdVezesApareciNabusca > 0)
                    {
                        cardDois.Visible = qtdVezesApareciNabusca > 0;
                        ltVezesApareciNabusca.Text = string.Format("<h6>você apareceu nas pesquisas</h6><h3>{0} {1}</h3>", qtdVezesApareciNabusca, qtdVezesApareciNabusca == 1 ? "vez" : "vezes");
                    }
                    else
                    {
                        cardDois.Visible = qtdVagasNaCidadeERegiao > 0;
                        ltVagasNaCidadeERegiao.Text = string.Format("<h6>vagas na sua região</h6><h3>{0} {1}</h3>", qtdVagasNaCidadeERegiao, qtdVagasNaCidadeERegiao == 1 ? "vaga" : "vagas");
                    }

                    if (qtdEmpresasPesquisaramnoPerfil > 0)
                    {
                        cardTres.Visible = qtdEmpresasPesquisaramnoPerfil > 0;
                        ltEmpresasPesquisaramnoPerfil.Text = string.Format("<h6>pesquisaram seu perfil</h6><h3>{0} {1}</h3>", qtdEmpresasPesquisaramnoPerfil, qtdEmpresasPesquisaramnoPerfil == 1 ? "vez" : "vezes");
                    }
                    else
                    {
                        cardTres.Visible = qtdBuscaPerfil > 0;
                        ltBuscaPerfil.Text = string.Format("<h6>vagas na sua região</h6><h3>{0} {1}</h3>", qtdBuscaPerfil, qtdBuscaPerfil == 1 ? "vez" : "vezes");
                    }

                    //lblEmpresas.Text = string.Format("{0} {1}", qtdEmpresasPesquisaramnoPerfil, qtdEmpresasPesquisaramnoPerfil == 1 ? "Empresa" : "Empresas");
                }

                pnExcluirCV.Visible = true;
                etapa02.Visible = false;
                etapa03.Visible = false;
            }
            else
            {
                pnExcluirCV.Visible = false;
                var principal = (Principal)Page.Master;

                if (principal != null)
                    principal.ExibirLoginPara("ExcluirCurriculo.aspx");
            }
        }
        #endregion

        #endregion

    }
}