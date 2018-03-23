//-- Data: 02/08/2016 17:52
//-- Autor: Gieyson Stelmak

using System;

namespace BNE.BLL.Notificacao
{
	public partial class ProcessamentoJornalVagasSteps // Tabela: alerta.LOG_Processamento_Jornal_Vagas_Steps
	{
	    public ProcessamentoJornalVagasSteps(ProcessamentoJornalVagas objProcessamentoJornalVagas, string descricao, TimeSpan horasDecorridas)
	    {
	        this.ProcessamentoJornalVagas = objProcessamentoJornalVagas;
            this.DescricaoStep = descricao;
	        this.ElapsedTime = horasDecorridas;
	    }

        /// <summary>
        /// Loga um log para acompanhar o tempo de processamento :)
        /// </summary>
        /// <param name="descricao"></param>
        /// <param name="horasDecorridas"></param>
	    public static void Logar(ProcessamentoJornalVagas objProcessamentoJornalVagas, string descricao, TimeSpan horasDecorridas)
	    {
	        var objProcessamentoJornalVagasSteps = new ProcessamentoJornalVagasSteps(objProcessamentoJornalVagas, descricao, horasDecorridas);
            objProcessamentoJornalVagasSteps.Save();
        }
    }
}