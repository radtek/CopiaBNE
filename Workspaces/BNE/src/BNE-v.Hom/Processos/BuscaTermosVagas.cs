using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BNE.Services.Plugins.PluginsEntrada;

namespace Processos
{
    public class BuscaTermosVagas
    {
        public static void Execute()
        {
            Console.WriteLine("Iniciou processo -> " + DateTime.Now);
            while(true)
            {
                try
                {
                    List<Vaga> lstVagasProcessamento = Vaga.RecuperaVagasAtivas();
                    
                    foreach(var vaga in lstVagasProcessamento)
                    {
                        PublicacaoVaga.ValidarTermosVaga(vaga);
                    }


                    Console.WriteLine("Fim do processo de 10.000 vagas. -> " + DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha no processo ->" + DateTime.Now + " Erro: " + ex.Message);
                }

            }
        }
    }
}
