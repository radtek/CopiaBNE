using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Employer.Componentes.UI.Web.Extensions;
using Employer.Componentes.UI.Web.Util;
using Telerik.Web.UI;

namespace Employer.Componentes.UI.Web
{
    public class ComboCheckbox : RadComboBox
    {
        public ComboCheckbox()
        {
            ItemTemplate = new CheckboxItemTemplate(this);
            EmptyMessage = "Selecione...";
            HighlightTemplatedItems = true;
            AllowCustomText = true;
            AutoPostBack = true;
            IsCaseSensitive = false;
            LoadingMessage = Resources.LoadingMessage;
            AutoCompleteSeparator = ",";
        }

        [DisplayName("Delimitador Js")]
        public string Delimiter { get; set; } = ",";

        public bool ShowCheckAllButton { get; set; } = true;

        public string OnClientCheckItem { get; set; }

        public override string CssClass
        {
            get
            {
                if (string.IsNullOrEmpty(base.CssClass))
                {
                    return Keys.Css.EmployerComboCheckbox.ToString();
                }
                return base.CssClass;
            }
            set { base.CssClass = value; }
        }

        public void CheckAllItems()
        {
            foreach (Control control1 in Items)
            {
                var control2 = (CheckBox) control1.FindControl("chk1");
                if (control2 != null)
                {
                    control2.Checked = true;
                }
            }
            UpdateText();
        }

        public Collection<RadComboBoxItem> GetCheckedItems()
        {
            var collection = new Collection<RadComboBoxItem>();
            foreach (RadComboBoxItem radComboBoxItem in Items)
            {
                var control = (CheckBox) radComboBoxItem.FindControl("chk1");
                if (control != null && control.Checked)
                {
                    collection.Add(radComboBoxItem);
                }
            }
            return collection;
        }

        private string GetCheckedTexts()
        {
            if (!AllowCustomText)
            {
                Text = string.Empty;
                return string.Empty;
            }
            var checkedItems = GetCheckedItems();
            var builder = new StringBuilder();
            foreach (var radComboBoxItem in checkedItems)
            {
                builder.AppendComa(radComboBoxItem.Text);
            }
            return builder.ToString();
        }

        public void ClearCheckeds()
        {
            foreach (Control control1 in Items)
            {
                var control2 = (CheckBox) control1.FindControl("chk1");
                if (control2 != null)
                {
                    control2.Checked = false;
                }
            }
            UpdateText();
        }

        private void UpdateText()
        {
            Text = GetCheckedTexts();
        }

        public void InverseSelection()
        {
            foreach (Control control1 in Items)
            {
                var control2 = (CheckBox) control1.FindControl("chk1");
                if (control2 != null)
                {
                    control2.Checked = !control2.Checked;
                }
            }
            UpdateText();
        }

        public void SetItemChecked(RadComboBoxItem item, bool isChecked)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var control = (CheckBox) item.FindControl("chk1");
            if (control != null)
            {
                control.Checked = isChecked;
            }
            UpdateText();
        }

        public void SetItemChecked(string value, bool isChecked)
        {
            SetItemChecked(Items.FindItemByValue(value, true), isChecked);
        }

