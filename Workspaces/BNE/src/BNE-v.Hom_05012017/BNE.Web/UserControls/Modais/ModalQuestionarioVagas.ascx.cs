using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalQuestionarioVagas : BaseUserControl
    {

        #region IdVagaSession - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        private int IdVaga
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region IdPergunta1 - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        private int? IdPergunta1
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

        #region IdPergunta2 - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        private int? IdPergunta2
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
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

        #region IdPergunta3 - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        private int? IdPergunta3
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());
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

        #region IdPergunta4 - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        private int? IdPergunta4
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel5.ToString()].ToString());
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

        #region Métodos

        #region Inicializar
        public void Inicializar(int idVaga, int? idCurriculo)
        {
            IdVaga = idVaga;
            PreencherCampos();

            if (idCurriculo.HasValue)
                PreencherRespostas(idVaga, idCurriculo.Value);
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            rbtRespostaSim01.Checked = rbtRespostaNao01.Checked =
            rbtRespostaSim02.Checked = rbtRespostaNao02.Checked =
            rbtRespostaSim03.Checked = rbtRespostaNao03.Checked =
            rbtRespostaSim04.Checked = rbtRespostaNao04.Checked = false;

            txtResposta1.Valor = txtResposta2.Valor = txtResposta3.Valor = txtResposta4.Valor = string.Empty;

            pnlPergunta01.Visible = pnlPergunta02.Visible = pnlPergunta03.Visible = pnlPergunta04.Visible = false;
            IdPergunta1 = IdPergunta2 = IdPergunta3 = IdPergunta4 = null;

            List<VagaPergunta> listaVagaPergunta = VagaPergunta.RecuperarListaPerguntas(this.IdVaga, null);

            if (listaVagaPergunta.Count >= 1) //Caso tenha ao menos uma pergunta
            {
                IdPergunta1 = listaVagaPergunta[0].IdVagaPergunta;
                litPergunta01.Text = listaVagaPergunta[0].DescricaoVagaPergunta;
                pnlPergunta01.Visible = true;
                rbtRespostaSim01.ValidationGroup = rbtRespostaNao01.ValidationGroup = txtResposta1.ValidationGroup = btnEnviar.ValidationGroup;

                var descritiva = listaVagaPergunta[0].TipoResposta.IdTipoResposta == (int)BLL.Enumeradores.TipoResposta.RespostaDescritiva;

                rbtRespostaSim01.Visible = rbtRespostaNao01.Visible = !descritiva;
                txtResposta1.Visible = descritiva;
            }
            if (listaVagaPergunta.Count >= 2)
            {
                IdPergunta2 = listaVagaPergunta[1].IdVagaPergunta;
                litPergunta02.Text = listaVagaPergunta[1].DescricaoVagaPergunta;
                pnlPergunta02.Visible = true;
                rbtRespostaSim02.ValidationGroup = rbtRespostaNao02.ValidationGroup = txtResposta2.ValidationGroup = btnEnviar.ValidationGroup;

                var descritiva = listaVagaPergunta[1].TipoResposta.IdTipoResposta == (int)BLL.Enumeradores.TipoResposta.RespostaDescritiva;

                rbtRespostaSim02.Visible = rbtRespostaNao02.Visible = !descritiva;
                txtResposta2.Visible = descritiva;
            }
            if (listaVagaPergunta.Count >= 3)
            {
                IdPergunta3 = listaVagaPergunta[2].IdVagaPergunta;
                litPergunta03.Text = listaVagaPergunta[2].DescricaoVagaPergunta;
                pnlPergunta03.Visible = true;
                rbtRespostaSim03.ValidationGroup = rbtRespostaNao03.ValidationGroup = txtResposta3.ValidationGroup = btnEnviar.ValidationGroup;

                var descritiva = listaVagaPergunta[2].TipoResposta.IdTipoResposta == (int)BLL.Enumeradores.TipoResposta.RespostaDescritiva;

                rbtRespostaSim03.Visible = rbtRespostaNao03.Visible = !descritiva;
                txtResposta3.Visible = descritiva;
            }
            if (listaVagaPergunta.Count.Equals(4))
            {
                IdPergunta4 = listaVagaPergunta[3].IdVagaPergunta;
                litPergunta04.Text = listaVagaPergunta[3].DescricaoVagaPergunta;
                pnlPergunta04.Visible = true;
                rbtRespostaSim04.ValidationGroup = rbtRespostaNao04.ValidationGroup = txtResposta4.ValidationGroup = btnEnviar.ValidationGroup;

                var descritiva = listaVagaPergunta[3].TipoResposta.IdTipoResposta == (int)BLL.Enumeradores.TipoResposta.RespostaDescritiva;

                rbtRespostaSim04.Visible = rbtRespostaNao04.Visible = !descritiva;
                txtResposta4.Visible = descritiva;
            }

            upPnlPergunta01.Update();
            upPnlPergunta02.Update();
            upPnlPergunta03.Update();
            upPnlPergunta04.Update();
        }
        #endregion

        #region PreencherRespostas
        private void PreencherRespostas(int idVaga, int idCurriculo)
        {
            //Escondendo o botão de enviar caso seja visualização da resposta.
            btnEnviar.Visible = false;

            rbtRespostaSim01.Enabled = rbtRespostaNao01.Enabled =
            rbtRespostaSim02.Enabled = rbtRespostaNao02.Enabled =
            rbtRespostaSim03.Enabled = rbtRespostaNao03.Enabled =
            rbtRespostaSim04.Enabled = rbtRespostaNao04.Enabled = false;

            txtResposta1.Enabled = txtResposta2.Enabled = txtResposta3.Enabled = txtResposta4.Enabled = false;

            List<VagaCandidatoPergunta> listaVagaCandidatoPergunta = VagaCandidatoPergunta.RecuperarListaResposta(idVaga, idCurriculo, null);

            foreach (VagaCandidatoPergunta objVagaCandidatoPergunta in listaVagaCandidatoPergunta)
            {
                if (this.IdPergunta1.HasValue && objVagaCandidatoPergunta.VagaPergunta.IdVagaPergunta.Equals(this.IdPergunta1.Value))
                {
                    if (objVagaCandidatoPergunta.FlagResposta.HasValue)
                    {
                        rbtRespostaSim01.Checked = Convert.ToBoolean(objVagaCandidatoPergunta.FlagResposta);
                        rbtRespostaNao01.Checked = Convert.ToBoolean(!objVagaCandidatoPergunta.FlagResposta);
                    }
                    else
                    {
                        txtResposta1.Valor = objVagaCandidatoPergunta.DescricaoResposta;
                    }
                }
                if (this.IdPergunta2.HasValue && objVagaCandidatoPergunta.VagaPergunta.IdVagaPergunta.Equals(this.IdPergunta2.Value))
                {
                    if (objVagaCandidatoPergunta.FlagResposta.HasValue)
                    {
                        rbtRespostaSim02.Checked = Convert.ToBoolean(objVagaCandidatoPergunta.FlagResposta);
                        rbtRespostaNao02.Checked = Convert.ToBoolean(!objVagaCandidatoPergunta.FlagResposta);
                    }
                    else
                    {
                        txtResposta2.Valor = objVagaCandidatoPergunta.DescricaoResposta;
                    }
                }
                if (this.IdPergunta3.HasValue && objVagaCandidatoPergunta.VagaPergunta.IdVagaPergunta.Equals(this.IdPergunta3.Value))
                {
                    if (objVagaCandidatoPergunta.FlagResposta.HasValue)
                    {
                        rbtRespostaSim03.Checked = Convert.ToBoolean(objVagaCandidatoPergunta.FlagResposta);
                        rbtRespostaNao03.Checked = Convert.ToBoolean(!objVagaCandidatoPergunta.FlagResposta);
                    }
                    else
                    {
                        txtResposta3.Valor = objVagaCandidatoPergunta.DescricaoResposta;
                    }
                }
                if (this.IdPergunta4.HasValue && objVagaCandidatoPergunta.VagaPergunta.IdVagaPergunta.Equals(this.IdPergunta4.Value))
                {
                    if (objVagaCandidatoPergunta.FlagResposta.HasValue)
                    {
                        rbtRespostaSim04.Checked = Convert.ToBoolean(objVagaCandidatoPergunta.FlagResposta);
                        rbtRespostaNao04.Checked = Convert.ToBoolean(!objVagaCandidatoPergunta.FlagResposta);
                    }
                    else
                    {
                        txtResposta4.Valor = objVagaCandidatoPergunta.DescricaoResposta;
                    }
                }
            }

            upPnlPergunta01.Update();
            upPnlPergunta02.Update();
            upPnlPergunta03.Update();
            upPnlPergunta04.Update();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeModalQuestionarioVagas.Show();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeModalQuestionarioVagas.Hide();
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region btnEnviar_Click
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if ((!IdPergunta1.HasValue || (rbtRespostaSim01.Checked || rbtRespostaNao01.Checked || !string.IsNullOrWhiteSpace(txtResposta1.Valor)))
                && (!IdPergunta2.HasValue || (rbtRespostaSim02.Checked || rbtRespostaNao02.Checked || !string.IsNullOrWhiteSpace(txtResposta2.Valor)))
                && (!IdPergunta3.HasValue || (rbtRespostaSim03.Checked || rbtRespostaNao03.Checked || !string.IsNullOrWhiteSpace(txtResposta3.Valor)))
                && (!IdPergunta4.HasValue || (rbtRespostaSim04.Checked || rbtRespostaNao04.Checked || !string.IsNullOrWhiteSpace(txtResposta4.Valor))))
            {
                VagaRespostaEventArgs eventArgs = new VagaRespostaEventArgs(this.IdVaga, IdPergunta1, IdPergunta2, IdPergunta3, IdPergunta4, rbtRespostaSim01.Checked, rbtRespostaSim02.Checked, rbtRespostaSim03.Checked, rbtRespostaSim04.Checked, txtResposta1.Valor, txtResposta2.Valor, txtResposta3.Valor, txtResposta4.Valor);
                if (Salvar != null)
                    Salvar(sender, eventArgs);
            }
            else
                base.ExibirMensagem("Selecione uma resposta para a pergunta!", TipoMensagem.Erro);
        }
        #endregion

        #endregion

        #region delegateSalvar
        public delegate void delegateSalvar(object sender, VagaRespostaEventArgs e);
        public event delegateSalvar Salvar;
        #endregion

    }

    public class VagaRespostaEventArgs : EventArgs
    {
        public int IdVaga { get; private set; }
        public int? IdPergunta1 { get; private set; }
        public int? IdPergunta2 { get; private set; }
        public int? IdPergunta3 { get; private set; }
        public int? IdPergunta4 { get; private set; }
        public bool? FlagRespostaPergunta1 { get; private set; }
        public bool? FlagRespostaPergunta2 { get; private set; }
        public bool? FlagRespostaPergunta3 { get; private set; }
        public bool? FlagRespostaPergunta4 { get; private set; }
        public string RespostaPergunta1 { get; private set; }
        public string RespostaPergunta2 { get; private set; }
        public string RespostaPergunta3 { get; private set; }
        public string RespostaPergunta4 { get; private set; }

        public VagaRespostaEventArgs(int idVaga, int? idPergunta1, int? idPergunta2, int? idPergunta3, int? idPergunta4, bool? flagRespostaPergunta1, bool? flagRespostaPergunta2, bool? flagRespostaPergunta3, bool? flagRespostaPergunta4, string respostaPergunta1, string respostaPergunta2, string respostaPergunta3, string respostaPergunta4)
        {
            this.IdVaga = idVaga;
            this.IdPergunta1 = idPergunta1;
            this.IdPergunta2 = idPergunta2;
            this.IdPergunta3 = idPergunta3;
            this.IdPergunta4 = idPergunta4;
            this.FlagRespostaPergunta1 = flagRespostaPergunta1;
            this.FlagRespostaPergunta2 = flagRespostaPergunta2;
            this.FlagRespostaPergunta3 = flagRespostaPergunta3;
            this.FlagRespostaPergunta4 = flagRespostaPergunta4;
            this.RespostaPergunta1 = respostaPergunta1;
            this.RespostaPergunta2 = respostaPergunta2;
            this.RespostaPergunta3 = respostaPergunta3;
            this.RespostaPergunta4 = respostaPergunta4;
        }
    }
}