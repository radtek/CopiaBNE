using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.UI;
using Employer.Componentes.UI.Web.Extensions;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Navegador por abas
    /// </summary>
    public class TabNavigator : MultiView
    {
        #region TabNavigatorLinkButton
        /// <summary>
        /// Link button usado nas abas do TabNavigator
        /// </summary>
        internal class TabNavigatorLinkButton : LinkButton
        {
            #region ViewIndex
            /// <summary>
            /// O índice da view no TabNavigator
            /// </summary>
            public int ViewIndex
            {
                get
                {
                    if (this.ViewState["ViewIndex"] == null)
                        return -1;
                    return Convert.ToInt32(this.ViewState["ViewIndex"]);
                }
                set
                {
                    this.ViewState["ViewIndex"] = value;
                }
            }
            #endregion
        }
        #endregion

        #region Privates
        private Collection<TabNavigatorLinkButton> colBotoes = new Collection<TabNavigatorLinkButton>();
        #endregion

        #region Properties

        #region AbaCssClass
        /// <summary>
        /// Classe css da aba
        /// </summary>
        public string AbaCssClass
        {
            get { return Convert.ToString(this.ViewState["AbaCssClass"]); }
            set { this.ViewState["AbaCssClass"] = value; }
        }
        #endregion

        #region BordaEsquerdaCssClass
        /// <summary>
        /// Classe css da borda esquerda das abas
        /// </summary>
        public string BordaEsquerdaCssClass
        {
            get { return Convert.ToString(this.ViewState["BordaEsquerdaCssClass"]); }
            set { this.ViewState["BordaEsquerdaCssClass"] = value; }
        }
        #endregion

        #region BordaEsquerdaSelecionadaCssClass
        /// <summary>
        /// Classe css da borda esquerda da aba selecionada
        /// </summary>
        public string BordaEsquerdaSelecionadaCssClass
        {
            get { return Convert.ToString(this.ViewState["BordaEsquerdaSelecionadaCssClass"]); }
            set { this.ViewState["BordaEsquerdaSelecionadaCssClass"] = value; }
        }
        #endregion

        #region MeioAbaCssClass
        /// <summary>
        /// Classe css do meio das abas
        /// </summary>
        public string MeioAbaCssClass
        {
            get { return Convert.ToString(this.ViewState["MeioAbaCssClass"]); }
            set { this.ViewState["MeioAbaCssClass"] = value; }
        }
        #endregion

        #region MeioAbaCssSelecionadaClass
        /// <summary>
        /// Classe css do meio da aba selecionada
        /// </summary>
        public string MeioAbaCssSelecionadaClass
        {
            get { return Convert.ToString(this.ViewState["MeioAbaCssSelecionadaClass"]); }
            set { this.ViewState["MeioAbaCssSelecionadaClass"] = value; }
        }
        #endregion

        #region BordaDireitaSelecionadaCssClass
        /// <summary>
        /// Classe css da borda direita da aba selecionada
        /// </summary>
        public string BordaDireitaSelecionadaCssClass
        {
            get { return Convert.ToString(this.ViewState["BordaDireitaSelecionadaCssClass"]); }
            set { this.ViewState["BordaDireitaSelecionadaCssClass"] = value; }
        }
        #endregion

        #region BordaDireitaCssClass
        /// <summary>
        /// Classe css da borda direita das abas
        /// </summary>
        public string BordaDireitaCssClass
        {
            get { return Convert.ToString(this.ViewState["BordaDireitaCssClass"]); }
            set { this.ViewState["BordaDireitaCssClass"] = value; }
        }
        #endregion

        #region BordaEsquerdaPrimeiraSelecionadaCssClass
        /// <summary>
        /// Classe css da borda esquerda da primeira aba quando selecionada
        /// </summary>
        public string BordaEsquerdaPrimeiraSelecionadaCssClass
        {
            get { return Convert.ToString(this.ViewState["BordaEsquerdaPrimeiraSelecionadaCssClass"]); }
            set { this.ViewState["BordaEsquerdaPrimeiraSelecionadaCssClass"] = value; }
        }
        #endregion

        #region BordaEsquerdaPrimeiraCssClass
        /// <summary>
        /// Classe css da borda esquerda da primeira aba
        /// </summary>
        public string BordaEsquerdaPrimeiraCssClass
        {
            get { return Convert.ToString(this.ViewState["BordaEsquerdaPrimeiraCssClass"]); }
            set { this.ViewState["BordaEsquerdaPrimeiraCssClass"] = value; }
        }
        #endregion

        #region BordaDireitaUltimaSelecionadaCssClass
        /// <summary>
        /// Classe css da borda direita da última aba quando selecionada
        /// </summary>
        public string BordaDireitaUltimaSelecionadaCssClass
        {
            get { return Convert.ToString(this.ViewState["BordaDireitaUltimaSelecionadaCssClass"]); }
            set { this.ViewState["BordaDireitaUltimaSelecionadaCssClass"] = value; }
        }
        #endregion

        #region BordaDireitaUltimaCssClass
        /// <summary>
        /// Classe css da borda direita da última aba
        /// </summary>
        public string BordaDireitaUltimaCssClass
        {
            get { return Convert.ToString(this.ViewState["BordaDireitaUltimaCssClass"]); }
            set { this.ViewState["BordaDireitaUltimaCssClass"] = value; }
        }
        #endregion

        #region ConteudoCssClass
        /// <summary>
        /// Classe css do consteúdo
        /// </summary>
        public String ConteudoCssClass
        {
            get { return Convert.ToString(this.ViewState["ConteudoCssClass"]); }
            set { this.ViewState["ConteudoCssClass"] = value; }
        }
        #endregion

        #region ActiveViewIndex
        /// <inheritdoc />
        public override int ActiveViewIndex
        {
            get
            {
                return base.ActiveViewIndex;
            }
            set
            {
                base.ActiveViewIndex = value;
            }
        }
        #endregion

        #region ShowNext
        /// <summary>
        /// Mostra a próxima aba
        /// </summary>
        public void ShowNext()
        {
            for (int i = this.ActiveViewIndex + 1; i < this.Views.Count; i++)
            {
                TabView v = (this.Views[i] as TabView);
                if (v.ShowButton && v.Enabled)
                {
                    this.ActiveViewIndex = i;
                    return;
                }
            }

            for (int i = 0; i < this.Views.Count; i++)
            {
                TabView v = (this.Views[i] as TabView);
                if (v.ShowButton && v.Enabled)
                {
                    this.ActiveViewIndex = i;
                    return;
                }
            }
        }
        #endregion

        #region ShowPrevious
        /// <summary>
        /// Mostra a aba anterior
        /// </summary>
        public void ShowPrevious()
        {
            for (int i = this.ActiveViewIndex - 1; i >= 0; i--)
            {
                TabView v = (this.Views[i] as TabView);
                if (v.ShowButton && v.Enabled)
                {
                    this.ActiveViewIndex = i;
                    return;
                }
            }

            for (int i = this.Views.Count - 1; i >= 0; i--)
            {
                TabView v = (this.Views[i] as TabView);
                if (v.ShowButton && v.Enabled)
                {
                    this.ActiveViewIndex = i;
                    return;
                }

            }
        }
        #endregion

        #region SelectedButtonIndex
        /// <summary>
        /// Índice do botão selecionado
        /// </summary>
        private int SelectedButtonIndex
        {
            get
            {
                for (int i = 0; i < colBotoes.Count; i++)
                {
                    if (colBotoes[i].ViewIndex == this.ActiveViewIndex)
                        return i;
                }
                return -1;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region CreateChildControls
        /// <inheritdoc />
        #pragma warning disable 618
        protected override void CreateChildControls()
        {
            for (int i = 0; i < this.Views.Count; i++)
            {
                View v = this.Views[i];

                if (v is TabView)
                {
                    if (!(v as TabView).ShowButton)
                        continue;

                    TabNavigatorLinkButton lb = new TabNavigatorLinkButton();
                    lb.Text = (v as TabView).Titulo;
                    lb.Enabled = (v as TabView).Enabled;
                    lb.ViewIndex = i;
                    colBotoes.Add(lb);

                    lb.OnClientClick = this.Page.GetPostBackEventReference(this, Convert.ToString(colBotoes.Count - 1));
                }
            }
            base.CreateChildControls();
        }
        #endregion

        #region DisableAllTabs
        /// <summary>
        /// Desabilita todas as abas
        /// </summary>
        public void DisableAllTabs()
        {
            for (int i = 0; i < this.Views.Count; i++)
            {
                View v = this.Views[i];

                if (v is TabView)
                {
                    (v as TabView).Enabled = false;
                }
            }
        }
        #endregion

        #region EnableAllTabs
        /// <summary>
        /// Habilita todas as abas
        /// </summary>
        public void EnableAllTabs()
        {
            for (int i = 0; i < this.Views.Count; i++)
            {
                View v = this.Views[i];

                if (v is TabView)
                {
                    (v as TabView).Enabled = true;
                }
            }
        }
        #endregion

        #region EnableTab
        /// <summary>
        /// Habilita a aba parametrizada
        /// </summary>
        /// <param name="index">O índice da aba que se deseja ativar</param>
        public void EnableTab(int index)
        {
            View v = this.Views[index];
            if (v is TabView)
            {
                (v as TabView).Enabled = true;
            }
        }
        #endregion

        #region DisableTab
        /// <summary>
        /// Desabilita a aba parametrizada
        /// </summary>
        /// <param name="index">O índice da aba que se deseja ativar</param>
        public void DisableTab(int index)
        {
            View v = this.Views[index];
            if (v is TabView)
            {
                (v as TabView).Enabled = false;
            }
        }
        #endregion

        #region ShowTab
        /// <summary>
        /// Exibe a aba parametrizada
        /// </summary>
        /// <param name="index">O índice da aba que se deseja exibir</param>
        public void ShowTab(int index)
        {
            View v = this.Views[index];
            if (v is TabView)
            {
                (v as TabView).ShowButton = true;
            }
        }
        #endregion

        #region HideTab
        /// <summary>
        /// Oculta a aba parametrizada
        /// </summary>
        /// <param name="index">O índice da aba que se deseja ocultar</param>
        public void HideTab(int index)
        {
            View v = this.Views[index];
            if (v is TabView)
            {
                (v as TabView).ShowButton = false;
            }
        }
        #endregion

        #region ShowAllTabs
        /// <summary>
        /// Exibe todas as abas
        /// </summary>
        public void ShowAllTabs()
        {
            for (int i = 0; i < this.Views.Count; i++)
            {
                View v = this.Views[i];

                if (v is TabView)
                {
                    (v as TabView).ShowButton = true;
                }
            }
        }
        #endregion

        #region HideAllTabs
        /// <summary>
        /// Oculta todas as abas
        /// </summary>
        public void HideAllTabs()
        {
            for (int i = 0; i < this.Views.Count; i++)
            {
                View v = this.Views[i];

                if (v is TabView)
                {
                    (v as TabView).ShowButton = false;
                }
            }
        }
        #endregion

        #region RenderControl
        /// <inheritdoc />
        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.Write("<div id=" + this.ClientID + ">");
            writer.WriteTagWithClass("ul", this.AbaCssClass);
            for (int i = 0; i < this.colBotoes.Count; i++)
            {
                LinkButton lkb = this.colBotoes[i];
                writer.Write("<li>");

                // borda da esquerda

                if (i == 0)
                {
                    if (i == this.SelectedButtonIndex)
                        writer.WriteSpanWithClass(this.BordaEsquerdaPrimeiraSelecionadaCssClass);
                    else
                        writer.WriteSpanWithClass(this.BordaEsquerdaPrimeiraCssClass);
                    writer.WriteEndTag("span");
                }
                else
                {
                    if (i == this.SelectedButtonIndex)
                        writer.WriteSpanWithClass(this.BordaEsquerdaPrimeiraSelecionadaCssClass);
                    else
                        if (i == this.ActiveViewIndex + 1)
                            writer.WriteSpanWithClass(this.BordaEsquerdaSelecionadaCssClass);
                        else
                            writer.WriteSpanWithClass(this.BordaEsquerdaCssClass);
                    writer.WriteEndTag("span");
                }

                // Parte central da aba
                if (i == this.SelectedButtonIndex)
                    writer.WriteSpanWithClass(this.MeioAbaCssSelecionadaClass);
                else
                    writer.WriteSpanWithClass(this.MeioAbaCssClass);
                lkb.RenderControl(writer);
                writer.WriteEndTag("span");

                // borda da direita
                if (i == this.colBotoes.Count - 1)
                {
                    if (i == this.SelectedButtonIndex)
                        writer.WriteSpanWithClass(this.BordaDireitaUltimaSelecionadaCssClass);
                    else
                        writer.WriteSpanWithClass(this.BordaDireitaUltimaCssClass);
                }
                else
                {
                    if (i == this.SelectedButtonIndex)
                        writer.WriteSpanWithClass(this.BordaDireitaSelecionadaCssClass);
                    else
                        writer.WriteSpanWithClass(this.BordaDireitaCssClass);
                }
                writer.WriteEndTag("span");

                writer.WriteEndTag("li");
            }
            writer.WriteEndTag("ul");

            // Conteúdo do controle
            if (String.IsNullOrEmpty(this.ConteudoCssClass))
                writer.Write("<div>");
            else
                writer.Write("<div class=\"" + this.ConteudoCssClass + "\" >");
            base.RenderControl(writer);
            writer.WriteEndTag("div");
            writer.WriteEndTag("div");
        }
        #endregion
        #endregion

        #region Events

        #region lb_Click
        /// <summary>
        /// Evento disparado quando o usuário clica em uma das abas
        /// </summary>
        /// <param name="sender">O objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void lb_Click(object sender, EventArgs e)
        {
            this.ActiveViewIndex = (sender as TabNavigatorLinkButton).ViewIndex;
        }
        #endregion

        #region OnPreRender
        /// <inheritdoc />
        protected override void OnPreRender(EventArgs e)
        {
            string eventArgument = Page.Request.Params["__EVENTTARGET"];

            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(this.UniqueID))
            {
                lb_Click(this.colBotoes[Convert.ToInt32(Page.Request.Params["__EVENTARGUMENT"])], null);
            }
            base.OnPreRender(e);
        }
        #endregion
        #endregion
    }
}
