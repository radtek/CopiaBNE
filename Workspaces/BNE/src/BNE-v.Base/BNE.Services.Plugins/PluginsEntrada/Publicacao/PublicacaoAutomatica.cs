using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using BNE.BLL;
using BNE.BLL.Custom;

namespace BNE.Services.Plugins.PluginsEntrada.Publicacao
{
    internal class PublicacaoAutomatica
    {

        #region ProcessarTextoVaga
        public static bool ProcessarTextoVaga(ref Vaga objVaga, ref string texto, string nomeCampo, string regexPublicacao, string regexFormatacaoVagaCapitalizacao, string regexFormatacaoVagaEspaco, List<RegraPublicacao> regrasPublicacao, IEnumerable<string> listaPalavrasProibidas, ref bool encontrouPalavrasProibidas)
        {

            #region AcaoPublicacao.IdentificarEscolaridade
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.IdentificarEscolaridade))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    Escolaridade objEscolaridade;
                    if (Escolaridade.CarregarPorNome(regraPublicacao.DescricaoPalavraSubstituicao, out objEscolaridade))
                    {
                        if (objVaga.Escolaridade != null)
                            objVaga.Escolaridade.CompleteObject();

                        if (objVaga.Escolaridade == null || (objVaga.Escolaridade != null && objVaga.Escolaridade.SequenciaPeso < objEscolaridade.SequenciaPeso))
                        {
                            objVaga.Escolaridade = objEscolaridade;
                            SalvarHistoricoVaga(objVaga, string.Format("Ajuste na escolaridade para {0}. Identificado escolaridade no texto {1}. Campo {2}.", regraPublicacao.DescricaoPalavraSubstituicao, regraPublicacao.DescricaoPalavraPublicacao, nomeCampo));
                        }
                    }
                }
            }
            #endregion

            #region AcaoPublicacao.IdentificarDeficiencia
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.IdentificarDeficiencia))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    if (objVaga.FlagDeficiencia == null || objVaga.Deficiencia == null)
                    {
                        if (objVaga.Deficiencia == null || (objVaga.Deficiencia != null && objVaga.Deficiencia.IdDeficiencia.Equals(0)))
                            objVaga.Deficiencia = new Deficiencia(7); //Qualquer

                        if (objVaga.FlagDeficiencia == null || (objVaga.FlagDeficiencia != null && objVaga.FlagDeficiencia.Value.Equals(false)))
                            objVaga.FlagDeficiencia = true;

                        SalvarHistoricoVaga(objVaga, string.Format("Ajuste na deficiência. Identificado deficiência no texto {0}. Campo {1}.", regraPublicacao.DescricaoPalavraPublicacao, nomeCampo));
                    }
                }
            }
            #endregion

            #region AcaoPublicacao.BloquearVaga
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.BloquearVaga))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    SalvarHistoricoVaga(objVaga, String.Format("Palavra {0} encontrada. Vaga bloqueada. Campo {1}.", regraPublicacao.DescricaoPalavraPublicacao, nomeCampo));
                    return false; //Retorna falso parando a auditoria.
                }
            }
            #endregion

            #region AcaoPublicacao.SubstituirPalavra
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.SubstituirPalavra))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    texto = regex.Replace(texto, regraPublicacao.DescricaoPalavraSubstituicao);
                    SalvarHistoricoVaga(objVaga, string.Format("Palavra {0} substituida por {1}. Campo {2}.", regraPublicacao.DescricaoPalavraPublicacao, regraPublicacao.DescricaoPalavraSubstituicao, nomeCampo));
                }
            }
            #endregion

            #region AcaoPublicacao.RemoverPalavra
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.RemoverPalavra))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    texto = regex.Replace(texto, String.Empty);
                    SalvarHistoricoVaga(objVaga, string.Format("Palavra {0} removida. Campo {1}.", regraPublicacao.DescricaoPalavraPublicacao, nomeCampo));
                }
            }
            #endregion

            #region AcaoPublicacao.RemoverPalavraProibida
            if (regrasPublicacao.Count(rp => rp.AcaoPublicacao == AcaoPublicacao.RemoverPalavraProibida) > 0) //Se deve aplicar regra para palavra proibida
            {
                foreach (var palavraProibida in listaPalavrasProibidas)
                {
                    var parametrosProibida = new
                    {
                        palavra = palavraProibida
                    };
                    string regexPalavraProibida = parametrosProibida.ToString(regexPublicacao);
                    var regex = new Regex(regexPalavraProibida, RegexOptions.IgnoreCase);
                    if (regex.IsMatch(texto))
                    {
                        texto = regex.Replace(texto, String.Concat(palavraProibida, " #PalavraProibida: ", palavraProibida, " "));
                        SalvarHistoricoVaga(objVaga, string.Format("Palavra proibida {0} encontrada. Campo {1}.", palavraProibida, nomeCampo));
                        encontrouPalavrasProibidas = true;
                    }
                }
            }
            #endregion

            #region AcaoPublicacao.ComplementarAtribuicoesJobFuncao
            if (regrasPublicacao.Count(rp => rp.AcaoPublicacao == AcaoPublicacao.ComplementarAtribuicoesJobFuncao) > 0)
            {
                if (String.IsNullOrEmpty(texto))
                {
                    texto = objVaga.Funcao.RecuperarDescricaoJob();
                    SalvarHistoricoVaga(objVaga, "Acrescentado a JOB, pois o mesmo não foi informado.");
                }
                if (texto.Length < 40)
                {
                    texto = String.Concat(texto, ". ", objVaga.Funcao.RecuperarDescricaoJob());
                    SalvarHistoricoVaga(objVaga, "Acrescentado a JOB, pois o mesmo era menor que 40 caracteres.");
                }
            }
            #endregion

            #region AcaoPublicacao.AplicarFormatacao
            if (regrasPublicacao.Count(rp => rp.AcaoPublicacao == AcaoPublicacao.AplicarFormatacao) > 0) //Se possui regra de formatação setada
            {
                FormataTexto(ref texto, regexFormatacaoVagaCapitalizacao, regexFormatacaoVagaEspaco);
                SalvarHistoricoVaga(objVaga, string.Format("Formatação aplicada. Campo {0}.", nomeCampo));
            }
            #endregion

            #region AcaoPublicacao.CapitalizacaoPalavra
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.CapitalizacaoPalavra))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    texto = regex.Replace(texto, s => s.Value.ToUpper());
                    SalvarHistoricoVaga(objVaga, string.Format("Palavra {0} capitalizada. Campo {1}.", regraPublicacao.DescricaoPalavraPublicacao, nomeCampo));
                }
            }
            #endregion

            return true;
        }
        #endregion

        #region ProcessarTextoCurriculo
        public static bool ProcessarTextoCurriculo(ref Curriculo objCurriculo, ref string texto, string nomeCampo, string regexPublicacao, string regexFormatacaoVagaCapitalizacao, string regexFormatacaoVagaEspaco, List<RegraPublicacao> regrasPublicacao, IEnumerable<string> listaPalavrasProibidas, ref bool encontrouPalavrasProibidas)
        {

            #region AcaoPublicacao.BloquearVaga
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.BloquearVaga))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    SalvarHistoricoCurriculo(objCurriculo, string.Format("Palavra {0} encontrada. Vaga bloqueada. Campo {1}.", regraPublicacao.DescricaoPalavraPublicacao, nomeCampo));
                    return false; //Retorna falso parando a auditoria.
                }
            }
            #endregion

            #region AcaoPublicacao.SubstituirPalavra
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.SubstituirPalavra))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    texto = regex.Replace(texto, regraPublicacao.DescricaoPalavraSubstituicao);
                    SalvarHistoricoCurriculo(objCurriculo, string.Format("Palavra {0} substituida por {1}. Campo {2}.", regraPublicacao.DescricaoPalavraPublicacao, regraPublicacao.DescricaoPalavraSubstituicao, nomeCampo));
                }
            }
            #endregion

            #region AcaoPublicacao.RemoverPalavra
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.RemoverPalavra))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    texto = regex.Replace(texto, String.Empty);
                    SalvarHistoricoCurriculo(objCurriculo, string.Format("Palavra {0} removida. Campo {1}.", regraPublicacao.DescricaoPalavraPublicacao, nomeCampo));
                }
            }
            #endregion

            #region AcaoPublicacao.RemoverPalavraProibida
            if (regrasPublicacao.Count(rp => rp.AcaoPublicacao == AcaoPublicacao.RemoverPalavraProibida) > 0) //Se deve aplicar regra para palavra proibida
            {
                foreach (var palavraProibida in listaPalavrasProibidas)
                {
                    var parametrosProibida = new
                    {
                        palavra = palavraProibida
                    };
                    string regexPalavraProibida = parametrosProibida.ToString(regexPublicacao);
                    var regex = new Regex(regexPalavraProibida, RegexOptions.IgnoreCase);
                    if (regex.IsMatch(texto))
                    {
                        texto = regex.Replace(texto, String.Concat(" #PalavraProibida",palavraProibida,"PalavraProibida#"));
                        SalvarHistoricoCurriculo(objCurriculo, string.Format("Palavra proibida {0} encontrada. Campo {1}.", palavraProibida, nomeCampo));
                        encontrouPalavrasProibidas = true;
                    }
                }
            }
            #endregion

            #region AcaoPublicacao.AplicarFormatacao
            if (regrasPublicacao.Count(rp => rp.AcaoPublicacao == AcaoPublicacao.AplicarFormatacao) > 0) //Se possui regra de formatação setada
            {
                FormataTexto(ref texto, regexFormatacaoVagaCapitalizacao, regexFormatacaoVagaEspaco);
                SalvarHistoricoCurriculo(objCurriculo, string.Format("Formatação aplicada. Campo {0}.", nomeCampo));
            }
            #endregion

            #region AcaoPublicacao.CapitalizacaoPalavra
            foreach (var regraPublicacao in regrasPublicacao.Where(rp => rp.AcaoPublicacao == AcaoPublicacao.CapitalizacaoPalavra))
            {
                var regex = RegraPublicacao.MontarRegex(regexPublicacao, regraPublicacao);
                if (regex.IsMatch(texto)) //Se acha no texto a palavra/regex que coincida com a ocorrência
                {
                    texto = regex.Replace(texto, s => s.Value.ToUpper());
                    SalvarHistoricoCurriculo(objCurriculo, string.Format("Palavra {0} capitalizada. Campo {1}.", regraPublicacao.DescricaoPalavraPublicacao, nomeCampo));
                }
            }
            #endregion

            return true;
        }
        #endregion

        #region FormataTexto
        private static void FormataTexto(ref string texto, string regexFormatacaoCapitalizacao, string regexFormatacaoEspaco)
        {
            var regex = new Regex(regexFormatacaoCapitalizacao, RegexOptions.IgnoreCase);
            texto = regex.Replace(texto.ToLower(), s => s.Value.ToUpper());
            regex = new Regex(regexFormatacaoEspaco, RegexOptions.IgnoreCase);
            texto = regex.Replace(texto, s => String.Concat(s.Value, " "));
        }
        #endregion

        #region SalvarHistoricoVaga
        public static void SalvarHistoricoVaga(BLL.Vaga objVaga, string descricaoHistorico)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga } ,
                    new SqlParameter { ParameterName = "@Des_Historico", SqlDbType = SqlDbType.VarChar, Size = 200, Value = descricaoHistorico }
                };

            #region spsalvarhistorico
            const string spsalvarhistorico = @"
            INSERT INTO BNE_Historico_Publicacao_Vaga (Idf_Vaga, Des_Historico, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Vaga, @Des_Historico, 0, GETDATE());
            ";
            #endregion

            DataAccessLayer.ExecuteNonQueryCmd(CommandType.Text, spsalvarhistorico, parms);
        }
        #endregion

        #region SalvarHistoricoCurriculo
        public static void SalvarHistoricoCurriculo(Curriculo objCurriculo, string descricaoHistorico)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo } ,
                    new SqlParameter { ParameterName = "@Des_Historico", SqlDbType = SqlDbType.VarChar, Size = 200, Value = descricaoHistorico }
                };

            #region spsalvarhistorico
            const string spsalvarhistorico = @"
            INSERT INTO BNE_Historico_Publicacao_Curriculo (Idf_Curriculo, Des_Historico, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Curriculo, @Des_Historico, 0, GETDATE());
            ";
            #endregion

            DataAccessLayer.ExecuteNonQueryCmd(CommandType.Text, spsalvarhistorico, parms);
        }
        #endregion

    }
}
