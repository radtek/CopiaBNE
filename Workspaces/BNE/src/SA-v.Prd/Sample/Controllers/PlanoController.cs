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

namespace Sample.Controllers
{
    public class PlanoController : Controller
    {
        private Model db = new Model();

        // GET: Plano
        public async Task<ActionResult> Index()
        {
            var cRM_Empresa = db.CRM_Empresa.Include(c => c.CRM_Situacao_Atendimento);
            return View(await cRM_Empresa.ToListAsync());
        }

        // GET: Plano/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Empresa cRM_Empresa = await db.CRM_Empresa.FindAsync(id);
            if (cRM_Empresa == null)
            {
                return HttpNotFound();
            } ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportPath = "/TfsReports/BNE/CRM/PlanoAdiquirido";
            reportViewer.ServerReport.ReportServerUrl = new Uri("http://relatorios.bne.com.br/ReportServer_PRD");
            reportViewer.ServerReport.ReportServerCredentials = new MyReportServerCredentials();
            reportViewer.Width = 1000;
            reportViewer.Height = 1200;

            List<ReportParameter> paramList = new List<ReportParameter>();

            paramList.Add(new ReportParameter("num_cnpj", cRM_Empresa.Num_CNPJ.ToString(), false));

            reportViewer.ServerReport.SetParameters(paramList);

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        // GET: Plano/Create
        public ActionResult Create()
        {
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento");
            return View();
        }

        // POST: Plano/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Num_CNPJ,Idf_Filial,Raz_Social,Dta_Cadastro,Dta_Ultimo_Acesso,Idf_Plano,Des_Plano,Dta_Inicio,Dta_Fim,Qtd_Dias_Plano_Ativo,Qtd_Acabar_Plano,Idf_Situacao_Atendimento,Dta_Ultimo_Atendimento,Nme_Cidade,Sig_Estado,Idf_Cidade,Dta_Retorno,Idf_Area_BNE")] CRM_Empresa cRM_Empresa)
        {
            if (ModelState.IsValid)
            {
                db.CRM_Empresa.Add(cRM_Empresa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Empresa.Idf_Situacao_Atendimento);
            return View(cRM_Empresa);
        }

        // GET: Plano/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Empresa cRM_Empresa = await db.CRM_Empresa.FindAsync(id);
            if (cRM_Empresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Empresa.Idf_Situacao_Atendimento);
            return View(cRM_Empresa);
        }

        // POST: Plano/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Num_CNPJ,Idf_Filial,Raz_Social,Dta_Cadastro,Dta_Ultimo_Acesso,Idf_Plano,Des_Plano,Dta_Inicio,Dta_Fim,Qtd_Dias_Plano_Ativo,Qtd_Acabar_Plano,Idf_Situacao_Atendimento,Dta_Ultimo_Atendimento,Nme_Cidade,Sig_Estado,Idf_Cidade,Dta_Retorno,Idf_Area_BNE")] CRM_Empresa cRM_Empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cRM_Empresa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Empresa.Idf_Situacao_Atendimento);
            return View(cRM_Empresa);
        }

        // GET: Plano/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Empresa cRM_Empresa = await db.CRM_Empresa.FindAsync(id);
            if (cRM_Empresa == null)
            {
                return HttpNotFound();
            }
            return View(cRM_Empresa);
        }

        // POST: Plano/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            CRM_Empresa cRM_Empresa = await db.CRM_Empresa.FindAsync(id);
            db.CRM_Empresa.Remove(cRM_Empresa);
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
