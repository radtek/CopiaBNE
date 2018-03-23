using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhatsJob.Domain
{
    public class Message
    {
        static BNE.ExceptionLog.DatabaseLogger _logger = new BNE.ExceptionLog.DatabaseLogger();

        public static void RespondMessages(Model.Channel channel)
        {
            try
            {
                using (var _ctx = new Data.WhatsJobsContext())
                {
                    DateTime expiredTime = DateTime.Now.AddMilliseconds(-Settings.Default.TimeMS_ToResponde);

                    var contacts = from m in _ctx.Message.Include("Contact")
                                   where m.Received
                                           && !m.Replied
                                           && m.WhatsChannel.Number == channel.Number
                                           && !_ctx.Message.Any(m2 => m2.Received
                                                                       && !m2.Replied
                                                                       && m2.WhatsChannel.Number == channel.Number
                                                                       && m2.Date > expiredTime)
                                   group m.Contact by m.Contact into c
                                   select c;

                    foreach (var c in contacts)
                    {
                        try
                        {
                            var messages = from m in _ctx.Message
                                           where m.Received
                                                && !m.Replied
                                                && m.WhatsChannel.Number == channel.Number
                                                && m.Contact.Number == c.Key.Number
                                                && m.Date < expiredTime
                                           select m;
                            string concat = messages.Select(m => m.TextMessage).ToList().Aggregate((i, j) => i + " " + j);

                            Model.Message response = Message.GetResponseFromBNE(concat, channel, c.Key);

                            if (response != null && !String.IsNullOrEmpty(response.TextMessage))
                            {
                                Model.Message m = new Model.Message(null, channel, c.Key, null, false);
                                m.TextMessage = Phrase.GetRandom().Description;
                                Domain.Message.Insert(m, _ctx);

                                Domain.Message.Insert(response, _ctx);
                            }

                            messages.ToList().ForEach(m => m.Replied = true);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, String.Format("Error responding contact {0}", c.Key.Number));
                        }
                    }

                    _ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro no RespondMessage");
            }
        }

        public static Model.Message GetResponseFromBNE(String message, Model.Channel channel, Model.Contact contact)
        {
            Model.Message retorno = new Model.Message(null, channel, contact, null, false);
            string link;

            message = StopWord.RemoveStopWords(message);
            List<DTO.VagaSolrBNE> vagas = Util.GetVagasBne(message);

            if (vagas.Count <= 0)
            {
                link = Util.GetLinkVagasBNE(null, null);
                retorno.TextMessage = link;
                return retorno;
            }

            var listaCidades = from vaga in vagas
                               group vaga by vaga.Cidade + "/" + vaga.UF into cidade
                               orderby cidade.Count() descending
                               select new { cidade = cidade.Key, count = cidade.Count() };

            var listaFuncoes = from vaga in vagas
                               group vaga by vaga.Funcao into funcao
                               orderby funcao.Count() descending
                               select new { funcao = funcao.Key, count = funcao.Count() };

            string nomeCidade = null;
            var cidadeMaiorOcorrencia = listaCidades.First();
            if ((float)cidadeMaiorOcorrencia.count / 100 > 0.15)
                nomeCidade = cidadeMaiorOcorrencia.cidade;

            string nomeFuncao = null;
            var funcaoMaiorOcorrencia = listaFuncoes.First();
            if ((float)funcaoMaiorOcorrencia.count / 100 > 0.15)
                nomeFuncao = funcaoMaiorOcorrencia.funcao;

            link = Util.GetLinkVagasBNE(nomeCidade, nomeFuncao);

            retorno.TextMessage = link;

            return retorno;
        }

        public static Model.Message GetResponseFromSine(String message, Model.Channel channel, Model.Contact contact)
        {
            Model.Message retorno = new Model.Message(null, channel, contact, null, false);

            message = StopWord.RemoveStopWords(message);
            List<DTO.VagaSine> vagas = Util.GetVagasSine(message);
            var listaCidades = from vaga in vagas
                               group vaga by vaga.dc + "/" + vaga.uf into cidade
                               orderby cidade.Count() descending
                               select new { cidade = cidade.Key, count = cidade.Count() };

            var listaFuncoes = from vaga in vagas
                               group vaga by vaga.df into funcao
                               orderby funcao.Count() descending
                               select new { funcao = funcao.Key, count = funcao.Count() };

            string c = listaCidades.First().cidade;
            string f = listaFuncoes.First().funcao;

            string link = String.Format("http://www.sine.com.br/vagas-empregos-em-{0}/{1}", Regex.Replace(c, "[ /]", "-").ToLower(), Regex.Replace(f, "[ /]", "-").ToLower());

            retorno.TextMessage = link;

            return retorno;
        }

        public static bool Insert(Model.Message objMessage)
        {
            try
            {
                using (var db = new Data.WhatsJobsContext())
                {
                    return Insert(objMessage, db);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao salvar mensagem (Domain.Message.Save)");
            }

            return false;
        }

        public static bool Insert(Model.Message objMessage, Data.WhatsJobsContext context)
        {
            try
            {
                context.Message.Add(objMessage);
                context.Entry(objMessage.WhatsChannel).State = System.Data.Entity.EntityState.Unchanged;
                context.Entry(objMessage.Contact).State = System.Data.Entity.EntityState.Unchanged;
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao salvar mensagem (Domain.Message.Save)");
            }

            return false;
        }
    }
}
