using BNE.BLL;
using BNE.CEP;
using BNE.BLL.Custom;
using BNE.BLL.Security;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using BNE.Web.Resources;
using JSONSharp;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL.Common;
using BNE.Componentes.EL;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.CadastroCurriculo
{
    public partial class ConferirDados : BaseUserControl
    {

        #region Propriedades

        #region IdPessoaFisica - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdPessoaFisica
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

        #region IdFuncaoPretendida1 - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdFuncaoPretendida1
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

        #region IdFuncaoPretendida2 - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdFuncaoPretendida2
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel3.ToString());
            }
        }
        #endregion

        #region IdFuncaoPretendida3 - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdFuncaoPretendida3
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel4.ToString());
            }
        }
        #endregion

        #region IdPessoaFisicaVeiculo - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdPessoaFisicaVeiculo
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

        #region IdIdioma - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdIdioma
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel6.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdExperienciaProfissional
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel7.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel7.ToString());
            }
        }
        #endregion

        #region IdFormacao - Variável 8
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdFormacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel8.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel8.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel8.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel8.ToString());
            }
        }
        #endregion

        #region CEPValido - Variável 9
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public bool CEPValido
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel9.ToString()] != null)
                    return Boolean.Parse(ViewState[Chave.Temporaria.Variavel9.ToString()].ToString());
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel9.ToString(), value);
            }
        }
        #endregion

        #region IdComplementar - Variável 10
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdComplementar
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel10.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel10.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel10.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel10.ToString());
            }
        }
        #endregion

        #region IdEspecializacao - Variável 11
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdEspecializacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel11.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel11.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel11.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel11.ToString());
            }
        }
        #endregion

        #region FotoWSBytes - Variavel12
        private byte[] FotoWSBytes
        {
            get
            {
                return (byte[])(ViewState[Chave.Temporaria.Variavel12.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel12.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        public delegate void dadosSalvosSucesso();
        public event dadosSalvosSucesso DadosSalvosSucesso;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(ConferirDados));

            if (!Page.IsPostBack)
                Inicializar();

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "ConferirDados");
            else
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "ConferirDados");

            ucBuscarCEP.LogradouroSelecionado += ucBuscarCEP_LogradouroSelecionado;
            ucBuscarCEP.Cancelar += ucBuscarCEP_Cancelar;
            ucEnvioCurriculo.EnviarConfirmacao += ucEnvioCurriculo_EnviarConfirmacao;
        }
        #endregion

        #region btlMiniCurriculo
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlMiniCurriculo_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlDadosPessoais
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlDadosPessoais_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoDados.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlFormacaoCursos_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlFormacaoCursos_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoFormacao.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlDadosComplementares_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlDadosComplementares_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoComplementar.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlGestao_Click
        /// <summary>
        /// habilitar aba Gestao
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlGestao_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect("~/CadastroCurriculoGestao.aspx");
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvar
        /// <summary>
        /// Evento disparado no click do btnSalvar
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPessoaFisica.Value);

                if (DadosSalvosSucesso != null)
                    DadosSalvosSucesso();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnImprimirCurriculo_Click
        /// <summary>
        /// Evento disparado no click do btnSalvar
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnImprimirCurriculo_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                var url = Page.GetRouteUrl(Enumeradores.RouteCollection.ImpressaoCurriculo.ToString(), new { Identificador = Curriculo.RecuperarIdPorPessoaFisica(new PessoaFisica(this.IdPessoaFisica.Value)) });

                ScriptManager.RegisterStartupScript(upPnlBotoes, upPnlBotoes.GetType(), "AbrirPopup", string.Format("AbrirCurriculo('{0}');", url), true);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnEnviarCurriculo_Click
        protected void btnEnviarCurriculo_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                InicializarEnvioCurriculo();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region rptExperienciaProfissional_ItemCommand
        protected void rptExperienciaProfissional_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                var IdExperiencia = e.CommandArgument;

                int idExperiencia = 0;
                if (Int32.TryParse(IdExperiencia.ToString(), out idExperiencia))
                {
                    PreencherCamposExperienciaProfissional(idExperiencia);
                }

                divExperienciaProfissionalTextBox.Style["display"] = "block";
                divExperienciaProfissionalLabel.Style["display"] = "none";

                upExperienciaProfissional.Update();
            }
        }
        #endregion

        #region rptExperienciaProfissional_ItemDataBound

        protected void rptExperienciaProfissional_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item == null && e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkAdmissaoDemissaoValor");

            if (drv["Dta_Admissao"] != DBNull.Value)
            {
                //lnk.CommandArgument = drv["Idf_Experiencia_Profissional"].ToString();
                lnk.Visible = (drv["Idf_Experiencia_Profissional"] != DBNull.Value);
                lnk.Text = string.Format("{0} - de {1} até {2} ({3})", drv["Raz_Social"].ToString(), Convert.ToDateTime(drv["Dta_Admissao"]).ToString("dd/MM/yyyy"),
                    (drv["Dta_Demissao"] != DBNull.Value ? Convert.ToDateTime(drv["Dta_Demissao"]).ToString("dd/MM/yyyy") : "hoje (Emprego atual)"),
                    BLL.Custom.Helper.CalcularTempoEmprego(drv["Dta_Admissao"].ToString(), (drv["Dta_Demissao"] != DBNull.Value ? drv["Dta_Demissao"].ToString() : DateTime.Now.ToString())));
            }
        }
        #endregion

        #region rptExperienciaProfissional_ItemCreated
        protected void rptExperienciaProfissional_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            //Exibe o último salário somente na última empresa
            if (e.Item.ItemIndex > 4)
            {
                Control c = (Control)e.Item.FindControl("trLinhaUltimoSalario");
                if (c != null)
                {
                    c.Visible = false;
                }
            }
        }
        #endregion //rptExperienciaProfissional_ItemCreated

        #region txtCEP_ValorAlterado
        /// <summary>
        /// Responsável por habilitar a modal de busca de CEP com os vários endereços
        /// vinculados ao CEP informado (caso o CEP informado esteja configurado para vários endereços).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCEP_ValorAlterado(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCEP.Valor) && ValidarVariosEnderecosPorCEP(txtCEP.Valor))
            {
                txtCEP.DisplayMensagemErroWS = "none";

                ucBuscarCEP.CarregarPorCep(txtCEP.Valor);
            }
            else
            {

                int qtdeCepEncontrados = 0;
                try
                {
                    var objCep = new Cep
                    {
                        sCep = txtCEP.Valor.Replace("-", string.Empty).Trim()
                    };

                    qtdeCepEncontrados = Cep.ContaConsulta(objCep);
                    
                    if (qtdeCepEncontrados == 0)
                    {
                        HabilitarCamposCEP(true);

                        txtCEP.DisplayMensagemErroWS = "block";
                        txtCEP.MensagemErroWS = "CEP inexistente";
                        txtCEP.Focus();

                        PreencherCamposEndereco(String.Empty, String.Empty, txtNumero.Text, txtComplemento.Text, String.Empty);
                    }
                    else
                    {
                        Cep.CompletarCEP(ref objCep);
                        HabilitarCamposCEP(false);

                        //Em alguns casos há um CEP mas não há um Logradouro descrito para o mesmo.
                        if (string.IsNullOrEmpty(objCep.Logradouro))
                            HabilitarCamposCEP(true);

                        lblCidade.Text = txtCidade.Text = String.Format("{0}/{1}", objCep.Cidade, objCep.Estado);

                        txtCEP.DisplayMensagemErroWS = "none";

                        PreencherCamposEndereco(objCep.sCep, objCep.Logradouro, txtNumero.Text, txtComplemento.Text, objCep.Bairro);

                        upEndereco.Update();
                        txtNumero.Focus();
                    }
                    
                }
                catch (Exception ex)
                {
                    HabilitarCamposCEP(true);
                    txtCEP.DisplayMensagemErroWS = "block";
                    txtCEP.MensagemErroWS = "CEP inexistente";
                    txtCEP.Focus();

                    EL.GerenciadorException.GravarExcecao(ex, "Falha na busca de CEP");
                }
            }
            AjustarEventosEndereco();
            upCidadeLabel.Update(); upCidadeTextbox.Update();
        }
        #endregion

        #region txtFuncaoExercida_TextChanged
        protected void txtFuncaoExercida_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas.Text = RecuperarJobFuncao(txtFuncaoExercida.Text);
            //VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region ucBuscarCEP_Cancelar
        void ucBuscarCEP_Cancelar()
        {
            HabilitarCamposCEP(ValidarVariosEnderecosPorCEP(txtCEP.Valor));
        }
        #endregion

        #region ucBuscarCEP_LogradouroSelecionado
        /// <summary>
        /// Ação a realizar ao encontrar cep pesquisado
        /// </summary>
        /// <param name="cep">cep encontrado</param>
        /// <param name="logradouro">logradouro selecionado</param>
        void ucBuscarCEP_LogradouroSelecionado(string cep, string logradouro)
        {
            txtCidade.Text = string.Empty;
            txtLogradouro.Text = string.Empty;
            txtBairro.Text = string.Empty;

            HabilitarCamposCEP(true);

            if (!string.IsNullOrEmpty(cep))
            {
                try
                {
                    var objCep = new Cep
                    {
                        sCep = cep,
                        Logradouro = Server.HtmlDecode(logradouro)
                    };

                    Cep.CompletarCEP(ref objCep);

                    lblCidade.Text = txtCidade.Text = String.Format("{0}/{1}", objCep.Cidade, objCep.Estado);
                    PreencherCamposEndereco(objCep.sCep, objCep.Logradouro, txtNumero.Text, txtComplemento.Text, objCep.Bairro);

                }
                catch(Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Falha na busca de CEP");
                }

                if (string.IsNullOrEmpty(txtLogradouro.Text))
                    this.HabilitarCamposCEP(true);
            }
            
            txtNumero.Focus();
            var parametros = new
            {
                StatusCidade = string.IsNullOrEmpty(txtCidade.Text) ? "1" : "0",
                StatusBairro = string.IsNullOrEmpty(txtBairro.Text) ? "1" : "0",
                StatusLogradouro = string.IsNullOrEmpty(txtLogradouro.Text) ? "1" : "0"
            };

            txtCEP.DisplayMensagemErroWS = "none";
            AjustarEventosEndereco();
            upCidadeLabel.Update(); upCidadeTextbox.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "InicializarStatus", "javaScript:InicializarStatus(" + new JSONReflector(parametros) + ");", true);
        }
        #endregion

        #region ucBuscarCEP_VoltarFoco
        protected void ucBuscarCEP_VoltarFoco()
        {
            txtCEP.Focus();
            ScriptManager.RegisterStartupScript(this, GetType(), "AjustarCamposVoltarFoco", "javaScript:AjustarCampos();", true);
        }
        #endregion

        #region btnAdicionarCidade_Click
        protected void btnAdicionarCidade_Click(object sender, EventArgs e)
        {
            try
            {
                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidadeDisponivel.Text, out objCidade))
                {
                    //TODO: Performance, armazenar o idcurriculo na viewstate para evitar Load desnecessario
                    Curriculo objCurriculo;
                    Curriculo.CarregarPorPessoaFisica((int)IdPessoaFisica, out objCurriculo);

                    CurriculoDisponibilidadeCidade objCurriculoDisponibilidadeCidade;
                    if (!CurriculoDisponibilidadeCidade.CarregarPorCurriculoCidade(objCurriculo, objCidade, out objCurriculoDisponibilidadeCidade))
                    {
                        objCurriculoDisponibilidadeCidade = new CurriculoDisponibilidadeCidade
                        {
                            Cidade = objCidade,
                            Curriculo = objCurriculo
                        };
                    }
                    objCurriculoDisponibilidadeCidade.FlagInativo = false;

                    objCurriculoDisponibilidadeCidade.Save();
                    CarregarGridCidade();
                }
                else
                    ExibirMensagem(MensagemAviso._100007, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvCidadeRowDeleting
        protected void gvCidadeRowDeleting(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = ((GridViewRow)(sender as Control).Parent.Parent).RowIndex;
                int id = Convert.ToInt32(gvCidade.DataKeys[rowIndex]["Idf_Curriculo_Disponibilidade_Cidade"]);
                CurriculoDisponibilidadeCidade objCurriculoDisponibilidadeCidade = CurriculoDisponibilidadeCidade.LoadObject(id);
                objCurriculoDisponibilidadeCidade.FlagInativo = true;
                objCurriculoDisponibilidadeCidade.Save();
                CarregarGridCidade();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region rptCidadeDisponivel_ItemCommand
        protected void rptCidadeDisponivel_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            divCidadeLabel.Style["display"] = "none";
            divCidadeTextBox.Style["display"] = "block";

            upCidadeDisponivel.Update();
        }
        #endregion

        #region bntAdicionarVeiculo_Click
        /// <summary>
        /// Adiciona o Veículo na Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bntAdicionarVeiculo_Click(object sender, EventArgs e)
        {
            try
            {
                PessoaFisicaVeiculo objPessoaFisicaVeiculo = IdPessoaFisicaVeiculo.HasValue ? PessoaFisicaVeiculo.LoadObject(IdPessoaFisicaVeiculo.Value) : new PessoaFisicaVeiculo();
                objPessoaFisicaVeiculo.PessoaFisica = new PessoaFisica(IdPessoaFisica.Value);
                objPessoaFisicaVeiculo.AnoVeiculo = Convert.ToInt16(txtAnoVeiculo.Valor);
                objPessoaFisicaVeiculo.DescricaoModelo = txtModelo.Valor;
                objPessoaFisicaVeiculo.TipoVeiculo = new TipoVeiculo(Convert.ToInt32(ddlTipoVeiculo.SelectedValue));
                objPessoaFisicaVeiculo.Save();
                CarregarGridVeiculos();
                LimparCamposVeiculo();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }

        #region gvModeloAno_ItemCommand
        protected void gvModeloAno_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    int id = Convert.ToInt32(gvModeloAno.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Veiculo"].ToString());
                    PessoaFisicaVeiculo objPessoaFisicaVeiculo = PessoaFisicaVeiculo.LoadObject(id);
                    objPessoaFisicaVeiculo.FlagInativo = true;
                    objPessoaFisicaVeiculo.Save();
                    CarregarGridVeiculos();
                }
                else if (e.CommandName.Equals("RowClick"))
                {
                    int id = Convert.ToInt32(gvModeloAno.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Veiculo"].ToString());
                    IdPessoaFisicaVeiculo = id;
                    PreencherCamposVeiculo();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvFormacao

        #region gvFormacao_ItemCommand
        protected void gvFormacao_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    int idFormacao = Convert.ToInt32(gvFormacao.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Formacao"]);
                    Formacao objFormacao = Formacao.LoadObject(idFormacao);
                    objFormacao.FlagInativo = true;
                    objFormacao.Save();
                    IdFormacao = null;
                    CarregarGridFormacao(objFormacao.PessoaFisica);
                    CarregarNivelEscolaridade();

                    AjustarJavascriptFormacao(this.gvFormacao);
                }
                else if (e.CommandName.Equals("RowClick") || e.CommandName.Equals("Editar"))
                {
                    int idFormacao = Convert.ToInt32(gvFormacao.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Formacao"]);

                    PreencherCamposFormacao(idFormacao);

                    upFormacao.Update();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region gvEspecializacao

        #region gvEspecializacao_ItemCommand
        protected void gvEspecializacao_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    int idFormacao = Convert.ToInt32(gvEspecializacao.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Formacao"]);
                    Formacao objFormacao = Formacao.LoadObject(idFormacao);
                    objFormacao.FlagInativo = true;
                    objFormacao.Save();
                    IdEspecializacao = null;
                    CarregarGridEspecializacao(objFormacao.PessoaFisica);
                    CarregarNivelEscolaridade();
                }
                else if (e.CommandName.Equals("RowClick") || e.CommandName.Equals("Editar"))
                {
                    int idFormacao = Convert.ToInt32(gvEspecializacao.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Formacao"]);
                    IdEspecializacao = idFormacao;
                    LimparCamposEspecializacao();
                    PreencherCamposEspecializacao();

                    upEspecializacao.Update();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region gvIdiomas

        #region gvIdioma_ItemCommand
        protected void gvIdioma_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    int idIdioma = Convert.ToInt32(gvIdioma.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pessoa_Fisica_Idioma"]);

                    PessoafisicaIdioma objPessoaFisicaIdioma = PessoafisicaIdioma.LoadObject(idIdioma);
                    objPessoaFisicaIdioma.FlagInativo = true;
                    objPessoaFisicaIdioma.Save();
                    CarregarGridIdioma(objPessoaFisicaIdioma.PessoaFisica);
                }
                else if (e.CommandName.Equals("RowClick"))
                {
                    int idIdioma = Convert.ToInt32(gvIdioma.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pessoa_Fisica_Idioma"]);

                    PessoafisicaIdioma objPessoaFisicaIdioma = PessoafisicaIdioma.LoadObject(idIdioma);
                    ddlIdioma.SelectedValue = objPessoaFisicaIdioma.Idioma.IdIdioma.ToString();
                    rblNivelIdioma.SelectedValue = objPessoaFisicaIdioma.NivelIdioma.IdNivelIdioma.ToString();

                    IdIdioma = objPessoaFisicaIdioma.IdPessoaFisicaIdioma;

                    upIdiomas.Update();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region gvComplementar

        #region gvComplementar_ItemCommand
        protected void gvComplementar_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    int idFormacao = Convert.ToInt32(gvComplementar.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Formacao"]);
                    Formacao objFormacao = Formacao.LoadObject(idFormacao);
                    objFormacao.FlagInativo = true;
                    objFormacao.Save();
                    IdComplementar = null;
                    CarregarGridComplementar(objFormacao.PessoaFisica);
                }
                else if (e.CommandName.Equals("RowClick") || e.CommandName.Equals("Editar"))
                {
                    int idFormacao = Convert.ToInt32(gvComplementar.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Formacao"]);
                    IdComplementar = idFormacao;
                    PreencherCamposComplementar();

                    upFormacao.Update();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region rptIdiomas_ItemCommand
        protected void rptIdiomas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            divIdiomasLabel.Style["display"] = "none";
            divIdiomasTextBox.Style["display"] = "block";

            upIdiomas.Update();
        }
        #endregion

        #region rptCursos_ItemCommand
        protected void rptCursos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            divFormacaoLabel.Style["display"] = "none";
            divFormacaoTextBox.Style["display"] = "block";

            AjustarJavascriptFormacao(this.upFormacao);

            upFormacao.Update();
        }
        #endregion

        #region rptEspecializacao_ItemCommand
        protected void rptEspecializacao_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            divEspecializacaoLabel.Style["display"] = "none";
            divEspecializacaoTextBox.Style["display"] = "block";

            upEspecializacao.Update();
        }
        #endregion

        #region rptComplementar_ItemCommand
        protected void rptComplementar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            divComplementaresLabel.Style["display"] = "none";
            divComplementaresTextBox.Style["display"] = "block";

            upComplementares.Update();
        }
        #endregion

        #region rptModeloAno_ItemCommand
        protected void rptModeloAno_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            divTipoVeiculoLabel.Style["display"] = "none";
            divTipoVeiculoTextBox.Style["display"] = "block";

            upTipoVeiculo.Update();
        }
        #endregion

        #region cvNivelIdioma_ServerValidate
        protected void cvNivelIdioma_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Convert.ToInt32(ddlIdioma.SelectedValue) < 1 || (String.IsNullOrEmpty(rblNivelIdioma.SelectedValue) || Convert.ToInt32(rblNivelIdioma.SelectedValue) < 1))
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        #endregion

        #region btnSalvarIdioma_Click
        protected void btnSalvarIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarIdioma();
                LimparCamposIdiomas();
                CarregarGridIdioma(new PessoaFisica(IdPessoaFisica.Value));

                upIdiomas.Update();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvarExperiencia_Click
        protected void btnSalvarExperiencia_Click(object sender, EventArgs e)
        {
            try
            {
                //validar data de demissão das experiências
                if (!ValidarDataDemissaoExperiencias())
                {
                    ExibirMensagem("A data de demissão não pode ser menor que a data de admissão, confira seus dados.", TipoMensagem.Erro);
                }
                else
                {
                    SalvarExperienciaProfissional(txtEmpresa.Text, ddlAtividadeEmpresa.SelectedValue, txtDataAdmissao.ValorDatetime, txtDataDemissao.ValorDatetime, txtFuncaoExercida.Text, txtAtividadeExercida.Valor, IdExperienciaProfissional);
                    LimparCamposExperiencia();
                    PreencherExperienciaProfissional(new PessoaFisica(IdPessoaFisica.Value));

                    divExperienciaProfissionalTextBox.Style["display"] = "none";
                    divExperienciaProfissionalLabel.Style["display"] = "block";

                    upExperienciaProfissional.Update();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvarFormacao_Click
        /// <summary>
        /// Evento disparado quando o botão avançar é clicado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvarFormacao_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarFormacao();
                LimparCamposFormacao();
                IdFormacao = null;
                CarregarGridFormacao(new PessoaFisica(IdPessoaFisica.Value));
                CarregarNivelEscolaridade();

                //divFormacaoTextBox.Style["display"] = "none";
                //divFormacaoLabel.Style["display"] = "block";

                upFormacao.Update();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvarEspecializacao_Click
        /// <summary>
        /// Evento disparado quando o botão avançar é clicado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvarEspecializacao_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarEspecializacao();
                LimparCamposEspecializacao();
                IdEspecializacao = null;
                CarregarGridEspecializacao(new PessoaFisica(IdPessoaFisica.Value));
                upEspecializacao.Update();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvarCursoComplementar_Click
        /// <summary>
        /// Evento disparado no click do btnSalvar
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnSalvarCursoComplementar_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarCursoComplementar();
                LimparCamposCursos();
                CarregarGridComplementar(new PessoaFisica(IdPessoaFisica.Value));
                txtInstituicaoComplementar.Focus();

                upComplementares.Update();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ucEnvioCurriculo_EnviarConfirmacao
        void ucEnvioCurriculo_EnviarConfirmacao()
        {
            ((Principal)this.Page.Master).ExibirMensagemConfirmacao("Confirmação", "Currículo enviado com sucesso!", false);
        }
        #endregion

        #endregion

        #region ucFoto_Error
        protected void ucFoto_Error(Exception ex)
        {
            if (ex is ImageSlicerException)
            {
                ExibirMensagem(ex.Message, TipoMensagem.Erro);
            }
            else
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnExisteFotoWS_Click
        protected void btlExisteFotoWS_Click(object sender, EventArgs e)
        {
            ucFoto.ImageData = FotoWSBytes;
            FotoWSBytes = null;
            btlExisteFotoWS.Visible = false;

            upFoto.Update();
        }
        #endregion

        #endregion

        #region Métodos

        #region ValidarDataDemissaoExperiencias

        /// <summary>
        /// Valida a data de demissão não deixando ser menor que a data de admissão.
        /// </summary>
        /// <returns></returns>
        private bool ValidarDataDemissaoExperiencias()
        {
            bool bolDtaValida = true;
            bolDtaValida = (txtDataAdmissao.ValorDatetime != null ? (txtDataAdmissao.ValorDatetime > txtDataDemissao.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);

            return bolDtaValida;
        }

        #endregion

        #region Inicializar
        /// <summary>
        /// Método utilizado para para preenchimento de componentes, funções de foco e navegação
        /// </summary>
        private void Inicializar()
        {
            InicializarCamposFormacao();

            btlGestao.Visible = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue;

            revEmail.ValidationExpression = Configuracao.regexEmail;
            //revMSN.ValidationExpression = Configuracao.regexEmail;

            //Aplicação de Regra: 
            //- Intervalo da Data de Nascimento. ( 14 anos >= idade <= 80 anos )
            int idadeMinima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMinima));
            int idadeMaxima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMaxima));

            DateTime dataAtual = DateTime.Now;

            DateTime dataMinima = DateTime.MinValue;
            DateTime dataMaxima = DateTime.MaxValue;
            try
            {
                dataMinima = Convert.ToDateTime(dataAtual.Day.ToString() + "/" + dataAtual.Month.ToString() + "/" + (dataAtual.Year - idadeMaxima).ToString());
                dataMaxima = Convert.ToDateTime(dataAtual.Day.ToString() + "/" + dataAtual.Month.ToString() + "/" + (dataAtual.Year - idadeMinima).ToString());
            }
            catch
            {
                dataMinima = DateTime.MinValue;
                dataMaxima = DateTime.MaxValue;
            }

            txtDataDeNascimento.DataMinima = dataMinima;
            txtDataDeNascimento.DataMaxima = dataMaxima;
            txtDataDeNascimento.MensagemErroIntervalo = String.Format(MensagemAviso._100005, idadeMinima, idadeMaxima);

            txtPretensaoSalarial.ValorMinimo = Convert.ToDecimal(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioMinimoNacional));

            //[Obsolete("Obtado por não utilização/disponibilização.")]
            //UIHelper.CarregarCheckBoxList(this.chblTipoContrato, TipoVinculo.ListarTipoVinculo());
            CarregarDropDown();
            CarregarRadioButtonList();
            PreencherCampos();

            HabilitarCamposCEP(ValidarVariosEnderecosPorCEP(txtCEP.Valor));

            AjustarEventos();

            UIHelper.ValidateFocus(btnSalvar);
            UIHelper.ValidateFocus(btlDadosComplementares);
            UIHelper.ValidateFocus(btlFormacaoCursos);
            UIHelper.ValidateFocus(btlMiniCurriculo);
            UIHelper.ValidateFocus(btnImprimirCurriculo);
            UIHelper.ValidateFocus(btnEnviarCurriculo);

            var parametros = new
            {
                msgFaixaSalarial = MensagemAviso._202001
            };
            ScriptManager.RegisterStartupScript(this, GetType(), "InicializarAutoCompleteMiniCurriculo", "javaScript:InicializarAutoCompleteMiniCurriculo(" + new JSONReflector(parametros) + ");", true);
        }
        #endregion

        #region InicializarCamposFormacao
        private void InicializarCamposFormacao()
        {
            var parametros = new
            {
                rfvInstituicao = this.rfvInstituicao.ClientID,
                rfvCidade = this.rfvCidadeFormacao.ClientID,
                rfvTituloCurso = this.rfvTituloCurso.ClientID,
                cvTituloCurso = this.cvTituloCurso.ClientID,
                cvSituacao = this.cvSituacao.ClientID,
                txtPeriodo = this.txtPeriodo.ClientID,
                txtAnoConclusao = this.txtAnoConclusao.ClientID
            };
            ScriptManager.RegisterStartupScript(this, GetType(), "InicializarCamposFormacao", "javaScript:InicializarCamposFormacao(" + new JSONReflector(parametros) + ");", true);
        }
        #endregion

        #region Salvar
        /// <summary>
        /// Método utilizado para salvar os dados preenchidos no formulário
        /// </summary>
        private bool Salvar()
        {

            #region Objetos

            PessoaFisica objPessoaFisica;
            Curriculo objCurriculo;
            PessoaFisicaFoto objPessoaFisicaFoto;
            UsuarioFilialPerfil objUsuarioFilialPerfil;
            CurriculoOrigem objCurriculoOrigem = null;
            PessoaFisicaComplemento objPessoaFisicaComplemento;
            Contato objContatoTelefone, objContatoCelular;
            var lstCurriculoDisponibilidade = new List<CurriculoDisponibilidade>();
            var listRedeSocial = new List<PessoaFisicaRedeSocial>();

            #endregion

            #region Instanciando objetos

            if (this.IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(this.IdPessoaFisica.Value);

                if (!PessoaFisicaFoto.CarregarFoto(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaFoto))
                    objPessoaFisicaFoto = new PessoaFisicaFoto();

                if (objPessoaFisica.Endereco != null)
                    objPessoaFisica.Endereco.CompleteObject();
                else
                    objPessoaFisica.Endereco = new Endereco();

                if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objPessoaFisicaComplemento))
                    objPessoaFisicaComplemento = new PessoaFisicaComplemento();

                if (!Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                    objCurriculo = new Curriculo();

                if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objPessoaFisica, out objUsuarioFilialPerfil))
                {
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                    //Como é a criação de um novo usuario filial perfil no cadastro de curriculo então é dado perfil de Acesso Não VIP
                    objUsuarioFilialPerfil.Perfil = new BNE.BLL.Perfil((int)Enumeradores.Perfil.AcessoNaoVIP);
                }
            }
            else
            {
                objPessoaFisica = new PessoaFisica();
                objCurriculo = new Curriculo();
                objPessoaFisicaFoto = new PessoaFisicaFoto();
                objPessoaFisica.Endereco = new Endereco();
                objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                //Como é a criação de um novo usuario filial perfil no cadastro de curriculo então é dado perfil de Acesso Não VIP
                objUsuarioFilialPerfil.Perfil = new BNE.BLL.Perfil((int)Enumeradores.Perfil.AcessoNaoVIP);
            }
            //objContatoCelular = new Contato(); objContatoTelefone = new Contato();
            #endregion

            #region Funções Pretendidadas

            List<FuncaoPretendida> listFuncoesPretendidas = new List<FuncaoPretendida>();
            SalvarFuncoesPretendidas(listFuncoesPretendidas);

            List<int?> listIdFuncoesPretendidas = new List<int?>();
            listIdFuncoesPretendidas.Add(IdFuncaoPretendida1);
            listIdFuncoesPretendidas.Add(IdFuncaoPretendida2);
            listIdFuncoesPretendidas.Add(IdFuncaoPretendida3);

            #endregion

            #region [ Tipo Contrato ]
            //[Obsolete("Obtado por não utilização/disponibilização.")]
            //var listTipoContrato = RetornaTipoContratoMarcados();
            #endregion

            bool limparLocalizacao;

            #region Pessoa Física
            SalvarDadosPessoaFisica(objPessoaFisica, out limparLocalizacao);
            #endregion

            #region Pessoa Física Complemento
            SalvarDadosPessoaFisicaComplemento(objPessoaFisicaComplemento);
            #endregion

            #region Currículo
            SalvarDadosCurriculo(objCurriculo, limparLocalizacao, out objCurriculoOrigem);
            #endregion

            #region SalvarDadosCurriculoDisponibilidade
            SalvarDadosCurriculoDisponibilidade(lstCurriculoDisponibilidade, objCurriculo);
            #endregion

            //#region Redes Sociais

            //SalvarRedesSociais(objPessoaFisica, listRedeSocial, objCurriculo);

            //#endregion

            #region SalvarContato
            SalvarDadosContato(objPessoaFisica, out objContatoTelefone, out objContatoCelular);
            #endregion

            #region ExperienciaProfissional

            //ExperienciaProfissional objExperienciaProfissional1 = SalvarExperienciasProfissionais(txtEmpresa1.Text, ddlAtividadeEmpresa1.SelectedValue, txtDataAdmissao1.ValorDatetime, txtDataDemissao1.ValorDatetime, txtFuncaoExercida1.Text, txtAtividadeExercida1.Valor, IdExperienciaProfissional1);
            //ExperienciaProfissional objExperienciaProfissional2 = SalvarExperienciasProfissionais(txtEmpresa2.Text, ddlAtividadeEmpresa2.SelectedValue, txtDataAdmissao2.ValorDatetime, txtDataDemissao2.ValorDatetime, txtFuncaoExercida2.Text, txtAtividadeExercida2.Valor, IdExperienciaProfissional2);
            //ExperienciaProfissional objExperienciaProfissional3 = SalvarExperienciasProfissionais(txtEmpresa3.Text, ddlAtividadeEmpresa3.SelectedValue, txtDataAdmissao3.ValorDatetime, txtDataDemissao3.ValorDatetime, txtFuncaoExercida3.Text, txtAtividadeExercida3.Valor, IdExperienciaProfissional3);

            #endregion

            #region SalvarDisponibilidadeMudarCidade
            SalvarDisponibilidadeMudarCidade(objCurriculo, objPessoaFisicaComplemento);
            #endregion

            #region Foto
            if (ucFoto.ImageData != null && !ucFoto.ImageData.Length.Equals(0))
            {
                objPessoaFisicaFoto.ImagemPessoa = ucFoto.ImageData;
                objPessoaFisicaFoto.FlagInativo = false;
            }
            else
            {
                objPessoaFisicaFoto.ImagemPessoa = null;
                objPessoaFisicaFoto.FlagInativo = true;
            }
            #endregion

            #region Salvar
            objCurriculo.SalvarCurriculoCompleto(objPessoaFisica, objPessoaFisicaComplemento, lstCurriculoDisponibilidade, objContatoTelefone, objContatoCelular, listFuncoesPretendidas, listIdFuncoesPretendidas, objCurriculoOrigem, objUsuarioFilialPerfil, listRedeSocial, objPessoaFisicaFoto);
            #endregion

            //Ajustando o ID da Pessoa Física
            this.IdPessoaFisica = objPessoaFisica.IdPessoaFisica;
            //Ajustando o ID do Curriculo
            base.IdCurriculo.Value = objCurriculo.IdCurriculo;

            Handlers.PessoaFisicaFoto.RemoverFotoCache(objPessoaFisica.NumeroCPF);

            return true;
        }
        #endregion

        #region SalvarDadosPessoaFisica
        private void SalvarDadosPessoaFisica(PessoaFisica objPessoaFisica, out bool limparLocalizacao)
        {
            limparLocalizacao = false;

            //objPessoaFisica.NumeroCPF = txtCPF.Valor;
            objPessoaFisica.DataNascimento = DateTime.Parse(this.txtDataDeNascimento.Valor);
            objPessoaFisica.NomePessoa = UIHelper.AjustarString(this.txtNome.Valor);
            objPessoaFisica.NomePessoaPesquisa = UIHelper.RemoverAcentos(this.txtNome.Valor);
            objPessoaFisica.Sexo = new Sexo(Convert.ToInt32(rblSexo.SelectedValue));
            objPessoaFisica.NumeroDDDCelular = txtTelefoneCelular.DDD;
            objPessoaFisica.NumeroCelular = txtTelefoneCelular.Fone;
            objPessoaFisica.NumeroDDDTelefone = txtTelefoneResidencial.DDD;
            objPessoaFisica.NumeroTelefone = txtTelefoneResidencial.Fone;
            objPessoaFisica.FlagInativo = false;
            objPessoaFisica.EmailPessoa = txtEmail.Text;

            objPessoaFisica.DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //Se houve alteração do CEP
            if ((!string.IsNullOrEmpty(objPessoaFisica.Endereco.NumeroCEP) && !objPessoaFisica.Endereco.NumeroCEP.Equals(txtCEP.Valor)))
                limparLocalizacao = true;

            //Endereco
            objPessoaFisica.Endereco.NumeroCEP = txtCEP.Valor;
            objPessoaFisica.Endereco.DescricaoLogradouro = txtLogradouro.Text;
            objPessoaFisica.Endereco.NumeroEndereco = txtNumero.Text;
            objPessoaFisica.Endereco.DescricaoComplemento = txtComplemento.Text;
            objPessoaFisica.Endereco.DescricaoBairro = txtBairro.Text;

            Cidade objCidade;
            if (Cidade.CarregarPorNome(txtCidade.Text, out objCidade))
                objPessoaFisica.Endereco.Cidade = objCidade;

            objPessoaFisica.Cidade = objCidade;

            //objPessoaFisica.NumeroRG = txtNumeroRG.Valor;
            //objPessoaFisica.NomeOrgaoEmissor = txtOrgaoEmissorRG.Valor;

            if (!ddlEstadoCivil.SelectedValue.Equals("0"))
                objPessoaFisica.EstadoCivil = new EstadoCivil(Convert.ToInt32(ddlEstadoCivil.SelectedValue));

            //Raça
            if (ddlRaca.SelectedValue.Equals("0"))
                objPessoaFisica.Raca = null;
            else
                objPessoaFisica.Raca = new Raca(Convert.ToInt32(ddlRaca.SelectedValue));

            //Deficiência
            if (!ddlDeficiencia.SelectedValue.Equals("0"))
                objPessoaFisica.Deficiencia = new Deficiencia(Convert.ToInt32(ddlDeficiencia.SelectedValue));
            else
                objPessoaFisica.Deficiencia = null;
        }
        #endregion

        #region SalvarDadosCurriculo
        private void SalvarDadosCurriculo(Curriculo objCurriculo, bool limparGeolocalizacao, out CurriculoOrigem objCurriculoOrigem)
        {
            objCurriculoOrigem = null;

            //Currículo 
            objCurriculo.ObservacaoCurriculo = txtObservacoes.Valor.UndoReplaceBreaks();
            objCurriculo.ValorPretensaoSalarial = Convert.ToDecimal(txtPretensaoSalarial.Valor);

            if (limparGeolocalizacao)
                objCurriculo.DescricaoLocalizacao = null;
        }
        #endregion

        #region SalvarDisponibilidadeMudarCidade
        private void SalvarDisponibilidadeMudarCidade(Curriculo objCurriculo, PessoaFisicaComplemento objPessoaFisicaComplemento)
        {
            objPessoaFisicaComplemento.FlagMudanca = false;
            if (!String.IsNullOrEmpty(txtCidadeDisponivel.Text))
            {
                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidadeDisponivel.Text, out objCidade))
                {
                    objCurriculo.CidadePretendida = objCidade;
                    objPessoaFisicaComplemento.FlagMudanca = true;
                }
                else
                    objCurriculo.CidadePretendida = null;
            }
        }
        #endregion

        #region SalvarDadosCurriculoDisponibilidade
        private void SalvarDadosCurriculoDisponibilidade(List<CurriculoDisponibilidade> lstCurriculoDisponibilidade, Curriculo objCurriculo)
        {
            CurriculoDisponibilidade objCurriculoDisponibilidade;

            if (ckblDisponibilidade.Items.FindByText("Manhã").Selected)
            {
                objCurriculoDisponibilidade = new CurriculoDisponibilidade();
                objCurriculoDisponibilidade.Curriculo = objCurriculo;
                objCurriculoDisponibilidade.Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Manha);
                lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
            }

            if (ckblDisponibilidade.Items.FindByText("Tarde").Selected)
            {
                objCurriculoDisponibilidade = new CurriculoDisponibilidade();
                objCurriculoDisponibilidade.Curriculo = objCurriculo;
                objCurriculoDisponibilidade.Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Tarde);
                lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
            }

            if (ckblDisponibilidade.Items.FindByText("Noite").Selected)
            {
                objCurriculoDisponibilidade = new CurriculoDisponibilidade();
                objCurriculoDisponibilidade.Curriculo = objCurriculo;
                objCurriculoDisponibilidade.Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Noite);
                lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
            }

            if (ckblDisponibilidade.Items.FindByText("Sábado").Selected)
            {
                objCurriculoDisponibilidade = new CurriculoDisponibilidade();
                objCurriculoDisponibilidade.Curriculo = objCurriculo;
                objCurriculoDisponibilidade.Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Sabado);
                lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
            }

            if (ckblDisponibilidade.Items.FindByText("Domingo").Selected)
            {
                objCurriculoDisponibilidade = new CurriculoDisponibilidade();
                objCurriculoDisponibilidade.Curriculo = objCurriculo;
                objCurriculoDisponibilidade.Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Domingo);
                lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
            }

            if (ckbDisponibilidadeViagem.Checked)
            {
                objCurriculoDisponibilidade = new CurriculoDisponibilidade();
                objCurriculoDisponibilidade.Curriculo = objCurriculo;
                objCurriculoDisponibilidade.Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Viagem);
                lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
            }
        }
        #endregion

        #region SalvarDadosContato
        private void SalvarDadosContato(PessoaFisica objPessoaFisica, out Contato objContatoTelefone, out Contato objContatoCelular)
        {
            if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, null))
            {
                if (!String.IsNullOrEmpty(txtTelefoneRecado.Fone) && !String.IsNullOrEmpty(txtTelefoneRecado.DDD))
                {
                    objContatoTelefone.NumeroDDDTelefone = txtTelefoneRecado.DDD;
                    objContatoTelefone.NumeroTelefone = txtTelefoneRecado.Fone;
                    objContatoTelefone.NomeContato = txtTelefoneRecadoFalarCom.Text;
                    objContatoTelefone.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoFixo);
                }
                else
                {
                    objContatoTelefone.NumeroDDDTelefone = string.Empty;
                    objContatoTelefone.NumeroTelefone = string.Empty;
                    objContatoTelefone.NomeContato = string.Empty;
                    objContatoTelefone.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoFixo);
                }
            }
            else
            {
                objContatoTelefone = new Contato();
                objContatoTelefone.NumeroDDDTelefone = txtTelefoneRecado.DDD;
                objContatoTelefone.NumeroTelefone = txtTelefoneRecado.Fone;
                objContatoTelefone.NomeContato = txtTelefoneRecadoFalarCom.Text;
                objContatoTelefone.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoFixo);
            }

            if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoCelular, out objContatoCelular, null))
            {
                if (!String.IsNullOrEmpty(txtCelularRecado.Fone))
                {
                    objContatoCelular.NumeroDDDCelular = txtCelularRecado.DDD;
                    objContatoCelular.NumeroCelular = txtCelularRecado.Fone;
                    objContatoCelular.NomeContato = txtCelularRecadoFalarCom.Text;
                    objContatoCelular.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoCelular);
                }
                else
                {
                    objContatoCelular.NumeroDDDCelular = string.Empty;
                    objContatoCelular.NumeroCelular = string.Empty;
                    objContatoCelular.NomeContato = string.Empty;
                    objContatoCelular.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoCelular);
                }
            }
            else
            {
                objContatoCelular = new Contato();
                objContatoCelular.NumeroDDDCelular = txtCelularRecado.DDD;
                objContatoCelular.NumeroCelular = txtCelularRecado.Fone;
                objContatoCelular.NomeContato = txtCelularRecadoFalarCom.Text;
                objContatoCelular.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoCelular);
            }
        }
        #endregion

        #region SalvarFuncoesPretendidas
        private void SalvarFuncoesPretendidas(List<FuncaoPretendida> listFuncoesPretendidas)
        {
            if (String.IsNullOrEmpty(txtFuncaoPretendida1.Text))
                listFuncoesPretendidas.Add(null);
            else
            {
                FuncaoPretendida objFuncaoPretendida = IdFuncaoPretendida1.HasValue ? FuncaoPretendida.LoadObject(IdFuncaoPretendida1.Value) : new FuncaoPretendida();
                Funcao objFuncao;
                if (Funcao.CarregarPorDescricao(txtFuncaoPretendida1.Text, out objFuncao))
                {
                    objFuncaoPretendida.Funcao = objFuncao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else
                {
                    objFuncaoPretendida.Funcao = null;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = txtFuncaoPretendida1.Text;
                }

                if (!objFuncaoPretendida.QuantidadeExperiencia.HasValue)
                    objFuncaoPretendida.QuantidadeExperiencia = 0;

                listFuncoesPretendidas.Add(objFuncaoPretendida);
            }

            if (String.IsNullOrEmpty(txtFuncaoPretendida2.Text))
                listFuncoesPretendidas.Add(null);
            else
            {
                FuncaoPretendida objFuncaoPretendida = IdFuncaoPretendida2.HasValue ? FuncaoPretendida.LoadObject(IdFuncaoPretendida2.Value) : new FuncaoPretendida();
                Funcao objFuncao;
                if (Funcao.CarregarPorDescricao(txtFuncaoPretendida2.Text, out objFuncao))
                {
                    objFuncaoPretendida.Funcao = objFuncao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else
                {
                    objFuncaoPretendida.Funcao = null;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = txtFuncaoPretendida2.Text;
                }

                if (!objFuncaoPretendida.QuantidadeExperiencia.HasValue)
                    objFuncaoPretendida.QuantidadeExperiencia = 0;

                listFuncoesPretendidas.Add(objFuncaoPretendida);
            }

            if (String.IsNullOrEmpty(txtFuncaoPretendida3.Text))
                listFuncoesPretendidas.Add(null);
            else
            {
                FuncaoPretendida objFuncaoPretendida = IdFuncaoPretendida3.HasValue ? FuncaoPretendida.LoadObject(IdFuncaoPretendida3.Value) : new FuncaoPretendida();
                Funcao objFuncao;
                if (Funcao.CarregarPorDescricao(txtFuncaoPretendida3.Text, out objFuncao))
                {
                    objFuncaoPretendida.Funcao = objFuncao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else
                {
                    objFuncaoPretendida.Funcao = null;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = txtFuncaoPretendida3.Text;
                }

                if (!objFuncaoPretendida.QuantidadeExperiencia.HasValue)
                    objFuncaoPretendida.QuantidadeExperiencia = 0;

                listFuncoesPretendidas.Add(objFuncaoPretendida);
            }
        }
        #endregion

        #region SalvarExperienciaProfissional
        private void SalvarExperienciaProfissional(string txtEmpresa, string ddlAtividadeEmpresa, DateTime? txtDataAdmissao, DateTime? txtDataDemissao, string txtFuncaoExercida, string txtAtividadeExercida, int? idExperienciaProfessional)
        {
            ExperienciaProfissional objExperienciaProfissional;
            if (!String.IsNullOrEmpty(txtEmpresa))
            {
                if (idExperienciaProfessional.HasValue)
                    objExperienciaProfissional = ExperienciaProfissional.LoadObject(idExperienciaProfessional.Value);
                else
                    objExperienciaProfissional = new ExperienciaProfissional();

                objExperienciaProfissional.PessoaFisica = new PessoaFisica(this.IdPessoaFisica.Value);
                objExperienciaProfissional.RazaoSocial = txtEmpresa;
                objExperienciaProfissional.AreaBNE = new AreaBNE(Convert.ToInt32(ddlAtividadeEmpresa));
                objExperienciaProfissional.DataAdmissao = (DateTime)txtDataAdmissao;
                objExperienciaProfissional.DataDemissao = txtDataDemissao;
                objExperienciaProfissional.VlrSalario = txtUltimoSalario.Valor;
                Funcao objFuncao;
                if (Funcao.CarregarPorDescricao(txtFuncaoExercida, out objFuncao))
                {
                    objExperienciaProfissional.Funcao = objFuncao;
                    objExperienciaProfissional.DescricaoFuncaoExercida = String.Empty;
                }
                else
                {
                    objExperienciaProfissional.Funcao = null;
                    objExperienciaProfissional.DescricaoFuncaoExercida = txtFuncaoExercida;
                }

                objExperienciaProfissional.DescricaoAtividade = txtAtividadeExercida;
                objExperienciaProfissional.Save();
            }
            else if (idExperienciaProfessional.HasValue)
            {
                objExperienciaProfissional = ExperienciaProfissional.LoadObject(idExperienciaProfessional.Value);
                objExperienciaProfissional.FlagInativo = true;
                objExperienciaProfissional.Save();
            }

            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objCurriculo))
            {
                objCurriculo.ValorUltimoSalario = txtUltimoSalario.Valor;
                objCurriculo.Save();
            }
        }
        #endregion

        #region SalvarDadosPessoaFisicaComplemento
        private void SalvarDadosPessoaFisicaComplemento(PessoaFisicaComplemento objPessoaFisicaComplemento)
        {
            if (ddlHabilitacao.SelectedValue.Equals("0"))
                objPessoaFisicaComplemento.CategoriaHabilitacao = null;
            else
                objPessoaFisicaComplemento.CategoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(ddlHabilitacao.SelectedValue));

            //objPessoaFisicaComplemento.NumeroHabilitacao = txtHabilitacao.Valor;
            objPessoaFisicaComplemento.NumeroAltura = txtAltura.Valor;
            objPessoaFisicaComplemento.NumeroPeso = txtPeso.Valor;
            objPessoaFisicaComplemento.FlagViagem = ckbDisponibilidadeViagem.Checked;
            objPessoaFisicaComplemento.DescricaoConhecimento = txtOutrosConhecimentos.Valor.UndoReplaceBreaks();

            if (!ddlFilhos.SelectedValue.Equals("-1"))
                objPessoaFisicaComplemento.FlagFilhos = ddlFilhos.SelectedValue.Equals("0") ? false : true;
            else
                objPessoaFisicaComplemento.FlagFilhos = null;
        }
        #endregion

        #region SalvarIdioma
        private void SalvarIdioma()
        {
            if (!ddlIdioma.SelectedValue.Equals("0"))
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                PessoafisicaIdioma objPessoaFisicaIdioma;

                if (!PessoafisicaIdioma.CarregarPorPessoaFisicaIdioma(IdPessoaFisica.Value, Convert.ToInt32(ddlIdioma.SelectedValue), out objPessoaFisicaIdioma))
                    objPessoaFisicaIdioma = new PessoafisicaIdioma();

                objPessoaFisicaIdioma.PessoaFisica = objPessoaFisica;
                objPessoaFisicaIdioma.Idioma = new Idioma(Convert.ToInt32(ddlIdioma.SelectedValue));
                objPessoaFisicaIdioma.NivelIdioma = new NivelIdioma(Convert.ToInt32(rblNivelIdioma.SelectedValue));
                objPessoaFisicaIdioma.FlagInativo = false;
                objPessoaFisicaIdioma.Save();
            }
        }
        #endregion

        #region AjustarEventos
        private void AjustarEventos()
        {
            this.txtFuncaoPretendida1.Attributes["OnChange"] += "FuncaoPretendida_OnChange(this);";
            this.txtFuncaoPretendida1.Attributes["OnBlur"] += "FuncaoPretendida_OnBlur();";

            ddlNivel.Attributes["OnChange"] += "ddlNivel_SelectedIndexChanged(this);";
            ddlNivelEspecializacao.Attributes["OnChange"] += "ddlNivelEspecializacao_SelectedIndexChanged(this);";

            //Nome
            lblNome.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divNomeLabel.ID, txtNome.ClientID);
            txtNome.OnBlurClient += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divNomeTextBox.ID, lblNome.ClientID, txtNome.ClientID);
            //Sexo
            lblSexo.Attributes["onClick"] += "EditarCampo('" + divSexoLabel.ID + "');";
            rblSexo.Attributes["onClick"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divSexoTextBox.ID, lblSexo.ClientID, rblSexo.ClientID);
            //Foto
            //rbiThumbnailVisualizacao.Attributes["onClick"] += "EditarCampo('" + divFotoLabel.ID + "');";
            ucFoto.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divFotoTextBox.ID, ucFoto.ClientID, ucFoto.ClientID);

            //Estado Civil
            lblEstadoCivil.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divEstadoCivilLabel.ID, ddlEstadoCivil.ClientID);
            ddlEstadoCivil.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divEstadoCivilTextBox.ID, lblEstadoCivil.ClientID, ddlEstadoCivil.ClientID);

            //Filhos
            lblFilhos.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divFilhosLabel.ID, ddlFilhos.ClientID);
            ddlFilhos.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divFilhosTextBox.ID, lblFilhos.ClientID, ddlFilhos.ClientID);

            //Habilitação
            lblHabilitacao.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divHabilitacaoLabel.ID, ddlHabilitacao.ClientID);
            ddlHabilitacao.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divHabilitacaoTextBox.ID, lblHabilitacao.ClientID, ddlHabilitacao.ClientID);

            //E-mail
            lblEmail.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divEmailLabel.ID, txtEmail.ClientID);
            txtEmail.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divEmailTextBox.ID, lblEmail.ClientID, txtEmail.ClientID);

            //Objeto auxiliar
            TextBox txt;

            //Telefone Celular
            lblTelefoneCelular.Attributes["onClick"] += "EditarCampo('" + divTelefoneCelularLabel.ID + "');";
            txt = (TextBox)txtTelefoneCelular.Controls[2];
            txt.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divTelefoneCelularTextBox.ID, lblTelefoneCelular.ClientID, txtTelefoneCelular.ClientID);

            //Telefone Residencial
            lblTelefoneResidencial.Attributes["onClick"] += "EditarCampo('" + divTelefoneResidencialLabel.ID + "');";
            txt = (TextBox)txtTelefoneResidencial.Controls[2];
            txt.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divTelefoneResidencialTextBox.ID, lblTelefoneResidencial.ClientID, txtTelefoneResidencial.ClientID);

            //Telefone Recado
            lblTelefoneRecado.Attributes["onClick"] += "EditarCampo('" + divTelefoneRecadoLabel.ID + "');";
            txt = (TextBox)txtTelefoneRecado.Controls[2];
            txt.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divTelefoneRecadoTextBox.ID, lblTelefoneRecado.ClientID, txtTelefoneRecado.ClientID);

            //Telefone Recado Falar Com
            lblTelefoneRecadoFalarCom.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divTelefoneRecadoFalarComLabel.ID, txtTelefoneRecadoFalarCom.ClientID);
            txtTelefoneRecadoFalarCom.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divTelefoneRecadoFalarComTextBox.ID, lblTelefoneRecadoFalarCom.ClientID, txtTelefoneRecadoFalarCom.ClientID);

            //Telefone Celular Recado
            lblCelularRecado.Attributes["onClick"] += "EditarCampo('" + divCelularRecadoLabel.ID + "');";
            txt = (TextBox)txtCelularRecado.Controls[2];
            txt.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divCelularRecadoTextBox.ID, lblCelularRecado.ClientID, txtCelularRecado.ClientID);

            //Telefone Celular Recado Falar Com
            lblCelularRecadoFalarCom.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divCelularRecadoFalarComLabel.ID, txtCelularRecadoFalarCom.ClientID);
            txtCelularRecadoFalarCom.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divCelularRecadoFalarComTextBox.ID, lblCelularRecadoFalarCom.ClientID, txtCelularRecadoFalarCom.ClientID);

            //CEP
            lblCep.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divCEPLabel.ID, txtCEP.ClientID);
            txt = (TextBox)txtCEP.Controls[2];
            txt.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divCEPTextBox.ID, lblCep.ClientID, txtCEP.ClientID);

            //Endereço
            AjustarEventosEndereco();

            //[Obsolete("Obtado por não utilização/disponibilização.")]
            //Tipo Contrato
            //            lblTipoContrato.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divTipoContratoLabel.ID, chblTipoContrato.ClientID);
            //            Page.ClientScript.RegisterStartupScript(this.GetType(), this.GetType().Name,
            //                @"
            //                $(document).ready(function() { if (typeof(ControlarTipoContrato) === 'function') { ControlarTipoContrato(" +
            //                lblTipoContrato.ClientID + ", " + chblTipoContrato.ClientID + "); }});", true);
            //chblTipoContrato.Attributes["ready"] += string.Format("ControlarTipoContrato({0})",chblTipoContrato.ClientID);

            //Funções Pretendidas
            lblFuncaoPretendida1.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divFuncaoPretendida1Label.ID, txtFuncaoPretendida1.ClientID);
            txtFuncaoPretendida1.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divFuncaoPretendida1TextBox.ID, lblFuncaoPretendida1.ClientID, txtFuncaoPretendida1.ClientID);
            lblFuncaoPretendida2.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divFuncaoPretendida2Label.ID, txtFuncaoPretendida2.ClientID);
            txtFuncaoPretendida2.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divFuncaoPretendida2TextBox.ID, lblFuncaoPretendida2.ClientID, txtFuncaoPretendida2.ClientID);
            lblFuncaoPretendida3.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divFuncaoPretendida3Label.ID, txtFuncaoPretendida3.ClientID);
            txtFuncaoPretendida3.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divFuncaoPretendida3TextBox.ID, lblFuncaoPretendida3.ClientID, txtFuncaoPretendida3.ClientID);

            //Pretensão
            lblPretensaoSalarial.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divPretensaoSalarialLabel.ID, txtPretensaoSalarial.ClientID);
            txtPretensaoSalarial.OnBlurClient += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divPretensaoSalarialTextBox.ID, lblPretensaoSalarial.ClientID, txtPretensaoSalarial.ClientID);

            #region Observações

            //Observações
            lblObservacoes.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divObservacoesLabel.ID, txtObservacoes.ClientID);
            txtObservacoes.OnBlurClient += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divObservacoesTextBox.ID, lblObservacoes.ClientID, txtObservacoes.ClientID);

            //Outros Conhecimentos
            lblOutrosConhecimentos.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divOutrosConhecimentosLabel.ID, txtOutrosConhecimentos.ClientID);
            txtOutrosConhecimentos.OnBlurClient += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divOutrosConhecimentosTextBox.ID, lblOutrosConhecimentos.ClientID, txtOutrosConhecimentos.ClientID);

            //Caracteristicas Pessoais
            lblCaracteristicasPessoaisRaca.Attributes["onClick"] += "EditarCampo('" + divCaracteristicasPessoaisLabel.ID + "','" + ddlRaca.ClientID + "');";
            lblCaracteristicasPessoaisAltura.Attributes["onClick"] += "EditarCampo('" + divCaracteristicasPessoaisLabel.ID + "','" + txtAltura.ClientID + "');";
            lblCaracteristicasPessoaisPeso.Attributes["onClick"] += "EditarCampo('" + divCaracteristicasPessoaisLabel.ID + "','" + txtPeso.ClientID + "');";
            //Raça
            ddlRaca.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divCaracteristicasPessoaisTextBox.ID, lblCaracteristicasPessoaisRaca.ClientID, ddlRaca.ClientID);
            txtAltura.OnBlurClient += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divCaracteristicasPessoaisTextBox.ID, lblCaracteristicasPessoaisAltura.ClientID, txtAltura.ClientID);
            txtPeso.OnBlurClient += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divCaracteristicasPessoaisTextBox.ID, lblCaracteristicasPessoaisPeso.ClientID, txtPeso.ClientID);

            //Disponibilidade para Trabalho
            lblDisponibilidade.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divDisponibilidadeLabel.ID, ckblDisponibilidade.ClientID);
            ckblDisponibilidade.Attributes["onClick"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divDisponibilidadeTextBox.ID, lblDisponibilidade.ClientID, ckblDisponibilidade.ClientID);

            //Disponibilidade para Viagem
            lblDisponibilidadeViagem.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divDisponibilidadeViagemLabel.ID, ckbDisponibilidadeViagem.ClientID);
            ckbDisponibilidadeViagem.Attributes["onClick"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divDisponibilidadeViagemTextBox.ID, lblDisponibilidadeViagem.ClientID, ckbDisponibilidadeViagem.ClientID);

            //Disponibilidade para Mudar de Cidade
            lblDisponibilidadeMorarEm.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divDisponibilidadeMorarEmLabel.ID, gvCidade.ClientID);
            //txtCidadeDisponivel.Attributes["onblur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divDisponibilidadeMorarEmTextBox.ID, lblDisponibilidadeMorarEm.ClientID, txtCidadeDisponivel.ClientID);

            //Tipo de veiculo
            lblModeloAno.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divTipoVeiculoLabel.ID, gvModeloAno.ClientID);
            lblIdiomas.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divIdiomasLabel.ID, gvIdioma.ClientID);
            lblComplementares.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divComplementaresLabel.ID, gvComplementar.ClientID);
            lblCursos.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divFormacaoLabel.ID, gvFormacao.ClientID);
            lblEspecializacao.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divEspecializacaoLabel.ID, gvEspecializacao.ClientID);
            //divTipoVeiculoTextBox.Attributes["onblur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divTipoVeiculoTextBox.ID, lblModeloAno.ClientID, gvModeloAno.ClientID);

            //Deficiencia
            lblDeficiencia.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divDeficienciaLabel.ID, ddlDeficiencia.ClientID);
            ddlDeficiencia.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divDeficienciaTextBox.ID, lblDeficiencia.ClientID, ddlDeficiencia.ClientID);

            #endregion

            //Redes sociais
            //lblTwitter.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divTwitterLabel.ID, txtTwitter.ClientID);
            //txtTwitter.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divTwitterTextBox.ID, lblTwitter.ClientID, txtTwitter.ClientID);

            //lblOrkut.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divOrkutLabel.ID, txtOrkut.ClientID);
            //txtOrkut.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divOrkutTextBox.ID, lblOrkut.ClientID, txtOrkut.ClientID);

            //lblFaceBook.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divFaceBookLabel.ID, txtFaceBook.ClientID);
            //txtFaceBook.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divFaceBookTextBox.ID, lblFaceBook.ClientID, txtFaceBook.ClientID);

            //lblMySpace.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divMySpaceLabel.ID, txtMySpace.ClientID);
            //txtMySpace.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divMySpaceTextBox.ID, lblMySpace.ClientID, txtMySpace.ClientID);

            //lblMSN.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divMSNLabel.ID, txtMSN.ClientID);
            //txtMSN.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divMSNTextBox.ID, lblMSN.ClientID, txtMSN.ClientID);

            //lblSkype.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divSkypeLabel.ID, txtSkype.ClientID);
            //txtSkype.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divSkypeTextBox.ID, lblSkype.ClientID, txtSkype.ClientID);

            //lblGTalk.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divGTalkLabel.ID, txtGTalk.ClientID);
            //txtGTalk.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divGTalkTextBox.ID, lblGTalk.ClientID, txtGTalk.ClientID);

            //lblLinkedin.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divLinkedinLabel.ID, txtLinkedin.ClientID);
            //txtLinkedin.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divLinkedinTextBox.ID, lblLinkedin.ClientID, txtLinkedin.ClientID);
        }
        #endregion

        #region AjustarEventosEndereco
        private void AjustarEventosEndereco()
        {
            if (!CEPValido)
            {
                lblLogradouro.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divEnderecoLabel.ID, txtLogradouro.ClientID);
                lblBairro.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divEnderecoLabel.ID, txtBairro.ClientID);
                lblCidade.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divCidadeLabel.ID, txtCidade.ClientID);
            }
            lblComplemento.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divEnderecoLabel.ID, txtComplemento.ClientID);
            lblNumero.Attributes["onClick"] += String.Format("EditarCampo('{0}','{1}');", divEnderecoLabel.ID, txtNumero.ClientID);

            txtLogradouro.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divEnderecoTextBox.ID, lblLogradouro.ClientID, txtLogradouro.ClientID);
            txtBairro.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divEnderecoTextBox.ID, lblBairro.ClientID, txtBairro.ClientID);
            txtCidade.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divCidadeTextBox.ID, lblCidade.ClientID, txtCidade.ClientID);

            txtComplemento.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divEnderecoTextBox.ID, lblComplemento.ClientID, txtComplemento.ClientID);
            txtNumero.Attributes["onBlur"] += String.Format("ExibirValorCampo('{0}','{1}', '{2}');", divEnderecoTextBox.ID, lblNumero.ClientID, txtNumero.ClientID);

            upEndereco.Update();
        }
        #endregion

        #region AjustasCriticasDoCurriculo
        private void AjustasCriticasDoCurriculo(Curriculo objCurriculo)
        {
            DataTable dt = Curriculo.RecuperarCriticas(objCurriculo.IdCurriculo, Resources.Configuracao.MensagemPadraoCriticaValidacao);

            foreach (DataRow dataRow in dt.Rows)
            {
                Control c = FindControl(dataRow["Nme_Campo_Foco"].ToString().Replace("txt", "lbl")); //TODO: HARDCODE
                if (c != null)
                {
                    Label lbl = c as Label;
                    lbl.CssClass += " " + Configuracao.CssLabelNaoInformado;
                    lbl.ToolTip += dataRow["Des_Erro"].ToString() + " ";
                }
            }
        }
        #endregion

        #region AjustarJavascriptFormacao
        private void AjustarJavascriptFormacao(Control controle)
        {
            var parametros = new
            {
                ObrigatorioSigla = 0,
                VisivelSigla = 0,
                ObrigatorioInstituicao = 0,
                VisivelInstituicao = 0,
                ObrigatorioCurso = 0,
                VisivelCurso = 0,
                ObrigatorioLocal = 0,
                VisivelLocal = 0,
                ObrigatorioCidadeEstado = 0,
                VisivelCidadeEstado = 0,
                ObrigatorioSituacao = 0,
                VisivelSituacao = 0,
                ObrigatorioPeriodo = 0,
                VisivelPeriodo = 0,
                ObrigatorioAnoConclusao = 0,
                VisivelAnoConclusao = 0
            };

            ScriptManager.RegisterStartupScript(controle, controle.GetType(), "InicializarStatusAjustarJavascriptFormacao", "javaScript:InicializarStatus( " + new JSONReflector(parametros).ToString() + " );", true);
            ScriptManager.RegisterStartupScript(controle, controle.GetType(), "AjustarCamposAjustarJavascriptFormacao", "javaScript:AjustarCampos();", true);
        }
        #endregion

        #region FindControl
        public T FindControl<T>(string id) where T : Control
        {
            return FindControl<T>(Page, id);
        }

        public static T FindControl<T>(Control startingControl, string id) where T : Control
        {
            /*
            T found = null;

            foreach (Control activeControl in startingControl.Controls)
            {
                found = activeControl as T;

                if (found == null)
                {
                    found = FindControl<T>(activeControl, id);                    
                }
                else if (string.Compare(id, found.ID, true) != 0)
                {
                    found = null;
                }

                if (found != null)
                {
                    break;
                }
            }

            return found;
             */
            // this is null by default
            T found = default(T);

            int controlCount = startingControl.Controls.Count;

            if (controlCount > 0)
            {
                for (int i = 0; i < controlCount; i++)
                {
                    Control activeControl = startingControl.Controls[i];
                    if (activeControl is T)
                    {
                        found = startingControl.Controls[i] as T;
                        if (string.Compare(id, found.ID, true) == 0) break;
                        else found = null;
                    }
                    else
                    {
                        found = FindControl<T>(activeControl, id);
                        if (found != null) break;
                    }
                }
            }
            return found;
        }
        #endregion

        #region CarregarDropDown
        private void CarregarDropDown()
        {
            UIHelper.CarregarDropDownList(ddlEstadoCivil, EstadoCivil.Listar(), new ListItem("Selecione", "0"));
            //Ajustando valor dos items da drop down de filhos
            UIHelper.CarregarDropDownList(ddlFilhos, BNE.BLL.Flag.Listar(), "Idf_Flag", "Des_Flag", new ListItem("Selecione", "-1"));
            UIHelper.CarregarDropDownList(ddlHabilitacao, CategoriaHabilitacao.Listar(), "Idf_Categoria_Habilitacao", "Des_Categoria_Habilitacao", new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlRaca, Raca.Listar(), "Idf_Raca", "Des_Raca_BNE", new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlDeficiencia, Deficiencia.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlTipoVeiculo, TipoVeiculo.Listar(), "Idf_Tipo_Veiculo", "Des_tipo_Veiculo", new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlIdioma, Idioma.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlSituacao, SituacaoFormacao.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlSituacaoEspecializacao, SituacaoFormacao.Listar(), new ListItem("Selecione", "0"));
            CarregarNivelEscolaridade();
        }
        #endregion

        #region CarregarNivelEscolaridade
        /// <summary>
        /// Carrega a CarregarNivelEscolaridade
        /// </summary>
        public void CarregarNivelEscolaridade()
        {
            UIHelper.CarregarDropDownList(ddlNivel, Escolaridade.ListaNivelEducacao(IdPessoaFisica.Value, true), "Idf_Escolaridade", "Des_BNE", new ListItem("Selecione", "0"));
            ddlNivel.SelectedIndex = 0;
            upNivel.Update();

            UIHelper.CarregarDropDownList(ddlNivelEspecializacao, Escolaridade.ListaNivelEducacao(IdPessoaFisica.Value, false), "Idf_Escolaridade", "Des_BNE", new ListItem("Selecione", "0"));
            upNivelEspecializacao.Update();
        }
        #endregion

        #region CarregarRadioButtonList
        private void CarregarRadioButtonList()
        {
            UIHelper.CarregarRadioButtonList(rblSexo, Sexo.Listar());
            UIHelper.CarregarRadioButtonList(rblNivelIdioma, NivelIdioma.Listar(), "Idf_Nivel_Idioma", "Des_Nivel_Idioma");
        }
        #endregion

        #region HabilitarCamposCEP
        private void HabilitarCamposCEP(bool habilitar)
        {
#warning ver este método
            //lblLogradouro.Enabled = 
            //txtLogradouro.Enabled = habilitar;
            //lblBairro.Enabled = 
            //txtBairro.Enabled = habilitar;
            //lblCidade.Enabled = txtCidade.Enabled = habilitar;
            CEPValido = !habilitar;
        }
        #endregion

        #region ExisteGraduacao
        /// <summary>
        /// Validar graduacao
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="siglaUF"></param>
        /// <returns></returns>
        public bool ExisteGraduacao()
        {
            int idPessoaFisica = IdPessoaFisica.Value;
            int idGraduacaoSuperior = 13; //Código identificador de Formacao Graduacao
            int idGraduacaoTecnologo = 12; //Código identificador de Formacao Graduacao

            Formacao objFormacao;
            if (Formacao.CarregarPorPessoaFisicaEscolaridade(idPessoaFisica, idGraduacaoSuperior, out objFormacao) ||
                Formacao.CarregarPorPessoaFisicaEscolaridade(idPessoaFisica, idGraduacaoTecnologo, out objFormacao))
                return true;
            else
                return false;
        }
        #endregion

        #region PreencherCampos
        /// <summary>
        /// Método utilizado para preencher os dados no formulário
        /// </summary>
        private void PreencherCampos()
        {
            if (this.IdPessoaFisica.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(this.IdPessoaFisica.Value);

                lblNome.Text = txtNome.Valor = objPessoaFisica.NomePessoa;
                lblCPF.Text = objPessoaFisica.NumeroCPF;
                //txtCPF.Valor = 
                lblDataNascimento.Text = txtDataDeNascimento.Valor = objPessoaFisica.DataNascimento.ToString(Configuracao.FormatoData);

                if (objPessoaFisica.Sexo != null)
                {
                    objPessoaFisica.Sexo.CompleteObject();
                    lblSexo.Text = objPessoaFisica.Sexo.DescricaoSexo;
                    rblSexo.SelectedValue = objPessoaFisica.Sexo.IdSexo.ToString();
                }

                if (objPessoaFisica.EstadoCivil != null)
                {
                    objPessoaFisica.EstadoCivil.CompleteObject();
                    lblEstadoCivil.Text = objPessoaFisica.EstadoCivil.DescricaoEstadoCivil;
                    ddlEstadoCivil.SelectedValue = objPessoaFisica.EstadoCivil.IdEstadoCivil.ToString();
                }
                else
                {
                    lblEstadoCivil.Text = Configuracao.TextoCampoObrigatorio;
                    lblEstadoCivil.CssClass = Configuracao.CssLabelObrigatorio;
                }

                //txtNumeroRG.Valor = objPessoaFisica.NumeroRG;
                //txtOrgaoEmissorRG.Valor = objPessoaFisica.NomeOrgaoEmissor;

                txtTelefoneCelular.DDD = objPessoaFisica.NumeroDDDCelular;
                txtTelefoneCelular.Fone = objPessoaFisica.NumeroCelular;

                if (!String.IsNullOrEmpty(objPessoaFisica.NumeroDDDCelular) && !String.IsNullOrEmpty(objPessoaFisica.NumeroCelular))
                    lblTelefoneCelular.Text = UIHelper.FormatarTelefone(objPessoaFisica.NumeroDDDCelular, objPessoaFisica.NumeroCelular);
                else
                {
                    lblTelefoneCelular.Text = Configuracao.TextoCampoNaoInformado;
                    lblTelefoneCelular.CssClass = Configuracao.CssLabelNaoInformado;
                }

                txtTelefoneResidencial.DDD = objPessoaFisica.NumeroDDDTelefone;
                txtTelefoneResidencial.Fone = objPessoaFisica.NumeroTelefone;
                if (!String.IsNullOrEmpty(objPessoaFisica.NumeroTelefone) && !String.IsNullOrEmpty(objPessoaFisica.NumeroDDDTelefone))
                    lblTelefoneResidencial.Text = UIHelper.FormatarTelefone(objPessoaFisica.NumeroDDDTelefone, objPessoaFisica.NumeroTelefone);
                else
                {
                    lblTelefoneResidencial.Text = Configuracao.TextoCampoNaoInformado;
                    lblTelefoneResidencial.CssClass = Configuracao.CssLabelNaoInformado;
                }

                PessoaFisicaComplemento objPessoaFisicaComplemento;
                if (PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
                {
                    //Carrega Habilitacao        
                    if (objPessoaFisicaComplemento.CategoriaHabilitacao != null)
                    {
                        objPessoaFisicaComplemento.CategoriaHabilitacao.CompleteObject();
                        lblHabilitacao.Text = objPessoaFisicaComplemento.CategoriaHabilitacao.DescricaoCategoriaHabilitacao;
                        ddlHabilitacao.SelectedValue = objPessoaFisicaComplemento.CategoriaHabilitacao.IdCategoriaHabilitacao.ToString();
                    }
                    else
                    {
                        lblHabilitacao.Text = Configuracao.TextoCampoNaoInformado;
                        lblHabilitacao.CssClass = Configuracao.CssLabelNaoInformado;
                    }

                    if (objPessoaFisicaComplemento.FlagFilhos != null)
                    {
                        lblFilhos.Text = objPessoaFisicaComplemento.FlagFilhos.Value ? "Sim" : "Não";
                        ddlFilhos.SelectedValue = objPessoaFisicaComplemento.FlagFilhos.Value ? "1" : "0";
                    }
                    else
                    {
                        lblFilhos.Text = Configuracao.TextoCampoNaoInformado;
                        lblFilhos.CssClass = Configuracao.CssLabelNaoInformado;
                    }

                    Contato objContatoTelefone;
                    if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisicaComplemento.PessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, null))
                    {
                        txtTelefoneRecado.DDD = string.IsNullOrEmpty(objContatoTelefone.NumeroTelefone) ? String.Empty : objContatoTelefone.NumeroDDDTelefone;
                        txtTelefoneRecado.Fone = objContatoTelefone.NumeroTelefone;
                        lblTelefoneRecado.Text = UIHelper.FormatarTelefone(objContatoTelefone.NumeroDDDTelefone, objContatoTelefone.NumeroTelefone);
                        lblTelefoneRecado.Text = String.IsNullOrEmpty(txtTelefoneRecado.Fone) ? Configuracao.TextoCampoNaoInformado : lblTelefoneRecado.Text;

                        if (string.IsNullOrEmpty(objContatoTelefone.NomeContato))
                            lblTelefoneRecadoFalarCom.Text = Configuracao.TextoCampoNaoInformado;
                        else
                            lblTelefoneRecadoFalarCom.Text = txtTelefoneRecadoFalarCom.Text = objContatoTelefone.NomeContato;

                        if (lblTelefoneRecado.Text.Contains(Configuracao.TextoCampoNaoInformado))
                            lblTelefoneRecado.CssClass = Configuracao.CssLabelNaoInformado;

                        if (lblTelefoneRecadoFalarCom.Text.Contains(Configuracao.TextoCampoNaoInformado))
                            lblTelefoneRecadoFalarCom.CssClass = Configuracao.CssLabelNaoInformado;
                    }
                    else
                    {
                        lblTelefoneRecado.Text = lblTelefoneRecadoFalarCom.Text = Configuracao.TextoCampoNaoInformado;
                        lblTelefoneRecado.CssClass = Configuracao.CssLabelNaoInformado;
                        lblTelefoneRecadoFalarCom.CssClass = Configuracao.CssLabelNaoInformado;
                    }

                    Contato objContatoCelular;
                    if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisicaComplemento.PessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoCelular, out objContatoCelular, null))
                    {
                        txtCelularRecado.DDD = string.IsNullOrEmpty(objContatoCelular.NumeroCelular) ? String.Empty : objContatoCelular.NumeroDDDCelular;
                        txtCelularRecado.Fone = objContatoCelular.NumeroCelular;
                        lblCelularRecado.Text = UIHelper.FormatarTelefone(objContatoCelular.NumeroDDDCelular, objContatoCelular.NumeroCelular);
                        lblCelularRecado.Text = string.IsNullOrEmpty(txtCelularRecado.Fone) ? Configuracao.TextoCampoNaoInformado : lblCelularRecado.Text;

                        if (string.IsNullOrEmpty(objContatoCelular.NomeContato))
                            lblCelularRecadoFalarCom.Text = Configuracao.TextoCampoNaoInformado;
                        else
                            lblCelularRecadoFalarCom.Text = txtCelularRecadoFalarCom.Text = objContatoCelular.NomeContato;

                        if (lblCelularRecado.Text.Contains(Configuracao.TextoCampoNaoInformado))
                            lblCelularRecado.CssClass = Configuracao.CssLabelNaoInformado;

                        if (lblCelularRecadoFalarCom.Text.Contains(Configuracao.TextoCampoNaoInformado))
                            lblCelularRecadoFalarCom.CssClass = Configuracao.CssLabelNaoInformado;
                    }
                    else
                    {
                        lblCelularRecado.Text = lblCelularRecadoFalarCom.Text = Configuracao.TextoCampoNaoInformado;
                        lblCelularRecado.CssClass = Configuracao.CssLabelNaoInformado;
                        lblCelularRecadoFalarCom.CssClass = Configuracao.CssLabelNaoInformado;
                    }

                    //Carrega Altura e Peso
                    if (objPessoaFisicaComplemento.NumeroAltura.HasValue)
                        lblCaracteristicasPessoaisAltura.Text = String.Format("Altura: {0}m ", objPessoaFisicaComplemento.NumeroAltura.ToString());
                    else
                    {
                        lblCaracteristicasPessoaisAltura.Text = String.Format("Altura: {0} ", Configuracao.TextoCampoNaoInformado);
                        lblCaracteristicasPessoaisAltura.CssClass = Configuracao.CssLabelNaoInformado;
                    }

                    txtAltura.Valor = objPessoaFisicaComplemento.NumeroAltura;

                    if (objPessoaFisicaComplemento.NumeroPeso.HasValue)
                        lblCaracteristicasPessoaisPeso.Text = String.Format("Peso: {0}Kg ", objPessoaFisicaComplemento.NumeroPeso.ToString());
                    else
                    {
                        lblCaracteristicasPessoaisPeso.Text = String.Format("Peso: {0} ", Configuracao.TextoCampoNaoInformado);
                        lblCaracteristicasPessoaisPeso.CssClass = Configuracao.CssLabelNaoInformado;
                    }

                    txtPeso.Valor = objPessoaFisicaComplemento.NumeroPeso;

                    if (string.IsNullOrEmpty(objPessoaFisicaComplemento.DescricaoConhecimento))
                    {
                        lblOutrosConhecimentos.CssClass = Configuracao.CssLabelNaoInformado;
                        lblOutrosConhecimentos.Text = Configuracao.TextoCampoNaoInformado;
                    }
                    else
                    {
                        lblOutrosConhecimentos.Text = objPessoaFisicaComplemento.DescricaoConhecimento.ReplaceBreaks();
                        txtOutrosConhecimentos.Valor = objPessoaFisicaComplemento.DescricaoConhecimento;
                    }

                    if (objPessoaFisicaComplemento.FlagViagem.HasValue)
                    {
                        lblDisponibilidadeViagem.Text = objPessoaFisicaComplemento.FlagViagem.Value ? "Sim" : "Não";
                        ckbDisponibilidadeViagem.Checked = objPessoaFisicaComplemento.FlagViagem.Value;
                    }
                    else
                    {
                        lblDisponibilidadeViagem.Text = Configuracao.TextoCampoNaoInformado;
                        lblDisponibilidadeViagem.CssClass = Configuracao.CssLabelNaoInformado;
                    }
                }

                //Carrega Raça
                if (objPessoaFisica.Raca != null)
                {
                    objPessoaFisica.Raca.CompleteObject();
                    lblCaracteristicasPessoaisRaca.Text = String.Format("Raça: {0}", objPessoaFisica.Raca.DescricaoRacaBNE);
                    ddlRaca.SelectedValue = objPessoaFisica.Raca.IdRaca.ToString();
                }
                else
                {
                    lblCaracteristicasPessoaisRaca.Text = String.Format("Raça: {0}", Configuracao.TextoCampoNaoInformado);
                    lblCaracteristicasPessoaisRaca.CssClass = Configuracao.CssLabelNaoInformado;
                }

                //Deficiência
                if (objPessoaFisica.Deficiencia != null)
                {
                    objPessoaFisica.Deficiencia.CompleteObject();
                    lblDeficiencia.Text = objPessoaFisica.Deficiencia.DescricaoDeficiencia;
                    ddlDeficiencia.SelectedValue = objPessoaFisica.Deficiencia.IdDeficiencia.ToString();
                }
                else
                {
                    lblDeficiencia.Text = Configuracao.TextoCampoNaoInformado;
                    lblDeficiencia.CssClass = Configuracao.CssLabelNaoInformado;
                }

                if (string.IsNullOrEmpty(objPessoaFisica.EmailPessoa))
                {
                    lblEmail.Text = Configuracao.TextoCampoNaoInformado;
                    lblEmail.CssClass = Configuracao.CssLabelNaoInformado;
                }
                else
                    lblEmail.Text = txtEmail.Text = objPessoaFisica.EmailPessoa;

                if (objPessoaFisica.Endereco != null)
                {
                    objPessoaFisica.Endereco.CompleteObject();
                    objPessoaFisica.Endereco.Cidade.CompleteObject();

                    PreencherCamposEndereco(objPessoaFisica.Endereco.NumeroCEP, objPessoaFisica.Endereco.DescricaoLogradouro, objPessoaFisica.Endereco.NumeroEndereco,
                        objPessoaFisica.Endereco.DescricaoComplemento, objPessoaFisica.Endereco.DescricaoBairro);

                    lblCidade.Text = txtCidade.Text = String.Format("{0}/{1}", objPessoaFisica.Endereco.Cidade.NomeCidade, objPessoaFisica.Endereco.Cidade.Estado.SiglaEstado);
                }

                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objCurriculo))
                {
                    //[Obsolete("Obtado por não utilização/disponibilização.")]
                    //List<CurriculoTipoVinculo> listFuncoesTipoVinculos =
                    //    CurriculoTipoVinculo.CarregarVinculoPretendidoPorCurriculo(objCurriculo.IdCurriculo);

                    //PreencherFormaDeContratoTipoVinculo(listFuncoesTipoVinculos);

                    List<FuncaoPretendida> listFuncoesPretendidas = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);

                    PreencherCamposFuncoesPretendidas(listFuncoesPretendidas);

                    if (objCurriculo.ValorPretensaoSalarial.HasValue)
                    {
                        lblPretensaoSalarial.Text = objCurriculo.ValorPretensaoSalarial.Value.ToString("N2");
                        txtPretensaoSalarial.Valor = objCurriculo.ValorPretensaoSalarial.Value;
                    }

                    //Observações
                    if (string.IsNullOrEmpty(objCurriculo.ObservacaoCurriculo))
                    {
                        lblObservacoes.CssClass = Configuracao.CssLabelNaoInformado;
                        lblObservacoes.Text = Configuracao.TextoCampoNaoInformado;
                    }
                    else
                    {
                        lblObservacoes.Text = objCurriculo.ObservacaoCurriculo.ReplaceBreaks();
                        txtObservacoes.Valor = objCurriculo.ObservacaoCurriculo;
                    }

                    var listaCidadePretendidas = CurriculoDisponibilidadeCidade.ListarCidade(objCurriculo.IdCurriculo);
                    if (listaCidadePretendidas.Count > 0)
                        lblDisponibilidadeMorarEm.Text = string.Join(", ", listaCidadePretendidas.ToArray());
                    else
                    {
                        lblDisponibilidadeMorarEm.Text = Configuracao.TextoCampoNaoInformado;
                        lblDisponibilidadeMorarEm.CssClass = Configuracao.CssLabelNaoInformado;
                    }

                    List<CurriculoDisponibilidade> lstCurriculoDisponibilidade = CurriculoDisponibilidade.Listar(objCurriculo.IdCurriculo);

                    if (lstCurriculoDisponibilidade.Count > 0)
                    {
                        foreach (CurriculoDisponibilidade objCurriculoDisponibilidade in lstCurriculoDisponibilidade)
                        {
                            if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals((int)BNE.BLL.Enumeradores.Disponibilidade.Manha))
                            {
                                ckblDisponibilidade.Items.FindByText("Manhã").Selected = true;
                                lblDisponibilidade.Text += "Manhã ";
                            }
                            else if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals((int)BNE.BLL.Enumeradores.Disponibilidade.Tarde))
                            {
                                ckblDisponibilidade.Items.FindByText("Tarde").Selected = true;
                                lblDisponibilidade.Text += lblDisponibilidade.Text.Length > 0 ? ", Tarde" : "Tarde ";
                            }
                            else if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals((int)BNE.BLL.Enumeradores.Disponibilidade.Noite))
                            {
                                ckblDisponibilidade.Items.FindByText("Noite").Selected = true;
                                lblDisponibilidade.Text += lblDisponibilidade.Text.Length > 0 ? ", Noite" : "Noite ";
                            }
                            else if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals((int)BNE.BLL.Enumeradores.Disponibilidade.Sabado))
                            {
                                ckblDisponibilidade.Items.FindByText("Sábado").Selected = true;
                                lblDisponibilidade.Text += lblDisponibilidade.Text.Length > 0 ? ", Sábado" : "Sábado ";
                            }
                            else if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals((int)BNE.BLL.Enumeradores.Disponibilidade.Domingo))
                            {
                                ckblDisponibilidade.Items.FindByText("Domingo").Selected = true;
                                lblDisponibilidade.Text += lblDisponibilidade.Text.Length > 0 ? ", Domingo" : "Domingo ";
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(lblDisponibilidade.Text))
                    {
                        lblDisponibilidade.Text = Configuracao.TextoCampoObrigatorio;
                        lblDisponibilidade.CssClass = Configuracao.CssLabelObrigatorio;
                    }

                    CarregarGridFormacao(objCurriculo.PessoaFisica);
                    CarregarGridEspecializacao(objCurriculo.PessoaFisica);
                    CarregarGridComplementar(objCurriculo.PessoaFisica);
                    CarregarGridIdioma(objCurriculo.PessoaFisica);
                    PreencherExperienciaProfissional(objCurriculo.PessoaFisica);

                    AjustasCriticasDoCurriculo(objCurriculo);
                }

                CarregarFoto(objPessoaFisica);

                CarregarGridVeiculos();
                divTipoVeiculoTextBox.Style["display"] = "none";
                divTipoVeiculoLabel.Style["display"] = "block";

                CarregarGridCidade();
                divCidadeTextBox.Style["display"] = "none";
                divCidadeLabel.Style["display"] = "block";
            }
        }

        //[Obsolete("Obtado por não utilização/disponibilização.")]
        //private void PreencherFormaDeContratoTipoVinculo(List<CurriculoTipoVinculo> listFuncoesTipoVinculos)
        //{
        //    if (listFuncoesTipoVinculos.Count > 0)
        //    {
        //        lblTipoContrato.Text = listFuncoesTipoVinculos.Select(a => TipoVinculo.LoadObject(a.IdTipoVinculo))
        //            .OrderBy(obj => obj.CodigoGrauTipoVinculo)
        //            .Select(a => a.DescricaoTipoVinculo)
        //            .Aggregate((a, b) => a + ", " + b);

        //        foreach (var item in listFuncoesTipoVinculos)
        //        {
        //            var res = (chblTipoContrato.Items ?? new ListItemCollection()).FindByValue(item.IdTipoVinculo.ToString());

        //            if (res != null)
        //            {
        //                res.Selected = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        lblTipoContrato.Text = Configuracao.TextoCampoObrigatorio;
        //        lblTipoContrato.CssClass = Configuracao.CssLabelObrigatorio;
        //    }
        //}
        #endregion

        #region CarregarFoto
        /// <summary>
        /// Rotina para carregar a foto do candidato
        /// </summary>
        private void CarregarFoto(PessoaFisica objPessoaFisica)
        {
            try
            {
                byte[] byteArray = PessoaFisicaFoto.RecuperarArquivo(objPessoaFisica.CPF);

                if (byteArray != null)
                {
                    ucFoto.LimparFoto();
                    ucFoto.ImageData = byteArray;
                }
                else
                {
                    using (var client = new IntegracaoSouEuMesmo.IntegracaoClient())
                    {
                        byteArray = client.RetornoFotoPessoaFisica(objPessoaFisica.CPF);
                    }

                    if (byteArray != null)
                    {
                        FotoWSBytes = byteArray;
                        btlExisteFotoWS.Visible = true;
                    }
                    else
                    {
                        FotoWSBytes = null;
                        btlExisteFotoWS.Visible = false;
                    }
                }

                upFoto.Update();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region PreencherCamposEndereco
        private void PreencherCamposEndereco(string numeroCEP, string logradouro, string numeroEndereco, string descricaoComplemento, string descricaoBairro)
        {
            if (string.IsNullOrEmpty(numeroCEP))
            {
                lblCep.Text = Configuracao.TextoCampoEnderecoObrigatorio;
                lblCep.CssClass = Configuracao.CssLabelObrigatorio;
            }
            else
            {
                lblCep.Text = txtCEP.Valor = numeroCEP;
                lblCep.CssClass = string.Empty;
            }

            if (string.IsNullOrEmpty(logradouro))
            {
                lblLogradouro.Text = string.IsNullOrEmpty(txtCEP.Valor) ? Configuracao.TextoCampoEnderecoObrigatorio : Configuracao.TextoCampoObrigatorio;
                lblLogradouro.CssClass = Configuracao.CssLabelObrigatorio;
            }
            else
            {
                lblLogradouro.Text = txtLogradouro.Text = logradouro;
                lblLogradouro.CssClass = string.Empty;
            }

            if (string.IsNullOrEmpty(numeroEndereco))
            {
                lblNumero.Text = Configuracao.TextoCampoObrigatorio;
                lblNumero.CssClass = Configuracao.CssLabelObrigatorio;
            }
            else
            {
                lblNumero.Text = txtNumero.Text = numeroEndereco;
                lblNumero.CssClass = string.Empty;
            }

            if (string.IsNullOrEmpty(descricaoComplemento))
            {
                lblComplemento.Text = Configuracao.TextoCampoNaoInformado;
                lblComplemento.CssClass = Configuracao.CssLabelNaoInformado;
            }
            else
            {
                lblComplemento.Text = txtComplemento.Text = descricaoComplemento;
                lblComplemento.CssClass = string.Empty;
            }

            if (string.IsNullOrEmpty(descricaoBairro))
            {
                lblBairro.Text = string.IsNullOrEmpty(txtCEP.Valor) ? Configuracao.TextoCampoEnderecoObrigatorio : Configuracao.TextoCampoObrigatorio;
                lblBairro.CssClass = Configuracao.CssLabelObrigatorio;
            }
            else
            {
                lblBairro.Text = txtBairro.Text = descricaoBairro;
                lblBairro.CssClass = string.Empty;
            }

            upEndereco.Update();
        }
        #endregion

        #region PreencherCamposFuncoesPretendidas
        private void PreencherCamposFuncoesPretendidas(List<FuncaoPretendida> listFuncoesPretendidas)
        {
            //short? expMeses, expAnos;

            if (listFuncoesPretendidas.Count >= 1)
            {
                FuncaoPretendida objFuncaoPretendida = listFuncoesPretendidas[0];

                if (objFuncaoPretendida.Funcao != null)
                {
                    objFuncaoPretendida.Funcao.CompleteObject();
                    lblFuncaoPretendida1.Text = txtFuncaoPretendida1.Text = objFuncaoPretendida.Funcao.DescricaoFuncao;
                }
                else
                    lblFuncaoPretendida1.Text = txtFuncaoPretendida1.Text = objFuncaoPretendida.DescricaoFuncaoPretendida;

                IdFuncaoPretendida1 = objFuncaoPretendida.IdFuncaoPretendida;
            }
            else
            {
                lblFuncaoPretendida1.Text = Configuracao.TextoCampoObrigatorio;
                lblFuncaoPretendida1.CssClass = Configuracao.CssLabelObrigatorio;
            }

            if (listFuncoesPretendidas.Count >= 2)
            {
                FuncaoPretendida objFuncaoPretendida = listFuncoesPretendidas[1];

                if (objFuncaoPretendida.Funcao != null)
                {
                    objFuncaoPretendida.Funcao.CompleteObject();
                    lblFuncaoPretendida2.Text = txtFuncaoPretendida2.Text = objFuncaoPretendida.Funcao.DescricaoFuncao;
                }
                else
                    lblFuncaoPretendida2.Text = txtFuncaoPretendida2.Text = objFuncaoPretendida.DescricaoFuncaoPretendida;

                IdFuncaoPretendida2 = objFuncaoPretendida.IdFuncaoPretendida;
            }
            else
            {
                lblFuncaoPretendida2.Text = Configuracao.TextoCampoNaoInformado;
                lblFuncaoPretendida2.CssClass = Configuracao.CssLabelNaoInformado;
            }

            if (listFuncoesPretendidas.Count.Equals(3))
            {
                FuncaoPretendida objFuncaoPretendida = listFuncoesPretendidas[2];

                if (objFuncaoPretendida.Funcao != null)
                {
                    objFuncaoPretendida.Funcao.CompleteObject();
                    lblFuncaoPretendida3.Text = txtFuncaoPretendida3.Text = objFuncaoPretendida.Funcao.DescricaoFuncao;
                }
                else
                    lblFuncaoPretendida3.Text = txtFuncaoPretendida3.Text = objFuncaoPretendida.DescricaoFuncaoPretendida;

                IdFuncaoPretendida3 = objFuncaoPretendida.IdFuncaoPretendida;
            }
            else
            {
                lblFuncaoPretendida3.Text = Configuracao.TextoCampoNaoInformado;
                lblFuncaoPretendida3.CssClass = Configuracao.CssLabelNaoInformado;
            }
        }
        #endregion

        #region PreencherExperienciaProfissional
        public void PreencherExperienciaProfissional(PessoaFisica objPessoaFisica)
        {
            DataTable dt = ExperienciaProfissional.ListarExperienciaPorPessoaFisicaDT(objPessoaFisica.IdPessoaFisica);

            while (dt.Rows.Count < 5)
            {
                DataRow dr = dt.NewRow();

                switch (dt.Rows.Count)
                {
                    case 0:
                        dr["Raz_Social"] = "Última Empresa: " + Configuracao.TextoCampoNaoInformado;
                        break;
                    case 1:
                        dr["Raz_Social"] = "Penúltima Empresa: " + Configuracao.TextoCampoNaoInformado;
                        break;
                    case 2:
                        dr["Raz_Social"] = "Antepenúltima Empresa: " + Configuracao.TextoCampoNaoInformado;
                        break;
                    case 3:
                        dr["Raz_Social"] = "Experiência Profissional: " + Configuracao.TextoCampoNaoInformado;
                        break;
                    case 4:
                        dr["Raz_Social"] = "Empresa: " + Configuracao.TextoCampoNaoInformado;
                        break;
                }

                dt.Rows.Add(dr);

            }

            if (dt.Rows.Count > 0)
                UIHelper.CarregarRepeater(rptExperienciaProfissional, dt);
        }
        #endregion

        #region PreencherCamposExperienciaProfissional
        private void PreencherCamposExperienciaProfissional(int idExperienciaProfissional)
        {
            IdExperienciaProfissional = idExperienciaProfissional;

            ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(idExperienciaProfissional);

            txtEmpresa.Text = objExperienciaProfissional.RazaoSocial;
            ddlAtividadeEmpresa.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString();
            txtDataAdmissao.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

            if (objExperienciaProfissional.DataDemissao.HasValue)
                txtDataDemissao.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);

            if (objExperienciaProfissional.Funcao != null)
            {
                objExperienciaProfissional.Funcao.CompleteObject();
                txtFuncaoExercida.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                txtSugestaoTarefas.Text = objExperienciaProfissional.Funcao.DescricaoJob;
            }
            else
                txtFuncaoExercida.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

            txtAtividadeExercida.Valor = objExperienciaProfissional.DescricaoAtividade;

            MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade);

            txtUltimoSalario.Valor = objExperienciaProfissional.VlrSalario;

            txtDataDemissao.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucConferirDados_txtDataAdmissao_txtValor','cphConteudo_ucConferirDados_txtDataDemissao_txtValor','divDtaDemissaoAviso');";

            divDtaDemissaoAviso.Visible = false;

            //Curriculo objCurriculo;
            //if (Curriculo.CarregarPorPessoaFisica(objExperienciaProfissional.PessoaFisica.IdPessoaFisica, out objCurriculo))
            //    txtUltimoSalario.Valor = objCurriculo.ValorUltimoSalario;
        }
        #endregion

        #region MostrarGraficoQualidadeExperienciaProfissional

        public void MostrarGraficoQualidadeExperienciaProfissional(int valor, Literal ltdestino)
        {
            if (valor > 1 && valor < 70)
            {
                ltdestino.Text = "<div class='icon icon-fraco icon-size'></br><span class='labelIcon'>FRACO</span></div>";
            }
            else if (valor > 70 && valor <= 140)
            {
                ltdestino.Text = "<div class='icon icon-regular icon-size'></br><span class='labelIcon'>REGULAR</span></div>";
            }
            else
            {
                ltdestino.Text = "<div class='icon icon-otimo icon-size'></br><span class='labelIcon'>ÓTIMO</span></div>";
            }
        }

        #endregion

        #region PreencherCamposFormacao
        private void PreencherCamposFormacao(int idFormacao)
        {
            IdFormacao = idFormacao;

            Formacao objFormacao = Formacao.LoadObject(idFormacao);

            if (objFormacao.Fonte != null)
            {
                objFormacao.Fonte.CompleteObject();
                txtInstituicao.Text = objFormacao.Fonte.SiglaFonte + " - " + objFormacao.Fonte.NomeFonte;
            }
            else
                txtInstituicao.Text = objFormacao.DescricaoFonte;

            if (objFormacao.Curso != null)
            {
                objFormacao.Curso.CompleteObject();
                txtTituloCurso.Text = objFormacao.Curso.DescricaoCurso;
            }
            else
                txtTituloCurso.Text = objFormacao.DescricaoCurso;

            if (objFormacao.Cidade != null)
            {
                objFormacao.Cidade.CompleteObject();
                txtCidadeFormacao.Text = String.Format("{0}/{1}", objFormacao.Cidade.NomeCidade, objFormacao.Cidade.Estado.SiglaEstado);
            }

            if (objFormacao.AnoConclusao.HasValue)
                txtAnoConclusao.Valor = objFormacao.AnoConclusao.Value.ToString();
            else
                txtAnoConclusao.Valor = string.Empty;

            // Escolaridade objEscolaridade;
            objFormacao.Escolaridade.CompleteObject();

            if (objFormacao.SituacaoFormacao != null)
                ddlSituacao.SelectedValue = objFormacao.SituacaoFormacao.IdSituacaoFormacao.ToString();

            if (objFormacao.NumeroPeriodo != null)
                txtPeriodo.Valor = objFormacao.NumeroPeriodo.Value.ToString();

            ListItem li = ddlNivel.Items.FindByValue(objFormacao.Escolaridade.IdEscolaridade.ToString());

            if (li == null)
                ddlNivel.Items.Add(new ListItem(objFormacao.Escolaridade.DescricaoBNE, objFormacao.Escolaridade.IdEscolaridade.ToString()));

            ddlNivel.SelectedValue = objFormacao.Escolaridade.IdEscolaridade.ToString();
            ddlNivel.Enabled = true;

            ScriptManager.RegisterStartupScript(upFormacao, upFormacao.GetType(), " AjustarParametrosPreencherCamposFormacao", "javaScript:AjustarParametros('" + ddlNivel.SelectedValue + "');", true);
            ScriptManager.RegisterStartupScript(upFormacao, upFormacao.GetType(), " AjustarCamposPreencherCamposFormacao", "javaScript:AjustarCampos();", true);

            upFormacao.Update();
        }
        #endregion

        #region PreencherCamposComplementar
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCamposComplementar()
        {
            if (IdComplementar.HasValue)
            {
                Formacao objFormacao = Formacao.LoadObject(IdComplementar.Value);

                if (objFormacao.Fonte != null)
                {
                    objFormacao.Fonte.CompleteObject();
                    txtInstituicaoComplementar.Text = objFormacao.Fonte.SiglaFonte + " - " + objFormacao.Fonte.NomeFonte;
                }
                else
                    txtInstituicaoComplementar.Text = objFormacao.DescricaoFonte;

                if (objFormacao.Curso != null)
                {
                    objFormacao.Curso.CompleteObject();
                    txtTituloCursoComplementar.Text = objFormacao.Curso.DescricaoCurso;
                }
                else
                    txtTituloCursoComplementar.Text = objFormacao.DescricaoCurso;

                if (objFormacao.Cidade != null)
                {
                    objFormacao.Cidade.CompleteObject();
                    txtCidadeComplementar.Text = String.Format("{0}/{1}", objFormacao.Cidade.NomeCidade, objFormacao.Cidade.Estado.SiglaEstado);
                }
                else
                    txtCidadeComplementar.Text = String.Empty;

                txtAnoConclusaoComplementar.Valor = objFormacao.AnoConclusao.HasValue ? objFormacao.AnoConclusao.ToString() : string.Empty;
                txtCargaHorariaComplementar.Valor = objFormacao.QuantidadeCargaHoraria.HasValue ? objFormacao.QuantidadeCargaHoraria.Value.ToString() : string.Empty;

                upComplementares.Update();
            }
        }
        #endregion

        #region PreencherCamposEspecializacao
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCamposEspecializacao()
        {
            if (IdEspecializacao.HasValue)
            {
                Formacao objFormacao = Formacao.LoadObject(IdEspecializacao.Value);

                if (objFormacao.Fonte != null)
                {
                    objFormacao.Fonte.CompleteObject();
                    txtInstituicaoEspecializacao.Text = objFormacao.Fonte.SiglaFonte + " - " + objFormacao.Fonte.NomeFonte;
                }
                else
                    txtInstituicaoEspecializacao.Text = objFormacao.DescricaoFonte;

                if (objFormacao.Curso != null)
                {
                    objFormacao.Curso.CompleteObject();
                    txtTituloCursoEspecializacao.Text = objFormacao.Curso.DescricaoCurso;
                }
                else
                    txtTituloCursoEspecializacao.Text = objFormacao.DescricaoCurso;

                if (objFormacao.Cidade != null)
                {
                    objFormacao.Cidade.CompleteObject();
                    txtCidadeEspecializacao.Text = String.Format("{0}/{1}", objFormacao.Cidade.NomeCidade, objFormacao.Cidade.Estado.SiglaEstado);
                }

                if (objFormacao.AnoConclusao.HasValue)
                    txtAnoConclusaoEspecializacao.Valor = objFormacao.AnoConclusao.Value.ToString();
                else
                    txtAnoConclusaoEspecializacao.Valor = string.Empty;

                // Escolaridade objEscolaridade;
                objFormacao.Escolaridade.CompleteObject();

                if (objFormacao.SituacaoFormacao != null)
                    ddlSituacaoEspecializacao.SelectedValue = objFormacao.SituacaoFormacao.IdSituacaoFormacao.ToString();

                if (objFormacao.NumeroPeriodo != null)
                    txtPeriodoEspecializacao.Valor = objFormacao.NumeroPeriodo.Value.ToString();

                upEspecializacao.Update();

                ddlNivelEspecializacao.Items.Add(new ListItem(objFormacao.Escolaridade.DescricaoBNE, objFormacao.Escolaridade.IdEscolaridade.ToString()));
                ddlNivelEspecializacao.SelectedValue = objFormacao.Escolaridade.IdEscolaridade.ToString();
                ScriptManager.RegisterStartupScript(upEspecializacao, upEspecializacao.GetType(), " AjustarParametrosPreencherCamposEspecializacao", "javaScript:AjustarParametros(" + ddlNivelEspecializacao.SelectedValue + ");", true);
                ScriptManager.RegisterStartupScript(upEspecializacao, upEspecializacao.GetType(), " AjustarCamposPreencherCamposEspecializacao", "javaScript:AjustarCamposEspecializacao();", true);
            }
        }
        #endregion

        #region LimparCamposEspecializacao
        /// <summary>
        /// Método utilizado para limpar todos os campos  ou colocar na opção default relacionados ao Formação 
        /// </summary>
        private void LimparCamposEspecializacao()
        {
            //DicFormacaoCursosGrid = new Dictionary<int, FormacaoGrid>();
            ddlNivelEspecializacao.SelectedIndex = 0;
            txtInstituicaoEspecializacao.Text = String.Empty;
            txtTituloCursoEspecializacao.Text = String.Empty;
            ddlSituacaoEspecializacao.SelectedIndex = 0;
            txtPeriodoEspecializacao.Valor = String.Empty;
            txtAnoConclusaoEspecializacao.Valor = String.Empty;
            txtCidadeEspecializacao.Text = String.Empty;
            //rblLocalEspecializacao.SelectedIndex = 0;

            var parametros = new
            {
                ObrigatorioSigla = 0,
                ObrigatorioInstituicao = 0,
                VisivelInstituicao = 0,
                ObrigatorioCurso = 0,
                VisivelCurso = 0,
                ObrigatorioLocal = 0,
                VisivelLocal = 0,
                ObrigatorioCidadeEstado = 0,
                VisivelCidadeEstado = 0,
                ObrigatorioSituacao = 0,
                VisivelSituacao = 0,
                ObrigatorioPeriodo = 0,
                VisivelPeriodo = 0,
                ObrigatorioAnoConclusao = 0,
                VisivelAnoConclusao = 0
            };

            ScriptManager.RegisterStartupScript(upEspecializacao, upEspecializacao.GetType(), "InicializarStatusLimparCamposEspecializacao", "javaScript:InicializarStatus( " + new JSONReflector(parametros).ToString() + " );", true);
            ScriptManager.RegisterStartupScript(upEspecializacao, upEspecializacao.GetType(), "AjustarCamposLimparCamposEspecializacao", "javaScript:AjustarCamposEspecializacao();", true);
        }
        #endregion

        #region SalvarFormacao
        private void SalvarFormacao()
        {
            if (!ddlNivel.SelectedValue.Equals("0"))
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                Formacao objFormacao = IdFormacao.HasValue ? Formacao.LoadObject(IdFormacao.Value) : new Formacao();

                objFormacao.PessoaFisica = objPessoaFisica;
                objFormacao.Escolaridade = new BNE.BLL.Escolaridade(Convert.ToInt32(ddlNivel.SelectedValue));

                string[] instituicaoSigla = txtInstituicao.Text.Split('-');

                string nomeInstituicao;
                if (instituicaoSigla.Length.Equals(1))
                    nomeInstituicao = instituicaoSigla[0].Trim();
                else
                    nomeInstituicao = instituicaoSigla[1].Trim();

                if (!String.IsNullOrEmpty(nomeInstituicao))
                {
                    Fonte objFonte;
                    if (Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
                    {
                        objFormacao.Fonte = objFonte;
                        objFormacao.DescricaoFonte = String.Empty;
                    }
                    else
                    {
                        objFormacao.Fonte = null;
                        objFormacao.DescricaoFonte = nomeInstituicao;
                    }
                }
                else
                    objFormacao.Fonte = null;

                if (!String.IsNullOrEmpty(txtTituloCurso.Text))
                {
                    Curso objCurso;
                    if (Curso.CarregarPorNome(txtTituloCurso.Text, out objCurso))
                    {
                        objFormacao.Curso = objCurso;
                        objFormacao.DescricaoCurso = String.Empty;
                    }
                    else
                    {
                        objFormacao.Curso = null;
                        objFormacao.DescricaoCurso = txtTituloCurso.Text;
                    }
                }
                else
                    objFormacao.Curso = null;

                if (!ddlSituacao.SelectedValue.Equals("0"))
                    objFormacao.SituacaoFormacao = new SituacaoFormacao(Convert.ToInt16(ddlSituacao.SelectedValue));
                else
                    objFormacao.SituacaoFormacao = null;

                short periodo;
                if (!string.IsNullOrEmpty(txtPeriodo.Valor))
                {
                    if (short.TryParse(txtPeriodo.Valor, out periodo))
                        objFormacao.NumeroPeriodo = periodo;
                    else
                        objFormacao.NumeroPeriodo = null;
                }

                short anoconclusao;
                if (!string.IsNullOrEmpty(txtAnoConclusao.Valor))
                {
                    if (short.TryParse(txtAnoConclusao.Valor, out anoconclusao))
                        objFormacao.AnoConclusao = anoconclusao;
                    else
                        objFormacao.AnoConclusao = null;
                }

                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidadeFormacao.Text, out objCidade))
                    objFormacao.Cidade = objCidade;
                else
                    objFormacao.Cidade = null;

                objFormacao.Save();
            }
        }
        #endregion

        #region SalvarEspecializacao
        private void SalvarEspecializacao()
        {
            if (!ddlNivelEspecializacao.SelectedValue.Equals("0"))
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                Formacao objFormacao = IdEspecializacao.HasValue ? Formacao.LoadObject(IdEspecializacao.Value) : new Formacao();

                objFormacao.PessoaFisica = objPessoaFisica;
                objFormacao.Escolaridade = new BNE.BLL.Escolaridade(Convert.ToInt32(ddlNivelEspecializacao.SelectedValue));

                string[] instituicaoSigla = txtInstituicaoEspecializacao.Text.Split('-');
                string nomeInstituicao;
                if (instituicaoSigla.Length.Equals(1))
                    nomeInstituicao = instituicaoSigla[0].Trim();
                else
                    nomeInstituicao = instituicaoSigla[1].Trim();

                if (!String.IsNullOrEmpty(nomeInstituicao))
                {
                    Fonte objFonte;
                    if (Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
                    {
                        objFormacao.Fonte = objFonte;
                        objFormacao.DescricaoFonte = String.Empty;
                    }
                    else
                    {
                        objFormacao.Fonte = null;
                        objFormacao.DescricaoFonte = nomeInstituicao;
                    }
                }
                else
                    objFormacao.Fonte = null;

                if (!String.IsNullOrEmpty(txtTituloCursoEspecializacao.Text))
                {
                    Curso objCurso;
                    if (Curso.CarregarPorNome(txtTituloCursoEspecializacao.Text, out objCurso))
                    {
                        objFormacao.Curso = objCurso;
                        objFormacao.DescricaoCurso = String.Empty;
                    }
                    else
                    {
                        objFormacao.Curso = null;
                        objFormacao.DescricaoCurso = txtTituloCursoEspecializacao.Text;
                    }
                }
                else
                    objFormacao.Curso = null;

                if (!ddlSituacaoEspecializacao.SelectedValue.Equals("0"))
                    objFormacao.SituacaoFormacao = new SituacaoFormacao(Convert.ToInt16(ddlSituacaoEspecializacao.SelectedValue));
                else
                    objFormacao.SituacaoFormacao = null;

                short periodo;
                if (short.TryParse(txtPeriodoEspecializacao.Valor, out periodo))
                    objFormacao.NumeroPeriodo = periodo;
                else
                    objFormacao.NumeroPeriodo = null;

                short anoconclusao;
                if (short.TryParse(txtAnoConclusaoEspecializacao.Valor, out anoconclusao))
                    objFormacao.AnoConclusao = anoconclusao;
                else
                    objFormacao.AnoConclusao = null;

                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidadeEspecializacao.Text, out objCidade))
                    objFormacao.Cidade = objCidade;
                else
                    objFormacao.Cidade = null;

                objFormacao.Save();
            }
        }
        #endregion

        #region SalvarCursoComplementar
        private void SalvarCursoComplementar()
        {
            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
            Formacao objFormacao = IdComplementar.HasValue ? Formacao.LoadObject(IdComplementar.Value) : new Formacao();

            objFormacao.PessoaFisica = objPessoaFisica;
            objFormacao.Escolaridade = new Escolaridade((int)Enumeradores.Escolaridade.OutrosCursos);

            string[] instituicaoSigla = txtInstituicaoComplementar.Text.Split('-');
            string nomeInstituicao;
            if (instituicaoSigla.Length.Equals(1))
                nomeInstituicao = instituicaoSigla[0].Trim();
            else
                nomeInstituicao = instituicaoSigla[1].Trim();

            Fonte objFonte;
            if (Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
            {
                objFormacao.Fonte = objFonte;
                objFormacao.DescricaoFonte = String.Empty;
            }
            else
            {
                objFormacao.Fonte = null;
                objFormacao.DescricaoFonte = nomeInstituicao;
            }

            Curso objCurso;
            if (Curso.CarregarPorNome(txtTituloCursoComplementar.Text, out objCurso))
            {
                objFormacao.Curso = objCurso;
                objFormacao.DescricaoCurso = String.Empty;
            }
            else
            {
                objFormacao.Curso = null;
                objFormacao.DescricaoCurso = txtTituloCursoComplementar.Text;
            }

            //objFormacao.FlgNacional = rblLocal.SelectedValue.Equals(Local.Brasil.GetHasCode().ToString());
            Cidade objCidade;
            if (Cidade.CarregarPorNome(txtCidadeComplementar.Text, out objCidade))
                objFormacao.Cidade = objCidade;
            else
                objFormacao.Cidade = null;

            short cargahoraria;
            if (short.TryParse(txtCargaHorariaComplementar.Valor, out cargahoraria))
                objFormacao.QuantidadeCargaHoraria = cargahoraria;
            else
                objFormacao.QuantidadeCargaHoraria = null;

            short anoconclusao;
            if (short.TryParse(txtAnoConclusaoComplementar.Valor, out anoconclusao))
                objFormacao.AnoConclusao = anoconclusao;
            else
                objFormacao.AnoConclusao = null;

            objFormacao.FlagInativo = false;

            objFormacao.Save();

            IdComplementar = null;
        }
        #endregion

        #region CarregarGridCidade
        /// <summary>
        /// Carrega grid Cidades Disponiveis Por Curriculo
        /// </summary>
        protected void CarregarGridCidade()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                //TODO: Performance, armazenar o idcurriculo na viewstate para evitar Load desnecessario
                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(base.IdPessoaFisicaLogada.Value, out objCurriculo))
                    UIHelper.CarregarGridView(gvCidade, CurriculoDisponibilidadeCidade.ListarCidadesPorCurriculo(objCurriculo.IdCurriculo));

                var listaCidadePretendidas = CurriculoDisponibilidadeCidade.ListarCidade(objCurriculo.IdCurriculo);
                if (listaCidadePretendidas.Count > 0)
                    lblDisponibilidadeMorarEm.Text = string.Join(", ", listaCidadePretendidas.ToArray());
            }

            upCidadeDisponivel.Update();
        }
        #endregion

        #region CarregarGridVeiculos
        /// <summary>
        /// Carrega grid Veiculos
        /// </summary>
        private void CarregarGridVeiculos()
        {
            DataTable dt = PessoaFisicaVeiculo.ListarVeiculosDT(IdPessoaFisica.Value);

            UIHelper.CarregarRadGrid(gvModeloAno, dt);
            UIHelper.CarregarRepeater(rptModeloAno, dt);

            if (gvModeloAno.Items.Count.Equals(0))
            {
                lblModeloAno.Visible = true;
                lblModeloAno.Text = Configuracao.TextoCampoNaoInformado;
                lblModeloAno.CssClass = Configuracao.CssLabelNaoInformado;
            }
            else
            {
                lblModeloAno.Visible = false;
            }

            upTipoVeiculo.Update();
        }
        #endregion

        #region CarregarGridIdioma
        public void CarregarGridIdioma(PessoaFisica objPessoaFisica)
        {
            DataTable dtIdioma = PessoafisicaIdioma.ListarPorPessoaFisicaDT(objPessoaFisica.IdPessoaFisica);

            UIHelper.CarregarRadGrid(gvIdioma, dtIdioma);
            UIHelper.CarregarRepeater(rptIdiomas, dtIdioma);

            if (dtIdioma.Rows.Count.Equals(0))
            {
                lblIdiomas.Visible = true;
                lblIdiomas.Text = Configuracao.TextoCampoNaoInformado;
                lblIdiomas.CssClass = Configuracao.CssLabelNaoInformado;
            }
            else
            {
                lblIdiomas.Visible = false;
            }

            upIdiomas.Update();
        }
        #endregion

        #region CarregarGridFormacao
        public void CarregarGridFormacao(PessoaFisica objPessoaFisica)
        {
            DataTable dtFormacao = Formacao.ListarFormacao(IdPessoaFisica.Value, true, false, false);

            UIHelper.CarregarRadGrid(gvFormacao, dtFormacao);
            UIHelper.CarregarRepeater(rptCursos, dtFormacao);

            if (dtFormacao.Rows.Count.Equals(0))
            {
                lblCursos.Visible = true;
                lblCursos.Text = Configuracao.TextoCampoNaoInformado;
                lblCursos.CssClass = Configuracao.CssLabelNaoInformado;
            }
            else
            {
                lblCursos.Visible = false;
            }

            pnlEspecializacao.Visible = pnlEspecializacaoNivel.Visible = ExisteGraduacao() || !gvEspecializacao.Items.Count.Equals(0);
            upEspecializacao.Update(); upEspecializacaoValor.Update();

            upFormacao.Update();
        }
        #endregion

        #region CarregarGridEspecializacao
        public void CarregarGridEspecializacao(PessoaFisica objPessoaFisica)
        {
            DataTable dtEspecializacao = Formacao.ListarFormacao(IdPessoaFisica.Value, false, true, false);

            UIHelper.CarregarRadGrid(gvEspecializacao, dtEspecializacao);
            UIHelper.CarregarRepeater(rptEspecializacao, dtEspecializacao);

            if (dtEspecializacao.Rows.Count.Equals(0))
            {
                lblEspecializacao.Visible = true;
                lblEspecializacao.Text = Configuracao.TextoCampoNaoInformado;
                lblEspecializacao.CssClass = Configuracao.CssLabelNaoInformado;
            }
            else
            {
                lblEspecializacao.Visible = false;
            }

            pnlEspecializacao.Visible = pnlEspecializacaoNivel.Visible = ExisteGraduacao() || !gvEspecializacao.Items.Count.Equals(0);
            upEspecializacaoValor.Update();

            upEspecializacao.Update();
        }
        #endregion

        #region CarregarGridComplementar
        /// <summary>
        /// Carrega a Grid de cursos
        /// </summary>
        public void CarregarGridComplementar(PessoaFisica objPessoaFisica)
        {
            DataTable dtComplementares = Formacao.ListarFormacao(objPessoaFisica.IdPessoaFisica, false, false, true);

            UIHelper.CarregarRadGrid(gvComplementar, dtComplementares);
            UIHelper.CarregarRepeater(rptComplementar, dtComplementares);

            if (dtComplementares.Rows.Count.Equals(0))
            {
                lblComplementares.Visible = true;
                lblComplementares.Text = Configuracao.TextoCampoNaoInformado;
                lblComplementares.CssClass = Configuracao.CssLabelNaoInformado;
            }
            else
            {
                lblComplementares.Visible = false;
            }

            upComplementares.Update();
        }
        #endregion

        #region LimparCamposVeiculo
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        private void LimparCamposVeiculo()
        {
            txtAnoVeiculo.Valor = String.Empty;
            txtModelo.Valor = String.Empty;
            ddlTipoVeiculo.SelectedIndex = 0;
            //upAbaDadosComplementares.Update();
        }
        #endregion

        #region LimparCamposIdiomas
        /// <summary>
        /// Método utilizado para limpar ou colocar na opção default todos os campos relacionados ao Idioma
        /// </summary>
        private void LimparCamposIdiomas()
        {
            ddlIdioma.SelectedIndex = 0;
            rblNivelIdioma.SelectedIndex = -1;
        }
        #endregion

        #region LimparCamposCursos
        /// <summary>
        /// Método utilizado para limpar todos os campos  ou colocar na opção default relacionados ao Formação 
        /// </summary>
        private void LimparCamposCursos()
        {
            txtInstituicaoComplementar.Text = String.Empty;
            txtTituloCursoComplementar.Text = String.Empty;
            txtCidadeComplementar.Text = String.Empty;
            txtCargaHorariaComplementar.Valor = String.Empty;
            txtAnoConclusaoComplementar.Valor = String.Empty;
        }
        #endregion

        #region LimparCamposExperiencia
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExperiencia()
        {
            txtEmpresa.Text = String.Empty;
            ddlAtividadeEmpresa.SelectedIndex = 0;
            txtDataAdmissao.Valor = String.Empty;
            txtDataDemissao.Valor = String.Empty;
            txtFuncaoExercida.Text = String.Empty;
            txtAtividadeExercida.Valor = String.Empty;
            txtSugestaoTarefas.Text = String.Empty;
        }
        #endregion

        #region LimparCamposFormacao
        /// <summary>
        /// Método utilizado para limpar ou colocar na opção default todos os campos relacionados ao Idioma
        /// </summary>
        private void LimparCamposFormacao()
        {
            ddlNivel.SelectedIndex = 0;

            txtInstituicao.Text = String.Empty;
            txtTituloCurso.Text = String.Empty;
            ddlSituacao.SelectedIndex = 0;
            txtPeriodo.Valor = String.Empty;
            txtAnoConclusao.Valor = String.Empty;
            txtCidade.Text = String.Empty;

            AjustarJavascriptFormacao(this.btnSalvarFormacao);
        }
        #endregion

        #region PreencherCamposVeiculo
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        private void PreencherCamposVeiculo()
        {
            if (IdPessoaFisicaVeiculo.HasValue)
            {
                PessoaFisicaVeiculo objPessoaFisicaVeiculo = PessoaFisicaVeiculo.LoadObject(IdPessoaFisicaVeiculo.Value);
                txtAnoVeiculo.Valor = objPessoaFisicaVeiculo.AnoVeiculo.ToString();
                txtModelo.Valor = objPessoaFisicaVeiculo.DescricaoModelo;
                ddlTipoVeiculo.SelectedValue = objPessoaFisicaVeiculo.TipoVeiculo.IdTipoVeiculo.ToString();
                //upAbaDadosComplementares.Update();
            }
        }
        #endregion

        #region InicializarEnvioCurriculo
        private void InicializarEnvioCurriculo()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                //checar se o candidato informou o e-mail
                if (txtEmail.Text != "")
                {
                    Curriculo objCurriculo;
                    if (Curriculo.CarregarPorPessoaFisica(base.IdPessoaFisicaLogada.Value, out objCurriculo))
                    {

                        //Passa os curriculos selecionados na pesquisa de curriculo
                        ucEnvioCurriculo.TipoEnvioCurriculo = Code.Enumeradores.TipoEnvioCurriculo.VIP;
                        ucEnvioCurriculo.EsconderCamposEnviarComoOcultarDados();
                        ucEnvioCurriculo.Inicializar();
                        ucEnvioCurriculo.ListIdCurriculos.Add(objCurriculo.IdCurriculo);
                        ucEnvioCurriculo.CarregarAssunto();
                        ucEnvioCurriculo.MostrarModal();
                    }
                }
                else
                {
                    ExibirMensagem("Você deve cadastrar o seu e-mail para poder enviar o currículo.", TipoMensagem.Aviso);
                }

            }
        }
        #endregion

        #endregion

        #region AjaxMethods

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="siglaUF"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }
        #endregion

        #region ValidarVariosEnderecosPorCEP
        /// <summary>
        /// Validar se o cep informado contém mais de um endereço associado ao mesmo.
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="siglaUF"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarVariosEnderecosPorCEP(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                try
                {
                    int qtdeCepEncontrados = 0;

                    var objCep = new Cep
                    {
                        sCep = valor.Replace("-", string.Empty).Trim()
                    };

                    qtdeCepEncontrados = Cep.ContaConsulta(objCep);
                    {
                        if (qtdeCepEncontrados > 1)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Falha na busca de CEP");
                }
            }
            return false;
        }
        #endregion

        #region ValidarEstadoCivil
        /// <summary>
        /// Validar Estado Civil
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string ValidarEstadoCivil(string valor)
        {
            valor = valor.Trim();
            if (string.IsNullOrEmpty(valor)) //Selecione
                return String.Empty;

            if (valor.Equals("0"))
                return String.Empty;

            return valor;
        }
        #endregion

        /// <summary>
        /// Validar funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            int? idOrigem = null;
            if (base.IdOrigem.HasValue)
                idOrigem = base.IdOrigem.Value;

            return Funcao.ValidarFuncaoPorOrigem(idOrigem, valor);
        }

        #region PesquisarMediaSalarial
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="desFuncao"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string PesquisarMediaSalarial(string desFuncao, string cidadeEstado)
        {
            //TODO: Comentado até ser inserida a matriz nova de função
            /*
            desFuncao = desFuncao.Trim();
            cidadeEstado = cidadeEstado.Trim();
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(desFuncao, out objFuncao))
            {
                Cidade objCidade;
                if (Cidade.CarregarPorNome(cidadeEstado, out objCidade))
                {
                    var pesquisa = BLL.Custom.SalarioBr.PesquisaSalarial.EfetuarPesquisa(objFuncao, objCidade.Estado);

                    if (pesquisa != null && (pesquisa.ValorTrainee != Decimal.Zero && pesquisa.ValorMaster != Decimal.Zero))
                        return String.Format("{0};{1}", pesquisa.ValorTrainee.ToString("N2"), pesquisa.ValorMaster.ToString("N2"));
                }
            }
            */
            return String.Empty;
        }
        #endregion

        #region RecuperarJobFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string RecuperarJobFuncao(string valor)
        {
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(valor, out objFuncao))
            {
                if (!string.IsNullOrEmpty(objFuncao.DescricaoJob))
                    return objFuncao.DescricaoJob;
                return String.Empty;
            }
            return String.Empty;
        }
        #endregion

        #endregion

    }
}
