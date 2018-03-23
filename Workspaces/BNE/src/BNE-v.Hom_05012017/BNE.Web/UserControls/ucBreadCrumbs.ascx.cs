using System;
using System.Text;
using BNE.Web.Code;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BNE.Web.Master;
using BNE.BLL;
using Label = System.Web.UI.WebControls.Label;

namespace BNE.Web.UserControls
{
    public partial class ucBreadCrumbs : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Iniciar();
        }

        protected void Iniciar()
        {
            StringBuilder resultado = new StringBuilder();
            
            string dominio = Page.Request.ServerVariables["HTTP_HOST"].ToString().Trim();

            Principal master = (Principal)Page.Master;

            //para não estourar erro na página caso não tenha dados ou se tiver só o diretório da home, não carregar breadcrumb
            if (master.DesBreadcrumb == null || master.DesBreadcrumbURL == null || master.DesBreadcrumbURL.Length <= 2) 
            {
                breadcrumbList.Visible = false;
                navBreadcrumbForms.Attributes.Add("style", "display:none");
                return;
            }

            string[] breadcrumbs = master.DesBreadcrumb.Split('|');
            string[] urlsBreadcrumbs = master.DesBreadcrumbURL.Split('|');

            //Monta os diretórios do breadcrumb
            int qtdDiretorios = breadcrumbs.Length;
            for (int i = 0; i < qtdDiretorios; i++)
            {
                int contador = i + 1;
                if (contador != qtdDiretorios)
                {
                    resultado.Append("<li itemprop='itemListElement' itemscope itemtype='http://schema.org/ListItem'><a itemprop='item' href='http://" + dominio + urlsBreadcrumbs[i].Trim() + "' title='" + breadcrumbs[i].Trim() + 
                        "' onclick=\"trackEvent(\'Master\', \'Click breadcrumb\',\'" + urlsBreadcrumbs[i].Trim() + "\');\"><span itemprop='name'>" + breadcrumbs[i].Trim() + "</span></a><meta itemprop='position' content='" + contador + "'></li>");
                }
                else
                {
                    nmePagina.Text = "<li class='active' itemprop='itemListElement' itemscope itemtype='http://schema.org/ListItem'><a class='ultimoDiretorio' itemprop='item' href='http://" + dominio + urlsBreadcrumbs[i].Trim() + 
                        "'><span itemprop='name'>" + breadcrumbs[i].Trim() + "</span></a><meta itemprop='position' content='" + contador + "'></li>";
                }
            }
            
            if(qtdDiretorios>0){
                string urlImg;
                string nmeVideo;
                string rotavideo = Rota.BuscaVideoPorDesUrl(urlsBreadcrumbs[qtdDiretorios - 1].Trim(), out urlImg, out nmeVideo);
                if (!String.IsNullOrEmpty(rotavideo))
                {
                    var pnl = master.FindControl("pnlVideo");
                    pnl.Visible = true;
                    HtmlIframe iframe = (HtmlIframe)master.FindControl("videoIf");
                    var img = master.FindControl("imgVideo") as HtmlImage;
                    hdfVideo.Value = urlImg;
                    img.Src = urlImg;
                    iframe.Attributes["src"] = rotavideo;
                    Label  lblNomeVideo =(Label)master.FindControl("lblNomeVideo");
                     
                   lblNomeVideo.Text = nmeVideo;
                    //liVideo.Visible = true;
                    //videoIf.Attributes["src"] = rotavideo;
                }
            }
            
             
            links.Text = resultado.ToString();            
        }
        
    }
}