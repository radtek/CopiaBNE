namespace BNE.BLL.DTO
{
    public class CelularSelecionadorDestinatario
    {
        public int IdCurriculo { get; set; }
        public int IdPessoaFisica { get; set; }
        public int IdUsuarioFilialPerfil { get; set; }
        public string Nome { get; set; }
        public bool VIP { get; set; }
        public string NumeroDDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string Email { get; set; }
    }
}
