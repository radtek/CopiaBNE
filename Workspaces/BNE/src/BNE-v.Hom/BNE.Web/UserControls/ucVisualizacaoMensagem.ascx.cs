using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls
{
    public partial class ucVisualizacaoMensagem : BaseUserControl
    {

        #region IdMensagem - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdMensagem
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region PreencherCampos
        public void PreencherCampos()
        {
            if (IdMensagem.HasValue)
            {
                MensagemCS objMensagemCS = MensagemCS.LoadObject(IdMensagem.Value);
                objMensagemCS.Curriculo.CompleteObject();
                objMensagemCS.Curriculo.PessoaFisica.CompleteObject();
                objMensagemCS.TipoMensagemCS.CompleteObject();
                objMensagemCS.UsuarioFilialPerfil.CompleteObject();
                objMensagemCS.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                lblCodigoValor.Text = objMensagemCS.IdMensagemCS.ToString();
                lblCandidatoValor.Text = objMensagemCS.Curriculo.PessoaFisica.PrimeiroNome;
                lblEnvioValor.Text = objMensagemCS.DataEnvio.Value.ToString("dd/MM/yyyy hh:mm");
                lblTipoValor.Text = CurriculoEntrevista.ExisteCurriculoEntrevista(objMensagemCS) ? "Convite para Entrevista" : "Mensagem";
                lblFormaEnvioValor.Text = objMensagemCS.TipoMensagemCS.DescricaoTipoMensagem;
                lblUsuarioValor.Text = objMensagemCS.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome;
                lblMensagemValor.Text = objMensagemCS.DescricaoMensagem;
            }
            upMensagem.Update();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeModalMensagem.Show();
        }
        #endregion
    }
}