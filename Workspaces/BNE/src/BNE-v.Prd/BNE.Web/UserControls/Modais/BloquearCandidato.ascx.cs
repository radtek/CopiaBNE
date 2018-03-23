using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class BloquearCandidato : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region Propriedades

        #region IDCurriculo - IDCurriculo
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IDCurriculo
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

        #region IdFilial - Variavel2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdFilial
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

        #endregion

        #region Delegates
        public delegate void fechar(string Mensagem);
        public event fechar Fechar;

        #endregion

        #region btiFechar_Click

        protected void btiFechar_Click(object sender, EventArgs e)
        {
            mpeBloquearCandidato.Hide();
        }

        #endregion

        #region EsconderModal
        public void EsconderModal()
        {
            mpeBloquearCandidato.Hide();
            upBloquearCandidato.Update();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            tbxMotivo.Text = string.Empty;
            mpeBloquearCandidato.Show();
        }
        #endregion

        #region InicializarBloquear
        public void InicializarBloquear(int? idCurriculo, int? idFilial, string Nome, string Email)
        {
            IDCurriculo = idCurriculo;
            IdFilial = idFilial;


            if (IDCurriculo.HasValue)
            {
                litPerguntaBloquear.Text = "Tem certeza que deseja bloquear o currículo?";
                litTextoNomeCandidato.Text = "Nome do Candidato:";
                pnlBloqueioEmail.Visible = true;
                litEmailCandidato.Text = Email;
                CarregarMotivos(IDCurriculo.Value);
            }
            else
            {
                litPerguntaBloquear.Text = "Tem certeza que deseja bloquear a empresa?";
                litTextoNomeCandidato.Text = "Nome da Empresa:";
            }

            lblNomeCandidato.Text = Nome;
            pnlBloquearCandidatoCentro.Visible = true;
            btnBloquearCandidato.Visible = true;
            btnBloqueadoCandidato.Visible = false;
            pnlPerguntaBloquear.Visible = false;
            
            upBloquearCandidato.Update();
        }
        #endregion

        #region InicializarBloqueado
        public void InicializarBloqueado(int? idCurriculo, int? idFilial, string Nome)
        {
            IDCurriculo = idCurriculo;
            IdFilial = idFilial;

            if (IDCurriculo.HasValue)
            {
                litPerguntaBloquear.Text = "Tem certeza que deseja bloquear o currículo?";
                litTextoNomeCandidato.Text = "Nome do Candidato:";
                CarregarMotivos(IDCurriculo.Value);
            }
            else
            {
                litPerguntaBloquear.Text = "Tem certeza que deseja bloquear a empresa?";
                litTextoNomeCandidato.Text = "Nome da Empresa:";
            }

            lblNomeCandidato.Text = Nome;
            btnBloquearCandidato.Visible = false;
            pnlBloquearCandidatoCentro.Visible = true;
            btnBloqueadoCandidato.Visible = true;
            pnlPerguntaBloquear.Visible = false;

            upBloquearCandidato.Update();
        }
        #endregion

        #region btnBloquearCandidato_Click
        protected void btnBloquearCandidato_Click(object sender, EventArgs e)
        {
            if (tbxMotivo.Text.Trim() == string.Empty)
            {
                ExibirMensagem("Informe o motivo. Campo Obrigatório!", TipoMensagem.Aviso);
                return;
            }
            pnlBloquearCandidatoCentro.Visible = false;
            pnlPerguntaBloquear.Visible = true;
        }
        #endregion

        #region btnDesbloquearCandidato_Click
        protected void btnDesbloquearCandidato_Click(object sender, EventArgs e)
        {
            if (tbxMotivo.Text.Trim() == string.Empty)
            {
                ExibirMensagem("Informe o motivo. Campo Obrigatório!", TipoMensagem.Aviso);
                return;
            }

            if (IDCurriculo.HasValue)
            {
                ExibirMensagem("O Currículo não pode ser desbloqueado. Entre em contato com o suporte.", TipoMensagem.Aviso);
                return;
            }
            else if (IdFilial.HasValue)
            {
                if (Filial.DesbloquearFilial(new Filial(IdFilial.Value), new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), tbxMotivo.Text.Trim()))
                {
                    if (Fechar != null)
                    {
                        tbxMotivo.Text = string.Empty;
                        Fechar("Empresa Desbloqueada com Sucesso!");
                    }
                }
            }

        }
        #endregion


        #region btnSimBloquear_Click
        protected void btnSimBloquear_Click(object sender, EventArgs e)
        {
            if (IDCurriculo.HasValue)
            {
                if (Curriculo.BloquearCurriculo(IDCurriculo.Value, tbxMotivo.Text.Trim(), new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value)))
                {
                    if (Fechar != null)
                    {
                        tbxMotivo.Text = string.Empty;
                        Fechar("Currículo Bloqueado com Sucesso!");
                    }
                }
            }
            else if (IdFilial.HasValue)
            {
                if (Filial.BloquearFilial(new Filial(IdFilial.Value), new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), tbxMotivo.Text.Trim()))
                {
                    if (Fechar != null)
                    {
                        tbxMotivo.Text = string.Empty;
                        Fechar("Empresa Bloqueada com Sucesso!");
                    }
                }
            }

        }
        #endregion

        #region btnNaoBloquear_Click
        protected void btnNaoBloquear_Click(object sender, EventArgs e)
        {
            tbxMotivo.Text = string.Empty;
            this.EsconderModal();
        }
        #endregion

        #region CarregarMotivos
        public void CarregarMotivos(int IDCurriculo)
        {
                UIHelper.CarregarRadGrid(gvBronquinha, Curriculo.LsitarMotivosBronquinha(IDCurriculo));
        }
        #endregion
    }
}