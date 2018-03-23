using LanHouse.Entities.BNE;
using LanHouse.Business.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using LanHouse.Business;
using DTO = LanHouse.Business.DTO;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace LanHouse.API.Controllers
{
    public class CurriculoController : ApiController
    {
        private Code.ICacheService cache = new Code.InMemoryCache();

        #region LiberarVip
        [HttpPost]
        public HttpResponseMessage LiberarVip(int idCurriculo, string codigo)
        {
            try
            {
                string retorno = Curriculo.ProcessarLiberacaoVip(idCurriculo, codigo);
            
                if (retorno != string.Empty)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, retorno);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Liberar Vip");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        
        #region GetCandidatoNome
        /// <summary>
        /// Pesquisar o nome do Candidato pelo cpf e data Nascimento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCandidatoNome(string cpf, string dataNascimento)
        {
            try
            {
                return Business.Curriculo.CarregarNomeCandidato(cpf, dataNascimento);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - carregar nome do candidato na tela de login");
                return "";
            }
        }
        #endregion

        #region GetCurriculo
        /// <summary>
        /// Carregar o Curriculo do Candidato pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize]
        public Business.DTO.Curriculo GetCurriculo(string cpf, string dataNascimento, string nome)
        {
            try
            {
                return Business.Curriculo.CarregarCurriculo(cpf, dataNascimento, nome);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar Currículo");
                return null;
            }
        }
        #endregion

        #region SalvarCV
        [HttpPost]
        public HttpResponseMessage SalvarCV([FromBody]Business.DTO.Curriculo objCurriculo, int passoCadastro)
        {
            try
            {
                string retorno = Curriculo.SalvarCV(objCurriculo, passoCadastro);

                if (retorno != string.Empty)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, retorno);

                return Request.CreateResponse(HttpStatusCode.OK, objCurriculo);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Salvar Currículo");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region GetFotoCandidato
        /// <summary>
        /// Carregar a foto do candidato
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public HttpResponseMessage GetFotoCandidato(string idCandidato)
        {
            try 
            {
                int idPessoaFisica = Convert.ToInt32(idCandidato);
                Image objImagem = null;

                var logo = PessoaFisica.CarregarImagemCandidato(idPessoaFisica);

                if (logo == null || !logo.Any())
                    objImagem = Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/logo_vazio.png"));
                else
                {
                    using (var mslogo = new MemoryStream(logo, 0, logo.Length))
                    {
                        objImagem = Image.FromStream(mslogo, true);
                        objImagem = objImagem.Width >= objImagem.Height ? ScaleImage(objImagem, 260, 200) : ScaleImage(objImagem, 170, 200);
                    }
                }

                var ms = new MemoryStream();
                objImagem.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return response;
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Carregar foto do candidato");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar foto do candidato");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region DeletarIdioma
        [HttpPost]
        [Authorize]
        /// <summary>
        /// Inativar o idioma 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage DeletarIdioma(int idIdioma)
        {
            try
            {
                bool sucesso = Curriculo.InativarIdioma(idIdioma);
                return Request.CreateResponse(HttpStatusCode.OK, sucesso ? "Sucesso" : "Erro");
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, string.Format("Lan House API - Inativar o idioma => {0}", idIdioma));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro");
            }
        }
        #endregion

        #region DeletarFormacao

        [HttpPost]
        [Authorize]
        /// <summary>
        /// Inativar a formação 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage DeletarFormacao(int idFormacao)
        {
            try
            {
                bool sucesso = Curriculo.InativarFormacao(idFormacao);
                return Request.CreateResponse(HttpStatusCode.OK, sucesso ? "Sucesso" : "Erro");
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, string.Format("Lan House API - Inativar formação => {0}",idFormacao));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro");
            }
        }
        #endregion

        #region DeletarExperiencia

        [HttpPost]
        [Authorize]
        /// <summary>
        /// Inativar a formação 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage DeletarExperiencia(int idExperiencia)
        {
            try
            {
                bool sucesso = Curriculo.InativarExperiencia(idExperiencia);
                return Request.CreateResponse(HttpStatusCode.OK, sucesso ? "Sucesso" : "Erro");
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, string.Format("Lan House API - Inativar experiência Profissional => {0}", idExperiencia));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro");
            }
        }
        #endregion

        #region ScaleImage
        /// <summary>
        /// TODO - remover este método daqui
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
        #endregion


        #region PessoaFisicaExiste
        [HttpGet]
        public bool PessoaFisicaExiste(decimal cpf) 
        {
            TAB_Pessoa_Fisica pf = null;
            return PessoaFisica.CarregarPorCPF(cpf, out pf);
        }
        #endregion

    }
}