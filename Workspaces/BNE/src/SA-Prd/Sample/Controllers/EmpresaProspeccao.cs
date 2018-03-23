using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using System.Security.Claims;
using Sample.Models.Empresa;
using System.Data.Entity.Infrastructure;
using System.Text;

namespace AdminLTE_Application.Controllers
{
    [Authorize]
    public class EmpresaProspeccao : Controller
    {
        private Model db = new Model();



        // GET: EmpresaComPlano
        public ActionResult Index(string button, GridProspect model)
        {

            string url = Request.Url.PathAndQuery.Replace("/EmpresaComPlano", "").Replace("/Index/", "");
            if (url.IndexOf("?") >= 0)
            {
                url = url.Substring(0, url.IndexOf("?"));
            }

            #region [Botoes]
            switch (button)
            {
                case "Final de Prazo":
                    model.currentFilterSituacao = 6;
                    break;
                case "Sem Atendimento":
                    model.currentFilterSituacao = 3;
                    break;
                case "Atendido":
                    model.currentFilterSituacao = 1;
                    break;
                case "Negociado":
                    model.currentFilterSituacao = 2;
                    break;
                case "Todas":
                    model.currentFilterSituacao = 0;
                    break;
            }
            if (!string.IsNullOrEmpty(button))
                model.page = 1;

            #endregion

            var userWithClaims = (ClaimsPrincipal)User;
            var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");
            var tipoVendedor = userWithClaims.Claims.First(c => c.Type == "tipoVendedor");
            decimal Num_CPF = Convert.ToDecimal(cpf.Value.ToString());

            if (model.listSearchOrder == null)
                model.listSearchOrder = new List<string>();

            if (!String.IsNullOrEmpty(model.orderby))
            {
                model.listSearchOrder.Remove(model.orderby.Replace("desc", "asc"));
                model.listSearchOrder.Remove(model.orderby.Replace("asc", "desc"));
                model.listSearchOrder.Add(model.orderby);
            }

            ViewBag.NameSortParm = model.sortOrder == "name" ? "name_desc" : "name";
            ViewBag.currentFilter = model.SearchString;
            ViewBag.currentFilterSituacao = model.currentFilterSituacao;
            ViewBag.CurrentSort = model.sortOrder;
            ViewBag.dtUltimoAtendimentoSort = "Data_Ultimo_Atendimento desc";
            ViewBag.dtUltimaAcaoClienteSort = "Data_Ultima_Acao_Site desc";
            ViewBag.vlrSituacaoSort = "vlr_percentual desc";
            ViewBag.empresaSort = "Raz_Social desc";
            ViewBag.sigUfSort = "Sig_Estado desc";
            ViewBag.areaSort = "Des_Area_BNE desc";
            ViewBag.cidadeSort = "Nme_Cidade desc";
            ViewBag.fimPlanoSort = "Dta_Fim_Plano desc";
            ViewBag.dtaRetornoSort = "dta_retorno desc";
            ViewBag.qtdAcoesSort = "Total_Acao desc";
            ViewBag.qtdFuncionarios = "qtd_Funcionarios desc";
            ViewBag.BoletoVencimento = "dta_vencimento desc";

           StringBuilder ordenacao = new StringBuilder();
            DbSqlQuery<VW_EMPRESA_SEM_PLANO> query;

            #region [Order switch]
            switch (model.orderby)
            {
                case "Data_Ultimo_Atendimento desc":
                    ViewBag.dtUltimoAtendimentoVendedorSort = model.orderby;
                    break;
                case "Data_Ultimo_Atendimento asc":
                    ViewBag.dtUltimoAtendimentoVendedorSort = model.orderby;
                    break;
                case "Data_Ultima_Acao_Site desc":
                    ViewBag.dtUltimaAcaoClienteSort = model.orderby;
                    break;
                case "Data_Ultima_Acao_Site asc":
                    ViewBag.dtUltimaAcaoClienteSort = model.orderby;
                    break;
                case "vlr_percentual desc":
                    ViewBag.vlrSituacaoSort = model.orderby;
                    break;
                case "vlr_percentual asc":
                    ViewBag.vlrSituacaoSort = model.orderby;
                    break;
                case "Total_Acao desc":
                    ViewBag.qtdAcoesSort = model.orderby;
                    break;
                case "Total_Acao asc":
                    ViewBag.qtdAcoesSort = model.orderby;
                    break;
                case "dta_retorno desc":
                    ViewBag.dtaRetornoSort = model.orderby;
                    break;
                case "dta_retorno asc":
                    ViewBag.dtaRetornoSort = model.orderby;
                    break;
                case "Raz_Social desc":
                    ViewBag.empresaSort = model.orderby;
                    break;
                case "Raz_Social asc":
                    ViewBag.empresaSort = model.orderby;
                    break;
                case "Sig_Estado desc":
                    ViewBag.sigUfSort = model.orderby;
                    break;
                case "Sig_Estado asc":
                    ViewBag.sigUfSort = model.orderby;
                    break;
                case "Des_Area_BNE desc":
                    ViewBag.areaSort = model.orderby;
                    break;
                case "Des_Area_BNE asc":
                    ViewBag.areaSort = model.orderby;
                    break;
                case "Nme_Cidade desc":
                    ViewBag.cidadeSort = model.orderby;
                    break;
                case "Nme_Cidade asc":
                    ViewBag.cidadeSort = model.orderby;
                    break;
                case "Dta_Fim_Plano desc":
                    ViewBag.fimPlanoSort = model.orderby;
                    break;
                case "Dta_Fim_Plano asc":
                    ViewBag.fimPlanoSort = model.orderby;
                    break;
                case "qtd_Funcionarios desc":
                    ViewBag.qtdFuncionarios = model.orderby;
                    break;
                case "qtd_Funcionarios asc":
                    ViewBag.qtdFuncionarios = model.orderby;
                    break;
                case "dta_vencimento desc":
                    ViewBag.BoletoVencimento = model.orderby;
                    break;
                case "dta_vencimento asc":
                    ViewBag.BoletoVencimento = model.orderby;
                    break;

            }
            #endregion

            #region [Condições]
            string spConsulta = "select top 40 * from dbo.vw_empresa_com_plano with(nolock) ";

            if (model.currentFilterSituacao > 0)
                spConsulta += " where idf_atendimento = " + model.currentFilterSituacao;

            if (tipoVendedor.Value.ToString() == "4")
            {
                if (!String.IsNullOrEmpty(url))
                {
                    try
                    {
                        Num_CPF = Convert.ToDecimal(url);
                        spConsulta += spConsulta.Contains("where") ? " and Num_cpf = " + Num_CPF : " where Num_cpf = " + Num_CPF;
                    }
                    catch (Exception)
                    {
                    }
                   
                }
            }
            else
            {
                if (cpf != null)
                {
                    spConsulta += model.currentFilterSituacao > 0 ? " and Num_cpf = " + Num_CPF : " where Num_cpf = " + Num_CPF;
                }
            }
            if (!string.IsNullOrEmpty(model.SearchString))
            {
                model.SearchString = model.SearchString.Replace(".", "").Replace("/", "").Replace("-", "").Trim();
                spConsulta += spConsulta.Contains("where") ? string.Format(" or raz_social = '{0}' or nme_cidade = '{0}' or Sig_estado = '{0}' ", model.SearchString) : string.Format(" where raz_social  like '{0}' or nme_cidade = '{0}' or Sig_estado = '{0}' ", model.SearchString);
                try
                {
                    spConsulta += string.Format(" or num_cnpj = {0} ", Convert.ToDecimal(model.SearchString));
                }
                catch (Exception ex)
                {

                }

            }
            #endregion

            #region [Ordenação]
            if (model.listSearchOrder.Count <= 0)
                query = db.VW_EMPRESA_SEM_PLANO.SqlQuery(spConsulta + " order by data_ultima_acao_site desc ");
            else
            {
                foreach (var item in model.listSearchOrder.AsEnumerable().Reverse())
                {
                    ordenacao.Append(item + ",");
                }

                query = db.VW_EMPRESA_SEM_PLANO.SqlQuery(spConsulta + "order by " + ordenacao.ToString().TrimEnd(','));
            }
            #endregion

            if (model.PageSize == null) { model.PageSize = 100; }

            model.orderby = string.Empty;

            model.page = (model.page ?? 1);
            model.PageSize = model.PageSize <= 0 ? 100 : model.PageSize;
            model.Empresas = query.ToPagedList(model.page.Value, model.PageSize);

            return View("~/Views/EmpresaComPlano/Index.cshtml", model);
        }

