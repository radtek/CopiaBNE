using System;
using System.ServiceProcess;
using BNE.BLL;
using BNE.EL;
using Quartz;
using Quartz.Impl;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BNE.BLL.Custom.Email;
using BNE.BLL.Custom;

namespace BNE.Services
{
    partial class EmpresaBuscaCVPerfil : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        private const string spDesatualizados = @"select  pf.Num_CPF,pf.nme_pessoa, pf.Dta_Nascimento , pf.eml_pessoa,
                                               fp.Des_Area_BNE, cid.nme_cidade,  cid.Sig_Estado
                                            From bne.TAB_Pesquisa_Curriculo_Curriculos pcc with(nolock)
                                            join bne.tab_pesquisa_curriculo pc with(nolock) 
                                            on pc.idf_pesquisa_curriculo = pcc.idf_pesquisa_curriculo
                                            cross apply( select top 1 area.Des_Area_BNE from
                                             bne.tab_pesquisa_curriculo_funcao pcf with(nolock)
                                             join plataforma.tab_funcao fun with(nolock) on fun.Idf_Funcao = pcf.Idf_Funcao
                                             join plataforma.TAB_Area_BNE area with(nolock) on area.Idf_Area_BNE = fun.Idf_Area_BNE
                                            where pc.idf_pesquisa_curriculo = pcf.idf_pesquisa_curriculo) as fp
                                            join bne.bne_curriculo cv with(nolock) on cv.idf_curriculo = pcc.idf_curriculo
                                            join bne.tab_pessoa_Fisica pf with(nolock) on pf.idf_pessoa_fisica = cv.idf_pessoa_Fisica
                                            join plataforma.tab_cidade cid with(nolock) on cid.idf_cidade = pf.idf_cidade
                                            where pc.idf_usuario_filial_perfil is not null
                                            and pc.idf_cidade is not null
                                            and cv.dta_atualizacao < @Dta_Atualizacao
                                            and pc.dta_cadastro between @Dta_inicio and @Dta_fim
                                            and cv.Idf_Situacao_Curriculo not in(6,7,8,11,12)
                                            and pf.eml_pessoa is not null
                                            group by pf.Num_CPF,pf.nme_pessoa, pf.Dta_Nascimento, pf.eml_pessoa,
                                               fp.Des_Area_BNE, cid.nme_cidade,  cid.Sig_Estado";


        public EmpresaBuscaCVPerfil()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
       
            int countLiberados = 0;
            var ObjCarta = CartaEmail.LoadObject((int)BLL.Enumeradores.CartaEmail.EmpresaBuscaSeuPerfil);
            var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
            var meses = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.MesesCurriculosDesatualizado));


            var parametros = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName = "@Dta_inicio", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now.AddDays(-7)},
                new SqlParameter{ParameterName = "@Dta_fim", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now},
                new SqlParameter{ParameterName = "@Dta_Atualizacao", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now.AddMonths(-meses)}
            };


            try
            {

                using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spDesatualizados, parametros))
                {
                    while (dr.Read())
                    {
                        try
                        {
                            var link = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(Convert.ToDecimal(dr["Num_Cpf"]), Convert.ToDateTime(dr["Dta_Nascimento"]), "/cadastro-de-curriculo-gratis");
                            link = $"{Helper.RecuperarURLAmbiente()}/logar/{link}";

                            var layoutCarta = ObjCarta.ValorCartaEmail.Replace("{Nome_Completo}", dr["Nme_Pessoa"].ToString()).
                                Replace("{Area}", dr["Des_Area_BNE"].ToString())
                                .Replace("{Cidade}", Helper.FormatarCidade(dr["Nme_Cidade"].ToString(), dr["Sig_Estado"].ToString()))
                                .Replace("{LinkAtualizar}", link);


                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                                  .Enviar(ObjCarta.DescricaoAssunto, layoutCarta, BLL.Enumeradores.CartaEmail.CandEtapaAnalise, emailRemetente, dr["eml_pessoa"].ToString());
                            countLiberados++;
                        }
                        catch (Exception)
                        {

                        }
                       
                    }

                   
                }
          
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Erro Robo EmpresaBuscaCVPerfil");
                GravaLogText("ERRO o EmpresaBuscaCVPerfil "+ ex.ToString());
            }

            GravaLogText("Finalizou o EmpresaBuscaCVPerfil em " + DateTime.Now + " Enviados: " + countLiberados);
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();
            GravaLogText("Finalizou o EmpresaBuscaCVPerfil OnStart");


            var job = JobBuilder.Create<EmpresaBuscaCVPerfil>()
                .Build();
            DayOfWeek[] dias = { DayOfWeek.Friday };

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(7, 0, dias))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }



        protected override void OnStop()
        {
            GravaLogText("Finalizou o EmpresaBuscaCVPerfil OnStop");
            _scheduler.Shutdown();

        }
        private static void GravaLogText(string mensagem)
        {
            using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "logEmpresaBuscaCV.txt"))
            {
                sw.WriteLine(DateTime.Now + "   Mensagem:   " + mensagem);
            }
        }

    }
}