using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BNE.BLL;
using BNE.Services.Base.ProcessosAssincronos;
using System.Data;
using System.Linq;
using System.Web;
 

namespace BNE.Testes
{
    [TestClass]
    public class Testes
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    string idCampanha = "10";
        //    var parametrosAtividade = new BNE.Services.Base.ProcessosAssincronos.ParametroExecucaoCollection 
        //                {
        //                    {"idCampanhaMensagem","idCampanhaMensagem", idCampanha, idCampanha}
        //                };

        //    var objPlugin = new BNE.Services.Plugins.PluginsEntrada.CampanhaMensagem();

        //    objPlugin.DoExecute(parametrosAtividade, null);
        //}

        //[TestMethod]
        //public void EnvioCandidatoVagaPerfil()
        //{
        //    string idVaga = "1488315";
        //    var parametrosAtividade = new BNE.Services.Base.ProcessosAssincronos.ParametroExecucaoCollection 
        //                {
        //                    {"idVaga","idVaga", idVaga, idVaga}
        //                };

        //    var objPluginE = new BNE.Services.Plugins.PluginsEntrada.EnvioCandidatoVagaPerfil();
        //    var objPluginS = new BNE.Services.Plugins.PluginSaida.PluginSaidaEmailSMSTanque();

        //    //var result = objPluginE.DoExecuteTask2(parametrosAtividade, null);
        //    //objPluginS.DoExecuteTask2(result, null);

        //}

        //[TestMethod]
        //public void BuscaTextoCampanha()
        //{
        //    BLL.Custom.CampanhaTanque objCampanha = new BLL.Custom.CampanhaTanque();
        //    var campanha = objCampanha.GetTextoCampanha(BLL.Enumeradores.CampanhaTanque.CampanhaRecrutamento);
        //}

        //[TestMethod]
        //public void TestaAuditoriaVaga()
        //{
        //    string idVaga = "791834";
        //    var parametrosAtividade = new BNE.Services.Base.ProcessosAssincronos.ParametroExecucaoCollection 
        //                {
        //                    {"idVaga","idVaga", idVaga, idVaga}
        //                };

        //    var objPlugin = new BNE.Services.Plugins.PluginsEntrada.PublicacaoVaga();

        //    objPlugin.DoExecute(parametrosAtividade, null);
        //}

        //[TestMethod]
        //public void EnviarEmail()
        //{
        //    Curriculo objCV = Curriculo.LoadObject(3049359);

        //    MensagemCS.SalvarEmail(objCV, null, null, null, "assunto teste", "mensagem teste",null, "atendimento@bne.com.br", "martysroka@bne.com.br", "", null, null);
        //}

        //[TestMethod]
        //[TestCategory("Vagas")]
        //public void ProcessaVaga()
        //{
        //    var parametros = new ParametroExecucaoCollection
        //            {
        //                {"codigo","codigo", "2474909", "2474909"},
        //                {"vagaIntegracao_vagaParaDeficiente","vagaIntegracao_vagaParaDeficiente", "false", "false"},
        //                {"email","email","administrativo@animaestagios.com.br","administrativo@animaestagios.com.br"},
        //                {"desCidade","desCidade","Salvador/BA","Salvador/BA"},
        //                {"dataAbertura","dataAbertura","",""},
        //                {"dataPrazo","dataPrazo","",""},
        //                {"desFuncao","desFuncao","Gerente de E-commerce","Gerente de E-commerce"},
        //                {"desEscolaridade","desEscolaridade","Ensino Medio","Ensino Medio"},
        //                {"desDeficiencia","desDeficiencia","",""},
        //                {"descricao","descricao","&lt;Br/&gt;&lt;b&gt;descricacaovaga&lt;/b&gt;:gerente de loja requisitos:  experiência  em carteira; experiência com vendas; shopping; liderança de equipe benefícios: alimentação + transporte","&lt;Br/&gt;&lt;b&gt;descricacaovaga&lt;/b&gt;:gerente de loja requisitos:  experiência  em carteira; experiência com vendas; shopping; liderança de equipe benefícios: alimentação + transporte"},
        //                {"empresa","empresa","Anima Rh","Anima Rh"},
        //                {"disponibilidades","disponibilidades","",""},
        //                {"tiposVinculo","tiposVinculo","Efetivo;Estagio","Efetivo;Estagio"},
        //                {"qtdeVaga","qtdeVaga","1","1"},
        //                {"OrigemImportacao","OrigemImportacao", "1","1"},
        //                {"Integrador","Integrador", "39","39"},
        //                {"vlrSalarioDe","vlrSalarioDe", "1800,00","1800,00"},
        //                {"vlrSalarioPara","vlrSalarioPara", "",""}
        //            };

