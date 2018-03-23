//-- Data: 15/02/2013 10:12
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BNE.BLL.Custom;

namespace BNE.BLL
{
    public partial class CartaEmail // Tabela: BNE_Carta_Email
    {

        #region ListarCartas
        /// <summary>
        /// Lista as cartas através de uma lista de ids
        /// </summary>
        /// <param name="idsCartas"></param>
        /// <returns>Dicionario de conteudos</returns>
        private static Dictionary<Enumeradores.CartaEmail, DTO.CartaEmail> ListarCartas(List<Enumeradores.CartaEmail> idsCartas)
        {
            return ListarCartas(idsCartas, null);
        }
        private static Dictionary<Enumeradores.CartaEmail, DTO.CartaEmail> ListarCartas(List<Enumeradores.CartaEmail> idsCartas, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>();
            var itensConteudos = new Dictionary<Enumeradores.CartaEmail, DTO.CartaEmail>();

            string query = "SELECT Idf_Carta_Email, Vlr_Carta_Email, Des_Assunto FROM BNE_Carta_Email WITH(NOLOCK) WHERE Idf_Carta_Email IN (";

            for (int i = 0; i < idsCartas.Count; i++)
            {
                string nomeConteudo = "@parm" + i;

                if (i > 0)
                    query += ", ";

                query += nomeConteudo;
                parms.Add(new SqlParameter(nomeConteudo, SqlDbType.Int, 4));
                parms[i].Value = Convert.ToInt32(idsCartas[i]);
            }

            query += ")";

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, query, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms);

            while (dr.Read())
            {
                var carta = (Enumeradores.CartaEmail)Enum.Parse(typeof(Enumeradores.CartaEmail), dr["Idf_Carta_Email"].ToString());
                itensConteudos.Add(carta, new DTO.CartaEmail { Conteudo = Convert.ToString(dr["Vlr_Carta_Email"]), Assunto = Convert.ToString(dr["Des_Assunto"]) });
            }
            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return itensConteudos;
        }
        #endregion

        #region RetornarConteudoEmpresa
        /// <summary>
        /// Retorna a carta desejada com o Layout da Empresa
        /// </summary>
        /// <param name="enumerador">Enumerador referente ao conteudo da carta</param>
        /// <param name="nomeEmpresa">Nome da Eliente</param>
        /// <param name="assunto">Assunto do E-mail</param>
        /// <param name="numeroCNPJ">CNPJ da Empresa</param>
        /// <returns></returns>
        public static String RetornarConteudoEmpresa(Enumeradores.CartaEmail enumerador, decimal numeroCNPJ, string nomeEmpresa, out string assunto)
        {
            var parametros = new
            {
                NumeroCNPJ = numeroCNPJ,
                NomeEmpresa = nomeEmpresa
            };
            return parametros.ToString(MontarConteudoEmpresa(Enumeradores.CartaEmail.ConteudoPadraoEmpresa, null, enumerador, out assunto));
        }
        /// <param name="enumerador">Enumerador referente ao conteudo da carta </param>
        /// <param name="conteudo">Conteúdo da carta</param>
        /// <param name="numeroCNPJ">CNPJ da Empresa</param>
        /// <param name="nomeEmpresa">Nome da Eliente</param>
        /// <param name="assunto">Assunto do E-mail</param>
        public static String RetornarConteudoEmpresa(Enumeradores.CartaEmail enumerador, string conteudo, decimal numeroCNPJ, string nomeEmpresa, out string assunto)
        {
            var parametros = new
            {
                NumeroCNPJ = numeroCNPJ,
                NomeEmpresa = nomeEmpresa
            };
            return parametros.ToString(MontarConteudoEmpresa(Enumeradores.CartaEmail.ConteudoPadraoEmpresa, conteudo, enumerador, out assunto));
        }
        #endregion

        #region RetornarConteudoBNE
        /// <summary>
        /// Retorna a carta desejada com o Layout do BNE
        /// </summary>
        /// <param name="enumerador"></param>
        /// <param name="assunto"></param>
        /// <returns></returns>
        public static String RetornarConteudoBNE(Enumeradores.CartaEmail enumerador, out string assunto)
        {
            return MontarConteudo(null, enumerador, true, out assunto);
        }
        public static String RetornarConteudoBNE(string conteudo, out string assunto)
        {
            return MontarConteudo(conteudo, null, false, out assunto);
        }
        #endregion

