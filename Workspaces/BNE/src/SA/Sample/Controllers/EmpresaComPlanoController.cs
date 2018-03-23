using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLTE_Application;
using PagedList;
using System.Security.Claims;

namespace AdminLTE_Application.Controllers
{
    [Authorize]
    public class EmpresaComPlanoController : Controller
    {
        private Model db = new Model();



        // GET: EmpresaSemPlano
        public ActionResult Index(string sortOrder, string currentFilter, string SearchString, int? page, string opcaoPesquisa, int? PageSize, string currentFilterSituacao)
        {

            string url = Request.Url.PathAndQuery.Replace("/EmpresaComPlano", "").Replace("/Index/", "");
            if (url.IndexOf("?")>=0) { 
            url = url.Substring(0, url.IndexOf("?"));
            }
            var userWithClaims = (ClaimsPrincipal)User;
            var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");
            var tipoVendedor = userWithClaims.Claims.First(c => c.Type == "tipoVendedor");
            decimal Num_CPF = Convert.ToDecimal(cpf.Value.ToString());

            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.currentFilter = SearchString;
            ViewBag.currentFilterSituacao = currentFilterSituacao;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.dtUltimoAtendimentoSort = "Ultimo Atendimento desc";
            ViewBag.dtUltimaAcaoClienteSort = "Ultima Ação Cliente desc";
            ViewBag.vlrSituacaoSort = "Situação desc";
            ViewBag.empresaSort = "Empresa desc";
            ViewBag.sigUfSort = "UF desc";
            ViewBag.areaSort = "Area desc";
            ViewBag.cidadeSort = "Cidade desc";
            ViewBag.fimPlanoSort = "Fim Plano desc";
            ViewBag.dtaRetornoSort = "Data Retorno desc";
            ViewBag.qtdAcoesSort = "Qtd de ações desc";
            ViewBag.qtdFuncionarios = "qtdFuncionarios desc";
            ViewBag.BoletoVencimento = "BoletoVencimento desc";

            if (SearchString != null)
            {
                page = 1;
            }

            var empresas = from s in db.VW_EMPRESA_COM_PLANO
                           select s;
            switch (sortOrder)
            {

                case "Ultimo Atendimento Vendedor desc":
                    empresas = empresas.OrderByDescending(s => s.Data_Ultimo_Atendimento);
                    ViewBag.dtUltimoAtendimentoVendedorSort = sortOrder;
                    break;
                case "Ultimo Atendimento Vendedor asc":
                    empresas = empresas.OrderBy(s => s.Data_Ultimo_Atendimento);
                    ViewBag.dtUltimoAtendimentoVendedorSort = sortOrder;
                    break;
                case "Ultima Ação Cliente desc":
                    empresas = empresas.OrderByDescending(s => s.Data_Ultima_Acao_Site);
                    ViewBag.dtUltimaAcaoClienteSort = sortOrder;
                    break;
                case "Ultima Ação Cliente asc":
                    empresas = empresas.OrderBy(s => s.Data_Ultima_Acao_Site);
                    ViewBag.dtUltimaAcaoClienteSort = sortOrder;
                    break;
                case "Situação desc":
                    empresas = empresas.OrderByDescending(s => s.Vlr_Percentual);
                    ViewBag.vlrSituacaoSort = sortOrder;
                    break;
                case "Situação asc":
                    empresas = empresas.OrderBy(s => s.Vlr_Percentual);
                    ViewBag.vlrSituacaoSort = sortOrder;
                    break;
                case "Qtd de ações desc":
                    empresas = empresas.OrderByDescending(s => s.Total_Acao);
                    ViewBag.qtdAcoesSort = sortOrder;
                    break;
                case "Qtd de ações asc":
                    empresas = empresas.OrderBy(s => s.Total_Acao);
                    ViewBag.qtdAcoesSort = sortOrder;
                    break;
                case "Data Retorno desc":
                    empresas = empresas.OrderByDescending(s => s.Dta_Retorno);
                    ViewBag.dtaRetornoSort = sortOrder;
                    break;
                case "Data Retorno asc":
                    empresas = empresas.OrderBy(s => s.Dta_Retorno);
                    ViewBag.dtaRetornoSort = sortOrder;
                    break;
                case "Empresa desc":
                    empresas = empresas.OrderByDescending(s => s.Raz_Social);
                    ViewBag.empresaSort = sortOrder;
                    break;
                case "Empresa asc":
                    empresas = empresas.OrderBy(s => s.Raz_Social);
                    ViewBag.empresaSort = sortOrder;
                    break;
                case "UF desc":
                    empresas = empresas.OrderByDescending(s => s.Sig_Estado);
                    ViewBag.sigUfSort = sortOrder;
                    break;
                case "UF asc":
                    empresas = empresas.OrderBy(s => s.Sig_Estado);
                    ViewBag.sigUfSort = sortOrder;
                    break;
                case "Area desc":
                    empresas = empresas.OrderByDescending(s => s.Des_Area_BNE);
                    ViewBag.areaSort = sortOrder;
                    break;
                case "Area asc":
                    empresas = empresas.OrderBy(s => s.Des_Area_BNE);
                    ViewBag.areaSort = sortOrder;
                    break;
                case "Cidade desc":
                    empresas = empresas.OrderByDescending(s => s.Nme_Cidade);
                    ViewBag.cidadeSort = sortOrder;
                    break;
                case "Cidade asc":
                    empresas = empresas.OrderBy(s => s.Nme_Cidade);
                    ViewBag.cidadeSort = sortOrder;
                    break;
                case "Fim Plano desc":
                    empresas = empresas.OrderByDescending(s => s.Dta_Fim_Plano);
                    ViewBag.fimPlanoSort = sortOrder;
                    break;
                case "Fim Plano asc":
                    empresas = empresas.OrderBy(s => s.Dta_Fim_Plano);
                    ViewBag.fimPlanoSort = sortOrder;
                    break;
                case "Num Funcionario desc":
                    empresas = empresas.OrderByDescending(s => s.qtd_Funcionarios);
                    ViewBag.qtdFuncionarios = sortOrder;
                    break;
                case "Num Funcionario asc":
                    empresas = empresas.OrderBy(s => s.qtd_Funcionarios);
                    ViewBag.qtdFuncionarios = sortOrder;
                    break;
                case "BoletoVencimento desc":
                    empresas = empresas.OrderByDescending(s => s.dta_vencimento);
                    ViewBag.BoletoVencimento = sortOrder;
                    break;
                case "BoletoVencimento asc":
                    empresas = empresas.OrderBy(s => s.dta_vencimento);
                    ViewBag.BoletoVencimento = sortOrder;
                    break;
                default:
                    empresas = empresas.OrderByDescending(s => s.Data_Ultima_Acao_Site);
                    break;
            }
            if (!String.IsNullOrEmpty(SearchString) && String.IsNullOrEmpty(currentFilterSituacao))
            {
                SearchString = SearchString.Replace(".", "").Replace("/", "").Replace("-", "");
                empresas = empresas.Where(s => s.Sig_Estado.Contains(SearchString.ToUpper()) || s.Raz_Social.Contains(SearchString.ToUpper()) || s.Des_Area_BNE.Contains(SearchString.ToUpper()) || s.Nme_Cidade.Contains(SearchString.ToUpper()) || s.Num_CNPJ.ToString().Contains(SearchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(SearchString) && !String.IsNullOrEmpty(currentFilterSituacao) && !String.Equals(currentFilterSituacao, "X"))
            {
                SearchString = SearchString.Replace(".", "").Replace("/", "").Replace("-", "");
                empresas = empresas.Where(s => s.Idf_Atendimento.ToString().Equals(currentFilterSituacao) && (s.Sig_Estado.Contains(SearchString.ToUpper()) || s.Raz_Social.Contains(SearchString.ToUpper()) || s.Des_Area_BNE.Contains(SearchString.ToUpper()) || s.Nme_Cidade.Contains(SearchString.ToUpper()) || s.Num_CNPJ.ToString().Contains(SearchString.ToUpper())));
            }
            if (String.IsNullOrEmpty(SearchString) && !String.IsNullOrEmpty(currentFilterSituacao) && !String.Equals(currentFilterSituacao, "X"))
            {
                empresas = empresas.Where(s => s.Idf_Atendimento.ToString().Equals(currentFilterSituacao));
            }
            if (tipoVendedor.Value.ToString() == "4") 
            {
                if (!String.IsNullOrEmpty(url)){
                    Num_CPF = Convert.ToDecimal(url);
                    empresas = empresas.Where(s => s.Num_CPF == Num_CPF);
                }
            }
            else
            {
                if (cpf != null)
                {
                    empresas = empresas.Where(s => s.Num_CPF == Num_CPF);
                }
            }
            if (PageSize == null) { PageSize = 100; }



            int pageSize = Convert.ToInt32(PageSize);
            int pageNumber = (page ?? 1);

            return View(empresas.ToPagedList(pageNumber, pageSize));
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
