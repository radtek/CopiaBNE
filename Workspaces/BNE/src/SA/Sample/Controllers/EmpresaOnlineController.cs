using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLTE_Application;

namespace Sample.Controllers
{
    public class EmpresaOnlineController : Controller
    {
        private Model db = new Model();

        // GET: EmpresaOnline
        public async Task<ActionResult> Index()
        {
            //var cRM_Vendedor_Empresa = db.CRM_Vendedor_Empresa.Include(c => c.CRM_Empresa).Include(c => c.CRM_Situacao_Atendimento).Include(c => c.CRM_Vendedor);
            return View();
        }

        // GET: EmpresaOnline/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Vendedor_Empresa cRM_Vendedor_Empresa = await db.CRM_Vendedor_Empresa.FindAsync(id);
            if (cRM_Vendedor_Empresa == null)
            {
                return HttpNotFound();
            }
            return View(cRM_Vendedor_Empresa);
        }

        // GET: EmpresaOnline/Create
        public ActionResult Create()
        {
            ViewBag.Num_CNPJ = new SelectList(db.CRM_Empresa, "Num_CNPJ", "Raz_Social");
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento");
            ViewBag.Num_CPF = new SelectList(db.CRM_Vendedor, "Num_CPF", "Nme_Vendedor");
            return View();
        }

        // POST: EmpresaOnline/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idf_Vendedor_Empresa,Num_CPF,Num_CNPJ,Dta_Inicio,Dta_Fim,Dta_Cadastro,Dta_Alteracao,Des_Obs_VendedorEmpresa,Idf_Situacao_Atendimento")] CRM_Vendedor_Empresa cRM_Vendedor_Empresa)
        {
            if (ModelState.IsValid)
            {
                db.CRM_Vendedor_Empresa.Add(cRM_Vendedor_Empresa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Num_CNPJ = new SelectList(db.CRM_Empresa, "Num_CNPJ", "Raz_Social", cRM_Vendedor_Empresa.Num_CNPJ);
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Vendedor_Empresa.Idf_Situacao_Atendimento);
            ViewBag.Num_CPF = new SelectList(db.CRM_Vendedor, "Num_CPF", "Nme_Vendedor", cRM_Vendedor_Empresa.Num_CPF);
            return View(cRM_Vendedor_Empresa);
        }

        // GET: EmpresaOnline/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Vendedor_Empresa cRM_Vendedor_Empresa = await db.CRM_Vendedor_Empresa.FindAsync(id);
            if (cRM_Vendedor_Empresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.Num_CNPJ = new SelectList(db.CRM_Empresa, "Num_CNPJ", "Raz_Social", cRM_Vendedor_Empresa.Num_CNPJ);
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Vendedor_Empresa.Idf_Situacao_Atendimento);
            ViewBag.Num_CPF = new SelectList(db.CRM_Vendedor, "Num_CPF", "Nme_Vendedor", cRM_Vendedor_Empresa.Num_CPF);
            return View(cRM_Vendedor_Empresa);
        }

        // POST: EmpresaOnline/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idf_Vendedor_Empresa,Num_CPF,Num_CNPJ,Dta_Inicio,Dta_Fim,Dta_Cadastro,Dta_Alteracao,Des_Obs_VendedorEmpresa,Idf_Situacao_Atendimento")] CRM_Vendedor_Empresa cRM_Vendedor_Empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cRM_Vendedor_Empresa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Num_CNPJ = new SelectList(db.CRM_Empresa, "Num_CNPJ", "Raz_Social", cRM_Vendedor_Empresa.Num_CNPJ);
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Vendedor_Empresa.Idf_Situacao_Atendimento);
            ViewBag.Num_CPF = new SelectList(db.CRM_Vendedor, "Num_CPF", "Nme_Vendedor", cRM_Vendedor_Empresa.Num_CPF);
            return View(cRM_Vendedor_Empresa);
        }

        // GET: EmpresaOnline/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Vendedor_Empresa cRM_Vendedor_Empresa = await db.CRM_Vendedor_Empresa.FindAsync(id);
            if (cRM_Vendedor_Empresa == null)
            {
                return HttpNotFound();
            }
            return View(cRM_Vendedor_Empresa);
        }

        // POST: EmpresaOnline/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CRM_Vendedor_Empresa cRM_Vendedor_Empresa = await db.CRM_Vendedor_Empresa.FindAsync(id);
            db.CRM_Vendedor_Empresa.Remove(cRM_Vendedor_Empresa);
            await db.SaveChangesAsync();
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
