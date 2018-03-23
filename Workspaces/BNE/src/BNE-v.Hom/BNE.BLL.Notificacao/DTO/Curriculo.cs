using System;
namespace BNE.BLL.Notificacao.DTO
{
    public class Curriculo
    {
        public int IdCurriculo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int QuantidadeQuemMeViu15Dias { get; set; }
        public int QuantidadeQuemMeViu30Dias { get; set; }
        public bool VIP { get; set; }
        public decimal CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string DDD{ get; set; }
        public string Celular { get; set; }
    }
}
