using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using JSONSharp;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using Parametro = BNE.BLL.Parametro;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;

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

        #region ClonarVaga - Variável 8
        public bool ClonarVaga
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

        #region urlApiSalarioBR
        public String urlApiSalarioBR
        {
            get
            {
                return ConfigurationManager.AppSettings["urlApiSalarioBR"];
            }
        }
        #endregion

        #region idPlanoRelatorioSalarioBR
        public String idPlanoRelatorioSalarioBR
        {
            get
            {
                return Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoRelatorioSalarialEmpresasIdentificador);
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

            ucContratoFuncao.MarcouEstagio += this.AvisaEstagio;
            ucContratoFuncao.MarcouAprendiz += this.AvisaAprendiz;

            if (DeveMostrarModalSucessoFacebook())
            {
                ucModalCompartilhamentoVaga.Inicializar();
                ucModalCompartilhamentoVaga.MostrarModal();
                pnlCompVaga.Visible = true;
                upCompVaga.Update();
            }

            AjustarIdade();
            Ajax.Utility.RegisterTypeForAjax(typeof(AnunciarVaga));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Registra eventos controle", "LocalPageLoad();", true);
        }
        #endregion

        #region btiEuQuero_Click
        protected void btiEuQuero_Click(object sender, ImageClickEventArgs e)
        {
            if (new Filial(base.IdFilial.Value).PossuiPlanoAtivo())
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
            rbDescritiva.Checked = false;
            upRespostas.Update();
        }
        #endregion

        #region rbNao_CheckedChanged
        protected void rbNao_CheckedChanged(object sender, EventArgs e)
        {
            rbSim.Checked = false;
            rbNao.Checked = true;
            rbDescritiva.Checked = false;
            upRespostas.Update();
        }
        #endregion

        #region rbDescritiva_CheckedChanged
        protected void rbDescritiva_CheckedChanged(object sender, EventArgs e)
        {
            rbSim.Checked = false;
            rbNao.Checked = false;
            rbDescritiva.Checked = true;
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
            else if (!rbNao.Checked && !rbSim.Checked && !rbDescritiva.Checked)
                ExibirMensagem(MensagemAviso._23007, TipoMensagem.Aviso);
            else
            {
                DTPerguntas = VagaPergunta.DataTablePerguntas(DTPerguntas, null, txtPergunta.Text, rbSim.Checked, rbDescritiva.Checked ? Enumeradores.TipoResposta.RespostaDescritiva : Enumeradores.TipoResposta.RespostaObjetiva);
                CarregarGrid();

                if (DTPerguntas.Rows.Count.Equals(4))
                    btnAdicionarPergunta.Enabled = false;

                txtPergunta.Text = string.Empty;
                rbNao.Checked = rbSim.Checked = rbDescritiva.Checked = false;
                gvPerguntasVaga.Visible = true;
            }
        }
        #endregion

        #region btnAvancar_Click
        protected void btnAvancar_Click(object sender, EventArgs e)
        {
            decimal? valorDe = null;
            decimal? valorPara = null;

            if (!string.IsNullOrWhiteSpace(txtSalarioDe.Text))
                valorDe = Convert.ToDecimal(txtSalarioDe.Text);

            if (!string.IsNullOrWhiteSpace(txtSalarioAte.Text))
                valorPara = Convert.ToDecimal(txtSalarioAte.Text);

            if (valorDe.HasValue && valorPara.HasValue && Decimal.Subtract(valorPara.Value, valorDe.Value) < 100 && !Decimal.Subtract(valorPara.Value, valorDe.Value).Equals(Decimal.Zero))
                ExibirMensagem("Quando informada uma faixa salarial, os valores informados devem ser iguais ou a diferença deve ser no mínimo de R$ 100,00.", TipoMensagem.Aviso);
            else if (string.IsNullOrEmpty(ucContratoFuncao.FuncaoDesc))
                ExibirMensagem("Preencha o campo função", TipoMensagem.Aviso);
            else
            {
                var objVaga = SalvarVaga();

                if (objVaga == null)
                    return;

                // litCodVaga.Text = objVaga.CodigoVaga;
                pnlAnunciarVaga.Visible = false;
                pnlAnunciarVagaConfirmacao.Visible = true;
                upAnunciarVaga.Update();

                upAnunciarVagaConfirmacao.Update();

                if (objVaga.Filial.PossuiPlanoAtivo())
                {
                    //pnlCodigoVaga.CssClass = "esquerda_sucesso";
                    pnlSejaAssinante.Visible = false;
                    btnVagasAnunciadas.Focus();
                }

                else { btnQueroAssinar.Focus(); btnVagasAnunciadas.Visible = false; }

                PublicarFacebook();
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

                objVaga.Save(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null);

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

        #region imgR1_Click
        protected void imgR1_Click(object sender, ImageClickEventArgs e)
        {
            Redirect("~/r1");
        }
        #endregion

        #region AvisaEstagio
        private void AvisaEstagio(bool tem_estagio)
        {
            hfEstagio.Value = tem_estagio ? "1" : "0";
        }
        #endregion

        #region AvisAprendiz
        private void AvisaAprendiz(bool tem_aprendiz)
        {
            hfAprendiz.Value = tem_aprendiz ? "1" : "0";
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

            pnlAnunciarVagaConfirmacao.Visible = false;
            pnlAnunciarVaga.Visible = true;

            upAnunciarVagaConfirmacao.Update();
            upAnunciarVaga.Update();

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            UIHelper.CarregarRadComboBox(rcbEscolaridade, Escolaridade.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbSexo, Sexo.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbDisponibilidade, Disponibilidade.Listar());

            UIHelper.CarregarDropDownList(ddlTipoDeficiencia, Deficiencia.Listar());

            ucContratoFuncao.AtualizarValidationGroup(this.btnAvancar.ValidationGroup);
            upContratoFuncao.Update();

            ddlTipoDeficiencia.SelectedValue = "0";

            DTPerguntas = null; //Inicializar DataTable 

            hdfValorSalarioMinimo.Value = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioMinimoNacional);
            hdfValorBolsamInimo.Value = Parametro.RecuperaValorParametro(Enumeradores.Parametro.BolsaMinimaEstagio);
            hdfValorBolsaMinimoAprendiz.Value = Parametro.RecuperaValorParametro(Enumeradores.Parametro.BolsaMinimaAprendiz);

            PreencherCampos();

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

            //Em caso de STC esconde a opção de confidencialidade
            if (STC.HasValue)
                cbConfidencial.Visible = !STC.Value;
            //------------------------------------
            txtSalarioDe.Attributes["OnBlur"] += "ValidarMediaSalarial();";
            txtSalarioAte.Attributes["OnBlur"] += "ValidarMediaSalarial();";
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

        #region PreencherCampos
        private void PreencherCampos()
        {
            if (IdVaga.HasValue)
            {
                Vaga objVaga = Vaga.LoadObject(IdVaga.Value);

                if (ClonarVaga) //Limpa o id da vaga para criar uma nova vaga e não sobrescrever
                    IdVaga = null;

                objVaga.Cidade.CompleteObject();
                txtCidadeAnunciarVaga.Text = UIHelper.FormatarCidade(objVaga.Cidade.NomeCidade, objVaga.Cidade.Estado.SiglaEstado);

                if (objVaga.Escolaridade != null)
                    rcbEscolaridade.SelectedValue = objVaga.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);

                if (objVaga.ValorSalarioDe.HasValue)
                    txtSalarioDe.Text = objVaga.ValorSalarioDe.Value.ToString("N2");

                if (objVaga.ValorSalarioPara.HasValue)
                    txtSalarioAte.Text = objVaga.ValorSalarioPara.Value.ToString("N2");

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
                    ucContratoFuncao.SetFuncao(string.Empty);
                }

                PreencherFaixaSalarial();

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

                //Bairro da vaga
                if (objVaga.IdBairro != null)
                {
                    BLL.DTO.Bairro objBairroVaga = RetornarBairro(objVaga.IdBairro.Value);

                    if (objBairroVaga != null)
                        txtBairroAnunciarVaga.Text = objBairroVaga.Nome;
                }
                else
                    txtBairroAnunciarVaga.Text = objVaga.NomeBairro;

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

                //Grid de perguntas
                DTPerguntas = VagaPergunta.RecuperarPerguntas(objVaga, null);
                CarregarGrid();

                //Identifica os itens selecionado na dropdown de disponibilidade.
                var listaDisponibilidade = VagaDisponibilidade.ListarIdentificadoresDisponibilidadePorVaga(objVaga);
                foreach (var item in listaDisponibilidade)
                    rcbDisponibilidade.SetItemChecked(item.ToString(CultureInfo.CurrentCulture), true);

                List<VagaTipoVinculo> listVagaTipoVinculo = VagaTipoVinculo.ListarTipoVinculoPorVaga(objVaga.IdVaga);
                foreach (var checkItem in ucContratoFuncao.TipoContratoItens)
                {
                    var tipoVinculo = listVagaTipoVinculo.FirstOrDefault(obj =>
                                                                            obj.TipoVinculo.IdTipoVinculo
                                                                            .ToString(CultureInfo.CurrentCulture)
                                                                            .Equals(checkItem.Value));
                    checkItem.Selected = tipoVinculo != null;
                }

                // Carregando cursos da vaga
                List<VagaCurso> listVagaCurso = VagaCurso.ListarCursoPorVaga(objVaga.IdVaga);
                ucContratoFuncao.DescricoesCursos = listVagaCurso.Select(vg => vg.Curso == null ? vg.DescricaoCurso : vg.Curso.DescricaoCurso).ToList();

                if (ucContratoFuncao.TemEstagio)
                    this.AvisaEstagio(true);
                else if (ucContratoFuncao.TemAprendiz)
                    this.AvisaAprendiz(true);

                //else if(ucContratoFuncao)
                //Se não é um usuário interno, desabilita alguns campos para edição
                if (!IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue && !ClonarVaga && objVaga.Funcao != null)
                {
                    DesabilitarCamposEdicaoVaga();
                }
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

        #region CarregarGrid
        private void CarregarGrid()
        {
            UIHelper.CarregarRadGrid(gvPerguntasVaga, DTPerguntas);
            upGvPerguntasVaga.Update();
        }
        #endregion

        #region SalvarVaga
        private Vaga SalvarVaga()
        {
            #region Parametros
            var parms = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.QuantidadeDiasPrazoVaga,
                    Enumeradores.Parametro.IdadeMinima,
                    Enumeradores.Parametro.IdadeMaxima,
                    Enumeradores.Parametro.SalarioMinimoNacional,
                    Enumeradores.Parametro.BolsaMinimaEstagio,
                    Enumeradores.Parametro.BolsaMinimaAprendiz
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

            decimal? valorDe = null;
            decimal? valorAte = null;

            if (!string.IsNullOrWhiteSpace(txtSalarioDe.Text))
                valorDe = Convert.ToDecimal(txtSalarioDe.Text);

            if (!string.IsNullOrWhiteSpace(txtSalarioAte.Text))
                valorAte = Convert.ToDecimal(txtSalarioAte.Text);

            Funcao objFuncao;
            Funcao.CarregarPorDescricao(ucContratoFuncao.FuncaoDesc, out objFuncao);
            if (objFuncao == null)
            {
                ExibirMensagem("Função Inválida. Só será possível salvar após sua correção", TipoMensagem.Erro);
                return null;
            }

            var minimoNacional = Convert.ToDecimal(valores[Enumeradores.Parametro.SalarioMinimoNacional]);
            var BolsaMinimoEstagio = Convert.ToDecimal(valores[Enumeradores.Parametro.BolsaMinimaEstagio]);
            var BolsaMinimoAprendiz = Convert.ToDecimal(valores[Enumeradores.Parametro.BolsaMinimaAprendiz]);

            if (valorDe.HasValue && valorAte.HasValue)
            {
                switch (objFuncao.IdFuncao)
                {
                    case (5747)://aprendiz
                        if (valorDe.Value < BolsaMinimoAprendiz || valorAte.Value < BolsaMinimoAprendiz)
                        {
                            ExibirMensagem(string.Format("O valor do salário deve ser maior que R$ {0}", BolsaMinimoAprendiz), TipoMensagem.Erro);
                            return null;
                        }
                        break;
                    case (5749)://estagio
                        if (valorDe.Value < BolsaMinimoEstagio || valorAte.Value < BolsaMinimoEstagio)
                        {
                            ExibirMensagem(string.Format("O valor do salário deve ser maior que R$ {0}", BolsaMinimoEstagio), TipoMensagem.Erro);
                            return null;
                        }
                        break;

                    default:
                        if (valorDe.Value < minimoNacional || valorAte.Value < minimoNacional)
                        {
                            ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que o Salário Mínimo Nacional R$ {0}", minimoNacional), TipoMensagem.Erro);
                            return null;
                        }
                        break;
                }

                if (valorDe.Value > valorAte.Value)
                {

                    ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que R$ {0}", valorDe.Value), TipoMensagem.Erro);
                    return null;
                }
            }
            else if (valorDe.HasValue)
            {

                switch (objFuncao.IdFuncao)
                {
                    case (5747)://aprendiz
                        if (valorDe.Value < BolsaMinimoAprendiz)
                        {
                            ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que R$ {0}", BolsaMinimoAprendiz), TipoMensagem.Erro);
                            return null;
                        }
                        break;
                    case (5749)://estagio
                        if (valorDe.Value < BolsaMinimoEstagio)
                        {
                            ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que R$ {0}", BolsaMinimoEstagio), TipoMensagem.Erro);
                            return null;
                        }
                        break;

                    default:
                        if (valorDe.Value < minimoNacional)
                        {
                            ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que o Salário Mínimo Nacional R$ {0}", minimoNacional), TipoMensagem.Erro);
                            return null;
                        }
                        break;
                }
            }

            else if (valorAte.HasValue)
            {

                switch (objFuncao.IdFuncao)
                {
                    case (5747)://aprendiz
                        if (valorAte.Value < BolsaMinimoAprendiz)
                        {
                            ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que R$  {0}", BolsaMinimoAprendiz), TipoMensagem.Erro);
                            return null;
                        }
                        break;
                    case (5749)://estagio
                        if (valorAte.Value < BolsaMinimoEstagio)
                        {
                            ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que R$ {0}", BolsaMinimoEstagio), TipoMensagem.Erro);
                            return null;
                        }
                        break;
                    default:
                        if (valorAte.Value < minimoNacional)
                        {
                            ExibirMensagem(string.Format("Faixa salarial mínima deve ser maior que o Salário Mínimo Nacional R$ {0}", minimoNacional), TipoMensagem.Erro);
                            return null;
                        }
                        break;
                }
            }

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
                FlagInativo = false,
                FlagAuditada = false,
                FlagEmpresaEmAuditoria = empresaEmAuditoria
            };

            var jaAuditada = objVaga.FlagAuditada;

            //Se é uma vaga nova ou é um usuário interno atribui função e a cidade
            if (!IdVaga.HasValue || base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                objVaga.DescricaoFuncao = ucContratoFuncao.FuncaoDesc;
                objVaga.Funcao = objFuncao;
                objVaga.OrigemAnuncioVaga = new OrigemAnuncioVaga((int)BLL.Enumeradores.OrigemAnuncioVaga.AnuncioVaga);

                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidadeAnunciarVaga.Text, out objCidade))
                    objVaga.Cidade = objCidade;
                else
                {
                    objVaga.Cidade = null;
                    ExibirMensagem("Cidade Inexistente.", TipoMensagem.Erro);
                    return null;
                }
            }

            //Bairro da Vaga
            BNE.BLL.DTO.Bairro bairroVaga = RetornarBairro(txtCidadeAnunciarVaga.Text.Split('/')[0], txtBairroAnunciarVaga.Text);

            if (bairroVaga == null)
            {
                objVaga.NomeBairro = txtBairroAnunciarVaga.Text;
            }
            else
            {
                objVaga.IdBairro = bairroVaga.IdBairro;
            }

            if (!rcbEscolaridade.SelectedValue.Equals("0"))
                objVaga.Escolaridade = new Escolaridade(Convert.ToInt32(rcbEscolaridade.SelectedValue));

            if (string.IsNullOrWhiteSpace(txtSalarioDe.Text))
            {
                objVaga.ValorSalarioDe = null;
            }
            else
            {
                objVaga.ValorSalarioDe = Convert.ToDecimal(txtSalarioDe.Text);
            }
            if (string.IsNullOrWhiteSpace(txtSalarioAte.Text))
            {
                objVaga.ValorSalarioPara = null;
            }
            else
            {
                objVaga.ValorSalarioPara = Convert.ToDecimal(txtSalarioAte.Text);
            }
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

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                objVaga.UsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                objVaga.Filial = new Filial(base.IdFilial.Value);

                //Se a vaga não for confidencial
                //Verifica se a filial possui origem
                //Se possuir, sempre salva a vaga para o seu STC
                OrigemFilial objOrigemFilial;
                if (!objVaga.FlagConfidencial && OrigemFilial.CarregarPorFilial(base.IdFilial.Value, out objOrigemFilial))
                {
                    objOrigemFilial.Origem.CompleteObject();
                    objVaga.Origem = objOrigemFilial.Origem;
                }
                else
                {
                    objVaga.Origem = new Origem(base.IdOrigem.Value);
                }
            }
            //task 35422 Quando a vaga é editada por usuario interno do bne não alterar a dta de abertura e prazo
            if (!base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                objVaga.CalcularAberturaEncerramento();
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
                        var tipoResposta = new TipoResposta(Convert.ToInt32(DTPerguntas.Rows[i]["Idf_Tipo_Resposta"]));
                        bool? flagReposta = tipoResposta.IdTipoResposta == (int)Enumeradores.TipoResposta.RespostaObjetiva ? DTPerguntas.Rows[i]["Flg_Resposta"].ToString().Equals("Sim") : (bool?)null;

                        objVagaPergunta = new VagaPergunta
                        {
                            DescricaoVagaPergunta = DTPerguntas.Rows[i]["Des_Vaga_Pergunta"].ToString(),
                            FlagResposta = flagReposta,
                            Vaga = objVaga,
                            TipoResposta = tipoResposta
                        };
                    }

                    listVagaPergunta.Add(objVagaPergunta);
                }
            }
            #endregion

            #region VagaCurso
            var listVagaCurso = new List<VagaCurso>();

            // Somente grava o curso se a vaga é de estágio
            if (ucContratoFuncao.TemEstagio)
                listVagaCurso = BLL.VagaCurso.ResolverVagaCurso(ucContratoFuncao.DescricoesCursos, objVaga);

            #endregion VagaCurso

            #region ValidaçãoDeVagasDeEstágio
            //Vagas que são cadastradas como vínculo estágio (4) devem ter a função "Estagiário"
            if(listVagaTipoVinculo.First().TipoVinculo.IdTipoVinculo == (int)Enumeradores.TipoVinculo.Estágio)
            {
                objVaga.Funcao = new Funcao(5749);
            }
            #endregion

            objVaga.SalvarVaga(listVagaDisponibilidade, listVagaTipoVinculo, listVagaPergunta, listVagaCurso, auditada, (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
         
            IdVaga = objVaga.IdVaga;

            return objVaga;
        }

        private BNE.BLL.DTO.Bairro RetornarBairro(string cidade, string bairro)
        {
            var url = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlAPICEP) + "/api/bairro/getbynomebairro/{cidade}/{nomebairro}".Replace("{cidade}", cidade).Replace("{nomebairro}", bairro);

            using (var client = new HttpClient())
            {
                // Send a request asynchronously continue when complete 
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsAsync<dynamic>().Result;

                    BNE.BLL.DTO.Bairro bairroVaga = JsonConvert.DeserializeObject<BNE.BLL.DTO.Bairro>(responseData);

                    return bairroVaga;
                }
            }

            return null;
        }

        private BNE.BLL.DTO.Bairro RetornarBairro(int idBairro)
        {
            var url = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlAPICEP) + "/api/bairro/getbyidbairro/{idbairro}".Replace("{idbairro}", idBairro.ToString());

            using (var client = new HttpClient())
            {
                // Send a request asynchronously continue when complete 
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsAsync<dynamic>().Result;

                    BNE.BLL.DTO.Bairro bairroVaga = JsonConvert.DeserializeObject<BNE.BLL.DTO.Bairro>(responseData);

                    return bairroVaga;
                }
            }

            return null;
        }

        private bool FuncaoValidaIntegracaoWebEstagios(Funcao objFuncao)
        {
            return Funcao.ValidarFuncaoLimitacaoIntegracaoWebEstagios(objFuncao.DescricaoFuncao);
        }
        #endregion

        #region DeveMostrarModalSucessoFacebook
        private bool DeveMostrarModalSucessoFacebook()
        {
            return Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"].Equals("mostrarSucessoFacebook");
        }
        #endregion

        #region ComprarPublicacaoImediata
        /// <summary>
        /// Fluxo seguido quando um cliente sem plano opta pela divulgação imediata da vaga.
        /// </summary>
        private void ComprarPublicacaoImediata()
        {
            if (base.IdFilial.HasValue && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                int idPlano;
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                var url = new Vaga(this.IdVaga.Value).ComprarPublicacaoImediata(objUsuarioFilialPerfil, out idPlano);
                base.PagamentoIdentificadorPlano.Value = idPlano;

                Redirect(url);
            }
        }
        #endregion

        #region PublicarFacebook
        /// <summary>
        /// Fluxo de publicação no facebook
        /// </summary>
        private void PublicarFacebook()
        {
            if (ckbCompartilharFacebook.Checked)
            {
                var parametros = new List<string>()
                    {
                        BLL.Vaga.MontarUrlVaga(this.IdVaga.Value)
                    };

                ScriptManager.RegisterStartupScript(this, GetType(), "CompartilharFacebook", "javaScript:abrirCompartilhamentoFacebook('" + string.Join("','", parametros) + "');", true);
            }
        }
        #endregion

        #region DesabilitarCamposEdicaoVaga
        private void DesabilitarCamposEdicaoVaga()
        {
            ucContratoFuncao.Enable = false;
            txtCidadeAnunciarVaga.Enabled = false;
            ucContratoFuncao.EnableTipoVinculo = false;

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

        #region ValidarFuncaoCidade
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string ValidarFuncaoCidade(string desFuncao, string siglaEstado)
        {
            desFuncao = desFuncao.Trim();
            siglaEstado = siglaEstado.Trim();
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(desFuncao, out objFuncao))
            {
                Cidade objCidade;
                if (Cidade.CarregarPorNome(siglaEstado, out objCidade))
                {
                    return String.Format("{0};{1}", objFuncao.IdFuncao, objCidade.Estado.SiglaEstado);
                }
            }

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