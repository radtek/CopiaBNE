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
            //if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            //    e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblFinal.Text = "";
            pbCurriculos.Value = 0;
            //if (!String.IsNullOrEmpty(IdfOrigem.Text))
            //{
                //BNEEntities db = new BNEEntities();

                //int idf = Int32.Parse(IdfOrigem.Text);

                //var origem = db.TAB_Origem.Where(p => p.Idf_Origem == idf).FirstOrDefault();

                pdf.generate_pdf_from_html pdf = new Curriculo_Pdf.pdf.generate_pdf_from_html();

                //DateTime dt_inicio = new DateTime(dtInicio.Value.Year, dtInicio.Value.Month, dtInicio.Value.Day, 0, 0, 0);
                //DateTime dt_fim = new DateTime(DtFim.Value.Year, DtFim.Value.Month, DtFim.Value.Day, 23, 59, 59);

                //int idf_cidade;

                //if (!Int32.TryParse(txbCidade.Text, out idf_cidade))
                //{
                //    idf_cidade = 0;
                //}

                List<int> curriculos = new List<int>()
                {
                   706217   ,
10357881    ,
710548  ,
1493233 ,
4723180 ,
12245877    ,
704704  ,
705688  ,
742055  ,
764690  ,
1534650 ,
1865539 ,
7261244 ,
12292816    ,
436648  ,
488955  ,
2334367 ,
767663  ,
1383264 ,
1541907 ,
1685409 ,
6690709 ,
7295069 ,
8012657 ,
8130809 ,
10919469    ,
12246056    ,
12309533    ,
12310726    ,
12396302    ,
39400   ,
169759  ,
704678  ,
704880  ,
705843  ,
707658  ,
715923  ,
722402  ,
724180  ,
725118  ,
742556  ,
752183  ,
753425  ,
759450  ,
761588  ,
763944  ,
766026  ,
1081171 ,
1421199 ,
1459221 ,
1461403 ,
1926134 ,
2014317 ,
2846616 ,
3094649 ,
4082497 ,
4272900 ,
4273265 ,
4965980 ,
6073925 ,
6694108 ,
12275021    ,
87449   ,
383633  ,
707085  ,
725301  ,
740703  ,
745069  ,
748147  ,
757531  ,
767198  ,
1617450 ,
1911698 ,
2180947 ,
12259410    

                };


              


             
                int size = curriculos.Count;
                int i = 0;
                foreach (int idf_curriculo in curriculos)
                {
                    i++;

                    pbCurriculos.Value = (int)((i * 100) / size);

                    pbCurriculos.CreateGraphics().DrawString(String.Format("Gerando {0} de {1}", i, size), new Font("Arial",
                        (float)10.25, FontStyle.Bold),
                        Brushes.Black, new PointF(pbCurriculos.Width / 2 - 5, pbCurriculos.Height / 2 - 7));


                    var cv = new BNE.BLL.Curriculo(idf_curriculo);
                    cv.CompleteObject();
                cv.PessoaFisica.CompleteObject();
                    string html = cv.RecuperarHTMLCurriculo(true, true, BLL.Enumeradores.FormatoVisualizacaoImpressao.Empresa);


                    var ret = pdf.pdf(html);
                var nome = cv.PessoaFisica.NomeCompleto.Replace(" ", "_").ToUpper();
                File.WriteAllBytes("pdf\\" + String.Format("CV_{0}_{1}.pdf", nome, idf_curriculo.ToString()), ret);
                }
                lblFinal.Text = "Currículos gerados";
            //}
            //else
            //{
            //    MessageBox.Show("Origem é obrigatório");
            //    IdfOrigem.Focus();
            //}
        }

        private void IdfOrigem_Leave(object sender, EventArgs e)
        {
            //if (!String.IsNullOrEmpty(IdfOrigem.Text))
            //{
            //    int idf;

            //    if(Int32.TryParse(IdfOrigem.Text, out idf)){
            //        BNEEntities db = new BNEEntities();

            //        var origem = db.TAB_Origem.Where(o => o.Idf_Origem == idf).FirstOrDefault();
            //        if(origem != null){
            //            lblOrigem.Text = origem.Des_Origem.Trim();
            //        }else{
            //            MessageBox.Show("Origem não encontrada");
            //            IdfOrigem.Text = "";
            //            IdfOrigem.Focus();
            //        }
            //    }else{
            //        MessageBox.Show("Número inválido");
            //        IdfOrigem.Text = "";
            //        IdfOrigem.Focus();
            //    }
            //}
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

                if (Int32.TryParse(txbCidade.Text, out idf))
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
