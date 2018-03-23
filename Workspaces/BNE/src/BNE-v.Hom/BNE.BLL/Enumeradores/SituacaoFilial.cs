namespace BNE.BLL.Enumeradores
{
    public enum SituacaoFilial
    {
        PublicadoEmpresa = 1,
        PublicadoAgencia = 2,
        AguardandoPublicacao = 3,
        ComCritica = 4,
        Bloqueado = 5,
        Cancelado = 6,
        /// <summary>
        /// Empresas que serão cadastradas através dos gatilhos de um cadastro de vaga rápida, empresas sem cnpj, irão entrar com essa situação.
        /// Isso evitará que ela entre bloqueada ou em auditoria e fique disparando fluxos relacionados a processos dessas situações.
        /// </summary>
        PreCadastro = 7,
        /// <summary>
        /// Empresas que se cadastram quando a receita estava fora do ar, entrarão com essa situação e aparecerão no topo na tela do administrador
        /// </summary>
        FaltaDadosReceita = 8,
        /// <summary>
        /// Cadastradas fora do horario Comercial (parametro), 
        /// </summary>
        ForaDoHorarioComercial
    }
}