        #region RetornarConteudoEmpresa
        /// <summary>
        /// Retorna a carta desejada com o Layout da Empresa
        /// </summary>
        /// <param name="enumerador">Enumerador referente ao conteudo da carta</param>
        /// <param name="nomeEmpresa">Nome da Eliente</param>
        /// <param name="assunto">Assunto do E-mail</param>
        /// <param name="numeroCNPJ">CNPJ da Empresa</param>
        /// <returns></returns>
        public static String RetornarConteudoVIPUniversitario(Enumeradores.CartaEmail enumerador, decimal numeroCNPJ, string nomeEmpresa, out string assunto)
        {
            var parametros = new
            {
                NumeroCNPJ = numeroCNPJ,
                NomeEmpresa = nomeEmpresa
            };
            return parametros.ToString(MontarConteudoEmpresa(Enumeradores.CartaEmail.ConteudoPadraoVIPUniversitario, null, enumerador, out assunto));
        }
        /// <param name="enumerador">Enumerador referente ao conteudo da carta </param>
        /// <param name="conteudo">Conteúdo da carta</param>
        /// <param name="numeroCNPJ">CNPJ da Empresa</param>
        /// <param name="nomeEmpresa">Nome da Eliente</param>
        /// <param name="assunto">Assunto do E-mail</param>
        public static String RetornarConteudoVIPUniversitario(Enumeradores.CartaEmail enumerador, string conteudo, decimal numeroCNPJ, string nomeEmpresa, out string assunto)
        {
            var parametros = new
            {
                NumeroCNPJ = numeroCNPJ,
                NomeEmpresa = nomeEmpresa
            };
            return parametros.ToString(MontarConteudoEmpresa(Enumeradores.CartaEmail.ConteudoPadraoVIPUniversitario, conteudo, enumerador, out assunto));
        }
        #endregion

        #region MontarConteudoEmpresa
        private static String MontarConteudoEmpresa(Enumeradores.CartaEmail padrao, string texto, Enumeradores.CartaEmail? enumerador, out string assunto)
        {
            assunto = string.Empty;

            var conteudos = new List<Enumeradores.CartaEmail>
                {
                    padrao
                };

            if (enumerador.HasValue)
                conteudos.Add(enumerador.Value);

            var valoresConteudos = ListarCartas(conteudos);

            if (enumerador.HasValue)
            {
                assunto = valoresConteudos[enumerador.Value].Assunto;
                if (string.IsNullOrEmpty(texto)) //Se não foi passado o texto, então recupera do enumerador
                    texto = valoresConteudos[enumerador.Value].Conteudo;
            }

            var parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.URLAmbiente,
                    Enumeradores.Parametro.URLImagens
                };

            var valoresParametros = Parametro.ListarParametros(parametros);

            var parametrosTemplate = new
            {
                UrlSite = string.Concat("http://", valoresParametros[Enumeradores.Parametro.URLAmbiente]),
                UrlImagens = valoresParametros[Enumeradores.Parametro.URLImagens],
                Conteudo = texto
            };

