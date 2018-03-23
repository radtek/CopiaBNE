using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.BLL;
using BNE.Web.Master;
using BNE.Web.UserControls;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using Telerik.Web.UI;
using BNE.BLL.Custom;

namespace BNE.Web
{

    public partial class PesquisaVagaAvancada : BasePage
    {
        public ComposedVariable<PesquisaPadrao> PesquisaVagaPadrao = new ComposedVariable<PesquisaPadrao>(Chave.Temporaria.PesquisaPadrao.ToString());

        #region Propriedades
        #region IdPesquisaVaga - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        protected int? IdPesquisaVaga
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

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            Ajax.Utility.RegisterTypeForAjax(typeof(PesquisaVagaAvancada));
        }
        #endregion

        #region btnBuscar_Click
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Buscar();
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnLimpar_Click
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        #endregion

        #region txtCidadePesquisa_TextChanged
        protected void txtCidadePesquisa_TextChanged(object serder, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCidadePesquisa.Text))
                return;

            string[] cidadeEstado = txtCidadePesquisa.Text.Split('/');
            if (cidadeEstado.Length == 2)
            {
                Estado objEstado = Estado.CarregarPorSiglaEstado(cidadeEstado[1]);
                PreencherEstado(objEstado);
            }
            else
            {
                Cidade cidade;
                if (Cidade.CarregarPorNome(txtCidadePesquisa.Text, out cidade))
                {
                    if (cidade.Estado != null)
                    {
                        cidade.Estado.CompleteObject();
                        PreencherEstado(cidade.Estado);
                    }
                }
            }

            upEstado.Update();
        }

        private void PreencherEstado(Estado objEstado)
        {
            rcbEstado.SelectedValue = objEstado.SiglaEstado;
            rcbEstado.Text = rcbEstado.SelectedValue;
        }

        #endregion

        #region txtFuncao_TextChanged
        //protected void txtFuncao_TextChanged(object serder, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(txtFuncao.Text))
        //    {
        //        Funcao objFuncao;
        //        if (Funcao.CarregarPorDescricao(txtFuncao.Text, out objFuncao))
        //        {
        //            objFuncao.AreaBNE.CompleteObject();
        //            rcbAreaBNE.SelectedValue = objFuncao.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
        //            rcbAreaBNE.Enabled = false;

        //            if (base.STC.Value)
        //                aceFuncao.ContextKey = String.Format("{0};{1}", base.IdOrigem.Value.ToString(CultureInfo.CurrentCulture), rcbAreaBNE.SelectedValue);
        //            else
        //                aceFuncao.ContextKey = String.Format("{0};{1}", string.Empty, rcbAreaBNE.SelectedValue);
        //        }
        //        else
        //        {
        //            if (base.STC.Value)
        //                aceFuncao.ContextKey = String.Format("{0};{1}", base.IdOrigem.Value.ToString(CultureInfo.CurrentCulture), string.Empty);
        //            else
        //                aceFuncao.ContextKey = String.Format("{0};{1}", string.Empty, string.Empty);
        //        }
        //    }
        //    else
        //    {
        //        if (base.STC.Value)
        //            aceFuncao.ContextKey = String.Format("{0};{1}", base.IdOrigem.Value.ToString(CultureInfo.CurrentCulture), string.Empty);
        //        else
        //            aceFuncao.ContextKey = String.Format("{0};{1}", string.Empty, string.Empty);

        //        rcbAreaBNE.ClearSelection();
        //        rcbAreaBNE.Enabled = true;
        //    }

        //    upAreaBNE.Update();
        //}
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            //Carrando as drop-down's
            UIHelper.CarregarRadComboBox(rcbEscolaridade, Escolaridade.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbEstado, Estado.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbDisponibilidade, Disponibilidade.Listar());
            UIHelper.CarregarRadComboBox(rcbTipoDeficiencia, Deficiencia.Listar(), new RadComboBoxItem("Indiferente", "-1"));
            ucTipoContratoFuncao.UmCurso = true;
            //var deficiencias = Deficiencia.Listar();
            //var qualquer = deficiencias.FirstOrDefault(a => a.Value == "Qualquer");
            //if (qualquer.Key != null)
            {
                //    deficiencias.Remove(qualquer.Key);
                //    var todasDeficiencias = deficiencias.ToList();

                //    todasDeficiencias.Insert(0, qualquer);
                //    deficiencias = todasDeficiencias.ToDictionary(a => a.Key, a => a.Value);
            }

            //var nenhuma = deficiencias.FirstOrDefault(a => a.Value == "Nenhuma");
            //if (nenhuma.Key != null)
            {
                //    deficiencias.Remove(nenhuma.Key);
            }

            //UIHelper.CarregarRadComboBox(rcbTipoDeficiencia, deficiencias);
            UIHelper.CarregarRadComboBox(rcbAreaBNE, AreaBNE.Listar(), new RadComboBoxItem("Qualquer", "0"));

            txtCidadePesquisa.Attributes["onBlur"] += "txtCidade_onBlur(this);";
            txtCidadePesquisa.Attributes["OnBlur"] += string.Format("setTimeout(\"ValidatorValidate($get('{0}'))\",500);", cvCidade.ClientID);

            txtSalarioAte.MensagemErroIntervalo = String.Format(MensagemAviso._304501, "Máximo", "maior", "Mínimo");

            ucTipoContratoFuncao.AtualizarValidationGroup(this.btnBuscar.ValidationGroup);
            upTipoContratoFuncaoArea.Update();

            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "PesquisaVagaAvancada");

            if (IdPesquisaVaga.HasValue)
                PreencherCampos();
            else
            {
                if (!VerificaRedirecionamento())
                {
                    foreach (var checkItem in ucTipoContratoFuncao.TipoContratoItens)
                    {
                        checkItem.Selected = (checkItem.Text ?? string.Empty).Equals("Efetivo", StringComparison.OrdinalIgnoreCase);
                    }
                    //foreach (var item in ucTipoContratoFuncao.TipoContratoItens)
                    //{
                    //    if (StringComparer.OrdinalIgnoreCase.Equals(item.Text, "Efetivo")
                    //        || StringComparer.OrdinalIgnoreCase.Equals(item.Text, "Estágio"))
                    //        item.Selected = true;
                    //}
                }
            }
        }

        private bool VerificaRedirecionamento()
        {
            if (!PesquisaVagaPadrao.HasValue || PesquisaVagaPadrao.Value == null)
                return false;

            try
            {
                var funcao = Helper.RemoverAcentos((PesquisaVagaPadrao.Value.Funcao ?? string.Empty).Trim());

                Func<ListItem, bool> findCheckBox;
                if (funcao.Equals("Estagio", StringComparison.OrdinalIgnoreCase)
                    || funcao.IndexOf("Estagiari", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    findCheckBox = listItem => Helper.RemoverAcentos((listItem.Text ?? string.Empty).Trim()).Equals("Estagio", StringComparison.OrdinalIgnoreCase);
                    ucTipoContratoFuncao.Focus();
                    ucTipoContratoFuncao.SetFocus(ContratoFuncao.TipoFoco.Funcao);
                }
                else if (funcao.Equals("Aprendiz", StringComparison.OrdinalIgnoreCase))
                {
                    findCheckBox = listItem => listItem.Text.Equals("Aprendiz", StringComparison.OrdinalIgnoreCase);
                    ucTipoContratoFuncao.Focus();
                    ucTipoContratoFuncao.SetFocus(ContratoFuncao.TipoFoco.Funcao);
                }
                else
                {
                    findCheckBox = listItem => false;
                }

                foreach (var checkItem in ucTipoContratoFuncao.TipoContratoItens)
                {
                    checkItem.Selected = findCheckBox(checkItem);
                }

                return true;
            }
            finally
            {
                txtCidadePesquisa.Text = PesquisaVagaPadrao.Value.Cidade;
                txtCidadePesquisa_TextChanged(txtCidadePesquisa, EventArgs.Empty);
                PesquisaVagaPadrao.Clear();
            }
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            ucTipoContratoFuncao.LimparCampos();

            txtCidadePesquisa.Text =
            txtPalavraChave.Valor =
            txtCodigoVaga.Valor =
            txtEmpresa.Valor = String.Empty;

            txtSalarioAte.Valor =
            txtSalario.Valor = null;

            rcbDisponibilidade.ClearCheckeds();
            rcbDisponibilidade.Text = rcbDisponibilidade.EmptyMessage;

            rcbAreaBNE.SelectedValue =
            rcbEscolaridade.SelectedValue =

            rcbTipoDeficiencia.SelectedValue = "-1";
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            BLL.PesquisaVaga objPesquisaVaga = BLL.PesquisaVaga.LoadObject(IdPesquisaVaga.Value);

            if (objPesquisaVaga.Funcao != null)
            {
                objPesquisaVaga.Funcao.CompleteObject();
                ucTipoContratoFuncao.SetFuncao(objPesquisaVaga.Funcao.DescricaoFuncao);

                objPesquisaVaga.Funcao.AreaBNE.CompleteObject();
                rcbAreaBNE.SelectedValue = objPesquisaVaga.Funcao.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                rcbAreaBNE.Enabled = false;
            }

            if (objPesquisaVaga.Cidade != null)
            {
                objPesquisaVaga.Cidade.CompleteObject();
                txtCidadePesquisa.Text = String.Format("{0}/{1}", objPesquisaVaga.Cidade.NomeCidade, objPesquisaVaga.Cidade.Estado.SiglaEstado);
                rcbEstado.SelectedValue = objPesquisaVaga.Cidade.Estado.SiglaEstado;
            }

            txtPalavraChave.Valor = objPesquisaVaga.DescricaoPalavraChave;

            if (objPesquisaVaga.Estado != null)
                rcbEstado.SelectedValue = objPesquisaVaga.Estado.SiglaEstado;

            if (objPesquisaVaga.Escolaridade != null)
                rcbEscolaridade.SelectedValue = objPesquisaVaga.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);

            //Identifica os itens da dropdown de disponibilidade.
            var disponibilidades = PesquisaVagaDisponibilidade.ListarIdentificadores(objPesquisaVaga);
            foreach (int disponibilidade in disponibilidades)
                rcbDisponibilidade.SetItemChecked(disponibilidade.ToString(CultureInfo.CurrentCulture), true);

            var vinculos = PesquisaVagaTipoVinculo.ListarIdentificadores(objPesquisaVaga);
            foreach (var checkItem in ucTipoContratoFuncao.TipoContratoItens)
            {
                checkItem.Selected = vinculos.Contains(Convert.ToInt32(checkItem.Value));
            }

            txtSalario.Valor = objPesquisaVaga.NumeroSalarioMin;
            txtSalarioAte.Valor = objPesquisaVaga.NumeroSalarioMax;
            txtCodigoVaga.Valor = objPesquisaVaga.DescricaoCodVaga;
            txtEmpresa.Valor = objPesquisaVaga.RazaoSocial;

            if (objPesquisaVaga.AreaBNE != null)
                rcbAreaBNE.SelectedValue = objPesquisaVaga.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);

            if (objPesquisaVaga.Deficiencia != null)
                rcbTipoDeficiencia.SelectedValue = objPesquisaVaga.Deficiencia.IdDeficiencia.ToString(CultureInfo.CurrentCulture);
        }
        #endregion

        #region Buscar
        private void Buscar()
        {
            Page.Validate(btnBuscar.ValidationGroup);

            if (!Page.IsValid)
                return;

            var objPesquisaVaga = new BLL.PesquisaVaga();
            var listPesquisaVagaDisponibilidade = new List<PesquisaVagaDisponibilidade>();
            var listPesquisaVagaTipoVinculo = new List<PesquisaVagaTipoVinculo>();

            //task 35150 - se passar o codigo da vaga ignorar os outros filtros.
            if (String.IsNullOrEmpty(txtCodigoVaga.Valor))
            {
                //Identifica os itens selecionado na dropdown de disponibilidade.

                foreach (RadComboBoxItem item in rcbDisponibilidade.GetCheckedItems())
                {
                    var objPesquisaVagaDisponibilidade = new PesquisaVagaDisponibilidade
                    {
                        Disponibilidade = new Disponibilidade(Convert.ToInt32(item.Value))
                    };
                    listPesquisaVagaDisponibilidade.Add(objPesquisaVagaDisponibilidade);
                }

                //Identifica os itens selecionado na dropdown de tipo vinculo.

                foreach (var item in ucTipoContratoFuncao.TipoContratoItens.Where(obj => obj.Selected))
                {
                    var objPesquisaVagaTipoVinculo = new PesquisaVagaTipoVinculo
                    {
                        TipoVinculo = new TipoVinculo(Convert.ToInt32(item.Value))
                    };
                    listPesquisaVagaTipoVinculo.Add(objPesquisaVagaTipoVinculo);
                }

                if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                    objPesquisaVaga.UsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoCandidato.Value);

                if (base.IdCurriculo.HasValue)
                    objPesquisaVaga.Curriculo = new Curriculo(base.IdCurriculo.Value);

                if (!String.IsNullOrEmpty(ucTipoContratoFuncao.FuncaoDesc))
                    objPesquisaVaga.Funcao = Funcao.CarregarPorDescricao(ucTipoContratoFuncao.FuncaoDesc);

                if (!rcbAreaBNE.SelectedValue.Equals("0"))
                    objPesquisaVaga.AreaBNE = new AreaBNE(Convert.ToInt32(rcbAreaBNE.SelectedValue));

                if (!rcbEstado.SelectedValue.Equals("0"))
                    objPesquisaVaga.Estado = new Estado(rcbEstado.SelectedValue);

                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidadePesquisa.Text, out objCidade))
                {
                    objPesquisaVaga.Cidade = objCidade;
                    objPesquisaVaga.Estado = null;
                }

                objPesquisaVaga.DescricaoPalavraChave = txtPalavraChave.Valor;

                if (!rcbEscolaridade.SelectedValue.Equals("0"))
                    objPesquisaVaga.Escolaridade = Escolaridade.LoadObject(Convert.ToInt32(rcbEscolaridade.SelectedValue));

                objPesquisaVaga.NumeroSalarioMin = txtSalario.Valor;
                objPesquisaVaga.NumeroSalarioMax = txtSalarioAte.Valor;

                objPesquisaVaga.RazaoSocial = txtEmpresa.Valor;

                if (!rcbTipoDeficiencia.SelectedValue.Equals("-1"))
                    objPesquisaVaga.Deficiencia = new Deficiencia(Convert.ToInt32(rcbTipoDeficiencia.SelectedValue));

                objPesquisaVaga.FlagPesquisaAvancada = true;
            }
            else
                objPesquisaVaga.DescricaoCodVaga = txtCodigoVaga.Valor;

            PesquisaVagaCurso objPesquisaVagaCurso = null;

            // Somente grava o curso se a vaga é de estágio
            #region [Curso]
            if (ucTipoContratoFuncao.TemEstagio)
            {
                var objCurso = new Curso();
                objPesquisaVagaCurso = new PesquisaVagaCurso();
                if (Curso.CarregarPorNome(ucTipoContratoFuncao.CursoDescricao, out objCurso))
                    objPesquisaVagaCurso.Curso = objCurso;
                else
                    objPesquisaVagaCurso.DescricaoCurso = ucTipoContratoFuncao.CursoDescricao;
            }
            #endregion

            objPesquisaVaga.Salvar(listPesquisaVagaDisponibilidade, listPesquisaVagaTipoVinculo, objPesquisaVagaCurso);

            /*
            TipoGatilho.DispararGatilhoPesquisaCandidato(System.Web.HttpContext.Current ?? Context, objPesquisaVaga);
            var url = string.Concat("http://", Helper.RecuperarURLVagas(), "/resultado-pesquisa-avancada-de-vagas/", objPesquisaVaga.IdPesquisaVaga);
            Redirect(url);
            */

            if (STC.HasValue && STC.Value) //Se for STC mantém o redirecionamento para o projeto antigo
            {
                Session.Add(Chave.Temporaria.Variavel6.ToString(), objPesquisaVaga.IdPesquisaVaga);
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaVaga.ToString(), null));
            }
            else
            {
                var url = string.Concat("http://", Helper.RecuperarURLVagas(), "/resultado-pesquisa-avancada-de-vagas/", objPesquisaVaga.IdPesquisaVaga);
                Redirect(url);
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

        #region RecuperarCidade
        /// <summary>
        /// Recuperar cidade
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RecuperarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return String.Empty;

            Cidade objCidade;
            if (valor.LastIndexOf('/').Equals(-1))
                if (Cidade.CarregarPorNome(valor, out objCidade))
                    return objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado;

            return String.Empty;
        }
        #endregion

        #region ValidarFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            Funcao objFuncao;
            return Funcao.CarregarPorDescricao(valor, out objFuncao);
        }
        #endregion

        #endregion

        protected void ucTipoContratoFuncao_OnFuncaoValida(object sender, EventArgs e)
        {
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(ucTipoContratoFuncao.FuncaoDesc, out objFuncao))
            {
                objFuncao.AreaBNE.CompleteObject();
                rcbAreaBNE.SelectedValue = objFuncao.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                rcbAreaBNE.Enabled = false;
                txtPalavraChave.Focus();
            }
        }

        protected void ucTipoContratoFuncao_OnFuncaoInvalida(object sender, EventArgs e)
        {
            rcbAreaBNE.ClearSelection();
            rcbAreaBNE.Enabled = true;

            ucTipoContratoFuncao.Focus();
            ucTipoContratoFuncao.SetFocus(ContratoFuncao.TipoFoco.Funcao);
        }

        protected void ucTipoContratoFuncao_OnFuncaoReset(object sender, EventArgs e)
        {
            rcbAreaBNE.ClearSelection();
            rcbAreaBNE.Enabled = true;
            rcbAreaBNE.Focus();
        }
    }
}