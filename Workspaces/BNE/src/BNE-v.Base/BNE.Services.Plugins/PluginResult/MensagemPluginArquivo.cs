using System;
using BNE.BLL.Enumeradores;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using Microsoft.Reporting.WinForms;

namespace BNE.Services.Plugins.PluginResult
{
    public class MensagemPluginArquivo:IPluginResult
    {
        private readonly string _inputPluginName = String.Empty;

        public string InputPluginName
        {
            get { return _inputPluginName; }
        }

        public bool FinishTask
        {
            get;
            private set;
        }

        public byte[] Arquivo
        {
            get;
            private set;
        }

        public String NomeArquivo
        {
            get;
            private set;
        }

        public MensagemPluginArquivo(InputPlugin objInputPlugin, String nomeArquivo,  byte[] arquivo, bool finishWithPonteAzul)
        {
            FinishWithPonteAzul = finishWithPonteAzul;
            FinishTask = false;
            _inputPluginName = objInputPlugin.MetadataName;
            Arquivo = arquivo;
            NomeArquivo = nomeArquivo;
        }

        public MensagemPluginArquivo(InputPlugin objInputPlugin, String nomeArquivo, ReportViewer rpv, string tipoArquivo, FormatoRelatorio formatoRelatorio, bool finishWithPonteAzul)
        {
            FinishWithPonteAzul = finishWithPonteAzul;
            string deviceInfo;

            if (tipoArquivo.Equals("pdf"))
            {
                if (formatoRelatorio == FormatoRelatorio.Retrato)
                {
                    deviceInfo = "<DeviceInfo>" +
                                              "  <OutputFormat>PDF</OutputFormat>" +
                                              "  <PageWidth>8.8in</PageWidth>" +
                                              "  <PageHeight>12.45in</PageHeight>" +
                                              "  <MarginTop>0.4in</MarginTop>" +
                                              "  <MarginLeft>0.4in</MarginLeft>" +
                                              "  <MarginRight>0.4in</MarginRight>" +
                                              "  <MarginBottom>0.4in</MarginBottom>" +
                                              "</DeviceInfo>";
                }
                else
                {

                    deviceInfo = "<DeviceInfo>" +
                                              " <OutputFormat>PDF</OutputFormat>" +
                                              " <PageWidth>12in</PageWidth>" +
                                              " <PageHeight>8.5in</PageHeight>" +
                                              " <MarginTop>0.25in</MarginTop>" +
                                              " <MarginBottom>0.25in</MarginBottom>" +
                                              " <MarginRight>0.25in</MarginRight>" +
                                              " <MarginLeft>0.25in</MarginLeft>" +
                                              "</DeviceInfo>";
                }
            }
            else
            {
                if (formatoRelatorio == FormatoRelatorio.Retrato)
                {
                    deviceInfo = "<DeviceInfo>" +
                              "  <OutputFormat>Excel</OutputFormat>" +
                              "  <PageWidth>8.8in</PageWidth>" +
                              "  <PageHeight>12.45in</PageHeight>" +
                              "  <MarginTop>0.4in</MarginTop>" +
                              "  <MarginLeft>0.4in</MarginLeft>" +
                              "  <MarginRight>0.4in</MarginRight>" +
                              "  <MarginBottom>0.4in</MarginBottom>" +
                              "</DeviceInfo>";
                }
                else
                {
                    deviceInfo = "<DeviceInfo>" +
                              "  <OutputFormat>Excel</OutputFormat>" +
                              "  <PageWidth>12in</PageWidth>" +
                              "  <PageHeight>8.5in</PageHeight>" +
                              "  <MarginTop>0.4in</MarginTop>" +
                              "  <MarginLeft>0.4in</MarginLeft>" +
                              "  <MarginRight>0.4in</MarginRight>" +
                              "  <MarginBottom>0.4in</MarginBottom>" +
                              "</DeviceInfo>";
                }
            }



            FinishTask = false;
            _inputPluginName = objInputPlugin.MetadataName;
            Arquivo = rpv.LocalReport.Render("pdf", deviceInfo);
            NomeArquivo = nomeArquivo;
        }

        public MensagemPluginArquivo(InputPlugin objInputPlugin, String nomeArquivo, bool pFinishTask, bool finishWithPonteAzul)
        {
            FinishWithPonteAzul = finishWithPonteAzul;
            FinishTask = pFinishTask;
            _inputPluginName = objInputPlugin.MetadataName;
            NomeArquivo = nomeArquivo;
        }

        public bool FinishWithPonteAzul
        {
            get;
            private set;
        }
    }
}