        //    var objPlugin = new BNE.Services.Plugins.PluginsEntrada.IntegracaoVagaPersist();

        //    objPlugin.DoExecute(parametros, null);

        //}

        //[TestMethod]
        //public void AtualizarPlanoPF()
        //{
        //    var cap = new BNE.Services.AsyncExecutor.Capabilities();
        //    //cap.SendMail("gieyson@bne.com.br", "gieyson@bne.com.br;polaco@bne.com.br", "teste send grid", "Oi, sou um teste");

        //    using (var stream = new FileStream(@"C:\file.txt", FileMode.Open))
        //    {
        //        var length = Convert.ToInt32(stream.Length);
        //        var data = new byte[length];
        //        stream.Read(data, 0, length);
        //        cap.SendMail("gieyson@bne.com.br", "gieyson@bne.com.br;polaco@bne.com.br", "teste send grid", "Oi, sou um teste", new Dictionary<string, byte[]> { { "File.txt", data } });
        //    }


        //}

        //[TestMethod]
        //public void CampanhaCV()
        //{
        //    var listaParametros = new List<BLL.AsyncServices.Enumeradores.Parametro>
        //    {
        //        BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoLotePadraoParaEnvio,
        //        BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoTetoParaEnvio,
        //        BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoMultiplicadorLoteInicial,
        //        BLL.AsyncServices.Enumeradores.Parametro.CampanhaPesquisaCurriculoLotePadraoParaEnvio,
        //        BLL.AsyncServices.Enumeradores.Parametro.TentativasRequisicaoBuscaCVCampanha,
        //        BLL.AsyncServices.Enumeradores.Parametro.EmailDestinoAlertaProblemaCampanha,
        //        BLL.AsyncServices.Enumeradores.Parametro.EmailRemetenteVagaPerfil,
        //        BLL.AsyncServices.Enumeradores.Parametro.IdfUsuarioEnvioCampanha,
        //        BLL.AsyncServices.Enumeradores.Parametro.IdfUsuarioEnvioCampanhaPesquisaCV
        //    };
        //    var dicionarioParametros = BLL.AsyncServices.Parametro.ListarParametros(listaParametros);

        //    var objPlugin = new BNE.Services.Plugins.PluginsEntrada.Campanha();
            
        //   // objPlugin.DeRecrutamento(CampanhaRecrutamento.LoadObject(194), dicionarioParametros);
        //   // objPlugin.DePesquisaCurriculo(CampanhaRecrutamento.LoadObject(170), dicionarioParametros);
        //}

        //[TestMethod]
        //public void EnviarSMS()
        //{
        //    // var parametrosAtividade = new ParametroExecucaoCollection
        //    //    {
        //    //        {"idVaga", "Vaga", "792945", "V0792945"}
        //    //    };

        //    //var objPlugin = new BNE.Services.Plugins.PluginsEntrada.();

        //    //objPlugin.ExecuteTask(parametrosAtividade, null);
        //    //objPlugin.DePesquisaCurriculo(CampanhaRecrutamento.LoadObject(170), dicionarioParametros);
        //}

