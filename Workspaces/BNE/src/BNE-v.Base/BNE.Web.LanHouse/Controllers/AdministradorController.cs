using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BNE.Web.LanHouse.BLL;
using System.Web;
using System.IO;
using System.Drawing;

namespace BNE.Web.LanHouse.Controllers
{
    public class AdministradorController : Controller//Code.AbstractController
    {
        private Code.ICacheService cache = new Code.InMemoryCache();

        //
        // GET: /Administrador/

        public ActionResult Manutencao()
        {
            return View();
        }

        #region Companhias
        [HttpGet]
        [ActionName("c")]
        public JsonResult Companhias(string sidx, string sord, int page, int rows)
        {
            var companhias = Companhia.SelectAll();

            var data = new
            {
                total = (int)Math.Ceiling((float)companhias.Count / 5),
                page = page,
                records = companhias.Count,
                rows = companhias.Select(c => new { id = c.Idf_Companhia, CNPJCompanhia = c.Num_CNPJ, Nome = c.Nme_Companhia, Logo = "/Home/LogoFilial/" + c.Num_CNPJ }).Skip((page - 1) * rows).Take(page * rows)
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion BuscarFilial

        #region Métodos públicos
        [ActionName("s")]
        public ActionResult Salvar(Models.ModelAjaxCompanhia model)
        {
            if (ModelState.IsValid)
            {
                if (Companhia.Salvar(model.IdCompanhia, model.NumeroCNPJ, model.NomeCompanhia, RecuperarLogo(model.Logo)))
                {
                    cache.Clear(String.Format("logo.{0}", model.NumeroCNPJ));
                    return Json(true);
                }
                return Json(false);
            }
            return Json(false);
        }

        [HttpGet]
        [ActionName("e")]
        public ActionResult Editar(int id)
        {
            var companhia = Companhia.SelectByID(id);

            return Json(new { cnpj = companhia.Num_CNPJ, nome = companhia.Nme_Companhia }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Upload()
        {
            Guid guidArquivo = Guid.NewGuid();

            string nomeArquivo = Request.Files[0].FileName;

            DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath("~/logos"));

            if (!directoryInfo.Exists)
                directoryInfo.Create();

            string local = directoryInfo.FullName + "\\" + guidArquivo.ToString() + Path.GetExtension(nomeArquivo);

            Request.Files[0].SaveAs(local);

            string urlImagem = Path.Combine("~/logos", guidArquivo.ToString() + Path.GetExtension(nomeArquivo)).Replace("\\", "/").Replace("~", "");

            return Json(new { url = urlImagem }, "text/plain");
        }

        public byte[] RecuperarLogo(string nomeArquivo)
        {
            string local = Server.MapPath(Path.Combine("~/logos", nomeArquivo));

            string extensao = Path.GetExtension(nomeArquivo);

            // Imagem original
            Image logo = Image.FromFile(local);

            MemoryStream ms = new MemoryStream();
            logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return ms.ToArray();
        }



        #endregion Métodos públicos

    }
}
