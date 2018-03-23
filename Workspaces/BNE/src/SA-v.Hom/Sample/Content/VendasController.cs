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
using Microsoft.Reporting.WebForms;
using Sample.Models;

namespace Sample.Content
{
    public class VendasController : Controller
    {
        private Model db = new Model();

        // GET: Vendas
        public async Task<ActionResult> Index()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportPath = "/TfsReports/BNE/CRM/Vendas";
            reportViewer.ServerReport.ReportServerUrl = new Uri("http://10.114.113.209/ReportServer_PRD");
            reportViewer.ServerReport.ReportServerCredentials = new MyReportServerCredentials();
            reportViewer.Width = 900;
            reportViewer.Height = 1400;

            List<ReportParameter> paramList = new List<ReportParameter>();


            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        // GET: Vendas/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Vendedor cRM_Vendedor = await db.CRM_Vendedor.FindAsync(id);
            if (cRM_Vendedor == null)
            {
                return HttpNotFound();
            }
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportPath = "/TfsReports/BNE/CRM/Vendas";
            reportViewer.ServerReport.ReportServerUrl = new Uri("http://10.114.113.209/ReportServer_PRD");
            reportViewer.ServerReport.ReportServerCredentials = new MyReportServerCredentials();
            reportViewer.Width = 900;
            reportViewer.Height = 1200;

            List<ReportParameter> paramList = new List<ReportParameter>();

            paramList.Add(new ReportParameter("cpf", cRM_Vendedor.Num_CPF.ToString(), false));

            reportViewer.ServerReport.SetParameters(paramList);

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        // GET: Vendas/Create
        public ActionResult Create()
        {
            ViewBag.idf_Tipo_Vendedor = new SelectList(db.CRM_Tipo_Vendedor, "idf_Tipo_Vendedor", "Des_Tipo_Vendedor");
            return View();
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Num_CPF,Nme_Vendedor,Dta_Ultimo_Atendimento,Flg_Efetua_Atendimento,Eml_Vendedor,Sen_Vendedor,Flg_Inativo,Dta_Ultimo_Acesso,Flg_Administrador,Dta_Ultima_Distribuicao_Empresa,idf_Tipo_Vendedor")] CRM_Vendedor cRM_Vendedor)
        {
            if (ModelState.IsValid)
            {
                db.CRM_Vendedor.Add(cRM_Vendedor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.idf_Tipo_Vendedor = new SelectList(db.CRM_Tipo_Vendedor, "idf_Tipo_Vendedor", "Des_Tipo_Vendedor", cRM_Vendedor.idf_Tipo_Vendedor);
            return View(cRM_Vendedor);
        }

        // GET: Vendas/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Vendedor cRM_Vendedor = await db.CRM_Vendedor.FindAsync(id);
            if (cRM_Vendedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.idf_Tipo_Vendedor = new SelectList(db.CRM_Tipo_Vendedor, "idf_Tipo_Vendedor", "Des_Tipo_Vendedor", cRM_Vendedor.idf_Tipo_Vendedor);
            return View(cRM_Vendedor);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Num_CPF,Nme_Vendedor,Dta_Ultimo_Atendimento,Flg_Efetua_Atendimento,Eml_Vendedor,Sen_Vendedor,Flg_Inativo,Dta_Ultimo_Acesso,Flg_Administrador,Dta_Ultima_Distribuicao_Empresa,idf_Tipo_Vendedor")] CRM_Vendedor cRM_Vendedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cRM_Vendedor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.idf_Tipo_Vendedor = new SelectList(db.CRM_Tipo_Vendedor, "idf_Tipo_Vendedor", "Des_Tipo_Vendedor", cRM_Vendedor.idf_Tipo_Vendedor);
            return View(cRM_Vendedor);
        }

        // GET: Vendas/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Vendedor cRM_Vendedor = await db.CRM_Vendedor.FindAsync(id);
            if (cRM_Vendedor == null)
            {
                return HttpNotFound();
            }
            return View(cRM_Vendedor);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            CRM_Vendedor cRM_Vendedor = await db.CRM_Vendedor.FindAsync(id);
            db.CRM_Vendedor.Remove(cRM_Vendedor);
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
