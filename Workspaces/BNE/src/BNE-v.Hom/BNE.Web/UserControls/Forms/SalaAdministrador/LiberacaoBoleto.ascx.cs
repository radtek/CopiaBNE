using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Data;
using System.Data.SqlClient;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class LiberacaoBoleto : BaseUserControl
    {

        #region Propriedades

        #region InformacaoPagamento
        /// <summary>
        /// Armazena a informação dos pagamentos pesquisados
        /// </summary>
        protected Pagamento.InformacaoPagamento InformacaoPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return (Pagamento.InformacaoPagamento)(ViewState[Chave.Temporaria.Variavel1.ToString()]);

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region DicPagamentos - Variavel 2
        private Dictionary<int, bool> DicPagamentos
        {
            get
            {
                return (Dictionary<int, bool>)(ViewState[Chave.Temporaria.Variavel2.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel2.ToString()] = value;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            if (!IsPostBack)
                Inicializar();
        }
        #endregion

        #region btPesquisar_Click
        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbPF.Checked)
                {
                    if (!string.IsNullOrEmpty(rcbPlano.SelectedValue) && rcbPlano.SelectedValue != "0")
                        InformacaoPagamento = BLL.Pagamento.PesquisarPagamentos(Convert.ToDateTime(txtDataArquivo.Text), Enumeradores.PlanoTipo.PessoaFisica, new Plano(Convert.ToInt32(rcbPlano.SelectedValue)));
                    else
                        InformacaoPagamento = BLL.Pagamento.PesquisarPagamentos(Convert.ToDateTime(txtDataArquivo.Text), Enumeradores.PlanoTipo.PessoaFisica);
                }
                else if (rbPJ.Checked)
                {
                    if (!string.IsNullOrEmpty(rcbPlano.SelectedValue) && rcbPlano.SelectedValue != "0")
                        InformacaoPagamento = BLL.Pagamento.PesquisarPagamentos(Convert.ToDateTime(txtDataArquivo.Text), Enumeradores.PlanoTipo.PessoaJuridica, new Plano(Convert.ToInt32(rcbPlano.SelectedValue)));
                    else
                        InformacaoPagamento = BLL.Pagamento.PesquisarPagamentos(Convert.ToDateTime(txtDataArquivo.Text), Enumeradores.PlanoTipo.PessoaJuridica);
                }
                else
                {
                    if (!string.IsNullOrEmpty(rcbPlano.SelectedValue) && rcbPlano.SelectedValue != "0")
                        InformacaoPagamento = BLL.Pagamento.PesquisarPagamentos(Convert.ToDateTime(txtDataArquivo.Text), new Plano(Convert.ToInt32(rcbPlano.SelectedValue)));
                    else
                        InformacaoPagamento = BLL.Pagamento.PesquisarPagamentos(Convert.ToDateTime(txtDataArquivo.Text));
                }

                gvLiberacaoBoletos.DataSource = InformacaoPagamento.ToList();
                gvLiberacaoBoletos.DataBind();

                pnlBotoes.Visible = true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region btAdicionarArquivo_Click

        protected void btAdicionarArquivo_Click(object sender, EventArgs e)
        {
            mpeModalAnexarArquivo.Show();
        }

        #endregion

        #region gvLiberacaoBoletos_NeedDataSource
        protected void gvLiberacaoBoletos_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if (InformacaoPagamento != null)
                gvLiberacaoBoletos.DataSource = InformacaoPagamento.ToList();
        }
        #endregion

        #region gvLiberacaoBoletos_ItemCommand
        protected void gvLiberacaoBoletos_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("MaisInformacoes"))
            {
                int idPagamento = Convert.ToInt32(gvLiberacaoBoletos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["IdPagamento"]);

                string desPlano, nomePessoa, numCPF, desIdentificador, numNotaFiscal, situacaoPg, infoUm, infoDois, infoTres, infoQuatro, infoCinco, infoSeis, infoSete, infoOito;

                if (BLL.Pagamento.RecuperarInfoPagamentoPF(idPagamento, out desPlano, out nomePessoa, out numCPF, out desIdentificador, out numNotaFiscal, out situacaoPg)) ;
                {
                    txtTipoPlanoInfo.Text = desPlano;
                    txtNome.Text = nomePessoa;
                    txtCPF.Text = numCPF;
                    txtNumBoleto.Text = desIdentificador;
                    txtNF.Text = numNotaFiscal;
                    txtSituacao.Text = situacaoPg;
                }

                BLL.PlanoParcela.RecuperarInformacoesNF(idPagamento, out  infoUm, out infoDois, out infoTres, out infoQuatro, out infoCinco, out infoSeis, out infoSete, out infoOito);

                lblInfoUm.Text = infoUm;
                lblInfoDois.Text = infoDois;
                lblInfoTres.Text = infoTres;
                lblInfoQuatro.Text = infoQuatro;
                lblInfoCinco.Text = infoCinco;
                lblInfoSeis.Text = infoSeis;
                lblInfoSete.Text = infoSete;
                lblInfoOito.Text = infoOito;

                upMaisInformacoes.Update();

                mpeModalMaisInformacoes.Show();
            }

            if (e.CommandName.Equals("EmitirNF"))
            {
                int idPagamento = Convert.ToInt32(gvLiberacaoBoletos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["IdPagamento"]);

                var objPagamento = BLL.Pagamento.LoadObject(idPagamento);

                if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value))
                    ExibirMensagem("Nota Fiscal Emitida com sucesso !", TipoMensagem.Aviso);
                else
                    ExibirMensagem("Erro ao emitir Nota Fiscal", TipoMensagem.Aviso);
            }
        }
        #endregion

        #region cbHeaderItem_CheckedChanged
        protected void cbHeaderItem_CheckedChanged(object sender, EventArgs e)
        {
            var cbHeaderItem = (CheckBox)sender;

            foreach (GridDataItem gdi in gvLiberacaoBoletos.Items)
            {
                int idPagamento = Convert.ToInt32(gvLiberacaoBoletos.MasterTableView.DataKeyValues[gdi.ItemIndex]["IdPagamento"].ToString());
                var cbDataItem = (CheckBox)gdi.FindControl("cbDataItem");
                DicPagamentos[idPagamento] = cbDataItem.Checked = cbHeaderItem.Checked;
            }

            upAnexo.Update();
        }
        #endregion

        #region cbDataItem_CheckedChanged
        protected void cbDataItem_CheckedChanged(object sender, EventArgs e)
        {
            var cbDataItem = (CheckBox)sender;
            DicPagamentos[Convert.ToInt32(cbDataItem.Attributes["CommandArgument"])] = cbDataItem.Checked;
        }
        #endregion

        #region btEmitirTodasNF_Click
        protected void btEmitirTodasNF_Click(object sender, EventArgs e)
        {
            List<int> listaPagamentos = new List<int>();
            List<int> listaNFEmitidas = new List<int>();
            List<int> listaNFNaoEmitidas = new List<int>();

            if (DicPagamentos.Count > 0)
            {
                listaPagamentos = DicPagamentos.Where(d => d.Value).Select(d => d.Key).ToList();

                foreach (int idfPagamento in listaPagamentos)
                {
                    var objPagamento = BLL.Pagamento.LoadObject(idfPagamento);

                    if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value))
                        listaNFEmitidas.Add(idfPagamento);
                    else
                        listaNFNaoEmitidas.Add(idfPagamento);
                }

                string mensagemAviso = string.Format("{0} Pagamentos Enviados. {1} notas emitidas, {2} notas não emitidas", listaPagamentos.Count.ToString(), listaNFEmitidas.Count.ToString(), listaNFNaoEmitidas.Count.ToString());

                ExibirMensagem(mensagemAviso, TipoMensagem.Aviso);
            }
        }
        #endregion

        #region gvLiberacaoBoletos_ItemDataBound
        protected void gvLiberacaoBoletos_ItemDataBound(object sender, GridItemEventArgs e)
        {
            DicPagamentos = new Dictionary<int, bool>();

            if (e.Item is GridDataItem)
            {
                string id = gvLiberacaoBoletos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["IdPagamento"].ToString();

                var cbDtaItem = (CheckBox)e.Item.FindControl("cbDataItem");
                cbDtaItem.Attributes.Add("CommandArgument", id);

                if (DicPagamentos.ContainsKey(Convert.ToInt32(id)))
                    cbDtaItem.Checked = DicPagamentos[Convert.ToInt32(id)];
            }
        }
        #endregion

        #region rcbPlano_SelectedIndexChanged
        protected void rcbPlano_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(rcbPlano.SelectedValue))
            {

            }
        }
        #endregion

        #region rbPF_CheckedChanged

        protected void rbPF_CheckedChanged(object sender, EventArgs e)
        {
            UIHelper.CarregarRadComboBox(rcbPlano, Plano.ListarPorTipo(Enumeradores.PlanoTipo.PessoaFisica), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));

            upAnexo.Update();
        }

        #endregion

        #region rbPJ_CheckedChanged
        protected void rbPJ_CheckedChanged(object sender, EventArgs e)
        {
            UIHelper.CarregarRadComboBox(rcbPlano, Plano.ListarPorTipo(Enumeradores.PlanoTipo.PessoaJuridica), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));

            upAnexo.Update();
        }
        #endregion

        #endregion

        #region Eventos Modal Adicionar Arquivo CNR

        #region btEnviar_Click
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            if (fAnexo.HasFile)
            {
                try
                {
                    MemoryStream ms = new MemoryStream(fAnexo.FileBytes);
                    Arquivo arq = Arquivo.ReceberArquivo(ms, fAnexo.FileName, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);

                    Pagamento objPagamento = null;
                    InformacaoPagamento = new Pagamento.InformacaoPagamento();
                    foreach (LinhaArquivo linha in arq.Linhas)
                    {
                        string falha;
                        if ((linha.BoletoBancario == null && linha.Transacao == null) || linha.Falha(out falha)) continue;

                        Pagamento.InformacaoPagamento.InformacaoPagamentoBoleto infoBol = new Pagamento.InformacaoPagamento.InformacaoPagamentoBoleto();
                        if (linha.BoletoBancario != null)
                        {
                            objPagamento = linha.BoletoBancario.Pagamento;
                            infoBol.Nome = linha.BoletoBancario.SacadoNome;
                            infoBol.NumeroBoleto = linha.BoletoBancario.NumeroNossoNumero;
                            infoBol.NumeroDocumento = Convert.ToDecimal(linha.BoletoBancario.SacadoNumCNPJCPF);
                        }
                        else if (linha.Transacao != null)
                        {
                            objPagamento = linha.Transacao.Pagamento;
                            infoBol.Nome = linha.Transacao.NomeTitularContaCorrenteDebito;
                            infoBol.NumeroBoleto = string.Empty;
                            infoBol.NumeroDocumento = linha.Transacao.NumeroCPFTitularContaCorrenteDebito.HasValue ? linha.Transacao.NumeroCPFTitularContaCorrenteDebito.Value : linha.Transacao.NumeroCNPJTitularContaCorrenteDebito.Value;
                        }
                        else
                        { continue; }

                        if (objPagamento == null)
                        {
                            continue;
                        }
                        objPagamento.CompleteObject();
                        infoBol.IdPagamento = objPagamento.IdPagamento;
                        infoBol.NotaFiscal = objPagamento.NumeroNotaFiscal;

                        if (objPagamento.PlanoParcela != null)
                        {
                            objPagamento.PlanoParcela.CompleteObject();
                            infoBol.Parcela = objPagamento.PlanoParcela.NumeroParcela().ToString();
                        }
                            
                        else
                            infoBol.Parcela = "1";

                        PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);

                        if (String.IsNullOrEmpty(objPlanoAdquirido.Plano.DescricaoPlano))
                            objPlanoAdquirido.Plano.CompleteObject();
                        infoBol.Plano = objPlanoAdquirido.Plano.DescricaoPlano;

                        if(objPagamento.TipoPagamento != null)
                        {
                            objPagamento.TipoPagamento.CompleteObject();
                            infoBol.TipoPagamento = objPagamento.TipoPagamento.DescricaoTipoPagamaneto;
                        }            
                        if (objPagamento.PlanoParcela != null)
                            infoBol.ValorPlano = objPagamento.PlanoParcela.ValorParcela;
                        else
                            infoBol.ValorPlano = objPagamento.ValorPagamento;

                        if (objPlanoAdquirido.Plano.PlanoTipo == null)
                            objPlanoAdquirido.Plano.CompleteObject();
                        if (objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                        {
                            InformacaoPagamento.PF.Add(infoBol);
                        }
                        else
                        {
                            //RETIRADA VAGA PREMIUM TASK: 41857
                            //Task 36029 - empresa que comprou plano, suas vagas deixam de ser premium
                            // BLL.ParametroVaga.ApagarParametroVagaPremium(objPlanoAdquirido.Filial.IdFilial);
                             InformacaoPagamento.PJ.Add(infoBol);
                        }
                    }

                    gvLiberacaoBoletos.DataSource = InformacaoPagamento.ToList();
                    gvLiberacaoBoletos.DataBind();

                    pnlBotoes.Visible = true;

                    upLiberacaoBoleto.Update();

                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    ExibirMensagemErro(ex, "Ocorreu um erro na leitura do arquivo CNR");
                }
            }
        }
        #endregion

        #region btVoltar_Click
        protected void btVoltar_Click(object sender, EventArgs e)
        {
            mpeModalAnexarArquivo.Hide();
        }
        #endregion

        #region btFechar_Click
        protected void btFechar_Click(object sender, ImageClickEventArgs e)
        {
            mpeModalAnexarArquivo.Hide();
        }
        #endregion

        #endregion

        #region Eventos Modal Mais Informacoes

        #region btFecharMaisInformacoes_Click
        protected void btFecharMaisInformacoes_Click(object sender, ImageClickEventArgs e)
        {
            mpeModalMaisInformacoes.Hide();
        }
        #endregion

        #region btVoltarMaisInfo_Click

        protected void btVoltarMaisInfo_Click(object sender, EventArgs e)
        {
            mpeModalMaisInformacoes.Hide();
        }

        #endregion

        #endregion

        #region Metodos
        private void Inicializar()
        {
            if (rbPF.Checked)
                UIHelper.CarregarRadComboBox(rcbPlano, Plano.ListarPorTipo(Enumeradores.PlanoTipo.PessoaFisica), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));
            else
                if (rbPJ.Checked)
                    UIHelper.CarregarRadComboBox(rcbPlano, Plano.ListarPorTipo(Enumeradores.PlanoTipo.PessoaJuridica), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));
                else
                    UIHelper.CarregarRadComboBox(rcbPlano, Plano.Listar(true), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));

            if (DicPagamentos != null && DicPagamentos.Count > 0)
                pnlBotoes.Visible = true;
            else
                pnlBotoes.Visible = false;
        }
        #endregion
        
    }
}