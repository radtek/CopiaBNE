using System;
using System.Globalization;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class VerDadosEmpresa : BaseUserControl
    {

        #region Propriedades

        #region FlagConfidencial - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o FlagConfidencial, usado na Vaga. Tem mais importancia que a FlagConfidencial da FilialBNE
        /// </summary>
        public bool? FlagConfidencial
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
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

        #region IdFilial - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        public new int IdFilial
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region btnFechar_Click
        protected void btnFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region lbtVendaVip_Click
        protected void lbtVendaVip_Click(Object sender, EventArgs e)
        {
            Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.AcessoDadosEmpresas));
        }
        #endregion

        #region btnCandidatar_Click
        protected void btnCandidata_Click(object sender, EventArgs e)
        {
            if (Candidatar != null)
                Candidatar();
        }
        #endregion

        #endregion

        #region Metodos

        #region InicializarEmpresaVerDados
        public void InicializarEmpresaVerDados(bool candidatoVIP, Curriculo objCurriculo, Vaga objVaga)
        {
            pnlDadosEmpresaVisualizacao.Visible = true;
            pnlDadosEmpresaConfidencial.Visible = false;

            Filial objFilial = Filial.LoadObject(IdFilial);

            objFilial.Endereco.CompleteObject();
            objFilial.Endereco.Cidade.CompleteObject();

            //Ajustando permissões de vip
            lblNomeEmpresaValor.Visible = candidatoVIP;
            lblTelefoneValor.Visible = candidatoVIP;
            imgLogo.Visible = candidatoVIP;
            lbtEmpresaBloqueada.Visible = !candidatoVIP;
            lbtTelefoneBloqueado.Visible = !candidatoVIP;
            //btiVendaVip.Visible = !candidatoVIP;
            //pnlEsquerdaInfoNaoVip.Visible = !candidatoVIP;
           // pnlEsquerdaInfoVip.Visible = candidatoVIP;
            //pnlEsquerdaIconeConfidencial.Visible = false;

            lblTitulo.Text = MensagemAviso._23009;
            lblAtividadeEmpresa.Visible = false;
            lblAtividadeEmpresaValor.Visible = false;
            lblNumeroFuncionarioValor.Text = objFilial.QuantidadeFuncionarios.ToString();
            lblCidadeValor.Text = String.Format("{0} / {1}", objFilial.Endereco.Cidade.NomeCidade, objFilial.Endereco.Cidade.Estado.SiglaEstado);
            lblBairroValor.Text = objFilial.Endereco.DescricaoBairro;
            lblDataCadastroValor.Text = objFilial.DataCadastro.ToShortDateString();
            lblCurriculosVisualizadosValor.Text = objFilial.RecuperarQuantidadeCurriculosVisualizados().ToString();
            lblVagasDivulgadasValor.Text = objFilial.RecuperarQuantidadeVagasDivuldadas().ToString();

            if (objCurriculo != null)
            {
                var objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(objCurriculo);
                //litValorPlanoVip.Text = new Plano(Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria)).RecuperarValor().ToString("C", CultureInfo.CurrentCulture);
            }

            //Verificando se a visualização é para uma vaga específica
            //utilizado para somente exibir a empresa e telefone da vaga se o tipo da origem for parceiro (não há filial cadastrada)
            Origem origem = null;
            if (objVaga != null)
            {
                if (objVaga.Origem == null)
                {
                    objVaga.CompleteObject();
                }
                origem = objVaga.Origem;
                if (origem.TipoOrigem == null)
                {
                    origem.CompleteObject();
                }
            }

            String nomeEmpresa = objFilial.RazaoSocial;
            String telefone = UIHelper.FormatarTelefone(objFilial.NumeroDDDComercial, objFilial.NumeroComercial);
            if (origem != null && origem.TipoOrigem.IdTipoOrigem == (int)Enumeradores.TipoOrigem.Parceiro)
            {
                nomeEmpresa = objVaga.NomeEmpresa;
                if (String.IsNullOrEmpty(objVaga.NumeroDDD) || String.IsNullOrEmpty(objVaga.NumeroTelefone))
                {
                    telefone = "Não Informado";
                }
                else
                {
                    telefone = UIHelper.FormatarTelefone(objVaga.NumeroDDD, objVaga.NumeroTelefone);
                }

                //escondendo campos que vagas de parceiro não contém
                lblNumeroFuncionario.Visible = false;
                lblNumeroFuncionarioValor.Visible = false;
                lblCidade.Visible = false;
                lblCidadeValor.Visible = false;
                lblBairro.Visible = false;
                lblBairroValor.Visible = false;
                lblDataCadastro.Visible = false;
                lblDataCadastroValor.Visible = false;
                lblCurriculosVisualizados.Visible = false;
                lblCurriculosVisualizadosValor.Visible = false;
                lblVagasDivulgadas.Visible = false;
                lblVagasDivulgadasValor.Visible = false;
            }
            else
            {
                //mostrando todos os campos caso a vaga não seja de parceiro
                lblNumeroFuncionario.Visible = true;
                lblNumeroFuncionarioValor.Visible = true;
                lblCidade.Visible = true;
                lblCidadeValor.Visible = true;
                lblBairro.Visible = true;
                lblBairroValor.Visible = true;
                lblDataCadastro.Visible = true;
                lblDataCadastroValor.Visible = true;
                lblCurriculosVisualizados.Visible = true;
                lblCurriculosVisualizadosValor.Visible = true;
                lblVagasDivulgadas.Visible = true;
                lblVagasDivulgadasValor.Visible = true;
            }

            if (candidatoVIP)
            {
                lblNomeEmpresaValor.Text = nomeEmpresa;
                imgLogo.Cnpj = objFilial.NumeroCNPJ.Value;
                lblTelefoneValor.Text = telefone;
            }
        }
        #endregion

        #region InicializarEmpresaVerDadosConfidencial
        public void InicializarEmpresaVerDadosConfidencial(string titulo, string mensagem)
        {
            //pnlEsquerdaIconeConfidencial.Visible = true;
            pnlDadosEmpresaConfidencial.Visible = true;
            pnlDadosEmpresaVisualizacao.Visible = false;
            lblTitulo.Text = titulo;
            lblMensagem.Text = mensagem;
            imgLogo.Visible = true;
        }
        #endregion

        #region AjustarVisualizacao
        private void AjustarVisualizacao(Vaga objVaga)
        {
            bool candidatoVip = false;

            Curriculo objCurriculo = null;
            if (base.IdCurriculo.HasValue)
            {
                objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
                candidatoVip = objCurriculo.FlagVIP;
            }

            if (FlagConfidencial.HasValue && FlagConfidencial.Value)
                InicializarEmpresaVerDadosConfidencial("Confidencial", MensagemAviso._23010);
            else
                InicializarEmpresaVerDados(candidatoVip, objCurriculo, objVaga); //TODO: Verificar a necessidade de passar o parametro candidatoVip

            if (Candidatar != null) //se a aspx ou ascx que contem o VerDadosEmpresa ter assinado o delegate Candidatar então mostra o botão 
                pnlBotaoCandidatar.Visible = true;

            upVerDadosEmpresa.Update();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal(Vaga objVaga = null)
        {
            LimparCampos();
            AjustarVisualizacao(objVaga);
            mpeVerDadosEmpresa.Show();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeVerDadosEmpresa.Hide();
            if (Fechar != null)
                Fechar();
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            lblNomeEmpresaValor.Text =
            lblTelefoneValor.Text =
            lblAtividadeEmpresaValor.Text =
            lblNumeroFuncionarioValor.Text =
            lblCidadeValor.Text =
            lblBairroValor.Text =
            lblDataCadastroValor.Text =
            lblCurriculosVisualizadosValor.Text =
            lblVagasDivulgadasValor.Text =
            lblTitulo.Text =
            lblMensagem.Text = String.Empty;

            //Ajustar visibilidade
            lblNomeEmpresaValor.Visible = true;
            lblTelefoneValor.Visible = true;
            imgLogo.Visible = false;
            lbtEmpresaBloqueada.Visible = false;
            lbtTelefoneBloqueado.Visible = false;
            //btiVendaVip.Visible = false;
            //pnlEsquerdaInfoNaoVip.Visible = false;
            //pnlEsquerdaInfoVip.Visible = false;
            //pnlEsquerdaIconeConfidencial.Visible = false;

        }
        #endregion

        #endregion

        #region Delegates
        public delegate void fechar();
        public event fechar Fechar;

        public delegate void candidatar();
        public event candidatar Candidatar;
        #endregion

       

    }
}