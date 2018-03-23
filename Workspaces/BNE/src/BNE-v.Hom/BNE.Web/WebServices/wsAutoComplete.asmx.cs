using BNE.BLL;
using BNE.Web.Code;
using System;
using System.Web.Script.Services;
using System.Web.Services;

namespace BNE.Web.WebServices
{
    /// <summary>
    /// Summary description for wsAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class wsAutoComplete : WebService
    {
        readonly string[] _erro = null;

        #region ListarCidadesSiglaEstadoPorNomeParcialEstado
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarCidadesSiglaEstadoPorNomeParcialEstado(string prefixText, int count, string contextKey)
        {
            try
            {
                return UIHelper.RecuperarItensAutoComplete(Cidade.RecuperarNomesCidadesEstado(prefixText, contextKey, count));
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return _erro;
            }
        }
        #endregion

        #region ListarFuncoesPorArea
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarFuncoesPorArea(string prefixText, int count, string contextKey)
        {
            try
            {
                int? origem = null;
                int? areaBNE = null;
                if (!String.IsNullOrEmpty(contextKey))
                {
                    string[] keys = contextKey.Split(new[] { ';' });

                    int idOrigem;
                    if (Int32.TryParse(keys[0], out idOrigem))
                        origem = idOrigem;

                    if (keys.Length > 1)
                    {
                        int idArea;
                        if (Int32.TryParse(keys[1], out idArea))
                            areaBNE = idArea;
                    }
                }

                return Funcao.RecuperarFuncoesPorArea(prefixText, count, origem, areaBNE);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return _erro;
            }
        }
        #endregion

        #region ListarSiglaNomeFonte
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarSiglaNomeFonte(string prefixText, int count)
        {
            try
            {
                return UIHelper.RecuperarItensAutoComplete(Fonte.RecuperarSiglaNomeFonte(prefixText, count));
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return _erro;
            }
        }
        #endregion

        #region ListarCursoFonte
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarCursoFonte(string prefixText, string contextKey, int count)
        {
            try
            {
                if (!String.IsNullOrEmpty(contextKey))
                {
                    string[] fonteNivelCurso = contextKey.Split(',');
                    if (fonteNivelCurso.Length > 1) //[0] - IdFonte [1] - IdNivelCurso
                    {
                        //Se for uma fonte personalizada
                        if (Convert.ToInt32(fonteNivelCurso[1]).Equals(0))
                            return CursoFonte.RecuperarCursoFonte(prefixText, null, count, Convert.ToInt32(fonteNivelCurso[0]));
                        return CursoFonte.RecuperarCursoFonte(prefixText, Convert.ToInt32(fonteNivelCurso[1]), count, Convert.ToInt32(fonteNivelCurso[0]));
                    }
                    return CursoFonte.RecuperarCursoFonte(prefixText, Convert.ToInt32(contextKey), count);
                }
                return Curso.RecuperarCursos(prefixText, count);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return _erro;
            }
        }
        #endregion

        #region ListarCursoNivel3
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarCursoNivel3(string prefixText, string contextKey, int count)
        {
            try
            {
                return Curso.RecuperarCursos(prefixText, count, false);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return _erro;
            }
        }
        #endregion

        #region ListarCursoNivel4
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarCursoNivel4(string prefixText, string contextKey, int count)
        {
            try
            {
                return Curso.RecuperarCursos(prefixText, count, true);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return _erro;
            }
        }
        #endregion

        #region ListarSiglaNomeFonteNivelCurso
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarSiglaNomeFonteNivelCurso(string prefixText, string contextKey, int count)
        {
            try
            {
                int? idNivelFonte = null;
                if (!string.IsNullOrEmpty(contextKey))
                    idNivelFonte = Convert.ToInt32(contextKey);

                return Fonte.RecuperarSiglaNomeFonteNivelCurso(prefixText.Replace("'", string.Empty), idNivelFonte, count);
            }
            catch (Exception ex)
            {
                string message = "A sentença informada foi: " + prefixText;
                EL.GerenciadorException.GravarExcecao(ex, message);
                return _erro;
            }
        }
        #endregion

        #region ListarFiliaisEmployer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarFiliaisEmployer(string prefixText, int count)
        {
            try
            {
                return Filial.ListarFiliaisEmployerAutoComplete(prefixText, count).ToArray();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return _erro;
            }
        }
        #endregion

        #region ListarFuncoesComId
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarFuncoesComId(string prefixText, int count, string contextKey)
        {
            try
            {
                int? origem = null;

                if (!String.IsNullOrEmpty(contextKey))
                    origem = Int32.Parse(contextKey);

                return UIHelper.RecuperarItensAutoComplete(Funcao.RecuperarFuncoesComId(prefixText, count, origem));
            }
            catch (Exception ex)
            {
                string message = "A sentença informada foi: " + prefixText;
                EL.GerenciadorException.GravarExcecao(ex, message);
                return _erro;
            }
        }
        #endregion

        #region ListarFiliaisPorNomeFantasia
        /// <summary>
        /// Retorna lista de filiais por nome fantasia
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarFiliaisPorNomeFantasia(string prefixText, int count, string contextKey)
        {
            try
            {
                return UIHelper.RecuperarItensAutoComplete(Filial.ListarFiliaisPorNomeFantasia(prefixText, count));
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return _erro;
            }
        }
        #endregion

        #region ListarSugestaoEmail
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarSugestaoEmail(string prefixText, int count)
        {
            try
            {
                return DeParaEmail.RecuperarAutocomplete(prefixText, count);
            }
            catch (Exception ex)
            {
                string message = "A sentença informada foi: " + prefixText;
                EL.GerenciadorException.GravarExcecao(ex, message);
                return _erro;
            }
        }
        #endregion

        #region ListarUsuariosAdm
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText">Texto a ser procurado</param>
        /// <param name="count">Quantidade de Resultados</param>
        /// <param name="contextKey">Chave de Contexto</param>
        /// <returns></returns>
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarUsuarios(string prefixText, int count, string contextKey)
        {
            try
            {
                int? origem = null;

                if (!String.IsNullOrEmpty(contextKey))
                    origem = Int32.Parse(contextKey);

                return UsuarioFilialPerfil.RecuperarUsuariosPorNome(null, prefixText, count, origem);
            }
            catch (Exception ex)
            {
                string message = "A sentença informada foi: " + prefixText;
                EL.GerenciadorException.GravarExcecao(ex, message);
                return _erro;
            }
        }
        #endregion

        #region ListarSugestEmailUsuarioFilial
        [WebMethod(EnableSession = false), ScriptMethod]
        public string[] ListarSugestEmailUsuarioFilial(string prefixText, int count, string contextKey)
        {
            try
            {
                if (!String.IsNullOrEmpty(contextKey))
                {
                    string[] pesqui = prefixText.Split(';', ':', ',');
                    if (pesqui[pesqui.Length - 1].Length > 2)
                        return UIHelper.RecuperarItensAutoComplete(EmailEnvioCurriculo.ListarSugestEmailUsuarioFilial(pesqui[pesqui.Length - 1].Trim(), count, Convert.ToInt32(contextKey)));
                }

                return null;
            }
            catch (Exception ex)
            {
                string message = "A sentença informada foi: " + prefixText;
                EL.GerenciadorException.GravarExcecao(ex, message);
                return _erro;
            }
        }
        #endregion

    }
}
