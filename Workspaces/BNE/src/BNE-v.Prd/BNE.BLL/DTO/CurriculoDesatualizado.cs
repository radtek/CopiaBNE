using System;

namespace BNE.BLL.DTO
{
    public class CurriculoDesatualizado
    {
        public decimal CPF { get; set; }
        public string NomePessoa { get; set; }
        public DateTime DataNascimento { get; set; }
        public int IdUsuarioFilialPerfil { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
