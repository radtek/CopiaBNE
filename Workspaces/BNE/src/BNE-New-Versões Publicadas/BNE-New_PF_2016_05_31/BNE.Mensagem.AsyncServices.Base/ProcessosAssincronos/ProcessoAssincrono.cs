using System;
using BNE.Services.AsyncServices.Base.Messaging;
using BNE.Services.AsyncServices.Base.ProcessosAssincronos;
using BNE.Mensagem.AsyncServices.BLL;
using Newtonsoft.Json;
using Enumeradores = BNE.Mensagem.AsyncServices.BLL.Enumeradores;

namespace BNE.Mensagem.AsyncServices.Base.ProcessosAssincronos
{
    public class ProcessoAssincrono
    {
        
        #region IniciarAtividade
        /// <summary>
        /// Insere uma atividade na fila
        /// </summary>
        /// <param name="tipoAtividade">O tipo da atividade</param>
        /// <param name="objTemplate">Template usado</param>
        /// <param name="objPlugins">A relação de plugins</param>
        /// <param name="parametrosEntrada">Os parâmetros de entrada</param>
        /// <param name="parametrosSaida">Os parâmetros de saída</param>
        /// <param name="enfileirarMensagem">Padrão true para enfileirar. Utilizado por processos de banco que enviam a mensagem de outro servidor</param>
        /// <param name="objSistema">Sistema</param>
        public static String IniciarAtividade(Enumeradores.TipoAtividade tipoAtividade, Model.Sistema objSistema, Model.Template objTemplate, PluginsCompatibilidade objPlugins, ParametroExecucaoCollection parametrosEntrada, ParametroExecucaoCollection parametrosSaida, bool enfileirarMensagem = true)
        {
            if (objPlugins == null)
                throw new ArgumentNullException("objPlugins");

            // 1 - Cria a atividade
            var objAtividade = new Atividade
            {
                PluginsCompatibilidade = objPlugins,
                DescricaoParametrosEntrada = JsonConvert.SerializeObject(parametrosEntrada),
                DescricaoParametrosSaida = JsonConvert.SerializeObject(parametrosSaida),
                StatusAtividade = new StatusAtividade((int)Enumeradores.StatusAtividade.AguardandoExecucao),
                TipoAtividadeSistema = TipoAtividadeSistema.RecuperarPorSistemaETemplate(objSistema, objTemplate)
            };

            objAtividade.Save();

            if (enfileirarMensagem)
            {
                try
                {
                    // 2 - Lança na fila
                    EnfileirarAtividade(objAtividade.IdAtividade, tipoAtividade, objSistema, objTemplate);
                }
                catch (Exception ex)
                {
                    new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
                    GravarErroAtividade(objAtividade.IdAtividade, ex.Message);
                    return String.Empty;
                }
            }

            return String.Empty;
        }
        #endregion

        #region RecuperarNomeFila
        /// <summary>
        /// Recupera o nome formatado da fila de mensagens
        /// </summary>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        /// <param name="objSistema">Sistema que está consumindo</param>
        /// <param name="objTemplate">Template usado</param>
        /// <returns>O nome formatado da fila</returns>
        public static string RecuperarNomeFila(Enumeradores.TipoAtividade tipoAtividade, Model.Sistema objSistema, Model.Template objTemplate)
        {
            String caminhoPadrao = Parametro.RecuperaValorParametro(Enumeradores.Parametro.CaminhoPadraoQueue);
            //caminhoPadrao = @".\private$\";
            return String.Format(@"{0}mensagem_{1}_{2}_{3}", caminhoPadrao, tipoAtividade.ToString().ToLower(), objSistema.Nome.ToLower(), objTemplate.Nome.ToLower());
        }
        #endregion

        #region EnfileirarAtividade
        /// <summary>
        /// Enfileira a atividade;
        /// Esse método não grava nem muda status da atividade na TAB_Atividade
        /// </summary>
        /// <param name="idAtividade">Idf da atividade no banco</param>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        /// <param name="objSistema">Sistema que está consumindo</param>
        /// <param name="objTemplate">Template usado</param>
        public static void EnfileirarAtividade(int idAtividade, Enumeradores.TipoAtividade tipoAtividade, Model.Sistema objSistema, Model.Template objTemplate)
        {
            var objMensagem = new MensagemAtividade
            {
                IdfAtividade = idAtividade
            };
            using (var queue = new QueueService<MensagemAtividade>(RecuperarNomeFila(tipoAtividade, objSistema, objTemplate), false))
            {
                queue.SendMessage(objMensagem);
            }
        }
        #endregion

        #region ReiniciarAtividade
        /// <summary>
        /// Reinicia uma atividade na fila
        /// </summary>
        /// <param name="idAtividade">O código da atividade</param>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        /// <param name="objSistema">Sistema</param>
        /// <param name="objTemplate">Template</param>
        public static void ReiniciarAtividade(int idAtividade, Enumeradores.TipoAtividade tipoAtividade, Model.Sistema objSistema, Model.Template objTemplate)
        {
            // Muda status para aguardando execução
            MudarStatusAtividade(idAtividade, Enumeradores.StatusAtividade.AguardandoExecucao);
            // Re-enfilera a atividade
            EnfileirarAtividade(idAtividade, tipoAtividade, objSistema, objTemplate);
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

        #region RecuperarParametrosAtividade
        /// <summary>
        /// Retorna os parâmetros de uma atividade
        /// </summary>
        /// <param name="idAtividade">O identificados de uma atividade</param>
        /// <returns>Os parâmetros da atividade</returns>
        public static ParametroExecucaoCollection RecuperarParametrosAtividade(int idAtividade)
        {
            return RecuperarParametros(Atividade.RecuperParametrosEntradaAtividade(idAtividade));
        }
        public static ParametroExecucaoCollection RecuperarParametros(string parametros)
        {
            var retorno = JsonConvert.DeserializeObject<ParametroExecucaoCollection>(parametros);
            if (retorno == null)
                return new ParametroExecucaoCollection();

            return retorno;
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

                // Copia os dados para a tarefa
                var objTarefa = new TarefaAssincrona
                {
                    IdfAtividade = idfAtividade,
                    InputPlugin = objAtividade.PluginsCompatibilidade.PluginEntrada.DescricaoPluginMetadata,
                    OutputPlugin = objAtividade.PluginsCompatibilidade.PluginSaida.DescricaoPluginMetadata,
                    NmeProcesso = objAtividade.PluginsCompatibilidade.PluginEntrada.DescricaoPlugin,
                    DataSolicitacao = objAtividade.DataCadastro,
                    Status = objAtividade.StatusAtividade.DescricaoStatusAtividade,
                    ParametrosEntrada = RecuperarParametros(objAtividade.DescricaoParametrosEntrada)
                };

                // Parâmetros
                objTarefa.ParametrosEntrada.Add("IdfAtividade", String.Empty, Convert.ToString(idfAtividade), String.Empty);

                objTarefa.ParametrosSaida = RecuperarParametros(objAtividade.DescricaoParametrosSaida);
                objTarefa.ParametrosSaida.Add("IdfAtividade", String.Empty, Convert.ToString(idfAtividade), String.Empty);

                return objTarefa;
            }
            catch (Exception ex)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
                throw new InvalidOperationException(String.Format("Falha ao carregar a atividade {0}", idfAtividade), ex);
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
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
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

    }
}
