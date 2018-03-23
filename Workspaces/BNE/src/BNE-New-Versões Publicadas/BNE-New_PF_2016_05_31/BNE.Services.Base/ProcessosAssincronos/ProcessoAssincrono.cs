using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using BNE.BLL.AsyncServices;
using BNE.Services.Base.Messaging;
using Enumeradores = BNE.BLL.AsyncServices.Enumeradores;

namespace BNE.Services.Base.ProcessosAssincronos
{
    /// <summary>
    /// Controlador de processos assíncronos
    /// </summary>
    public static class ProcessoAssincrono
    {
        #region Métodos

        #region IniciarAtividade

        /// <summary>
        /// Insere uma atividade na fila
        /// </summary>
        /// <param name="tipoAtividade">O tipo da atividade</param>
        /// <param name="objPlugins">A relação de plugins</param>
        /// <param name="parametrosEntrada">Os parâmetros de entrada</param>
        /// <param name="parametrosSaida">Os parâmetros de saída</param>
        /// <param name="tipoSaida">Tipo de Saida</param>
        /// <param name="anexo">O anexo</param>
        /// <param name="nomeAnexo">O nome do anexo</param>
        /// <param name="dtaAgendamento">A data de agendamento</param>
        /// <param name="enfileirarMensagem">Padrão true para enfileirar. Utilizado por processos de banco que enviam a mensagem de outro servidor</param>
        public static String IniciarAtividade(Enumeradores.TipoAtividade tipoAtividade, PluginsCompatibilidade objPlugins, ParametroExecucaoCollection parametrosEntrada, ParametroExecucaoCollection parametrosSaida, TipoSaida? tipoSaida, byte[] anexo, String nomeAnexo, DateTime dtaAgendamento, bool enfileirarMensagem = true)
        {
            if (objPlugins == null)
                throw new ArgumentNullException("objPlugins");

            // 1 - Cria a atividade
            var objAtividade = new Atividade
                {
                    PluginsCompatibilidade = objPlugins,
                    DescricaoParametrosEntrada = CollectionToXml(parametrosEntrada),
                    DescricaoParametrosSaida = CollectionToXml(parametrosSaida),
                    DataAgendamento = dtaAgendamento,
                    StatusAtividade = new StatusAtividade((int)Enumeradores.StatusAtividade.AguardandoExecucao)
                };

            if (tipoSaida.HasValue)
            {
                objAtividade.TipoSaida = new BLL.AsyncServices.TipoSaida((int)tipoSaida);
            }

            if (anexo != null)
            {
                objAtividade.DescricaoCaminhoArquivoUpload = SalvarArquivoAnexo(anexo, Guid.NewGuid() + nomeAnexo);
            }

            objAtividade.Save();

            if (enfileirarMensagem)
            {
                try
                {
                    // 2 - Lança na fila
                    EnfileirarAtividade(objAtividade.IdAtividade, tipoAtividade);
                }
                catch (Exception ex)
                {
                    var guid = EL.GerenciadorException.GravarExcecao(ex);
                    GravarErroAtividade(objAtividade.IdAtividade, ex.Message);
                    return guid.ToString();
                }
            }

            return String.Empty;
        }
        #endregion

        #region ReenviarSolicitacao
        public static String ReenviarSolicitacao(int idArquivoGeradoSelecionado, int idUsuarioLogado, out Atividade objAtividade)
        {

            //Copiando as informações de uma atividade e usando-as na criação de outra.
            objAtividade = Atividade.ReenviarSolicitacao(null, idArquivoGeradoSelecionado, idUsuarioLogado);
            objAtividade.PluginsCompatibilidade.CompleteObject();
            objAtividade.PluginsCompatibilidade.PluginEntrada.CompleteObject();

            try
            {
                //Método responsável por adicionar o item na fila do Message Queue.
                EnfileirarAtividade(objAtividade.IdAtividade, (Enumeradores.TipoAtividade)objAtividade.PluginsCompatibilidade.PluginEntrada.TipoAtividade.IdTipoAtividade);
            }
            catch (Exception ex)
            {
                var guid = EL.GerenciadorException.GravarExcecao(ex);
                GravarErroAtividade(objAtividade.IdAtividade, ex.Message);
                objAtividade = null;
                return guid.ToString();
            }
            return String.Empty;
        }
        #endregion

        #region ExcluirArquivoGerado
        /// <summary>
        /// Exclui um arquivo gerado
        /// </summary>
        /// <param name="nomeArquivo">O nome do arquivo</param>
        public static void ExcluirArquivoGerado(String nomeArquivo)
        {
            ExcluirArquivo(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CaminhoGeradosRelatorios), nomeArquivo);
        }
        #endregion

