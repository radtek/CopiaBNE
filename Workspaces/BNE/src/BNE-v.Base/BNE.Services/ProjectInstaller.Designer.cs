using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;
using BNE.Services.Properties;

namespace BNE.Services
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.EnviodeSMSSemanalInstaller = new System.ServiceProcess.ServiceInstaller();
            this.AtualizarCurriculoInstaller = new System.ServiceProcess.ServiceInstaller();
            this.ArquivarVagaInstaller = new System.ServiceProcess.ServiceInstaller();
            this.AtualizarPlanoInstaller = new System.ServiceProcess.ServiceInstaller();
            this.AtualizarEmpresaInstaller = new System.ServiceProcess.ServiceInstaller();
            this.EnviarCurriculo = new System.ServiceProcess.ServiceInstaller();
            this.AtualizaSitemapInstaller = new System.ServiceProcess.ServiceInstaller();
            this.InativarCurriculoInstaller = new System.ServiceProcess.ServiceInstaller();
            this.EnvioEmailMailingInstaller = new System.ServiceProcess.ServiceInstaller();
            this.BuscaCoordenadaInstaller = new System.ServiceProcess.ServiceInstaller();
            this.IntegracaoWebfopagInstaller = new System.ServiceProcess.ServiceInstaller();
            this.DestravaSMSPlanoEmployerInstaller = new System.ServiceProcess.ServiceInstaller();
            this.OperadoraCelularInstaller = new System.ServiceProcess.ServiceInstaller();
            this.ControleParcelasInstaller = new System.ServiceProcess.ServiceInstaller();
            this.EmailsInvalidosInstaller = new System.ServiceProcess.ServiceInstaller();
            this.AllInEmailSincronizacaoListaInstaller = new System.ServiceProcess.ServiceInstaller();
            this.EmailFiliaisInstaller = new System.ServiceProcess.ServiceInstaller();
            this.IntegrarVagasInstaller = new System.ServiceProcess.ServiceInstaller();
            this.EnvioSMSEmpresasInstaller = new System.ServiceProcess.ServiceInstaller();
            this.EnviarEmailAlertaExperienciaProfissionalInstaller = new System.ServiceProcess.ServiceInstaller();
            this.AllinEmailQuemMeViuInstaller = new System.ServiceProcess.ServiceInstaller();
            this.ControleFinanceiroInstaller = new System.ServiceProcess.ServiceInstaller();
            this.EnvioSMSEmailEmpresasCvsNaoVistosInstaller = new System.ServiceProcess.ServiceInstaller();
            this.SondaBancoDoBrasilInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            this.serviceProcessInstaller.Committed += new System.Configuration.Install.InstallEventHandler(this.ServiceProcessInstaller_Committed);
            this.serviceProcessInstaller.BeforeInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceProcessInstaller_BeforeInstall);
            // 
            // EnviodeSMSSemanalInstaller
            // 
            this.EnviodeSMSSemanalInstaller.DisplayName = "BNE.Services.EnvioSMSSemanal";
            this.EnviodeSMSSemanalInstaller.ServiceName = "BNE.Services.EnvioSMSSemanal";
            this.EnviodeSMSSemanalInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // AtualizarCurriculoInstaller
            // 
            this.AtualizarCurriculoInstaller.DisplayName = "BNE.Services.AtualizarCurriculo";
            this.AtualizarCurriculoInstaller.ServiceName = "BNE.Services.AtualizarCurriculo";
            this.AtualizarCurriculoInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ArquivarVagaInstaller
            // 
            this.ArquivarVagaInstaller.DisplayName = "BNE.Services.ArquivarVaga";
            this.ArquivarVagaInstaller.ServiceName = "BNE.Services.ArquivarVaga";
            this.ArquivarVagaInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // AtualizarPlanoInstaller
            // 
            this.AtualizarPlanoInstaller.DisplayName = "BNE.Services.AtualizarPlano";
            this.AtualizarPlanoInstaller.ServiceName = "BNE.Services.AtualizarPlano";
            this.AtualizarPlanoInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // AtualizarEmpresaInstaller
            // 
            this.AtualizarEmpresaInstaller.DisplayName = "BNE.Services.AtualizarEmpresa";
            this.AtualizarEmpresaInstaller.ServiceName = "BNE.Services.AtualizarEmpresa";
            this.AtualizarEmpresaInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // EnviarCurriculo
            // 
            this.EnviarCurriculo.DisplayName = "BNE.Services.EnviarCurriculo";
            this.EnviarCurriculo.ServiceName = "BNE.Services.EnviarCurriculo";
            this.EnviarCurriculo.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // AtualizaSitemapInstaller
            // 
            this.AtualizaSitemapInstaller.DisplayName = "BNE.Services.AtualizaSitemap";
            this.AtualizaSitemapInstaller.ServiceName = "BNE.Services.AtualizaSitemap";
            this.AtualizaSitemapInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // InativarCurriculoInstaller
            // 
            this.InativarCurriculoInstaller.DisplayName = "BNE.Services.InativarCurriculo";
            this.InativarCurriculoInstaller.ServiceName = "BNE.Services.InativarCurriculo";
            this.InativarCurriculoInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // EnvioEmailMailingInstaller
            // 
            this.EnvioEmailMailingInstaller.DisplayName = "BNE.Services.EnvioEmailMailing";
            this.EnvioEmailMailingInstaller.ServiceName = "BNE.Services.EnvioEmailMailing";
            this.EnvioEmailMailingInstaller.StartType = System.ServiceProcess.ServiceStartMode.Disabled;
            // 
            // BuscaCoordenadaInstaller
            // 
            this.BuscaCoordenadaInstaller.DisplayName = "BNE.Services.BuscaCoordenada";
            this.BuscaCoordenadaInstaller.ServiceName = "BNE.Services.BuscaCoordenada";
            this.BuscaCoordenadaInstaller.StartType = System.ServiceProcess.ServiceStartMode.Disabled;
            // 
            // IntegracaoWebfopagInstaller
            // 
            this.IntegracaoWebfopagInstaller.DisplayName = "BNE.Services.IntegracaoWebfopag";
            this.IntegracaoWebfopagInstaller.ServiceName = "BNE.Services.IntegracaoWebfopag";
            this.IntegracaoWebfopagInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // DestravaSMSPlanoEmployerInstaller
            // 
            this.DestravaSMSPlanoEmployerInstaller.DisplayName = "BNE.Services.DestravaSMSPlanoEmployer";
            this.DestravaSMSPlanoEmployerInstaller.ServiceName = "BNE.Services.DestravaSMSPlanoEmployer";
            this.DestravaSMSPlanoEmployerInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // OperadoraCelularInstaller
            // 
            this.OperadoraCelularInstaller.DisplayName = "BNE.Services.OperadoraCelular";
            this.OperadoraCelularInstaller.ServiceName = "BNE.Services.OperadoraCelular";
            this.OperadoraCelularInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ControleParcelasInstaller
            // 
            this.ControleParcelasInstaller.DisplayName = "BNE.Services.ControleParcelas";
            this.ControleParcelasInstaller.ServiceName = "BNE.Services.ControleParcelas";
            this.ControleParcelasInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // EmailsInvalidosInstaller
            // 
            this.EmailsInvalidosInstaller.DisplayName = "BNE.Services.EmailsInvalidos";
            this.EmailsInvalidosInstaller.ServiceName = "BNE.Services.EmailsInvalidos";
            this.EmailsInvalidosInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // AllInEmailSincronizacaoListaInstaller
            // 
            this.AllInEmailSincronizacaoListaInstaller.DisplayName = "BNE.Services.AllInEmailSincronizacaoLista";
            this.AllInEmailSincronizacaoListaInstaller.ServiceName = "BNE.Services.AllInEmailSincronizacaoLista";
            this.AllInEmailSincronizacaoListaInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // EmailFiliaisInstaller
            // 
            this.EmailFiliaisInstaller.DisplayName = "BNE.Services.EmailsFiliais";
            this.EmailFiliaisInstaller.ServiceName = "BNE.Services.EmailsFiliais";
            this.EmailFiliaisInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // IntegrarVagasInstaller
            // 
            this.IntegrarVagasInstaller.DisplayName = "BNE.Services.IntegrarVagas";
            this.IntegrarVagasInstaller.ServiceName = "BNE.Services.IntegrarVagas";
            this.IntegrarVagasInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // EnvioSMSEmpresasInstaller
            // 
            this.EnvioSMSEmpresasInstaller.DisplayName = "BNE.Services.EnvioSMSEmpresas";
            this.EnvioSMSEmpresasInstaller.ServiceName = "BNE.Services.EnvioSMSEmpresas";
            this.EnvioSMSEmpresasInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // EnviarEmailAlertaExperienciaProfissionalInstaller
            // 
            this.EnviarEmailAlertaExperienciaProfissionalInstaller.DisplayName = "BNE.Services.EnviarEmailAlertaExperienciaProfissional";
            this.EnviarEmailAlertaExperienciaProfissionalInstaller.ServiceName = "BNE.Services.EnviarEmailAlertaExperienciaProfissional";
            this.EnviarEmailAlertaExperienciaProfissionalInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // AllinEmailQuemMeViuInstaller
            // 
            this.AllinEmailQuemMeViuInstaller.DisplayName = "BNE.Services.AllinEmailQuemMeViu";
            this.AllinEmailQuemMeViuInstaller.ServiceName = "BNE.Services.AllinEmailQuemMeViu";
            this.AllinEmailQuemMeViuInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ControleFinanceiroInstaller
            // 
            this.ControleFinanceiroInstaller.DisplayName = "BNE.Services.ControleFinanceiro";
            this.ControleFinanceiroInstaller.ServiceName = "BNE.Services.ControleFinanceiro";
            this.ControleFinanceiroInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // EnvioSMSEmailEmpresasCvsNaoVistosInstaller
            // 
            this.EnvioSMSEmailEmpresasCvsNaoVistosInstaller.DisplayName = "BNE.Services.EnvioSMSEmailEmpresasCvsNaoVistos";
            this.EnvioSMSEmailEmpresasCvsNaoVistosInstaller.ServiceName = "BNE.Services.EnvioSMSEmailEmpresasCvsNaoVistos";
            this.EnvioSMSEmailEmpresasCvsNaoVistosInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // SondaBancoDoBrasilInstaller
            // 
            this.SondaBancoDoBrasilInstaller.DisplayName = "BNE.Services.SondaBancoDoBrasil";
            this.SondaBancoDoBrasilInstaller.ServiceName = "BNE.Services.SondaBancoDoBrasil";
            this.SondaBancoDoBrasilInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.EnviodeSMSSemanalInstaller,
            this.AtualizarCurriculoInstaller,
            this.ArquivarVagaInstaller,
            this.AtualizarPlanoInstaller,
            this.AtualizarEmpresaInstaller,
            this.EnviarCurriculo,
            this.AtualizaSitemapInstaller,
            this.InativarCurriculoInstaller,
            this.EnvioEmailMailingInstaller,
            this.BuscaCoordenadaInstaller,
            this.IntegracaoWebfopagInstaller,
            this.DestravaSMSPlanoEmployerInstaller,
            this.OperadoraCelularInstaller,
            this.ControleParcelasInstaller,
            this.EmailsInvalidosInstaller,
            this.AllInEmailSincronizacaoListaInstaller,
            this.EmailFiliaisInstaller,
            this.IntegrarVagasInstaller,
            this.EnvioSMSEmpresasInstaller,
            this.EnviarEmailAlertaExperienciaProfissionalInstaller,
            this.AllinEmailQuemMeViuInstaller,
            this.ControleFinanceiroInstaller,
            this.EnvioSMSEmailEmpresasCvsNaoVistosInstaller,
            this.SondaBancoDoBrasilInstaller});

        }

        #endregion

        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller EnviodeSMSSemanalInstaller;
        private ServiceInstaller AtualizarCurriculoInstaller;
        private ServiceInstaller ArquivarVagaInstaller;
        private ServiceInstaller AtualizarPlanoInstaller;
        private ServiceInstaller AtualizarEmpresaInstaller;
        private ServiceInstaller EnviarCurriculo;
        private ServiceInstaller AtualizaSitemapInstaller;
        private ServiceInstaller InativarCurriculoInstaller;
        private ServiceInstaller EnvioEmailMailingInstaller;
        private ServiceInstaller BuscaCoordenadaInstaller;
        private ServiceInstaller IntegracaoWebfopagInstaller;
        private ServiceInstaller DestravaSMSPlanoEmployerInstaller;
        private ServiceInstaller OperadoraCelularInstaller;
        private ServiceInstaller ControleParcelasInstaller;
        private ServiceInstaller EmailsInvalidosInstaller;
        private ServiceInstaller AllInEmailSincronizacaoListaInstaller;
        private ServiceInstaller EmailFiliaisInstaller;
        private ServiceInstaller IntegrarVagasInstaller;
        private ServiceInstaller EnvioSMSEmpresasInstaller;
        private ServiceInstaller EnviarEmailAlertaExperienciaProfissionalInstaller;
        private ServiceInstaller AllinEmailQuemMeViuInstaller;
        private ServiceInstaller ControleFinanceiroInstaller;
        private ServiceInstaller EnvioSMSEmailEmpresasCvsNaoVistosInstaller;
        private ServiceInstaller SondaBancoDoBrasilInstaller;
    }
}