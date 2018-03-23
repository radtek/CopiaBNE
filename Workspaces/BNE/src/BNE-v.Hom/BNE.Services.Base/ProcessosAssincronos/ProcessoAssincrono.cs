using System;
using System.IO;
using System.Xml;
using BNE.BLL.AsyncServices;
using BNE.EL;
using BNE.Services.Base.Messaging;
using StatusAtividade = BNE.BLL.AsyncServices.Enumeradores.StatusAtividade;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;

namespace BNE.Services.Base.ProcessosAssincronos
{
    /// <summary>
    ///     Controlador de processos assíncronos
    /// </summary>
    public static class ProcessoAssincrono
    {
        #region Métodos

        #region IniciarAtividade
        /// <summary>
        ///     Insere uma atividade na fila
        /// </summary>
        /// <param name="tipoAtividade">O tipo da atividade</param>
        /// <param name="parametrosEntrada">Os parâmetros de entrada</param>
        /// <param name="anexo">O anexo</param>
        /// <param name="nomeAnexo">O nome do anexo</param>
        /// <param name="dtaAgendamento">A data de agendamento</param>
        /// <param name="enfileirarMensagem">
        ///     Padrão true para enfileirar. Utilizado por processos de banco que enviam a mensagem de
        ///     outro servidor
        /// </param>
        public static string IniciarAtividade(TipoAtividade tipoAtividade, ParametroExecucaoCollection parametrosEntrada, byte[] anexo = null, string nomeAnexo = null, DateTime dtaAgendamento = default(DateTime), bool enfileirarMensagem = true)
        {
            var objPlugins = PluginsCompatibilidade.CarregarPorTipoAtividade(tipoAtividade);

            if (objPlugins == null)
                throw new Exception("Plugin não encontrado");

            if (dtaAgendamento == default(DateTime))
                dtaAgendamento = DateTime.Now;

            // 1 - Cria a atividade
            var objAtividade = new Atividade
            {
                PluginsCompatibilidade = objPlugins,
                DescricaoParametrosEntrada = CollectionToXml(parametrosEntrada),
                DataAgendamento = dtaAgendamento,
                StatusAtividade = new BLL.AsyncServices.StatusAtividade((int) StatusAtividade.AguardandoExecucao)
            };

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
                    var guid = GerenciadorException.GravarExcecao(ex);
                    GravarErroAtividade(objAtividade.IdAtividade, ex.Message);
                    return guid;
                }
            }

