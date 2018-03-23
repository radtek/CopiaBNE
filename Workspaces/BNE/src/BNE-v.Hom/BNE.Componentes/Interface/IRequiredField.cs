namespace BNE.Componentes.Interface
{
    /// <summary>
    /// Interface que declara o comportamento de campo requerido
    /// </summary>
    public interface IRequiredField
    {
        /// <summary>
        /// Retorna se o controle é ou não de preencimento obrigatório
        /// </summary>
        bool Obrigatorio { get; set; }
    }
}
