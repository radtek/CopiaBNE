using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.Struct;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class EmailParaFilial : BaseUserControl
    {

        #region Propriedades

        #region IdGrupoCidade - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdGrupoCidade
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
                else
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

        #region IdEmailDestinatarioCidade - Variavel 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdEmailDestinatarioCidade
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel2.ToString());

            }
        }
        #endregion

        #region EmailExcluir - Variavel3
        private List<int> EmailExcluir
        {
            get { return (List<int>)ViewState[Chave.Temporaria.Variavel3.ToString()]; }
            set { ViewState[Chave.Temporaria.Variavel3.ToString()] = value; }
        }
        #endregion

        #region DtEmail - Variavel4
        private DataTable DtEmail
        {
            get { return (DataTable)ViewState[Chave.Temporaria.Variavel4.ToString()]; }
            set { ViewState[Chave.Temporaria.Variavel4.ToString()] = value; }
        }
        #endregion

        #region RowIndex - Variavel 5
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? RowIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel5.ToString()].ToString());
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel5.ToString());

            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;
            txtEmail.ExpressaoValidacao = Configuracao.regexEmail;

            if (!IsPostBack)
            {
                txtFilial.Attributes["OnChange"] += "txtFilial_OnChange();";
                txtFilial.Attributes["OnBlur"] += string.Format("ValidatorValidate($get('{0}'));", cvFilial.ClientID);
            }

            Ajax.Utility.RegisterTypeForAjax(typeof(EmailParaFilial));
        }
        #endregion

        #region Pesquisa

        #region btnPesquisaCadastro_Click
        protected void btnPesquisaCadastro_Click(object sender, EventArgs e)
        {
            InicializarCadastro();
        }
        #endregion

        #region btnPesquisaPesquisar_Click
        protected void btnPesquisaPesquisar_Click(object sender, EventArgs e)
        {
            //Atualizando o index atual para evitar erro na paginação.
            gvPesquisa.CurrentPageIndex = 0;
            CarregarGridPesquisa();
        }
        #endregion

        #region btnPesquisaVoltar_Click
        protected void btnPesquisaVoltar_Click(object sender, EventArgs e)
        {
            Voltar();
        }
        #endregion

        #region btnExibirDetalhesCancelar_Click
        protected void btnExibirDetalhesCancelar_Click(object sender, EventArgs e)
        {
            mpeExibirDetalhes.Hide();
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            if (IdEmailDestinatarioCidade.HasValue)
                ExecutarExclusaoEmail();
            else
                ExecutarExclusaoGrupoCidade();
        }
        #endregion

        #region GridView

        #region gvPesquisa_ItemCommand
        protected void gvPesquisa_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("ExibirDetalhes"))
            {
                var grupoCidade = gvPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Grupo_Cidade"];
                int idGrupoCidade;
                if (Int32.TryParse(grupoCidade.ToString(), out idGrupoCidade))
                {
                    GrupoCidade objGrupoCidade = GrupoCidade.LoadObject(idGrupoCidade);
                    ExibirDetalhes(objGrupoCidade);
                }
            }
            else if (e.CommandName.Equals("EditarEmail"))
            {
                var grupoCidade = gvPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Grupo_Cidade"];
                int idGrupoCidade;
                if (Int32.TryParse(grupoCidade.ToString(), out idGrupoCidade))
                {
                    GrupoCidade objGrupoCidade = GrupoCidade.LoadObject(idGrupoCidade);
                    EditarGrupoCidade(objGrupoCidade);
                }
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                var grupoCidade = gvPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Grupo_Cidade"];
                int idGrupoCidade;
                if (Int32.TryParse(grupoCidade.ToString(), out idGrupoCidade))
                {
                    IdGrupoCidade = idGrupoCidade;

                    ucConfirmacaoExclusao.Inicializar("Confirmação", "Tem certeza que deseja excluir o grupo?");
                    ucConfirmacaoExclusao.MostrarModal();
                }
            }
        }
        #endregion

        #region gvPesquisa_PageIndexChanged
        protected void gvPesquisa_PageIndexChanged(object souce, GridPageChangedEventArgs e)
        {
            gvPesquisa.CurrentPageIndex = e.NewPageIndex;
            CarregarGridPesquisa();
        }
        #endregion

        #region gvExibirDetalhesPesquisa_ItemCommand
        protected void gvExibirDetalhesPesquisa_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditarEmail"))
            {
                var grupoCidade = gvExibirDetalhesPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Grupo_Cidade"];
                int idGrupoCidade;
                if (Int32.TryParse(grupoCidade.ToString(), out idGrupoCidade))
                {
                    mpeExibirDetalhes.Hide();
                    GrupoCidade objGrupoCidade = GrupoCidade.LoadObject(idGrupoCidade);
                    EditarGrupoCidade(objGrupoCidade);
                }
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                var grupoCidade = gvExibirDetalhesPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Grupo_Cidade"];
                int idGrupoCidade;
                if (Int32.TryParse(grupoCidade.ToString(), out idGrupoCidade))
                {
                    IdGrupoCidade = idGrupoCidade;
                    mpeExibirDetalhes.Hide();
                    ucConfirmacaoExclusao.Inicializar("Confirmação", "Tem certeza que deseja excluir o grupo?");
                    ucConfirmacaoExclusao.MostrarModal();
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region Cadastro

        #region btnCadastroSalvar_Click
        protected void btnCadastroSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Collection<RadComboBoxItem> items = ccbCidade.GetCheckedItems();
                if (items.Count > 0)
                {

                    SalvarCadastro();
					btnCadastroLimpar_Click(btnCadastroLimpar, new EventArgs());
                    //LimparCadastro();

                    IdGrupoCidade = null;
                    IdEmailDestinatarioCidade = null;

                    //base.ExibirMensagem("Grupo cadastrado com sucesso!", TipoMensagem.Aviso);
					base.ExibirMensagemConfirmacao(MensagemAviso._100001, MensagemAviso._100001, false);
                }
                else
                    base.ExibirMensagem("É necessário selecionar uma cidade!", TipoMensagem.Erro);

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnCadastroLimpar_Click
        protected void btnCadastroLimpar_Click(object sender, EventArgs e)
        {
            IdGrupoCidade = null;
            IdEmailDestinatarioCidade = null;

            LimparCadastro();
        }
        #endregion

        #region btnCadastroVoltar_Click
        protected void btnCadastroVoltar_Click(object sender, EventArgs e)
        {
            InicializarPesquisa();
        }
        #endregion

        #region btnCarregarCidades_Click
        protected void btnCarregarCidades_Click(object sender, EventArgs e)
        {
            ConfigurarCidades();
        }
        #endregion

        #region btiAdicionar_Click
        protected void btiAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagem;
                if (ValidarResponsavel(out mensagem))
                {
                    AdicionarEmail();
                    LimparCadastroFilialNomeEmail();
                    IdEmailDestinatarioCidade = null;
                    CarregarGridCadastro();
                }
                else
                    base.ExibirMensagem(mensagem, TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region GridView

        #region gvCadastroEmail_PageIndexChanged
        protected void gvCadastroEmail_PageIndexChanged(object souce, GridPageChangedEventArgs e)
        {
            gvCadastroEmail.CurrentPageIndex = e.NewPageIndex;
            CarregarGridCadastro();
        }
        #endregion

        #region gvCadastroEmail_ItemCommand
        protected void gvCadastroEmail_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditarEmail"))
            {
                DataRow drEmail = DtEmail.Rows[e.Item.ItemIndex];
                //Editando a verba e preenchendo os campos
                EditarEmail(drEmail, e.Item.ItemIndex);
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                var email = gvCadastroEmail.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Email_Destinatario_Cidade"];
                int idEmail;
                Int32.TryParse(email.ToString(), out idEmail);
                IdEmailDestinatarioCidade = idEmail;
                RowIndex = e.Item.ItemIndex;

                ucConfirmacaoExclusao.Inicializar("Confirmação", "Tem certeza que deseja excluir o e-mail?");
                ucConfirmacaoExclusao.MostrarModal();
            }
        }
        #endregion

        #endregion

        #endregion

        #region txtFilial_TextChanged
        protected void txtFilial_TextChanged(object sender, EventArgs e)
        {
            Filial objFilial;
			if (!string.IsNullOrEmpty(txtFilial.Text))
			{
				if (Filial.CarregarFilialEmployerPorApelido(txtFilial.Text, out objFilial))
				{
					GerenteFilial objGerenfilial = Filial.CarregarFilialEmployerPorFilial(objFilial);
					txtNome.Valor = objGerenfilial.Nome;
					txtEmail.Valor = objGerenfilial.Email;
				}
			}
			else
			{
				txtNome.Valor = String.Empty;
				txtEmail.Valor = String.Empty;
			}
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ValidatorValidateFilial", string.Format("alert();ValidatorValidate($get('{0}');", cvFilial.ClientID), true);
            upTxtNome.Update(); upTxtEmail.Update(); upTxtTelefone.Update();
        }
        #endregion

        #region ckbResponsavel_CheckedChanged
        protected void ckbResponsavel_CheckedChanged(object sender, EventArgs e)
        {
            var ckb = (CheckBox)sender;

            AjustarCampoTelefone(ckb.Checked);
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            InicializarPesquisa();
            UIHelper.CarregarRadComboBox(ccbEstado, Estado.Listar());
        }
        #endregion

        #region RetornarTextoEncurtado
        protected string RetornarTextoEncurtado(string texto, int quantidadeCaracteres)
        {
            string textoConcatenar = "...";

            if (String.IsNullOrEmpty(texto))
                return string.Empty;
            else
            {
                if (texto.Length <= quantidadeCaracteres)
                    return texto.Substring(0, texto.Length);
                else
                    return String.Format("{0}{1}", texto.Substring(0, quantidadeCaracteres - textoConcatenar.Length), textoConcatenar);
            }
        }
        #endregion

        #region AjustarDataTable
        private void AjustarDataTable(DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                dc.AllowDBNull = true;
                dc.AutoIncrement = false;
                dc.AutoIncrementStep = 1;
                dc.ReadOnly = false;
            }
        }
        #endregion

        #region Pesquisa

        #region InicializarPesquisa
        public void InicializarPesquisa()
        {
            IdGrupoCidade = null;

            //Ajustando a grid 
            gvPesquisa.PageSize = 5;
            gvExibirDetalhesPesquisa.PageSize = 1;

            pnlCadastro.Visible = false;
            pnlPesquisa.Visible = true;

            CarregarGridPesquisa();

            upPnlCadastro.Update();
            upGvPesquisa.Update();
        }
        #endregion

        #region CarregarGridPesquisa
        private void CarregarGridPesquisa()
        {
            int totalRegistros = 0;
            UIHelper.CarregarRadGrid(gvPesquisa, GrupoCidade.PesquisarGrupoCidade(gvPesquisa.CurrentPageIndex, gvPesquisa.PageSize, out totalRegistros, null, txtPequisa.Valor), totalRegistros);
            upGvPesquisa.Update();
        }
        #endregion

        #region CarregarGridDetalhes
        private void CarregarGridDetalhes(GrupoCidade objGrupoCidade)
        {
            int totalRegistros = 0;
            UIHelper.CarregarRadGrid(gvExibirDetalhesPesquisa, GrupoCidade.PesquisarGrupoCidade(gvExibirDetalhesPesquisa.CurrentPageIndex, gvExibirDetalhesPesquisa.PageSize, out totalRegistros, objGrupoCidade.IdGrupoCidade, string.Empty), totalRegistros);
            upGvExibirDetalhesPesquisa.Update();
        }
        #endregion

        #region ExibirDetalhes
        private void ExibirDetalhes(GrupoCidade objGrupoCidade)
        {
            CarregarGridDetalhes(objGrupoCidade);
            mpeExibirDetalhes.Show();
        }
        #endregion

        #region EditarGrupoCidade
        private void EditarGrupoCidade(GrupoCidade objGrupoCidade)
        {
            IdGrupoCidade = objGrupoCidade.IdGrupoCidade;
            InicializarCadastro();
        }
        #endregion

        #region ExcluirGrupoCidade
        private void ExcluirGrupoCidade(GrupoCidade objGrupoCidade)
        {
            try
            {
                objGrupoCidade.Inativar();
                base.ExibirMensagemConfirmacao("Confirmação de Exclusão", "Grupo excluído com sucesso!", false);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ExecutarExclusaoGrupoCidade
        private void ExecutarExclusaoGrupoCidade()
        {
            GrupoCidade objGrupoCidade = GrupoCidade.LoadObject(IdGrupoCidade.Value);
            ExcluirGrupoCidade(objGrupoCidade);
            CarregarGridPesquisa();
            IdGrupoCidade = null;
        }
        #endregion

        #endregion

        #region Cadastro

        #region CarregarGridCadastro
        private void CarregarGridCadastro()
        {
            UIHelper.CarregarRadGrid(gvCadastroEmail, DtEmail);
            upGvCadastroEmail.Update();
        }
        #endregion

        #region InicializarCadastro
        public void InicializarCadastro()
        {
            //Ajustando a grid 
            gvCadastroEmail.PageSize = 5;

            pnlCadastro.Visible = true;
            pnlPesquisa.Visible = false;

            //Desabilitando o combo de cidade
            ccbCidade.Enabled = false;

            IdEmailDestinatarioCidade = null;

            //Inicializando a datable de emails
            EmailExcluir = new List<int>();
            int totalRegistros = 0;
            DtEmail = EmailDestinatarioCidade.PesquisarEmailPorGrupo(0, Int16.MaxValue, out totalRegistros, IdGrupoCidade.HasValue ? (int)IdGrupoCidade.Value : 0);
            AjustarDataTable(DtEmail);

            LimparCadastro();
            PreencherCamposCadastro();
            CarregarGridCadastro();

            upGeral.Update();
        }
        #endregion

        #region LimparCadastro
        private void LimparCadastro()
        {
            ccbEstado.ClearCheckeds();
            ccbCidade.ClearCheckeds();
            ccbEstado.Text = ccbEstado.EmptyMessage;
            ccbCidade.Text = ccbCidade.EmptyMessage;

            ConfigurarCidades();

            LimparCadastroFilialNomeEmail();

            EmailExcluir = new List<int>();
            int totalRegistros = 0;
            DtEmail = EmailDestinatarioCidade.PesquisarEmailPorGrupo(0, Int16.MaxValue, out totalRegistros, IdGrupoCidade.HasValue ? (int)IdGrupoCidade.Value : 0);
            AjustarDataTable(DtEmail);

            CarregarGridCadastro();

            upTxtEmail.Update();
        }
        #endregion

        #region LimparCadastroFilialNomeEmail
        private void LimparCadastroFilialNomeEmail()
        {
            btnListaSalvar.Text = "Adicionar";
            upBtnListaSalvar.Update();

            txtFilial.Text = string.Empty;
            txtNome.Valor = string.Empty;
            ckbResponsavel.Checked = false;
            AjustarCampoTelefone(false);
            txtEmail.Valor = string.Empty;
            txtTelefone.DDD = string.Empty;
            txtTelefone.Fone = string.Empty;

            upTxtFilial.Update();
            upTxtNome.Update();
            upTxtTelefone.Update();
            upCkbResponsavel.Update();
            upTxtEmail.Update();
        }
        #endregion

        #region PreencherCamposCadastro
        private void PreencherCamposCadastro()
        {
            PreencherCamposCadastroGrupo();
            PreencherCamposCadastroEmail();
        }
        #endregion

        #region PreencherCamposCadastroGrupo
        private void PreencherCamposCadastroGrupo()
        {
            if (IdGrupoCidade.HasValue)
            {
                GrupoCidade objGrupoCidade = GrupoCidade.LoadObject(IdGrupoCidade.Value);

                //Carregando os valores da lista de estados
                List<Cidade> listCidade = objGrupoCidade.ListarCidades();
                var estados = listCidade.Select(c => c.Estado.SiglaEstado).Distinct();

                foreach (string estado in estados)
                    ccbEstado.SetItemChecked(estado, true);

                ConfigurarCidades();

                foreach (Cidade objCidade in listCidade)
                    ccbCidade.SetItemChecked(objCidade.IdCidade.ToString(), true);

                if (objGrupoCidade.Filial != null)
                {
                    objGrupoCidade.Filial.CompleteObject();
                    txtFilial.Text = objGrupoCidade.Filial.RazaoSocial;
                }
            }
        }
        #endregion

        #region PreencherCamposCadastroEmail
        private void PreencherCamposCadastroEmail()
        {
            if (IdEmailDestinatarioCidade.HasValue)
            {
                btnListaSalvar.Text = "Salvar";
                upBtnListaSalvar.Update();

                EmailDestinatarioCidade objEmailDestinatarioCidade = EmailDestinatarioCidade.LoadObject(IdEmailDestinatarioCidade.Value);
                objEmailDestinatarioCidade.EmailDestinatario.CompleteObject();

                txtNome.Valor = objEmailDestinatarioCidade.EmailDestinatario.NomePessoa;
                ckbResponsavel.Checked = objEmailDestinatarioCidade.FlagResponsavel;
                txtEmail.Valor = objEmailDestinatarioCidade.EmailDestinatario.DescricaoEmail;
                txtTelefone.DDD = objEmailDestinatarioCidade.EmailDestinatario.NumeroDDDTelefone;
                txtTelefone.Fone = objEmailDestinatarioCidade.EmailDestinatario.NumeroTelefone;

                AjustarCampoTelefone(ckbResponsavel.Checked);
            }
        }
        private void PreencherCamposCadastroEmail(DataRow dr)
        {
            btnListaSalvar.Text = "Salvar";
            upBtnListaSalvar.Update();

            txtNome.Valor = dr["Nme_Pessoa"].ToString();
            ckbResponsavel.Checked = Convert.ToBoolean(dr["Flg_Responsavel"]);
            txtEmail.Valor = dr["Des_Email"].ToString();
            txtTelefone.DDD = dr["Num_DDD_Telefone"].ToString();
            txtTelefone.Fone = dr["Num_Telefone"].ToString();
			// Busca a Filial
			int id=0;
			if(int.TryParse(dr["idf_Filial"].ToString(), out id))
			{
                Filial objFilial = new Filial(Convert.ToInt32(dr["idf_Filial"]));
				GerenteFilial objNome = Filial.CarregarFilialEmployerPorFilial(objFilial);
				txtFilial.Text = objNome.Nome;			
			}

            AjustarCampoTelefone(ckbResponsavel.Checked);

            upTxtFilial.Update();
            upTxtNome.Update();
            upTxtTelefone.Update();
            upCkbResponsavel.Update();
            upTxtEmail.Update();
        }
        #endregion

        #region SalvarCadastro
        private void SalvarCadastro()
        {
            GrupoCidade objGrupoCidade;
            if (IdGrupoCidade.HasValue)
                objGrupoCidade = GrupoCidade.LoadObject(IdGrupoCidade.Value);
            else
            {
                objGrupoCidade = new GrupoCidade
                {
                    FlagInativo = false
                };
            }
						
            if (String.IsNullOrEmpty(txtFilial.Text))
                objGrupoCidade.Filial = null;
            else
            {
                Filial objFilial;
                if (Filial.CarregarFilialEmployerPorApelido(txtFilial.Text, out objFilial))
                    objGrupoCidade.Filial = objFilial;
            }

            var listIdCidade = ccbCidade.GetCheckedItems().Select(item => Convert.ToInt32(item.Value)).ToList();

            objGrupoCidade.Salvar(listIdCidade, DtEmail, EmailExcluir);
        }
        #endregion

        #region ExcluirEmail
        private void ExcluirEmail(EmailDestinatarioCidade objEmailDestinatarioCidade)
        {
            try
            {
                if (objEmailDestinatarioCidade != null)
                {
                    if (!EmailExcluir.Contains(objEmailDestinatarioCidade.IdEmailDestinatarioCidade))
                        EmailExcluir.Add(objEmailDestinatarioCidade.IdEmailDestinatarioCidade);
                }
                DtEmail.Rows.RemoveAt((int)RowIndex);
                RowIndex = null;
                base.ExibirMensagemConfirmacao("Confirmação de Exclusão", "E-mail excluído com sucesso!", false);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ExecutarExclusaoEmail
        private void ExecutarExclusaoEmail()
        {
            EmailDestinatarioCidade objEmailDestinatarioCidade = IdEmailDestinatarioCidade > 0 ? EmailDestinatarioCidade.LoadObject(IdEmailDestinatarioCidade.Value) : null;
            ExcluirEmail(objEmailDestinatarioCidade);
            CarregarGridCadastro();
            IdEmailDestinatarioCidade = null;
        }
        #endregion

        #region ValidarResponsavel
        private bool ValidarResponsavel(out string mensagem)
        {
            mensagem = string.Empty;
            bool valido = true;
            if (ckbResponsavel.Checked)
            {
                foreach (GridDataItem gdi in gvCadastroEmail.Items)
                {
                    bool flagResponsavel = Convert.ToBoolean(gdi.GetDataKeyValue("Flg_Responsavel"));
                    var email = gdi.GetDataKeyValue("Idf_Email_Destinatario_Cidade");
                    int idEmail;
                    if (Int32.TryParse(email.ToString(), out idEmail))
                    {
                        if (flagResponsavel && !idEmail.Equals(IdEmailDestinatarioCidade))
                        {
                            valido = false;
                            mensagem = "É permitido apenas um responsável por grupo!";
                            break;
                        }
                    }
                    else if (flagResponsavel)
                    {
                        valido = false;
                        mensagem = "É permitido apenas um responsável por grupo!";
                        break;
                    }
                }

                //Se for um novo cadastro valida se para as cidades selecionadas já existe responsável.
                if (!IdGrupoCidade.HasValue)
                {
                    EmailDestinatarioCidade objEmailDestinatarioCidade;
                    foreach (RadComboBoxItem rcbi in ccbCidade.GetCheckedItems())
                    {
                        objEmailDestinatarioCidade = new EmailDestinatarioCidade();
                        if (EmailDestinatarioCidade.CarregarResponsavelCidade(new Cidade(Convert.ToInt32(rcbi.Value)), out objEmailDestinatarioCidade))
                        {
                            valido = false;
                            mensagem = "É permitido apenas um responsável por cidade!";
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(txtTelefone.DDD) || string.IsNullOrEmpty(txtTelefone.Fone))
                {
                    valido = false;
                    mensagem = "Para cadastrar um Responsável informe um telefone fixo válido.";
                }

            }

            return valido;
        }
        #endregion

        #region EditarEmail
        private void EditarEmail(DataRow dr, int RowIndex)
        {
            //IdEmailDestinatarioCidade = Convert.ToInt32(gvCadastroEmail.MasterTableView.DataKeys[RowIndex]["Idf_Email_Destinatario_Cidade"]);

            int idEmailDestinatarioCidadeAux;
            if (!Int32.TryParse(dr["Idf_Email_Destinatario_Cidade"].ToString(), out idEmailDestinatarioCidadeAux))
            {
                idEmailDestinatarioCidadeAux = RowIndex * -1; //Colocando valor negativo para identificar depois.

                while (FindDataRowEmailPorID(idEmailDestinatarioCidadeAux) != null)
                {
                    idEmailDestinatarioCidadeAux--;
                }

                dr["Idf_Email_Destinatario_Cidade"] = idEmailDestinatarioCidadeAux;
            }
            IdEmailDestinatarioCidade = idEmailDestinatarioCidadeAux;

            PreencherCamposCadastroEmail(dr);
        }
        #endregion

        #endregion

        #region ConfigurarCidades
        private void ConfigurarCidades()
        {
            var estados = ccbEstado.GetCheckedItems().Select(item => item.Value).ToList();
            UIHelper.CarregarRadComboBox(ccbCidade, Cidade.ListarPorEstados(estados), "Idf_Cidade", "Nme_Cidade");
            ccbCidade.Enabled = ccbCidade.Items.Count > 0;
            upCcbCidade.Update();
        }
        #endregion

        #region AdicionarEmail
        private void AdicionarEmail()
        {
            DataRow dataRowEmail;
            if (IdEmailDestinatarioCidade.HasValue)
            {
                dataRowEmail = FindDataRowEmailPorID((int)IdEmailDestinatarioCidade);

                //Se existe a row no datatable então atualiza os dados.
                if (dataRowEmail != null)
                    GravarDataRowEmail(dataRowEmail, false);
            }
            else
            {
                dataRowEmail = DtEmail.NewRow();
                GravarDataRowEmail(dataRowEmail, true);
                DtEmail.Rows.Add(dataRowEmail);
            }
        }
        #endregion

        #region GravarDataRowEmail
        private void GravarDataRowEmail(DataRow dataRowEmail, bool newRow)
        {
            if (newRow)
            {
                dataRowEmail["Dta_Cadastro"] = DateTime.Now;
            }

            dataRowEmail["Idf_Usuario_Gerador"] = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value;

            //dataRowEmail["Idf_Email_Destinatario_Cidade"] = objImplantacaoDeParaVerba.CodigoCliente;
            dataRowEmail["Flg_Responsavel"] = ckbResponsavel.Checked;
            dataRowEmail["Nme_Pessoa"] = txtNome.Valor;
            dataRowEmail["Des_Email"] = txtEmail.Valor;
            dataRowEmail["Num_DDD_Telefone"] = txtTelefone.DDD;
            dataRowEmail["Num_Telefone"] = txtTelefone.Fone;

			Filial objFilial;
			if (!string.IsNullOrEmpty(txtFilial.Text))
				if (Filial.CarregarFilialEmployerPorApelido(txtFilial.Text, out objFilial))
					dataRowEmail["idf_Filial"] = objFilial.IdFilial.ToString();
				else
					dataRowEmail["idf_Filial"] = System.DBNull.Value;
			else
				dataRowEmail["idf_Filial"] = System.DBNull.Value;

            dataRowEmail["Dta_Alteracao"] = DateTime.Now;
            dataRowEmail["Flg_Inativo"] = false;
        }
        #endregion

        #region FindDataRowEmailPorID
        private DataRow FindDataRowEmailPorID(int idEmail)
        {
            foreach (DataRow drEmail in DtEmail.Rows)
            {
                if (drEmail["Idf_Email_Destinatario_Cidade"].ToString().Equals(idEmail.ToString()))
                {
                    return drEmail;
                }
            }
            return null;
        }
        #endregion

        #region AjustarCampoTelefone
        private void AjustarCampoTelefone(bool campoObrigatorio)
        {
            txtTelefone.Obrigatorio = campoObrigatorio;

            txtTelefone.CssClassTextBoxDDI = campoObrigatorio ? "textbox_padrao ddi campo_obrigatorio" : "textbox_padrao ddi";
            txtTelefone.CssClassTextBoxDDD = campoObrigatorio ? "textbox_padrao ddd campo_obrigatorio" : "textbox_padrao ddd";
            txtTelefone.CssClassTextBoxFone = campoObrigatorio ? "textbox_padrao campo_obrigatorio" : "textbox_padrao";

            upCkbResponsavel.Update();
            upTxtTelefone.Update();
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void voltar();
        public event voltar Voltar;
        #endregion

        #region AjaxMethods

        #region ValidarFilial
        /// <summary>
        /// Validar filial employer
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="siglaUF"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarFilial(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Filial objFilial;
            return Filial.CarregarFilialEmployerPorApelido(valor, out objFilial);
        }
        #endregion

        #endregion

    }
}