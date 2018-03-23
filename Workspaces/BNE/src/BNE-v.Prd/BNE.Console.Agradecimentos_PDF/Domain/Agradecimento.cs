using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Console.Agradecimentos_PDF.Domain
{
    public class Agradecimento
    {
        public static void GerarPDF()
        {
            PDF.generate_pdf_from_html pdf = new BNE.Console.Agradecimentos_PDF.PDF.generate_pdf_from_html();
            
            var page = @"<div class='pagina'><span>Nome: {0}</span><span style='display: {1}'>E-mail: {2}</span><span>Cidade: {3}</span><span>Data: {4}</span><span>Depoimento: {5}</span></div>";

            var html = "<style> span {display: block; margin-bottom: 20px; } div.pagina { page-break-after:always; padding-top: 50px; padding-left:45px;  padding-right: 50px; } </style> ";

            var agradecimentos = BLL.Agradecimento.CarregarAgradecimentosVIP();

            int count = 0;
            foreach (var a in agradecimentos)
            {
                count++;
                System.Console.WriteLine(count + " > " + a.NomePessoa);

                a.Cidade.CompleteObject();
                a.Cidade.Estado.CompleteObject();

                var displayEmail = (string.IsNullOrEmpty(a.EmailPessoa)) ? "none" : "block";

                html += string.Format(page,
                    a.NomePessoa, displayEmail, a.EmailPessoa, a.Cidade.NomeCidade + "/" +  a.Cidade.Estado.SiglaEstado
                    , a.DataCadastro.Value.ToString("dd/MM/yyyy hh:mm:ss"), a.DescricaoMensagem);
            }


            System.Console.WriteLine("Gravando html");
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("saida\\agradecimentos_26_05_2017_OK_VIP.html");
            file.WriteLine(html);
            file.Close();


            //System.Console.WriteLine("Gerando PDF");
            //var ret = pdf.pdf(html);
            //File.WriteAllBytes("saida\\agradecimentos_09_05_2017.pdf", ret);
        } 
    }
}