            return string.Empty;
        }
        #endregion

        #region ExcluirArquivoGerado
        /// <summary>
        ///     Exclui um arquivo gerado
        /// </summary>
        /// <param name="nomeArquivo">O nome do arquivo</param>
        public static void ExcluirArquivoGerado(string nomeArquivo)
        {
            ExcluirArquivo(Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.CaminhoGeradosRelatorios), nomeArquivo);
        }
        #endregion

        #region ExcluirArquivoAnexo
        /// <summary>
        ///     Exclui um arquivo anexo
        /// </summary>
        /// <param name="nomeArquivo">O nome do arquivo</param>
        public static void ExcluirArquivoAnexo(string nomeArquivo)
        {
            ExcluirArquivo(Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.CaminhoAnexosRelatorios), nomeArquivo);
        }
        #endregion

        #region ExcluirArquivo
        /// <summary>
        ///     Exclui um arquivo
        /// </summary>
        /// <param name="caminhoDiretorio">O diretório</param>
        /// <param name="nomeArquivo">O nome do arquivo a ser excluído</param>
        private static void ExcluirArquivo(string caminhoDiretorio, string nomeArquivo)
        {
            if (!Directory.Exists(caminhoDiretorio))
                return;
            try
            {
                var caminhoArquivo = Path.Combine(caminhoDiretorio, nomeArquivo);
                File.Delete(caminhoArquivo);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region SalvarArquivoAnexo
        /// <summary>
        ///     Salva os bytes como um arquivo no diretório de anexos
        /// </summary>
        /// <param name="arquivo">Os bytes do arquivo</param>
        /// <param name="nomeArquivo">O nome original com o qual o arquivo vai ser salvo</param>
        /// <returns>O nome final do arquivo</returns>
        public static string SalvarArquivoAnexo(byte[] arquivo, string nomeArquivo)
        {
            return SalvarArquivo(Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.CaminhoAnexosRelatorios), arquivo, nomeArquivo);
        }
        #endregion

        #region SalvarArquivo
        /// <summary>
        ///     Salva os bytes como um arquivo em um diretório especificado
        /// </summary>
        /// <param name="caminhoDiretorio">O diretório</param>
        /// <param name="arquivo">Os bytes do arquivo</param>
        /// <param name="nomeArquivo">O nome original com o qual o arquivo vai ser salvo</param>
        /// <returns>O nome final do arquivo</returns>
        private static string SalvarArquivo(string caminhoDiretorio, byte[] arquivo, string nomeArquivo)
        {
            if (!Directory.Exists(caminhoDiretorio))
                Directory.CreateDirectory(caminhoDiretorio);

            var caminhoArquivo = Path.Combine(caminhoDiretorio, nomeArquivo);

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
        ///     Muda o status de uma atividade na fila
        /// </summary>
        /// <param name="idfAtividade">O código da atividade</param>
        /// <param name="novoStatus">O novo status da atividade</param>
        public static void MudarStatusAtividade(int idfAtividade, StatusAtividade novoStatus)
        {
            Atividade.AtualizarStatusAtividade(idfAtividade, novoStatus);
        }
        #endregion

        #region ReiniciarAtividade
        /// <summary>
        ///     Reinicia uma atividade na fila
        /// </summary>
        /// <param name="idfAtividade">O código da atividade</param>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        public static void ReiniciarAtividade(int idfAtividade, TipoAtividade tipoAtividade)
        {
            // Muda status para aguardando execução
            MudarStatusAtividade(idfAtividade, StatusAtividade.AguardandoExecucao);
            // Re-enfilera a atividade
            EnfileirarAtividade(idfAtividade, tipoAtividade);
        }
        #endregion

        #region GravarErroAtividade
        /// <summary>
        ///     Grava o status de erro em uma atividade
        /// </summary>
        /// <param name="idAtividade">O código da atividade</param>
        /// <param name="errorMessage">O novo status da atividade</param>
        public static void GravarErroAtividade(int idAtividade, string errorMessage)
        {
            Atividade.DefinirStatusErro(idAtividade, errorMessage);
        }
        #endregion

        #region RecuperarNomeFila
        /// <summary>
        ///     Recupera o nome formatado da fila de mensagens
        /// </summary>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        /// <returns>O nome formatado da fila</returns>
        public static string RecuperarNomeFila(TipoAtividade tipoAtividade)
        {
            var caminhoPadrao = Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.CaminhoPadraoQueue);
            //string caminhoPadrao = @"FormatName:Direct=OS:EMPVW0114202\private$\";
            return string.Format(@"{0}bne_{1}", caminhoPadrao, tipoAtividade);
        }

        public static string RecuperarNomeFilaLocal(TipoAtividade tipoAtividade)
        {
            var caminhoPadrao = @".\private$\";
            return string.Format(@"{0}bne_{1}", caminhoPadrao, tipoAtividade);
        }
        #endregion

        #region RecuperarAtividade
        /// <summary>
        ///     Recupera uma atividade específica
        /// </summary>
        /// <param name="idfAtividade">O idf da atividade</param>
        /// <returns>O resumo da tarefa</returns>
        public static TarefaAssincrona RecuperarAtividade(int idfAtividade)
        {
            try
            {
                return TarefaAssincrona.RecuperarTarefaAssincrona(new Atividade(idfAtividade));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw new InvalidOperationException(string.Format("Falha ao carregar a atividade {0}", idfAtividade), ex);
            }
        }
        #endregion

        #region CollectionToXml
        /// <summary>
        ///     Converte uma coleção para um xml
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
        ///     Converte um XmlDocument em coleção
        /// </summary>
        /// <param name="xmlDoc">O XmlDocument a ser transformado</param>
        /// <returns>A coleção resultante</returns>
        public static ParametroExecucaoCollection XmlToCollection(XmlDocument xmlDoc)
        {
            if (xmlDoc == null)
                return new ParametroExecucaoCollection();

            return ParametroExecucaoCollection.FromXML(xmlDoc.OuterXml);
        }
        public static ParametroExecucaoCollection XmlToCollection(XmlReader xmlReader)
        {
            if (xmlReader == null)
                return new ParametroExecucaoCollection();

            return ParametroExecucaoCollection.FromXML(xmlReader.ReadOuterXml());
        }
        public static ParametroExecucaoCollection XmlToCollection(string xmlString)
        {
            if (xmlString == null)
                return new ParametroExecucaoCollection();

            return ParametroExecucaoCollection.FromXML(xmlString);
        }
        #endregion

        #region RecuperarArquivoAnexo
        /// <summary>
        ///     Recupera um arquivo do diretório de anexos
        /// </summary>
        /// <param name="nomeArquivo">O nome do arquivo</param>
        /// <returns>Os bytes do arquivo</returns>
        public static byte[] RecuperarArquivoAnexo(string nomeArquivo)
        {
            return RecuperarArquivo(Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.CaminhoAnexosRelatorios), nomeArquivo);
        }
        #endregion

        #region RecuperarArquivo
        /// <summary>
        ///     Recupera um arquivo como array de bytes
        /// </summary>
        /// <param name="caminhoDiretorio">O caminho do diretório aonde o arquivo está armazenado</param>
        /// <param name="nomeArquivo">O nome do arquivo</param>
        /// <returns>Os bytes do arquivo</returns>
        private static byte[] RecuperarArquivo(string caminhoDiretorio, string nomeArquivo)
        {
            var caminhoArquivo = Path.Combine(caminhoDiretorio, nomeArquivo);
            return File.ReadAllBytes(caminhoArquivo);
        }
        #endregion

        #region EnfileirarAtividade
        /// <summary>
        ///     Enfileira a atividade;
        ///     Esse método não grava nem muda status da atividade na TAB_Atividade
        /// </summary>
        /// <param name="idAtividade">Idf da atividade no banco</param>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        public static void EnfileirarAtividade(int idAtividade, TipoAtividade tipoAtividade)
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

        #endregion
    }
}