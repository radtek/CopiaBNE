using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using System.Security.Claims;
using System.Globalization;
using Sample.Models;

namespace AdminLTE_Application.Controllers
{
    [Authorize]
    public class EmpresaSemPlanoController : Controller
    {
        private Model db = new Model();

        // GET: EmpresaSemPlano
        public ActionResult Index(string button, GridEmpresaSemPlano model)
        {
            string url = Request.Url.PathAndQuery.Replace("/EmpresaSemPlano", "").Replace("/Index/", "");
            if (url.IndexOf("?") >= 0)
            {
                url = url.Substring(0, url.IndexOf("?"));
            }
            var userWithClaims = (ClaimsPrincipal)User;
            var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");
            var tipoVendedor = userWithClaims.Claims.First(c => c.Type == "tipoVendedor");
            decimal Num_CPF = Convert.ToDecimal(cpf.Value.ToString());

            if (button != null)
                model.pag = 1;

            using(var context = new Model())
            {
                context.Database.CommandTimeout = 60;
                model.pag = model.pag <= 0 ? 1 : model.pag;
                model.rowsPag = model.rowsPag <= 1 ? 100 : model.rowsPag;

                var empresas = from s in context.VW_EMPRESA_SEM_PLANO
                               select s;

                #region [Filtro Situação]
                model.FlSituacao = button;

                switch (button)
                {
                    case "Negociado":
                        empresas = empresas.Where(s => s.Idf_Atendimento.ToString().Equals("2"));//passar para enumerador
                        break;
                    case "Atendido":
                        empresas = empresas.Where(s => s.Idf_Atendimento.ToString().Equals("1"));//passar para enumerador
                        break;
                    case "Sem atendimento":
                        empresas = empresas.Where(s => s.Idf_Atendimento.ToString().Equals("3"));//passar para enumerador
                        break;
                    case "Final de prazo":
                        empresas = empresas.Where(s => s.Idf_Atendimento.ToString().Equals("6"));//passar para enumerador
                        break;
                    case "Prospcção":
                        empresas = empresas.Where(s => s.Idf_Atendimento.ToString().Equals("7"));//passar para enumerador
                        break;
                    default:
                        model.FlSituacao = string.Empty;
                        break;
                }
                #endregion


                if (!string.IsNullOrEmpty(model.FlPlano))
                    empresas = empresas.Where(s => s.Ultimo_Plano.Contains(model.FlPlano));
                if (!string.IsNullOrEmpty(model.FlEmpresa))
                {
                    var FlEmpresa = model.FlEmpresa.Replace(".", "").Replace("/", "").Replace("-", "");
                    empresas = empresas.Where(s => s.Raz_Social.ToUpper().Contains(FlEmpresa.ToUpper()) || s.Num_CNPJ.ToString().Equals(FlEmpresa.ToUpper()));
                }
                if (!string.IsNullOrEmpty(model.FlArea))
                    empresas = empresas.Where(s => s.Des_Area_BNE.Contains(model.FlArea));
                if (!string.IsNullOrEmpty(model.FlCidade))
                    empresas = empresas.Where(s => s.Nme_Cidade.Contains(model.FlCidade));
                if (!string.IsNullOrEmpty(model.FlEstado))
                    empresas = empresas.Where(s => s.Sig_Estado.Equals(model.FlEstado));

                if (tipoVendedor.Value.ToString() == "4")
                {
                    if (!String.IsNullOrEmpty(url))
                    {
                        try
                        {
                            Num_CPF = Convert.ToDecimal(url);
                            empresas = empresas.Where(s => s.Num_CPF == Num_CPF);
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
                        empresas = empresas.Where(s => s.Num_CPF == Num_CPF);
                    }
                }
                if (!string.IsNullOrEmpty(model.FlDataFimPlano))
                {
                    try
                    {
                        DateTime datainicio;
                        DateTime datafim;
                        datainicio = DateTime.ParseExact(model.FlDataFimPlano.Substring(0, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                        datafim = DateTime.ParseExact(model.FlDataFimPlano.Substring(13, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                        datafim = datafim.AddDays(1);
                        empresas = empresas.Where(s => s.Dta_Fim_Plano >= datainicio);
                        empresas = empresas.Where(s => s.Dta_Fim_Plano <= datafim);
                    }
                    catch (Exception)
                    {
                        ViewBag.erro = "Data de Fim Plano em formato errado.";
                    }
                  
                }

                if (!string.IsNullOrEmpty(model.FlDataRetorno))
                {
                    try
                    {
                        DateTime datainicio;
                        DateTime datafim;
                        datainicio = DateTime.ParseExact(model.FlDataRetorno.Substring(0, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                        datafim = DateTime.ParseExact(model.FlDataRetorno.Substring(13, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                        datafim = datafim.AddDays(1);
                        empresas = empresas.Where(s => s.Dta_Retorno >= datainicio);
                        empresas = empresas.Where(s => s.Dta_Retorno <= datafim);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.erro = "Data de retorno em formato errado.";
                    }

                }
                if (!string.IsNullOrEmpty(model.FlDataCadastro))
                {
                    try
                    {
                        DateTime datainicio;
                        DateTime datafim;
                        datainicio = DateTime.ParseExact(model.FlDataCadastro.Substring(0, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                        datafim = DateTime.ParseExact(model.FlDataCadastro.Substring(13, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                        datafim = datafim.AddDays(1);
                        empresas = empresas.Where(s => s.Dta_Cadastro >= datainicio);
                        empresas = empresas.Where(s => s.Dta_Cadastro <= datafim);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.erro = "Data de cadastro em formato errado.";
                    }
                }

                #region [Ordenacao]
                switch (model.Ordenacao)
                {

                    case "Ultimo_Atendimento_Vendedor desc":
                        empresas = empresas.OrderByDescending(s => s.Data_Ultimo_Atendimento);
                        break;
                    case "Ultimo_Atendimento_Vendedor asc":
                        empresas = empresas.OrderBy(s => s.Data_Ultimo_Atendimento);
                        break;
                    case "Ultima_Acao_Cliente desc":
                        empresas = empresas.OrderByDescending(s => s.Data_Ultima_Acao_Site);
                        break;
                    case "Ultima_Acao_Cliente asc":
                        empresas = empresas.OrderBy(s => s.Data_Ultima_Acao_Site);
                        break;
                    case "Ultimo_Atendimento desc":
                        empresas = empresas.OrderByDescending(s => s.Data_ultimo_ATC_empresa);
                        break;
                    case "Ultimo_Atendimento asc":
                        empresas = empresas.OrderBy(s => s.Data_ultimo_ATC_empresa);
                        break;
                    case "Situação desc":
                        empresas = empresas.OrderByDescending(s => s.Vlr_Percentual);
                        break;
                    case "Situação asc":
                        empresas = empresas.OrderBy(s => s.Vlr_Percentual);
                        break;
                    case "Qtd_de_acoes desc":
                        empresas = empresas.OrderByDescending(s => s.Total_Acao);
                        break;
                    case "Qtd_de_acoes asc":
                        empresas = empresas.OrderBy(s => s.Total_Acao);
                        break;
                    case "Data_Retorno desc":
                        empresas = empresas.OrderByDescending(s => s.Dta_Retorno);
                        break;
                    case "Data_Retorno asc":
                        empresas = empresas.OrderBy(s => s.Dta_Retorno);
                        break;
                    case "Data_Cadastro desc":
                        empresas = empresas.OrderByDescending(s => s.Dta_Cadastro);
                        break;
                    case "Data_Cadastro asc":
                        empresas = empresas.OrderBy(s => s.Dta_Cadastro);
                        break;
                    case "Empresa desc":
                        empresas = empresas.OrderByDescending(s => s.Raz_Social);
                        break;
                    case "Empresa asc":
                        empresas = empresas.OrderBy(s => s.Raz_Social);
                        break;
                    case "UF desc":
                        empresas = empresas.OrderByDescending(s => s.Sig_Estado);
                        break;
                    case "UF asc":
                        empresas = empresas.OrderBy(s => s.Sig_Estado);
                        break;
                    case "Area desc":
                        empresas = empresas.OrderByDescending(s => s.Des_Area_BNE);
                        break;
                    case "Area asc":
                        empresas = empresas.OrderBy(s => s.Des_Area_BNE);
                        break;
                    case "Cidade desc":
                        empresas = empresas.OrderByDescending(s => s.Nme_Cidade);
                        break;
                    case "Cidade asc":
                        empresas = empresas.OrderBy(s => s.Nme_Cidade);
                        break;
                    case "Fim_Plano desc":
                        empresas = empresas.OrderByDescending(s => s.Dta_Fim_Plano);
                        break;
                    case "Fim_Plano asc":
                        empresas = empresas.OrderBy(s => s.Dta_Fim_Plano);
                        break;
                    case "Num_Funcionario desc":
                        empresas = empresas.OrderByDescending(s => s.qtd_Funcionarios);
                        break;
                    case "Num_Funcionario asc":
                        empresas = empresas.OrderBy(s => s.qtd_Funcionarios);
                        break;
                    case "Boleto_Vencimento desc":
                        empresas = empresas.OrderByDescending(s => s.dta_vencimento);
                        break;
                    case "Boleto_Vencimento asc":
                        empresas = empresas.OrderBy(s => s.dta_vencimento);
                        break;
                    case "Plano desc":
                        empresas = empresas.OrderByDescending(s => s.Ultimo_Plano);
                        break;
                    case "Plano asc":
                        empresas = empresas.OrderBy(s => s.Ultimo_Plano);
                        break;
                    default:
                        empresas = empresas.OrderByDescending(s => s.Data_Ultima_Acao_Site);
                        model.Ordenacao = "Ultimo_Atendimento_Vendedor desc";
                        break;
                }
                #endregion

                model.Empresas = empresas.ToPagedList(model.pag, model.rowsPag);
           
            }
          return View("~/Views/EmpresaSemPlano/Index.cshtml", model);
        }

        // GET: EmpresaSemPlano/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO = db.VW_EMPRESA_SEM_PLANO.Find(id);
            if (vW_EMPRESA_SEM_PLANO == null)
            {
                return HttpNotFound();
            }
            return View(vW_EMPRESA_SEM_PLANO);
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
        public ActionResult Create([Bind(Include = "Num_CPF,Num_CNPJ,Dta_Cadastro,Des_Atendimento,Data_Ultimo_Atendimento,Data_Ultima_Acao_Site,Data_ultimo_ATC_empresa,Idf_Atendimento,Raz_Social,Sig_Estado,Des_Area_BNE,Nme_Cidade,Dta_Fim_Plano,Total_Acao,Dta_Retorno,qtd_Funcionarios,dta_vencimento")] VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO)
        {
            if (ModelState.IsValid)
            {
                db.VW_EMPRESA_SEM_PLANO.Add(vW_EMPRESA_SEM_PLANO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vW_EMPRESA_SEM_PLANO);
        }

        // GET: EmpresaSemPlano/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO = db.VW_EMPRESA_SEM_PLANO.Find(id);
            if (vW_EMPRESA_SEM_PLANO == null)
            {
                return HttpNotFound();
            }
            return View(vW_EMPRESA_SEM_PLANO);
        }

        // POST: EmpresaSemPlano/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Num_CPF,Num_CNPJ,Dta_Cadastro,Des_Atendimento,Data_Ultimo_Atendimento,Data_Ultima_Acao_Site,Data_ultimo_ATC_empresa,Idf_Atendimento,Raz_Social,Sig_Estado,Des_Area_BNE,Nme_Cidade,Dta_Fim_Plano,Total_Acao,Dta_Retorno")] VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vW_EMPRESA_SEM_PLANO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vW_EMPRESA_SEM_PLANO);
        }

        // GET: EmpresaSemPlano/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO = db.VW_EMPRESA_SEM_PLANO.Find(id);
            if (vW_EMPRESA_SEM_PLANO == null)
            {
                return HttpNotFound();
            }
            return View(vW_EMPRESA_SEM_PLANO);
        }

        // POST: EmpresaSemPlano/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO = db.VW_EMPRESA_SEM_PLANO.Find(id);
            db.VW_EMPRESA_SEM_PLANO.Remove(vW_EMPRESA_SEM_PLANO);
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