        #region ExcluirArquivoAnexo
        /// <summary>
        /// Exclui um arquivo anexo
        /// </summary>
        /// <param name="nomeArquivo">O nome do arquivo</param>
        public static void ExcluirArquivoAnexo(String nomeArquivo)
        {
            ExcluirArquivo(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CaminhoAnexosRelatorios), nomeArquivo);
        }
        #endregion

        #region ExcluirArquivo
        /// <summary>
        /// Exclui um arquivo
        /// </summary>
        /// <param name="caminhoDiretorio">O diretório</param>        
        /// <param name="nomeArquivo">O nome do arquivo a ser excluído</param>        
        private static void ExcluirArquivo(String caminhoDiretorio, String nomeArquivo)
        {
            if (!Directory.Exists(caminhoDiretorio))
                return;
            try
            {
                String caminhoArquivo = Path.Combine(caminhoDiretorio, nomeArquivo);
                File.Delete(caminhoArquivo);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region RecuperarArquivoAnexo
        /// <summary>
        /// Recupera um arquivo do diretório de anexos
        /// </summary>
        /// <param name="nomeArquivo">O nome do arquivo</param>
        /// <returns>Os bytes do arquivo</returns>
        public static byte[] RecuperarArquivoAnexo(String nomeArquivo)
        {
            return RecuperarArquivo(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CaminhoAnexosRelatorios), nomeArquivo);
        }
        #endregion

        #region RecuperarArquivo
        /// <summary>
        /// Recupera um arquivo como array de bytes
        /// </summary>
        /// <param name="caminhoDiretorio">O caminho do diretório aonde o arquivo está armazenado</param>
        /// <param name="nomeArquivo">O nome do arquivo</param>
        /// <returns>Os bytes do arquivo</returns>
        private static byte[] RecuperarArquivo(String caminhoDiretorio, String nomeArquivo)
        {
            String caminhoArquivo = Path.Combine(caminhoDiretorio, nomeArquivo);
            return File.ReadAllBytes(caminhoArquivo);
        }
        #endregion

        #region SalvarArquivoAnexo
        /// <summary>
        /// Salva os bytes como um arquivo no diretório de anexos
        /// </summary>
        /// <param name="arquivo">Os bytes do arquivo</param>
        /// <param name="nomeArquivo">O nome original com o qual o arquivo vai ser salvo</param>
        /// <returns>O nome final do arquivo</returns>
        public static String SalvarArquivoAnexo(byte[] arquivo, String nomeArquivo)
        {
            return SalvarArquivo(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CaminhoAnexosRelatorios), arquivo, nomeArquivo);
        }
        #endregion

        #region SalvarArquivoGerado
        /// <summary>
        /// Salva os bytes como um arquivo no diretório de arquivos gerados
        /// </summary>
        /// <param name="arquivo">Os bytes do arquivo</param>
        /// <param name="nomeArquivo">O nome original com o qual o arquivo vai ser salvo</param>
        /// <returns>O nome final do arquivo</returns>
        public static String SalvarArquivoGerado(byte[] arquivo, String nomeArquivo)
        {
            return SalvarArquivo(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CaminhoGeradosRelatorios), arquivo, nomeArquivo);
        }
        #endregion

        #region SalvarArquivo
        /// <summary>
        /// Salva os bytes como um arquivo em um diretório especificado
        /// </summary>
        /// <param name="caminhoDiretorio">O diretório</param>
        /// <param name="arquivo">Os bytes do arquivo</param>
        /// <param name="nomeArquivo">O nome original com o qual o arquivo vai ser salvo</param>
        /// <returns>O nome final do arquivo</returns>
        private static String SalvarArquivo(String caminhoDiretorio, byte[] arquivo, String nomeArquivo)
        {
            if (!Directory.Exists(caminhoDiretorio))
                Directory.CreateDirectory(caminhoDiretorio);

            String caminhoArquivo = Path.Combine(caminhoDiretorio, nomeArquivo);

            using (var objFileStream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                objFileStream.Write(arquivo, 0, arquivo.Length);
                objFileStream.Close();
            }

            return nomeArquivo;
        }
        #endregion

        #region MudarStatusAtividade
        /// <summary>
        /// Muda o status de uma atividade na fila
        /// </summary>
        /// <param name="idfAtividade">O código da atividade</param>
        /// <param name="novoStatus">O novo status da atividade</param>
        public static void MudarStatusAtividade(int idfAtividade, Enumeradores.StatusAtividade novoStatus)
        {
            Atividade.AtualizarStatusAtividade(idfAtividade, novoStatus);
        }
        #endregion

        #region ReiniciarAtividade
        /// <summary>
        /// Reinicia uma atividade na fila
        /// </summary>
        /// <param name="idfAtividade">O código da atividade</param>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        public static void ReiniciarAtividade(int idfAtividade, Enumeradores.TipoAtividade tipoAtividade)
        {
            // Muda status para aguardando execução
            MudarStatusAtividade(idfAtividade, Enumeradores.StatusAtividade.AguardandoExecucao);
            // Re-enfilera a atividade
            EnfileirarAtividade(idfAtividade, tipoAtividade);
        }
        #endregion

        #region GravarErroAtividade
        /// <summary>
        /// Grava o status de erro em uma atividade
        /// </summary>
        /// <param name="idAtividade">O código da atividade</param>
        /// <param name="errorMessage">O novo status da atividade</param>
        public static void GravarErroAtividade(int idAtividade, String errorMessage)
        {
            Atividade.DefinirStatusErro(idAtividade, errorMessage);
        }
        #endregion

        #region RecuperarNomeFila
        /// <summary>
        /// Recupera o nome formatado da fila de mensagens
        /// </summary>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        /// <returns>O nome formatado da fila</returns>
        public static String RecuperarNomeFila(Enumeradores.TipoAtividade tipoAtividade)
        {
            String caminhoPadrao = Parametro.RecuperaValorParametro(Enumeradores.Parametro.CaminhoPadraoQueue);
            //caminhoPadrao = @".\private$\";
            return String.Format(@"{0}bne_{1}", caminhoPadrao, tipoAtividade.ToString());
        }
        #endregion

        #region RecuperarAtividadesNaFila
        public static Collection<TarefaAssincrona> RecuperarAtividadesNaFila(int pagina, int tamanhoPagina, String condicoes, List<SqlParameter> parametros,
            String direcaoOrdenacao, String colunaOrdenacao, out int qtdRegistros, string qtdAtividadesPorTipo)
        {
            var tarefas = new Collection<TarefaAssincrona>();
            DataTable dtAtividades = Atividade.RecuperaAtividadesGrid(pagina, tamanhoPagina, condicoes, parametros, direcaoOrdenacao, colunaOrdenacao, out qtdRegistros, qtdAtividadesPorTipo);

            String urlDownloadAnexo = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlDownloadAnexosRelatorios);
            String urlDownloadGerado = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlDownloadGeradosRelatorios);

            foreach (DataRow row in dtAtividades.Rows)
            {
                tarefas.Add(TarefaAssincrona.LoadFromDataRow(row, urlDownloadGerado, urlDownloadAnexo));
            }

            return tarefas;
        }
        #endregion

        #region RecuperarParametrosAtividade
        /// <summary>
        /// Retorna os parâmetros de uma atividade
        /// </summary>
        /// <param name="idAtividade">O identificados de uma atividade</param>
        /// <returns>Os parâmetros da atividade</returns>
        public static ParametroExecucaoCollection RecuperarParametrosAtividade(int idAtividade)
        {
            return XmlToCollection(Atividade.RecuperParametrosEntradaAtividade(idAtividade));
        }
        #endregion

        #region RecuperarAtividade
        /// <summary>
        /// Recupera uma atividade específica
        /// </summary>
        /// <param name="idfAtividade">O idf da atividade</param>
        /// <returns>O resumo da tarefa</returns>
        public static TarefaAssincrona RecuperarAtividade(int idfAtividade)
        {
            try
            {
                Atividade objAtividade = Atividade.LoadObject(idfAtividade);
                objAtividade.StatusAtividade.CompleteObject();
                objAtividade.PluginsCompatibilidade.CompleteObject();
                objAtividade.PluginsCompatibilidade.PluginEntrada.CompleteObject();
                objAtividade.PluginsCompatibilidade.PluginSaida.CompleteObject();
                objAtividade.PluginsCompatibilidade.PluginSaida.TipoAtividade.CompleteObject();

                // Copia os dados para a tarefa
                var objTarefa = new TarefaAssincrona
                    {
                        IdfAtividade = idfAtividade,
                        InputPlugin = objAtividade.PluginsCompatibilidade.PluginEntrada.DescricaoPluginMetadata,
                        OutputPlugin = objAtividade.PluginsCompatibilidade.PluginSaida.DescricaoPluginMetadata,
                        NmeProcesso = objAtividade.PluginsCompatibilidade.PluginEntrada.DescricaoPlugin,
                        DataSolicitacao = objAtividade.DataCadastro,
                        DataExpiracao = objAtividade.DataCadastro.AddDays(objAtividade.PluginsCompatibilidade.PluginEntrada.TipoAtividade.NumeroDiasExpiracao),
                        Status = objAtividade.StatusAtividade.DescricaoStatusAtividade,
                        DescricaoCaminhoArquivoUpload = objAtividade.DescricaoCaminhoArquivoUpload,
                        DescricaoCaminhoArquivoGerado = objAtividade.DescricaoCaminhoArquivoGerado
                    };

                if (objAtividade.TipoSaida != null)
                {
                    objTarefa.TipoSaida = (TipoSaida)objAtividade.TipoSaida.IdTipoSaida;
                    objTarefa.DescricaoTipoSaida = objTarefa.TipoSaida.Value.GetDescription();
                }
                else
                {
                    objTarefa.TipoSaida = null;
                    objTarefa.DescricaoTipoSaida = String.Empty;
                }

                // Anexos
                if (!String.IsNullOrEmpty(objAtividade.DescricaoCaminhoArquivoUpload))
                {
                    objTarefa.Anexos = new Dictionary<string, byte[]>
                        {
                            {objAtividade.DescricaoCaminhoArquivoUpload, RecuperarArquivoAnexo(objAtividade.DescricaoCaminhoArquivoUpload)}
                        };
                }

                // Parâmetros
                objTarefa.ParametrosEntrada = XmlToCollection(objAtividade.DescricaoParametrosEntrada);
                objTarefa.ParametrosEntrada.Add("IdfAtividade", String.Empty, Convert.ToString(idfAtividade), String.Empty);
                if (objTarefa.TipoSaida != null)
                    objTarefa.ParametrosEntrada.Add("TipoSaida", String.Empty, Convert.ToString(objTarefa.TipoSaida.Value.GetHashCode()), objTarefa.DescricaoTipoSaida);

                objTarefa.ParametrosSaida = XmlToCollection(objAtividade.DescricaoParametrosSaida);
                objTarefa.ParametrosSaida.Add("IdfAtividade", String.Empty, Convert.ToString(idfAtividade), String.Empty);
                if (objTarefa.TipoSaida != null)
                    objTarefa.ParametrosSaida.Add("TipoSaida", String.Empty, Convert.ToString(objTarefa.TipoSaida.Value.GetHashCode()), objTarefa.DescricaoTipoSaida);

                objTarefa.TipoAtividade = (Enumeradores.TipoAtividade)objAtividade.PluginsCompatibilidade.PluginEntrada.TipoAtividade.IdTipoAtividade;
                return objTarefa;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw new InvalidOperationException(String.Format("Falha ao carregar a atividade {0}", idfAtividade), ex);
            }
        }
        #endregion

        #region CollectionToXml
        /// <summary>
        /// Converte uma coleção para um xml
        /// </summary>
        /// <param name="objDic">A coleção a ser convertida</param>
        /// <returns>O xml resultante</returns>
        internal static XmlDocument CollectionToXml(ParametroExecucaoCollection objDic)
        {
            if (objDic == null)
                return null;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(objDic.ToXML());
            return xmlDoc;
        }
        #endregion

        #region XmlToCollection
        /// <summary>
        /// Converte um XmlDocument em coleção
        /// </summary>
        /// <param name="xmlDoc">O XmlDocument a ser transformado</param>
        /// <returns>A coleção resultante</returns>
        internal static ParametroExecucaoCollection XmlToCollection(XmlDocument xmlDoc)
        {
            if (xmlDoc == null)
                return new ParametroExecucaoCollection();

            return ParametroExecucaoCollection.FromXML(xmlDoc.OuterXml);
        }
        #endregion

        #region EnfileirarAtividade
        /// <summary>
        /// Enfileira a atividade;
        /// Esse método não grava nem muda status da atividade na TAB_Atividade
        /// </summary>
        /// <param name="idAtividade">Idf da atividade no banco</param>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        public static void EnfileirarAtividade(int idAtividade, Enumeradores.TipoAtividade tipoAtividade)
        {
            var objMensagem = new MensagemAtividade
                {
                    IdfAtividade = idAtividade
                };
            using (var queue = new QueueService<MensagemAtividade>(RecuperarNomeFila(tipoAtividade), false))
            {
                queue.SendMessage(objMensagem);
            }
        }
        #endregion

        #region ExcluirAtividade
        /// <summary>
        /// Exclúi uma atividade 
        /// </summary>
        /// <param name="idfAtividade">O identificador da atividade</param>
        /// <returns>O sucesso da operação</returns>
        public static Boolean ExcluirAtividade(int idfAtividade)
        {
            try
            {
                Atividade.Excluir(idfAtividade);
                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return false;
            }
        }
        #endregion

        #region ExcluirTodasAtividadesFinalizadas
        /// <summary>
        /// Exclui todas as atividades que já foram executadas pelo processo
        /// </summary>
        public static void ExcluirTodasAtividadesFinalizadas()
        {
            Atividade.ExcluirAtividadesConcluidas();
        }
        #endregion

        #endregion
    }
}
