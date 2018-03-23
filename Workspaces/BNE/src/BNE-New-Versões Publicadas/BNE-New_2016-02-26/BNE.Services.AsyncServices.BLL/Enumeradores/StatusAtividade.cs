using System.ComponentModel;

namespace BNE.Services.AsyncServices.BLL.Enumeradores
{
    public enum StatusAtividade
    {
        /// <summary>
        /// Aguardando execução na fila
        /// </summary>
        [Description("Aguardando execução")]
        AguardandoExecucao = 1,
        /// <summary>
        /// Sendo executado
        /// </summary>
        [Description("Executando")]
        Executando = 2,
        /// <summary>
        /// Executado com erros
        /// </summary>
        [Description("Finalizada com erro")]
        FinalizadoComErro = 3,
        /// <summary>
        /// Executado mas não encontrou dados para o processo
        /// </summary>
        [Description("Não há dados para geração")]
        FinalizadoSemDados = 4,
        /// <summary>
        /// Finalizado com sucesso
        /// </summary>
        [Description("Finalizado com sucesso")]
        FinalizadoComSucesso = 5,
    }
}