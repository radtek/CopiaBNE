using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using BNE.BLL;
using BNE.BLL.Custom.Solr.Buffer;
using BNE.BLL.DTO.OperadoraCelular;
using BNE.BLL.Integracoes.Nexcore;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.OperadoraCelular.Properties;

namespace BNE.Services.OperadoraCelular
{
    public partial class OperadoraCelular : ServiceBase
    {
        #region Construtor
        public OperadoraCelular()
        {
            InitializeComponent();
            HttpClient = new HttpClientOperadoraCelular();
            EventLogWriter = new EventLogWriter(Settings.Default.LogName, GetType().Name);
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly string HoraExecucao = Settings.Default.OperadoraCelularHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.OperadoraCelularDelayMinutos;

        private HttpClientOperadoraCelular HttpClient { get; set; }
        private EventLogWriter EventLogWriter { get; set; }

        // 42 é a quantidade máxima de números de celulares que o webservice consegue retornar por requisição
        private const int MaximaQtdeCelularesPorRequest = 40;
        // 6000 é o tamanho máximo de um lote atualizável em um único comando SQL Server
        private const int TamanhoLote = 10000;
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
                var horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                var tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(
                    string.Format(
                        "Serviço Operadora Celular - O Serviço está aguardando {0} para sua iniciar sua execução.",
                        tempoParaExecutar), EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int) tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Serviço Operadora Celular - O Serviço está aguardando {0} para sua próxima execução, pois levou {1} para concluir a execução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Serviço Operadora Celular - O Serviço vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #region Iniciar
        public void Iniciar()
        {
            try
            {
#if !DEBUG
                AjustarThread(DateTime.Now, true);
#endif

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.InicioExecucao);

                    BuscarOperadoraCelular();

                    EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.FimExecucao);

#if !DEBUG
                    AjustarThread(DateTime.Now, false);
#endif
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
            }
        }
        #endregion

        #region CarregarIdInicialPessoaFisica
        private int CarregarIdInicialPessoaFisica()
        {
            // se for domingo, carregar o id com o valor 1, ou seja, vai varrer toda a tabela
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                return 1;

            // senao, pega o menor id entre as tabelas PessoaFisica e Contato
            // que tem a Dta_Cadastro mais recente
            var dataCadastro = DateTime.Now.Date.AddDays(-1);

            return PessoaFisica.CarregarIdCadastradoEm(dataCadastro);
        }
        #endregion CarregarIdInicial

        #region CarregarIdInicialContato
        private int CarregarIdInicialContato()
        {
            // se for domingo, carregar o id com o valor 1, ou seja, vai varrer toda a tabela
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                return 1;

            // senao, pega o menor de Contato
            // que tem a Dta_Cadastro mais recente
            var dataCadastro = DateTime.Now.Date.AddDays(-1);

            return Contato.CarregarIdCadastradoEm(dataCadastro);
        }
        #endregion CarregarIdInicial

        #region BuscarOperadoraCelular
        public void BuscarOperadoraCelular()
        {
            var tasklist = new List<Task>
            {
                Task.Factory.StartNew(BuscarOperadoraCelularPessoaFisica),
                Task.Factory.StartNew(BuscarOperadoraCelularContato)
            };

            Task.WaitAll(tasklist.ToArray());
        }

        public void BuscarOperadoraCelularPessoaFisica()
        {
            var idInicial = CarregarIdInicialPessoaFisica();

            if (idInicial < 1)
                return; // não há nada no cadastro a ser atualizado

            var idFinal = idInicial + TamanhoLote - 1;

            bool haCargaPessoaFisica;

            // agrupa a carga nas tabelas em lotes e atualiza a operadora de celular
            do
            {
                haCargaPessoaFisica = HaCargaTabelaPessoaFisica(idInicial, idFinal);

                idInicial += TamanhoLote;
                idFinal += TamanhoLote;
            } while (haCargaPessoaFisica);
        }

        public void BuscarOperadoraCelularContato()
        {
            var idInicial = CarregarIdInicialContato();

            if (idInicial < 1)
                return; // não há nada no cadastro a ser atualizado

            var idFinal = idInicial + TamanhoLote - 1;

            bool haCargaContato;

            // agrupa a carga nas tabelas em lotes e atualiza a operadora de celular
            do
            {
                haCargaContato = HaCargaTabelaContato(idInicial, idFinal);

                idInicial += TamanhoLote;
                idFinal += TamanhoLote;
            } while (haCargaContato);
        }
        #endregion BuscarOperadoraCelular

        #region HaCargaTabelaPessoaFisica
        private bool HaCargaTabelaPessoaFisica(int idInicial, int idFinal)
        {
            var lista = PessoaFisica.CarregarOperadoraCelular(idInicial, idFinal);

            if (lista.Count > 0)
            {
                return ProcessarPessoaFisica(lista);
            }

            // nao tem mais carga
            return false;
        }
        #endregion

        #region HaCargaTabelaContato
        private bool HaCargaTabelaContato(int idInicial, int idFinal)
        {
            var lista = Contato.CarregarOperadoraCelular(idInicial, idFinal);

            if (lista.Count > 0)
            {
                return ProcessarDataTableContato(lista);
            }

            // nao tem mais carga
            return false;
        }
        #endregion

