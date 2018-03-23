using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.BLL;
using BNE.BLL.Integracoes.Nexcore;
using BNE.EL;

namespace BNE.Console.ConsultaOperadoraCelular
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsultarOperadora.C();

            /*
             *  drFuncao["IdFuncao"] = row["Idf_Funcao"];
                drFuncao["DescFuncao"] = row["Des_Funcao"];
                drFuncao["Novo"] = false;
                if (!Convert.ToBoolean(row["Flg_Inativo"]))
                {
                    drFuncao["class"] = "liActive";
                    drFuncao["Ativa"] = true;
                }
                else
                {
                    drFuncao["class"] = "liActive liEsconder";
                    drFuncao["Ativa"] = false;
                }
                drFuncao["Similar"] = Convert.ToBoolean(row["Flg_Similar"]);
                Funcoes.Rows.Add(drFuncao);
             * */

        }



    }

    public class ConsultarOperadora
    {

        public ConsultarOperadora()
        {
            HttpClient = new HttpClientOperadoraCelular();
        }

        public static void C()
        {
            var obj = new ConsultarOperadora();

            dynamic ddd21 = new ExpandoObject();
            ddd21.NumeroDDD = Convert.ToInt32(21);
            ddd21.NoveDigitos = true;

            dynamic ddd22 = new ExpandoObject();
            ddd22.NumeroDDD = Convert.ToInt32(22);
            ddd22.NoveDigitos = true;

            var listaDDD = new List<dynamic>
            {
                ddd21,
                //ddd22
            };

            var listaPesquisados = CelularLog.JaPesquisado();
            //var listaPesquisados = new List<string>();
            //lote = 9999999;
            foreach (dynamic ddd in listaDDD)
            {
                int numeroDDD = ddd.NumeroDDD;

                int w = 0;

                var dataTable = new DataTable();
                dataTable.Columns.Add("DDD");
                dataTable.Columns.Add("Numero");
                dataTable.Columns.Add("Idf_Operadora_Celular");

                for (int i = 0; i < 9999999; i++)
                {

                    var telefone = GerarNumero();
                    
                    /*
                    System.Console.WriteLine(telefone);
                    w++;
                    if (w == 20)
                        System.Console.ReadKey();
                    */

                    if (!listaPesquisados.Contains(numeroDDD + telefone))
                    {
                        var dr = dataTable.NewRow();
                        dr["DDD"] = numeroDDD;
                        dr["Numero"] = telefone;
                        dr["Idf_Operadora_Celular"] = 0;
                        dataTable.Rows.Add(dr);
                        listaPesquisados.Add(numeroDDD + telefone);
                        w++;

                        if (w == 200)
                        {
                            var stopWatchProcess = new System.Diagnostics.Stopwatch();
                            stopWatchProcess.Start();
                            obj.ProcessarDataTable(dataTable, numeroDDD);
                            dataTable.Rows.Clear();
                            stopWatchProcess.Stop();
                            System.Console.WriteLine("Para processar " + w + " telefones levou " + stopWatchProcess.Elapsed);
                            w = 0;
                        }
                    }
                }
            }
            System.Console.WriteLine("Digite algo!");
            System.Console.ReadKey();
        }

        private const int MaximaQtdeCelularesPorRequest = 42;

        private static Random random;
        private static object syncObj = new object();
        private static void InitRandomNumber(int seed)
        {
            random = new Random(seed);
        }
        private static string GerarNumero()
        {
            var telefone = "99";
            lock (syncObj)
            {
                if (random == null)
                    random = new Random();
                return telefone + random.Next(0, 9999999).ToString().PadLeft(7, '0');
            }
        }

        #region ProcessarDataTable
        private bool ProcessarDataTable(DataTable dt, int DDD)
        {
            int qtdeRegistros = dt.Rows.Count;

            // agrupa a lista de celulares em pequenos lotes e faz a consulta ao cliente http
            for (int rowInicial = 0; rowInicial < qtdeRegistros; rowInicial += MaximaQtdeCelularesPorRequest)
            {
                int rowFinal = rowInicial + MaximaQtdeCelularesPorRequest > qtdeRegistros ?
                    qtdeRegistros :
                    rowInicial + MaximaQtdeCelularesPorRequest;

                if (!ConsultarOperadoraCelular(rowInicial, rowFinal, dt))
                    return false;
            }

            Persistir(dt, DDD);

            return true;
        }
        #endregion

        private HttpClientOperadoraCelular HttpClient
        {
            get;
            set;
        }

        #region ConsultarOperadoraCelular
        private bool ConsultarOperadoraCelular(int rowInicial, int rowFinal, DataTable dt)
        {
            string[] arrayCelulares = new string[rowFinal - rowInicial];

            // constroi os numeros de celulares a partir dos dados provenientes do BD
            for (int i = 0, j = rowInicial; j < rowFinal; ++i, ++j)
            {
                DataRow rowAtual = dt.Rows[j];

                arrayCelulares[i] = String.Format("{0}{1}", rowAtual["DDD"], rowAtual["Numero"]);
            }

            // consulta cliente http
            string[] arrayOperadoras;
            if (HttpClient.GetListaOperadoras(out arrayOperadoras, arrayCelulares))
            {
                // ocorreu tudo bem a consulta; interpretar resposta
                for (int i = 0, j = rowInicial; j < rowFinal; ++i, ++j)
                {
                    DataRow rowAtual = dt.Rows[j];

                    int operadoraNova;
                    Int32.TryParse(arrayOperadoras[i], out operadoraNova);

                    int operadoraAntiga;
                    Int32.TryParse(rowAtual["Idf_Operadora_Celular"].ToString(), out operadoraAntiga);

                    // se a operadora nova for NULL, mas a antiga não era, então mantém a operadora antiga
                    rowAtual["Idf_Operadora_Celular"] = operadoraNova;
                }

                return true;
            }

            // houve falha durante a consulta ao cliente http
            return false;
        }
        #endregion

        #region Persistir
        private void Persistir(DataTable dt, int ddd)
        {
            var grupos = dt.AsEnumerable().GroupBy(row => Convert.ToInt32(row["Idf_Operadora_Celular"]));

            foreach (var grupo in grupos)
            {
                // separa-se em tabelas PessoaFisica ou Contato dependendo da coluna de ID que houver dentro do DataTable
                int operadora = grupo.Key;
                var telefones = grupo.Select(row => row["Numero"].ToString()).ToArray();
                BLL.CelularLog.InserirTelefone(operadora, ddd, telefones);
            }
        }
        #endregion
    }
}
