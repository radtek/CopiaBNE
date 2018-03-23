namespace BNE.Web.UserControls
{
    public partial class ucCurso : System.Web.UI.UserControl
    {
        public string ImageURLMiniatura { set { hlImagemCurso.ImageUrl = value; } }
        public string TituloCurso { set { litTitulo.Text = value; } }
        public string CargaHoraria { set { litCargaHoraria.Text = value; } }
        public string Valor { set { litValor.Text = value; } }
        public bool MostrarParcela { set { pnlParcela.Visible = value; } }
        public string QuantidadeParcela { set { litQuantidadeParcela.Text = value; } }
        public string ValorParcela { set { liValorParcela.Text = value; } }
        public string IdCurso { set { btiDetalhes.CommandArgument = value; } }
    }
}