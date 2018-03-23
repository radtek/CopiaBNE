using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BNE.Curriculo_Pdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblFinal.Text = "";
            pbCurriculos.Value = 0;
            if (!String.IsNullOrEmpty(IdfOrigem.Text))
            {
                BNEEntities db = new BNEEntities();

                int idf = Int32.Parse(IdfOrigem.Text);

                var origem = db.TAB_Origem.Where(p => p.Idf_Origem == idf).FirstOrDefault();

                pdf.generate_pdf_from_html pdf = new Curriculo_Pdf.pdf.generate_pdf_from_html();

                DateTime dt_inicio = new DateTime(dtInicio.Value.Year, dtInicio.Value.Month, dtInicio.Value.Day, 0, 0, 0);
                DateTime dt_fim = new DateTime(DtFim.Value.Year, DtFim.Value.Month, DtFim.Value.Day, 23, 59, 59);

                int idf_cidade;

                if (!Int32.TryParse(IdfOrigem.Text, out idf_cidade))
                {
                    idf_cidade = 0;
                }

                List<int> curriculos;

                if (idf_cidade > 0)
                {
                    curriculos = (
                        from ori in db.BNE_Curriculo_Origem
                        join c in db.BNE_Curriculo on ori.Idf_Curriculo equals c.Idf_Curriculo
                        where ori.Idf_Origem == idf &&
                        (
                            (ori.Dta_Cadastro >= dt_inicio && ori.Dta_Cadastro <= dt_fim)
                            ||
                            (c.Dta_Atualizacao >= dt_inicio && c.Dta_Atualizacao <= dt_fim)
                        ) &&
                        c.Idf_Cidade_Endereco == idf_cidade
                        select ori.Idf_Curriculo
                    ).ToList();
                }
                else
                {
                    curriculos = (
                        from ori in db.BNE_Curriculo_Origem
                        join c in db.BNE_Curriculo on ori.Idf_Curriculo equals c.Idf_Curriculo
                        where ori.Idf_Origem == idf &&
                        (
                            (ori.Dta_Cadastro >= dt_inicio && ori.Dta_Cadastro <= dt_fim)
                            ||
                            (c.Dta_Atualizacao >= dt_inicio && c.Dta_Atualizacao <= dt_fim)
                        )
                        select ori.Idf_Curriculo
                    ).ToList();
                }
                

                //var curriculos = db.BNE_Curriculo_Origem.Where(oc => oc.Idf_Origem == idf && (oc.Dta_Cadastro >= dt_inicio && oc.Dta_Cadastro <= dt_fim)).Select(oc => oc.Idf_Curriculo).ToList();

                int size = curriculos.Count;
                int i = 0;
                foreach (int idf_curriculo in curriculos)
                {
                    i++;

                    pbCurriculos.Value = (int)((i * 100) / size);

                    pbCurriculos.CreateGraphics().DrawString(String.Format("Gerando {0} de {1}", i, size), new Font("Arial",
                        (float)10.25, FontStyle.Bold),
                        Brushes.Black, new PointF(pbCurriculos.Width / 2 - 5, pbCurriculos.Height / 2 - 7));

                    string html = new BNE.BLL.Curriculo(idf_curriculo).RecuperarHTMLCurriculo(true, true, BLL.Enumeradores.FormatoVisualizacaoImpressao.Empresa);

                    var ret = pdf.pdf(html);

                    File.WriteAllBytes("pdf\\" + String.Format("Feira_Estagio_Emprego_{0}_{1}.pdf", origem.Des_Origem.Trim(), idf_curriculo.ToString()), ret);
                }
                lblFinal.Text = "Currículos gerados";
            }
            else
            {
                MessageBox.Show("Origem é obrigatório");
                IdfOrigem.Focus();
            }
        }

        private void IdfOrigem_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(IdfOrigem.Text))
            {
                int idf;

                if(Int32.TryParse(IdfOrigem.Text, out idf)){
                    BNEEntities db = new BNEEntities();

                    var origem = db.TAB_Origem.Where(o => o.Idf_Origem == idf).FirstOrDefault();
                    if(origem != null){
                        lblOrigem.Text = origem.Des_Origem.Trim();
                    }else{
                        MessageBox.Show("Origem não encontrada");
                        IdfOrigem.Text = "";
                        IdfOrigem.Focus();
                    }
                }else{
                    MessageBox.Show("Número inválido");
                    IdfOrigem.Text = "";
                    IdfOrigem.Focus();
                }
            }
        }

        private void txbCidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txbCidade_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txbCidade.Text))
            {
                int idf;

                if (Int32.TryParse(IdfOrigem.Text, out idf))
                {
                    BNEEntities db = new BNEEntities();

                    var cidade = db.TAB_Cidade.Where(o => o.Idf_Cidade == idf).FirstOrDefault();
                    if (cidade != null)
                    {
                        lblCidade.Text = cidade.Nme_Cidade.Trim();
                    }
                    else
                    {
                        MessageBox.Show("Origem não encontrada");
                        IdfOrigem.Text = "";
                        IdfOrigem.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Número inválido");
                    txbCidade.Text = "";
                    txbCidade.Focus();
                }
            }
        }
    }
}
