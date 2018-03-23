using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using BNE.BLL.Integracoes.Nexcore;
using BNE.Services.Code;
using BNE.Services.Properties;

namespace BNE.Services
{
    public partial class OperadoraCelular : ServiceBase
    {
        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly string HoraExecucao = Settings.Default.OperadoraCelularHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.OperadoraCelularDelayMinutos;
        private const string EventSourceName = "OperadoraCelular";

        private HttpClientOperadoraCelular HttpClient
        {
            get;
            set;
        }

        // 42 é a quantidade máxima de números de celulares que o webservice consegue retornar por requisição
        private const int MaximaQtdeCelularesPorRequest = 42;
        // 6000 é o tamanho máximo de um lote atualizável em um único comando SQL Server
        private const int TamanhoLote = 6000;
        #endregion

        #region Construtor
        public OperadoraCelular()
        {
            InitializeComponent();
            HttpClient = new HttpClientOperadoraCelular();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(Iniciar);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #endregion

        #region Metodos

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (primeiraExecucao)
            {
                string[] horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                TimeSpan tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(EventSourceName, String.Format("Serviço Operadora Celular - O Serviço está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Serviço Operadora Celular - O Serviço está aguardando {0} para sua próxima execução, pois levou {1} para concluir a execução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Serviço Operadora Celular - O Serviço vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
            }
        }
        #endregion

        #region Iniciar

        public void Iniciar()
        {
            try
            {
                AjustarThread(DateTime.Now, true);
                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                    BuscarOperadoraCelular();    // executa operação

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);

                    AjustarThread(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }

        #endregion

        #region CarregarIdInicial
        private int CarregarIdInicial()
        {
            // se for domingo, carregar o id com o valor 1, ou seja, vai varrer toda a tabela
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                return 1;

            // senao, pega o menor id entre as tabelas PessoaFisica e Contato
            // que tem a Dta_Cadastro mais recente
            DateTime dataCadastro = DateTime.Now.Date.AddDays(-1);
            int idPessoaFisica = BLL.PessoaFisica.CarregarIdCadastradoEm(dataCadastro);
            int idContato = BLL.Contato.CarregarIdCadastradoEm(dataCadastro);

            // dentre os id's das duas tabelas, pega o menor
            return idPessoaFisica < idContato ? idPessoaFisica : idContato;
        }
        #endregion CarregarIdInicial

        #region BuscarOperadoraCelular

        public void BuscarOperadoraCelular()
        {
            int idInicial = CarregarIdInicial();

            if (idInicial < 1)
                return; // não há nada no cadastro a ser atualizado

            int idFinal = idInicial + TamanhoLote - 1;

            bool haCargaPessoaFisica;
            bool haCargaContato;

            // agrupa a carga nas tabelas em lotes e atualiza a operadora de celular
            do
            {
                haCargaPessoaFisica = HaCargaTabelaPessoaFisica(idInicial, idFinal);
                haCargaContato = HaCargaTabelaContato(idInicial, idFinal);

                idInicial += TamanhoLote;
                idFinal += TamanhoLote;
            } while (haCargaPessoaFisica || haCargaContato);
        }

        #endregion BuscarOperadoraCelular

        #region HaCargaTabelaPessoaFisica
        private bool HaCargaTabelaPessoaFisica(int idInicial, int idFinal)
        {
            DataTable dt;

            if (BLL.PessoaFisica.CarregarOperadoraCelular(idInicial, idFinal, out dt))
                return ProcessarDataTable(dt);

            // nao tem mais carga
            return false;
        }
        #endregion

        #region HaCargaTabelaContato
        private bool HaCargaTabelaContato(int idInicial, int idFinal)
        {
            DataTable dt;

            if (BLL.Contato.CarregarOperadoraCelular(idInicial, idFinal, out dt))
                return ProcessarDataTable(dt);

            // nao tem mais carga
            return false;
        }
        #endregion

        #region ProcessarDataTable
        private bool ProcessarDataTable(DataTable dt)
        {
            IncluirColunaAlterar(dt);

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

            Persistir(dt);

            return true;
        }
        #endregion

        #region IncluirColunaAlterar
        private void IncluirColunaAlterar(DataTable dt)
        {
            // coluna que mostra qual DataRow alterar no banco de dados
            dt.Columns.Add("Alterar", typeof(bool));
        }
        #endregion

        #region ConsultarOperadoraCelular
        private bool ConsultarOperadoraCelular(int rowInicial, int rowFinal, DataTable dt)
        {
            string[] arrayCelulares = new string[rowFinal - rowInicial];

            // constroi os numeros de celulares a partir dos dados provenientes do BD
            for (int i = 0, j = rowInicial; j < rowFinal; ++i, ++j)
            {
                DataRow rowAtual = dt.Rows[j];

                arrayCelulares[i] = 
                    String.Format("{0}{1}", rowAtual["DDD"], rowAtual["Numero"]);
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
                    if (operadoraNova == 0 && operadoraAntiga != 0)
                        rowAtual["Alterar"] = false;
                    else
                    {
                        // só persiste alteração se a operadora nova for diferente da antiga
                        rowAtual["Alterar"] = operadoraNova != operadoraAntiga;
                        rowAtual["Idf_Operadora_Celular"] = operadoraNova;
                    }
                }

                return true;
            }

            // houve falha durante a consulta ao cliente http
            return false;
        }
        #endregion

        #region Persistir
        private void Persistir(DataTable dt)
        {
            var grupos =
                dt.AsEnumerable()
                    .Where(row => Convert.ToBoolean(row["Alterar"]))
                    .GroupBy(row => Convert.ToInt32(row["Idf_Operadora_Celular"]));

            foreach (var grupo in grupos)
            {
                // separa-se em tabelas PessoaFisica ou Contato dependendo da coluna de ID que houver dentro do DataTable
                if (dt.Columns.Contains("Idf_Pessoa_Fisica"))
                {
                    int[] pessoasFisicas =
                        grupo
                            .Select(row => Convert.ToInt32(row["Idf_Pessoa_Fisica"]))
                            .ToArray();

                    int operadora = grupo.Key;

                    BLL.PessoaFisica.AtualizarOperadoraCelular(operadora, pessoasFisicas);
                }
                else if (dt.Columns.Contains("Idf_Contato"))
                {
                    int[] contatos =
                        grupo
                            .Select(row => Convert.ToInt32(row["Idf_Contato"]))
                            .ToArray();

                    int operadora = grupo.Key;

                    BLL.Contato.AtualizarOperadoraCelular(operadora, contatos);
                }
                else
                    throw new InvalidOperationException("DataTable não tem nem coluna Idf_Pessoa_Fisica, nem Idf_Contato");
            }
        }
        #endregion

        #endregion
    }
}
