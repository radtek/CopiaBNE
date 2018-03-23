using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls
{
    public partial class UcValidacaoCelular : BaseUserControl
    {

        #region NumeroDDDCelular - Variavel 1
        public string NumeroDDDCelular
        {
            get
            {
                return ViewState[Chave.Temporaria.Variavel1.ToString()].ToString();
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel1.ToString()] = value;
            }
        }
        #endregion NumeroDDDCelular - Variavel 1

        #region NumeroCelular - Variavel 2
        public string NumeroCelular
        {
            get
            {
                return ViewState[Chave.Temporaria.Variavel2.ToString()].ToString();
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel2.ToString()] = value;
            }
        }
        #endregion NumeroDDDCelular - Variavel 2

        #region Codigo
        public string Codigo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(txtCodigoValidacao.Valor))
                    return string.Empty;

                return txtCodigoValidacao.Valor.Replace(" ", string.Empty);
            }
        }
        #endregion Codigo

        #region btlEnviarNovoCodigo_Click
        protected void btlEnviarNovoCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                EL.GerenciadorException.GravarExcecao(new Exception("UcValidaaoCelular >> btlEnviarNovoCodigo_Click NUM: " + NumeroCelular));
                OnBeforeVerificarEnviarCodigoCelular();

                PessoaFisica.ValidacaoCelularEnviarCodigo(NumeroDDDCelular, NumeroCelular);

                OnCodigoEnviado();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion btlEnviarNovoCodigo_Click

        #region txtCodigoValidacao_OnValorAlterado
        protected void txtCodigoValidacao_OnValorAlterado(object sender, EventArgs e)
        {
            try
            {
                OnBeforeVerificarEnviarCodigoCelular();

                if (PessoaFisica.ValidacaoCelularValidarCodigo(NumeroDDDCelular, NumeroCelular, txtCodigoValidacao.Valor))
                {
                    OnCodigoValido();
                    imgCodigoValido.Visible = true;
                }
                else
                    imgCodigoValido.Visible = false;
            }
            catch (Exception ex)
            {
                imgCodigoValido.Visible = false;
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion txtCodigoValidacao_OnValorAlterado

        #region Delegates
        public event EventHandler CodigoValido;
        public event EventHandler BeforeVerificarEnviarCodigoCelular;
        public event EventHandler CodigoEnviado;
        protected virtual void OnCodigoValido()
        {
            EventHandler handler = CodigoValido;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        protected virtual void OnBeforeVerificarEnviarCodigoCelular()
        {
            EventHandler handler = BeforeVerificarEnviarCodigoCelular;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        protected virtual void OnCodigoEnviado()
        {
            EventHandler handler = CodigoEnviado;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion Delegates

    }
}