        //[TestMethod]
        //public void EnviarCampanhaSMS()
        //{

        //   List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> lista =  Curriculo.RecuperarListaCvsEnvioSMS(3187, 1212, false);
        //}
        //[TestMethod]
        //public void LogarAllin()
        //{
        //    BNE.BLL.Custom.BufferAtualizacaoCurriculoAllin.FazerLoginAllin();
        //}

        //[TestMethod]
        //public void EnviarExtratoCV()
        //{
        //    DataTable dt = Curriculo.ListarCurriculosEnvioNotificacao(DateTime.Now);
        //    var ListaVip = BLL.NotificacoesVip.DTO.Curriculo.RetortarListaVipNotificacao(dt);

        //    //BLL.NotificacoesVip.Notificacoes.Extrato extrato = new BLL.NotificacoesVip.Notificacoes.Extrato();
        //    //extrato.Notificar(ListaVip[0], 4, 0);
        //}
        //[TestMethod]
        //public void EnviarDicaCV()
        //{
        //    //BLL.NotificacoesVip.Notificacoes.Video dica = new BLL.NotificacoesVip.Notificacoes.Video();
        //    //dica.Notificar(new BLL.NotificacoesVip.DTO.Curriculo(){
        //    //    nomeCompleto = "Marty",
        //    //    idUsuarioFilialPerfil = 9129749,
        //    //    idPessoaFisica= 11393105,
        //    //    idCurriculo=9058801,
        //    //    email= "martysroka@bne.com.br",
        //    //    numeroCPF=5071933691,
        //    //    dataNascimento=Convert.ToDateTime("1989-09-02"),
        //    //    dataCadastro=Convert.ToDateTime("2016-07-20"),
        //    //    dataInicioPlano=Convert.ToDateTime("2016-07-21"),
        //    //    funcao="Programador",
        //    //    cidade = "Curitiba",
        //    //    siglaEstado = "PR"
        //    //}, 6, 1);

        //}

        //[TestMethod]
        //public void ExecutarNotificacoesVip()
        //{
        //    BLL.NotificacoesVip.NotificacoesVip.IniciarNotificacoesVip();
        //}

        [TestMethod]
        public void CriarParcelasRecorrente()
        {
           // var planoAdquirido = PlanoAdquirido.LoadObject(1937116);

            var pagamento = Pagamento.LoadObject(1048927);

            var boleto = BoletoBancario.ProcessarBoletoNovo(pagamento, null, false);

            //var parcelaAberto = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(planoAdquirido);

            //var boletos = BLL.BoletoBancario.ProcessarBoletoNovo(planoAdquirido, null);


            //if (parcelaAberto == null && planoAdquirido.Plano.FlagRecorrente && planoAdquirido.Plano.PlanoFormaPagamento.IdPlanoFormaPagamento == (int)BLL.Enumeradores.PlanoFormaPagamento.Mensalidade)
            //{
            //    var objTransacaoUltima =
            //        Transacao.CarregarUltimaTransacaoPagaPorPlanoAdquirido(planoAdquirido.IdPlanoAdquirido);
            //    //criar uma nova parcela
            //    var objPlanoParcela = PlanoParcela.CriarParcelaRecorrenciaPeloPlanoAdquirido(planoAdquirido, null);
            //    //criar um novo pagamento

            //    var objPagamento = Pagamento.criarPagamentoRecorrencia(objPlanoParcela, planoAdquirido, objTransacaoUltima, null);



            //}
        }

        public class Telefonessss
        {
             public string DDD { get; set; }
             public Int32 Numero { get; set; }
        }

        [TestMethod]
        public void ListaObjetos()
        {
            
            System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=pdffile.pdf");
           // System.Web.HttpContext.Current.Response.TransmitFile(Server.MapPath("~/F:\\pdffile.pdf"));
            System.Web.HttpContext.Current.Response.End();


        }
    }
}
