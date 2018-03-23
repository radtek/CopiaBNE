using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BNE.Web.LanHouse.BLL;
using BNE.Web.LanHouse.Models;

namespace BNE.Web.LanHouse.Controllers
{
    public class HomeController : Controller
    {
        private Code.ICacheService cache = new Code.InMemoryCache();

        [Code.EntrouOrigem]
        //[Code.RequireHttpsOnProduction]
        public ActionResult Index()
        {
            var model = new ModelHomeIndex
            {
                ValorVIP = new BNE.BLL.Plano(Parametro.IdentificadorPlanoVIP()).RecuperarValor()
            };
            return View("Index", model);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Layout()
        {
            return View();
        }

        #region LogoFilial
        public ActionResult LogoFilial(string id)
        {
            string cnpj = id;

            Image objImagem = cache.Get(String.Format("logo.{0}", cnpj), () =>
            {
                decimal cnpjDecimal = Convert.ToDecimal(cnpj);

                Image retorno;
                var logo = Companhia.RecuperarFoto(cnpjDecimal);
                if (logo == null || !logo.Any())
                    retorno = Image.FromFile(Server.MapPath("~/Content/img/logo_vazio.png"));
                else
                {
                    using (var mslogo = new MemoryStream(logo, 0, logo.Length))
                    {
                        retorno = Image.FromStream(mslogo, true);
                    }
                }

                return retorno;
            });

            var ms = new MemoryStream();
            objImagem.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(ms, "image/png");
        }
        #endregion LogoFilial

        #region FotoPessoa
        public ActionResult FotoPessoa(string id)
        {
            string cpf = id;

            decimal cpfDecimal = Code.Helper.ConverterCpfParaDecimal(id);

            Image objImagem = cache.Get(String.Format("foto.{0}", cpf), () =>
            {
                Image retorno;

                byte[] imageBytes = BNE.BLL.PessoaFisicaFoto.RecuperarArquivo(cpfDecimal);

                if (imageBytes == null || !imageBytes.Any())
                    retorno = Image.FromFile(Server.MapPath("~/Content/img/Anonimo.gif"));
                else
                {
                    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                    {
                        //ms.Write(imageBytes, 0, imageBytes.Length);
                        retorno = Image.FromStream(ms, true);
                    }
                }

                return retorno;
            });

            var ms2 = new MemoryStream();
            objImagem.Save(ms2, ImageFormat.Png);
            ms2.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(ms2, "image/png");
        }
        #endregion FotoPessoa

        #region Métodos privados

        #region Base64ToImage
        private Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);

            Image image;
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(ms, true);
            }

            return image;
        }
        #endregion Base64ToImage

        #endregion Métodos privados

    }
}
