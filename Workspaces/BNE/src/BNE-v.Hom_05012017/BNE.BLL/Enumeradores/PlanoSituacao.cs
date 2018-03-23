namespace BNE.BLL.Enumeradores
{
    public enum PlanoSituacao
    {
        AguardandoLiberacao = 0,
        Liberado            = 1,
        Encerrado           = 2,
        Cancelado           = 3,
        Bloqueado           = 4,
        /// <summary>
        /// Indica que a venda do plano já está concretizada e que será liberado em um momento futuro. 
        /// Utilizado quando uma parcela do plano é paga por um usuário com com plano já liberado. Esse plano será liberado automaticamente na data de encerramento do plano vigente.
        /// </summary>
        LiberacaoFutura     = 5,
        /// <summary>
        /// Plano de liberação automática será liberado pelo robo "AtualizaPlanoPJ" automaticamente na Data de Início do plano,
        /// independentemente de parcelas pagas ou não.
        /// </summary>
        LiberacaAutomatica = 6
    }
}