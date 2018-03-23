using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls.Forms.Mensagens
{
    public partial class CorpoMensagem : BaseUserControl
    {
        #region Propriedades

        #region IdMensagemCS - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdMensagemCS
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

        #region NomeAnexo - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public string NomeAnexo
        {
            get
            {
                return Convert.ToString(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #region ArquivoAnexo - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public byte[] ArquivoAnexo
        {
            get
            {
                return (byte[])(ViewState[Chave.Temporaria.Variavel3.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }
        #endregion

        #region PropriedadeTipoMensagem - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public TipoMensagemSala PropriedadeTipoMensagem
        {
            get
            {
                return (TipoMensagemSala)(ViewState[Chave.Temporaria.Variavel4.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
            }
        }
        #endregion

        #region IdUsuarioFilialPerfil - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdUsuarioFilialPerfil
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

        #region DesPesquisa - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public string DesPesquisa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel6.ToString()].ToString();
                return string.Empty;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        #region PropriedadeMensagemOrigem - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public MensagemSalaOrigem PropriedadeMensagemSalaOrigem
        {
            get
            {
                return (MensagemSalaOrigem)(ViewState[Chave.Temporaria.Variavel7.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Delegates

        public delegate void delegateEventoBotaoVoltar(TipoMensagemSala tipoMensagem, string desPesquisa);
        public event delegateEventoBotaoVoltar EventoBotaoVoltar;

        public delegate void delegateDownloadAnexo(string url);
        public event delegateDownloadAnexo EventoDownloadAnexo;

        #endregion

        #region btnAvancarMensagem_Click

        protected void btnAvancarMensagem_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bti = (ImageButton)sender;

            //Atualiza flag Mensagem Lida
            var objMensagemCS = new MensagemCS();
            objMensagemCS = MensagemCS.LoadObject(Convert.ToInt32(bti.CommandArgument));
            if (objMensagemCS.FlagLido == false)
            {
                objMensagemCS.FlagLido = true;
                objMensagemCS.Save();
            }

            PreencherCampos(Convert.ToInt32(bti.CommandArgument));
        }

        #endregion

        #region btnVoltarMensagem_Click

        protected void btnVoltarMensagem_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bti = (ImageButton)sender;

            //Atualiza flag Mensagem Lida
            var objMensagemCS = new MensagemCS();
            objMensagemCS = MensagemCS.LoadObject(Convert.ToInt32(bti.CommandArgument));
            if (objMensagemCS.FlagLido == false)
            {
                objMensagemCS.FlagLido = true;
                objMensagemCS.Save();
            }

            PreencherCampos(Convert.ToInt32(bti.CommandArgument));
        }

        #endregion

        #region lbAnexo_Click

        protected void lbAnexo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NomeAnexo) && ArquivoAnexo != null)
            {
                string caminhoArquivo = GerarLinkDownload(ArquivoAnexo, NomeAnexo);
                string serverName = Request.ServerVariables["HTTP_HOST"];
                caminhoArquivo = string.Format("http://{0}" + caminhoArquivo.Replace('\\', '/'), serverName);

                if (EventoDownloadAnexo != null)
                    EventoDownloadAnexo(caminhoArquivo);
            }
        }

        #endregion

        #endregion

        #region Metodos

        #region Inicializar

        public void Inicializar()
        {
            if (IdMensagemCS.HasValue && IdUsuarioFilialPerfil.HasValue)
                PreencherCampos(IdMensagemCS.Value);
        }
        #endregion

        #region PreencherCampos

        public void PreencherCampos(int idMensagemCS)
        {
            MensagemCS objMensagemCS;
            lblRemetente.Text = string.Empty;

            if (PropriedadeTipoMensagem.Equals(TipoMensagemSala.MensagensEnviadas))
            {
                objMensagemCS = MensagemCS.LoadObjectAnteriorProximoMensagensEnviadas(idMensagemCS, IdUsuarioFilialPerfil.Value, DesPesquisa);

                Curriculo objCurriculo = Curriculo.LoadObject(objMensagemCS.Curriculo.IdCurriculo);
                lblParaValor.Text = objCurriculo.PessoaFisica.PrimeiroNome + " - Código CV " + objMensagemCS.Curriculo.IdCurriculo;
            }
            else
            {
                objMensagemCS = MensagemCS.LoadObjectAnteriorProximoMensagensRecebidas(idMensagemCS, IdUsuarioFilialPerfil.Value, DesPesquisa);
                lblParaValor.Text = objMensagemCS.DescricaoEmailDestinatario;
            }

            switch (PropriedadeMensagemSalaOrigem)
            {
                case MensagemSalaOrigem.SalaSelecionador:
                    if (objMensagemCS.UsuarioFilialPerfil != null)
                    {
                        objMensagemCS.UsuarioFilialPerfil.CompleteObject();
                        if (objMensagemCS.UsuarioFilialPerfil.Filial != null)
                        {
                            objMensagemCS.UsuarioFilialPerfil.Filial.CompleteObject();
                            lblRemetente.Text = objMensagemCS.UsuarioFilialPerfil.Filial.RazaoSocial;
                        }
                    }
                    break;
                case MensagemSalaOrigem.SalaVip:
                    if (objMensagemCS.UsuarioFilialPerfil != null)
                    {
                        objMensagemCS.UsuarioFilialPerfil.CompleteObject();
                        objMensagemCS.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                        lblRemetente.Text = objMensagemCS.UsuarioFilialPerfil.PessoaFisica.NomePessoa;
                    }
                    break;
            }

            lblAssunto.Text = objMensagemCS.DescricaoEmailAssunto;

            if (objMensagemCS.DescricaoEmailRemetente != null && !objMensagemCS.TipoMensagemCS.IdTipoMensagemCS.Equals(Enumeradores.TipoMensagem.SMS.GetHashCode()))
                lblRemetente.Text += string.Format(" [{0}] ", objMensagemCS.DescricaoEmailRemetente);

            lblDataHorasEnviadaValor.Text = objMensagemCS.DataCadastro.ToShortDateString() + " às " + objMensagemCS.DataCadastro.ToShortTimeString();
        
            lblCorpoMensagem.Text = objMensagemCS.DescricaoMensagem;

            if (objMensagemCS.NomeAnexo != null && objMensagemCS.ArquivoAnexo != null)
            {
                NomeAnexo = objMensagemCS.NomeAnexo;
                ArquivoAnexo = objMensagemCS.ArquivoAnexo;
                lbAnexo.Visible = imgAnexo.Visible = lblAnexo.Visible = true;
                lbAnexo.Text = objMensagemCS.NomeAnexo;
            }
            else
                lbAnexo.Visible = imgAnexo.Visible = lblAnexo.Visible = false;

            if (objMensagemCS.TipoMensagemCS.IdTipoMensagemCS.Equals(Enumeradores.TipoMensagem.SMS.GetHashCode()))
                imgRemetente.ImageUrl = "~/img/icone_sms.png";
            else
                imgRemetente.ImageUrl = "~/img/icn_msg.png";


            if (objMensagemCS.IdMensagemCSAnterior.HasValue)
            {
                btiVoltarMensagem.Visible = true;
                btiVoltarMensagem.CommandArgument = objMensagemCS.IdMensagemCSAnterior.Value.ToString();
            }
            else
                btiVoltarMensagem.Visible = false;

            if (objMensagemCS.IdMensagemProxima.HasValue)
            {
                btiAvancarMensagem.Visible = true;
                btiAvancarMensagem.CommandArgument = objMensagemCS.IdMensagemProxima.Value.ToString();
            }
            else
                btiAvancarMensagem.Visible = false;


            if (EventoBotaoVoltar != null)
                EventoBotaoVoltar(PropriedadeTipoMensagem, DesPesquisa);

            lblTitulo.Text = PropriedadeTipoMensagem.Equals(TipoMensagemSala.MensagensEnviadas) ? "Mensagens Enviadas" : "Mensagens Recebidas";

            upMensagem.Update();
        }

        #endregion

        #region GerarLinkDownload
        /// <summary>
        /// Passa a string para gerar o arquivo para a tela de geração de arquivo (DownloadArquivo.aspx)
        /// </summary>
        /// <param name="strFinal">String de dados que devem estar no arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gerado</param>
        private string GerarLinkDownload(byte[] arrayFinal, string nomeArquivo)
        {
            string caminhoArquivo = String.Format("{0}{1}_{2}_{3}", Resources.Configuracao.PastaArquivoTemporario, DateTime.Now.Ticks, Session.SessionID, nomeArquivo);
            string caminhoNomeArquivo = Server.MapPath(caminhoArquivo);

            FileStream fs = null;
            try
            {
                fs = new FileStream(caminhoNomeArquivo, FileMode.CreateNew, FileAccess.Write);
                fs.Write(arrayFinal, 0, arrayFinal.Length);
                fs.Close();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            return caminhoArquivo;
        }
        #endregion

        #endregion
    }
}