using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using BNE.Web.Properties;
using Image = System.Drawing.Image;

namespace BNE.Web.Handlers
{
    public class CurriculoThumbFotoParaSelecionadora : IHttpHandler, IReadOnlySessionState
    {
        #region [ Fields / Atributos ]
        private readonly static ConcurrentDictionary<int, Tuple<DateTime, byte[]>> ThumbCache = new ConcurrentDictionary<int, Tuple<DateTime, byte[]>>();
        private static readonly int MinutosParaLimpezaCache = Settings.Default.PessoaFisicaFotoDelayLimparCacheMinutos;
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

                var thumb = ThumbCache.GetOrAdd(curriculoId, thumbFactory =>
                {
                    byte[] image;
                    Tuple<DateTime, byte[]> res;
                    if (RecuperarFotoThumb(thumbFactory, out image))
                    {
                        res = new Tuple<DateTime, byte[]>(DateTime.Now, image);
                    }
                    else
                    {
                        res = new Tuple<DateTime, byte[]>(DateTime.Now, null);
                    }

                    return res;
                });

                if (thumb.Item2 != null)
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

            if (ThumbCache.Count <= 0)
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
            var toClean = ThumbCache.Where(keyValuePair => (DateTime.Now - keyValuePair.Value.Item1).TotalMinutes >= MinutosParaLimpezaCache);
            foreach (var keyValuePair in toClean)
            {
                Tuple<DateTime, byte[]> pair;
                ThumbCache.TryRemove(keyValuePair.Key, out pair);
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
            context.Response.Redirect("~/img/img_sem_foto.gif", false);
        }
        #endregion

        #region Disponivel
        private void Disponivel(HttpContext context, byte[] contentFile)
        {
            using (var b = new Bitmap(new MemoryStream(contentFile)))
            {
                context.Response.ContentType = "image/jpeg";
                b.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            }
        }
        #endregion

        #region RecuperarFoto
        public static bool RecuperarFotoThumb(int curriculoId, out byte[] imageArray)
        {
            if (!RecuperarFotoBaseDeDados(curriculoId, out imageArray))
                return false;

            if (imageArray == null || imageArray.Length <= 0)
                return false;

            try
            {
                using (var thumbStream = new MemoryStream())
                using (var originalStream = new MemoryStream(imageArray))
                using (var originalImage = Image.FromStream(originalStream))
                using (var thumb = originalImage.GetThumbnailImage(32, 32, () => false, IntPtr.Zero))
                {
                    thumb.Save(thumbStream, ImageFormat.Jpeg);
                    imageArray = thumbStream.GetBuffer();
                }
                return true;
            }
            catch (Exception ex)
            {
                imageArray = new byte[0];
                EL.GerenciadorException.GravarExcecao(ex, string.Format("CurrículoId='{0}'", curriculoId));
                return false;
            }
        }

        public static bool RecuperarFotoBaseDeDados(int curriculoId, out byte[] imageArray)
        {
            try
            {
                imageArray = BLL.PessoaFisicaFoto.RecuperarFotoPorCurriculoId(curriculoId);
                return true;
            }
            catch (Exception ex)
            {
                imageArray = new byte[0];
                EL.GerenciadorException.GravarExcecao(ex, string.Format("CurrículoId='{0}'", curriculoId));
                return false;
            }
        }
        #endregion

    }
}