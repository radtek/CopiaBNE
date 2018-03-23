using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text;
using BNE.CLR.Custom;
using Microsoft.SqlServer.Server;

namespace BNE.CLR
{

    public class CLR
    {

        #region MontarUrlCurriculoVisualizacao
        /// <summary>
        /// Padronização da url de currículo para visualizacao
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="identificadorCurriculo"></param>
        /// <returns></returns>
        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlString MontarUrlCurriculoVisualizacao(SqlString nomeFuncao, SqlString nomeCidade, SqlString siglaEstado, SqlInt32 identificadorCurriculo)
        {
            using (var conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                object urlRota;

                using (var cmd = new SqlCommand("SELECT Des_URL FROM BNE_Rota R WITH(NOLOCK) WHERE R.Idf_Rota = @Idf_Rota", conn))
                {
                    var parm = new SqlParameter { ParameterName = "@Idf_Rota", SqlDbType = SqlDbType.Int, Value = 11 };
                    cmd.Parameters.Add(parm);

                    urlRota = cmd.ExecuteScalar();

                    cmd.Parameters.Clear();
                }

                object urlSite;

                using (var cmd = new SqlCommand("SELECT Vlr_Parametro FROM plataforma.TAB_Parametro P WITH(NOLOCK) WHERE P.Idf_Parametro = @Idf_Parametro", conn))
                {
                    var parm = new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Value = 156 };
                    cmd.Parameters.Add(parm);

                    urlSite = cmd.ExecuteScalar();

                    cmd.Parameters.Clear();
                }

                if (urlRota != null && urlSite != null)
                {
                    var parametros = new
                    {
                        URLSite = string.Concat("http://", urlSite),
                        Funcao = NormalizarURL(nomeFuncao.ToString()),
                        Cidade = NormalizarURL(nomeCidade.ToString()),
                        SiglaEstado = siglaEstado.ToString().ToLower(),
                        Identificador = identificadorCurriculo
                    };

                    return FormatObject.ToString(parametros, string.Concat("{URLSite}/", urlRota));
                }
            }
            return string.Empty;
        }
        #endregion

        #region MontarUrlCurriculo
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="identificadorCurriculo"></param>
        /// <returns></returns>
        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlString MontarUrlCurriculo(SqlString nomeFuncao, SqlString nomeCidade, SqlString siglaEstado, SqlInt32 identificadorCurriculo)
        {
            using (var conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                object urlRota;

                using (var cmd = new SqlCommand("SELECT Des_URL FROM BNE_Rota R WITH(NOLOCK) WHERE R.Idf_Rota = @Idf_Rota", conn))
                {
                    var parm = new SqlParameter { ParameterName = "@Idf_Rota", SqlDbType = SqlDbType.Int, Value = 11 };
                    cmd.Parameters.Add(parm);

                    urlRota = cmd.ExecuteScalar();

                    cmd.Parameters.Clear();
                }

                object urlSite;

                using (var cmd = new SqlCommand("SELECT Vlr_Parametro FROM plataforma.TAB_Parametro P WITH(NOLOCK) WHERE P.Idf_Parametro = @Idf_Parametro", conn))
                {
                    var parm = new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Value = 156 };
                    cmd.Parameters.Add(parm);

                    urlSite = cmd.ExecuteScalar();

                    cmd.Parameters.Clear();
                }

                if (urlRota != null && urlSite != null)
                {
                    var parametros = new
                    {
                        URLSite = string.Concat("http://",urlSite),
                        Funcao = NormalizarURL(nomeFuncao.ToString()),
                        Cidade = NormalizarURL(nomeCidade.ToString()), 
                        SiglaEstado = siglaEstado.ToString().ToLower(),
                        Identificador = identificadorCurriculo
                    };

                    return FormatObject.ToString(parametros, string.Concat("{URLSite}/", urlRota));
                }
            }
            return string.Empty;
        }
        #endregion

        #region MontarUrlVaga
        /// <summary>
        /// Padronização da url de vaga
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeAreaBNE"> </param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="identificadorVaga"></param>
        /// <returns></returns>
        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlString MontarUrlVaga(SqlString nomeFuncao, SqlString nomeAreaBNE, SqlString nomeCidade, SqlString siglaEstado, SqlInt32 identificadorVaga)
        {
            using (var conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                object urlRota;

                using (var cmd = new SqlCommand("SELECT Des_URL FROM bne.BNE_Rota R WITH(NOLOCK) WHERE R.Idf_Rota = @Idf_Rota", conn))
                {
                    var parm = new SqlParameter { ParameterName = "@Idf_Rota", SqlDbType = SqlDbType.Int, Value = 7 };
                    cmd.Parameters.Add(parm);

                    urlRota = cmd.ExecuteScalar();

                    cmd.Parameters.Clear();
                }

                object urlSite;

                using (var cmd = new SqlCommand("SELECT Vlr_Parametro FROM plataforma.TAB_Parametro P WITH(NOLOCK) WHERE P.Idf_Parametro = @Idf_Parametro", conn))
                {
                    var parm = new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Value = 156 };
                    cmd.Parameters.Add(parm);

                    urlSite = cmd.ExecuteScalar();

                    cmd.Parameters.Clear();
                }

                if (urlRota != null && urlSite != null)
                {
                    var parametros = new
                    {
                        URLSite = string.Concat("http://",urlSite),
                        Funcao = NormalizarURL(nomeFuncao.ToString()),
                        AreaBNE = NormalizarURL(nomeAreaBNE.ToString()),
                        Cidade = NormalizarURL(nomeCidade.ToString()),
                        SiglaEstado = siglaEstado.ToString().ToLower(),
                        Identificador = identificadorVaga
                    };

                    return FormatObject.ToString(parametros, string.Concat("{URLSite}/", urlRota));
                }
            }
            return string.Empty;
        }
        #endregion

        #region NormalizarURL
        /// <summary>
        /// Extension method de string para padronizar as palavaras de URL
        /// </summary>
        /// <param name="s">Texto que será manipulado</param>
        /// <returns></returns>
        public static string NormalizarURL(string s)
        {
            s = RemoverAcentos(s);
            s = s.ToLower();
            s = s.Replace(' ', '-');
            s = s.Replace('.', '_');

            return s;
        }
        #endregion

        #region RemoverAcentos
        /// <summary>
        /// Método que remove os acentos de uma string
        /// </summary>
        /// <param name="texto">Texto a ser removido a acentuação</param>
        /// <returns>String sem acentos</returns>
        public static string RemoverAcentos(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char t in s)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }
            return sb.ToString();
        }
        #endregion

    }
}
