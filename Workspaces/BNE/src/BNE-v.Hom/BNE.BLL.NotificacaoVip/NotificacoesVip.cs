using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace BNE.BLL.NotificacoesVip
{
    public class NotificacoesVip
    {
        private static readonly List<BNE.BLL.NotificacaoVip> _notificacoes = NotificacaoVip.RetornarTodos();

        #region IniciarNotificacoesVip
        public static void IniciarNotificacoesVip()
        {
            try
            {
                // Carregar Lista de CVs VIP em que a ultima data de verificação de notificação é menor que a data de hoje
                var dt = Curriculo.ListarCurriculosEnvioNotificacao(DateTime.Now);
                var ListaVip = DTO.Curriculo.RetortarListaVipNotificacao(dt);

                //Parallel.ForEach(ListaVip, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, vip =>
                foreach(var vip in ListaVip)
                {
                    NotificarVip(vip);
                };
            }
            catch (Exception ex)
            {
                GravaLog(ex, "Falha no start de execução de notificacao de VIP. Método: IniciarNotificacoesVip");
            }
        }
        #endregion

        #region NotificarVip
        private static void NotificarVip(DTO.Curriculo vip)
        {
            try
            {
                GravaLog(null, "Iniciou Vip -> " + vip.idCurriculo);

                DateTime dataInicioPlano = vip.dataInicioPlano.Date;
                DateTime hoje = DateTime.Now.Date;

                //Definir Ciclo Do Mes
                TimeSpan diferencaDatas = hoje - dataInicioPlano;
                int diasCadastro = diferencaDatas.Days + 1;
                int cicloCV = 0;

                //Ciclo dura 30 dias, então caso a diferença seja maior que 30, é um ciclo maior... divirir por 30 e ver o resto
                if (diasCadastro <= 30)
                    cicloCV = diasCadastro;
                else
                    cicloCV = (diasCadastro % 30);

                NotificacaoVip notificacaoParaExecutar = _notificacoes.Where(x => x.NumeroDia == cicloCV).FirstOrDefault();
                if (notificacaoParaExecutar != null) //Verifica se existe ciclo para o dia
                {
                    //Verifica quantas vezes essa notificação já foi enviada para o CV
                    int qtdEnviosMesmaNotificacao = NotificacaoVipCurriculo.RetornarQuantidadeEnvioMesmaNotificacao(vip.idCurriculo, notificacaoParaExecutar.IdNotificacaoVip);

                    try
                    {
                        var AssemblyNotificacao = Assembly.GetExecutingAssembly();
                        var classeNotificacao = AssemblyNotificacao.GetTypes().Where(x => x.Namespace == "BNE.BLL.NotificacoesVip.Notificacoes" && x.Name == notificacaoParaExecutar.DescricaoClasseResponsavel).FirstOrDefault();
                        var objNotificacao = Activator.CreateInstance(classeNotificacao);

                        MethodInfo executorNotificacao = classeNotificacao.GetMethod("Notificar");

                        var parms = new object[] { vip, (short)notificacaoParaExecutar.IdNotificacaoVip, qtdEnviosMesmaNotificacao };

                        //GravaLog(null, "Vai invocar a notificação do Vip -> " + vip.idCurriculo);
                        executorNotificacao.Invoke(objNotificacao, parms);
                        //GravaLog(null, "Executou a notificação do Vip -> " + vip.idCurriculo);
                    }
                    catch (Exception ex)
                    {
                        GravaLog(ex, "Classe de notificação VIP não encontrada ou com erros. Classe: " + notificacaoParaExecutar.DescricaoClasseResponsavel + " - Notificação nº: " + notificacaoParaExecutar.IdNotificacaoVip + " - CV: " + vip.idCurriculo);
                    }
                }
                else
                {
                    GravaLog(null, "Nada para executar do Vip -> " + vip.idCurriculo);
                }

                GravarDataVerificacaoNotificacao(vip.idCurriculo);
            }
            catch (Exception ex)
            {
                GravaLog(ex, "Falha na verificacao de notificacoes para o CV: " + vip.idCurriculo);
            }

        }
        #endregion

        #region GravarDataVerificacaoNotificacao
        private static void GravarDataVerificacaoNotificacao(int idCurriculo)
        {
            ParametroCurriculo objParametroCurriculo;
            if (!ParametroCurriculo.CarregarParametroPorCurriculo(BLL.Enumeradores.Parametro.UltimaValidacaoNotificacaoVip, new Curriculo(idCurriculo), out objParametroCurriculo, null))
            {
                objParametroCurriculo = new ParametroCurriculo
                {
                    Curriculo = new Curriculo(idCurriculo),
                    Parametro = new Parametro((int)BLL.Enumeradores.Parametro.UltimaValidacaoNotificacaoVip),
                    ValorParametro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                objParametroCurriculo.Save();
            }
            else
            {
                objParametroCurriculo.ValorParametro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                objParametroCurriculo.Save();
            }
        }
        #endregion

        #region GravaLogText
        private static void GravaLog(Exception ex, string customMsg)
        {
            using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
            {
                sw.WriteLine(DateTime.Now + "   Mensagem:   " + customMsg);

            }

            if (ex != null)
                EL.GerenciadorException.GravarExcecao(ex, "NotificacoesVip -> " + customMsg);
        }
        #endregion
    }
}
