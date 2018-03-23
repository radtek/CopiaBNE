using BNE.BLL;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace BNE.Web.UserControls
{
    public partial class IdiomaEmPesquisaCurriculo : BaseUserControl
    {

        #region [ Properties / Events ]

        #region Funcoes - Variavel1

        public DataTable IdiomasSelecionados
        {
            get
            {
                if (!rcbIdioma.SelectedItem.Value.Equals("0"))
                    AdicionarIdiomas(rcbIdioma.SelectedIndex, rcbIdioma.SelectedItem.Text, rcbNivel.SelectedIndex, rcbNivel.SelectedItem.Text);
   
               return Idiomas;
            }
        }
        /// <summary>
        /// Propriedade que armazena e recupera o datatable de funcoes
        /// </summary>
        private DataTable Idiomas
        {
            get
            {
                return (DataTable)ViewState[Chave.Temporaria.PesquisaIdiomas.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PesquisaIdiomas.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region [ Load ]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

        }

     
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void Inicializar()
        {
            if (rcbIdioma.Items.Count < 1 && rcbNivel.Items.Count < 1 && Idiomas ==null  )
            {
                UIHelper.CarregarRadComboBox(rcbIdioma, Idioma.Listar(), new RadComboBoxItem("Qualquer", "0"));
                UIHelper.CarregarRadComboBox(rcbNivel, NivelIdioma.ListarDicionary(), new RadComboBoxItem("Qualquer", "0"));

                InicializarDataTableIdioma();
            }
            

        }

        #endregion

        public void InicializarDataTableIdioma()
        {
            Idiomas = new DataTable();

            Idiomas.Columns.Add(new DataColumn("idIdioma", typeof(Int32)));
            Idiomas.Columns.Add(new DataColumn("DescricaoIdioma", typeof(String)));
            Idiomas.Columns.Add(new DataColumn("idNivel", typeof(Int32)) { 
            AllowDBNull = true });
            Idiomas.Columns.Add(new DataColumn("DescricaoNivel", typeof(String)));
            
        }

        public void LimparCampos()
        {
            Inicializar();
            rcbNivel.SelectedValue =
            rcbIdioma.SelectedValue = "0";
            InicializarDataTableIdioma();
            rptIdioma.DataBind();
        }
      
        public override void Dispose()
        {
            base.Dispose();
        }

        #region AdicionarIdiomas
      
        private void AdicionarIdiomas(int idIdioma, string idioma,int idNivel, string nivel)
        {
            var jaExiste = false;
            foreach (DataRow item in Idiomas.Rows)
            {
                if (item["idIdioma"].ToString() == idIdioma.ToString() && item["idNivel"].ToString() == (idNivel >0 ? idNivel.ToString() : ""))
                    jaExiste = true;
            }
            if (jaExiste) 
                return;
            
            var dr = Idiomas.NewRow();
            dr["idIdioma"] = idIdioma;
            dr["DescricaoIdioma"] = idioma;
            if(idNivel >0)
                dr["idNivel"] = idNivel;
            else 
                dr["idNivel"] = DBNull.Value;
            dr["DescricaoNivel"] = nivel;

            Idiomas.Rows.Add(dr);
        }
        #endregion

        #region CarregarRepeater
        /// <summary>
        /// Bind no repeater
        /// </summary>
        private void CarregarRepeater()
        {
            rptIdioma.DataSource = Idiomas;
            rptIdioma.DataBind();
            upIdiomas.Update();
        }
        #endregion
        
        #region repeater_ItemCommand
        protected void repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            foreach (DataRow row in Idiomas.Rows)
            {
                if ((row["idIdioma"].ToString() + row["idNivel"]) == e.CommandArgument.ToString())
                {
                    row.Delete();
                    break;
                }
            }

            CarregarRepeater();
        }
        #endregion

        #region SetIdiomas

        public void SetIdiomas(List<PesquisaCurriculoIdioma> listaIdiomas)
        {
            Inicializar();
            listaIdiomas.ForEach(x => AdicionarIdiomas(x.Idioma.IdIdioma, rcbIdioma.Items.FindItemByValue(x.Idioma.IdIdioma.ToString()).Text,
                                            x.NivelIdioma != null ? x.NivelIdioma.IdNivelIdioma : 0, rcbNivel.Items.FindItemByValue(x.NivelIdioma != null ? x.NivelIdioma.IdNivelIdioma.ToString() : "0").Text));
            CarregarRepeater();

        }

        public void SetIdiomas(List<RastreadorCurriculoIdioma> listaIdiomas)
        {
            Inicializar();
            listaIdiomas.ForEach(x => AdicionarIdiomas(x.Idioma.IdIdioma, rcbIdioma.Items.FindItemByValue(x.Idioma.IdIdioma.ToString()).Text,
                                            x.NivelIdioma != null ? x.NivelIdioma.IdNivelIdioma : 0, rcbNivel.Items.FindItemByValue(x.NivelIdioma != null ? x.NivelIdioma.IdNivelIdioma.ToString() : "0").Text));
            CarregarRepeater();
         
        }
        
        #endregion

        #region btnAdicionar
        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (rcbIdioma.SelectedItem.Value.Equals("0") && !rcbNivel.SelectedItem.Value.Equals("0"))
                base.ExibirMensagem("Selecionar o idioma para o nível " + rcbNivel.SelectedItem.Text, TipoMensagem.Aviso);
            else if (!rcbIdioma.SelectedItem.Value.Equals("0"))
            {
                AdicionarIdiomas(rcbIdioma.SelectedIndex, rcbIdioma.SelectedItem.Text, rcbNivel.SelectedIndex, rcbNivel.SelectedItem.Text);
                CarregarRepeater();
                rcbIdioma.SelectedValue = "0";
                rcbNivel.SelectedValue = "0";
                upDDL.Update();
            }
        }
        #endregion
     
    }
}