        // GET: EmpresaSemPlano/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VW_EMPRESA_COM_PLANO VW_EMPRESA_COM_PLANO = db.VW_EMPRESA_COM_PLANO.Find(id);
            if (VW_EMPRESA_COM_PLANO == null)
            {
                return HttpNotFound();
            }
            return View(VW_EMPRESA_COM_PLANO);
        }

        // GET: EmpresaSemPlano/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpresaSemPlano/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Num_CPF,Num_CNPJ,Dta_Cadastro,Des_Atendimento,Data_Ultimo_Atendimento,Data_Ultima_Acao_Site,Data_ultimo_ATC_empresa,Idf_Atendimento,Raz_Social,Sig_Estado,Des_Area_BNE,Nme_Cidade,Dta_Fim_Plano,Total_Acao,Dta_Retorno,qtd_Funcionarios,dta_vencimento")] VW_EMPRESA_COM_PLANO VW_EMPRESA_COM_PLANO)
        {
            if (ModelState.IsValid)
            {
                db.VW_EMPRESA_COM_PLANO.Add(VW_EMPRESA_COM_PLANO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(VW_EMPRESA_COM_PLANO);
        }

        // GET: EmpresaSemPlano/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VW_EMPRESA_COM_PLANO VW_EMPRESA_COM_PLANO = db.VW_EMPRESA_COM_PLANO.Find(id);
            if (VW_EMPRESA_COM_PLANO == null)
            {
                return HttpNotFound();
            }
            return View(VW_EMPRESA_COM_PLANO);
        }

        // POST: EmpresaSemPlano/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Num_CPF,Num_CNPJ,Dta_Cadastro,Des_Atendimento,Data_Ultimo_Atendimento,Data_Ultima_Acao_Site,Data_ultimo_ATC_empresa,Idf_Atendimento,Raz_Social,Sig_Estado,Des_Area_BNE,Nme_Cidade,Dta_Fim_Plano,Total_Acao,Dta_Retorno")] VW_EMPRESA_COM_PLANO VW_EMPRESA_COM_PLANO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(VW_EMPRESA_COM_PLANO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(VW_EMPRESA_COM_PLANO);
        }

        // GET: EmpresaSemPlano/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VW_EMPRESA_COM_PLANO VW_EMPRESA_COM_PLANO = db.VW_EMPRESA_COM_PLANO.Find(id);
            if (VW_EMPRESA_COM_PLANO == null)
            {
                return HttpNotFound();
            }
            return View(VW_EMPRESA_COM_PLANO);
        }

        // POST: EmpresaSemPlano/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            VW_EMPRESA_COM_PLANO VW_EMPRESA_COM_PLANO = db.VW_EMPRESA_COM_PLANO.Find(id);
            db.VW_EMPRESA_COM_PLANO.Remove(VW_EMPRESA_COM_PLANO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}