            return parametrosTemplate.ToString(valoresConteudos[padrao].Conteudo);
        }
        #endregion

        #region MontarConteudo
        private static String MontarConteudo(string texto, Enumeradores.CartaEmail? enumerador, bool adicionarRodape, out string assunto)
        {
            assunto = string.Empty;

            var conteudos = new List<Enumeradores.CartaEmail>
                {
                    Enumeradores.CartaEmail.ConteudoPadraoBNE
                };

            if (enumerador.HasValue)
                conteudos.Add(enumerador.Value);

            if (adicionarRodape)
                conteudos.Add(Enumeradores.CartaEmail.ConteudoPadraoBNERodape);

            var valoresConteudos = ListarCartas(conteudos);

            if (enumerador.HasValue)
            {
                assunto = valoresConteudos[enumerador.Value].Assunto;
                if (string.IsNullOrEmpty(texto)) //Se não foi passado o texto, então recupera do enumerador
                    texto = valoresConteudos[enumerador.Value].Conteudo;
            }

            string rodape = string.Empty;

            if (adicionarRodape)
                rodape = valoresConteudos[Enumeradores.CartaEmail.ConteudoPadraoBNERodape].Conteudo;

            var parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.URLAmbiente,
                    Enumeradores.Parametro.URLImagens
                };

            var valoresParametros = Parametro.ListarParametros(parametros);

            var parametrosTemplate = new
            {
                UrlSite = string.Concat("http://", valoresParametros[Enumeradores.Parametro.URLAmbiente]),
                UrlImagens = valoresParametros[Enumeradores.Parametro.URLImagens],
                Conteudo = texto,
                Rodape = rodape
            };

            return parametrosTemplate.ToString(valoresConteudos[Enumeradores.CartaEmail.ConteudoPadraoBNE].Conteudo);
        }
        #endregion

        #region MontarExtratoVaga
        public static string MontarExtratoVaga(DTO.Vaga objVaga)
        {
            string urlSite = string.Concat("http://", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente));
            string urlImagens = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLImagens);

            return MontarExtratoVaga(objVaga, urlSite, urlImagens);
        }
        private static string MontarExtratoVaga(DTO.Vaga objVaga, string urlSite, string urlImagens)
        {
            var sb = new StringBuilder();
            sb.Append("<table align='center' cellpadding='0' cellspacing='0' style='line-height: 98%;' width='540'>");
            sb.AppendFormat("<tr><td background='{0}/templateemail/img_top_box_vaga.png' bgcolor='#DDF0F8' height='12'><font face='Arial, Helvetica, sans-serif' size='1'>&nbsp;</font></td></tr>", urlImagens);

            MontarMensagemRastreadorVagaConteudo(objVaga, sb);

            sb.AppendFormat("<tr><td background='{0}/templateemail/img_bottom_box_vaga.png' bgcolor='#DDF0F8' height='12'><font face='Arial, Helvetica, sans-serif' size='1'>&nbsp;</font></td></tr>", urlImagens);
            sb.Append("</table>");

            return sb.ToString();
        }
        private static void MontarMensagemRastreadorVagaConteudo(DTO.Vaga objVaga, StringBuilder sb)
        {
            sb.Append("<tr><td bgcolor='#DDF0F8'><table align='center' cellpadding='0' cellspacing='0' width='500'><tr><td><font size='2' face='Arial, Helvetica, sans-serif'><div>");

            #region Função e Quantidade de Vagas
            sb.AppendFormat("<b>{0} ({1} {2})</b><br>", objVaga.DescricaoFuncao, objVaga.QuantidadeVaga, objVaga.QuantidadeVaga.Equals(1) ? "vaga" : "vagas");
            #endregion

            #region Salário e Cidade
            string salario = objVaga.ValorSalarioInicial.HasValue && objVaga.ValorSalarioFinal.HasValue
                                 ? string.Format("{0} a {1}", objVaga.ValorSalarioInicial.Value.ToString("C"), objVaga.ValorSalarioFinal.Value.ToString("C"))
                                 : "A combinar";

            sb.AppendFormat("{0} - {1}/{2}<br>", salario, objVaga.NomeCidade, objVaga.SiglaEstado);
            #endregion

            #region Atribuições
            if (!string.IsNullOrEmpty(objVaga.DescricaoAtribuicoes))
                sb.AppendFormat("<b>Atribuições:</b> {0}<br>", objVaga.DescricaoAtribuicoes);
            #endregion

            #region Benefícios
            if (!string.IsNullOrEmpty(objVaga.DescricaoBeneficio))
                sb.AppendFormat("<b>Benefícios:</b> {0}<br>", objVaga.DescricaoBeneficio);
            #endregion

            #region Requisitos
            string descricao = MontarMensagemRastreadorVagaConteudoRequisito(objVaga);
            if (!string.IsNullOrEmpty(descricao))
                sb.AppendFormat("<b>Requisitos:</b> {0}<br>", descricao);
            #endregion

            #region Disponibilidade de Trabalho
            if (!string.IsNullOrEmpty(objVaga.DescricaoDisponibilidade))
                sb.AppendFormat("<b>Disponibilidade de Trabalho:</b> {0}<br>", objVaga.DescricaoDisponibilidade);
            #endregion

            #region Tipo de Vínculo
            if (!string.IsNullOrEmpty(objVaga.DescricaoTipoVinculo))
                sb.AppendFormat("<b>Tipo de Contrato:</b> {0}<br>", objVaga.DescricaoTipoVinculo);
            #endregion

            sb.Append("</div></font></td></tr></table></td></tr>");
        }
        private static string MontarMensagemRastreadorVagaConteudoRequisito(DTO.Vaga objVaga)
        {
            string descricaoRequisito = null;

            if (!string.IsNullOrEmpty(objVaga.DescricaoEscolaridade))
                descricaoRequisito = string.Format("{0}<br>", objVaga.DescricaoEscolaridade);

            if (!string.IsNullOrEmpty(objVaga.DescricaoRequisito))
                descricaoRequisito += string.Format("{0}", objVaga.DescricaoRequisito);

            return descricaoRequisito;
        }
        #endregion

        #region RecuperarConteudo
        public static string RecuperarConteudo(Enumeradores.CartaEmail carta)
        {
            return RecuperarConteudo(carta, null);
        }
        public static string RecuperarConteudo(Enumeradores.CartaEmail carta, SqlTransaction trans)
        {
            var conteudos = new List<Enumeradores.CartaEmail>
                {
                    carta
                };

            var valoresConteudos = ListarCartas(conteudos, trans);

            return valoresConteudos[carta].Conteudo;
        }
        #endregion

        #region RecuperarCarta
        public static DTO.CartaEmail RecuperarCarta(Enumeradores.CartaEmail carta)
        {
            return RecuperarCarta(carta, null);
        }
        public static DTO.CartaEmail RecuperarCarta(Enumeradores.CartaEmail carta, SqlTransaction trans)
        {
            var valoresConteudos = ListarCartas(new List<Enumeradores.CartaEmail> { carta }, trans);
            return valoresConteudos[carta];
        }
        #endregion
    }
}