        private void RegisterJavascript()
        {
            PageResourceManager.GetCurrent(Page).RegisterJavascript("ComboCheckGlobal", Resources.ComboCheckBoxJavascriptGlobal);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "ComboChk" + ClientID, Resources.ComboCheckBoxJavascriptIndividual.Replace("{control_id}", ClientID));
            OnClientDropDownClosed = "onDropDownClosing";
            OnClientDropDownOpened = "onDropDownOpening";
            var control = (CheckBox) Header.FindControl("SelectAll");
            if (control == null)
            {
                return;
            }
            control.Attributes["onclick"] = "SelectAllClick(\"" + ClientID + "\"," + (AllowCustomText ? "true" : "false") + ")";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!AllowCustomText)
            {
                AllowCustomText = true;
                Text = EmptyMessage;
                AllowCustomText = false;
            }
            base.Render(writer);
        }

        protected override void OnInit(EventArgs e)
        {
            if (ShowCheckAllButton)
            {
                HeaderTemplate = new CheckBoxHeaderTemplate();
            }
            RegisterJavascript();
            base.OnInit(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!AllowCustomText)
            {
                Text = string.Empty;
            }
            else
            {
                foreach (Control control1 in Items)
                {
                    var control2 = (CheckBox) control1.FindControl("chk1");
                    if (control2 != null)
                    {
                        control2.Checked = false;
                    }
                }
                var strArray = Text.Replace(", ", ",").Split(',');
                var arrayList = new ArrayList();
                if (strArray.Length > 0)
                {
                    foreach (var text in strArray)
                    {
                        if (string.Join(",", (string[]) arrayList.ToArray(typeof(string))).ToLowerInvariant().IndexOf(text.ToLowerInvariant()) <= -1)
                        {
                            var itemByText = Items.FindItemByText(text, true);
                            if (itemByText != null)
                            {
                                arrayList.Add(itemByText.Text);
                                var control = (CheckBox) itemByText.FindControl("chk1");
                                if (control != null)
                                {
                                    control.Checked = true;
                                }
                            }
                        }
                    }
                    Text = string.Join(",", (string[]) arrayList.ToArray(typeof(string)));
                }
                base.OnTextChanged(e);
            }
        }

        internal class CheckboxItemTemplate : ITemplate, IDisposable
        {
            private HtmlGenericControl div;
            private CheckBox chk;
            private Label lbl;
            private readonly ComboCheckbox parent;

            public CheckboxItemTemplate(ComboCheckbox parent)
            {
                this.parent = parent;
            }

            public void Dispose()
            {
                div.Dispose();
                chk.Dispose();
                lbl.Dispose();
            }

            public void InstantiateIn(Control container)
            {
                div = new HtmlGenericControl("div");
                div.Attributes["onclick"] = "StopPropagation(event)";
                chk = new CheckBox();
                chk.ID = "chk1";
                lbl = new Label();
                lbl.AssociatedControlID = chk.ClientID;
                lbl.DataBinding += lbl_DataBinding;
                div.Controls.Add(chk);
                div.Controls.Add(lbl);
                container.Controls.Add(div);
                lbl.DataBind();
            }

            private void lbl_DataBinding(object sender, EventArgs e)
            {
                var label = sender as Label;
                var bindingContainer = (RadComboBoxItem) label.BindingContainer;
                label.Text = Convert.ToString(DataBinder.Eval(bindingContainer, "Text"));
                var control = (CheckBox) bindingContainer.FindControl("chk1");
                if (control == null)
                {
                    return;
                }
                var str = string.Empty;
                if (!string.IsNullOrEmpty(parent.OnClientCheckItem))
                {
                    str = ";" + parent.OnClientCheckItem + "($get(\"" + control.ClientID + "\"),GetItem(\"" + bindingContainer.ComboBoxParent.ClientID + "\"," + bindingContainer.Index + "))";
                }
                if (parent.AllowCustomText)
                {
                    control.Attributes["onclick"] = "onCheckBoxClick(\"" + bindingContainer.ComboBoxParent.ClientID + "\")" + str;
                }
                else
                {
                    control.Attributes["onclick"] = "onCheckBoxClickDefaultText(\"" + bindingContainer.ComboBoxParent.ClientID + "\" )" + str;
                }
            }
        }

        internal class CheckBoxHeaderTemplate : ITemplate, IDisposable
        {
            private CheckBox chk;
            private Label lbl;

            public void Dispose()
            {
                chk.Dispose();
                lbl.Dispose();
            }

            public void InstantiateIn(Control container)
            {
                chk = new CheckBox();
                chk.ID = "SelectAll";
                lbl = new Label();
                lbl.AssociatedControlID = chk.ClientID;
                lbl.Text = Resources.ComboCheckboxSelectAll;
                container.Controls.Add(chk);
                container.Controls.Add(lbl);
            }
        }
    }
}