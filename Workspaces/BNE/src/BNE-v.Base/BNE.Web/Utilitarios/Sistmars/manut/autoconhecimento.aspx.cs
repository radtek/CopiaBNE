using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BNE.Sistmars.BLL;

namespace SistMars.manut
{
	/// <summary>
	/// Summary description for autoconhecimento.
	/// </summary>
	public class frmAutoConhecimento : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtTexto;
		protected System.Web.UI.WebControls.Button cmdGravar;
		protected System.Web.UI.WebControls.DropDownList cboPontuacao;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
				CarregarComboPontuacao();
		}

		/// <summary>
		/// Carrega combo de pontuação
		/// </summary>
		private void CarregarComboPontuacao()
		{
			//seleciona código e descricao da caracteristica do banco
            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_ListarPontuacaoCombo",null).Tables[0]; 

			//adiciona item vazio na combo
			AdicionarItemComboPontuacao("0", "", true);

			for(int i=0;i<dt.Rows.Count;i++)
			{
				//adiciona item na combo
				AdicionarItemComboPontuacao(
					dt.Rows[i]["PON_CodigoPontuacaoTexto"].ToString(),
					dt.Rows[i]["Descricao"].ToString(), false);
			}

			dt.Dispose();
		}

		/// <summary>
		/// Adiciona um item na combo de pontuacao
		/// </summary>
		private void AdicionarItemComboPontuacao(string valor, string texto, bool selecionado)
		{
			ListItem oItem = new ListItem();
			oItem.Value = valor;
			oItem.Text  = texto;
			oItem.Selected = selecionado;
			cboPontuacao.Items.Add(oItem);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.cboPontuacao.SelectedIndexChanged += new System.EventHandler(this.cboPontuacao_SelectedIndexChanged);
			this.cmdGravar.Click += new System.EventHandler(this.cmdGravar_Click);
			this.ID = "frmAutoConhecimento";
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void cboPontuacao_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CarregarTexto();
		}

		/// <summary>
		/// Carrega o texto
		/// </summary>
		private void CarregarTexto()
		{
			if(cboPontuacao.SelectedItem.Value == "0")
			{
				txtTexto.Text = "-| não disponível |-";
				HabilitarControles(false);
			}
			else
			{
				//seleciona código e descricao da caracteristica do banco
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@PON_CodigoPontuacaoTexto", SqlDbType.Int, 4));
                parms[0].Value = cboPontuacao.SelectedItem.Value;
                DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_RetornarPontuacaoTexto",parms).Tables[0];
				txtTexto.Text = dt.Rows[0]["PON_Texto"].ToString();
				HabilitarControles(true);

				dt.Dispose();
			}
		}

		/// <summary>
		/// Habilita ou desabilita os controles
		/// </summary>
		/// <param name="Habilitar">true habilita, false desabilita</param>
		private void HabilitarControles(bool Habilitar)
		{
			txtTexto.Enabled = Habilitar;
			cmdGravar.Enabled = Habilitar;
		}

		private void cmdGravar_Click(object sender, System.EventArgs e)
		{
			GravarDados();
		}

		/// <summary>
		/// Grava os dados no banco
		/// </summary>
		private void GravarDados()
		{
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PON_CodigoPontuacaoTexto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PON_Texto", SqlDbType.Text, 80));
            parms[0].Value = cboPontuacao.SelectedItem.Value;
            parms[1].Value = txtTexto.Text;

			try
			{
                DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_AtualizarPontuacaoTexto", parms);
			}
			catch(Exception ex)
			{
				string body = "Erro ao tentar gravar Pontuação Texto!<br>";
				body += "Descrição do erro:<br>" + ex.Message;
				new util().EnviarEmailErro(body);

				Response.Write(erro.AbrePopupErro("Err2", "Erros", true));
			}
			
		}
	}
}
