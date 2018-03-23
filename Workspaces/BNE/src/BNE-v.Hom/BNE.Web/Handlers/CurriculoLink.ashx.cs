using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using BNE.Web.Properties;

namespace BNE.Web.Handlers
{
    /// <summary>
    /// Summary description for CurriculoLink
    /// </summary>
    public class CurriculoLink : IHttpHandler, IReadOnlySessionState
    {

        #region [ Fields / Atributos ]
        private readonly static ConcurrentDictionary<int, Tuple<DateTime, string>> CurriculoLinkCache = new ConcurrentDictionary<int, Tuple<DateTime, string>>();
        private static readonly int MinutosParaLimpezaCache = Settings.Default.PessoaFisicaDelayCurriculoLink;
        private static DateTime _ultimaLimpeza = DateTime.Now;
        private static readonly object Sincronizacao = new object();
        #endregion

        #region ProcessRequest

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int curriculoId;
                if (!ValidRequest(context, out curriculoId))
                    return;

                var thumb = CurriculoLinkCache.GetOrAdd(curriculoId, thumbFactory =>
                {
                    string curriculoLink;
                    Tuple<DateTime, string> res;
                    if (RecuperarLinkCurriculo(thumbFactory, out curriculoLink))
                    {
                        res = new Tuple<DateTime, string>(DateTime.Now, curriculoLink);
                    }
                    else
                    {
                        res = new Tuple<DateTime, string>(DateTime.Now, "");
                    }

                    return res;
                });

                if (!string.IsNullOrWhiteSpace(thumb.Item2))
                {
                    Disponivel(context, thumb.Item2);
                }
                else
                {
                    NaoDisponivel(context);
                }

            }
            finally
            {
                VerificarLimpeza();
            }
        }

        private void VerificarLimpeza()
        {
            if ((DateTime.Now - _ultimaLimpeza).TotalMinutes < MinutosParaLimpezaCache)
                return;

            if (CurriculoLinkCache.Count <= 0)
                return;

            lock (Sincronizacao)
            {
                if ((DateTime.Now - _ultimaLimpeza).TotalMinutes < MinutosParaLimpezaCache)
                    return;

                _ultimaLimpeza = DateTime.Now;
                Task.Factory.StartNew(LimparThumbCache);
            }
        }

        private void LimparThumbCache()
        {
            var toClean = CurriculoLinkCache.Where(keyValuePair => (DateTime.Now - keyValuePair.Value.Item1).TotalMinutes >= MinutosParaLimpezaCache);
            foreach (var keyValuePair in toClean)
            {
                Tuple<DateTime, string> pair;
                CurriculoLinkCache.TryRemove(keyValuePair.Key, out pair);
            }
        }

        private bool ValidRequest(HttpContext context, out int curriculoId)
        {
            if (context.Request.QueryString["targetId"] == null)
            {
                NaoDisponivel(context);
                curriculoId = 0;
                return false;
            }

            var keyId = context.Request.QueryString["targetId"];
            if (!Int32.TryParse(keyId.ToString(CultureInfo.InvariantCulture), out curriculoId))
            {
                NaoDisponivel(context);
                return false;
            }

            if (curriculoId <= 0)
            {
                NaoDisponivel(context);
                return false;
            }

            if (context.Session == null)
            {
                NaoDisponivel(context);
                return false;
            }

            if (context.Session[typeof(SessionVariable<int>) + Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()] ==
                null)
            {
                if (context.Session[
                    typeof(SessionVariable<int>) + Chave.Permanente.IdUsuarioFilialPerfilLogadoUsuarioInterno.ToString()] ==
                    null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion

        #region NaoDisponivel
        private void NaoDisponivel(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("");
        }
        #endregion

        #region Disponivel
        private void Disponivel(HttpContext context, string linkFile)
        {
            context.Response.Redirect(linkFile);
        }
        #endregion

        #region RecuperarFoto
        public static bool RecuperarLinkCurriculo(int curriculoId, out string link)
        {
            if (!RecuperarLinkBaseDeDados(curriculoId, out link))
                return false;

            if (string.IsNullOrWhiteSpace(link))
                return false;

            return true;
        }

        public static bool RecuperarLinkBaseDeDados(int curriculoId, out string link)
        {
            try
            {
                var curriculoDadosSitemap = Curriculo.RecuperarCurriculoSitemap(new Curriculo(curriculoId));
                link = SitemapHelper.MontarUrlVisualizacaoCurriculo(curriculoDadosSitemap.DescricaoFuncao, curriculoDadosSitemap.NomeCidade, curriculoDadosSitemap.SiglaEstado, (int)curriculoDadosSitemap.IdfCurriculo);
                return true;
            }
            catch (Exception ex)
            {
                link = "";
                EL.GerenciadorException.GravarExcecao(ex, string.Format("CurrículoId='{0}'", curriculoId));
                return false;
            }
        }
        #endregion
    }
}