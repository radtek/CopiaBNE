using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BNE.Services.Plugins.PluginsEntrada;
using System.Text.RegularExpressions;


namespace Processos
{
    public class LimpaCampos
    {
        public static void Execute()
            {
                Console.WriteLine("Iniciou processo -> " + DateTime.Now);
                List<Vaga> vagasAtivas = new List<Vaga>();

                vagasAtivas = Vaga.TrazerVagasParaLimparCampo();


                if (vagasAtivas == null || vagasAtivas.Count == 0)
                {
                    Console.WriteLine("Nenhuma vaga encontrada. -> " + DateTime.Now);
                }
                else
                {
                    try
                    {
                        foreach (Vaga vaga in vagasAtivas)
                        {
                            try
                            {
                                using (StreamWriter sw = File.AppendText("C:\\Users\\sonyluz\\Documents\\Sony\\LimpaCampos\\idsBNE22032016.txt"))
                                {
                                    sw.WriteLine("ID Vaga: " + vaga.IdVaga);
                                }

                                //vaga.EmailVaga = Regex.Replace(vaga.EmailVaga, "osanunci.+", "");
                                vaga.DescricaoAtribuicoes = Regex.Replace(vaga.DescricaoAtribuicoes, @"[wW]indow\._zx = window\._zx.+\;", "");
                                vaga.Save(null, BNE.BLL.Enumeradores.VagaLog.LimpaCampos);

                            }
                            catch
                            {
                                Console.WriteLine("Falha para processar vaga ->" + vaga.IdVaga + DateTime.Now);
                            }

                         }
                    }
                    catch
                    {
                        Console.WriteLine("Falha no processo ->" + DateTime.Now);
                    }

                    Console.WriteLine("Fim do processo vagas corrigidas. -> " + DateTime.Now);
                }
            }
    }
}
