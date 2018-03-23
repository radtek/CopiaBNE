using BNE.Console.Agradecimentos_PDF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Console.Agradecimentos_PDF
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Console.WriteLine("Iniciando a geração do PDF");


            Agradecimento.GerarPDF();



            System.Console.WriteLine("PDF gerado");
            System.Console.ReadKey();
        }
    }
}
