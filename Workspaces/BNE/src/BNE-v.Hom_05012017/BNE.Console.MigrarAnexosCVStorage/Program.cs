using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Console.MigrarAnexosCVStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderName = "curriculos";
            int idPessoa = 0;
            string nomeArquivo = string.Empty;
            byte[] arquivoAnexo = null;
            int contador = 0;

            try
            {
                System.Console.WriteLine("Vai buscar os anexos");

                while(contador <= 68)
                {
                    IDataReader drAnexos = BLL.PessoaFisicaComplemento.CarregarTodosAnexos();

                    System.Console.WriteLine("Retornou os anexos");

                    while(drAnexos.Read())
                    {
                        idPessoa = (int)drAnexos[0];
                        nomeArquivo = drAnexos[2].ToString();
                        arquivoAnexo = (byte[])drAnexos[3];

                        try
                        {
                            System.Console.WriteLine(string.Format("Salvando o arquivo {0}",nomeArquivo));

                            BLL.StorageManager.SalvarArquivo(folderName, drAnexos[1] + "_" + nomeArquivo, arquivoAnexo);

                            //atualizar tabela de migração
                            BLL.PessoaFisicaComplemento.AtualizarTabelaMigracaoAnexo(idPessoa, nomeArquivo);
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex, string.Format("Erro ao salvar anexo {0} para Storage BNE, pessoa: {1}", nomeArquivo, idPessoa));
                        }
                    }

                    contador++;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, string.Format("Erro no processo de migração para Storage BNE, pessoa: {1}", nomeArquivo ,idPessoa));
            }
        }
    }
}
