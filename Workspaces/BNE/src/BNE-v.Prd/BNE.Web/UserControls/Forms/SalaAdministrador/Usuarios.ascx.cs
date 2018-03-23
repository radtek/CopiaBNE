using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using BNE.Web.Code.Enumeradores;
using Resources;
using Telerik.Web.UI;
using BNE.Web.Code;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class Usuarios : BaseUserControl
    {

        #region Propriedades

        #region IdUsuarioFilialPerfil - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID do usuário filial perfil
        /// </summary>
        public int? IdUsuarioFilialPerfil
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
            }
        }
        #endregion

        #region StatusUsuarios - Variavel
        private int? StatusUsuarios
        {
            get
            {
                if (ViewState["StatusUsuarios"] != null)
                    return Int32.Parse(ViewState["StatusUsuarios"].ToString());
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add("StatusUsuarios", value);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            StatusUsuarios = Convert.ToInt32(rbLstStatusUsuarios.SelectedValue);

            HttpContext context = HttpContext.Current;
            context.Session["Usuarios_SalaAdmConfiguracoes"] = ccbTipo.SelectedValue;


            gvUsuarios.PageSize = 6;
            gvUsuarios.CurrentPageIndex = 0;
            UIHelper.CarregarRadComboBox(ccbTipo, Perfil.ListarAdministrador(), "Idf_Perfil", "Des_Perfil");
            ccbTipo.CheckAllItems();
            UIHelper.CarregarDropDownList(ddlCadastroTipoPerfil, Perfil.ListarAdministrador(), "Idf_Perfil", "Des_Perfil");

            CarregarGrid();
        }
        #endregion

        #region CarregarGrid
        public void CarregarGrid()
        {
            //
            int? status;
            if (StatusUsuarios != 2) //filtra por ativo ou inativo
                status = StatusUsuarios;
            else
                status = null; //carrega Todos os Planos 

            rbLstStatusUsuarios.SelectedValue = StatusUsuarios.ToString(); //deixar o radioButton selecionado de acordo com os registros da grid
            //upRdStatusPlanos.Update();

            var cpf = txtCPFPesquisa.Valor.Replace(".", "").Replace("-", "").Replace(",", "");
            var nome = txtNomePesquisa.Valor;

            if (ccbTipo.GetCheckedItems().Count.Equals(0))
                ccbTipo.CheckAllItems();

            string idPerfil = string.Empty;
            for (int i = 0; i < ccbTipo.GetCheckedItems().Count; i++)
            {
                RadComboBoxItem item = ccbTipo.GetCheckedItems()[i];

                if (i < ccbTipo.GetCheckedItems().Count - 1)
                    idPerfil += item.Value + ", ";
                else
                    idPerfil += item.Value;
            }

            int totalRegistros;
            UIHelper.CarregarRadGrid(gvUsuarios, UsuarioFilialPerfil.ListarUsuariosAdministradorPorFiltros(idPerfil, status, cpf, nome, gvUsuarios.CurrentPageIndex, gvUsuarios.PageSize, out totalRegistros), totalRegistros);
        }
        #endregion

        #region AjustarEMostrarModalCadastrar
        private void AjustarEMostrarModalCadastrar(bool editarUsuario, bool editarSenha)
        {
            btnSalvar.Visible = btnSalvarSenha.Visible = false;
            lblCadastroTipoPerfil.Visible = lblCadastroCPF.Visible = lblCadastroSenha.Visible = lblCadastroValorCPF.Visible = txtCadastroCPF.Visible = txtCadastroSenha.Visible = ddlCadastroTipoPerfil.Visible = false;

            if (IdUsuarioFilialPerfil.HasValue)
            {
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject((int)IdUsuarioFilialPerfil);
                objUsuarioFilialPerfil.PessoaFisica.CompleteObject();
                
                lblCadastroValorCPF.Text = txtCadastroCPF.Valor = objUsuarioFilialPerfil.PessoaFisica.NumeroCPF;
                ddlCadastroTipoPerfil.SelectedValue = objUsuarioFilialPerfil.Perfil.IdPerfil.ToString(CultureInfo.CurrentCulture);

                lblCadastroCPF.Visible = lblCadastroValorCPF.Visible = true;
                    
                if (editarSenha)
                {
                    lblCadastroSenha.Visible = true;
                    txtCadastroSenha.Visible = true;
                    btnSalvarSenha.Visible = true;
                }

                if (editarUsuario)
                {
                    lblCadastroTipoPerfil.Visible = ddlCadastroTipoPerfil.Visible = true;
                    btnSalvar.Visible = true;
                }
            }
            else
            {
                lblCadastroValorCPF.Text = txtCadastroCPF.Valor = txtCadastroSenha.Valor = string.Empty;
                lblCadastroCPF.Visible = txtCadastroCPF.Visible = true;
                lblCadastroSenha.Visible = txtCadastroSenha.Visible = true;
                lblCadastroTipoPerfil.Visible = ddlCadastroTipoPerfil.Visible = true;
                btnSalvar.Visible = true;
            }
            upTxtCadastroCPF.Update();
            upTxtCadastroSenha.Update();
            upDdlCadastroTipoPerfil.Update();
            upBotoes.Update();
            mpeModalCadastrar.Show();
        }
        #endregion

        #region Salvar
        private bool Salvar(bool editarSenha)
        {
            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(txtCadastroCPF.Valor, out objPessoaFisica))
            {
                var objPerfil = new Perfil(Convert.ToInt32(ddlCadastroTipoPerfil.SelectedValue));

                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (IdUsuarioFilialPerfil.HasValue)
                {
                    objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject((int)IdUsuarioFilialPerfil);
                }
                else
                {
                    if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilPessoaFisicaPerfil(objPessoaFisica, objPerfil, out objUsuarioFilialPerfil))
                    {
                        objUsuarioFilialPerfil = new UsuarioFilialPerfil
                        {
                            PessoaFisica = PessoaFisica.CarregarPorCPF(txtCadastroCPF.Valor),
                            DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                        };
                    }
                }

                Usuario objUsuario;
                if (!Usuario.CarregarPorPessoaFisica(objUsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objUsuario))
                {
                    objUsuario = new Usuario
                        {
                            PessoaFisica = objUsuarioFilialPerfil.PessoaFisica
                        };
                }

                if (editarSenha)
                    objUsuario.SenhaUsuario = txtCadastroSenha.Valor;

                objUsuarioFilialPerfil.FlagInativo = false;
                objUsuarioFilialPerfil.Perfil = objPerfil;
                objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = DateTime.Now.ToLongDateString();
                objUsuarioFilialPerfil.Salvar(objUsuario);
                return true;
            }
            
            ExibirMensagem("CPF não cadastrado!", TipoMensagem.Aviso);
            return false;
        }
        #endregion

        #region SalvarSenha
        private bool SalvarSenha()
        {
            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(txtCadastroCPF.Valor, out objPessoaFisica))
            {
                Usuario objUsuario;
                if (!Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario))
                {
                    objUsuario = new Usuario
                    {
                        PessoaFisica = objPessoaFisica
                    };
                }

                objUsuario.SenhaUsuario = txtCadastroSenha.Valor;
                objUsuario.Save();
                return true;
            }

            ExibirMensagem("CPF não cadastrado!", TipoMensagem.Aviso);
            return false;
        }
        #endregion

        protected bool ValidaFiltrosInformados()
        {
            try
            {
                bool retorno = true;

                retorno = ValidaFiltroCPF();

                return retorno;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected bool ValidaFiltroCPF()
        {
            var cpf = txtCPFPesquisa.Valor;

            if (string.IsNullOrEmpty(cpf))
                return true; //Sem preenchimento, permite realizar a busca

            Int64 numcpf;
            bool cpfValido = Int64.TryParse(cpf.Replace(".", "").Replace("-", ""), out numcpf);
            if (!cpfValido)
            {
                txtCPFPesquisa.Valor = string.Empty;
                ExibirMensagem("Campo CPF com valores inválidos. Informe apenas números ou um CPF no formato 999.999.999-99.", TipoMensagem.Aviso);
                return false;
            }

            return true;
        }

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;
        }
        #endregion

        #region Delegates
        public delegate void DelegateVoltar();
        public event DelegateVoltar Voltar;
        #endregion

        #region gvUsuarios_PageIndexChanged
        protected void gvUsuarios_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvUsuarios.CurrentPageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        #endregion

        #region gvUsuarios_ItemCommand
        protected void gvUsuarios_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditarUsuario"))
            {
                IdUsuarioFilialPerfil = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);
                AjustarEMostrarModalCadastrar(true, false);
            }
            if (e.CommandName.Equals("EditarSenha"))
            {
                IdUsuarioFilialPerfil = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);
                AjustarEMostrarModalCadastrar(false, true);
            }
            if (e.CommandName.Equals("ExcluirUsuario"))
            {
                IdUsuarioFilialPerfil = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);
                ucConfirmacaoExclusao.Inicializar("Atenção!", "Tem certeza que deseja desabilitar o usuário?");
                ucConfirmacaoExclusao.MostrarModal();
            }
        }
        #endregion

        #region btnFiltrar_Click
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (!ValidaFiltrosInformados())
                return;

            StatusUsuarios = Convert.ToInt32(rbLstStatusUsuarios.SelectedValue);    
            gvUsuarios.CurrentPageIndex = 0;
            CarregarGrid();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Voltar();
        }
        #endregion

        #region btnCadastro_Click
        protected void btnCadastro_Click(object sender, EventArgs e)
        {
            IdUsuarioFilialPerfil = null;
            AjustarEMostrarModalCadastrar(true, true);
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                //Se é um novo usuario deve salvar a senha
                bool salvarSenha = !IdUsuarioFilialPerfil.HasValue;

                if (Salvar(salvarSenha))
                {
                    mpeModalCadastrar.Hide();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvarSenha_Click
        protected void btnSalvarSenha_Click(object sender, EventArgs e)
        {
            try
            {
                if (SalvarSenha())
                {
                    mpeModalCadastrar.Hide();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject((int)IdUsuarioFilialPerfil);
            objUsuarioFilialPerfil.FlagInativo = true;
            objUsuarioFilialPerfil.Save();
            CarregarGrid();
            ucConfirmacaoExclusao.FecharModal();
            upGvUsuarios.Update();
        }
        #endregion

        #endregion

    }
}