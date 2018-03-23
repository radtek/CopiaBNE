using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.AsyncServices;
using BNE.BLL.Custom.Email;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using JSONSharp;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using Parametro = BNE.BLL.Parametro;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using System.Data.SqlClient;

namespace BNE.Web
{

    public partial class AnunciarVaga : BasePage
    {

        #region Propriedades

        #region DTPerguntas

        private DataTable DTPerguntas
        {
            get
            {
                return (DataTable)ViewState[Chave.Temporaria.Variavel1.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        private List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #region IdVagaSession - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int? IdVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
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

        #region UrlOrigem - Variável 4
        /// <summary>
        /// </summary>
        private string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel4.ToString()]).ToString();
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

        #region LblRequisitoColorDefault - Variável 5
        /// <summary>
        /// </summary>
        private Color LblRequisitoColorDefault
        {
            get
            {
                return (Color)(ViewState[Chave.Temporaria.Variavel5.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
            }
        }
        #endregion

        #region LblAtribuicaoColorDefault - Variável 6
        /// <summary>
        /// </summary>
        private Color LblAtribuicaoColorDefault
        {
            get
            {
                return (Color)(ViewState[Chave.Temporaria.Variavel6.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
            }
        }
        #endregion

        #region IdPesquisaCurriculo - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int? IdPesquisaCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel7.ToString()].ToString());
                return null;
            }
        }
        #endregion

        #region BoolClonarVaga - Variável 8
        public bool BoolClonarVaga
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel8.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel8.ToString()] = value;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            AjustarIdade();
            Ajax.Utility.RegisterTypeForAjax(typeof(AnunciarVaga));
        }
        #endregion

        #region btiEuQuero_Click
        protected void btiEuQuero_Click(object sender, ImageClickEventArgs e)
        {
            //Verifica se a filial logada tem um plano ativo
            PlanoQuantidade objPlanoQuantidade;
            if (PlanoQuantidade.CarregarPlanoAtualVigente(base.IdFilial.Value, out objPlanoQuantidade))
            {
                //Exibe a modal de confirmação de divulgação em massa
                if (IdVaga.HasValue)
                {
                    Vaga objVaga = Vaga.LoadObject((int)IdVaga);
                    objVaga.AnunciarEmMassa();
                    ucModalConfirmacao.PreencherCampos("Divulgação em Massa", MensagemAviso._24026, false, "OK");
                    ucModalConfirmacao.MostrarModal();
                }
            }
            else
            {
                base.UrlDestinoPagamento.Value = GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null);
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
            }
        }
        #endregion

        #region rbSim_CheckedChanged
        protected void rbSim_CheckedChanged(object sender, EventArgs e)
        {
            rbSim.Checked = true;
            rbNao.Checked = false;
            upRespostas.Update();
        }
        #endregion

        #region rbNao_CheckedChanged
        protected void rbNao_CheckedChanged(object sender, EventArgs e)
        {
            rbSim.Checked = false;
            rbNao.Checked = true;
            upRespostas.Update();
        }
        #endregion

        #region gvPerguntasVaga_ItemCommand
        protected void gvPerguntasVaga_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Delete"))
            {
                string idVagaPergunta = gvPerguntasVaga.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga_Pergunta"].ToString();

                //Percorre o DataTable para excluir 
                for (int i = 0; i < DTPerguntas.Rows.Count; i++)
                {
                    string idDataTable = DTPerguntas.Rows[i]["Idf_Vaga_Pergunta"].ToString();
                    if (idDataTable.Equals(idVagaPergunta))
                        DTPerguntas.Rows[i].Delete();
                }

                CarregarGrid();

                if (DTPerguntas.Rows.Count < 4)
                    btnAdicionarPergunta.Enabled = true;

                if (DTPerguntas.Rows.Count.Equals(-1))
                    gvPerguntasVaga.Visible = false;
            }
        }
        #endregion