        #region ProcessarDataTable
        private bool ProcessarPessoaFisica(List<PessoaFisicaOperadoraCelular> lista)
        {
            if (Processar(new List<BLL.DTO.OperadoraCelular.OperadoraCelular>(lista)))
            {
                PersistirPessoaFisica(lista);
            }

            return true;
        }

        private bool ProcessarDataTableContato(List<ContatoOperadoraCelular> lista)
        {
            if (Processar(new List<BLL.DTO.OperadoraCelular.OperadoraCelular>(lista)))
            {
                PersistirContato(lista);
            }

            return true;
        }

        private bool Processar(List<BLL.DTO.OperadoraCelular.OperadoraCelular> lista)
        {
            var qtdeRegistros = lista.Count;

            // agrupa a lista de celulares em pequenos lotes e faz a consulta ao cliente http
            for (var rowInicial = 0; rowInicial < qtdeRegistros; rowInicial += MaximaQtdeCelularesPorRequest)
            {
                var rowFinal = rowInicial + MaximaQtdeCelularesPorRequest > qtdeRegistros
                    ? qtdeRegistros
                    : rowInicial + MaximaQtdeCelularesPorRequest;

                if (!ConsultarOperadoraCelular(rowInicial, rowFinal, lista))
                    return false;
            }

            return true;
        }
        #endregion

        //#region IncluirColunaAlterar
        //private void IncluirColunaAlterar(ref DataTable dt)
        //{
        //    // coluna que mostra qual DataRow alterar no banco de dados
        //    dt.Columns.Add("Alterar", typeof(bool));
        //}
        //#endregion

        #region ConsultarOperadoraCelular
        private bool ConsultarOperadoraCelular(int rowInicial, int rowFinal,
            List<BLL.DTO.OperadoraCelular.OperadoraCelular> lista)
        {
            var arrayCelulares = new string[rowFinal - rowInicial];

            // constroi os numeros de celulares a partir dos dados provenientes do BD
            for (int i = 0, j = rowInicial; j < rowFinal; ++i, ++j)
            {
                var rowAtual = lista.ToArray()[j];
                arrayCelulares[i] = string.Format("{0}{1}", rowAtual.DDD, rowAtual.Numero);
            }

            // consulta cliente http
            string[] arrayOperadoras;
            if (HttpClient.GetListaOperadoras(out arrayOperadoras, arrayCelulares))
            {
                // ocorreu tudo bem a consulta; interpretar resposta
                for (int i = 0, j = rowInicial; j < rowFinal; ++i, ++j)
                {
                    var rowAtual = lista.ToArray()[j];

                    int operadoraNova;
                    int.TryParse(arrayOperadoras[i], out operadoraNova);

                    var operadoraAntiga = 0;
                    if (rowAtual.IdOperadoraCelular.HasValue)
                    {
                        operadoraAntiga = rowAtual.IdOperadoraCelular.Value;
                    }

                    // se a operadora nova for NULL, mas a antiga não era, então mantém a operadora antiga
                    if (operadoraNova != 0 || operadoraAntiga == 0)
                    {
                        // só persiste alteração se a operadora nova for diferente da antiga
                        rowAtual.Alterar = operadoraNova != operadoraAntiga;
                        rowAtual.IdOperadoraCelular = operadoraNova;
                    }
                }

                return true;
            }

            // houve falha durante a consulta ao cliente http
            return false;
        }
        #endregion

        #region PersistirPessoaFisica
        private void PersistirPessoaFisica(List<PessoaFisicaOperadoraCelular> lista)
        {
            var grupos =
                lista.Where(c => c.Alterar && c.IdOperadoraCelular.HasValue).GroupBy(c => c.IdOperadoraCelular.Value);
            foreach (var grupo in grupos)
            {
                var operadora = grupo.Key;
                var pessoasFisicas = grupo.Select(m => m.IdPessoaFisica).ToList();
                PessoaFisica.AtualizarOperadoraCelular(operadora, pessoasFisicas);
                try
                {
                    var curriculos =
                        grupo.Where(m => m.IdCurriculo.HasValue)
                            .Select(m => new Curriculo(m.IdCurriculo.Value))
                            .ToList();
                    if (curriculos.Any())
                    {
                        curriculos.ForEach(BufferAtualizacaoCurriculo.Update);
                    }
                }
                catch (Exception ex)
                {
                    GerenciadorException.GravarExcecao(ex, "Falha ao atualizar os currículos no Solr");
                }
            }
        }
        #endregion

        #region PersistirContato
        private void PersistirContato(List<ContatoOperadoraCelular> lista)
        {
            var grupos =
                lista.Where(c => c.Alterar && c.IdOperadoraCelular.HasValue).GroupBy(c => c.IdOperadoraCelular.Value);
            foreach (var grupo in grupos)
            {
                var operadora = grupo.Key;
                var contatos = grupo.Select(m => m.IdContato).ToList();
                Contato.AtualizarOperadoraCelular(operadora, contatos);
                try
                {
                    var curriculos =
                        grupo.Where(m => m.IdCurriculo.HasValue)
                            .Select(m => new Curriculo(m.IdCurriculo.Value))
                            .ToList();
                    if (curriculos.Any())
                    {
                        curriculos.ForEach(BufferAtualizacaoCurriculo.Update);
                    }
                }
                catch (Exception ex)
                {
                    GerenciadorException.GravarExcecao(ex, "Falha ao atualizar os currículos no Solr");
                }
            }
        }
        #endregion

        #endregion
    }
}