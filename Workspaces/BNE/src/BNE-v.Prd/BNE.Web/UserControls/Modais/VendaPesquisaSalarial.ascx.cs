using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using FormatObject = BNE.BLL.Common.FormatObject;

namespace BNE.Web.UserControls.Modais
{
    public partial class VendaPesquisaSalarial : BaseUserControl
    {

        #region Métodos

        #region Show
        public void Show(Control caller, string nomeEmpresa, string descricaoFuncao)
        {
            revEmail.ValidationExpression = Configuracao.regexEmail;
            PreencherCampos(nomeEmpresa, descricaoFuncao);
            ScriptManager.RegisterStartupScript(caller, caller.GetType(), this.GetType().ToString() + DateTime.Now.Ticks, "javaScript:ModalVendaInformacaoEmpresa.Show();", true);
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos(string nomeEmpresa, string descricaoFuncao)
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                txtNomeEmpresa.Valor = nomeEmpresa;
                txtFuncao.Text = descricaoFuncao;

                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                {
                    txtEmail.Text = objUsuarioFilial.EmailComercial;
                    txtTelefone.DDD = objUsuarioFilial.NumeroDDDComercial;
                    txtTelefone.Fone = objUsuarioFilial.NumeroComercial;
                }
                upTxtEmpresa.Update();
                upTxtEmail.Update();
                upTxtFuncao.Update();
                upTxtTelefone.Update();
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            revEmail.ValidationExpression = Configuracao.regexEmail;
        }
        #endregion

        #region btnEnviar_Click
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                var objFilial = Filial.LoadObject(base.IdFilial.Value);
                var objPessoa = new PessoaFisica(base.IdPessoaFisicaLogada.Value);
                objFilial.Endereco.CompleteObject();
                objFilial.Endereco.Cidade.CompleteObject();
                
                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                {
                    string funcaoPessoa;
                    if (objUsuarioFilial.Funcao != null)
                    {
                        objUsuarioFilial.Funcao.CompleteObject();
                        funcaoPessoa = objUsuarioFilial.Funcao.DescricaoFuncao;
                    }
                    else
                        funcaoPessoa = objUsuarioFilial.DescricaoFuncao;

                    var parametros = new
                    {
                        RazaoSocial  = objFilial.RazaoSocial,
                        NumeroCNPJ = objFilial.CNPJ,
                        NomePessoa = objPessoa.NomeCompleto,
                        FuncaoPessoa = funcaoPessoa,
                        TelefoneComercial = Helper.FormatarTelefone(objFilial.NumeroDDDComercial, objFilial.NumeroComercial),
                        EmailComercial = objUsuarioFilial.EmailComercial,
                        Cidade = Helper.FormatarCidade(objFilial.Endereco.Cidade.NomeCidade, objFilial.Endereco.Cidade.Estado.SiglaEstado),
                        NumeroFuncionarios = objFilial.QuantidadeFuncionarios,
                        EmpresaSolicitada = txtNomeEmpresa.Valor,
                        FuncaoSolicitada  = txtFuncao.Text ,
                        EmailRetorno   = txtEmail.Text ,
                        TelefoneRetorno   = Helper.FormatarTelefone(txtTelefone.DDD, txtTelefone.Fone)
                    };

                    string assunto;
                    string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.VendaPesquisaSalarial, out assunto);
                    string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
                    string emailDestinatario = Parametro.RecuperaValorParametro(Enumeradores.Parametro.VendaPesquisaSalarial);

                    string mensagem = FormatObject.ToString(parametros, template);

                    EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem,Enumeradores.CartaEmail.VendaPesquisaSalarial, emailRemetente, emailDestinatario);
                }
                base.ExibirMensagemConfirmacao("Sucesso!","Solicitação enviada! Em breve entraremos em contato.", false);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Erro ao enviar busca de empresa");
            }
        }
        #endregion

        #endregion

    }
}