        #region btnAdicionarPergunta_Click
        protected void btnAdicionarPergunta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPergunta.Text))
                ExibirMensagem(MensagemAviso._23008, TipoMensagem.Aviso);
            else if (!rbNao.Checked && !rbSim.Checked)
                ExibirMensagem(MensagemAviso._23007, TipoMensagem.Aviso);
            else
            {
                DTPerguntas = VagaPergunta.DataTablePerguntas(DTPerguntas, null, txtPergunta.Text, rbSim.Checked, (int)Enumeradores.TipoPergunta.RespostaObjetiva);
                CarregarGrid();

                if (DTPerguntas.Rows.Count.Equals(4))
                    btnAdicionarPergunta.Enabled = false;

                txtPergunta.Text = string.Empty;
                rbNao.Checked = rbSim.Checked = false;
                gvPerguntasVaga.Visible = true;
            }
        }
        #endregion

        #region btComprar_Click
        protected void btComprar_Click(object sender, EventArgs e)
        {
            if (base.IdFilial.HasValue && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                var objPlano = Plano.LoadObject(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.VendaPlanoCIA_PlanoSmsEmailVagaIdentificador)));
                var quantidadePrazoBoleto = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPJ));
                var objFilial = Filial.LoadObject(base.IdFilial.Value);
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

                UsuarioFilial objUsuarioFilial;
                UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial);

                var objPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPJ(objUsuarioFilialPerfil, objFilial, objUsuarioFilial, objPlano, quantidadePrazoBoleto);

                Redirect("~/Pagamento_v2.aspx?IdPlanoAdquirido=" + objPlanoAdquirido.IdPlanoAdquirido.ToString() + "&Idf_Vaga=" + IdVaga.GetValueOrDefault().ToString());
            }
        }
        #endregion

        #region btnAvancar_Click
        protected void btnAvancar_Click(object sender, EventArgs e)
        {
            if (txtSalarioDe.Valor.HasValue && txtSalarioAte.Valor.HasValue && Decimal.Subtract(txtSalarioAte.Valor.Value, txtSalarioDe.Valor.Value) < 100 && !Decimal.Subtract(txtSalarioAte.Valor.Value, txtSalarioDe.Valor.Value).Equals(Decimal.Zero))
                ExibirMensagem("Quando informada uma faixa salarial, os valores informados devem ser iguais ou a diferença deve ser no mínimo de R$ 100,00.", TipoMensagem.Aviso);
            else if (string.IsNullOrEmpty(ucContratoFuncao.FuncaoDesc))
                ExibirMensagem("Preencha o campo função", TipoMensagem.Aviso);
            else
            {
                var saveResult = SalvarVaga();

                if (saveResult == null)
                    return;

                litCodVaga.Text = saveResult;
                pnlAnunciarVagaConferir.Visible = false;
                pnlAnunciarVaga.Visible = false;
                pnlAnunciarVagaConfirmacao.Visible = true;
                upAnunciarVaga.Update();
                upAnunciarVagaConferir.Update();

                upAnunciarVaga.Focus();

                upAnunciarVagaConfirmacao.Update();

                var objVaga = Vaga.LoadObject(IdVaga.Value);
                objVaga.Filial.CompleteObject();

                int idTipoVinculo = TipoVinculo.CarregarPorIdfVaga(IdVaga.Value);

                if (idTipoVinculo == (int)Enumeradores.TipoVinculo.Estágio)
                    pnlImgWebEstagio.Visible = true;
                else
                    if (objVaga.Filial.PossuiPlanoAtivo())
                        pnlImgR1.Visible = true;
                    else
                        pnlImgmVendaSMS.Visible = true;

                AjustarAnunciarVagaConfirmacao();
            }
        }
        #endregion

        #region btnSalvarAdm_Click
        protected void btnSalvarAdm_Click(object sender, EventArgs e)
        {
            if (IdVaga.HasValue)
            {
                if (SalvarVaga() != null)
                {
                    ExibirMensagem(MensagemAviso._24026, TipoMensagem.Aviso);
                    Response.Redirect("SalaAdministradorVagas.aspx", false);
                }
            }
        }
        #endregion

        #region btnArquivarAdm_Click
        protected void btnArquivarAdm_Click(object sender, EventArgs e)
        {
            if (IdVaga.HasValue)
            {
                Vaga objVaga = Vaga.LoadObject(IdVaga.Value);

                objVaga.FlagVagaArquivada = true;

                objVaga.Save();

                ExibirMensagem("Registro Salvo com Sucesso", TipoMensagem.Aviso);
                Response.Redirect("SalaAdministradorVagas.aspx", false);
            }
        }
        #endregion

        #region btnVoltarAdm_Click
        protected void btnVoltarAdm_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalaAdministradorVagas.aspx", false);
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (base.UrlDestinoPagamento.HasValue)
            {
                string paginaRedirect = base.UrlDestinoPagamento.Value;
                base.UrlDestinoPagamento.Clear();
                Response.Redirect(paginaRedirect, false);

            }
            else if (base.UrlDestino.HasValue)
            {
                string paginaRedirect = base.UrlDestino.Value;
                base.UrlDestino.Clear();
                Response.Redirect(paginaRedirect, false);
            }
            else
            {
                if (!string.IsNullOrEmpty(UrlOrigem))
                    Response.Redirect(UrlOrigem);
                else
                    Response.Redirect("Default.aspx", false);
            }
        }
        #endregion

        #region btnAnunciarVaga_Click
        protected void btnAnunciarVaga_Click(object sender, EventArgs e)
        {
            try
            {
                litCodVaga.Text = SalvarVaga();

                pnlAnunciarVagaConferir.Visible = false;
                pnlAnunciarVagaConfirmacao.Visible = true;

                upAnunciarVaga.Focus();

                upAnunciarVagaConfirmacao.Update();
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region txtSalarioDe_ValorAlterado
        protected void txtSalarioDe_ValorAlterado(object sender, EventArgs e)
        {
            AjustarFaixaSalarial();
        }
        #endregion

        #region txtSalarioAte_ValorAlterado
        protected void txtSalarioAte_ValorAlterado(object sender, EventArgs e)
        {
            AjustarFaixaSalarial();
        }
        #endregion

        #region ckbDeficiencia_CheckedChanged
        protected void ckbDeficiencia_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbDeficiencia.Checked)
                ddlTipoDeficiencia.SelectedValue = "7"; //Qualquer
            else
                ddlTipoDeficiencia.SelectedValue = "0"; //Nenhuma

            pnlTipoDeficiencia.Visible = ckbDeficiencia.Checked;
            upTipoDeficiencia.Update();
        }
        #endregion

        #region btQueroContratarWebEstagios_Click
        protected void btQueroContratarWebEstagios_Click(object sender, EventArgs e)
        {
            AbrirModal();
        }
        #endregion

        #region imgR1_Click
        protected void imgR1_Click(object sender, ImageClickEventArgs e)
        {
            Redirect("~/r1");
        }
        #endregion

        #region btnResultadoPesquisa_Click
        protected void btnResultadoPesquisa_Click(object sender, EventArgs e)
        {
            Session.Add(Chave.Temporaria.Variavel6.ToString(), IdPesquisaCurriculo);

            Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        private void Inicializar()
        {
            AjustarPermissoes();

            //Ajustando fluxo do RHOffice
            if (base.STC.Value)
            {
                if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                    InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "AnunciarVaga");
            }
            else
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "AnunciarVaga");

            AjustarTituloTela("Anunciar Vagas");

            if (!base.STC.Value)
                ExibirMenuSecaoEmpresa();

            pnlAnunciarVagaConfirmacao.Visible = false;
            pnlAnunciarVagaConferir.Visible = false;
            pnlAnunciarVaga.Visible = true;

            upAnunciarVagaConfirmacao.Update();
            upAnunciarVagaConferir.Update();
            upAnunciarVaga.Update();

            btnResultadoPesquisa.Visible = IdPesquisaCurriculo.HasValue;

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            CarregarParametros();

            UIHelper.CarregarRadComboBox(rcbEscolaridade, Escolaridade.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbSexo, Sexo.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbDisponibilidade, Disponibilidade.Listar());

            UIHelper.CarregarDropDownList(ddlTipoDeficiencia, Deficiencia.Listar());

            ucContratoFuncao.CarregarParametros();
            ucContratoFuncao.AtualizarValidationGroup(this.btnAvancar.ValidationGroup);
            upContratoFuncao.Update();

            ddlTipoDeficiencia.SelectedValue = "0";

            DTPerguntas = null; //Inicializar DataTable 

            PreencherCampos();

            //Ajustando a expressao de validacao.
            txtSalarioDe.ValorMinimo = Convert.ToDecimal(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioMinimoNacional));
            txtSalarioDe.MensagemErroIntervalo = string.Format("Valor mínimo de R$ {0}", txtSalarioDe.ValorMinimo);
            revEmail.ValidationExpression = Configuracao.regexEmail;

            var parametros = new
            {
                msgFaixaSalarial = "Faixa salarial para {0}: R$ {1} até R$ {2}"
            };
            ScriptManager.RegisterStartupScript(this, GetType(), "InicializarVariaveis", "javaScript:InicializarVariaveis(" + new JSONReflector(parametros) + ");", true);

            pnlPublicarAnunciarVaga.Visible = false;
            pnlBotoes.Visible = true;

            if (Permissoes.Contains((int)Enumeradores.Permissoes.ManterVaga.PublicarVaga))
            {
                pnlPublicarAnunciarVaga.Visible = true;
                pnlBotoes.Visible = false;
            }

            LblAtribuicaoColorDefault = lblAtribuicoesConfirmacao.ForeColor;
            LblRequisitoColorDefault = lblRequisitoConfirmacao.ForeColor;
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, Enumeradores.CategoriaPermissao.ManterVaga);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.ManterVaga.AcessarTelaAnunciarVaga))
                {
                    Session.Add(Chave.Temporaria.Variavel1.ToString(), MensagemAviso._300034);
                    Response.Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, Enumeradores.CategoriaPermissao.ManterVaga);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.ManterVaga.AcessarTelaAnunciarVaga))
                {
                    Session.Add(Chave.Temporaria.Variavel1.ToString(), MensagemAviso._300034);
                    Response.Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.AnunciarVaga }));
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            ucContratoFuncao.LimparCampos();
            rcbEscolaridade.SelectedValue = "0";
            txtSalarioDe.Valor = null;
            txtSalarioAte.Valor = null;
            txtBeneficios.Valor = String.Empty;
            txtIdadeMinima.Valor = String.Empty;
            txtIdadeMaxima.Valor = String.Empty;
            rcbSexo.SelectedValue = "0";
            txtNumeroVagas.Valor = String.Empty;
            txtRequisitos.Text = String.Empty;
            txtAtribuicoes.Text = String.Empty;
            txtNomeFantasia.Text = String.Empty;
            txtTelefone.DDD = String.Empty;
            txtTelefone.Fone = String.Empty;
            txtEmail.Text = String.Empty;
            cbConfidencial.Checked = false;
            cbReceberCadaCVEmail.Checked = false;
            cbReceberTodosCvsEmail.Checked = false;
            ddlTipoDeficiencia.SelectedValue = "0";
            ckbDeficiencia.Checked = false;
            pnlTipoDeficiencia.Visible = false;
            upTipoDeficiencia.Update();

            rcbDisponibilidade.ClearCheckeds();
            rcbDisponibilidade.ClearSelection();


            //Limpando data grid de perguntas
            DTPerguntas = null;
            gvPerguntasVaga.DataSource = DTPerguntas;
            gvPerguntasVaga.DataBind();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            if (IdVaga.HasValue)
            {
                Vaga objVaga = Vaga.LoadObject(IdVaga.Value);

                if (BoolClonarVaga) //Limpa o id da vaga para criar uma nova vaga e não sobrescrever
                    IdVaga = null;

                objVaga.Cidade.CompleteObject();
                txtCidadeAnunciarVaga.Text = UIHelper.FormatarCidade(objVaga.Cidade.NomeCidade, objVaga.Cidade.Estado.SiglaEstado);

                if (objVaga.Escolaridade != null)
                    rcbEscolaridade.SelectedValue = objVaga.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);

                txtSalarioDe.Valor = objVaga.ValorSalarioDe;
                txtSalarioAte.Valor = objVaga.ValorSalarioPara;

                AjustarFaixaSalarial();
                PreencherFaixaSalarial();

                if (objVaga.NumeroIdadeMinima != null)
                    txtIdadeMinima.Valor = objVaga.NumeroIdadeMinima.ToString();

                if (objVaga.NumeroIdadeMaxima != null)
                    txtIdadeMaxima.Valor = objVaga.NumeroIdadeMaxima.ToString();

                if (objVaga.Sexo != null)
                    rcbSexo.SelectedValue = objVaga.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);

                if (objVaga.QuantidadeVaga.HasValue)
                    txtNumeroVagas.Valor = objVaga.QuantidadeVaga.Value.ToString(CultureInfo.CurrentCulture);

                if (objVaga.Funcao != null)
                {
                    objVaga.Funcao.CompleteObject();
                    ucContratoFuncao.SetFuncao(objVaga.Funcao.DescricaoFuncao);
                    txtSugestaoTarefasAnunciarVaga.Text = objVaga.Funcao.DescricaoJob;
                    upTxtSugestaoTarefasAnunciarVaga.Update();
                }
                else
                {
                    ucContratoFuncao.SetFuncao(objVaga.DescricaoFuncao);
                }

                txtBeneficios.Valor = objVaga.DescricaoBeneficio;
                txtRequisitos.Text = objVaga.DescricaoRequisito;
                txtAtribuicoes.Text = objVaga.DescricaoAtribuicoes;
                txtNomeFantasia.Text = objVaga.NomeEmpresa;
                txtTelefone.DDD = objVaga.NumeroDDD;
                txtTelefone.Fone = objVaga.NumeroTelefone;
                txtEmail.Text = objVaga.EmailVaga;
                cbConfidencial.Checked = objVaga.FlagConfidencial;

                #region Publicação da vaga

                chkLiberarVaga.Checked = objVaga.FlagLiberada.HasValue && objVaga.FlagLiberada.Value;
                chkPublicarVaga.Checked = objVaga.FlagAuditada.HasValue && objVaga.FlagAuditada.Value;

                #endregion

                if (objVaga.FlagReceberCadaCV.HasValue)
                    cbReceberCadaCVEmail.Checked = objVaga.FlagReceberCadaCV.Value;

                if (objVaga.FlagReceberTodosCV.HasValue)
                    cbReceberTodosCvsEmail.Checked = objVaga.FlagReceberTodosCV.Value;

                //Deficiência
                if (objVaga.Deficiencia != null)
                {
                    ddlTipoDeficiencia.SelectedValue = objVaga.Deficiencia.IdDeficiencia.ToString(CultureInfo.CurrentCulture);
                    if (!ddlTipoDeficiencia.SelectedValue.Equals("0"))
                    {
                        ckbDeficiencia.Checked = true;
                        pnlTipoDeficiencia.Visible = true;
                        upTipoDeficiencia.Update();
                    }
                }

                if (objVaga.FlagDeficiencia.HasValue && objVaga.FlagDeficiencia.Value)
                {
                    ckbDeficiencia.Checked = true;
                    pnlTipoDeficiencia.Visible = true;
                    upTipoDeficiencia.Update();
                }

                txtPalavrasChave.Valor = string.Join(",", VagaPalavraChave.ListarPalavrasChave(objVaga.IdVaga).ToArray());

                //Grid de perguntas
                DTPerguntas = VagaPergunta.RecuperarPerguntas(objVaga.IdVaga, null);
                CarregarGrid();

                //Identifica os itens selecionados na dropdown de idioma.
                List<VagaDisponibilidade> listVagaDisponibilidade = VagaDisponibilidade.ListarDisponibilidadesPorVaga(objVaga.IdVaga);
                foreach (VagaDisponibilidade objVagaDisponibilidade in listVagaDisponibilidade)
                    rcbDisponibilidade.SetItemChecked(objVagaDisponibilidade.Disponibilidade.IdDisponibilidade.ToString(CultureInfo.CurrentCulture), true);

                List<VagaTipoVinculo> listVagaTipoVinculo = VagaTipoVinculo.ListarTipoVinculoPorVaga(objVaga.IdVaga);
                foreach (var checkItem in ucContratoFuncao.TipoContratoItens)
                {
                    var tipoVinculo = listVagaTipoVinculo.FirstOrDefault(obj =>
                                                                            obj.TipoVinculo.IdTipoVinculo
                                                                            .ToString(CultureInfo.CurrentCulture)
                                                                            .Equals(checkItem.Value));
                    checkItem.Selected = tipoVinculo != null;
                }
            }
            else if (IdPesquisaCurriculo.HasValue)
            {
                var objPesquisaCurriculo = BLL.PesquisaCurriculo.LoadObject(IdPesquisaCurriculo.Value);

                if (objPesquisaCurriculo.Funcao != null)
                {
                    objPesquisaCurriculo.Funcao.CompleteObject();
                    ucContratoFuncao.SetFuncao(objPesquisaCurriculo.Funcao.DescricaoFuncao);
                    txtSugestaoTarefasAnunciarVaga.Text = objPesquisaCurriculo.Funcao.DescricaoJob;
                    upTxtSugestaoTarefasAnunciarVaga.Update();
                }

                if (objPesquisaCurriculo.Cidade != null)
                {
                    objPesquisaCurriculo.Cidade.CompleteObject();
                    objPesquisaCurriculo.Cidade.Estado.CompleteObject();
                    txtCidadeAnunciarVaga.Text = UIHelper.FormatarCidade(objPesquisaCurriculo.Cidade.NomeCidade, objPesquisaCurriculo.Cidade.Estado.SiglaEstado);
                }

                if (objPesquisaCurriculo.Escolaridade != null)
                    rcbEscolaridade.SelectedValue = objPesquisaCurriculo.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);

                txtSalarioDe.Valor = objPesquisaCurriculo.NumeroSalarioMin;
                txtSalarioAte.Valor = objPesquisaCurriculo.NumeroSalarioMax;

                AjustarFaixaSalarial();
                PreencherFaixaSalarial();

                if (objPesquisaCurriculo.DataIdadeMin.HasValue)
                    txtIdadeMinima.Valor = (DateTime.Today.Year - objPesquisaCurriculo.DataIdadeMin.Value.Year).ToString(CultureInfo.CurrentCulture);

                if (objPesquisaCurriculo.DataIdadeMax.HasValue)
                    txtIdadeMaxima.Valor = (DateTime.Today.Year - objPesquisaCurriculo.DataIdadeMax.Value.Year).ToString(CultureInfo.CurrentCulture);

                if (objPesquisaCurriculo.Sexo != null)
                    rcbSexo.SelectedValue = objPesquisaCurriculo.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);

                PreencherCamposDadosFilial();

                //Deficiência
                if (objPesquisaCurriculo.Deficiencia != null)
                {
                    ddlTipoDeficiencia.SelectedValue = objPesquisaCurriculo.Deficiencia.IdDeficiencia.ToString(CultureInfo.CurrentCulture);
                    if (!ddlTipoDeficiencia.SelectedValue.Equals("0"))
                    {
                        ckbDeficiencia.Checked = true;
                        pnlTipoDeficiencia.Visible = true;
                        upTipoDeficiencia.Update();
                    }
                }

                //Identifica os itens selecionados na dropdown de idioma.
                var listaDisponibilidade = PesquisaCurriculoDisponibilidade.ListarPorPesquisa(objPesquisaCurriculo);
                foreach (var objDisponibilidade in listaDisponibilidade)
                    rcbDisponibilidade.SetItemChecked(objDisponibilidade.Disponibilidade.IdDisponibilidade.ToString(CultureInfo.CurrentCulture), true);

                //var listVagaTipoVinculo = PesquisaCurriculoTipoVinculo.ListarPorPesquisaList(objPesquisaCurriculo.IdPesquisaCurriculo);
                //foreach (var checkItem in ucContratoFuncao.TipoContratoItens)
                //{
                //    var tipoVinculo = listVagaTipoVinculo.FirstOrDefault(obj =>
                //                                                            obj.TipoVinculo.IdTipoVinculo
                //                                                            .ToString(CultureInfo.CurrentCulture)
                //                                                            .Equals(checkItem.Value));
                //    checkItem.Selected = tipoVinculo != null;
                //}

            }
            else
                PreencherCamposDadosFilial();
        }
        #endregion

        #region PreencherCamposDadosFilial
        private void PreencherCamposDadosFilial()
        {
            if (base.IdFilial.HasValue)
            {
                var dtoFilial = Filial.CarregarDTO(base.IdFilial.Value);

                txtNomeFantasia.Text = dtoFilial.NomeFantasia;
                txtTelefone.DDD = dtoFilial.NumeroDDDComercial;
                txtTelefone.Fone = dtoFilial.NumeroComercial;

                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                    txtEmail.Text = objUsuarioFilial.EmailComercial;
            }

            txtNumeroVagas.Valor = "1";
        }
        #endregion

        #region PreencherFaixaSalarial
        private void PreencherFaixaSalarial()
        {
            string pesquisa = PesquisarMediaSalarial(ucContratoFuncao.FuncaoDesc, txtCidadeAnunciarVaga.Text);

            if (!string.IsNullOrEmpty(pesquisa))
            {
                faixa_salarial.InnerHtml = string.Format("Faixa salarial para {0}: R$ {1} até R$ {2}", ucContratoFuncao.FuncaoDesc, pesquisa.Split(';')[0], pesquisa.Split(';')[1]);
                faixa_salarial.Style["display"] = "block";
            }
            else
            {
                faixa_salarial.InnerText = string.Empty;
                faixa_salarial.Style["display"] = "none";
            }
        }
        #endregion

        #region AjustarIdade
        /// <summary>
        /// Carrega os parâmetros iniciais da aba de dados gerais.
        /// </summary>
        private void AjustarIdade()
        {
            try
            {
                var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.IdadeMinima,
                        Enumeradores.Parametro.IdadeMaxima
                    };

                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                string idadeMinima = valoresParametros[Enumeradores.Parametro.IdadeMinima];
                string idadeMaxima = valoresParametros[Enumeradores.Parametro.IdadeMaxima];
                txtIdadeMinima.ValorMinimo = idadeMinima;
                txtIdadeMinima.ValorMaximo = idadeMaxima;
                txtIdadeMaxima.ValorMinimo = idadeMinima;
                txtIdadeMaxima.ValorMaximo = idadeMaxima;

                txtIdadeMinima.MensagemErroIntervalo = String.Format(MensagemAviso._100005, idadeMinima, idadeMaxima);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region CarregarParametros
        /// <summary>
        /// Carrega os parâmetros iniciais da aba de dados gerais.
        /// </summary>
        private void CarregarParametros()
        {
            ucContratoFuncao.CarregarParametros();
        }
        #endregion

        #region AjustarAnunciarVagaConfirmacao
        private void AjustarAnunciarVagaConfirmacao()
        {
            CarregarGridConfirmacao();
            divIdadeConfirmacao.Visible = true;
            divSexoConfirmacao.Visible = true;
            divSalarioConfirmacao.Visible = true;
            divBeneficiosConfirmacao.Visible = true;
            divNumeroVagasConfirmacao.Visible = true;
            divHorarioConfirmacao.Visible = true;
            divTipoContratoConfirmacao.Visible = true;
            divRequisitoConfirmacao.Visible = true;
            divEscolaridadeConfirmacao.Visible = true;
            divDeficienciaConfirmacao.Visible = true;

            //Campos Obrigatórios
            lblFuncaoConfirmacaoValor.Text = ucContratoFuncao.FuncaoDesc;
            lblCidadeConfirmacaoValor.Text = txtCidadeAnunciarVaga.Text;
            lblEmailConfirmacaoValor.Text = txtEmail.Text;
            lblNumeroVagasConfirmacaoValor.Text = txtNumeroVagas.Valor;

            //Campos não Obrigatórios
            if (!string.IsNullOrEmpty(txtIdadeMaxima.Valor) || !string.IsNullOrEmpty(txtIdadeMinima.Valor))
                lblIdadeConfirmacaoValor.Text = string.Format("{0} até {1}", txtIdadeMinima.Valor, txtIdadeMaxima.Valor);
            else
                divIdadeConfirmacao.Visible = false;

            if (!string.IsNullOrEmpty(rcbSexo.Text) && !rcbSexo.Text.Equals("Selecione"))
                lblSexoConfirmacaoValor.Text = rcbSexo.Text;
            else
                divSexoConfirmacao.Visible = false;

            if (txtSalarioDe.Valor.HasValue && txtSalarioAte.Valor.HasValue)
                lblSalarioConfirmacaoValor.Text = string.Format("{0} a {1}", txtSalarioDe.Valor, txtSalarioAte.Valor);
            else
                divSalarioConfirmacao.Visible = false;

            if (!string.IsNullOrEmpty(txtBeneficios.Valor))
                lblBeneficiosConfirmacaoValor.Text = txtBeneficios.Valor;
            else
                divBeneficiosConfirmacao.Visible = false;

            if (!string.IsNullOrEmpty(rcbDisponibilidade.Text))
                lblHorarioConfirmacaoValor.Text = rcbDisponibilidade.Text;
            else
                divHorarioConfirmacao.Visible = false;

            if (!string.IsNullOrEmpty(rcbEscolaridade.Text) && !rcbEscolaridade.Text.Equals("Selecione"))
                lblEscolaridadeConfirmacaoValor.Text = rcbEscolaridade.Text;
            else
                divEscolaridadeConfirmacao.Visible = false;

            var checkedItems = ucContratoFuncao.TipoContratoItens.Where(obj => obj.Selected).Select(a => a.Text).ToArray();
            if (checkedItems.Length > 0)
                lblTipoContratoConfirmacaoValor.Text = checkedItems.Aggregate((a, b) => a + ", " + b);
            else
                divTipoContratoConfirmacao.Visible = false;

            //if (!string.IsNullOrEmpty(rcbTipoContrato.Text))
            //lblTipoContratoConfirmacaoValor.Text = rcbTipoContrato.Text;
            //else
            //divTipoContratoConfirmacao.Visible = false;

            if (!string.IsNullOrEmpty(txtRequisitos.Text))
            {
                lblRequisitoConfirmacaoValor.Text = txtRequisitos.Text;
                lblRequisitoConfirmacaoValor.ForeColor = LblRequisitoColorDefault;
            }
            else
            {
                lblRequisitoConfirmacaoValor.Text = "Campo não preenchido";
                lblRequisitoConfirmacaoValor.ForeColor = Color.Red;
            }

            if (!string.IsNullOrEmpty(txtAtribuicoes.Text))
            {
                lblAtribuicoesConfirmacaoValor.Text = txtAtribuicoes.Text;
                lblAtribuicoesConfirmacaoValor.ForeColor = LblAtribuicaoColorDefault;
            }
            else
            {
                lblAtribuicoesConfirmacaoValor.Text = "Campo não preenchido";
                lblAtribuicoesConfirmacaoValor.ForeColor = Color.Red;
            }

            //Deficiência
            if (!ddlTipoDeficiencia.SelectedValue.Equals("0", StringComparison.InvariantCultureIgnoreCase))
                lblDeficienciaConfirmacaoValor.Text = ddlTipoDeficiencia.SelectedItem.Text;
            else
                lblDeficienciaConfirmacaoValor.Text = "Não há!";

            if (cbConfidencial.Checked)
            {
                lblNomeFantasiaConfirmacaoValor.Text = lblTelefoneConfirmacaoValor.Text = "Dado Confidencial";
                lblNomeFantasiaConfirmacaoValor.ForeColor = lblTelefoneConfirmacaoValor.ForeColor = Color.Red;
            }
            else
            {
                if (!string.IsNullOrEmpty(txtTelefone.Fone) && !string.IsNullOrEmpty(txtTelefone.DDD))
                    lblTelefoneConfirmacaoValor.Text = UIHelper.FormatarTelefone(txtTelefone.DDD, txtTelefone.Fone);
                if (!string.IsNullOrEmpty(txtNomeFantasia.Text))
                    lblNomeFantasiaConfirmacaoValor.Text = txtNomeFantasia.Text;
            }

            cbReceberCadaCVEmailConfirmacao.Checked = cbReceberCadaCVEmail.Checked;
            cbReceberTodosCvsEmailConfirmacao.Checked = cbReceberTodosCvsEmail.Checked;

            upAnunciarVagaConferir.Update();
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            UIHelper.CarregarRadGrid(gvPerguntasVaga, DTPerguntas);
            upGvPerguntasVaga.Update();
        }
        #endregion

        #region CarregarGridConfirmacao
        private void CarregarGridConfirmacao()
        {
            if (DTPerguntas != null)
            {
                UIHelper.CarregarRadGrid(gvAnunciarVagasConfirmacao, DTPerguntas);
                upGvAnunciarVagasConfirmacao.Update();
            }
        }
        #endregion

        #region SalvarVaga
        private string SalvarVaga()
        {
            #region Parametros
            var parms = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.QuantidadeDiasPrazoVaga,
                    Enumeradores.Parametro.IdadeMinima,
                    Enumeradores.Parametro.IdadeMaxima,
                    Enumeradores.Parametro.SalarioMinimoNacional
                };
            var valores = Parametro.ListarParametros(parms);
            #endregion

            #region Validação
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                ExibirMensagem("O campo \"E-mail confidencial para retorno\" deve ser informado", TipoMensagem.Erro);
                return null;
            }
            if (!string.IsNullOrEmpty(txtIdadeMinima.Valor))
            {
                var idadeAtual = Convert.ToInt32(txtIdadeMinima.Valor);
                var idadeMinima = Convert.ToInt32(valores[Enumeradores.Parametro.IdadeMinima]);
                var idadeMaxima = Convert.ToInt32(valores[Enumeradores.Parametro.IdadeMaxima]);

                if (idadeAtual < idadeMinima || idadeAtual > idadeMaxima)
                {
                    ExibirMensagem(String.Format(MensagemAviso._100005, idadeMinima, idadeMaxima), TipoMensagem.Erro);
                    return null;
                }
            }
            if (!string.IsNullOrEmpty(txtIdadeMaxima.Valor))
            {
                var idadeAtual = Convert.ToInt32(txtIdadeMaxima.Valor);
                var idadeMinima = Convert.ToInt32(valores[Enumeradores.Parametro.IdadeMinima]);
                var idadeMaxima = Convert.ToInt32(valores[Enumeradores.Parametro.IdadeMaxima]);

                if (idadeAtual < idadeMinima || idadeAtual > idadeMaxima)
                {
                    ExibirMensagem(String.Format(MensagemAviso._100005, idadeMinima, idadeMaxima), TipoMensagem.Erro);
                    return null;
                }
            }
            if (txtSalarioDe.Valor.HasValue)
            {
                var salarioMinimo = Convert.ToDecimal(valores[Enumeradores.Parametro.SalarioMinimoNacional]);

                if (txtSalarioDe.Valor < salarioMinimo)
                {
                    ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que o Salário Mínimo Nacional R$ {0}", salarioMinimo), TipoMensagem.Erro);
                    return null;
                }
            }

            if (txtSalarioAte.Valor.HasValue)
            {
                var salarioMinimo = Convert.ToDecimal(valores[Enumeradores.Parametro.SalarioMinimoNacional]);

                if (txtSalarioAte.Valor < salarioMinimo)
                {
                    ExibirMensagem(string.Format("Faixa salarial máxima deve ser maior que o Salário Mínimo Nacional R$ {0}", salarioMinimo), TipoMensagem.Erro);
                    return null;
                }
            }

            Funcao objFuncao;
            Funcao.CarregarPorDescricao(ucContratoFuncao.FuncaoDesc, out objFuncao);
            if (objFuncao == null)
            {
                ExibirMensagem("Função Inválida. Só será possível salvar após sua correção", TipoMensagem.Erro);
                return null;
            }

            if (!FuncaoValidaIntegracaoWebEstagios(objFuncao))
            {
                ExibirMensagem("Função Indisponível. Só será possível salvar após a mudança.", TipoMensagem.Erro);
                return null;
            }

            //validar tipo do contrato com a escolaridade



            #endregion

            #region Vaga

            bool empresaEmAuditoria = false;

            if (base.IdFilial.HasValue)
            {
                var objFilial = new Filial(base.IdFilial.Value);
                empresaEmAuditoria = objFilial.EmpresaEmAuditoria();
            }

            Vaga objVaga = (IdVaga.HasValue) ? Vaga.LoadObject(IdVaga.Value) : new Vaga
                {
                    FlagVagaRapida = false,
                    FlagInativo = false,
                    FlagAuditada = false,
                    FlagEmpresaEmAuditoria = empresaEmAuditoria
                };

            var jaAuditada = objVaga.FlagAuditada;

            objVaga.DescricaoFuncao = ucContratoFuncao.FuncaoDesc;
            objVaga.Funcao = objFuncao;

            Cidade objCidade;
            if (Cidade.CarregarPorNome(txtCidadeAnunciarVaga.Text, out objCidade))
                objVaga.Cidade = objCidade;
            else
            {
                objVaga.Cidade = null;
                ExibirMensagem("Cidade Inexistente.", TipoMensagem.Erro);
                return null;
            }

            if (!rcbEscolaridade.SelectedValue.Equals("0"))
                objVaga.Escolaridade = new Escolaridade(Convert.ToInt32(rcbEscolaridade.SelectedValue));

            objVaga.ValorSalarioDe = txtSalarioDe.Valor;
            objVaga.ValorSalarioPara = txtSalarioAte.Valor;
            objVaga.DescricaoBeneficio = txtBeneficios.Valor;

            short idade;
            if (Int16.TryParse(txtIdadeMinima.Valor, out idade))
                objVaga.NumeroIdadeMinima = idade;

            if (Int16.TryParse(txtIdadeMaxima.Valor, out idade))
                objVaga.NumeroIdadeMaxima = idade;

            if (!rcbSexo.SelectedValue.Equals("0"))
                objVaga.Sexo = new Sexo(Convert.ToInt32(rcbSexo.SelectedValue));

            objVaga.QuantidadeVaga = Convert.ToInt16(txtNumeroVagas.Valor);
            objVaga.DescricaoRequisito = txtRequisitos.Text;
            objVaga.DescricaoAtribuicoes = txtAtribuicoes.Text;
            objVaga.Deficiencia = new Deficiencia(Convert.ToInt16(ddlTipoDeficiencia.SelectedValue.Substring(0, 1)));
            objVaga.FlagDeficiencia = ckbDeficiencia.Checked;
            objVaga.NomeEmpresa = txtNomeFantasia.Text;
            objVaga.NumeroDDD = txtTelefone.DDD;
            objVaga.NumeroTelefone = txtTelefone.Fone;
            objVaga.EmailVaga = txtEmail.Text;
            objVaga.FlagConfidencial = cbConfidencial.Checked;
            objVaga.FlagReceberCadaCV = cbReceberCadaCVEmail.Checked;
            objVaga.FlagReceberTodosCV = cbReceberTodosCvsEmail.Checked;
            objVaga.FlagLiberada = false;
            objVaga.FlagAuditada = false;
            objVaga.DataAbertura = DateTime.Now;
            objVaga.DataPrazo = DateTime.Now.AddDays(Convert.ToInt32(valores[Enumeradores.Parametro.QuantidadeDiasPrazoVaga]));

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                objVaga.UsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                objVaga.Filial = new Filial(base.IdFilial.Value);

                //Verifica se a filial possui origem
                //Se possuir, sempre salva a vaga para o seu STC
                OrigemFilial objOrigem = new OrigemFilial();
                if (OrigemFilial.CarregarPorFilial(base.IdFilial.Value, out objOrigem))
                {
                    objOrigem.Origem.CompleteObject();
                    objVaga.Origem = objOrigem.Origem;
                }
                else
                {
                    objVaga.Origem = new Origem(base.IdOrigem.Value);
                }
            }

            #endregion

            bool auditada = false;

            #region Publicação da vaga
            if (base.IdPerfil.HasValue)
            {
                if ((base.IdPerfil.Value == (int)Enumeradores.Perfil.AdministradorSistema) || (base.IdPerfil.Value == (int)Enumeradores.Perfil.Revisor))
                {
                    if (!jaAuditada.GetValueOrDefault() && chkPublicarVaga.Checked)
                    {
                        auditada = true;
                        objVaga.DataAuditoria = DateTime.Now;
                    }

                    objVaga.FlagLiberada = chkLiberarVaga.Checked;
                    objVaga.FlagAuditada = chkPublicarVaga.Checked;

                    if (objVaga.FlagAuditada.Value)
                    {
                        if (string.IsNullOrEmpty(txtPalavrasChave.Valor))
                        {
                            ExibirMensagem("O campo palavra-chave deve ser informado", TipoMensagem.Erro);
                            return null;
                        }
                    }

                }

                objVaga.Cidade.CompleteObject();
                objVaga.Cidade.Estado.CompleteObject();
                objVaga.Funcao.CompleteObject();
                objVaga.Filial.CompleteObject();
            }
            #endregion

            #region VagaDisponibilidade
            var listVagaDisponibilidade = rcbDisponibilidade.GetCheckedItems().Select(item => new VagaDisponibilidade
                {
                    Disponibilidade = new Disponibilidade(Convert.ToInt32(item.Value)),
                    Vaga = objVaga
                }).ToList();
            #endregion

            #region VagaTipoVinculo

            var listVagaTipoVinculo = ucContratoFuncao.TipoContratoItens.Where(obj => obj.Selected).Select(item => new VagaTipoVinculo
                {
                    Vaga = objVaga,
                    TipoVinculo = new BNE.BLL.TipoVinculo(Convert.ToInt32(item.Value))
                }).ToList();
            #endregion

            #region VagaPergunta
            var listVagaPergunta = new List<VagaPergunta>();
            if (DTPerguntas != null)
            {
                for (int i = 0; i < DTPerguntas.Rows.Count; i++)
                {
                    int idVaga;

                    VagaPergunta objVagaPergunta;
                    if (Int32.TryParse(DTPerguntas.Rows[i]["Idf_Vaga_Pergunta"].ToString(), out idVaga))
                        objVagaPergunta = VagaPergunta.LoadObject(idVaga);
                    else
                    {
                        objVagaPergunta = new VagaPergunta
                            {
                                DescricaoVagaPergunta = DTPerguntas.Rows[i]["Des_Vaga_Pergunta"].ToString(),
                                FlagResposta = DTPerguntas.Rows[i]["Flg_Resposta"].ToString().Equals("Sim"),
                                Vaga = objVaga,
                                TipoResposta = new TipoResposta(Convert.ToInt32(DTPerguntas.Rows[i]["Idf_Tipo_Resposta"]))
                            };
                    }

                    listVagaPergunta.Add(objVagaPergunta);
                }
            }
            #endregion

            #region Palavras-Chave
            List<string> listPalavrasChave = string.IsNullOrEmpty(txtPalavrasChave.Valor) ? new List<string>() : txtPalavrasChave.Valor.Split(',').ToList();
            #endregion

            objVaga.SalvarVaga(listVagaDisponibilidade, listVagaTipoVinculo, listVagaPergunta, listPalavrasChave, auditada);

            IdVaga = objVaga.IdVaga;

            if (!empresaEmAuditoria)
            {
                Usuario objUsuario;
                if (Usuario.CarregarPorPessoaFisica(base.IdPessoaFisicaLogada.Value, out objUsuario))
                {
                    if (!auditada) //Se a vaga não está publicada, manda para publicação automática
                    {
                        var parametros = new ParametroExecucaoCollection
                            {
                                {"idVaga", "Vaga", objVaga.IdVaga.ToString(CultureInfo.InvariantCulture), objVaga.CodigoVaga},
                                {"EnfileraRastreador", "Deve enfileirar rastreador", "true", "Verdadeiro"}
                            };

                        ProcessoAssincrono.IniciarAtividade(
                            TipoAtividade.PublicacaoVaga,
                            PluginsCompatibilidade.CarregarPorMetadata("PublicacaoVaga", "PublicacaoVagaRastreador"),
                            parametros,
                            null,
                            null,
                            null,
                            null,
                            DateTime.Now);
                    }

                    //Se foi auditada agora, por falha no processo automático, envia a vaga para o precesso do rastreador assincrono
                    if (auditada && (objVaga.Deficiencia == null || (objVaga.Deficiencia != null && objVaga.Deficiencia.IdDeficiencia.Equals(0))) && !objVaga.Funcao.IdFuncao.Equals((int)Enumeradores.Funcao.Estagiario))
                    {
                        var parametros = new ParametroExecucaoCollection
                            {
                                {"idVaga", "Vaga", objVaga.IdVaga.ToString(CultureInfo.InvariantCulture), objVaga.CodigoVaga}
                            };

                        ProcessoAssincrono.IniciarAtividade(
                            TipoAtividade.RastreadorVagas,
                            PluginsCompatibilidade.CarregarPorMetadata("RastreadorVagas", "PluginSaidaEmailSMS"),
                            parametros,
                            null,
                            null,
                            null,
                            null,
                            DateTime.Now);
                    }
                }
            }

            return objVaga.CodigoVaga;
        }

        private bool FuncaoValidaIntegracaoWebEstagios(Funcao objFuncao)
        {
            return Funcao.ValidarFuncaoLimitacaoIntegracaoWebEstagios(objFuncao.DescricaoFuncao);
        }
        #endregion

        #region AjustarFaixaSalarial
        private void AjustarFaixaSalarial()
        {
            if (txtSalarioDe.Valor.HasValue)
            {
                txtSalarioAte.ValorMinimo = txtSalarioDe.Valor.Value;
                txtSalarioAte.MensagemErroIntervalo = string.Format("Valor mínimo de R$ {0}", txtSalarioAte.ValorMinimo);
            }

            if (txtSalarioAte.Valor.HasValue)
            {
                txtSalarioDe.ValorMaximo = txtSalarioAte.Valor.Value;
                txtSalarioDe.MensagemErroIntervalo = string.Format("Valor mínimo de R$ {0} e máximo de R$ {1}", txtSalarioDe.ValorMinimo, txtSalarioDe.ValorMaximo);
            }
        }
        #endregion

        #region AbrirModal

        private void AbrirModal()
        {

            EnvioEmail();
        }

        #endregion

        #region EnvioEmail
        private void EnvioEmail()
        {
       

            ucModalEnvioEmail.MostrarModal(BNE.Web.UserControls.Modais.EnvioEmail.TipoEnvioEmail.EmailWebEstagios);
            pnlLEmail.Visible = true;
            upLEmail.Update();
        }
        #endregion

       

        #endregion

        #region AjaxMethod

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
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

        #region PesquisarMediaSalarial
        /// <summary>
        /// Média salarial
        /// </summary>
        /// <param name="desFuncao"></param>
        /// <param name="cidadeEstado"></param>
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
        /// <param name="valorFuncao"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string RecuperarJobFuncao(string valorFuncao)
        {
            if (string.IsNullOrWhiteSpace(valorFuncao))
                return string.Empty;

            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(valorFuncao, out objFuncao))
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