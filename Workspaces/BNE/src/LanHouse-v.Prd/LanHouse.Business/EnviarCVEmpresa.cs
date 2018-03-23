using LanHouse.Business.Custom;
using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class EnviarCVEmpresa
    {
        #region QuantidadeEnvioDisponiveisUsuario
        /// <summary>
        /// Retorna a quantidade de envios de CV disponível para o usuário
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static int QuantidadeEnvioDisponiveisUsuario(int idCurriculo)
        {
            using (var entity = new LanEntities())
            {
                var valorParametro = (
                    from par in entity.TAB_Parametro
                    where par.Idf_Parametro == (int)Business.Enumeradores.Parametro.QuantidadeEnvioCurriculoEmpresa
                    select par).FirstOrDefault().Vlr_Parametro;

                var query = (
                        from parametro_cv in entity.TAB_Parametro_Curriculo
                        where parametro_cv.Idf_Curriculo == idCurriculo
                        && parametro_cv.Idf_Parametro == (int)Business.Enumeradores.Parametro.QuantidadeEnvioCurriculoEmpresa
                        select parametro_cv.Vlr_Parametro).FirstOrDefault();

                return query != null? Convert.ToInt32(query) : Convert.ToInt32(valorParametro);
            }
        }
        #endregion

        #region VerificaEnvioCurriculo
        /// <summary>
        /// Verificar se o candidato já envio o CV para a empresa
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static bool VerificaEnvioCurriculo(int idCurriculo, int idFilial, out BNE_Intencao_Filial objIntencao_Filial)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from filial_candidato in entity.BNE_Intencao_Filial
                    where filial_candidato.Idf_Curriculo == idCurriculo
                        && filial_candidato.Idf_Filial == idFilial
                    select filial_candidato).FirstOrDefault();

                objIntencao_Filial = query;
                return query != null;
            }
        }
        #endregion

        #region RegistrarIntencaoCandidato
        /// <summary>
        /// Registrar Intenção do candidato para Filial
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public static bool RegistrarIntencaoCandidato(int idCurriculo, int idEmpresa)
        {
            bool retorno = false;

            using (var context = new LanEntities())
            {
                try
                {
                    //carregar o Idf_Companhia pelo idFilial
                    int idCompanhia = Business.Companhia.RecuperarIdf_Companhia(idEmpresa);

                    #region Registrar a Intenção
                    //Registrar a Intenção
                    context.BNE_Intencao_Filial.Add(new BNE_Intencao_Filial()
                    {
                        Idf_Filial = idEmpresa,
                        Idf_Curriculo = idCurriculo,
                        Dta_Cadastro = DateTime.Now,
                        Flg_Inativo = false,
                    });
                    #endregion

                    #region Registrar a Oportunidade Curriculo
                    //Registrar a Oportunidade Curriculo
                    context.LAN_Oportunidade_Curriculo.Add(new LAN_Oportunidade_Curriculo()
                    {
                        Idf_Curriculo = idCurriculo,
                        Idf_Filial = idEmpresa,
                        Idf_Companhia = idCompanhia,
                        Idf_Fase_Cadastro = (int)Business.Enumeradores.LAN_Fase_Cadastro.Minicurriculo,
                        Dta_Cadastro = DateTime.Now,
                        Flg_Inativo = false
                    });
                    #endregion

                    #region Registrar a Oportunidade Curriculo Historico
                    //Registrar a Oportunidade Curriculo Historico
                    context.LAN_Oportunidade_Curriculo_Historico.Add(new LAN_Oportunidade_Curriculo_Historico()
                    {
                        Idf_Curriculo = idCurriculo,
                        Idf_Filial = idEmpresa,
                        Idf_Companhia = idCompanhia,
                        Dta_Cadastro = DateTime.Now
                    });

                    #endregion

                    //salvar os registros
                    context.SaveChanges();

                    retorno = true;
                }
                catch
                {

                }

                return retorno;
            }
        }

        #endregion

        #region CarregarResponsavelEmpresa
        /// <summary>
        /// Carregar o email do responsavel pela empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public static string CarregarResponsavelEmpresa(int idEmpresa)
        {
            using (var context = new LanEntities())
            {
                // 3- Pegar Email do responsável pela empresa
                var usuarioResponsavel = (from ufp in context.TAB_Usuario_Filial_Perfil
                                          join uf in context.BNE_Usuario_Filial on ufp.Idf_Usuario_Filial_Perfil equals uf.Idf_Usuario_Filial_Perfil
                                          where ufp.Idf_Filial == idEmpresa && uf.Eml_Comercial != ""
                                          select uf.Eml_Comercial).FirstOrDefault();

                return usuarioResponsavel;
            }
        }

        #endregion

        #region GerarHtmlCV
        /// <summary>
        /// Gerar Html do CV para gerar PDF
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static Html GerarHtmlCV(int idCurriculo)
        {
            Html CVHtml = new Html();

            using (var context = new LanEntities())
            {
                CVHtml.html = GerarHtmlEmail(idCurriculo);
                return CVHtml;
            }
        }
        #endregion

        #region AtualizarSaldoEnvioEmail
        /// <summary>
        /// Insere ou atualiza o parametro de EnvioCVEmpresa
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="context"></param>
        public static void AtualizarSaldoEnvioEmail(int idCurriculo)
        {
            using (var context = new LanEntities())
            {
                var parametroEnvio = (
                            from p in context.TAB_Parametro_Curriculo
                            where p.Idf_Curriculo == idCurriculo && p.Idf_Parametro == (int)Business.Enumeradores.Parametro.QuantidadeEnvioCurriculoEmpresa
                            select p).FirstOrDefault();

                if (parametroEnvio == null)
                {
                    parametroEnvio = new TAB_Parametro_Curriculo();

                    parametroEnvio.Flg_Inativo = false;
                    parametroEnvio.Idf_Curriculo = idCurriculo;
                    parametroEnvio.Idf_Parametro = (int)Business.Enumeradores.Parametro.QuantidadeEnvioCurriculoEmpresa;
                    parametroEnvio.Vlr_Parametro = (Convert.ToInt32(parametroEnvio.Vlr_Parametro) - 1).ToString();
                    parametroEnvio.Dta_Cadastro = DateTime.Now;

                    context.TAB_Parametro_Curriculo.Add(parametroEnvio);
                }
                else
                {
                    parametroEnvio.Vlr_Parametro = (Convert.ToInt32(parametroEnvio.Vlr_Parametro) - 1).ToString();
                }

                context.SaveChanges();
            }
        }


        #endregion

        #region GerarHtmlEmail
        /// <summary>
        /// Gera o HTMl do CV para enviar por email
        /// </summary>
        /// <returns></returns>
        public static string GerarHtmlEmail(int idCurriculo)
        {
            string html = string.Empty;

            //Carregar o CV do Candidato
            BNE_Curriculo objCurriculo;
            bool existe = Business.Curriculo.CarregarPorId(idCurriculo, out objCurriculo);

            //Carregar a Pessoa Fisica do Candidato
            TAB_Pessoa_Fisica objPessoaFisica;
            bool temPF = Business.PessoaFisica.CarregarPorCV(idCurriculo, out objPessoaFisica);

            Business.DTO.Curriculo dtoCurriculo = Business.Curriculo.CarregarCurriculo(objPessoaFisica.Num_CPF.ToString(), objPessoaFisica.Dta_Nascimento.ToString(), objPessoaFisica.Nme_Pessoa);

            if(dtoCurriculo != null)
            {
                html = PreencherCVnoHtml(dtoCurriculo);
            }
            
            return html;
        }

        #endregion

        #region PreencherCVnoHtml
        public static string PreencherCVnoHtml(Business.DTO.Curriculo dtoCurriculo)
        {
            //Carregar Carta com Html do CV
            string html = Business.CartaEmail.CarregarCartaporId((int)Business.Enumeradores.CartaEmail.CurriculoModeloBNE);

            html = html.Replace("{imprimirCV.curriculo.nome}", dtoCurriculo.nome);
            html = html.Replace("{imprimirCV.curriculo.sexo}", dtoCurriculo.Sexo == 1 ? "Masculino" : "Feminino");
            html = html.Replace("{showEstadoCivil}", Business.EstadoCivil.CarregarEstadoCivilporId(dtoCurriculo.idEstadoCivil).Des_Estado_Civil);
            html = html.Replace("{imprimirCV.curriculo.dataNascimento}", dtoCurriculo.DataNascimento);
            html = html.Replace("{calcularIdadeCandidato}", Business.Custom.Uteis.CalcularIdade(Convert.ToDateTime(dtoCurriculo.DataNascimento),DateTime.Now).ToString());

            html = html.Replace("{imprimirCV.curriculo.dddTelefone}", dtoCurriculo.DDDTelefone);
            html = html.Replace("{imprimirCV.curriculo.telefone}", dtoCurriculo.Telefone);
            html = html.Replace("{imprimirCV.curriculo.dddCelular}", dtoCurriculo.DDDCelular);
            html = html.Replace("{imprimirCV.curriculo.celular}", dtoCurriculo.Celular);

            html = html.Replace("{imprimirCV.curriculo.cidade}", dtoCurriculo.Cidade);
            html = html.Replace("{imprimirCV.curriculo.email}", dtoCurriculo.email);

            

            html = html.Replace("{imprimirCV.curriculo.funcao}", FuncaoPretendida.CarregarDescricaoFuncaoPretendidaPorCurriculo(dtoCurriculo.idCurriculo));
            html = html.Replace("{imprimirCV.curriculo.pretensaoSalarial}", dtoCurriculo.PretensaoSalarial.ToString("C"));
            html = html.Replace("{imprimirCV.curriculo.email}", dtoCurriculo.email);

            //Escolaridade
            html = html.Replace("{imprimirCV.curriculo.formacoes}", FormatarFormcoes(dtoCurriculo.formacoes));
            html = html.Replace("{imprimirCV.curriculo.cursos}", FormatarCursos(dtoCurriculo.cursos));
            html = html.Replace("{imprimirCV.curriculo.idiomasCandidato}", FormatarIdiomas(dtoCurriculo.idiomasCandidato));

            //Experiencias
            html = html.Replace("{imprimirCV.curriculo.experiencias}", FormatarExperienciasProfissionais(dtoCurriculo));

            //Dados complementares
            html = html.Replace("{imprimirCV.curriculo.dadosComplementares}",FormatarDadosComplementares(dtoCurriculo));

            return html;
        }
        #endregion

        #region FormatarFormcoes
        /// <summary>
        /// Formata a exibição da Formações do candidato
        /// </summary>
        /// <param name="formacoes"></param>
        /// <returns></returns>
        public static string FormatarFormcoes(IList<Business.DTO.Formacao> formacoes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var formacao in formacoes)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td valign=\"top\" style=\"text-align: left; padding: 5px 0 5px 0\">{0}<a href=\"#\">{1}</a> <a href=\"#\">{2}</a> <a href=\"#\">{3}</a> <a href=\"#\">{4}</a></td>",
                    formacao.nivel, formacao.curso, formacao.instituicao, formacao.cidadeFormacao, formacao.anoConclusao);
                sb.Append("</tr>");
            }

            return sb.ToString();
        }
        #endregion

        #region FormatarCursos
        /// <summary>
        /// Formata a exibição dos cursos do candidato
        /// </summary>
        /// <param name="cursos"></param>
        /// <returns></returns>
        public static string FormatarCursos(IList<Business.DTO.Formacao> cursos)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var formacao in cursos)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td valign=\"top\" style=\"text-align: left; padding: 5px 0 5px 0\">{0}<a href=\"#\">{1}</a> <a href=\"#\">{2}</a> <a href=\"#\">{3}</a> <a href=\"#\">{4}</a></td>",
                    formacao.nivel, formacao.curso, formacao.instituicao, formacao.cidadeFormacao, formacao.anoConclusao);
                sb.Append("</tr>");
            }

            return sb.ToString();
        }
        #endregion

        #region FormatarIdiomas
        /// <summary>
        /// Formata a exibição dos idimas do candidato
        /// </summary>
        /// <param name="idiomas"></param>
        /// <returns></returns>
        public static string FormatarIdiomas(IList<Business.DTO.IdiomaCandidato> idiomasCandidato)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var idioma in idiomasCandidato)
            {
                sb.Append("<tr class=\"linha_idiomas\" id=\"trLinhaIdiomas\">");
                sb.AppendFormat("<td style=\"text-align: left;\">");
                sb.Append("<ul>");
                sb.AppendFormat("<li><a href=\"#\">Nível {0}</a> em <a href=\"#\">{1}</a></li>",idioma.nivelTexto,idioma.text.ToLower());
                sb.Append("</ul>");
                sb.Append("</td></tr>");
            }

            return sb.ToString();
        }
        #endregion

        #region FormatarExperienciasProfissionais
        /// <summary>
        /// Formata as experiência profissionais do candidato
        /// </summary>
        /// <param name="curriculo"></param>
        /// <returns></returns>
        public static string FormatarExperienciasProfissionais(Business.DTO.Curriculo curriculo)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < curriculo.experiencias.Count;i++)
            {
                switch(i)
                {
                    case 0:
                        sb.Append("<table style=\"margin-left: 20px; width: 700px;\" cellpadding=\"0\" cellspacing=\"0\"><tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan=\"2\" valign=\"top\" style=\"text-align: left; width: 650px; font-style: italic; padding-top: 5px; font-weight: bold;\">");
                        sb.AppendFormat("{0} - de {1} de {2} até {3} de {4}", curriculo.experiencias[i].Razao, System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(curriculo.experiencias[i].dataAdmissao.Month),
                            curriculo.experiencias[i].dataAdmissao.ToString("yyyy"), curriculo.experiencias[i].dataDemissao.HasValue? System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(curriculo.experiencias[i].dataDemissao.Value.Month):"",
                            curriculo.experiencias[i].dataDemissao.HasValue? curriculo.experiencias[i].dataDemissao.Value.ToString("yyyy"):"");            
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td valign=\"top\" style=\"text-align: left; font-weight: bold; width: 175px;\">Função Exercida:</td>");
                        sb.Append("<td valign=\"top\" style=\"text-align: left;\">");
                        sb.AppendFormat("{0}", curriculo.experiencias[i].funcaoEmpresa);
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td valign=\"top\" style=\"text-align: left; font-weight: bold; width: 175px;\">Atribuições:</td>");
                        sb.AppendFormat("<td valign=\"top\" style=\"text-align: justify; text-justify: inter-word;\">{0}</td></tr>", curriculo.experiencias[i].DesAtividades);
                        sb.Append("</tbody></table>");
                        break;
                    case 1:
                        sb.Append("<table style=\"margin-left: 10px; width: 700px;\" cellpadding=\"0\" cellspacing=\"0\"><tbody>");
                        sb.Append("<tr><td colspan=\"2\" valign=\"top\" style=\"text-align: left; width: 550px; font-style: italic;padding-top: 5px; font-weight: bold;\">");
                        sb.AppendFormat("{0} - de {1} de {2} até {3} de {4}", curriculo.experiencias[i].Razao, System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(curriculo.experiencias[i].dataAdmissao.Month),
                            curriculo.experiencias[i].dataAdmissao.ToString("yyyy"), curriculo.experiencias[i].dataDemissao.HasValue ? System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(curriculo.experiencias[i].dataDemissao.Value.Month) : "",
                            curriculo.experiencias[i].dataDemissao.HasValue ? curriculo.experiencias[i].dataDemissao.Value.ToString("yyyy") : "");            
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td valign=\"top\" style=\"text-align: left; font-weight: bold; width: 175px;\">Função Exercida:</td>");
                        sb.Append("<td valign=\"top\" style=\"text-align: left;\">");
                        sb.AppendFormat("{0}", curriculo.experiencias[i].funcaoEmpresa);
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td valign=\"top\" style=\"text-align: left; font-weight: bold; width: 175px;\">Atribuições:</td>");
                        sb.AppendFormat("<td valign=\"top\" style=\"text-align: justify; text-justify: inter-word;\">{0}</td></tr>", curriculo.experiencias[i].DesAtividades);

                        sb.Append("</tbody></table>");
                        break;
                    case 2:
                        sb.Append("<table style=\"margin-left: 10px; width: 700px;\" cellpadding=\"0\" cellspacing=\"0\"><tbody>");
                        sb.Append("<tr><td colspan=\"2\" valign=\"top\" style=\"text-align: left; width: 550px; font-style: italic;padding-top: 5px; font-weight: bold;\">");
                        sb.AppendFormat("{0} - de {1} de {2} até {3} de {4}", curriculo.experiencias[i].Razao, System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(curriculo.experiencias[i].dataAdmissao.Month),
                            curriculo.experiencias[i].dataAdmissao.ToString("yyyy"), curriculo.experiencias[i].dataDemissao.HasValue ? System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(curriculo.experiencias[i].dataDemissao.Value.Month) : "",
                            curriculo.experiencias[i].dataDemissao.HasValue ? curriculo.experiencias[i].dataDemissao.Value.ToString("yyyy") : "");
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td valign=\"top\" style=\"text-align: left; font-weight: bold; width: 175px;\">Função Exercida:</td>");
                        sb.Append("<td valign=\"top\" style=\"text-align: left;\">");
                        sb.AppendFormat("{0}", curriculo.experiencias[i].funcaoEmpresa);
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td valign=\"top\" style=\"text-align: left; font-weight: bold; width: 175px;\">Atribuições:</td>");
                        sb.AppendFormat("<td valign=\"top\" style=\"text-align: justify; text-justify: inter-word;\">{0}</td></tr>", curriculo.experiencias[i].DesAtividades);

                        sb.Append("</tbody></table>");
                        break;
                }
            }

            return sb.ToString();
        }
        #endregion

        #region FormatarDadosComplementares
        /// <summary>
        /// Formatar os dados complementares do candidato
        /// </summary>
        /// <param name="curriculo"></param>
        /// <returns></returns>
        public static string FormatarDadosComplementares(Business.DTO.Curriculo curriculo)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr>");
            sb.Append("<td valign=\"top\" style=\"text-align: left; font-weight: bold;\"><span>Disponível para trabalhar nos períodos:</span>");
            sb.AppendFormat("{0} {1} {2} {3}", curriculo.periodoManha.Value? "Manhã;":"", curriculo.periodoTarde.Value? "Tarde;":"",curriculo.periodoNoite.Value? "Noite;":"", curriculo.periodoFimdeSemana.Value? "Fim de semana;":"");
            sb.Append("</td></tr>");

            sb.AppendFormat("<tr><td>{0}</td></tr>", curriculo.Observacao);

            sb.AppendFormat("<tr><td>{0}</td></tr>", curriculo.TemDeficiencia? Business.Deficiencia.CarregarDescricaoDeficieciaporId(curriculo.idDeficiencia):"");

            return sb.ToString();
        }
        #endregion
    }
}