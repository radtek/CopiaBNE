using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BNE.Console.PessoaFisicaComplemento
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentId = Convert.ToInt32(ConfigurationManager.AppSettings["startfrom"]);
            var maxID = BLL.PessoaFisicaComplemento.ListarMaiorId();
            var top = Convert.ToInt32(ConfigurationManager.AppSettings["top"]);

            try
            {
                while (currentId < maxID)
                {
                    System.Console.WriteLine("CurrentID: " + currentId);
                    var listaPF = BLL.PessoaFisicaComplemento.ListarTodasComAnexo(currentId, top);
                    currentId = listaPF.OrderByDescending(pfc => pfc.PessoaFisica.IdPessoaFisica).Select(pfc => pfc.PessoaFisica.IdPessoaFisica).FirstOrDefault();

                    foreach (var objPessoaFisicaComplemento in listaPF)
                    {
                        try
                        {
                            Attachment.Save(objPessoaFisicaComplemento.PessoaFisica.IdPessoaFisica, objPessoaFisicaComplemento.ArquivoAnexo, objPessoaFisicaComplemento.NomeAnexo);
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("Falha ao salvar o anexo da PF: " + objPessoaFisicaComplemento.PessoaFisica.IdPessoaFisica);
                            System.Console.WriteLine("Message: " + ex.Message);
                            System.Console.WriteLine("StackTrace: " + ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Message: " + ex.Message);
                System.Console.WriteLine("StackTrace: " + ex.StackTrace);
                System.Console.ReadKey();
            }

        }
    }
}
